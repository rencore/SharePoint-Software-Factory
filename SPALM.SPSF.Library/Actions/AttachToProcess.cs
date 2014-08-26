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
    public class AttachToProcess : ConfigurableAction
    {
        private int _ProcessID = 0;

        [Input(Required = true)]
        public int ProcessID
        {
          get { return _ProcessID; }
          set { _ProcessID = value; }
        }

        public override void Execute()
        {
            DTE service = (DTE)this.GetService(typeof(DTE));

            try
            {
              EnvDTE.Processes processes = service.Debugger.LocalProcesses;
              foreach (EnvDTE.Process proc in processes)
              {
                if (proc.ProcessID == ProcessID)
                {
                  Helpers.LogMessage(service, this, "Attaching to process " + proc.Name + " (" + ProcessID.ToString() + ")");
                  proc.Attach();
                }
              }
            }
            catch (Exception ex)
            {
              MessageBox.Show("Process not found. Maybe the process is not started");
              Helpers.LogMessage(service, this, ex.ToString());
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