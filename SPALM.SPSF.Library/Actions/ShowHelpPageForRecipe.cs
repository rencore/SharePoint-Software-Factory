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
    /// Displays the help page of the current recipe. Takes the argument "RecipeName" and
    /// creates a URL HTML/RecipeName/index.html
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class ShowHelpPageForRecipe : ConfigurableAction
    {
        protected string GetBasePath()
        {
            return base.GetService<IConfigurationService>(true).BasePath;
        }

        public override void Execute()
        {
            DTE dte = GetService<DTE>(true);
            string recipeName = "";
            try
            {
                IDictionaryService dictionary = GetService<IDictionaryService>();
                recipeName = dictionary.GetValue("RecipeName").ToString();
            }
            catch (Exception)
            {
                Helpers.LogMessage(dte, this, "Could not find recipe name in argument 'RecipeName'");
                return;
            }

            try
            {
                if (recipeName == "")
                {
                    Helpers.LogMessage(dte, this, "Value in 'RecipeName' is empty");
                    return;
                }


                string basePath = GetBasePath();

                string url = basePath += "\\Help\\OutputHTML\\SPSF_RECIPE_" + recipeName.ToUpper() + ".html";
                if (!File.Exists(url))
                {
                    Helpers.LogMessage(dte, this, "Could not find help page " + url);
                    return;
                }

                Window win = dte.Windows.Item(EnvDTE.Constants.vsWindowKindCommandWindow);
                CommandWindow comwin = (CommandWindow)win.Object;
                comwin.SendInput("nav \"" + url + "\"", true);

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