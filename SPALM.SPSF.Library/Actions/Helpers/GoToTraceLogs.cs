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
    public class GoToTraceLogs : ConfigurableAction
    {

      public override void Execute()
      {
        DTE dte = GetService<DTE>(true);

        try
        {
            string traceLogPath = new SharePointBrigdeHelper(dte).GetPathToTraceLogs();
            if (!Directory.Exists(traceLogPath))
            {
                throw new FileNotFoundException("Trace Log folder " + traceLogPath + " not found");
            }

            DateTime oldestDate = DateTime.Now.AddYears(-10);
            string newestFile = "";
            DirectoryInfo traceDir = new DirectoryInfo(traceLogPath);
            foreach(FileSystemInfo fileInfo in traceDir.GetFileSystemInfos("*.log"))
            {
                if (fileInfo.LastWriteTime > oldestDate)
                {
                    oldestDate = fileInfo.LastWriteTime;
                    newestFile = fileInfo.FullName;
                }
            }

            if (!string.IsNullOrEmpty(newestFile))
            {
                traceLogPath = newestFile;
            }

            //start process with ShellExecute
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = traceLogPath;
            psi.Arguments = "";
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