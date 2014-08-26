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
    [ServiceDependency(typeof(DTE))]
    public class GoToDebuggingWebApp : ConfigurableAction
    {
        protected string GetBasePath()
        {
            return base.GetService<IConfigurationService>(true).BasePath;
        }

        public override void Execute()
        {
          DTE dte = GetService<DTE>(true);

          try
          {
            string url = Helpers.GetApplicationConfigValue(dte, "DebuggingWebApp", "").ToString();

            if (url != "")
            {
                Helpers.OpenWebPage(dte, url);
            }
            else
            {
                Helpers.LogMessage(dte, this, "There is no web application for debugging given for the project.");
            }
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