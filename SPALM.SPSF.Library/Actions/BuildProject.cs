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
    public class BuildProject : ConfigurableAction
    {
        private string _TargetName = "";

        [Input(Required = true)]
        public string TargetName
        {
          get { return _TargetName; }
          set { _TargetName = value; }
        }

        public override void Execute()
        {
            
            DTE service = (DTE)this.GetService(typeof(DTE));
            Project project = Helpers.GetSelectedProject(service);
            service.Solution.SolutionBuild.BuildProject(TargetName, project.UniqueName, true);         
        }

        /// <summary>
        /// Removes the previously added reference, if it was created
        /// </summary>
        public override void Undo()
        {
        }
    }    
}