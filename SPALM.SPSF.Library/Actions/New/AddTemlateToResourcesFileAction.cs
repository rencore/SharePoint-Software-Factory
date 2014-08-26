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
using Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates;
using Microsoft.Practices.Common.Services;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ComponentModel.Design;
using System.Xml;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// Adds the content to the recourcefile
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class AddTemlateToResourcesFileAction : BaseTemplateAction
    {
        public AddTemlateToResourcesFileAction() : base()
        {
            
        }

        [Input(Required = false)]
        public string FeatureName { get; set; }

        //[Input(Required = true)]
        //public string TemplateFileName { get; set; }
        
        [Input(Required = false)]
        public bool ApplicationResources { get; set; }

        [Input(Required = false)]
        public bool FeatureResources { get; set; }
        
        [Input(Required = false)]
        public bool AdminResources { get; set; }
        
        public override void Execute()
        {
            if (ExcludeCondition)
            {
                return;
            }
            if (!AdditionalCondition)
            {
              return;
            }

            DTE dte = (DTE)this.GetService(typeof(DTE));
            Project project = this.GetTargetProject(dte);

            string evaluatedTemplateFileName = EvaluateParameterAsString(dte, TemplateFileName);
            

            string templateContent = GenerateContent(evaluatedTemplateFileName, "Resources.resx");
            if (Helpers2.TemplateContentIsEmpty(templateContent))
            {
                //do nothing if no content has been generated
                return;
            }

            //get the project where we pla
            Project globalresproject = Helpers.GetResourcesProject(dte);
            Project selectedproject = Helpers.GetSelectedProject(dte);

            string resourcesFilename = "";
            if (FeatureResources)
            {
                //for feature resources name is "Resources.resx"
                resourcesFilename = "Resources.resx";
            }
            else if (ApplicationResources)
            {
              resourcesFilename = Helpers.GetOutputNameWithoutExtensions(selectedproject) + ".AppResources.resx";
            }
            else
            {
              //global resources get the name of the current project project
              resourcesFilename = Helpers.GetOutputNameWithoutExtensions(selectedproject) + ".resx";
            }            

            //1. find a resourcefile
            if (ApplicationResources)
            {
                
                if (globalresproject != null)
                {
                    //global resources project -> use the file 12/resources     
                    AddApplicationResources(globalresproject, resourcesFilename, templateContent);
                }
                else
                {                    
                    AddApplicationResources(selectedproject, resourcesFilename, templateContent);
                }
            }
            else if (FeatureResources)
            {
                string evaluatedFeatureName = EvaluateParameterAsString(dte, FeatureName);

                //Find the feature folder

                ProjectItem folderOfFeature = Helpers2.GetFolderOfFeature(project, evaluatedFeatureName);
                if (folderOfFeature != null)
                {
                  if (Helpers2.IsSharePointVSTemplate(dte, project))
                  {
                    //put resources.resx directly into under the feature
                    AddResourcesToFolder(folderOfFeature, resourcesFilename, templateContent);
                  }
                  else
                  {
                    //now add a folder "Resources" in the feature and put the "Resource.<culture>.resx" there
                    ProjectItems whereToAdd = Helpers.GetProjectItemsByPath(folderOfFeature.ProjectItems, "Resources");
                    AddResourcesToFolder(whereToAdd.Parent as ProjectItem, resourcesFilename, templateContent);
                  }
                }
            }
            else if(AdminResources)
            {
                //put the content into 
                if (globalresproject != null)
                {
                    //global resources project -> use the file 12/resources     
                    AddAdminResources(globalresproject, resourcesFilename, templateContent);
                }
                else
                {
                    AddAdminResources(selectedproject, resourcesFilename, templateContent);
                }
            }
            else
            {
                //put the content into 
                if (globalresproject != null)
                {
                    //global resources project -> use the file 12/resources     
                    AddGlobalResources(globalresproject, resourcesFilename, templateContent);
                }
                else
                {
                    AddGlobalResources(selectedproject, resourcesFilename, templateContent);
                }
            }
        }

        private void AddGlobalResources(Project _CurrentProject, string resourcefilename, string content)
        {
          if (Helpers2.IsSharePointVSTemplate(_CurrentProject.DTE, _CurrentProject))
          {
            AddResources(_CurrentProject, "Resources\\Resources", resourcefilename, content, SPFileType.RootFile, "Resources");
          }
          else
          {
            //get the folder where to place the resx file
            ProjectItems whereToAdd = Helpers2.GetDeploymentPath(_CurrentProject.DTE, _CurrentProject, SPFileType.RootFile, "Resources");
            AddResourcesToFolder(whereToAdd.Parent as ProjectItem, resourcefilename, content);
          }
        }

        private void AddAdminResources(Project _CurrentProject, string resourcefilename, string content)
        {
            //files for central administration
          if (Helpers2.IsSharePointVSTemplate(_CurrentProject.DTE, _CurrentProject))
          {
            AddResources(_CurrentProject, "Resources\\AppGlobalResources", resourcefilename, content, SPFileType.AppGlobalResource, "");
          }
          else
          {
            ProjectItems whereToAdd = Helpers2.GetDeploymentPath(_CurrentProject.DTE, _CurrentProject, SPFileType.RootFile, "CONFIG\\AdminResources");
            AddResourcesToFolder(whereToAdd.Parent as ProjectItem, resourcefilename, content);
          }
        }

        private void AddApplicationResources(Project _CurrentProject, string resourcefilename, string content)
        {         
          if (Helpers2.IsSharePointVSTemplate(_CurrentProject.DTE, _CurrentProject))
          {
            AddResources(_CurrentProject, "Resources\\AppGlobalResources", resourcefilename, content, SPFileType.AppGlobalResource, "");            
          }
          else
          {
            //deploy resx file to 14\Config\Resources 
            ProjectItems whereToAdd = Helpers2.GetDeploymentPath(_CurrentProject.DTE, _CurrentProject, SPFileType.RootFile, "CONFIG\\Resources");
            AddResourcesToFolder(whereToAdd.Parent as ProjectItem, resourcefilename, content);
          }
        }

        private void AddResources(Project _CurrentProject, string folderName, string resourcefilename, string content, SPFileType deploymentFileType, string deploymentPath)
        {
          //in VSS we would create an empty element "Resources" and would add the resource.resx there with different deploymenttargets
          //1. erstelle folder "Resources" und packe Text\Resources\SharePointProjectItem.spdata.t4 rein
          //2. packe file da hinein und setze noch schnell den deployment path
          //1. erstelle Folder Resources\AppResources

          ProjectItems contentTypeFolder = null;
          string SPDataTemplate = @"Text\Resources\SharePointProjectItem.spdata.t4";
          string evaluatedSPDataTemplate = EvaluateParameterAsString(_CurrentProject.DTE, SPDataTemplate);
          string spDataContent = GenerateContent(evaluatedSPDataTemplate, "SharePointProjectItem.spdata");
          ProjectItem spDataItem = Helpers2.AddFileToProject(_CurrentProject.DTE, _CurrentProject, folderName, "SharePointProjectItem.spdata", spDataContent, false, false, out contentTypeFolder);

          //add the resx file or the content to the folder
          ProjectItem resourceFileItem = AddResourcesToFolder(contentTypeFolder.Parent as ProjectItem, resourcefilename, content);

          //ensure that the resources element folder is part of package
          Helpers2.AddVSElementToVSPackage(_CurrentProject.DTE, _CurrentProject, contentTypeFolder.Parent as ProjectItem);

          //set the deployment type to Resource, AppGlobalResource
          Helpers2.SetDeploymentType(resourceFileItem, deploymentFileType);

          Helpers2.SetDeploymentPath(resourceFileItem, deploymentPath);
        }

        private ProjectItem AddResourcesToFolder(ProjectItem resourcesfolder, string resourcefilename, string content)
        {
            EnvDTE.DTE dte = this.GetService<EnvDTE.DTE>(true);
            ProjectItem resourcesFile = null;

            try
            {
                //find the resources xml file in the feature folder, if it exists
                
                if (resourcesfolder != null)
                {
                    ///12/template/Features
                    resourcesFile = Helpers.GetProjectItemByName(resourcesfolder.ProjectItems, resourcefilename);
                }

                if (resourcesFile == null)
                {
                    //es gibt kein resourcesfile
                    //Resources.resx
                    /*<?xml version="1.0" encoding="utf-8"?>
                    <root>
                      <data name="<#= FeatureName #>_FeatureTitle">
                        <value><#= FeatureTitle #></value>
                      </data>
                      <data name="<#= FeatureName #>_FeatureDescription">
                        <value><#= FeatureDescription #></value>
                      </data>  
                    </root>
                     * */

                    //path to ResourcesTemplate.resx
                    string templateFilepath = Path.Combine(GetTemplateBasePath(), "Text\\ResourcesTemplate.resx");

                    XmlDocument resdoc = new XmlDocument();
                    resdoc.Load(templateFilepath);

                    //save the file in the project
                    string resourcefolderpath = Helpers.GetFullPathOfProjectItem(resourcesfolder) + resourcefilename;
                    resdoc.Save(resourcefolderpath);

                    //attach the file to the project
                    resourcesFile = Helpers.AddFromFile(resourcesfolder, resourcefolderpath);
                    resourcesFile.Properties.Item("BuildAction").Value = 2;
                }

                if (resourcesFile != null)
                {
                    //there is a resourcesfile                    
                    string path = Helpers.GetFullPathOfProjectItem(resourcesFile);

                    XmlDocument doc = new XmlDocument();
                    doc.Load(path);

                    XmlDocumentFragment docFrag = doc.CreateDocumentFragment();
                    docFrag.InnerXml = content;

                    XmlNode node = doc.SelectSingleNode("/root");
                    if (node != null)
                    {
                        node.AppendChild(docFrag);

                        Helpers.EnsureCheckout(dte, path);

                        Helpers.CheckLicenseComment(doc);

                        XmlWriter xw = XmlWriter.Create(path, Helpers.GetXmlWriterSettings(path));
                        doc.Save(xw);
                        xw.Flush();
                        xw.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return resourcesFile;
        }

        
        /// <summary>
        /// 
        /// </summary>
        public override void Undo()
        {
            //TODO: Delete the created projectitems
        }
    }    
}