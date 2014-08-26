#region Using Directives

using System;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using EnvDTE;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text.RegularExpressions;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.Win32;
using Microsoft.VisualStudio.Shell;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// Run SPDiposeChecker if it is installed on the machine
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class RunDisposeChecker : ConfigurableAction
    {
        private ErrorList list = null;
        private Project currentProject = null;

        private bool IsDisposeCheckerInstalled()
        {
            //The registry key:
            string SoftwareKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(SoftwareKey))
            {
                //Let's go through the registry keys and get the info we need:
                foreach (string skName in rk.GetSubKeyNames())
                {
                    using (RegistryKey sk = rk.OpenSubKey(skName))
                    {
                        try
                        {
                            //If the key has value, continue, if not, skip it:
                            if (!(sk.GetValue("DisplayName") == null))
                            {
                                if (sk.GetValue("DisplayName").ToString() == "SharePoint Dispose Check")
                                {
                                    return true;
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            return false;
        }

        public override void Execute()
        {
            try
            {
                DTE service = (DTE)this.GetService(typeof(DTE));
                list = new ErrorList(service);

                if (IsDisposeCheckerInstalled())
                {
                    string disposeCheckerPath = "";
                    string[] folders = new string[] { 
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Microsoft\SharePoint Dispose Check\SPDisposeCheck.exe") };
                    bool disposeCheckerFound = false;
                    foreach (string path in folders)
                    {
                        if (File.Exists(path))
                        {
                            disposeCheckerFound = true;
                            disposeCheckerPath = path;
                            break;
                        }
                    }

                    if (!disposeCheckerFound)
                    {
                        MessageBox.Show("SPDisposeCheck not found at installation location");
                    }   

                    //first check for rebuild
                    foreach (Project project in Helpers.GetSelectedProjects(service))
                    {
                        if (DeploymentHelpers.CheckRebuildForProject(service, project))
                        {
                        }
                    }

                    foreach (Project project in Helpers.GetSelectedProjects(service))
                    {
                        currentProject = project;
                        string projectpath = project.FullName;
                        projectpath = projectpath.Substring(0, projectpath.LastIndexOf('\\', projectpath.Length - 2));
                        RunProcess(service, disposeCheckerPath, "\"" + projectpath + "\"");                                           
                    }
                }
                else
                {
                    if (MessageBox.Show("SPDisposeCheck is not installed. Go to download page?", "Not installed", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Window win = service.Windows.Item(EnvDTE.Constants.vsWindowKindCommandWindow);
                        CommandWindow comwin = (CommandWindow)win.Object;
                        comwin.SendInput("nav \"http://code.msdn.microsoft.com/SPDisposeCheck\"", true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private static OutputWindowPane OWP = null;

        internal void RunProcess(DTE dte, string command, string parameters)
        {
            Window win = dte.Windows.Item(EnvDTE.Constants.vsWindowKindOutput);
            OutputWindow comwin = (OutputWindow)win.Object;
            if (OWP == null)
            {
                foreach (OutputWindowPane w in comwin.OutputWindowPanes)
                {
                    if (w.Name == "SPSF SharePoint Software Factory")
                    {
                        OWP = w;
                    }
                }
            }
            if (OWP == null)
            {
                OWP = comwin.OutputWindowPanes.Add("SharePoint Software Factory");
            }
            OWP.Clear();
            OWP.Activate();

            string dotnet = RuntimeEnvironment.GetRuntimeDirectory();

            OWP.OutputString(command + " " + parameters + "\n");

            System.Diagnostics.Process snProcess = new System.Diagnostics.Process();

            snProcess.StartInfo.FileName = command;
            snProcess.StartInfo.Arguments = parameters;
            snProcess.StartInfo.CreateNoWindow = true;
            snProcess.StartInfo.UseShellExecute = false;
            snProcess.StartInfo.RedirectStandardOutput = true;
            snProcess.StartInfo.RedirectStandardError = true;
            snProcess.OutputDataReceived += new DataReceivedEventHandler(snProcess_OutputDataReceived);
            snProcess.ErrorDataReceived += new DataReceivedEventHandler(snProcess_ErrorDataReceived);
            snProcess.Start();
            snProcess.BeginErrorReadLine();
            snProcess.BeginOutputReadLine();
        }



        void snProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                ReadData(e.Data);
                if (OWP != null)
                {
                    OWP.OutputString(e.Data + "\n");
                    OWP.ForceItemsToTaskList();
                }
            }
        }

        void snProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                ReadData(e.Data);
                if (OWP != null)
                {
                    OWP.OutputString(e.Data + "\n");
                    OWP.ForceItemsToTaskList();
                }
            }
        }

        private string ID = "";
        private string Line = "";
        private string Source = "";
        private string Notes = "";
        private string Statement = "";
        private string MoreInformation = "";
        private string Module = "";

        private void ReadData(string input)
        {
            if (input.StartsWith("ID:"))
            {
                ID = GetValue(input, "ID:");
            }
            if (input.StartsWith("Line:"))
            {
                Line = GetValue(input, "Line:");
            }
            if (input.StartsWith("Notes:"))
            {
                Notes = GetValue(input, "Notes:");
            }
            if (input.StartsWith("Source:"))
            {
                Source = GetValue(input, "Source:");
            }
            if (input.StartsWith("Module:"))
            {
                Module = GetValue(input, "Module:");
            }
            if (input.StartsWith("Statement:"))
            {
                Statement = GetValue(input, "Statement:");
            }
            if (input.StartsWith("More Information:"))
            {
                MoreInformation = GetValue(input, "More Information:");
            }
            if (input.StartsWith("---"))
            {
                //finished
                if (ID != "")
                {
                    try
                    {
                        int iLine = 0;
                        try
                        {
                            iLine = Int32.Parse(Line);
                        }
                        catch (Exception)
                        {
                        }
                        if (Source != "")
                        {
                            list.AddError(currentProject, Source, Notes + " " + Statement + ", see:" + MoreInformation, TaskErrorCategory.Error, iLine, 0);
                        }
                        else
                        {
                            list.AddError(currentProject, Module, Notes + " " + Statement + ", see:" + MoreInformation, TaskErrorCategory.Error, iLine, 0);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                ID = "";
                Line = "0";
                Source = "";
                Notes = "";
                Statement = "";
                Module = "";
            }
        }

        private string GetValue(string input, string key)
        {
            return input.Substring(key.Length + 1);
        }

        /*
          private ErrorListProvider _errorListProvider = null;

          /// <summary>  
          /// Write an entry to the Visual Studio Error List.  
          /// </summary>  
          /// <param name="category">Category: Error or Warning</param>  
          /// <param name="text">The text of the error or warning</param>  
          /// <param name="code">The code of the error or warning</param>  
          /// <param name="path">The path to the file containing the error</param>  
          /// <param name="line">The line in the file where the error occured</param>  
          /// <param name="column">The column in the file where the error occured</param>  
          /// <returns>The error or warning output string</returns>  
          public string WriteVisualStudioErrorList(string text, string code, string path, int line, int column)
          {
            if (_errorListProvider == null)
            {
              IServiceProvider serviceProvider =
              new ServiceProvider(VsPackage.ApplicationObject as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
              _errorListProvider = new ErrorListProvider(serviceProvider);
              _errorListProvider.ProviderName = "Factory Guide Errors";
              _errorListProvider.ProviderGuid = new Guid("5A10E43F-8D1D-4026-98C0-E6B502058901");
              // _errorListProvider.ForceShowErrors();  
            }

            string textline = null;

            // Determine the task priority  
            TaskPriority priority = TaskPriority.Normal;
            TaskErrorCategory errorCategory = TaskErrorCategory.Error;

            switch (errorCategory)
            {
              case TaskErrorCategory.Error:
                _errors++;
                break;
              case TaskErrorCategory.Warning:
                _warnings++;
                break;
            }

            // Check if this error is already in the error list, don't report more than once  
            bool alreadyReported = false;
            foreach (ErrorTask task in _errorListProvider.Tasks)
            {
              if (task.ErrorCategory == errorCategory &&
                  task.Document == path &&
                  task.Line == line - 1 &&
                  task.Column == column - 1 &&
                  task.Text == text)
              {
                alreadyReported = true;
                break;
              }
            }

            if (!alreadyReported)
            {
              // Add error to task list  
              ErrorTask task = new ErrorTask();
              task.Document = path;
              task.Line = line - 1; // The task list does +1 before showing this number.  
              task.Column = column - 1; // The task list does +1 before showing this number.  
              task.Text = text;
              task.Priority = priority; // High or Normal  
              task.ErrorCategory = errorCategory; // Error or Warning, no support for Message yet  
              task.Category = TaskCategory.BuildCompile;
              // task.HierarchyItem = hierarchy;  
              task.Navigate += new EventHandler(NavigateTo);
              if (VisualStudioExtensions.ContainsLink(text))
              {
                task.Help += new EventHandler(task_Help);
              }
              _errorListProvider.Tasks.Add(task);

              switch (errorCategory)
              {
                case TaskErrorCategory.Error:
                  _uniqueErrors++;
                  break;
                case TaskErrorCategory.Warning:
                  _uniqueWarnings++;
                  break;
              }

              string categoryString = category == MessageCategory.Error ? "error" : "warning";
              textline = MessageGeneration.Generate(category, text, code, path, line, column);
            }

            return textline;
          }

          /// <summary>  
          /// Navigate to the file, line and column reported in the task  
          /// </summary>  
          /// <param name="sender">The Task to navigate to</param>  
          /// <param name="arguments"></param>  
          private void NavigateTo(object sender, EventArgs arguments)
          {
            Microsoft.VisualStudio.Shell.Task task = sender as Microsoft.VisualStudio.Shell.Task;

            if (task == null)
            {
              throw new ArgumentException("sender");
            }

            // If the name of the file connected to the task is empty there is nowhere to navigate to  
            if (String.IsNullOrEmpty(task.Document))
            {
              return;
            }

            IServiceProvider serviceProvider =
    new ServiceProvider(VsPackage.ApplicationObject as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);

            IVsUIShellOpenDocument openDoc = serviceProvider.GetService(typeof(IVsUIShellOpenDocument)) as IVsUIShellOpenDocument;

            if (openDoc == null)
            {
              return;
            }

            IVsWindowFrame frame;
            Microsoft.VisualStudio.OLE.Interop.IServiceProvider sp;
            IVsUIHierarchy hier;
            uint itemid;
            Guid logicalView = VSConstants.LOGVIEWID_Code;

            if (ErrorHandler.Failed(openDoc.OpenDocumentViaProject(
                task.Document, ref logicalView, out sp, out hier, out itemid, out frame))
                || frame == null
            )
            {
              return;
            }

            object docData;
            frame.GetProperty((int)__VSFPROPID.VSFPROPID_DocData, out docData);

            // Get the VsTextBuffer  
            VsTextBuffer buffer = docData as VsTextBuffer;
            if (buffer == null)
            {
              IVsTextBufferProvider bufferProvider = docData as IVsTextBufferProvider;
              if (bufferProvider != null)
              {
                IVsTextLines lines;
                ErrorHandler.ThrowOnFailure(bufferProvider.GetTextBuffer(out lines));
                buffer = lines as VsTextBuffer;
                Debug.Assert(buffer != null, "IVsTextLines does not implement IVsTextBuffer");

                if (buffer == null)
                {
                  return;
                }
              }
            }

            // Finally, perform the navigation.  
            IVsTextManager mgr = serviceProvider.GetService(typeof(VsTextManagerClass)) as IVsTextManager;
            if (mgr == null)
            {
              return;
            }

            mgr.NavigateToLineAndColumn(buffer, ref logicalView, task.Line, task.Column, task.Line, task.Column);
          }

          /// <summary>  
          /// Determines whether the task text contains a url, we assume that url is help.  
          /// </summary>  
          /// <param name="text">The task text.</param>  
          /// <returns>  
          ///     <c>true</c> if the text contains a link, assume its a help link; otherwise, <c>false</c>.  
          /// </returns>  
          static bool ContainsLink(string text)
          {
            if (text == null)
            {
              throw new ArgumentNullException("text");
            }

            Match urlMatches = Regex.Match(text,
                        @"((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)");
            return urlMatches.Success;
          }

          /// <summary>  
          /// Handles the Help event of the task control.  
          /// </summary>  
          /// <param name="sender">The Task to parse for a guidance link.</param>  
          /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>  
          static void task_Help(object sender, EventArgs e)
          {
            Microsoft.VisualStudio.Shell.Task task = sender as Microsoft.VisualStudio.Shell.Task;

            if (task == null)
            {
              throw new ArgumentException("sender");
            }

            string url = null;
            Match urlMatches = Regex.Match(task.Text,
                        @"((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)");
            if (urlMatches.Success)
            {
              url = urlMatches.Captures[0].Value;
            }

            if (url != null)
            {
              VsPackage.ApplicationObject.ItemOperations.Navigate(url,
                                                vsNavigateOptions.vsNavigateOptionsDefault);
            }
          }  

         * */
        /// <summary>
        /// Removes the previously added reference, if it was created
        /// </summary>
        public override void Undo()
        {
        }
    }
}