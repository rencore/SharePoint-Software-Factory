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
using System.Xml;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// Add a new item to a given folder
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class EnableWorkflowFoundation : ConfigurableAction
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
            string templatesDir = Path.Combine(GetTemplateBasePath(), "Items.Cache");
            //Helpers.EnsureGaxPackageRegistration("{14822709-B5A1-4724-98CA-57A101D1B079}", GetPackageGuid(), templatesDir, GetPackageCaption());
            Helpers.EnsureGaxPackageRegistration("{14822709-B5A1-4724-98CA-57A101D1B079}", "{14822709-B5A1-4724-98CA-57A101D1B079}", templatesDir, GetPackageCaption());

            string projectTypeGuids = "";
            object service = null;
            Microsoft.VisualStudio.Shell.Interop.IVsSolution solution = null;
            Microsoft.VisualStudio.Shell.Interop.IVsHierarchy hierarchy = null;
            Microsoft.VisualStudio.Shell.Interop.IVsAggregatableProject aggregatableProject = null;
            int result = 0;

            service = GetService(typeof(Microsoft.VisualStudio.Shell.Interop.IVsSolution));
            solution = (Microsoft.VisualStudio.Shell.Interop.IVsSolution)service;

            result = solution.GetProjectOfUniqueName(proj.UniqueName, out hierarchy);

            if (result == 0)
            {
                aggregatableProject = (Microsoft.VisualStudio.Shell.Interop.IVsAggregatableProject)hierarchy;
                result = aggregatableProject.GetAggregateProjectTypeGuids(out projectTypeGuids);

                //workflows
                if (!projectTypeGuids.ToUpper().Contains("{14822709-B5A1-4724-98CA-57A101D1B079}"))
                {
                    if (projectTypeGuids == "")
                    {
                        projectTypeGuids = "{14822709-B5A1-4724-98CA-57A101D1B079}";
                    }
                    else
                    {
                        projectTypeGuids = "{14822709-B5A1-4724-98CA-57A101D1B079}" + ";" + projectTypeGuids;
                    }
                }

                //csharp
                if (!projectTypeGuids.ToUpper().Contains("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"))
                {
                    if (projectTypeGuids != "")
                    {
                        projectTypeGuids += ";";
                    }
                    projectTypeGuids += "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";
                }

                aggregatableProject.SetAggregateProjectTypeGuids(projectTypeGuids);
            }
        }

        public override void Execute()
        {
            SetProjectTypeGuids(project);

            //add import statement
            //<Import Project="$(MSBuildToolsPath)\Workflow.Targets" />

            DTE service = (DTE)this.GetService(typeof(DTE));

            string fileName = project.FullName;
            Helpers.SelectProject(project);
            if (service.SuppressUI || MessageBox.Show("The project file of project " + project.Name + " must be updated. Can SPSF save and unload the project?", "Unloading project", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                service.Documents.CloseAll(vsSaveChanges.vsSaveChangesPrompt);

                try
                {
                    Helpers.LogMessage(project.DTE, this, "Updating csproj file");
                    service.ExecuteCommand("File.SaveAll", string.Empty);
                    service.ExecuteCommand("Project.UnloadProject", string.Empty);
                    MigrateFile(fileName);
                    service.ExecuteCommand("Project.ReloadProject", string.Empty);
                }
                catch 
                {
                    Helpers.LogMessage(project.DTE, this, "Could not update project file. Add following statement manually:");
                    Helpers.LogMessage(project.DTE, this, "<Import Project=\"$(MSBuildToolsPath)\\Workflow.Targets\" />");
                }
            }

        }

        private void MigrateFile(string path)
        {
            XmlDocument csprojfile = new XmlDocument();
            csprojfile.Load(path);

            XmlNamespaceManager newnsmgr = new XmlNamespaceManager(csprojfile.NameTable);
            newnsmgr.AddNamespace("ns", "http://schemas.microsoft.com/developer/msbuild/2003");

            XmlNode nodeProject = csprojfile.SelectSingleNode("/ns:Project", newnsmgr);

            //check for nodeimport
            CheckImportNode(csprojfile, nodeProject, @"$(MSBuildToolsPath)\Workflow.Targets", "");

            csprojfile.Save(path);
        }

        private void CheckImportNode(XmlDocument csprojfile, XmlNode nodeProject, string projectString, string conditionString)
        {
            Helpers.LogMessage((DTE)this.GetService(typeof(DTE)), this, "Adding new Import node");

            XmlNamespaceManager newnsmgr = new XmlNamespaceManager(csprojfile.NameTable);
            newnsmgr.AddNamespace("ns", "http://schemas.microsoft.com/developer/msbuild/2003");
            XmlNode nodeImport = csprojfile.SelectSingleNode("/ns:Project/ns:Import[@Project='" + projectString + "']", newnsmgr);
            if (nodeImport == null)
            {
                //ok, die 1. node ist noch nicht da
                XmlElement importNode = csprojfile.CreateElement("Import", "http://schemas.microsoft.com/developer/msbuild/2003");
                nodeProject.AppendChild(importNode);
                XmlAttribute importAttribute = csprojfile.CreateAttribute("Project"); //, "http://schemas.microsoft.com/developer/msbuild/2003");
                importAttribute.Value = projectString;
                importNode.Attributes.Append(importAttribute);
                if (conditionString != "")
                {
                    XmlAttribute condiAttribute = csprojfile.CreateAttribute("Condition"); //, "http://schemas.microsoft.com/developer/msbuild/2003");
                    condiAttribute.Value = conditionString;
                    importNode.Attributes.Append(condiAttribute);
                }
            }
            else
            {
                if (conditionString != "")
                {
                    //ok, die node ist da, ist aber auch die condition richtig?
                    if ((nodeImport.Attributes["Condition"] != null) && (nodeImport.Attributes["Condition"].Value.Trim() == conditionString))
                    {
                        //ok, alles ist korrekt, wir machen nix
                    }
                    else
                    {
                        //ok, wenn condition da, dann wert setzen, ansonsten Conditionattribute erzeugen
                        if (nodeImport.Attributes["Condition"] != null)
                        {
                            nodeImport.Attributes["Condition"].Value = conditionString;
                        }
                        else
                        {
                            XmlAttribute condiAttribute = csprojfile.CreateAttribute("Condition"); //, "http://schemas.microsoft.com/developer/msbuild/2003");
                            condiAttribute.Value = conditionString;
                            nodeImport.Attributes.Append(condiAttribute);
                        }
                    }
                }
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