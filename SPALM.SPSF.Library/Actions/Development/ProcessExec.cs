using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using EnvDTE;
using System.ComponentModel;

namespace SPALM.SPSF.Library
{
  internal class ProcessExec : IDisposable
  {
    // Methods
    private DTE dte;
    private string command;
    private string arguments;
    private string workingDirectory;
    private bool createNoWindow = true;
    private string status;
    private int counter = 0;

    public ProcessExec(DTE _dte, string _command, string _arguments, string _status)
    {
      dte = _dte;
      command = _command;
      arguments = _arguments;
      status = _status;
      workingDirectory = "";
      createNoWindow = true;
    }

    public ProcessExec(DTE _dte, string _command, string _arguments, string _status, string _workingDirectory, bool _createNoWindow)
    {
      dte = _dte;
      command = _command;
      arguments = _arguments;
      status = _status;
      workingDirectory = _workingDirectory;
      createNoWindow = _createNoWindow;
    }


		public void RunProcess()
		{
			if (createNoWindow)
			{
				//perform work automatically
				Helpers.LogMessage(dte, this, status);
				worker_DoWork();
			}
			else
			{
				//open a window directly, execute the command and leave the window open
				ProcessStartInfo psi = new ProcessStartInfo();
				psi.FileName = command;
				psi.Arguments = arguments;
				psi.WorkingDirectory = workingDirectory;
				psi.CreateNoWindow = false;
				psi.UseShellExecute = false;
				System.Diagnostics.Process p = new System.Diagnostics.Process();
				p.StartInfo = psi;
				p.Start();
			}
		}

    void worker_DoWork()
    {
      ProcessStartInfo psi = new ProcessStartInfo();
      psi.FileName = command;
      psi.Arguments = arguments;
      psi.WorkingDirectory = workingDirectory;
      psi.CreateNoWindow = createNoWindow;
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
          Helpers.LogMessage(dte, this, text);
          if (counter < 100)
          {
            counter += 10;
          }
          Helpers.ShowProgress(dte, "Running process...", counter);
        }
        System.Threading.Thread.Sleep(100);
      }

			Helpers.LogMessage(dte, this, p.StandardOutput.ReadToEnd());
			Helpers.HideProgress(dte);
    }

    public void Dispose()
    {
      GC.SuppressFinalize(this);
    }
  }
}
