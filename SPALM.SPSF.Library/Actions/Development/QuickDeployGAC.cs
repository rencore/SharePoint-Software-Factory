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

#endregion

namespace SPALM.SPSF.Library.Actions
{
    [ServiceDependency(typeof(DTE))]
    public class QuickDeployGAC : ConfigurableAction
    {
        protected string GetBasePath()
        {
          return base.GetService<IConfigurationService>(true).BasePath;
        }

        public override void Execute()
        {
            DTE dte = GetService<DTE>(true);

          
            if (DeploymentHelpers.CheckRebuildForSelectedProjects(dte))
            {
              DeploymentHelpers.QuickDeployGAC(dte);
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