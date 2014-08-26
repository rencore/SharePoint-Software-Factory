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
using Microsoft.VisualStudio.Shell.Interop;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// registers the projecttypeguids for WAP (web applicaiton projects)
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class EnableWAPTemplates : ConfigurableAction
    {
        [Input(Required = true)]
        public Project Project
        {
            get
            {
                return this.project;
            }
            set
            {
                this.project = value;
            }
        }
        private Project project;

        protected string GetBasePath()
        {
            return base.GetService<IConfigurationService>(true).BasePath;
        }

        protected string GetPackageGuid()
        {
            return base.GetService<IConfigurationService>(true).CurrentPackage.Guid;
        }

        protected string GetPackageCaption()
        {
            return base.GetService<IConfigurationService>(true).CurrentPackage.Caption;
        }

        private string GetTemplateBasePath()
        {
            return new DirectoryInfo(this.GetBasePath() + @"\Templates").FullName;
        }


        public void SetProjectTypeGuids(EnvDTE.Project proj)
        {
            if (Helpers2.IsSharePointVSTemplate(proj.DTE, proj))
            {
                //for VS2010 projects the guid do not need to be added
                return;
            }

            bool reloadRequired = false;

            string templatesDir = Path.Combine(GetTemplateBasePath(), "Items.Cache");
            //Helpers.EnsureGaxPackageRegistration("{14822709-B5A1-4724-98CA-57A101D1B079}", GetPackageGuid(), templatesDir, GetPackageCaption());
            Helpers.EnsureGaxPackageRegistration("{349C5853-65DF-11DA-9384-00065B846F21}", "{349C5853-65DF-11DA-9384-00065B846F21}", templatesDir, GetPackageCaption());

            string projectTypeGuids = "";

            //Microsoft.VisualStudio.Shell.Interop.IVsSolution solution = null;
            Microsoft.VisualStudio.Shell.Interop.IVsHierarchy hierarchy = null;
            //Microsoft.VisualStudio.Shell.Interop.IVsAggregatableProject aggregatableProject = null;
            Microsoft.VisualStudio.Shell.Flavor.IVsAggregatableProjectCorrected aggregatableProject = null;
            int result = 0;

            //service = GetService(typeof(Microsoft.VisualStudio.Shell.Interop.IVsSolution));
            //solution = (Microsoft.VisualStudio.Shell.Interop.IVsSolution)service;

            IVsSolution solution = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SVsSolution)) as IVsSolution;
            if (solution != null)
            {
                result = solution.GetProjectOfUniqueName(proj.UniqueName, out hierarchy);

                if (result == 0)
                {

                    //aggregatableProject = (Microsoft.VisualStudio.Shell.Interop.IVsAggregatableProject)hierarchy;
                    aggregatableProject = (Microsoft.VisualStudio.Shell.Flavor.IVsAggregatableProjectCorrected)hierarchy;
                    result = aggregatableProject.GetAggregateProjectTypeGuids(out projectTypeGuids);

                    //csharp
                    if (!projectTypeGuids.ToUpper().Contains("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"))
                    {
                        if (projectTypeGuids != "")
                        {
                            projectTypeGuids += ";";
                        }
                        projectTypeGuids += "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";
                    }

                    //wap, warnung: Attach guid before {FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}
                    if (!projectTypeGuids.ToUpper().Contains(String.Intern("{349c5851-65df-11da-9384-00065b846f21}").ToUpper()))
                    {
                        reloadRequired = true;
                        if (projectTypeGuids == "")
                        {
                            projectTypeGuids = "{349c5851-65df-11da-9384-00065b846f21}";
                        }
                        else
                        {
                            projectTypeGuids = "{349c5851-65df-11da-9384-00065b846f21}" + ";" + projectTypeGuids;
                        }
                    }

                    aggregatableProject.SetAggregateProjectTypeGuids(projectTypeGuids);
                }
            }
            else
            {
                Helpers.LogMessage(proj.DTE, this, "Could not add ProjectTypeGuids '{349c5851-65df-11da-9384-00065b846f21};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}' to the project");
            }

            if (reloadRequired)
            {
                Project selProj = Helpers.GetSelectedProject(proj.DTE);
                MessageBox.Show("To enable visual editing of ascx controls the type of the project has been changed." + Environment.NewLine + Environment.NewLine + "The project '" + selProj.Name + "' needs to be reloaded manually ('Unload Project').", "Reload project", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Helpers.SelectProject(project);

                Helpers.LogMessage(project.DTE, this, "Updating csproj file");
                proj.DTE.ExecuteCommand("File.SaveAll", string.Empty);
                proj.DTE.ExecuteCommand("Project.UnloadProject", string.Empty);
                proj.DTE.ExecuteCommand("Project.ReloadProject", string.Empty);

                //Project selProj = Helpers.GetSelectedProject(proj.DTE);
                //Helpers.LogMessage(proj.DTE, this, "selected project " + selProj.Name);

                /*
               Window win = proj.DTE.Windows.Item(EnvDTE.Constants.vsWindowKindCommandWindow);
               CommandWindow comwin = (CommandWindow)win.Object;
               comwin.SendInput("Project.UnloadProject", true);
               comwin.SendInput("Project.ReloadProject", true);
                
               
               proj.DTE.ExecuteCommand("File.SaveAll", string.Empty);
               proj.DTE.ExecuteCommand("Project.UnloadProject", string.Empty);
               proj.DTE.ExecuteCommand("Project.ReloadProject", string.Empty);
               */

            }
        }

        public override void Execute()
        {
            SetProjectTypeGuids(project);
        }

        /// <summary>
        /// Removes the previously added reference, if it was created
        /// </summary>
        public override void Undo()
        {
        }
    }
}