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
using System.ComponentModel.Design;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// Displays the help 
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class ShowHelp : ConfigurableAction
    {
        private string _HelpPage;

        protected string GetBasePath()
        {
            return base.GetService<IConfigurationService>(true).BasePath;
        }

        [Input(Required = false)]
        public string HelpPage
        {
            get { return _HelpPage; }
            set { _HelpPage = value; }
        }

        public override void Execute()
        {
            DTE dte = GetService<DTE>(true);

            try
            {
                string basePath = GetBasePath();
                string uriString = basePath + "\\Help\\OutputHTML\\SPSF_ROOT.html";
                if (!string.IsNullOrEmpty(_HelpPage))
                {
                    uriString = basePath + "\\Help\\OutputHTML\\" + _HelpPage;
                }

                if (File.Exists(uriString))
                {
                    Helpers.LogMessage(dte, this, "Opening " + uriString);
                    Helpers.OpenWebPage(dte, uriString);
                    return;
                }
                else
                {
                    Helpers.LogMessage(dte, this, "Help not found at " + uriString);
                }
            }
            catch (Exception ex)
            {
                Helpers.LogMessage(dte, this, ex.ToString());
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