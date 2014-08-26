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
using System.ComponentModel;
using System.Drawing;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    [ServiceDependency(typeof(DTE))]
    public class RunCommand : ConfigurableAction
    {
        private string _Command = "";
        private string _Arguments = "";
        private string _WorkingDirectory = "";
        private bool _CreateNoWindow = true;
        private bool _WaitForExit = false;

        [Input(Required = true)]
        public bool WaitForExit
        {
            get { return _WaitForExit; }
            set { _WaitForExit = value; }
        }

        [Input(Required = true)]
        public string Command
        {
            get { return _Command; }
            set { _Command = value; }
        }

        [Input(Required = false)]
        public string WorkingDirectory
        {
            get { return _WorkingDirectory; }
            set { _WorkingDirectory = value; }
        }

        [Input(Required = false)]
        public string Arguments
        {
            get { return _Arguments; }
            set { _Arguments = value; }
        }

        [Input(Required = false)]
        public bool CreateNoWindow
        {
            get { return _CreateNoWindow; }
            set { _CreateNoWindow = value; }
        }

        BackgroundWorker processWorker = null;

        public override void Execute()
        {
            string commandPart = _Command;
            string arguments = "";

            if (!string.IsNullOrEmpty(_Arguments))
            {
                commandPart = _Command;
                arguments = _Arguments;
            }
            else
            {
                try
                {
                    if (_Command.Contains(" "))
                    {
                        if (_Command.StartsWith("\""))
                        {
                            //e.g. for "C:\\Program Files..." -o operation...
                            int closedquot = _Command.IndexOf('"', 1) + 1;
                            commandPart = _Command.Substring(0, closedquot).Trim();
                            arguments = _Command.Substring(closedquot).Trim();
                        }
                        else
                        {
                            commandPart = _Command.Substring(0, _Command.IndexOf(" "));
                            arguments = _Command.Substring(_Command.IndexOf(" "));
                        }
                    }
                }
                catch
                { }
            }

            if (WaitForExit)
            {
                Helpers.RunProcess(GetService<DTE>(true), commandPart, arguments, true, false);
            }
            else
            {

                RunCommandCommand command = new RunCommandCommand();
                command.dte = GetService<DTE>(true);
                command.Command = commandPart;
                command.Arguments = arguments;
                command.CreateNoWindow = CreateNoWindow;
                command.WorkingDirectory = WorkingDirectory;

                processWorker = new BackgroundWorker();
                processWorker.DoWork += new DoWorkEventHandler(processWorker_DoWork);
                processWorker.RunWorkerAsync(command);
            }
        }

        void processWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument is RunCommandCommand)
            {
                RunCommandCommand command = e.Argument as RunCommandCommand;
                try
                {
                    Helpers.ShowProgress(command.dte, "Starting...", 10);
                    //Helpers.LogMessage(command.dte, command.dte, "Running command " + command.Command + " " + command.Arguments);

                    if (command.CreateNoWindow)
                    {
                        Helpers.RunProcess(command.dte, command.Command, command.Arguments, true, false);

                        /*
                        ProcessStartInfo psi = new ProcessStartInfo();
                        psi.FileName = command.Command;
                        psi.Arguments = command.Arguments;
                        psi.WorkingDirectory = command.WorkingDirectory;
                        psi.CreateNoWindow = true;
                        psi.UseShellExecute = false;
                        psi.RedirectStandardInput = true;
                        psi.RedirectStandardOutput = true;
                        psi.RedirectStandardError = true;

                        // Create the process.
                        System.Diagnostics.Process p = new System.Diagnostics.Process();

                        // Associate process info with the process.
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
                                Helpers.LogMessage(command.dte, command.dte, text);
                                if (counter < 100)
                                {
                                    counter += 10;
                                }
                                Helpers.ShowProgress(command.dte, "Running...", counter);
                            }
                            System.Threading.Thread.Sleep(100);
                        }

                        Helpers.LogMessage(command.dte, command.dte, p.StandardOutput.ReadToEnd());

                        if (p.ExitCode != 0)
                        {
                            Helpers.LogMessage(command.dte, command.dte, "Failure");
                        }
                        else
                        {
                            Helpers.LogMessage(command.dte, command.dte, "Finished successfully");
                        }

                        Helpers.ShowProgress(command.dte, "Finished...", 100);
                         * */
                    }
                    else
                    {
                        //new window
                        ProcessStartInfo psi = new ProcessStartInfo();
                        psi.FileName = command.Command;
                        psi.Arguments = command.Arguments;
                        psi.WorkingDirectory = command.WorkingDirectory;
                        psi.CreateNoWindow = false;
                        psi.UseShellExecute = false;
                        System.Diagnostics.Process p = new System.Diagnostics.Process();
                        p.StartInfo = psi;
                        bool fStarted = p.Start();
                    }
                }
                catch (Exception ex)
                {
                    Helpers.LogMessage(command.dte, command.dte, ex.Message);
                }

                Helpers.HideProgress(command.dte);

            }
        }

        /// <summary>
        /// Removes the previously added reference, if it was created
        /// </summary>
        public override void Undo()
        {
        }
    }

    class RunCommandCommand
    {
        public DTE dte = null;
        public string WorkingDirectory = "";
        public string Command = "";
        public string Arguments = "";
        public bool CreateNoWindow = false;
    }
}