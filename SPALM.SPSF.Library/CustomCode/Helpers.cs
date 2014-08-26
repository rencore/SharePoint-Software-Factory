#region Using Directives

using System;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.Practices.RecipeFramework.Library;
using EnvDTE;
using System.Xml;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Win32;
using EnvDTE80;
using System.DirectoryServices;
using Microsoft.VisualStudio.Shell.Interop;
using System.Net;
using SPALM.SPSF.SharePointBridge;
using System.Reflection;
using Microsoft.VisualStudio.SharePoint;

#endregion

namespace SPALM.SPSF.Library
{

    public static class Helpers
    {
        private static ArrayList featurenames;
        private static object lastcontext = null;

        public static string GetGACUtil(DTE dte)
        {
            string keyPath = @"SOFTWARE\Microsoft\Microsoft SDKs\Windows\";
            string keyValue = @"CurrentInstallFolder";
            string sdkpath = getRegKey(RegistryHive.LocalMachine, keyPath, keyValue);
            if (!string.IsNullOrEmpty(sdkpath))
            {
                string filename1 = Path.Combine(sdkpath, @"Bin\gacutil.exe");
                if (File.Exists(filename1))
                {
                    return filename1;
                }
            }
            string filename2 = GetBasePath() + "\\Tools\\gacutil.exe";
            if (File.Exists(filename2))
            {
                return filename2;
            }

            return "";
        }

        public static string GetDiscoPath()
        {
            string keyPath = @"SOFTWARE\Microsoft\Microsoft SDKs\Windows\";
            string keyValue = @"CurrentInstallFolder";
            string sdkpath = getRegKey(RegistryHive.LocalMachine, keyPath, keyValue);
            if (!string.IsNullOrEmpty(sdkpath))
            {
                string filename1 = Path.Combine(sdkpath, @"Bin\disco.exe");
                if (File.Exists(filename1))
                {
                    return filename1;
                }
            }


            /*
            Microsoft SDKs\Windows\v7.0A\bin
            Microsoft SDKs\Windows\v6.0A\bin
            Microsoft SDKs\Windows\v7.1A\bin
            */

            string[] programmFilesFolders = new string[] { Helpers.GetProgramsFolder32(), Helpers.GetProgramsFolder() };
            string[] sdkVersions = new string[] { "v6.0A", "v7.0A", "v7.1A" };

            foreach (string programmFilesFolder in programmFilesFolders)
            {
                foreach (string sdkVersion in sdkVersions)
                {
                    string checkPath = programmFilesFolder + "\\Microsoft SDKs\\Windows\\" + sdkVersion + "\\bin\\disco.exe";
                    //File.WriteAllText("C:\\ZZZ.txt", checkPath);
                    if(File.Exists(checkPath))
                    {
                        return checkPath;
                    }
                }
            }

            return "";
        }


        public static string getRegKey(RegistryHive type, string keyPath, string keyValue)
        {
            string value = "";
            RegistryKey key = null;
            switch (type)
            {
                case RegistryHive.LocalMachine:
                    key = Registry.LocalMachine.OpenSubKey(keyPath, false);
                    break;
                case RegistryHive.CurrentUser:
                    key = Registry.CurrentUser.OpenSubKey(keyPath, false);
                    break;
                case RegistryHive.ClassesRoot:
                    key = Registry.ClassesRoot.OpenSubKey(keyPath, false);
                    break;
                case RegistryHive.CurrentConfig:
                    key = Registry.CurrentConfig.OpenSubKey(keyPath, false);
                    break;
                case RegistryHive.Users:
                    key = Registry.Users.OpenSubKey(keyPath, false);
                    break;
                case RegistryHive.PerformanceData:
                    key = Registry.PerformanceData.OpenSubKey(keyPath, false);
                    break;
            }
            if (keyValue != null)
                value = key.GetValue(keyValue).ToString();
            return "";
        }


        public static ProjectItem GetProjectItemByName(ProjectItems pitems, string name)
        {
            if (pitems != null)
            {
                foreach (ProjectItem pitem in pitems)
                {
                    if (pitem.Name.ToUpper() == name.ToUpper())
                    {
                        return pitem;
                    }
                }
            }
            return null;
        }



        /// <summary>
        /// Add a file to a project folder with the same name and overwrites existing files
        /// </summary>
        /// <param name="asmxfileParentFolder"></param>
        /// <param name="finaldiscopath"></param>
        public static ProjectItem AddFile(ProjectItem parentFolder, string sourceFilePath)
        {
            //get the single file
            string filename = sourceFilePath.Substring(sourceFilePath.LastIndexOf("\\") + 1);

            //check, if a file with the same name exists already in the target folder
            try
            {
                ProjectItem existingFile = Helpers.GetProjectItemByName(parentFolder.ProjectItems, filename);
                if (existingFile != null)
                {
                    existingFile.Delete();
                }
            }
            catch (Exception)
            {
            }
            return Helpers.AddFromFileCopy(parentFolder, sourceFilePath);
        }

        private static OutputWindowPane OWP = null;


        /// <summary>
        /// checks if any new file has been added to 12 folder after the last modification of manifest.xml
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static bool ManifestRebuildRequired(Project project)
        {
            string projectpath = Helpers.GetFullPathOfProjectItem(project);
            string path12 = Path.Combine(projectpath, GetSharePointHive(project.DTE));
            if (!Directory.Exists(path12))
            {
                //no files in 12
                return false;
            }
            ProjectItem manifest = GetProjectItemByName(project.ProjectItems, "manifest.xml");
            if (manifest != null)
            {
                string manifestpath = Helpers.GetFullPathOfProjectItem(manifest);
                DateTime lastmanifestwrite = File.GetLastWriteTime(manifestpath);

                //check if files in the project have been added
                foreach (string s in Directory.GetFiles(path12, "*.*", SearchOption.AllDirectories))
                {
                    if (File.GetCreationTime(s) > lastmanifestwrite)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// checks if any file of type cs, resx or xml has been changed and a rebuild maybe required e.g. right before deployment
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public static bool RebuildRequired(Project project)
        {
            try
            {
                //is there a any file that is younger as the wsp?
                string projectpath = Helpers.GetFullPathOfProjectItem(project);
                string wsppath = DeploymentHelpers.GetWSPFilePath(project.DTE, project);
                string manifestpath = Path.Combine(projectpath, "manifest.xml");
                if (wsppath != "")
                {
                    if (File.Exists(wsppath))
                    {
                        DateTime lastwspwrite = File.GetLastWriteTime(wsppath);

                        //generally: is the create wsp younger than the manifest
                        if (File.Exists(manifestpath))
                        {
                            DateTime manifestwrite = File.GetLastWriteTime(manifestpath);
                            if (manifestwrite > lastwspwrite)
                            {
                                //manifest has been changed and is not part of the current wsp
                                LogMessage(project.DTE, project, "Rebuild required because of file " + manifestpath);
                                return true;
                            }
                        }

                        //check if files in the project have changed
                        foreach (string s in Directory.GetFiles(projectpath, "*.*", SearchOption.AllDirectories))
                        {
                            if (s.EndsWith(".cache", StringComparison.InvariantCultureIgnoreCase))
                            {
                            }
                            else if (s.EndsWith("PackageFileList.txt", StringComparison.InvariantCultureIgnoreCase))
                            {
                            }
                            else if (s.EndsWith(".csproj", StringComparison.InvariantCultureIgnoreCase))
                            {
                            }
                            else if (s.EndsWith(".inf", StringComparison.InvariantCultureIgnoreCase))
                            {
                            }
                            else if (s.EndsWith("StyleCopViolations.xml", StringComparison.InvariantCultureIgnoreCase))
                            {
                            }
                            else if (s.EndsWith("setup.rpt", StringComparison.InvariantCultureIgnoreCase))
                            {
                            }
                            else if (s.EndsWith("csproj.user", StringComparison.InvariantCultureIgnoreCase))
                            {
                            }
                            else if (File.GetLastWriteTime(s) > lastwspwrite)
                            {
                                LogMessage(project.DTE, project, "Rebuild required because of file " + s);
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        private static string GetOutputFolder(Project project)
        {
            //returns GAC or 80/BIN
            try
            {
                return Path.Combine(Helpers.GetFullPathOfProjectItem(project), project.Properties.Item("TargetZone").Value.ToString());
            }
            catch (Exception)
            {
            }
            return "";
        }

        private static bool ProjectContainsDLL(Project project)
        {
            string projectpath = Helpers.GetFullPathOfProjectItem(project);

            //is there any *cs. item with compile?
            foreach (string s in Directory.GetFiles(projectpath, "*.cs", SearchOption.AllDirectories))
            {
                //except folder "Properties" and 12
                if (s.ToLower().StartsWith(Path.Combine(projectpath.ToLower(), "properties")))
                {
                }
                else if (s.ToLower().StartsWith(Path.Combine(projectpath.ToLower(), GetSharePointHive(project.DTE))))
                {
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetProjectFolder(Project project)
        {
            string projectpath = Helpers.GetFullPathOfProjectItem(project);
            FileInfo info = new FileInfo(projectpath);
            return info.DirectoryName;
        }

        public static void RunProcess(DTE dte, string command, string parameters)
        {
            RunProcess(dte, command, parameters, false, "", true);
        }

        public static void RunProcess(DTE dte, string command, string parameters, bool WaitForExit, bool ClearScreen)
        {
            RunProcess(dte, command, parameters, WaitForExit, "", ClearScreen);
        }

        public static void RunProcessAsync(DTE dte, string command, string parameters, string WorkingDirectory)
        {
            try
            {
                Helpers.LogMessage(dte, dte, "Running command \"" + command + "\" " + parameters);
                Helpers.LogMessage(dte, dte, "Please wait");

                int counter = 0;
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = command;
                psi.Arguments = parameters;
                psi.WorkingDirectory = WorkingDirectory;
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo = psi;
                
                
                // Run the process.
                bool fStarted = p.Start();

                if (!fStarted)
                    throw new Exception("Unable to start process.");

                while (!p.HasExited)
                {
                    string text = p.StandardOutput.ReadLine();
                    if (!String.IsNullOrEmpty(text))
                    {
                        Helpers.LogMessage(dte, dte, text);
                        if (counter < 100)
                        {
                            counter += 10;
                        }
                    }
                    System.Threading.Thread.Sleep(100);
                }

                Helpers.LogMessage(dte, dte, p.StandardOutput.ReadToEnd());

                if (p.ExitCode != 0)
                {
                    Helpers.LogMessage(dte, dte, "Failure");
                }
                else
                {
                    Helpers.LogMessage(dte, dte, "Finished successfully");
                }              
                
                Helpers.ShowProgress(dte, "Finished...", 100);
            }
            catch (Exception ex)
            {
                Helpers.LogMessage(dte,dte, "Error: " + ex.ToString());
            }
        }


        public static void RunProcess(DTE dte, string command, string parameters, bool WaitForExit, string WorkingDirectory, bool ClearScreen)
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
            if (ClearScreen)
            {
                OWP.Clear();
            }
            OWP.Activate();

            string dotnet = RuntimeEnvironment.GetRuntimeDirectory();

            OWP.OutputString("Running command " + command + " " + parameters + "\n");
            OWP.OutputString("Please wait...\n");

            System.Diagnostics.Process snProcess = new System.Diagnostics.Process();

            if (!string.IsNullOrEmpty(WorkingDirectory))
            {
                snProcess.StartInfo.WorkingDirectory = WorkingDirectory;
            }
            snProcess.StartInfo.FileName = command;
            snProcess.StartInfo.Arguments = parameters;
            snProcess.StartInfo.CreateNoWindow = true;
            snProcess.StartInfo.UseShellExecute = false;
            snProcess.StartInfo.RedirectStandardOutput = true;
            snProcess.StartInfo.RedirectStandardError = true;
            snProcess.OutputDataReceived += new DataReceivedEventHandler(snProcess_OutputDataReceived);
            snProcess.ErrorDataReceived += new DataReceivedEventHandler(snProcess_ErrorDataReceived);
            snProcess.Exited += new EventHandler(snProcess_Exited);
            snProcess.Start();
            snProcess.BeginErrorReadLine();
            snProcess.BeginOutputReadLine();
            if (WaitForExit)
            {
                snProcess.WaitForExit();
                if (snProcess.ExitCode != 0)
                {
                    throw new Exception("Command '" + command + " " + parameters + "' failed");
                }
            }            
        }

        static void snProcess_Exited(object sender, EventArgs e)
        {
            if (OWP != null)
            {
                if ((sender as System.Diagnostics.Process) != null)
                {
                    if ((sender as System.Diagnostics.Process).ExitCode != 0)
                    {
                        OWP.OutputString("Error during execution\n");
                        OWP.ForceItemsToTaskList();
                    }
                    else
                    {
                        OWP.OutputString("Finished successfully\n");
                        OWP.ForceItemsToTaskList();
                    }
                }
                else
                {
                    OWP.OutputString("Finished successfully\n");
                    OWP.ForceItemsToTaskList();
                }
            }
        }


        static void snProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                if (OWP != null)
                {
                    OWP.OutputString(e.Data + "\n");
                    OWP.ForceItemsToTaskList();
                }
            }
        }

        static void snProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                if (OWP != null)
                {
                    OWP.OutputString(e.Data + "\n");
                    OWP.ForceItemsToTaskList();
                }
            }
        }

        public static bool FeatureExists(string featurename, ITypeDescriptorContext context, DTE service)
        {
            featurename = featurename.ToUpper();

            bool reloadarray = false;

            if (context != null)
            {
                //check if we need to reload the list of features
                if (lastcontext == null)
                {
                    lastcontext = context.Instance;
                    reloadarray = true;
                }
                else
                {
                    if (context.Instance.GetType() == lastcontext.GetType())
                    {
                        object currentcontext = context.Instance;
                        if (!currentcontext.Equals(lastcontext))
                        {
                            lastcontext = currentcontext;
                            reloadarray = true;
                        }
                    }
                }
            }
            else
            { //context is null = reload
                reloadarray = true;
            }
            if ((featurenames == null) || reloadarray)
            {
                //firstload: load all feature names
                featurenames = new ArrayList();

                foreach (Project project in service.Solution.Projects)
                {
                    try
                    {
                        if (project != null)
                        {
                            //check if the feature with the same name already exists
                            ProjectItem featuresfolder = GetProjectItemByName(GetProjectItemByName(GetProjectItemByName(project.ProjectItems, GetSharePointHive(service)).ProjectItems, "template").ProjectItems, "Features");
                            foreach (ProjectItem pitem in featuresfolder.ProjectItems)
                            {
                                featurenames.Add(pitem.Name.ToUpper());
                            }
                        }

                    }
                    catch (Exception)
                    {
                    }
                }
            }
            foreach (object s in featurenames)
            {
                if (s.ToString() == featurename)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetSelectedType(DTE service)
        {
            string type = "unknown";
            if (service.SelectedItems.Count > 0)
            {
                try
                {
                    SelectedItem item = service.SelectedItems.Item(1);
                    if (item.ProjectItem != null)
                    {
                        type = "ProjectItem";
                    }
                    if (item.Project != null)
                    {
                        Project project = (Project)item.Project;
                        type = "Project";
                        if (project.Object is SolutionFolder)
                        {
                            type = "SolutionFolder";
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            return type;
        }

        public static Project GetSelectedProject(DTE service)
        {
            if (service.SelectedItems.Count > 0)
            {
                try
                {
                    SelectedItem item = service.SelectedItems.Item(1);
                    Project project = null;

                    if (item is Project)
                    {
                        project = (Project)item;
                    }
                    else if (item is ProjectItem)
                    {
                        project = ((ProjectItem)item).ContainingProject;
                    }
                    else
                    {
                        if (item.ProjectItem != null)
                        {
                            project = item.ProjectItem.ContainingProject;
                        }
                        else if (item.Project != null)
                        {
                            project = item.Project;
                        }
                    }
                    if (project != null)
                    {
                        return project;
                    }
                }
                catch (Exception)
                {
                }
            }
            return null;
        }

        public static ProjectItems GetItemsOfSelectedItem(DTE service)
        {
            if (service.SelectedItems.Count > 0)
            {
                try
                {
                    SelectedItem item = service.SelectedItems.Item(1);
                    if (item is Project)
                    {
                        return ((Project)item).ProjectItems;
                    }
                    else if (item is ProjectItem)
                    {
                        return ((ProjectItem)item).ProjectItems;
                    }
                    else
                    {
                        if (item.ProjectItem != null)
                        {
                            return item.ProjectItem.ProjectItems;
                        }
                        else if (item.Project != null)
                        {
                            return item.Project.ProjectItems;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            return null;
        }

        public static void OpenProjectItem(ProjectItem pitem)
        {
            try
            {
                if (pitem != null)
                {
                    pitem.Open(EnvDTE.Constants.vsViewKindPrimary).Activate();
                }
            }
            catch (Exception)
            {
            }
        }

        public static List<Project> GetAllProjects(DTE vs)
        {
            List<Project> all = new List<Project>();

            foreach (Project project in vs.Solution.Projects)
            {
                if (project.Object is SolutionFolder)
                {
                    SolutionFolder x = (SolutionFolder)project.Object;
                    foreach (ProjectItem pitem in x.Parent.ProjectItems)
                    {
                        if (pitem.Object != null)
                        {
                            if (pitem.Object is Project)
                            {
                                all.Add(pitem.Object as Project);
                            }
                        }
                    }
                }
                else
                {
                    all.Add(project);
                }
            }
            return all;
        }

        internal static string GetTargetName(object target)
        {
            if(target == null){
                return "null";
            }
            if (target is Project)
            {
                return "Project: " + (target as Project).Name;
            }
            if (target is ProjectItem)
            {
                return "ProjectItem: " + (target as ProjectItem).Name;
            }
            if (target is SolutionFolder)
            {
                return "SolutionFolder: " + (target as SolutionFolder).ToString();
            }
            if (target is Solution)
            {
                return "Solution: "+ (target as Solution).FullName;
            }

            return "unknown: "+ target.ToString() ;
        }

        public static Project GetResourcesProject(DTE vs)
        {
            Project globalresproject = null;

            foreach (Project project in vs.Solution.Projects)
            {
                if (project.Name.EndsWith("Resources") || (project.Name.Equals("Resources", StringComparison.CurrentCultureIgnoreCase)))
                {
                    globalresproject = project;
                }
                if (project.Object is SolutionFolder)
                {
                    SolutionFolder x = (SolutionFolder)project.Object;
                    foreach (ProjectItem pitem in x.Parent.ProjectItems)
                    {
                        if (pitem.Name.EndsWith("Resources"))
                        {
                            if (pitem.Object != null)
                            {
                                if (pitem.Object is Project)
                                {
                                    globalresproject = pitem.Object as Project;
                                }
                            }
                        }
                    }
                }
            }

            return globalresproject;
        }

        public static Project GetProjectByName(DTE vs, string projectName)
        {
            foreach (Project project in vs.Solution.Projects)
            {
                if (project.Object is SolutionFolder)
                {
                    SolutionFolder x = (SolutionFolder)project.Object;
                    foreach (ProjectItem pitem in x.Parent.ProjectItems)
                    {
                        if (pitem.Name.Equals(projectName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (pitem.Object != null)
                            {
                                if (pitem.Object is Project)
                                {
                                    return pitem.Object as Project;
                                }
                            }
                        }
                    }
                }
                else if (project.Name.Equals(projectName, StringComparison.CurrentCultureIgnoreCase))
                {
                    return project;
                }

            }

            return null;
        }

        public static string GetGlobalResourceFilename(Project globalresproject)
        {
            return globalresproject.Name.Replace(".Resources", "").Replace(".", "");
        }

        public static string GetLocalResourceFilename(Project selectedproject)
        {
            return selectedproject.Name.Replace(".Customization", "").Replace(".", "");
        }

        public static bool IsCustomizationProject(Project project)
        {
            //can we cast the project to ISharePointSolution
            try
            {
                //can we convert the feature to a sharepointfeature and check the scope???
                ISharePointProjectService projectService = Helpers2.GetSharePointProjectService(project.DTE);
                ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(project);
                if (sharePointProject != null)
                {
                    return true;
                }
            }
            catch (Exception)
            {

            }

            try
            {
                string manifestXml = Path.Combine(Helpers.GetProjectFolder(project), "manifest.xml");
                if (File.Exists(manifestXml))
                {
                    return true;
                }
            }
            catch (Exception)
            {
            }

            try
            {
                //checks if it contains the folder 12 (to catch WSPBuilder Projects)
                ProjectItem file12 = Helpers.GetProjectItemByName(project.ProjectItems, "12");
                if (file12 != null)
                {
                    return true;
                }
                ProjectItem file14 = Helpers.GetProjectItemByName(project.ProjectItems, "14");
                if (file14 != null)
                {
                    return true;
                }
                ProjectItem file14b = Helpers.GetProjectItemByName(project.ProjectItems, "SharePointRoot");
                if (file14b != null)
                {
                    return true;
                }                
                ProjectItem file15 = Helpers.GetProjectItemByName(project.ProjectItems, "15");
                if (file15 != null)
                {
                    return true;
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        internal static string GetProjectGuid(Project proj)
        {
            try
            {
                object service = null;
                Microsoft.VisualStudio.Shell.Interop.IVsSolution solution = null;
                Microsoft.VisualStudio.Shell.Interop.IVsHierarchy hierarchy = null;
                int result = 0;

                service = GetService(proj.DTE, typeof(Microsoft.VisualStudio.Shell.Interop.IVsSolution));
                solution = (Microsoft.VisualStudio.Shell.Interop.IVsSolution)service;

                result = solution.GetProjectOfUniqueName(proj.UniqueName, out hierarchy);
                Guid projectGuid;
                if (result == 0)
                {
                    hierarchy.GetGuidProperty(
                        Microsoft.VisualStudio.VSConstants.VSITEMID_ROOT,
                        (int)__VSHPROPID.VSHPROPID_ProjectIDGuid,
                        out projectGuid);

                    if (projectGuid != null)
                    {
                        return projectGuid.ToString();
                    }
                }
            }
            catch { }
            return Guid.Empty.ToString();
        }

        internal static string GetProjectTypeGuids(EnvDTE.Project proj)
        {

            string projectTypeGuids = "";
            try
            {
                object service = null;
                Microsoft.VisualStudio.Shell.Interop.IVsSolution solution = null;
                Microsoft.VisualStudio.Shell.Interop.IVsHierarchy hierarchy = null;
                Microsoft.VisualStudio.Shell.Interop.IVsAggregatableProject aggregatableProject = null;
                int result = 0;

                service = GetService(proj.DTE, typeof(Microsoft.VisualStudio.Shell.Interop.IVsSolution));
                solution = (Microsoft.VisualStudio.Shell.Interop.IVsSolution)service;

                result = solution.GetProjectOfUniqueName(proj.UniqueName, out hierarchy);

                if (result == 0)
                {
                    aggregatableProject = (Microsoft.VisualStudio.Shell.Interop.IVsAggregatableProject)hierarchy;
                    result = aggregatableProject.GetAggregateProjectTypeGuids(out projectTypeGuids);
                }
            }
            catch
            {
            }
            return projectTypeGuids;

        }

        internal static object GetService(object serviceProvider, System.Type type)
        {
            return GetService(serviceProvider, type.GUID);
        }

        internal static object GetService(object serviceProviderObject, System.Guid guid)
        {

            object service = null;
            Microsoft.VisualStudio.OLE.Interop.IServiceProvider serviceProvider = null;
            IntPtr serviceIntPtr;
            int hr = 0;
            Guid SIDGuid;
            Guid IIDGuid;

            SIDGuid = guid;
            IIDGuid = SIDGuid;
            serviceProvider = (Microsoft.VisualStudio.OLE.Interop.IServiceProvider)serviceProviderObject;
            hr = serviceProvider.QueryService(ref SIDGuid, ref IIDGuid, out serviceIntPtr);

            if (hr != 0)
            {
                System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(hr);
            }
            else if (!serviceIntPtr.Equals(IntPtr.Zero))
            {
                service = System.Runtime.InteropServices.Marshal.GetObjectForIUnknown(serviceIntPtr);
                System.Runtime.InteropServices.Marshal.Release(serviceIntPtr);
            }

            return service;
        }


        internal static ProjectItem GetFeatureXML(Project _CurrentProject, string ParentFeatureName)
        {
            ProjectItem featureFolder = Helpers2.GetFeature(_CurrentProject.DTE, _CurrentProject, ParentFeatureName);
            ProjectItem featureXMLFile = null;

            if (Helpers2.IsSharePointVSTemplate(_CurrentProject.DTE, _CurrentProject))
            {
                if (featureFolder != null)
                {
                    //is in the feature already the contenttypes.xml?
                    featureXMLFile = Helpers.GetProjectItemByName(featureFolder.ProjectItems, ParentFeatureName + ".feature");
                }
            }
            else
            {
                if (featureFolder != null)
                {
                    //is in the feature already the contenttypes.xml?
                    featureXMLFile = Helpers.GetProjectItemByName(featureFolder.ProjectItems, "feature.xml");
                }
            }
            return featureXMLFile;
        }

        internal static string GetFeatureProperty(ProjectItem featureXMLFile, string attribute)
        {
            string path = Helpers.GetFullPathOfProjectItem(featureXMLFile);

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            if (Helpers2.IsSharePointVSTemplate(featureXMLFile.DTE, featureXMLFile.ContainingProject))
            {
                //what is the scope of the feature
                XmlNamespaceManager featurensmgr = new XmlNamespaceManager(doc.NameTable);
                featurensmgr.AddNamespace("ns", "http://schemas.microsoft.com/VisualStudio/2008/SharePointTools/FeatureModel");

                XmlNode featurenode = doc.SelectSingleNode("/ns:feature ", featurensmgr);
                if (featurenode != null)
                {
                    if (featurenode.Attributes[attribute] != null)
                    {
                        return featurenode.Attributes[attribute].Value;
                    }
                }
            }
            return "";
        }

        internal static string GetFeatureId(ProjectItem featureXMLFile)
        {
            return GetFeatureProperty(featureXMLFile, "id");
        }

        internal static string GetFeatureScope(ProjectItem featureXMLFile)
        {
            return GetFeatureProperty(featureXMLFile, "scope");
        }


        internal static string GetFeatureReceiverClass(ProjectItem featureXMLFile)
        {
            string path = Helpers.GetFullPathOfProjectItem(featureXMLFile);

            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("ns", "http://schemas.microsoft.com/sharepoint/");

            XmlNode node = doc.SelectSingleNode("/ns:Feature", nsmgr);
            if (node != null)
            {
                if (node.Attributes["ReceiverClass"] != null)
                {
                    string receiverFullClass = node.Attributes["ReceiverClass"].Value;
                    string receiverClass = receiverFullClass.Substring(receiverFullClass.LastIndexOf(".") + 1);
                    return receiverClass;
                }
            }
            return "";
        }

        internal static string GetAssemblyName(Project p, string snPath)
        {
            try
            {
                string assemblyfullname = "";

                //get key filename of the current selected project
                string keyfilename = "";

                try
                {
                    keyfilename = Path.GetDirectoryName(p.FullName) + "\\" + p.Properties.Item("AssemblyOriginatorKeyFile").Value;
                    if (!File.Exists(keyfilename))
                    {
                        //file is stored on solution level
                        keyfilename = Path.GetDirectoryName(p.DTE.Solution.FileName) + "\\" + p.Properties.Item("AssemblyOriginatorKeyFile").Value;
                    }

                    //fdsa.fdsfad.fdsafdas, Version=1.0.0.0, Culture=neutral, PublicKeyToken=80b59a59cf67db9f
                    assemblyfullname = p.Properties.Item("AssemblyName").Value + ", Version=" + p.Properties.Item("AssemblyVersion").Value + ", Culture=neutral, PublicKeyToken=";
                }
                catch (Exception)
                {
                }

                if (!File.Exists(keyfilename))
                {
                    throw new FileNotFoundException("The key file could not be found " + keyfilename);
                }

                string publickeyfilepath = Path.GetTempFileName();

                //extract public key into temp file
                ProcessStartInfo startInfo = new ProcessStartInfo(snPath, string.Format("-p \"{0}\" \"{1}\"", keyfilename, publickeyfilepath));
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                System.Diagnostics.Process snProcess = System.Diagnostics.Process.Start(startInfo);
                snProcess.WaitForExit(10000);

                //Get public key token from temp fie
                ProcessStartInfo startInfo2 = new ProcessStartInfo(snPath, string.Format("-t \"{0}\"", publickeyfilepath));
                startInfo2.CreateNoWindow = true;
                startInfo2.UseShellExecute = false;
                startInfo2.RedirectStandardOutput = true;
                System.Diagnostics.Process snProcess2 = System.Diagnostics.Process.Start(startInfo2);
                string output = snProcess2.StandardOutput.ReadToEnd();
                snProcess2.WaitForExit(10000);

                string z = output;

                //output looks like
                //"\r\nMicrosoft (R) .NET Framework Strong Name Utility  Version 2.0.50727.42\r\nCopyright (c) Microsoft Corporation.  All rights reserved.\r\n\r\nPublic key token is 80b59a59cf67db9f\r\n"
                string key = output.Substring(output.IndexOf("Public key token is") + 20, 16);

                assemblyfullname += key;

                return assemblyfullname;
            }
            catch (Exception)
            {
            }
            return "";
        }

        internal static string GetFeatureReceiverNamespace(ProjectItem featureXMLFile)
        {
            string path = Helpers.GetFullPathOfProjectItem(featureXMLFile);

            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("ns", "http://schemas.microsoft.com/sharepoint/");

            XmlNode node = doc.SelectSingleNode("/ns:Feature", nsmgr);
            if (node != null)
            {
                if (node.Attributes["ReceiverClass"] != null)
                {
                    string receiverFullClass = node.Attributes["ReceiverClass"].Value;
                    string receiverNamespace = receiverFullClass.Substring(0, receiverFullClass.LastIndexOf(".") - 1);
                    return receiverNamespace;
                    //findReceiverClass
                    //hat einen Receiver, suche ihn
                }
            }
            return "";
        }

        /// <summary>
        /// returns the given folder or creates the folder
        /// </summary>
        /// <param name="projectItems"></param>
        /// <param name="p"></param>
        /// <param name="p_3"></param>
        /// <returns></returns>
        internal static ProjectItem GetProjectFolder(ProjectItems projectItems, string folderName, bool createIfMissing)
        {
            ProjectItem folderItem = GetProjectItemByName(projectItems, folderName);
            if (folderItem == null)
            {
                if (createIfMissing)
                {
                    folderItem = projectItems.AddFolder(folderName, EnvDTE.Constants.vsProjectItemKindPhysicalFolder);
                }
            }
            return folderItem;
        }

        /*
        internal static string TransformTemplate(string basepath, Hashtable additionalArguments, string templateFile)
        {
            IDictionary<string, PropertyData> arguments = new Dictionary<string, PropertyData>();
            foreach (string str2 in additionalArguments.Keys)
            {
                Type type = additionalArguments[str2].GetType();
                PropertyData data = new PropertyData(additionalArguments[str2], type);
                arguments.Add(str2, data);
            }

            int a = arguments.Count;
            object x = arguments["FeatureReceiverNamespace"].Value;

            TemplateHost host = new TemplateHost(basepath, arguments);
            host.TemplateFile = templateFile;
            string template = File.ReadAllText(host.TemplateFile);
            ITextTemplatingEngine engine = new Microsoft.VisualStudio.TextTemplating.Engine();
            string output = engine.ProcessTemplate(template, host);
            if (host.Errors.HasErrors)
            {
                throw new TemplateException(host.Errors);
            }
            if (host.Errors.HasWarnings)
            {
                StringBuilder builder = new StringBuilder();
                foreach (CompilerError error in host.Errors)
                {
                    builder.AppendLine(error.ToString());
                }
                MessageBox.Show(builder.ToString());
            }
            return output;
        }
        */

        internal static ProjectItem FindItem(Project _CurrentProject, string extension, List<string> patterns)
        {
            ProjectItem founditem = null;
            foreach (ProjectItem item in _CurrentProject.ProjectItems)
            {
                //is it a file?
                if (item.Name.EndsWith(extension))
                {
                    if (ScanFile(item, patterns))
                    {
                        return item;
                    }
                }
                founditem = FindChildItem(item, extension, patterns);
                if (founditem != null)
                {
                    return founditem;
                }
            }
            return founditem;
        }

        private static bool ScanFile(ProjectItem item, List<string> patterns)
        {
            string filename = Helpers.GetFullPathOfProjectItem(item);
            string contents = File.ReadAllText(filename);
            foreach (string s in patterns)
            {
                if (!contents.Contains(s))
                {
                    return false;
                }
            }
            return true;
        }

        private static ProjectItem FindChildItem(ProjectItem parentItem, string extension, List<string> patterns)
        {
            ProjectItem founditem = null;
            foreach (ProjectItem item in parentItem.ProjectItems)
            {
                //is it a file?
                if (item.Name.EndsWith(extension))
                {
                    if (ScanFile(item, patterns))
                    {
                        return item;
                    }
                }
                founditem = FindChildItem(item, extension, patterns);
                if (founditem != null)
                {
                    return founditem;
                }
            }
            return founditem;
        }

        internal static ProjectItem GetFeatureFolder(Project selectedProject, bool createIfNotExists)
        {
            try
            {
                //can we convert the feature to a sharepointfeature and check the scope???
                ISharePointProjectService projectService = Helpers2.GetSharePointProjectService(selectedProject.DTE);
                ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(selectedProject);
                if (sharePointProject != null)
                {
                    ProjectItem item = GetFolder(selectedProject, "Features", createIfNotExists);
                    return item;
                }
                //is there a folder "features" directly in the project and is this folder of type 
            }
            catch { }

            ProjectItem featuresFolder = null;
            try
            {
                featuresFolder = Helpers.GetFolder(Helpers.GetFolder(Helpers.GetFolder(selectedProject, GetSharePointHive(selectedProject.DTE), createIfNotExists), "Template", createIfNotExists), "Features", createIfNotExists);
            }
            catch (Exception)
            {
            }

            try
            {
                if (featuresFolder == null)
                {
                    //try "SharePointRoot"
                    featuresFolder = Helpers.GetFolder(Helpers.GetFolder(Helpers.GetFolder(selectedProject, "SharePointRoot", createIfNotExists), "Template", createIfNotExists), "Features", createIfNotExists);
                }
            }
            catch (Exception)
            {
            }
            return featuresFolder;
        }

        public static bool ExtractWSPToProject(DTE dte, string _WSPFilename, Project _TargetProject, string fullPathToExtractExe)
        {
            Helpers.LogMessage(dte, dte, "Starting import of WSP file " + _WSPFilename);

            WSPExtractor extractor = new WSPExtractor(fullPathToExtractExe, _WSPFilename);
            extractor.ExtractToSolutionToProject(dte, _TargetProject);

            Helpers.LogMessage(dte, dte, "Import finished");

            return true;
        }


        /// <summary>
        /// Returns the feature ProjectItem with the given name
        /// </summary>
        /// <param name="project"></param>
        /// <param name="featurename"></param>
        /// <returns></returns>
        public static ProjectItem GetFeatureFolder(Project project, string featurename)
        {
            try
            {
                return GetProjectItemByName(GetProjectItemByName(GetProjectItemByName(GetProjectItemByName(project.ProjectItems, GetSharePointHive(project.DTE)).ProjectItems, "template").ProjectItems, "Features").ProjectItems, featurename);
            }
            catch (Exception)
            {
            }
            return null;
        }

        static public void CopyFolder(DTE dte, string sourceFolder, string destFolder, ProjectItems parentProjectItems)
        {
            //first copy all files
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                if (File.Exists(dest) && !dte.SuppressUI)
                {
                    if (MessageBox.Show("Project file " + name + " already exists. Overwrite?", "Overwrite file", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Helpers.LogMessage(dte, dte, "Warning: File " + dest + " replaced.");
                        File.Copy(file, dest, true);
                    }
                }
                else
                {
                    File.Copy(file, dest, true);
                }


                try
                { //add file to project items
                    Helpers.AddFromFile(parentProjectItems, dest);
                }
                catch (Exception)
                { //file already existed
                }
            }

            //second create the directories in the project if they are not there
            string[] sourcefolders = Directory.GetDirectories(sourceFolder);
            foreach (string sourcefolder in sourcefolders)
            {
                string name = Path.GetFileName(sourcefolder);
                string dest = Path.Combine(destFolder, name);

                //create folder in the project if it not exists
                ProjectItem projectFolder = null;
                try
                {
                    projectFolder = Helpers.GetProjectItemByName(parentProjectItems, name);
                }
                catch (Exception)
                {
                }
                if (projectFolder == null)
                {
                    projectFolder = parentProjectItems.AddFolder(name, EnvDTE.Constants.vsProjectItemKindPhysicalFolder);
                }
                CopyFolder(dte, sourcefolder, dest, projectFolder.ProjectItems);
            }
        }

        internal static ProjectItem IncludeFolderInProject(string path, ProjectItems projectItems, string subfoldername)
        {
            ProjectItem res = null;
            try
            {
                //includes an existing folder into the project
                //with workaround
                //create dummy in the folder
                //add the file to VS
                //delete the dummy file
                string dummyfilename = "dummy" + Guid.NewGuid().ToString() + ".txt";
                string dummyfilepath = Path.Combine(path, Path.Combine(subfoldername, dummyfilename));

                TextWriter writer = new StreamWriter(dummyfilepath);
                writer.Write("dummyfile, please delete");
                writer.Close();

                ProjectItem dummyfile = Helpers.AddFromFile(projectItems, dummyfilepath);
                //return the created folder
                res = Helpers.GetProjectItemByName(projectItems, subfoldername);

                //clean up
                dummyfile.Delete();
                if (File.Exists(dummyfilepath))
                {
                    File.Delete(dummyfilepath);
                }
            }
            catch (Exception)
            {
            }
            return res;
        }

        internal static ProjectItem GetFolder(Project _CurrentProject, string folderName, bool createIfNotExists)
        {
            return GetFolder(Helpers.GetFullPathOfProjectItem(_CurrentProject), _CurrentProject.ProjectItems, folderName, createIfNotExists);
        }

        internal static ProjectItem GetFolder(ProjectItem _ProjectItem, string folderName, bool createIfNotExists)
        {
            return GetFolder(Helpers.GetFullPathOfProjectItem(_ProjectItem), _ProjectItem.ProjectItems, folderName, createIfNotExists);
        }

        internal static ProjectItem GetFolder(string pathToParent, ProjectItems _projectItems, string folderName, bool createIfNotExists)
        {
            ProjectItem res = GetProjectItemByName(_projectItems, folderName);
            if (res == null)
            {
                if (createIfNotExists)
                {
                    //folder not in the project, so we try to create one
                    try
                    {
                        res = _projectItems.AddFolder(folderName, EnvDTE.Constants.vsProjectItemKindPhysicalFolder);
                    }
                    catch (Exception)
                    {

                        //the folder could not be created, maybe the folder exists, but isnot part of the project
                        string folderpath = Path.Combine(pathToParent, folderName);
                        if (Directory.Exists(folderpath))
                        {
                            //ok, folder is there, lets include it in the project
                            res = IncludeFolderInProject(pathToParent, _projectItems, folderName);
                        }
                    }
                }
            }
            return res;
        }


        /// <summary>
        /// Adds an entry ElementsFile into the feature.xml of the feature
        /// </summary>
        /// <param name="featurefolder"></param>
        /// <param name="resfilename"></param>
        internal static void AddElementsFileToFeature(DTE dte, ProjectItem featurefolder, string resfilename)
        {

            ProjectItem featureXMLFile = GetProjectItemByName(featurefolder.ProjectItems, "feature.xml");
            string featurepath = Helpers.GetFullPathOfProjectItem(featureXMLFile);

            Helpers.LogMessage(dte, dte, "Adding elements file to feature " + featurepath);

            XmlDocument featuredoc = new XmlDocument();
            featuredoc.Load(featurepath);

            //is there already a ElementManifest ContentTypes.xml?
            XmlNamespaceManager featurensmgr = new XmlNamespaceManager(featuredoc.NameTable);
            featurensmgr.AddNamespace("ns", "http://schemas.microsoft.com/sharepoint/");

            XmlNode manifestsnode = featuredoc.SelectSingleNode("/ns:Feature/ns:ElementManifests", featurensmgr);
            if (manifestsnode != null)
            {
                bool manifestexists = false;
                XmlNodeList existingmanifests = manifestsnode.SelectNodes("ns:ElementManifest", featurensmgr);
                foreach (XmlNode existingmanifest in existingmanifests)
                {
                    try
                    {
                        if (existingmanifest.Attributes["Location"].Value.ToUpper() == resfilename.ToUpper())
                        {
                            manifestexists = true;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                if (manifestexists)
                {
                    //everything ok
                }
                else
                {
                    //add the contenttypes.xml as manifest to feature.xml
                    XmlNode maninode = featuredoc.CreateElement("ElementFile", "http://schemas.microsoft.com/sharepoint/");
                    manifestsnode.AppendChild(maninode);

                    XmlAttribute maniattr = featuredoc.CreateAttribute("Location");
                    maniattr.Value = resfilename;
                    maninode.Attributes.Append(maniattr);
                }
            }

            //CheckNamespaces(featuredoc);

            Helpers.EnsureCheckout(dte, featurepath);

            XmlWriter xw2 = XmlWriter.Create(featurepath, Helpers.GetXmlWriterSettings(featurepath));
            featuredoc.Save(xw2);
            xw2.Flush();
            xw2.Close();
        }

        /// <summary>
        /// Ensures that the guidance package is registred for s specific project type, e.g. workflows
        /// </summary>
        /// <returns></returns>
        internal static bool EnsureGaxPackageRegistration(string projectType, string packageGuid, string templatesDir, string packageName)
        {
            //1. get the id of the package
            //2. get the templates dir of the package

            if (!packageGuid.StartsWith("{"))
            {
                packageGuid = "{" + packageGuid + "}";
            }

            //open the template key for Workflows
            try
            {
                RegistryKey startupKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\\Microsoft\\VisualStudio\\9.0\\Projects\\" + projectType + "\\AddItemTemplates\\TemplateDirs\\");

                //is there already our guidance package?
                RegistryKey gaxkey = null;
                try
                {
                    gaxkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\VisualStudio\\9.0\\Projects\\" + projectType + "\\AddItemTemplates\\TemplateDirs\\" + packageGuid + "\\/1", false);
                }
                catch (Exception)
                {
                }
                if (gaxkey == null)
                {
                    //key missing, create now
                    RegistryKey gaxkey1 = startupKey.CreateSubKey(packageGuid);
                    RegistryKey gaxkey2 = gaxkey1.CreateSubKey("/1");

                    gaxkey2.SetValue(string.Empty, packageName);
                    gaxkey2.SetValue("SortPriority", 100, RegistryValueKind.DWord);
                    gaxkey2.SetValue("TemplatesDir", templatesDir);

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Guidance Package " + packageName + " could not be registered for project type " + projectType + ": " + ex.ToString());
            }
            return false;
        }

        internal static string GetLicenseComment()
        {
            return " Code initially generated with SharePoint Software Factory, Version 4.1 , spsf.codeplex.com ";
        }

        internal static string CheckLicenseCommentCS(string Content)
        {
            //TODO Add license code to cs file
            return Content;
        }

        /// <summary>
        /// Adds a license comment to the document if unlicensed version is used
        /// </summary>
        /// <param name="featuredoc"></param>
        internal static void CheckLicenseComment(XmlDocument xmlDoc)
        {
            string licenseComment = GetLicenseComment();
            if (licenseComment == "")
            {
                return;
            }

            try
            {
                bool addcomment = true;
                foreach (XmlNode firstNode in xmlDoc.ChildNodes)
                {
                    if (firstNode.NodeType == XmlNodeType.Comment)
                    {
                        XmlComment existingComment = firstNode as XmlComment;
                        if (existingComment.Data == licenseComment)
                        {
                            addcomment = false;
                        }
                    }
                    else if (firstNode.NodeType == XmlNodeType.Element)
                    {
                        break; //do not search until end of xml
                    }
                }

                if (addcomment)
                {
                    AddLicenseComment(xmlDoc, licenseComment);
                }
            }
            catch (Exception)
            {
            }
        }

        private static void AddLicenseComment(XmlDocument xmlDoc, string licenseComment)
        {
            XmlComment newComment = xmlDoc.CreateComment(licenseComment);
            XmlElement root = xmlDoc.DocumentElement;
            xmlDoc.InsertBefore(newComment, root);
        }

        /// <summary>
        /// returns the solution folder with the given name
        /// </summary>
        /// <param name="soln"></param>
        /// <param name="_SolutionFolder"></param>
        /// <returns></returns>
        internal static Project GetSolutionFolder(EnvDTE80.Solution2 soln, string _SolutionFolder)
        {
            foreach (Project p in soln.Projects)
            {
                if (p.Name == _SolutionFolder)
                {
                    if (p.Object is SolutionFolder)
                    {
                        return p;
                    }
                }
            }
            return null;
        }

        internal static string GetSaveName(string itemname)
        {
            itemname = itemname.Replace(" ", "");
            itemname = itemname.Replace(",", "");
            itemname = itemname.Replace(".", "_");
            itemname = itemname.Replace(")", "");
            itemname = itemname.Replace("(", "");
            itemname = itemname.Replace("ß", "");
            itemname = itemname.Replace("ä", "");
            itemname = itemname.Replace("ö", "");
            itemname = itemname.Replace("Ü", "");
            itemname = itemname.Replace("\\", "");
            itemname = itemname.Replace("}", "");
            itemname = itemname.Replace("{", "");
            itemname = itemname.Replace("]", "");
            itemname = itemname.Replace("[", "");
            return itemname;
        }

        internal static bool IsFeatureFolder(ProjectItem projectItem)
        {
            if (projectItem.Collection.Parent is ProjectItem)
            {
                if (((ProjectItem)projectItem.Collection.Parent).Name.ToUpper() == "FEATURES")
                {
                    return true;
                }
            }
            return false;
        }

        internal static List<Project> GetSelectedProjects(DTE service)
        {
            List<Project> projects = new List<Project>();

            if (service.SelectedItems.Count > 0)
            {
                for (int i = 1; i < service.SelectedItems.Count + 1; i++)
                {
                    SelectedItem item = service.SelectedItems.Item(i);

                    if ((item.Project == null) && (item.ProjectItem == null))
                    {
                        //solution is selected
                        AddProjects(service.Solution, projects);
                    }
                    else if (item.Project is Project)
                    {
                        if (item.Project.Object is SolutionFolder)
                        {
                            SolutionFolder sfolder = item.Project.Object as SolutionFolder;
                            AddProjects(sfolder, projects);
                        }
                        else
                        {
                            AddProjects(item.Project, projects);
                        }
                    }
                    else if (item.ProjectItem != null)
                    {
                        AddProjects(item.ProjectItem.ContainingProject, projects);
                    }
                }
            }
            return projects;
        }

        private static void AddProjects(SolutionFolder sfolder, List<Project> projects)
        {
            foreach (ProjectItem pitem in sfolder.Parent.ProjectItems)
            {
                if (pitem.Object is Project)
                {
                    projects.Add(pitem.Object as Project);
                }
            }
        }

        private static void AddProjects(Solution solution, List<Project> projects)
        {
            foreach (Project project in solution.Projects)
            {
                AddProjects(project, projects);
            }
        }

        private static void AddProjects(Project project, List<Project> projects)
        {
            if (project.Object is SolutionFolder)
            {
                SolutionFolder sfolder = project.Object as SolutionFolder;
                AddProjects(sfolder, projects);
            }
            else
            {
                projects.Add(project);
            }
        }

        internal static List<Project> GetSelectedDeploymentProjects(DTE service)
        {
            List<Project> projects = new List<Project>();

            if (service.SelectedItems.Count > 0)
            {
                for (int i = 1; i < service.SelectedItems.Count + 1; i++)
                {
                    SelectedItem item = service.SelectedItems.Item(i);

                    if ((item.Project == null) && (item.ProjectItem == null))
                    {
                        //solution is selected
                        AddDeploymentProjects(service.Solution, projects);
                    }
                    else if (item.Project is Project)
                    {
                        if (item.Project.Object is SolutionFolder)
                        {
                            SolutionFolder sfolder = item.Project.Object as SolutionFolder;
                            AddDeploymentProjects(sfolder, projects);
                        }
                        else
                        {
                            AddDeploymentProjects(item.Project, projects);
                        }
                    }
                    else if (item.ProjectItem != null)
                    {
                        AddDeploymentProjects(item.ProjectItem.ContainingProject, projects);
                    }
                }
            }
            return projects;
        }

        private static void AddDeploymentProjects(SolutionFolder sfolder, List<Project> projects)
        {
            foreach (ProjectItem pitem in sfolder.Parent.ProjectItems)
            {
                if (pitem.Object is Project)
                {
                    if (IsCustomizationProject((Project)pitem.Object))
                    {
                        projects.Add(pitem.Object as Project);
                    }
                }
            }
        }

        private static void AddDeploymentProjects(Solution solution, List<Project> projects)
        {
            foreach (Project project in solution.Projects)
            {
                AddDeploymentProjects(project, projects);
            }
        }

        private static void AddDeploymentProjects(Project project, List<Project> projects)
        {
            if (project.Object is SolutionFolder)
            {
                SolutionFolder sfolder = project.Object as SolutionFolder;
                AddDeploymentProjects(sfolder, projects);
            }
            else
            {
                if (IsCustomizationProject((Project)project))
                {
                    projects.Add(project);
                }
            }
        }

        internal static bool ContainsCustomizationProject(SolutionFolder f)
        {
            foreach (ProjectItem pi in f.Parent.ProjectItems)
            {
                if (pi.Object is Project)
                {
                    if (Helpers.IsCustomizationProject(pi.Object as Project))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// returns the output filename of the project (e.g. the assembly name)
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        internal static string GetOutputName(Project project)
        {
            try
            {
                return project.Properties.Item("OutputFileName").Value.ToString();
            }
            catch (Exception)
            {
            }
            return "";
        }

        /// <summary>
        /// returns the assembly name without extension .dll
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        internal static string GetOutputNameWithoutExtensions(Project project)
        {
            try
            {
                string output = GetOutputName(project);
                if (output.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Path.GetFileNameWithoutExtension(output);
                }
                return output;
            }
            catch (Exception)
            {
            }
            return "";
        }

        public static ProjectItems GetProjectItemsByPath(Project project, string destinationFolder)
        {
            return GetProjectItemsByPath(project.ProjectItems, destinationFolder);
        }

        /// <summary>
        /// wheretoadd replacement
        /// </summary>
        /// <param name="destinationFolder"></param>
        /// <returns></returns>
        public static ProjectItems GetProjectItemsByPath(ProjectItems projectItems, string destinationFolder)
        {
            ProjectItems whereToAdd;
            if ((destinationFolder != String.Empty) && (destinationFolder != null))
            {

                //only works for a single foldername
                ProjectItem _folder = DteHelper.FindItemByName(projectItems, destinationFolder, false);
                if (_folder != null)
                {
                    whereToAdd = _folder.ProjectItems;
                }
                else
                {

                    ProjectItems pitems = projectItems;
                    string projectpath = "";
                    if (projectItems.Parent is Project)
                    {
                        projectpath = Helpers.GetFullPathOfProjectItem(projectItems.Parent as Project);
                    }
                    else if (projectItems.Parent is ProjectItem)
                    {
                        projectpath = Helpers.GetFullPathOfProjectItem(projectItems.Parent as ProjectItem);
                    }

                    //folder doesnt exist
                    //create the folder
                    char[] sep = { '\\' };
                    string[] folders = destinationFolder.Split(sep);
                    for (int i = 0; i < folders.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(folders[i]))
                        {
                            projectpath += folders[i] + "\\";
                            //does the folder already exist?
                            _folder = DteHelper.FindItemByName(pitems, folders[i], false);
                            if (_folder != null)
                            {
                                //folder exists
                                pitems = _folder.ProjectItems;
                            }
                            else
                            {   //create folder
                                try
                                {
                                    _folder = pitems.AddFolder(folders[i], EnvDTE.Constants.vsProjectItemKindPhysicalFolder);
                                    pitems = _folder.ProjectItems;
                                }
                                catch (Exception)
                                {
                                    _folder = pitems.AddFromDirectory(projectpath);
                                    pitems = _folder.ProjectItems;
                                }
                            }
                        }
                    }
                    whereToAdd = _folder.ProjectItems;
                }
            }
            else
            {
                whereToAdd = projectItems;
            }
            return whereToAdd;
        }

        /// <summary>
        ///find the project item by path
        /// </summary>
        /// <param name="destinationFolder"></param>
        /// <returns></returns>
        public static ProjectItem FindProjectItemByPath(Project project, string itemPath)
        {
            ProjectItem res = null;
            ProjectItems pitems = project.ProjectItems;

            char[] sep = { '\\' };
            string[] folders = itemPath.Split(sep);
            for (int i = 0; i < folders.Length; i++)
            {
                res = GetProjectItemByName(pitems, folders[i]);
                if (res != null)
                {
                    pitems = res.ProjectItems;
                }
            }

            return res;
        }

        internal static void EnsureCheckout(DTE dte, ProjectItem item)
        {
            try
            {
                string itemname1 = Helpers.GetFullPathOfProjectItem(item);
                Helpers.EnsureCheckout(dte, itemname1);
            }
            catch { }
        }

        internal static void EnsureCheckout(DTE dte, Project project)
        {
            try
            {
                string projectname = Helpers.GetFullPathOfProjectItem(project);
                Helpers.EnsureCheckout(dte, project.FullName);
            }
            catch
            {
            }
        }

        internal static void EnsureCheckout(DTE dte, string itemname)
        {
            if (dte.SourceControl.IsItemUnderSCC(itemname))
            {
                if (!dte.SourceControl.IsItemCheckedOut(itemname))
                {
                    LogMessage(dte, dte, "Checking out item " + itemname);
                    dte.SourceControl.CheckOutItem(itemname);
                    LogMessage(dte, dte, "Checking out item finished");
                }
            }
        }

        /// <summary>
        /// takes a filename e.g. "..\12\TEMPLATE\FEATURES\LegacyDocumentLibrary\DocLib\file.xml" and returns ".\12\TEMPLATE\FEATURES\LegacyDocumentLibrary"
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static string GetLocalFeatureDirectory(string filepath)
        {
            FileInfo sinfo = new FileInfo(filepath);
            try
            {
                DirectoryInfo currentDir = sinfo.Directory;
                while (currentDir.Parent.Name.ToLower() != "features")
                {
                    currentDir = currentDir.Parent;
                }
                return currentDir.FullName;
            }
            catch (Exception)
            {
            }
            return "";
        }

        public static string GetResourceValue(string title)
        {
            string resfilename = "";
            string reskey = title.Substring(11);

            if (reskey.Contains(","))
            {
                resfilename = reskey.Substring(0, reskey.IndexOf(","));
                reskey = reskey.Substring(reskey.IndexOf(",") + 1);
                if (reskey.EndsWith(";"))
                {
                    reskey = reskey.Replace(";", "");
                }
            }
            else
            {
                if (reskey.EndsWith(";"))
                {
                    reskey = reskey.Substring(0, reskey.Length - 1);
                }
            }

            if (resfilename != "")
            {
                string finalresfile = "";

                string resfile1 = Helpers.GetSharePointHive() + @"\Resources\" + resfilename + ".resx";
                string resfile2 = Helpers.GetSharePointHive() + @"\Resources\" + resfilename + ".en-US.resx";
                if (File.Exists(resfile1))
                {
                    finalresfile = resfile1;
                }
                else if (File.Exists(resfile2))
                {
                    finalresfile = resfile2;
                }

                if (File.Exists(finalresfile))
                {
                    XmlDocument resdoc = new XmlDocument();
                    resdoc.Load(finalresfile);
                    XmlNode resnode = resdoc.SelectSingleNode("/root/data[@name='" + reskey + "']");
                    if (resnode != null)
                    {
                        return resnode.InnerText;
                    }
                }
            }
            return title;
        }

        /// <summary>
        /// Returns true if the current projectitem is within a feature with the required scopes
        /// </summary>
        /// <param name="featureFolder"></param>
        /// <param name="checkForScopes">list of scopes Web;Site;Farm</param>
        /// <returns></returns>
        internal static bool IsFeatureScope(ProjectItem featureFolder, string checkForScopes)
        {
            if (string.IsNullOrEmpty(checkForScopes))
            {
                return true;
            }

            try
            {
                try
                {
                    //can we convert the feature to a sharepointfeature and check the scope???
                    ISharePointProjectService projectService = Helpers2.GetSharePointProjectService(featureFolder.DTE);
                    if (featureFolder.Name.EndsWith(".feature"))
                    {
                        featureFolder = featureFolder.Collection.Parent as ProjectItem;
                    }
                    ISharePointProjectFeature sharePointFeature = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectFeature>(featureFolder);
                    if(sharePointFeature != null)
                    {
                        if (checkForScopes.ToUpper().Contains(sharePointFeature.Model.Scope.ToString().ToUpper()))
                        {
                            return true;
                        }
                    }

                }
                catch { }

                ProjectItem featureXML = Helpers.GetProjectItemByName(((ProjectItem)featureFolder).ProjectItems, "feature.xml");
                if (featureXML != null)
                {
                    string featurepath = Helpers.GetFullPathOfProjectItem(featureXML);

                    XmlDocument featuredoc = new XmlDocument();
                    featuredoc.Load(featurepath);

                    //what is the scope of the feature
                    XmlNamespaceManager featurensmgr = new XmlNamespaceManager(featuredoc.NameTable);
                    featurensmgr.AddNamespace("ns", "http://schemas.microsoft.com/sharepoint/");

                    XmlNode featurenode = featuredoc.SelectSingleNode("/ns:Feature", featurensmgr);
                    if (featurenode != null)
                    {

                        if (featurenode.Attributes["Scope"] != null)
                        {
                            string scope = featurenode.Attributes["Scope"].Value;
                            if (checkForScopes.Contains(scope))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        internal static bool IsAppPoolRunning(DTE dte, string appPoolName)
        {
            using (DirectoryEntry appPoolsEntry = new DirectoryEntry(string.Format("IIS://{0}/W3SVC/AppPools", "localhost")))
            {
                foreach (DirectoryEntry AppPool in appPoolsEntry.Children)
                {

                    string props = "";
                    foreach (string s in AppPool.Properties.PropertyNames)
                    {
                        try
                        {
                            props += AppPool.Properties[s].PropertyName + "=" + AppPool.Properties[s].Value + Environment.NewLine;
                        }
                        catch (Exception)
                        {
                        }
                    }

                    if (appPoolName.Equals(AppPool.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        int intStatus = (int)AppPool.InvokeGet("AppPoolState");
                        if (intStatus == 2)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        internal static void RecycleAllAppPools(DTE dte)
        {
            Helpers.LogMessage(dte, dte, "*** Recycling all applicatin pools ***");
            using (DirectoryEntry appPoolsEntry = new DirectoryEntry(string.Format("IIS://{0}/W3SVC/AppPools", "localhost")))
            {
                foreach (DirectoryEntry AppPool in appPoolsEntry.Children)
                {

                    Helpers.LogMessage(dte, dte, "Recycle application pool " + AppPool.Name + "...");
                    AppPool.Invoke("Recycle", null);
                }
            }
        }

        internal static void RecycleAppPool(DTE dte, string appPoolName)
        {
            Helpers.LogMessage(dte, dte, "Recycling app pool '" + appPoolName + "'");
            using (DirectoryEntry appPoolsEntry = new DirectoryEntry(string.Format("IIS://{0}/W3SVC/AppPools", "localhost")))
            {
                foreach (DirectoryEntry AppPool in appPoolsEntry.Children)
                {
                    if (appPoolName.Equals(AppPool.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        Helpers.LogMessage(dte, dte, "AppPool " + appPoolName + " found.");
                        AppPool.Invoke("Recycle", null);
                        return;
                    }
                }
            }
            Helpers.LogMessage(dte, dte, "AppPool " + appPoolName + " not found.");
        }

        internal static XmlWriterSettings GetXmlWriterSettings(string targetFileName)
        {
            if (targetFileName.ToUpper().EndsWith(".XML"))
            {
                return GetXmlWriterSettingsAttributesInNewLine();
            }
            else
            {
                return GetXmlWriterSettingsAttributesInOneLine();
            }
        }

        internal static XmlWriterSettings GetXmlWriterSettingsAttributesInNewLine()
        {
            XmlWriterSettings wSettings = new XmlWriterSettings();
            wSettings.Indent = true;
            wSettings.NewLineOnAttributes = true;
            wSettings.IndentChars = "\t";
            return wSettings;
        }

        internal static XmlWriterSettings GetXmlWriterSettingsAttributesInOneLine()
        {
            XmlWriterSettings wSettings = new XmlWriterSettings();
            wSettings.Indent = true;
            wSettings.NewLineOnAttributes = false;
            wSettings.IndentChars = "\t";
            return wSettings;
        }

        internal static ProjectItem AddFromFile(ProjectItem projectItem, string filename)
        {
            return AddFromFile(projectItem.ProjectItems, filename);
        }

        internal static ProjectItem AddFromFileCopy(ProjectItem projectItem, string filename)
        {
            ProjectItem pitem = projectItem.ProjectItems.AddFromFileCopy(filename);
            //Helpers.LogMessage(projectItem.DTE, projectItem.DTE, Helpers.GetFullPathOfProjectItem(pitem) + ": File Added");

            Helpers.CheckSharePointReferences(pitem);

            return pitem;
        }

        private static void CheckSharePointReferences(ProjectItem pitem)
        {
            try
            {
                if (pitem.Name.EndsWith(".cs", StringComparison.InvariantCultureIgnoreCase))
                {
                    VSLangProj.VSProject proj = pitem.ContainingProject.Object as VSLangProj.VSProject;
                    proj.References.Add("Microsoft.SharePoint");
                    proj.References.Add("Microsoft.SharePoint.Security");
                }
            }
            catch (Exception)
            {
            }
        }

        internal static ProjectItem AddFromFile(ProjectItems projectItems, string filename)
        {
            ProjectItem pitem = projectItems.AddFromFile(filename);

            try
            {
                Helpers.CheckSharePointReferences(pitem);
                //Helpers.LogMessage(projectItems.DTE, projectItems.DTE, filename + ": File Added");
           }
            catch (Exception)
            {
            }

            return pitem;
        }

        internal static string GetPathOfProjectItems(ProjectItems projectItems)
        {
            if (projectItems.Parent is ProjectItem)
            {
                return Helpers.GetFullPathOfProjectItem(projectItems.Parent as ProjectItem);
            }
            else
            {
                //return path to the project
                return Helpers.GetFullPathOfProjectItem(projectItems.ContainingProject);
            }
        }

        internal static ProjectItem AddFromTemplate(ProjectItems projectItems, string template, string TargetFileName)
        {
            return AddFromTemplate(projectItems, template, TargetFileName, false);
        }

        internal static ProjectItem AddFromTemplate(ProjectItems projectItems, string template, string TargetFileName, bool overwrite)
        {
            ProjectItem pitem = null;
            try
            {
                //is there already a project item with the target name 
                ProjectItem existingItem = GetProjectItemByName(projectItems, TargetFileName);
                if (existingItem != null)
                {
                    //item with the same name already there
                    //overwrite it?
                    if (overwrite || projectItems.DTE.SuppressUI)
                    {
                        //overwrite the file with the new contents
                        string pathToExistingItem = Helpers.GetFullPathOfProjectItem(existingItem);
                        Helpers.EnsureCheckout(projectItems.DTE, pathToExistingItem);
                        File.Copy(template, pathToExistingItem, true);
                        Helpers.LogMessage(projectItems.DTE, projectItems.DTE, pathToExistingItem + ": Contents updated");
                    }
                    else
                    {
                        if (MessageBox.Show("File " + TargetFileName + " already exists. Overwrite?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            //overwrite the file with the new contents
                            string pathToExistingItem = Helpers.GetFullPathOfProjectItem(existingItem);
                            Helpers.EnsureCheckout(projectItems.DTE, pathToExistingItem);
                            File.Copy(template, pathToExistingItem, true);
                            Helpers.LogMessage(projectItems.DTE, projectItems.DTE, pathToExistingItem + ": Contents updated");
                        }
                    }
                    return existingItem;
                }

                //is there already a file with the same name, but it is not part of the project
                string pathToExistingFile = Path.Combine(GetPathOfProjectItems(projectItems), TargetFileName);
                if (File.Exists(pathToExistingFile))
                {
                    //ups, file with same name exists, but is not part of the project
                    //update the contents and include the file to the project
                    Helpers.EnsureCheckout(projectItems.DTE, pathToExistingFile);
                    File.Copy(template, pathToExistingFile, true);
                    return Helpers.AddFromFile(projectItems, pathToExistingFile);
                }

                //default situation: a file or item with the same name does not exists at the target location
                pitem = projectItems.AddFromTemplate(template, TargetFileName);
//                Helpers.LogMessage(projectItems.DTE, projectItems.DTE, Helpers.GetFullPathOfProjectItem(pitem) + ": File Added");
                Helpers.CheckSharePointReferences(pitem);
            }
            catch (Exception)
            {
                Helpers.LogMessage(projectItems.DTE, projectItems.DTE, Helpers.GetFullPathOfProjectItem(pitem) + ": Already exists");
            }

            return pitem;
        }

        internal static ProjectItem AddFromFile(Project project, string filename)
        {
            return AddFromFile(project.ProjectItems, filename);
        }

        public static void LogMessage(DTE dte, object sender, string message)
        {
            //string logmessage = "[" + DateTime.Now.ToLongTimeString() + "] " + sender.GetType().Name + ": " + message;
            //string logmessage = "[" + DateTime.Now.ToLongTimeString() + "] " + message;
            string logmessage = message;
            WriteToOutputWindow(dte, logmessage);
            //LogMessage(dte, dte, logmessage);
            //Trace.WriteLine(logmessage);
            Application.DoEvents();
        }

        public static void WriteToOutputWindow(DTE dte, string message)
        {
            WriteToOutputWindow(dte, message, false);
        }

        public static void WriteToOutputWindow(DTE dte, string message, bool clearBefore)
        {
            //Log(message);
            try
            {
                Window win = dte.Windows.Item(EnvDTE.Constants.vsWindowKindOutput);
                OutputWindow comwin = (OutputWindow)win.Object;
                if (OWP == null)
                {
                    foreach (OutputWindowPane w in comwin.OutputWindowPanes)
                    {
                        if (w.Name == "SharePoint Software Factory")
                        {
                            OWP = w;
                        }
                    }
                }
                if (OWP == null)
                {
                    OWP = comwin.OutputWindowPanes.Add("SharePoint Software Factory");
                }
                if (clearBefore)
                {
                    OWP.Clear();
                }
                OWP.Activate();


                Application.DoEvents();
                OWP.OutputString(message + Environment.NewLine);
                OWP.ForceItemsToTaskList();
            }
            catch (Exception)
            {
            }
        }

        internal static void ReportStatus(DTE dte, string message, int percentage)
        {
            try
            {
                dte.StatusBar.Progress(true, message, percentage, 100);
            }
            catch (Exception)
            {
            }
        }

        public static void NavigateProjectItem(ProjectItem item, List<NameValueItem> _list, string internalXPath, string internalNamespace, XmlNodeHandler nodeHandler, string fileExtension)
        {
            if (item.Name.ToLower().EndsWith(fileExtension))
            {
                try
                {
                    string path = GetFullPathOfProjectItem(item);

                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);

                    XmlNodeList foundnodes = null;

                    if (internalNamespace != "")
                    {
                        XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                        nsmgr.AddNamespace("ns", internalNamespace);
                        foundnodes = doc.SelectNodes(internalXPath, nsmgr);
                    }
                    else
                    {
                        foundnodes = doc.SelectNodes(internalXPath);
                    }

                    XmlDocument resourceDoc = null;
                    if (foundnodes.Count > 0)
                    {
                        //jetzt macht es sinn auch das zugehörige Resourcefile zu suchen
                        //suche das XmlDocument mit den Resourcen für das aktuelle Feature
                        //1. könnte ein feature sein, dann sind die daten im Features/Feature1/Resources/Resources.resx
                        //2. könnte eine sitedefinition sein, dann in 12/Resources suchen
                        try
                        {
                            ProjectItem featurefolder = GetFeatureFolderByProjectItem(item);
                            if (featurefolder != null)
                            {
                                ProjectItem resourceitem = GetProjectItemByName(GetProjectItemByName(featurefolder.ProjectItems, "Resources").ProjectItems, "Resources.resx");
                                if (resourceitem != null)
                                {
                                    string respath = Helpers.GetFullPathOfProjectItem(resourceitem);
                                    resourceDoc = new XmlDocument();
                                    resourceDoc.Load(respath);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            //no resource document found
                        }
                    }
                    foreach (XmlNode node in foundnodes)
                    {

                        foreach (NameValueItem nvitem in nodeHandler.GetNameValueItems(node, resourceDoc, item))
                        {
                            if (nvitem != null)
                            {
                                _list.Add(nvitem);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            if (item.Object is Project)
            {
                Project p = item.Object as Project;
                NavigateProjectItems(p.ProjectItems, _list, internalXPath, internalNamespace, nodeHandler, fileExtension);
            }
            else
            {
                if (item.ProjectItems != null)
                {
                    if (item.ProjectItems.Count > 0)
                    {
                        NavigateProjectItems(item.ProjectItems, _list, internalXPath, internalNamespace, nodeHandler, fileExtension);
                    }
                }
            }
        }

        internal static string GetFullPathOfProjectItem(ProjectItem item)
        {

            string path = item.get_FileNames(1);
            if (string.IsNullOrEmpty(path))
            {
                path = item.Properties.Item("FullPath").Value.ToString();
            }
            return path;
        }
        internal static string GetFullPathOfProjectItem(Project project)
        {
            return project.Properties.Item("FullPath").Value.ToString();
        }
        /// <summary>
        /// searches 
        /// </summary>
        /// <param name="projectitems"></param>
        /// <param name="_list"></param>
        /// <param name="internalXPath"></param>
        /// <param name="internalNamespace"></param>
        /// <param name="nodeHandler"></param>
        public static void NavigateProjectItems(ProjectItems projectitems, List<NameValueItem> _list, string internalXPath, string internalNamespace, XmlNodeHandler nodeHandler, string fileExtension)
        {
            if (projectitems != null)
            {
                foreach (ProjectItem item in projectitems)
                {
                    NavigateProjectItem(item, _list, internalXPath, internalNamespace, nodeHandler, fileExtension);


                }
            }
        }

        //searches from the current item to the top to find the feature folder of the current item
        internal static ProjectItem GetFeatureFolderByProjectItem(ProjectItem item)
        {
            ProjectItem featureFolder = null;

            string path = Helpers.GetFullPathOfProjectItem(item);
            if (path.ToUpper().Contains("\\TEMPLATE\\FEATURES"))
            {
                //ok we are below features, it is worth to search for the current feature
                //Find the feature name in the path of the item
                /*path = path.Substring(path.IndexOf("\\TEMPLATE\\FEATURES", StringComparison.InvariantCultureIgnoreCase) + 19);
                if (path.Contains("\\"))
                {
                  path = path.Substring(0, path.IndexOf("\\"));
                }

                //ok, in path we have the feature name
                //so we try to select the project item in 12/templates.. from top down
                featureFolder = GetProjectItemByName(GetProjectItemByName(GetProjectItemByName(GetProjectItemByName(item.ContainingProject.ProjectItems, GetSharePointHive(item.ContainingProject.DTE)).ProjectItems, "Template").ProjectItems, "Features").ProjectItems, path);
              */
                featureFolder = item;
                while (!(featureFolder.Collection.Parent as ProjectItem).Name.Equals("Features", StringComparison.InvariantCultureIgnoreCase))
                {
                    featureFolder = featureFolder.Collection.Parent as ProjectItem;
                }
            }
            return featureFolder;
        }

        /// <summary>
        /// Returns the name of the application (name of the visual studio solution)
        /// </summary>
        /// <param name="dte"></param>
        /// <returns></returns>
        internal static string GetApplicationName(DTE dte)
        {
            string appname = dte.Solution.Properties.Item("Name").Value.ToString();
            appname = System.Text.RegularExpressions.Regex.Replace(appname, @"[^\w\.@-]", string.Empty);
            return appname;
        }

        internal static string GetSaveApplicationName(DTE dte)
        {
            string appname = GetApplicationName(dte);
            appname = appname.Replace(".", "_");
            return appname;
        }

        internal static string GetSaveProjectName(Project project)
        {
            string appname = project.Name;
            appname = appname.Replace(".", "_");
            return appname;
        }

        internal static string GetSolutionId(DTE service)
        {
            string resSolutionid = "";

            Project project = Helpers.GetSelectedProject(service);
            if (project != null)
            {
                if (Helpers2.IsSharePointVSTemplate(service, project))
                {
                    try
                    {
                        ISharePointProjectService projectService = Helpers2.GetSharePointProjectService(service);
                        ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(project);
                        resSolutionid = sharePointProject.Package.Model.SolutionId.ToString(); ;
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                try
                {
                    //1. check for manifest.xml in root folder of project

                    string projectDirectory = Helpers.GetProjectFolder(project);
                    string manifestXmlPath = Path.Combine(projectDirectory, "manifest.xml");
                    string wspSolutionIdFile = Path.Combine(projectDirectory, "solutionid.txt");

                    if (File.Exists(manifestXmlPath))
                    {
                        XmlDocument manifestXml = new XmlDocument();
                        manifestXml.Load(manifestXmlPath);
                        XmlNamespaceManager wspnsmgr = new XmlNamespaceManager(manifestXml.NameTable);
                        wspnsmgr.AddNamespace("ns", "http://schemas.microsoft.com/sharepoint/");
                        XmlNode solutionNode = manifestXml.SelectSingleNode("/ns:Solution", wspnsmgr);
                        if (solutionNode != null)
                        {
                            if (solutionNode.Attributes["SolutionId"] != null)
                            {
                                resSolutionid = solutionNode.Attributes["SolutionId"].Value;
                            }
                        }
                    }
                    else if (File.Exists(wspSolutionIdFile))
                    {
                        //check for solutionid.txt
                        resSolutionid = File.ReadAllText(wspSolutionIdFile);
                    }
                }
                catch (Exception)
                {
                }
            }
            return resSolutionid;
        }

        /// <summary>
        /// Returns the value of a PropertyGroup element in the csproj file
        /// </summary>
        /// <param name="service"></param>
        /// <param name="ConfigValue"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        internal static object GetProjectPropertyGroupValue(DTE service, string propertyName, string defaultValue)
        {
            Project project = GetSelectedProject(service);
            return GetProjectPropertyGroupValue(project, propertyName, defaultValue);
        }

        public static bool GetIsSandboxedSolution(Project project)
        {
            //can we cast the project to ISharePointSolution
            try
            {
                //can we convert the feature to a sharepointfeature and check the scope???
                ISharePointProjectService projectService = Helpers2.GetSharePointProjectService(project.DTE);
                ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(project);
                if (sharePointProject != null)
                {
                    if (sharePointProject.IsSandboxedSolution)
                    {
                        return true;
                    }
                }
            }
            catch { }

            try
            {
                return Boolean.Parse(GetProjectPropertyGroupValue(project, "SandboxedSolution", "false").ToString());
            }
            catch
            {
            }
            return false;
        }

        internal static object GetProjectPropertyGroupValue(Project project, string propertyName, string defaultValue)
        {
            IVsSolution solution = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SVsSolution)) as IVsSolution;
            DTE dte = project.DTE;
            //IServiceProvider sp = dte as IServiceProvider;
            IServiceProvider sp = new Microsoft.VisualStudio.Shell.ServiceProvider(project.DTE as
        Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
            string uniqueName = project.UniqueName;
            IVsHierarchy hierarchy;

            hierarchy = DteHelper.GetVsHierarchy(sp, project);

            //solution.GetProjectOfUniqueName(uniqueName, out hierarchy);

            IVsBuildPropertyStorage buildPropertyStorage = hierarchy as IVsBuildPropertyStorage;
            string value = null;
            int hr = buildPropertyStorage.GetPropertyValue(propertyName, null, (uint)_PersistStorageType.PST_PROJECT_FILE, out value);

            if (value == null)
            {
                value = defaultValue;
            }
            return value;
        }

        internal static void SetProjectPropertyGroupValue(Project project, string propertyName, string newValue)
        {
            IVsSolution solution = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SVsSolution)) as IVsSolution;
            DTE dte = project.DTE;
            //IServiceProvider sp = dte as IServiceProvider;
            IServiceProvider sp = new Microsoft.VisualStudio.Shell.ServiceProvider(project.DTE as
            Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
            string uniqueName = project.UniqueName;
            IVsHierarchy hierarchy;

            hierarchy = DteHelper.GetVsHierarchy(sp, project);

            IVsBuildPropertyStorage buildPropertyStorage = hierarchy as IVsBuildPropertyStorage;
            int hr = buildPropertyStorage.SetPropertyValue(propertyName, null, (uint)_PersistStorageType.PST_PROJECT_FILE, newValue);


        }

        internal static void SetProjectPropertyGroupValue(IVsSolution solution, DTE service, string propertyName, string newValue)
        {
            Project project = GetSelectedProject(service);
            string uniqueName = project.UniqueName;
            IVsHierarchy hierarchy;
            solution.GetProjectOfUniqueName(uniqueName, out hierarchy);

            IVsBuildPropertyStorage buildPropertyStorage = hierarchy as IVsBuildPropertyStorage;
            int hr = buildPropertyStorage.SetPropertyValue(propertyName, null, (uint)_PersistStorageType.PST_PROJECT_FILE, newValue);

            LogMessage(service, project, "Updating project property '" + propertyName + "' with value '" + newValue + "'");
        }

        internal static bool IsTargetInSTSADMCustomizationProject(object target)
        {
            try
            {
                Project checkProject = GetParentProject(target);
                if (checkProject != null)
                {
                    //check for a file in 12/config/stsadmcommands.*.xml
                    string stsadmConfigFilesDir = Helpers.GetFullPathOfProjectItem(checkProject) + "\\" + Helpers.GetSharePointVersion(checkProject.DTE) + "\\CONFIG";
                    foreach (string stsadmConfigFile in Directory.GetFiles(stsadmConfigFilesDir, "*.xml"))
                    {

                        if (Path.GetFileName(stsadmConfigFile).StartsWith("stsadmcommands.", StringComparison.InvariantCultureIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        internal static Project GetParentProject(object target)
        {
            if (target is Project)
            {
                return target as Project;
            }

            if (target is ProjectItem)
            {
                return (target as ProjectItem).ContainingProject;
            }
            return null;
        }
        internal static bool IsTargetInCustomizationProject(object target)
        {
            if (target is ProjectItem)
            {
                if ((target as ProjectItem).Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFile)
                {
                    //not enable on file level, but on folders it could be ok
                    return false;
                }
            }

            Project checkProject = GetParentProject(target);

            if (checkProject != null)
            {
                if (Helpers.IsCustomizationProject(checkProject))
                {
                    return true;
                }
                else if (checkProject.Object is SolutionFolder)
                {
                    /*
                    SolutionFolder f = ((Project)target).Object as SolutionFolder;
                    string i = f.Parent.Name;
                    string o = ((Project)target).Name;
                    return true;
                     * */
                }
            }
            return false;
        }

        internal static Dictionary<string, CustomResXResourceReader> GetResourcesInSolution(DTE dte)
        {
            Dictionary<string, CustomResXResourceReader> dict = new Dictionary<string, CustomResXResourceReader>();

            //find all resx files in the current solution
            if (dte.Solution != null)
            {
                //search each project in the solution
                foreach (Project project in dte.Solution.Projects)
                {
                    if (project.Object is SolutionFolder)
                    {
                        SolutionFolder x = (SolutionFolder)project.Object;
                        foreach (ProjectItem pitem in x.Parent.ProjectItems)
                        {
                            if (pitem.Object != null)
                            {
                                if (pitem.Object is Project)
                                {
                                    GetResourcesInProject(pitem.Object as Project, dict);
                                }
                            }
                        }
                    }
                    else
                    {
                        GetResourcesInProject(project, dict);
                    }
                }
            }
            return dict;
        }

        internal static void GetResourcesInProject(Project project, Dictionary<string, CustomResXResourceReader> dict)
        {
            try
            {
                string projectDir = Directory.GetParent(Helpers.GetFullPathOfProjectItem(project)).FullName;
                string resdir = Path.Combine(projectDir, @"12\Resources");
                if (Directory.Exists(resdir))
                {
                    ReadResxFilesInDirectory(resdir, dict);
                }
                resdir = Path.Combine(projectDir, @"Resources");
                if (Directory.Exists(resdir))
                {
                    ReadResxFilesInDirectory(resdir, dict);
                }
            }
            catch (Exception)
            {
            }
        }

        internal static void ReadResxFilesInDirectory(string resourcefolder, Dictionary<string, CustomResXResourceReader> resourcesDictionary)
        {
            foreach (string resxFile in Directory.GetFiles(resourcefolder, "*.resx", SearchOption.AllDirectories))
            {
                try
                {
                    string key = Path.GetFileNameWithoutExtension(resxFile);
                    if (!resourcesDictionary.ContainsKey(key))
                    {
                        CustomResXResourceReader resxReader = new CustomResXResourceReader(resxFile);
                        resourcesDictionary.Add(resxReader.GetKey(), resxReader);
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        internal static void LoadResources(string resourcefolder, Dictionary<string, CustomResXResourceReader> resourcesDictionary)
        {
            ReadResxFilesInDirectory(resourcefolder, resourcesDictionary);
        }

        //resourcekey = $Resources:core,nocodeworkflowlibraryFeatureTitle;
        internal static string GetResourceString(string resourcekey, string featurename, Dictionary<string, CustomResXResourceReader> resourcesDictionary)
        {
            if ((resourcekey == "") && (featurename != ""))
            {
                //Title is empty, then return feature name
                return featurename;
            }

            if (!resourcekey.StartsWith("$Resources:"))
            {
                return resourcekey;
            }

            if (resourcekey.Contains(","))
            {
                string filename = resourcekey.Substring(resourcekey.IndexOf(":") + 1, resourcekey.IndexOf(",") - resourcekey.IndexOf(":") - 1);
                string datakey = resourcekey.Substring(resourcekey.IndexOf(",") + 1, resourcekey.Length - resourcekey.IndexOf(",") - 1);
                if (datakey.EndsWith(";"))
                {
                    datakey = datakey.Replace(";", "");
                }
                return GetResourceStringInFile(filename, datakey, resourcesDictionary);
            }
            else
            {
                //no resourcefilename given, search for core resx files
                string datakey = resourcekey.Substring(resourcekey.IndexOf(":") + 1);
                if (datakey.EndsWith(";"))
                {
                    datakey = datakey.Substring(0, datakey.Length - 1);
                }
                string resourceValue = GetResourceStringInFile("core", datakey, resourcesDictionary);
                if (resourceValue == "")
                {
                    resourceValue = GetResourceStringInFile("spscore", datakey, resourcesDictionary);
                }
                if (resourceValue == "")
                {
                    if (featurename != "")
                    {
                        //try with feature name
                        resourceValue = GetResourceStringInFile(featurename, datakey, resourcesDictionary);
                    }
                }
                if (resourceValue != "")
                {
                    return resourceValue;
                }
            }

            return resourcekey;
        }

        internal static string GetResourceStringInFile(string keyfile, string resourcekey, Dictionary<string, CustomResXResourceReader> resourcesDictionary)
        {
            CustomResXResourceReader resReader = null;
            if (resourcesDictionary.ContainsKey(keyfile))
            {
                //file without .en-us. found
                resReader = resourcesDictionary[keyfile];
            }
            else if (resourcesDictionary.ContainsKey(keyfile + ".en-US"))
            {
                //file without .en-us. found
                resReader = resourcesDictionary[keyfile + ".en-US"];
            }
            else
            {
                //find a file with .en-us. 
                foreach (string key in resourcesDictionary.Keys)
                {
                    if (key.StartsWith(keyfile, StringComparison.CurrentCultureIgnoreCase))
                    {
                        resReader = resourcesDictionary[key];
                    }
                }
            }

            if (resReader != null)
            {
                return resReader.GetValue(resourcekey);
            }
            return resourcekey;

        }

        public static void ShowProgress(DTE dte, string message, int percentage)
        {
            if (dte != null)
            {
                try
                {
                    dte.StatusBar.Progress(true, message, percentage, 100);
                }
                catch (Exception)
                {
                }
            }
        }

        public static void HideProgress(DTE dte)
        {
            if (dte != null)
            {
                try
                {
                    dte.StatusBar.Progress(false, "Finished", 100, 100);
                }
                catch (Exception)
                {
                }
            }
        }

        public static void GetAllFeatures(List<NameValueItem> list, Project project)
        {
            if (Helpers2.IsSharePointVSTemplate(project.DTE, project))
            {
                GetAllFeaturesInVSTemplate(list, project);
            }
            else
            {
                GetAllFeaturesInHIVETemplate(list, project);
            }
        }

        internal static void GetAllFeaturesInVSTemplate(List<NameValueItem> list, Project project)
        {
            ProjectItem featuresFolder = null;
            try
            {
                featuresFolder = Helpers.GetProjectItemByName(project.ProjectItems, "Features");
            }
            catch (Exception)
            {
            }

            if (featuresFolder != null)
            {
                foreach (ProjectItem feature in featuresFolder.ProjectItems)
                {
                    try
                    {
                        string scope = "Web";
                        string id = "";
                        ProjectItem featureXML = Helpers.GetProjectItemByName(feature.ProjectItems, feature.Name + ".feature");
                        if (featureXML != null)
                        {
                            string featurepath = Helpers.GetFullPathOfProjectItem(featureXML);

                            XmlDocument featuredoc = new XmlDocument();
                            featuredoc.Load(featurepath);

                            //what is the scope of the feature
                            XmlNamespaceManager featurensmgr = new XmlNamespaceManager(featuredoc.NameTable);
                            featurensmgr.AddNamespace("ns", "http://schemas.microsoft.com/VisualStudio/2008/SharePointTools/FeatureModel");

                            XmlNode featurenode = featuredoc.SelectSingleNode("/ns:feature ", featurensmgr);
                            if (featurenode != null)
                            {
                                if (featurenode.Attributes["scope"] != null)
                                {
                                    scope = featurenode.Attributes["scope"].Value;
                                }
                                if (featurenode.Attributes["featureId"] != null)
                                {
                                    id = featurenode.Attributes["featureId"].Value;
                                }
                            }
                        }
                        AddNameValueItem(feature.Name, id, scope, list);
                    }
                    catch { }
                }
            }
        }

        private static void GetAllFeaturesInHIVETemplate(List<NameValueItem> list, Project project)
        {

            //geht all subfolders in folder 12/templates/features/ of the project
            ProjectItem featuresFolder = GetFeatureFolder(project, false);
            if (featuresFolder != null)
            {
                foreach (ProjectItem feature in featuresFolder.ProjectItems)
                {
                    string scope = "";
                    string id = "";
                    ProjectItem featureXML = Helpers.GetProjectItemByName(feature.ProjectItems, "feature.xml");
                    if (featureXML != null)
                    {
                        string featurepath = Helpers.GetFullPathOfProjectItem(featureXML);

                        XmlDocument featuredoc = new XmlDocument();
                        featuredoc.Load(featurepath);

                        //what is the scope of the feature
                        XmlNamespaceManager featurensmgr = new XmlNamespaceManager(featuredoc.NameTable);
                        featurensmgr.AddNamespace("ns", "http://schemas.microsoft.com/sharepoint/");

                        XmlNode featurenode = featuredoc.SelectSingleNode("/ns:Feature", featurensmgr);
                        if (featurenode != null)
                        {
                            if (featurenode.Attributes["Scope"] != null)
                            {
                                scope = featurenode.Attributes["Scope"].Value;
                            }
                            if (featurenode.Attributes["Id"] != null)
                            {
                                id = featurenode.Attributes["Id"].Value;
                            }
                        }
                    }
                    AddNameValueItem(feature.Name, id, scope, list);
                }
            }
        }

        internal static void AddNameValueItem(string Name, string Value, string Description, List<NameValueItem> list)
        {
            NameValueItem item = new NameValueItem();
            item.ItemType = "Feature";
            item.Name = Name;
            item.Description = Description;
            item.Value = Value;
            list.Add(item);
        }

        /// <summary>
        /// returns all project in the current solution (include project in solution folders
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        internal static List<Project> GetAllProjects(_DTE dte)
        {
            List<Project> resProjects = new List<Project>();

            foreach (Project project in dte.Solution.Projects)
            {
                if (project.Object is SolutionFolder)
                {
                    SolutionFolder x = (SolutionFolder)project.Object;
                    foreach (ProjectItem pitem in x.Parent.ProjectItems)
                    {
                        if (pitem.Object != null)
                        {
                            if (pitem.Object is Project)
                            {
                                resProjects.Add(pitem.Object as Project);
                            }
                        }
                    }
                }
                else
                {
                    resProjects.Add(project);
                }
            }
            return resProjects;
        }

        /// <summary>
        /// Returns the sharePoint version (14 or 15)
        /// </summary>
        /// <param name="parentAction"></param>
        /// <param name="currentProject"></param>
        /// <returns></returns>
        internal static string GetSharePointVersion(DTE dte)
        {
            return GetApplicationConfigValue(dte, "SharePointVersion", "15");
        }

        internal static string GetSharePointHive(DTE dte)
        {
            return GetSharePointVersion(dte);
        }

        /// <summary>
        /// Returns the current sharepoint license of the application
        /// </summary>
        /// <param name="dte"></param>
        /// <returns></returns>
        private static string GetSharePointLicense(DTE dte)
        {
            return GetApplicationConfigValue(dte, "SharePointLicense", "Foundation");
        }

        public static string GetVersionedFolder(DTE service)
        {
            string version = Helpers.GetApplicationConfigValue(service, "SharePointVersion", "15");
            if (version.Equals("15"))
            {
                return "/15";
            }
            else return "";
        }

        public static string GetApplicationConfigValue(DTE service, string ConfigValue, string DefaultValue)
        {
            try
            {
                XmlDocument doc = GetApplicationConfigFile(service, false);

                if (doc == null)
                {
                    return DefaultValue;
                }

                XmlNamespaceManager newnsmgr = new XmlNamespaceManager(doc.NameTable);
                newnsmgr.AddNamespace("ns", "http://schemas.microsoft.com/developer/msbuild/2003");

                XmlNode node = doc.SelectSingleNode("/ns:Project/ns:PropertyGroup/ns:" + ConfigValue, newnsmgr);
                if (node != null)
                {
                    return node.InnerText;
                }
            }
            catch { }
            if (DefaultValue != null)
            {
                return DefaultValue;
            }
            return "";
        }

        /// <summary>
        /// Takes the application.config and runs msbuild to get a given config value
        /// </summary>
        /// <param name="service"></param>
        /// <param name="ConfigValue"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public static string GetApplicationConfigValueEvaluated(DTE service, string ConfigValue, string DefaultValue, string basePath)
        {
            string msbuildPath = @"C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\MSBuild.exe"; // Deployment.msbuild /t:Deploy /nologo /v:d >> Deployment.log;
            string msbuildFilePath = basePath + "\\Tools\\GetConfigValueEvaluated.msbuild";

            string solutionDirectory = Path.GetDirectoryName((string)service.Solution.Properties.Item("Path").Value);
            string configFileName = Path.Combine(solutionDirectory, "Application.config");

            string arguments = msbuildFilePath + " /t:GetValue /nologo /p:ConfigFileName=\"" + configFileName + "\";ConfigValue=" + ConfigValue;

            ProcessStartInfo startInfo2 = new ProcessStartInfo(msbuildPath, arguments);
            startInfo2.CreateNoWindow = true;
            startInfo2.UseShellExecute = false;
            startInfo2.RedirectStandardOutput = true;
            System.Diagnostics.Process snProcess2 = System.Diagnostics.Process.Start(startInfo2);
            string output = snProcess2.StandardOutput.ReadToEnd();
            snProcess2.WaitForExit(10000);

            string finalResult = "";
            StringReader reader = new StringReader(output);
            string line = reader.ReadLine();
            bool startReading = false;
            while (line != null)
            {
                if (line.Contains("ENDSECTION"))
                {
                    break;
                }

                if (startReading)
                {
                    finalResult += line.Trim();
                }

                if (line.Contains("STARTSECTION"))
                {
                    startReading = true;
                }

                line = reader.ReadLine();
            }

            return finalResult;
        }

        public static void SetApplicationConfigValue(DTE dte, string _KeyName, string _KeyValue)
        {
            string solutionDirectory = Path.GetDirectoryName((string)dte.Solution.Properties.Item("Path").Value);
            string toPath = Path.Combine(solutionDirectory, "Application.config");

            XmlDocument doc = GetApplicationConfigFile(dte, true);

            XmlNamespaceManager newnsmgr = new XmlNamespaceManager(doc.NameTable);
            newnsmgr.AddNamespace("ns", "http://schemas.microsoft.com/developer/msbuild/2003");

            XmlNode node = doc.SelectSingleNode("/ns:Project/ns:PropertyGroup/ns:" + _KeyName, newnsmgr);
            if (node == null)
            {
                XmlNode rootnode = doc.SelectSingleNode("/ns:Project/ns:PropertyGroup", newnsmgr);
                node = doc.CreateElement(_KeyName);
                rootnode.AppendChild(node);
            }

            Helpers.EnsureCheckout(dte, toPath);

            node.InnerText = _KeyValue;
            doc.Save(toPath);
        }

        public static XmlDocument GetApplicationConfigFile(DTE dte, bool createIfNotExists)
        {
            //Path to the solution directory
            string solutionDirectory = Path.GetDirectoryName((string)dte.Solution.Properties.Item("Path").Value);
            string toPath = Path.Combine(solutionDirectory, "Application.config");

            XmlDocument doc = new XmlDocument();

            try
            {
                if (!File.Exists(toPath))
                {
                    if (!createIfNotExists)
                    {
                        return null;
                    }
                    Helpers.LogMessage(dte, dte, "ApplicationConfiguration.xml not found. Creating file");

                    string contents = "<Project ToolsVersion=\"3.5\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">" + Environment.NewLine;
                    contents += "\t<PropertyGroup>" + Environment.NewLine;
                    contents += "\t\t<Company>Company</Company>" + Environment.NewLine;
                    contents += "\t\t<ApplicationVersion>1.0.0.0</ApplicationVersion>" + Environment.NewLine;
                    contents += "\t\t<SharePointVersion>15</SharePointVersion>	" + Environment.NewLine;
                    contents += "\t\t<AutoCreateResxFiles>false</AutoCreateResxFiles>" + Environment.NewLine;
                    contents += "\t\t<AutoGenerateManifest>true</AutoGenerateManifest>" + Environment.NewLine;
                    contents += "\t\t<DebuggingWebApp>http://" + Environment.MachineName + "</DebuggingWebApp>" + Environment.NewLine;
                    contents += "\t\t<StartupUrl>http://"+Environment.MachineName+"</StartupUrl>" + Environment.NewLine;
                    contents += "<SPSFVersion>"+FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion+"</SPSFVersion>" + Environment.NewLine;
                    contents += "\t</PropertyGroup>	" + Environment.NewLine;
                    contents += "</Project>" + Environment.NewLine;

                    File.WriteAllText(toPath, contents);
                    doc.Load(toPath);

                    DteHelper.SelectSolution(dte);
                    dte.ItemOperations.AddExistingItem(toPath);
                    dte.ActiveWindow.Close(EnvDTE.vsSaveChanges.vsSaveChangesNo);
                }
                else
                {
                    doc.Load(toPath);
                }
            }
            catch (Exception)
            {
            }
            return doc;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SharePointLicenses">List of SharePoint licenses e.g. 'Foundation;Standard;Enterprise'</param>
        /// <returns>true, if the current license of the solution is in the selection</returns>
        internal static bool SolutionIsInLicense(DTE dte, string SharePointLicenses)
        {
            if (string.IsNullOrEmpty(SharePointLicenses))
            { //no limitation for a license
                return true;
            }

            if (dte != null)
            {
                string currentLicense = GetSharePointLicense(dte);
                if (SharePointLicenses.Contains(currentLicense))
                {
                    return true;
                }
            }
            return false;
        }

        internal static DTE GetDTEFromTarget(object target)
        {
            if (target is Project)
            {
                return (target as Project).DTE;
            }
            else if (target is ProjectItem)
            {
                return (target as ProjectItem).DTE;
            }
            else if (target is SolutionFolder)
            {
                return (target as SolutionFolder).DTE;
            }
            else if (target is Solution)
            {
                return (target as Solution).DTE;
            }
            return null;
        }

        public static ProjectItem GetFirstFeatureWithScope(DTE dte, Project project, string featureScope)
        {

            if (Helpers2.IsSharePointVSTemplate(dte, project))
            {
                //if value is empty try to find a sharepoint project where the siteurl is set

                //Trace.WriteLine("Searching for feature of type " + featureScope);

                ISharePointProjectService projectService = Helpers2.GetSharePointProjectService(dte);
                ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(project);

                foreach (ISharePointProjectFeature existingFeature in sharePointProject.Features)
                {
                    //Trace.WriteLine("Checking feature " + existingFeature.Name + " with scope " + existingFeature.Model.Scope.ToString());

                    if ((featureScope == "Site") && (existingFeature.Model.Scope == Microsoft.VisualStudio.SharePoint.Features.FeatureScope.Site))
                    {
                        //Trace.WriteLine("Found feature with scope site.");
                        return projectService.Convert<ISharePointProjectFeature, EnvDTE.ProjectItem>(existingFeature);
                    }
                    else if ((featureScope == "Web") && (existingFeature.Model.Scope == Microsoft.VisualStudio.SharePoint.Features.FeatureScope.Web))
                    {
                        //Trace.WriteLine("Found feature with scope web.");
                        return projectService.Convert<ISharePointProjectFeature, EnvDTE.ProjectItem>(existingFeature);
                    }
                    else if ((featureScope == "WebApplication") && (existingFeature.Model.Scope == Microsoft.VisualStudio.SharePoint.Features.FeatureScope.WebApplication))
                    {
                        //Trace.WriteLine("Found feature with scope WebApplication.");
                        return projectService.Convert<ISharePointProjectFeature, EnvDTE.ProjectItem>(existingFeature);
                    }
                    else if ((featureScope == "Farm") && (existingFeature.Model.Scope == Microsoft.VisualStudio.SharePoint.Features.FeatureScope.Farm))
                    {
                        //Trace.WriteLine("Found feature with scope Farm.");
                        return projectService.Convert<ISharePointProjectFeature, EnvDTE.ProjectItem>(existingFeature);
                    }
                }

                return null;
            }
            else
            {
                string sharePointVersion = Helpers.GetSharePointVersion(dte);

                try
                {
                    ProjectItem featuresFolder = Helpers.GetFeatureFolder(project, false);
                    foreach (ProjectItem feature in featuresFolder.ProjectItems)
                    {
                        try
                        {
                            ProjectItem featureXML = Helpers.GetProjectItemByName(feature.ProjectItems, "feature.xml");

                            string featurepath = Helpers.GetFullPathOfProjectItem(featureXML);

                            XmlDocument featuredoc = new XmlDocument();
                            featuredoc.Load(featurepath);

                            //what is the scope of the feature
                            XmlNamespaceManager featurensmgr = new XmlNamespaceManager(featuredoc.NameTable);
                            featurensmgr.AddNamespace("ns", "http://schemas.microsoft.com/sharepoint/");

                            XmlNode featurenode = featuredoc.SelectSingleNode("/ns:Feature", featurensmgr);
                            if (featurenode != null)
                            {
                                if (featurenode.Attributes["Scope"] != null)
                                {
                                    string scope = featurenode.Attributes["Scope"].Value;
                                    if (scope == featureScope)
                                    {
                                        return feature;
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            return null;
        }

        internal static bool SolutionHasVersion(DTE dte, string SharePointVersions)
        {
            if (string.IsNullOrEmpty(SharePointVersions))
            { //no limitation for a license
                return true;
            }

            if (dte != null)
            {
                string currentVersion = GetSharePointVersion(dte);
                if (SharePointVersions.Contains(currentVersion))
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetCommonProgramsFolder()
        {
            string commonProgramFolder = System.Environment.GetEnvironmentVariable("CommonProgramW6432");
            if (string.IsNullOrEmpty(commonProgramFolder))
            {
                commonProgramFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
            }
            if (commonProgramFolder.Contains(" (x86)"))
            {
                if (!Directory.Exists(commonProgramFolder))
                {
                    commonProgramFolder = commonProgramFolder.Replace(" (x86)", "");
                }
            }
            //if folder is Program Files(x86), but it does not exists, we return without (x86)check in some case
            return commonProgramFolder;
        }

        public static string GetProgramsFolder32()
        {
            string programFolder32 = System.Environment.GetEnvironmentVariable("ProgramW6432");
            if (string.IsNullOrEmpty(programFolder32))
            {
                programFolder32 = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            }
            if (!programFolder32.EndsWith(" (x86)"))
            {
                programFolder32 += " (x86)";
            }
            return programFolder32;
        }

        public static string GetProgramsFolder()
        {
            string programFolder = System.Environment.GetEnvironmentVariable("ProgramW6432");
            if (string.IsNullOrEmpty(programFolder))
            {
                programFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            }
            if (programFolder.Contains(" (x86)"))
            {
                if (!Directory.Exists(programFolder))
                {
                    programFolder = programFolder.Replace(" (x86)", "");
                }
            }
            return programFolder;
        }

        internal static string GetInstalledSharePointVersion()
        {
            //is teh Microsoft.SharePoint.dll available, check only for 14 or 15 is not enough because 
            //folder 14 is also on 15 installations available and the other way round when Office Developer Tools for VS2012 are installed
            string check15dll = GetCommonProgramsFolder() + "\\Microsoft Shared\\web server extensions\\15\\ISAPI\\Microsoft.SharePoint.dll";
            if (File.Exists(check15dll))
            {
                return "15";
            }
            return "14";
        }

        /// <summary>
        /// returns the path to the local sharepoint hive
        /// </summary>
        /// <returns></returns>
        public static string GetSharePointHive()
        {
            return GetCommonProgramsFolder() + "\\Microsoft Shared\\web server extensions\\" + GetInstalledSharePointVersion();
        }

        internal static void LogSettings(DTE dte, string action)
        {

            string content = "";


            content += "Action: " + action + Environment.NewLine;

            content += Environment.NewLine;
            content += "Globals in DTE" + Environment.NewLine;
            foreach (string s in (Array)dte.Globals.VariableNames)
            {
                try
                {
                    content += s + " = " + dte.Globals[s] + Environment.NewLine;
                }
                catch (Exception)
                {
                }
            }

            content += Environment.NewLine;
            content += "Solution setting: " + Environment.NewLine;

            for (int i = 0; i < dte.Solution.Properties.Count; i++)
            {
                try
                {
                    content += dte.Solution.Properties.Item(i).Name + " = " + dte.Solution.Properties.Item(i).Value.ToString() + Environment.NewLine;
                }
                catch (Exception)
                {
                }
            }

            content += Environment.NewLine;
            content += "Globals in Solution" + Environment.NewLine;
            foreach (string s in (Array)dte.Solution.Globals.VariableNames)
            {
                try
                {
                    content += s + " = " + dte.Solution.Globals[s] + Environment.NewLine;
                }
                catch (Exception)
                {
                }
            }

            foreach (Project project in dte.Solution.Projects)
            {
                content += Environment.NewLine;
                content += "Project setting: " + project.Name;
                for (int i = 0; i < project.Properties.Count; i++)
                {
                    try
                    {
                        content += project.Properties.Item(i).Name + "=" + project.Properties.Item(i).Value.ToString() + Environment.NewLine;
                    }
                    catch (Exception)
                    {
                    }
                }
                content += Environment.NewLine;
                content += "Globals im Project " + project.Name + Environment.NewLine;
                foreach (string s in (Array)project.Globals.VariableNames)
                {
                    try
                    {
                        content += s + " = " + project.Globals[s] + Environment.NewLine;
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            //File.WriteAllText("C:\\logsettings_" + Guid.NewGuid().ToString() + ".txt", content);
        }

        public static string GetDefaultIISWebApp()
        {
            using (DirectoryEntry iisApps = new DirectoryEntry(string.Format("IIS://{0}/W3SVC", "localhost")))
            {
                foreach (DirectoryEntry iisApp in iisApps.Children)
                {
                    string n = iisApp.Name;
                    string name = "";
                    string ports = "";

                    PropertyValueCollection pvc1 = iisApp.Properties["ServerComment"];
                    foreach (object value in pvc1)
                    {
                        name = value.ToString();
                    }

                    PropertyValueCollection pvc = iisApp.Properties["ServerBindings"];
                    foreach (object value in pvc)
                    {
                        // Format is IPAddress:Port:HostHeader
                        string[] Bits = value.ToString().Split(':');
                        string IPAddress = Bits[0];
                        string TCPIPPort = Bits[1];
                        string HostHeader = Bits[2];


                        ports = String.Format("IP = {0}, Port = {1}, Header = {2}",
                        (IPAddress.Length == 0) ? "(All Unassigned)" : IPAddress,
                        TCPIPPort,
                        (HostHeader.Length == 0) ? "(No Host Header)" : HostHeader);
                    }

                    string a = name + "(" + ports + ")";
                }
            }

            /*
            DirectoryEntry entry = new DirectoryEntry("IIS://localhost/w3svc/1");
            PropertyValueCollection pvc = entry.Properties["ServerBindings"];
            foreach (object value in pvc)
            {
              // Format is IPAddress:Port:HostHeader
              string[] Bits = value.ToString().Split(':');
              string IPAddress = Bits[0];
              string TCPIPPort = Bits[1];
              string HostHeader = Bits[2];


              string s = String.Format("IP = {0}, Port = {1}, Header = {2}",
              (IPAddress.Length == 0) ? "(All Unassigned)" : IPAddress,
              TCPIPPort,
              (HostHeader.Length == 0) ? "(No Host Header)" : HostHeader);
            }
             * */

            return "http://" + Environment.MachineName.ToLower();
            //return "localhost";
        }

        internal static string GetDebuggingWebApp(DTE dte, Project project, string basePath)
        {
            try
            {
                ISharePointProjectService projectService = Helpers2.GetSharePointProjectService(dte);
                ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(project);
                if (sharePointProject.SiteUrl != null)
                {
                    return sharePointProject.SiteUrl.ToString();
                }
            }
            catch { }

            return GetDebuggingWebApp(dte, basePath);
        }

        internal static string GetDebuggingWebApp(DTE dte, string basePath)
        {
            string value = GetApplicationConfigValueEvaluated(dte, "DebuggingWebApp", "", basePath);
            if (value == "")
            {
                value = GetApplicationConfigValue(dte, "DebuggingWebApp", "");
            }

            //if value is empty try to find a sharepoint project where the siteurl is set
            if (string.IsNullOrEmpty(value))
            {
                try
                {
                    ISharePointProjectService projectService = Helpers2.GetSharePointProjectService(dte);
                    foreach (ISharePointProject sharePointProject in projectService.Projects)
                    {
                        try
                        {
                            if (sharePointProject.SiteUrl != null)
                            {
                                return sharePointProject.SiteUrl.ToString();
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                catch { }
            }

            return value;
        }

        public static void DebuggerAttachToW3WP(DTE dte, string basePath)
        {
            try
            {
                //TODO: ist ein Projekt ausgewÃ¤hlt? wenn nicht, nimm das startup project
                //Muss das Projekt noch gebaut und deployed werden?
                //appool recyclen? owstimer restarten?
                Project currentProject = Helpers.GetSelectedProject(dte);
                if (currentProject == null)
                {
                    Helpers.LogMessage(dte, dte, "No Project selected, starting the StartupProject");

                    SolutionBuild2 sb = (SolutionBuild2)dte.Solution.SolutionBuild;
                    string msg = "";

                    foreach (String item in (Array)sb.StartupProjects)
                    {
                        msg += item;
                    }
                    currentProject = dte.Solution.Item(msg);
                }

                if (currentProject == null)
                {
                    return;
                }
                //1. Get DefaultDebuggingWebApp
                string debuggingWebApp = GetDebuggingWebApp(dte, basePath);

                if (debuggingWebApp == "")
                {
                    Helpers.LogMessage(dte, dte, "Warning: Could not find parameter 'DebuggingWebApp' in file Application.config");
                    return;
                }

                //DeploymentForm dform = new DeploymentForm();
                //DialogResult userResult = dform.ShowDialog();
                DialogResult userResult = MessageBox.Show("Do you need a full redeployment before debugging (click No to run a quick deployment)?", "Full Redeploy needed?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (userResult != DialogResult.Cancel)
                {
                    Helpers.LogMessage(dte, dte, "Building project " + currentProject.Name);
                    dte.Solution.SolutionBuild.BuildProject(currentProject.ConfigurationManager.ActiveConfiguration.ConfigurationName, currentProject.UniqueName, true);
                    if (dte.Solution.SolutionBuild.LastBuildInfo != 0)
                    {
                        //no successful build, cancel deployment
                        Helpers.LogMessage(dte, dte, "No successfull build. Cancelling...");
                        return;
                    }

                    //ok, not cancelled
                    if (userResult == DialogResult.No)
                    {
                        //quickdeploy
                        DeploymentHelpers.QuickDeploy(dte, currentProject);

                        DeploymentHelpers.QuickDeployGAC(dte, currentProject);

                    }
                    else
                    {
                        //full deploy
                        Helpers.LogMessage(dte, dte, "Starting project " + currentProject.Name);
                        DeploymentHelpers.RedeployProject(dte, currentProject);
                    }
                }
                else
                {
                    return;
                }

                //2. Get ApplicationPool of webapp
                string appPoolOfWebApp = GetAppPoolOfWebApp(dte, debuggingWebApp);


                if (appPoolOfWebApp == "")
                {
                    Helpers.LogMessage(dte, dte, "Warning: Could not retrieve web application in IIS");
                    return;
                }

                Helpers.RecycleAppPool(dte, appPoolOfWebApp);

                //wait 5 seconds that recycling may start
                System.Threading.Thread.Sleep(2000);

                for (int i = 0; i < 20; i++)
                {
                    Helpers.LogMessage(dte, dte, "Waiting for recycle of application pool " + appPoolOfWebApp);
                    if (Helpers.IsAppPoolRunning(dte, appPoolOfWebApp))
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(2000);
                }


                for (int i = 0; i < 20; i++)
                {
                    Helpers.LogMessage(dte, dte, "Waiting for restart of SharePoint...");
                    if (Helpers.PingServer(dte, debuggingWebApp))
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(2000);
                }

                string processIdOfWebApp = Helpers.GetW3WPProcessIdForApplicationPool(dte, appPoolOfWebApp);
                for (int i = 0; i < 20; i++)
                {
                    Helpers.LogMessage(dte, dte, "Waiting for AppPool restarted...");
                    processIdOfWebApp = Helpers.GetW3WPProcessIdForApplicationPool(dte, appPoolOfWebApp);
                    if (processIdOfWebApp != "")
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(2000);
                }

                Helpers.LogMessage(dte, dte, "ProcessId for " + appPoolOfWebApp + " is " + processIdOfWebApp);


                //3. Get W3WP Process of ApplicationPool
                if (processIdOfWebApp == "")
                {
                    Helpers.LogMessage(dte, dte, "Warning: w3wp process not found for application pool " + appPoolOfWebApp);
                    return;
                }

                EnvDTE.Processes processes = dte.Debugger.LocalProcesses;
                foreach (EnvDTE.Process proc in processes)
                {
                    if (proc.ProcessID.ToString() == processIdOfWebApp)
                    {
                        Helpers.LogMessage(dte, dte, "Successfully found process in VS with process id '" + processIdOfWebApp + "', ready for attaching to process");
                        Trace.TraceInformation("Attaching OK");
                        proc.Attach();
                    }
                }

                Helpers.OpenWebPage(dte, debuggingWebApp);
            }
            catch (Exception ex)
            {
                Helpers.LogMessage(dte, dte, ex.Message);
            }
        }

        internal static string GetAppPoolOfWebApp(DTE dte, string debuggingWebApp)
        {
            string appPoolOfWebApp = "";
            if (debuggingWebApp.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
            {
                debuggingWebApp = debuggingWebApp.Substring(7);
            }
            if (debuggingWebApp.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
            {
                debuggingWebApp = debuggingWebApp.Substring(8);
            }
            if (debuggingWebApp.IndexOf("/") > 0)
            {
                debuggingWebApp = debuggingWebApp.Substring(0, debuggingWebApp.IndexOf("/"));
            }
            if (debuggingWebApp.EndsWith("/"))
            {
                debuggingWebApp = debuggingWebApp.Substring(0, debuggingWebApp.Length - 1);
            }


            string[] Bits = debuggingWebApp.ToString().Split(':', '/');

            string debuggingWebAppHost = Bits[0];
            string debuggingWebAppPort = "80";
            if (Bits.Length > 1)
            {
                debuggingWebAppPort = Bits[1];
            }

            if (debuggingWebAppHost.ToLower() == "localhost")
            {
                debuggingWebAppHost = "";
            }
            if (debuggingWebAppHost.ToLower() == Environment.MachineName.ToLower())
            {
                debuggingWebAppHost = "";
            }

            Helpers.LogMessage(dte, dte, "Retrieving webapplication for " + debuggingWebApp + " (" + debuggingWebAppHost + ":" + debuggingWebAppPort + ")");

            using (DirectoryEntry appPoolsEntry = new DirectoryEntry(string.Format("IIS://{0}/W3SVC", "localhost")))
            {
                foreach (DirectoryEntry entry in appPoolsEntry.Children)
                {
                    if (entry.SchemaClassName == "IIsWebServer")
                    {
                        try
                        {
                            if (entry.Properties["ServerBindings"] != null)
                            {
                                string serverBindings = entry.Properties["ServerBindings"].Value.ToString();
                                string[] serverBindingsBits = serverBindings.ToString().Split(':');
                                if (serverBindingsBits.Length >= 3)
                                {

                                    string IPAddress = serverBindingsBits[0];
                                    string TCPIPPort = serverBindingsBits[1];
                                    string HostHeader = serverBindingsBits[2];

                                    if (IPAddress == debuggingWebAppHost)
                                    {
                                        if (TCPIPPort == debuggingWebAppPort)
                                        {
                                            string props = "";
                                            foreach (string s in entry.Properties.PropertyNames)
                                            {
                                                try
                                                {
                                                    props += entry.Properties[s].PropertyName + "=" + entry.Properties[s].Value + Environment.NewLine;
                                                }
                                                catch (Exception)
                                                {
                                                }
                                            }

                                            appPoolOfWebApp = entry.Properties["AppPoolId"].Value.ToString();
                                            if (appPoolOfWebApp == "DefaultAppPool")
                                            {
                                                //Ok, default app pool, we hope that in ServerComment is the real app pool name
                                                appPoolOfWebApp = entry.Properties["ServerComment"].Value.ToString();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch { }
                    }
                }
            }

            return appPoolOfWebApp;
        }

        private static bool PingServer(DTE dte, string deploymentWebapp)
        {
            return PingServer(dte, deploymentWebapp, 2000);
        }

        private static bool PingServer(DTE dte, string deploymentWebapp, int timeOutMilliSeconds)
        {
            try
            {
                //open the webpage
                Helpers.LogMessage(dte, dte, "Requesting url '" + deploymentWebapp + "' to force restart of w3wp.exe...");
                WebRequest webRequest = WebRequest.Create(deploymentWebapp);
                webRequest.Timeout = timeOutMilliSeconds;
                webRequest.Credentials = CredentialCache.DefaultCredentials;
                WebResponse response = webRequest.GetResponse();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                Helpers.LogMessage(dte, dte, "Requesting url '" + deploymentWebapp + "' returned " + ex.Message);
            }
            return false;
        }

        private static string GetW3WPProcessIdForApplicationPool(DTE dte, string appPoolOfWebApp)
        {
            Helpers.LogMessage(dte, dte, "Searching for w3wp process with application pool '" + appPoolOfWebApp + "'");
            string processIdOfWebApp = "";
            System.Management.ManagementClass MgmtClass = new System.Management.ManagementClass("Win32_Process");
            foreach (System.Management.ManagementObject mo in MgmtClass.GetInstances())
            {
                try
                {
                    if (mo["Name"].ToString() == "w3wp.exe")
                    {
                        string apppool = mo["CommandLine"].ToString();
                        apppool = apppool.Substring(apppool.IndexOf("-ap") + 5);
                        apppool = apppool.Substring(0, apppool.IndexOf("\""));
                        if (apppool == appPoolOfWebApp)
                        {
                            processIdOfWebApp = mo["ProcessId"].ToString();
                            Helpers.LogMessage(dte, dte, "Successfully identified w3wp process, using process id  '" + processIdOfWebApp + "'");
                            return processIdOfWebApp;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            return "";
        }

        internal static void SelectProject(Project project)
        {
            UIHierarchy tree = project.DTE.Windows.Item(EnvDTE.Constants.vsWindowKindSolutionExplorer).Object as UIHierarchy;
            SelectProject(tree, project);
            //tree.DoDefaultAction();
        }

        public static void SelectProject(UIHierarchy tree, Project CoreRecipes)
        {
            foreach (UIHierarchyItem subnode in tree.UIHierarchyItems)
            {
                SelectProject(subnode, CoreRecipes);
            }
        }

        public static void SelectProject(UIHierarchyItem node, Project CoreRecipes)
        {
            foreach (UIHierarchyItem subnode in node.UIHierarchyItems)
            {
                if (subnode.Object is Project)
                {
                    Project p = subnode.Object as Project;
                    if (p.Name == CoreRecipes.Name)
                    {
                        subnode.Select(vsUISelectionType.vsUISelectionTypeSelect);
                    }
                }
                if (subnode.Object is ProjectItem)
                {
                    ProjectItem p = subnode.Object as ProjectItem;
                    if (p.SubProject != null)
                    {
                        if (p.SubProject.Name == CoreRecipes.Name)
                        {
                            subnode.Select(vsUISelectionType.vsUISelectionTypeSelect);
                        }
                    }
                }
                SelectProject(subnode, CoreRecipes);
            }
        }


        internal static void OpenWebPage(DTE dte, string url)
        {
            Helpers.LogMessage(dte, dte, "Opening web page " + url);
            System.Diagnostics.Process snProcess = new System.Diagnostics.Process();
            snProcess.StartInfo.FileName = "\"" + url + "\"";
            snProcess.StartInfo.Arguments = ""; // "\"" + url + "\"";
            snProcess.StartInfo.CreateNoWindow = false;
            snProcess.StartInfo.UseShellExecute = true;
            snProcess.Start();
        }

        internal static string GetBasePath()
        {
            string gaxBaseDir = "";
            try
            {
                string keyname="SPALM.SPSF," + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
                RegistryKey gaxKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\11.0\ExtensionManager\EnabledExtensions", false);
                if (gaxKey != null)
                {

                    var key = gaxKey.GetValue(keyname);
                    if (key !=null)
                    {
                        if (Directory.Exists(key.ToString()))
                        {
                            return key.ToString();
                        }
                    }
                }

                gaxKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\11.0_Config\Menus\", false);
                object firstTemplate = gaxKey.GetValue("SPALM.SPSF");
                //return something like C:\SPALM\SharePointSoftwareFactory.Base\SharePointSoftwareFactory.Base\bin\Debug\SharePointSoftwareFactory.BaseUI.dll,1000,1

                string gaxDll = firstTemplate.ToString().Substring(0, firstTemplate.ToString().IndexOf(","));
                gaxBaseDir = Directory.GetParent(gaxDll).FullName;
            }
            catch (Exception)
            {
            }
            finally{
                Helpers.Log("GaxBaseDir: "+gaxBaseDir);
            }

            if (!Directory.Exists(gaxBaseDir))
            {
                gaxBaseDir = AssemblyLoadDirectory;
            }
            
#if DEBUG
            Helpers.Log("GaxBaseDir (AssemblyDir): "+gaxBaseDir);
#endif

            return gaxBaseDir;
        }

        static public string AssemblyLoadDirectory
        {
            get
            {
                string codeBase = Assembly.GetCallingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        } 


        internal static void Log(string p)
        {
//#if DEBUG
//            Debug.WriteLine(p);
//            string path = @"c:\";
//            string logfile = Path.Combine(path, "spsf.log");
//            File.AppendAllText(Path.Combine(path, "spsf.log"), "[" + DateTime.Now.ToLongTimeString() + "] " + p + Environment.NewLine);
//#endif
        }

        internal static void AttachToUserCodeHost(DTE dte)
        {
            Project currentProject = Helpers.GetSelectedProject(dte);
            if (currentProject == null)
            {
                Helpers.LogMessage(dte, dte, "No Project selected");
                return;
            }
            Helpers.AttachToUserCodeHost(currentProject.DTE, currentProject);

        }

        internal static void AttachToUserCodeHost(DTE dte, Project project)
        {
            //SPUCWorkerProcess.exe
            //find the id of the SPUCWorkerProcess.exe process

            string processIdOfWebApp = "";
            Helpers.LogMessage(dte, dte, "Searching for SPUCWorkerProcess.exe process (sandboxed solution)");
            System.Management.ManagementClass MgmtClass = new System.Management.ManagementClass("Win32_Process");
            foreach (System.Management.ManagementObject mo in MgmtClass.GetInstances())
            {
                try
                {
                    if (mo["Name"].ToString() == "SPUCWorkerProcess.exe")
                    {
                        processIdOfWebApp = mo["ProcessId"].ToString();
                        Helpers.LogMessage(dte, dte, "Successfully identified SPUCWorkerProcess.exe process, using process id  '" + processIdOfWebApp + "'");
                        break;
                    }
                }
                catch (Exception)
                {
                }
            }

            AttachToVSProcess(dte, processIdOfWebApp);
        }

        internal static void StartNewInstance(DTE dte)
        {
            //1. get selected project
            Project currentProject = Helpers.GetSelectedProject(dte);
            if (currentProject == null)
            {
                Helpers.LogMessage(dte, dte, "No Project selected, starting the StartupProject");
                SolutionBuild2 sb = (SolutionBuild2)dte.Solution.SolutionBuild;
                string msg = "";
                foreach (String item in (Array)sb.StartupProjects)
                {
                    msg += item;
                }
                currentProject = dte.Solution.Item(msg);
            }
            if (currentProject == null)
            {
                return;
            }

            //2. ask the user if he wants quick deploy or full deployment
            DeploymentForm deployform = new DeploymentForm();
            DialogResult userResult = deployform.ShowDialog(); // MessageBox.Show("Do you need a full redeployment before debugging (click No to run a quick deployment)?", "Full Redeploy needed?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (userResult != DialogResult.Cancel)
            {
                if (userResult == DialogResult.Ignore)
                {
                    //no deployment, directly start debugging
                    string debuggingWebApp = DeploymentHelpers.GetSiteUrl(currentProject.DTE, currentProject);
                    string appPoolOfWebApp = GetAppPoolOfWebApp(currentProject.DTE, debuggingWebApp);
                    string startupUrl = DeploymentHelpers.GetStartupUrl(currentProject.DTE, currentProject);
                    Helpers.AttachProjectToW3WP(currentProject.DTE, currentProject, appPoolOfWebApp);
                    Helpers.OpenWebPage(currentProject.DTE, startupUrl);
                }
                else
                {
                    Helpers.LogMessage(dte, dte, "Building project " + currentProject.Name);
                    dte.Solution.SolutionBuild.BuildProject(currentProject.ConfigurationManager.ActiveConfiguration.ConfigurationName, currentProject.UniqueName, true);
                    if (dte.Solution.SolutionBuild.LastBuildInfo != 0)
                    {
                        //no successful build, cancel deployment
                        Helpers.LogMessage(dte, dte, "No successfull build. Cancelling...");
                        return;
                    }

                    //ok, not cancelled
                    if (userResult == DialogResult.No)
                    {
                        StartNewProcessQuickDeploy(dte, currentProject);

                    }
                    else
                    {
                        StartNewProcessFullRedeploy(dte, currentProject);
                    }
                }
            }
            else
            {
                return;
            }
        }

        private static BackgroundWorker startNewProcessQuickDeployWorker;
        internal static void StartNewProcessQuickDeploy(DTE dte, Project project)
        {
            startNewProcessQuickDeployWorker = new BackgroundWorker();
            startNewProcessQuickDeployWorker.DoWork += new DoWorkEventHandler(startNewProcessQuickDeployWorker_DoWork);
            startNewProcessQuickDeployWorker.RunWorkerAsync(project);
        }

        static void startNewProcessQuickDeployWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument is Project)
            {
                Project project = e.Argument as Project;

                string debuggingWebApp = DeploymentHelpers.GetSiteUrl(project.DTE, project);
                string appPoolOfWebApp = GetAppPoolOfWebApp(project.DTE, debuggingWebApp);
                string startupUrl = DeploymentHelpers.GetStartupUrl(project.DTE, project);

                DeploymentHelpers.QuickDeploy(project.DTE, project);
                DeploymentHelpers.QuickDeployGAC(project.DTE, project);
                Helpers.RecycleAppPool(project.DTE, appPoolOfWebApp);

                Helpers.WaitForAppPool(project.DTE, appPoolOfWebApp);
                Helpers.RefreshServer(project.DTE, debuggingWebApp);
                Helpers.AttachProjectToW3WP(project.DTE, project, appPoolOfWebApp);
                Helpers.OpenWebPage(project.DTE, startupUrl);
            }
        }

        private static BackgroundWorker startNewProcessFullDeployWorker;
        internal static void StartNewProcessFullRedeploy(DTE dte, Project project)
        {
            startNewProcessFullDeployWorker = new BackgroundWorker();
            startNewProcessFullDeployWorker.DoWork += new DoWorkEventHandler(startNewProcessFullDeployWorker_DoWork);
            startNewProcessFullDeployWorker.RunWorkerAsync(project);
        }

        static void startNewProcessFullDeployWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument is Project)
            {
                Project project = e.Argument as Project;

                string debuggingWebApp = DeploymentHelpers.GetSiteUrl(project.DTE, project);
                string appPoolOfWebApp = GetAppPoolOfWebApp(project.DTE, debuggingWebApp);
                string startupUrl = DeploymentHelpers.GetStartupUrl(project.DTE, project);

                List<SharePointDeploymentJob> jobs = new List<SharePointDeploymentJob>();
                SharePointDeploymentJob job = new SharePointDeploymentJob();
                job.WSPName = DeploymentHelpers.GetWSPName(project.DTE, project);
                job.WSPFilename = DeploymentHelpers.GetWSPFilePath(project.DTE, project);
                job.TargetSiteUrl = debuggingWebApp;
                jobs.Add(job);
                new SharePointBrigdeHelper(project.DTE).PerformDeploymentOperation("redeploy", jobs);


                Helpers.RefreshServer(project.DTE, debuggingWebApp);
                Helpers.AttachProjectToW3WP(project.DTE, project, appPoolOfWebApp);
                Helpers.OpenWebPage(project.DTE, startupUrl);
            }
        }

        private static void AttachProjectToW3WP(DTE dte, Project project, string appPoolOfWebApp)
        {
            string processIdOfWebApp = "";
            if (Helpers.GetIsSandboxedSolution(project))
            {
                AttachToUserCodeHost(dte, project);
            }
            else
            {
                processIdOfWebApp = Helpers.GetW3WPProcessIdForApplicationPool(dte, appPoolOfWebApp);
                for (int i = 0; i < 20; i++)
                {
                    Helpers.LogMessage(dte, dte, "Waiting for AppPool restarted...");
                    processIdOfWebApp = Helpers.GetW3WPProcessIdForApplicationPool(dte, appPoolOfWebApp);
                    if (processIdOfWebApp != "")
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(2000);
                }

                Helpers.LogMessage(dte, dte, "ProcessId for " + appPoolOfWebApp + " is " + processIdOfWebApp);
            }

            AttachToVSProcess(dte, processIdOfWebApp);
        }

        private static void AttachToVSProcess(DTE dte, string processIdOfWebApp)
        {
            //3. Get W3WP Process of ApplicationPool
            if (processIdOfWebApp == "")
            {
                Helpers.LogMessage(dte, dte, "Warning: Process not found");
                return;
            }

            EnvDTE.Processes processes = dte.Debugger.LocalProcesses;
            foreach (EnvDTE.Process proc in processes)
            {
                if (proc.ProcessID.ToString() == processIdOfWebApp)
                {
                    Helpers.LogMessage(dte, dte, "Successfully found process in VS with process id '" + processIdOfWebApp + "', ready for attaching to process");
                    Trace.TraceInformation("Attaching OK");
                    proc.Attach();
                }
            }
        }

        private static void WaitForAppPool(DTE dte, string appPool)
        {
            for (int i = 0; i < 20; i++)
            {
                Helpers.LogMessage(dte, dte, "Waiting for recycle of application pool " + appPool);
                if (Helpers.IsAppPoolRunning(dte, appPool))
                {
                    break;
                }
                System.Threading.Thread.Sleep(2000);
            }

        }

        private static void RefreshServer(DTE dte, string url)
        {
            for (int i = 0; i < 10; i++)
            {
                Helpers.LogMessage(dte, dte, "Waiting for restart of SharePoint...");
                if (Helpers.PingServer(dte, url, 20000))
                {
                    break;
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        internal static void ExcludeItemFromSCC(DTE service, Project project, ProjectItem projectItem)
        {
            string itemname = Helpers.GetFullPathOfProjectItem(projectItem);
            if (service.SourceControl.IsItemUnderSCC(itemname))
            {
                //exclude from source control
                service.SourceControl.ExcludeItem(project.FullName, itemname);


                //remove write protection from the file and all child files
                File.SetAttributes(itemname, FileAttributes.Normal);
                Helpers.LogMessage(service, service, "Removed write protection from " + itemname);
                foreach (ProjectItem childItem in projectItem.ProjectItems)
                {
                    string childFilename = Helpers.GetFullPathOfProjectItem(childItem);
                    File.SetAttributes(childFilename, FileAttributes.Normal);
                    Helpers.LogMessage(service, service, "Removed write protection from " + childFilename);
                }
            }
        }

        internal static bool IsValidNamespace(string value)
        {
            bool nextMustBeStartChar = true;
            if (value.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                switch (char.GetUnicodeCategory(c))
                {
                    case UnicodeCategory.UppercaseLetter:
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.TitlecaseLetter:
                    case UnicodeCategory.ModifierLetter:
                    case UnicodeCategory.OtherLetter:
                    case UnicodeCategory.LetterNumber:
                        {
                            nextMustBeStartChar = false;
                            continue;
                        }
                    case UnicodeCategory.NonSpacingMark:
                    case UnicodeCategory.SpacingCombiningMark:
                    case UnicodeCategory.DecimalDigitNumber:
                    case UnicodeCategory.ConnectorPunctuation:
                        if (!nextMustBeStartChar || (c == '_'))
                        {
                            break;
                        }
                        return false;

                    default:
                        goto Label_008C;
                }
                nextMustBeStartChar = false;
                continue;
            Label_008C:
                if (!IsSpecialTypeChar(c, ref nextMustBeStartChar))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsSpecialTypeChar(char ch, ref bool nextMustBeStartChar)
        {
            switch (ch)
            {
                case '[':
                case ']':
                case '$':
                case '&':
                case '*':
                case '+':
                case ',':
                case '-':
                case '.':
                case ':':
                case '<':
                case '>':
                    nextMustBeStartChar = true;
                    return true;

                case '`':
                    return true;
            }
            return false;
        }

    }

    public class CustomResXResourceReader
    {
        private string fileName = "";
        private string fileKey = "";
        private XmlDocument resdoc = null;

        public CustomResXResourceReader(string fileName)
        {
            this.fileName = fileName;
            if (fileName.ToUpper().Contains(@"12\TEMPLATE\FEATURES\"))
            {
                //file comes as FEATURES\SlideLibrary\Resources\resources.resx
                //we make SlideLibrary.resx from it
                string featureName = Directory.GetParent(fileName).Parent.Name;
                fileKey = Path.GetFileNameWithoutExtension(fileName);
                fileKey = fileKey.Replace("Resources", featureName);
                fileKey = fileKey.Replace("resources", featureName);
            }
            else
            {
                fileKey = Path.GetFileNameWithoutExtension(fileName);
            }
        }

        public string GetKey()
        {
            return fileKey;
        }

        public string GetValue(string key)
        {
            if (resdoc == null)
            {
                resdoc = new XmlDocument();
                if (File.Exists(fileName))
                {
                    resdoc.Load(fileName);
                }
            }
            XmlNode resnode = resdoc.SelectSingleNode("//root/data[@name='" + key + "']/value");
            if (resnode == null)
            {
                resnode = resdoc.SelectSingleNode("//root/Data[@Name='" + key + "']/Value");
            }
            if (resnode == null)
            {
                resnode = resdoc.SelectSingleNode("//root/Data[@name='" + key + "']/Value");
            }
            if (resnode != null)
            {
                return resnode.InnerText;
            }
            return "";
        }
    }
}