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
using System.Text.RegularExpressions;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.Win32;
using Microsoft.VisualStudio.Shell;
using Microsoft.Practices.Common.Services;
using System.Collections.Generic;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates;
using System.CodeDom.Compiler;
using System.Text;
using System.Xml;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// Renames a project
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class MigrateSelectedProjects : ConfigurableAction
    {
        public override void Execute()
        {
            DTE service = (DTE)this.GetService(typeof(DTE));
            
            try
            {
                Project selectedProject = Helpers.GetSelectedProject(service);
                if (selectedProject != null)
                {
                    Helpers.LogMessage(service, this, "*** Migrating selected projects ***");
                    MigrateProject(service, selectedProject);
                }
                else
                {
                    Helpers.LogMessage(service, this, "*** Migrating contained projects ***");
                    foreach (Project project in Helpers.GetAllProjects(service))
                    {
                        MigrateProject(service, project);
                    } 
                }                              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        [Input(Required = true)]
        public string DeploymentProject { get; set; }

        private void MigrateProject(DTE service, Project project)
        {
            Helpers.LogMessage(service, this, "Migrating project '" + project.Name + "'");
            try
            {
                if (Helpers.IsCustomizationProject(project))
                {
                    Helpers2.AddBuildDependency(service, Helpers.GetProjectByName(service, this.DeploymentProject), project);
                    MigrateCustomizationProject(service, project);
                }
                else if (project.Name.Equals(this.DeploymentProject, StringComparison.InvariantCultureIgnoreCase))
                {
                    MigrateProjectDeployment(project);
                }
            }
            catch (Exception ex)
            {
                Helpers.LogMessage(service, this, "Error: Migration of project '" + project.Name + "' failed: " + ex.Message);
            }
        }

        private void MigrateCustomizationProject(DTE service, Project project)
        {
            Helpers.EnsureCheckout(service, project);

            Helpers.LogMessage(service, this, "*** Migrating project '" + project.Name + "' ****");

            if (Helpers2.IsSharePointVSTemplate(service, project))
            {
              try
              {
                ProjectItem packageGeneratorTT = Helpers.GetProjectItemByName(Helpers.GetProjectItemByName(project.ProjectItems, "Package").ProjectItems, "Package.Generator.tt");
                if (packageGeneratorTT != null)
                {
                  Helpers.LogMessage(service, this, "Deleting Package/Package.Generator.tt");
                  Helpers.EnsureCheckout(service, packageGeneratorTT);
                  packageGeneratorTT.Delete();
                }
              }
              catch
              {
                Helpers.LogMessage(service, this, "Warning: Could not delete Package/Package.Generator.tt");
              }

              try
              {
                ProjectItem packageGeneratorTMP = Helpers.GetProjectItemByName(Helpers.GetProjectItemByName(project.ProjectItems, "Package").ProjectItems, "Package.Generator.tmp");
                if (packageGeneratorTMP != null)
                {
                  Helpers.LogMessage(service, this, "Deleting Package/Package.Generator.tmp");
                  Helpers.EnsureCheckout(service, packageGeneratorTMP);
                  packageGeneratorTMP.Delete();
                }
              }
              catch
              {
                Helpers.LogMessage(service, this, "Warning: Could not delete Package/Package.Generator.tmp");
              }                            

              Helpers.LogMessage(service, this, "Settings IncludeAssemblyInPackge to True");
              try
              {
                Helpers.SetProjectPropertyGroupValue(project, "IncludeAssemblyInPackage", "True");
              }
              catch
              { 
              }
            }

            //4. add property Sandboxedsolution if property is not available
            try
            {
                string testvalue = Helpers.GetProjectPropertyGroupValue(project, "Sandboxedsolution", "NOTFOUND").ToString();
                if (testvalue == "NOTFOUND")
                {
                    Helpers.LogMessage(service, this, "Updated project property 'Sandboxedsolution'");
                    Helpers.SetProjectPropertyGroupValue(project, "Sandboxedsolution", "False");
                }
            }
            catch { }

            try
            {
                string nameOfProject =  project.Name;
                //Helpers2.MoveProjectToSolutionFolder(service, project.Name, "Solutions");
               //after moving we need to get the moved project again from the name
                project = Helpers.GetProjectByName(service, nameOfProject);
            }
            catch { }



            //5. Add import to SharePoint.targets (e.g. for WSP projects)
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
                    //project.Imports.AddNewImport(@"C:\Windows\Microsoft.NET\Framework\v3.5\Microsoft.CSharp.targets", null);
                    //project.Save(fileName);
                    service.ExecuteCommand("Project.ReloadProject", string.Empty);
                }
                catch { }
            }      

        }


        protected string GetBasePath()
        {
            return base.GetService<IConfigurationService>(true).BasePath;
        }

        protected string GetTemplateBasePath()
        {
            return new DirectoryInfo(this.GetBasePath() + @"\Templates").FullName;
        }

        private void MigrateProjectDeployment(Project project)
        {
            Helpers.LogMessage((DTE)this.GetService(typeof(DTE)), this, "*** Migrating project '" + project.Name + "' ****");
            Helpers.EnsureCheckout(project.DTE, project);

            string templateBasePath = this.GetTemplateBasePath();

            //replace scripts
            ProjectItem scriptsFolder = Helpers.GetProjectFolder(project.ProjectItems, "Scripts", true);
            Helpers.AddFromTemplate(scriptsFolder.ProjectItems, Path.Combine(templateBasePath, @"Solutions\Projects\Deploy\Scripts\SPSD_Main.ps1"), "SPSD_Main.ps1", true);
            Helpers.AddFromTemplate(scriptsFolder.ProjectItems, Path.Combine(templateBasePath, @"Solutions\Projects\Deploy\Scripts\SPSD_Base.ps1"), "SPSD_Base.ps1", true);
            Helpers.AddFromTemplate(scriptsFolder.ProjectItems, Path.Combine(templateBasePath, @"Solutions\Projects\Deploy\Scripts\SPSD_Deployment.ps1"), "SPSD_Deployment.ps1", true);
            Helpers.AddFromTemplate(scriptsFolder.ProjectItems, Path.Combine(templateBasePath, @"Solutions\Projects\Deploy\Scripts\SPSD_Utilities.ps1"), "SPSD_Utilities.ps1", true);
            Helpers.AddFromTemplate(scriptsFolder.ProjectItems, Path.Combine(templateBasePath, @"Solutions\Projects\Deploy\Scripts\SharePointVersions.xml"), "SharePointVersions.xml", true);

            //batch files
            ProjectItem batchesFolder = Helpers.GetProjectFolder(project.ProjectItems, "Batches", true);
            Helpers.AddFromTemplate(batchesFolder.ProjectItems, Path.Combine(templateBasePath, @"Solutions\Projects\Deploy\Batches\Deploy.bat"), "Deploy.bat", true);
            Helpers.AddFromTemplate(batchesFolder.ProjectItems, Path.Combine(templateBasePath, @"Solutions\Projects\Deploy\Batches\Retract.bat"), "Retract.bat", true);
            Helpers.AddFromTemplate(batchesFolder.ProjectItems, Path.Combine(templateBasePath, @"Solutions\Projects\Deploy\Batches\Undeploy.bat"), "Undeploy.bat", true);
            Helpers.AddFromTemplate(batchesFolder.ProjectItems, Path.Combine(templateBasePath, @"Solutions\Projects\Deploy\Batches\Update.bat"), "Update.bat", true);
        
        }

        private void Rename12ToSharePointRoot(Project project)
        {
            ProjectItem hiveItem = null;
            try
            {
                hiveItem = Helpers.GetProjectItemByName(project.ProjectItems, "12");
                if (hiveItem != null)
                {
                    hiveItem.Name = "SharePointRoot";
                    Helpers.LogMessage(project.DTE, this, "Renamed folder '12' to 'SharePointRoot'");
                    return;
                }
            }
            catch { }

            try
            {
                hiveItem = Helpers.GetProjectItemByName(project.ProjectItems, "14");
                if (hiveItem != null)
                {
                    hiveItem.Name = "SharePointRoot";
                    Helpers.LogMessage(project.DTE, this, "Renamed folder '14' to 'SharePointRoot'");
                    return;
                }
            }
            catch { }

            try
            {
                hiveItem = Helpers.GetProjectItemByName(project.ProjectItems, "15");
                if (hiveItem != null)
                {
                    hiveItem.Name = "SharePointRoot";
                    Helpers.LogMessage(project.DTE, this, "Renamed folder '15' to 'SharePointRoot'");
                    return;
                }
            }
            catch { }
            Helpers.LogMessage(project.DTE, this, "Warning: Could not rename folder 12, 14 or 15 to 'SharePointRoot'");
        }






        private void MigrateFile(string path)
        {
          try
          {
            File.Copy(path, path + ".bak", true);
          }
          catch {}

            string removedContent = "";

            XmlDocument csprojfile = new XmlDocument();
            csprojfile.Load(path);

            XmlNamespaceManager newnsmgr = new XmlNamespaceManager(csprojfile.NameTable);
            newnsmgr.AddNamespace("ns", "http://schemas.microsoft.com/developer/msbuild/2003");

            XmlNode nodeProject = csprojfile.SelectSingleNode("/ns:Project", newnsmgr);

            //check for nodeimport
            CheckImportNode(csprojfile, nodeProject, @"$(SolutionDir)\SPSF.targets", @"!Exists('$(MSBuildProjectDirectory)\..\SPSF.targets')");
            CheckImportNode(csprojfile, nodeProject, @"$(MSBuildProjectDirectory)\..\SPSF.targets", @"Exists('$(MSBuildProjectDirectory)\..\SPSF.targets')");

            RemoveImportNode(csprojfile, nodeProject, @"$(SolutionDir)\SharePointTargets.targets");
            RemoveImportNode(csprojfile, nodeProject, @"$(MSBuildProjectDirectory)\..\SharePointTargets.targets");

            /*
             <Import Condition="!Exists('$(MSBuildProjectDirectory)\..\SPSF.targets')" Project="$(SolutionDir)\SPSF.targets" />
             <Import Condition=" Exists('$(MSBuildProjectDirectory)\..\SPSF.targets')" Project="$(MSBuildProjectDirectory)\..\SPSF.targets" />
    
             */


            XmlNode nodeBeforeBuild = csprojfile.SelectSingleNode("/ns:Project/ns:Target[@Name='BeforeBuild' and @DependsOnTargets='$(BeforeBuildDependsOn)']", newnsmgr);
            if (nodeBeforeBuild == null)
            {
                XmlNode nodeBeforeBuildOther = csprojfile.SelectSingleNode("/ns:Project/ns:Target[@Name='BeforeBuild']", newnsmgr);
                if (nodeBeforeBuildOther != null)
                {   //remove this node with same name
                    removedContent += nodeBeforeBuildOther.OuterXml + Environment.NewLine;
                    nodeProject.RemoveChild(nodeBeforeBuildOther);
                }
                AddBeforeBuildNode(csprojfile, nodeProject);
            }

            XmlNode nodeAfterBuild = csprojfile.SelectSingleNode("/ns:Project/ns:Target[@Name='AfterBuild' and @DependsOnTargets='$(AfterBuildDependsOn)']", newnsmgr);
            if (nodeAfterBuild == null)
            {
                XmlNode nodeAfterBuildOther = csprojfile.SelectSingleNode("/ns:Project/ns:Target[@Name='AfterBuild']", newnsmgr);
                if (nodeAfterBuildOther != null)
                {   //remove this node with same name
                    removedContent += nodeAfterBuildOther.OuterXml + Environment.NewLine;
                    nodeProject.RemoveChild(nodeAfterBuildOther);
                }
                AddAfterBuildNode(csprojfile, nodeProject);
            }

            if (removedContent != "")
            {
                XmlComment comment = csprojfile.CreateComment("Following content has been removed during migration with SPSF" + Environment.NewLine + removedContent);
                nodeProject.AppendChild(comment);
            }
                      
            csprojfile.Save(path);
        }

        private void RemoveImportNode(XmlDocument csprojfile, XmlNode nodeProject, string projectString)
        {
            Helpers.LogMessage((DTE)this.GetService(typeof(DTE)), this, "Removing import node");

            XmlNamespaceManager newnsmgr = new XmlNamespaceManager(csprojfile.NameTable);
            newnsmgr.AddNamespace("ns", "http://schemas.microsoft.com/developer/msbuild/2003");
            XmlNode nodeImport = csprojfile.SelectSingleNode("/ns:Project/ns:Import[@Project='" + projectString + "']", newnsmgr);
            if (nodeImport != null)
            {
                nodeImport.ParentNode.RemoveChild(nodeImport);
            }
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
                XmlAttribute condiAttribute = csprojfile.CreateAttribute("Condition"); //, "http://schemas.microsoft.com/developer/msbuild/2003");
                condiAttribute.Value = conditionString;
                importNode.Attributes.Append(condiAttribute);
            }
            else
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

        private void AddAfterBuildNode(XmlDocument csprojfile, XmlNode nodeProject)
        {
            Helpers.LogMessage((DTE)this.GetService(typeof(DTE)), this, "Adding new Target node 'AfterBuild'");
            XmlElement afterBuildNode = csprojfile.CreateElement("Target", "http://schemas.microsoft.com/developer/msbuild/2003");
            nodeProject.AppendChild(afterBuildNode);
            XmlAttribute nameAttribute2 = csprojfile.CreateAttribute("Name"); //, "http://schemas.microsoft.com/developer/msbuild/2003");
            nameAttribute2.Value = "AfterBuild";
            afterBuildNode.Attributes.Append(nameAttribute2);
            XmlAttribute dependsAttribute2 = csprojfile.CreateAttribute("DependsOnTargets");//, "http://schemas.microsoft.com/developer/msbuild/2003");
            dependsAttribute2.Value = "$(AfterBuildDependsOn)";
            afterBuildNode.Attributes.Append(dependsAttribute2);
        }

        private void AddBeforeBuildNode(XmlDocument csprojfile, XmlNode nodeProject)
        {
            Helpers.LogMessage((DTE)this.GetService(typeof(DTE)), this, "Adding new Target node 'BeforeBuild'");
            XmlElement beforeBuildNode = csprojfile.CreateElement("Target", "http://schemas.microsoft.com/developer/msbuild/2003");
            nodeProject.AppendChild(beforeBuildNode);
            XmlAttribute nameAttribute = csprojfile.CreateAttribute("Name");//, "http://schemas.microsoft.com/developer/msbuild/2003");
            nameAttribute.Value = "BeforeBuild";
            beforeBuildNode.Attributes.Append(nameAttribute);
            XmlAttribute dependsAttribute = csprojfile.CreateAttribute("DependsOnTargets");//, "http://schemas.microsoft.com/developer/msbuild/2003");
            dependsAttribute.Value = "$(BeforeBuildDependsOn)";
            beforeBuildNode.Attributes.Append(dependsAttribute);
        }



        /// <summary>
        /// Removes the previously added reference, if it was created
        /// </summary>
        public override void Undo()
        {
        }
    }
}