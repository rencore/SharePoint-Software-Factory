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
    public class OpenItemInBrowser : ConfigurableAction
    {
        private ProjectItem projectItem;

        [Input(Required = true)]
        public ProjectItem SelectedItem
        {
            get { return projectItem; }
            set { projectItem = value; }
        }

        protected string GetBasePath()
        {
            return base.GetService<IConfigurationService>(true).BasePath;
        }

        public override void Execute()
        {
            DTE service = (DTE)this.GetService(typeof(DTE));

            try
            {
                if (projectItem != null)
                {
                    string webUrl = Helpers.GetDebuggingWebApp(service, GetBasePath());

                    if (webUrl == "")
                    {
                        Helpers.LogMessage(service, this, "No web application for debugging defined in Application.config.");
                        return;
                    }

                    string relativetargetFileName = DeploymentHelpers.GetDeploymentPathOfItem(service, projectItem); //path starting with template

                    //check itempath for paths
                    string translatedPath = "";
                    if (relativetargetFileName.ToLower().StartsWith(@"\template\layouts\"))
                    {
                        //is layouts file
                        translatedPath = "/_layouts"+Helpers.GetVersionedFolder(service)+"/"+  GetRelativePath(relativetargetFileName, @"\template\layouts\");
                    }
                    else if (relativetargetFileName.ToLower().StartsWith(@"\template\admin\"))
                    {
                        //is layouts file
                        webUrl = new SharePointBrigdeHelper(service).GetCentralAdministrationUrl();
                        translatedPath = "/_admin/" + GetRelativePath(relativetargetFileName, @"\template\admin\");
                    }
                    else if (relativetargetFileName.ToLower().StartsWith(@"\template\images\"))
                    {
                        //is layouts file
                        translatedPath = "/_layouts" + Helpers.GetVersionedFolder(service) + "/images/" + GetRelativePath(relativetargetFileName, @"\template\images\");
                    }
                    else if (relativetargetFileName.ToLower().Contains(@"\isapi\"))
                    {
                        //is layouts file
                        translatedPath = "/_vti_bin/" + GetRelativePath(relativetargetFileName, @"\isapi\");
                    }
                    else if (relativetargetFileName.ToLower().Contains(@"\admisapi\"))
                    {
                        //is layouts file
                        webUrl = new SharePointBrigdeHelper(service).GetCentralAdministrationUrl();
                        translatedPath = "/_vti_adm/" + GetRelativePath(relativetargetFileName, @"\admisapi\");
                    }

                    if (webUrl.EndsWith("/"))
                    {
                        webUrl = webUrl.Substring(0, webUrl.Length - 1);
                    }

                    Helpers.LogMessage(service, this, "Opening " + webUrl + translatedPath);

                    /*
					Window win = service.Windows.Item(EnvDTE.Constants.vsWindowKindCommandWindow);
					CommandWindow comwin = (CommandWindow)win.Object;
					comwin.SendInput("nav \"" + webUrl + translatedPath + "\"", true);
                    */

                    Helpers.OpenWebPage(service, webUrl + translatedPath);
                    return;
                }
            }
            catch (Exception)
            {
            }
        }

        private string GetRelativePath(string itemPath, string replaceString)
        {
            string result = "";
            result = itemPath.Substring(itemPath.IndexOf(replaceString, StringComparison.InvariantCultureIgnoreCase) + replaceString.Length);
            result = result.Replace("\\", "/");
            return result;
        }

        /// <summary>
        /// Removes the previously added reference, if it was created
        /// </summary>
        public override void Undo()
        {
        }
    }
}