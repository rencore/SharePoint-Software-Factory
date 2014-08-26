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
    public class OpenWSP : ConfigurableAction
    {
        private Project _Project = null;

        [Input(Required = true)]
        public Project Project
        {
            get
            {
                return _Project;
            }
            set
            {
                _Project = value;
            }
        }

      public override void Execute()
      {
        DTE dte = GetService<DTE>(true);

        try
        {
            string wspPath = DeploymentHelpers.GetWSPFilePath(dte, _Project);

            if (!DeploymentHelpers.CheckRebuildForProject(dte, _Project))
            {
                return;
            }


            
            if (!File.Exists(wspPath))
            {
                throw new FileNotFoundException("WSP Solution file not found at location " + wspPath);
            }

            //copy the file as cab file to a temp folder
            string wspName = Path.GetFileNameWithoutExtension(wspPath);
            string tempWspName = Path.Combine(Path.GetTempPath(), wspName + ".cab");
            File.Copy(wspPath, tempWspName, true);

            //start process with ShellExecute
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = tempWspName;
            psi.Arguments = "";
            psi.WorkingDirectory = Path.GetDirectoryName(wspPath);
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