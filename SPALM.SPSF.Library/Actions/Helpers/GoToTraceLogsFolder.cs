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
using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    [ServiceDependency(typeof(DTE))]
    public class GoToTraceLogsFolder : ConfigurableAction
    {

      public override void Execute()
      {
        DTE dte = GetService<DTE>(true);

        try
        {
            string traceLogPath = new SharePointBrigdeHelper(dte).GetPathToTraceLogs();
            Helpers.LogMessage(dte, this, "Trace log location: " + traceLogPath);

            if (!Directory.Exists(traceLogPath))
            {
                throw new FileNotFoundException("Trace Log folder " + traceLogPath + " not found");
            }

            //start process with ShellExecute
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "explorer.exe";
            psi.Arguments = "\"" + traceLogPath + "\"";
            psi.CreateNoWindow = true;
            psi.UseShellExecute = true;
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo = psi;
            p.Start();
        }
        catch (Exception ex)
        {
            Helpers.LogMessage(dte, this, ex.Message);
        }        
      }

      /// <summary>
      /// Removes the previously added reference, if it was created
      /// </summary>
      public override void Undo()
      {
      }
  }    
}