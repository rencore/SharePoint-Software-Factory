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

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class ExecuteMSBuildTarget : ConfigurableAction
    {
        private string _BuildTarget = "";

        [Input(Required = true)]
        public string BuildTarget
        {
            get { return _BuildTarget; }
            set { _BuildTarget = value; }
        }

        [Input(Required = true)]
        public ProjectItem TargetsFile
        {
            get { return _TargetsFile; }
            set { _TargetsFile = value; }
        }
        private ProjectItem _TargetsFile;

        private int GetOSArchitecture()
        {
            string pa = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            return ((String.IsNullOrEmpty(pa) || String.Compare(pa, 0, "x86", 0, 3, true) == 0) ? 32 : 64);
        }

        private DTE dte;

        public override void Execute()
        {
            dte = GetService<DTE>(true);

            string msbuildExe = @"C:\Windows\Microsoft.NET\Framework\v2.0.50727\MSBuild.exe";

            if (IntPtr.Size == 4)
            {
                if (GetOSArchitecture() == 64)
                {
                    //use 64bit version
                    msbuildExe = @"C:\Windows\Microsoft.NET\Framework64\v2.0.50727\MSBuild.exe";
                }
            }

            string targetsFilePath = "";
            if(_TargetsFile != null)
            {
                targetsFilePath = Helpers.GetFullPathOfProjectItem(_TargetsFile);  //_TargetsFile.Properties.Item("FullPath").Value.ToString();
            }

            if (!File.Exists(msbuildExe))
            {
                Helpers.LogMessage(dte, this, "MSBuild.exe not found at location " + msbuildExe);
                return;
            }

            System.Diagnostics.Process snProcess = new System.Diagnostics.Process();

            Helpers.LogMessage(dte, this, "Running " + "\"" + msbuildExe + "\"" + targetsFilePath + " /nologo /verbosity:normal /target:" + BuildTarget);
                
            snProcess.StartInfo.FileName = "\"" + msbuildExe + "\"";
            snProcess.StartInfo.Arguments = targetsFilePath + " /nologo /verbosity:d /target:" + BuildTarget;
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
            if (dte != null)
            {
                Helpers.LogMessage(dte, this, e.Data);
            }
        }

        void snProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (dte != null)
            {
                Helpers.LogMessage(dte, this, e.Data);
            }
        }

        public void RunSkript()
        {
        }

        /// <summary>
        /// Removes the previously added reference, if it was created
        /// </summary>
        public override void Undo()
        {
        }
    }

    public class MSBuildRUnner
    {
        string command = "";
        DTE dte = null;

        public MSBuildRUnner(string _command, DTE _dte)
        {
            command = _command;
            dte = _dte;
        }

        public void Run()
        {
            Window win = dte.Windows.Item(EnvDTE.Constants.vsWindowKindCommandWindow);
            CommandWindow comwin = (CommandWindow)win.Object;
            comwin.SendInput(command, true);
        }
    }
}