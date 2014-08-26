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
    public class RunBatch : ConfigurableAction
    {
			private ProjectItem _BatchFile = null;

        [Input(Required = true)]
        public ProjectItem BatchFile
        {
					get { return _BatchFile; }
					set { _BatchFile = value; }
        }

        public override void Execute()
        {
            string targetsFilePath = "";
						if (_BatchFile != null)
            {
							targetsFilePath = Helpers.GetFullPathOfProjectItem(_BatchFile);
            }

            System.Diagnostics.Process snProcess = new System.Diagnostics.Process();
						snProcess.StartInfo.FileName = targetsFilePath;
            snProcess.StartInfo.Arguments = "";
            snProcess.StartInfo.CreateNoWindow = false;
            snProcess.StartInfo.UseShellExecute = false;
						snProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(targetsFilePath);
            snProcess.Start();          
        }

        /// <summary>
        /// Removes the previously added reference, if it was created
        /// </summary>
        public override void Undo()
        {
        }
    }    
}