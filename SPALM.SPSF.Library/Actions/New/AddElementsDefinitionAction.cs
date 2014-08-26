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
    /// Adds a new Elements definition, e.g. a contenttype and registers the element in the parent feature
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class AddElementsDefinitionAction : BaseTemplateAction
    {
        public AddElementsDefinitionAction() : base()
        {

        }

        /// <summary>
        /// Contains the name of the element, used to create filenames and folders
        /// </summary>
        [Input(Required = true)]
        public string ElementsName { get; set; }

        /// <summary>
        /// Contains the type of element, e.g. "ContentTypes". This is used to summarize elements of the same type (e.g. in feature "ContentTypes.xml" or in a folder "ContentTypes")
        /// </summary>
        [Input(Required = true)]
        public string ElementsCategory { get; set; }

        /// <summary>
        /// Required for Visual Studio 2010 project template 
        /// </summary>
        [Input(Required = true)]
        public string SPDataTemplate { get; set; }

        [Output]
        public ProjectItem CreatedElementFolder { get; set; }

        [Output]
        public ProjectItem CreatedElementFile { get; set; }

        /// <summary>
        /// Name of the parent feature
        /// </summary>
        [Input(Required = false)]
        public string ParentFeatureName { get; set; }

        public override void Execute()
        {
            DTE dte = (DTE)this.GetService(typeof(DTE));
            Project project = this.GetTargetProject(dte);

            //1. get correct parameters ("$(FeatureName)" as "FeatureX")       
            string evaluatedParentFeatureName = EvaluateParameterAsString(dte, ParentFeatureName);
            string evaluatedSPDataTemplate = EvaluateParameterAsString(dte, SPDataTemplate);
            string evaluatedElementsTemplate = EvaluateParameterAsString(dte, TemplateFileName);
            string evaluatedElementsCategory = EvaluateParameterAsString(dte, ElementsCategory);
            string evaluatedElementsName = EvaluateParameterAsString(dte, ElementsName);
            string evaluatedTargetFileName = EvaluateParameterAsString(dte, TargetFileName);
            string evaluatedDeploymentPath = EvaluateParameterAsString(dte, DeploymentPath);
            
            if (string.IsNullOrEmpty(evaluatedTargetFileName))
            {
                evaluatedTargetFileName = "Elements.xml";
            }

            //2. create the Element
            if (Helpers2.IsSharePointVSTemplate(dte, project))
            {
                AddElementsDefinitionToVSTemplate(dte, project, evaluatedParentFeatureName, evaluatedSPDataTemplate, evaluatedElementsTemplate, evaluatedElementsCategory, evaluatedElementsName, evaluatedTargetFileName);
            }
        }

        private void AddElementsDefinitionToVSTemplate(DTE dte, Project project, string evaluatedParentFeatureName, string evaluatedSPDataTemplate, string evaluatedElementsTemplate, string evaluatedElementsCategory, string evaluatedElementsName, string evaluatedTargetFileName)
        {
            string elementsXmlContent = GenerateContent(evaluatedElementsTemplate, "Elements.xml");
            string spDataContent = GenerateContent(evaluatedSPDataTemplate, "SharePointProjectItem.spdata");

            //1. create the folder for the content type, which means adding the spdata stuff to a folder in the project

            string targetFolder = evaluatedElementsCategory + @"\" + evaluatedElementsName;
            ProjectItems contentTypeFolder = null;
            ProjectItem spDataItem = Helpers2.AddFileToProject(dte, project, targetFolder, "SharePointProjectItem.spdata", spDataContent, true, false, out contentTypeFolder);

            //2. put the elements.xml in that folder
            CreatedElementFile = Helpers2.AddFile(dte, contentTypeFolder, evaluatedTargetFileName, elementsXmlContent, true, this.Open);

            if (this.DeploymentTypeIsSet)
            {
                //default deployment type is ElementManifest
                Helpers2.SetDeploymentType(CreatedElementFile, this.DeploymentType);
            }
            else
            {
                Helpers2.SetDeploymentType(CreatedElementFile, SPFileType.ElementManifest);
            }

            //Trace.WriteLine("AddElementsDefinitionAction FindProjectItemByPath " + targetFolder);
            ProjectItem elementFolder = Helpers.FindProjectItemByPath(project, targetFolder);

            //3. add the item to the selected feature or to the package            
            if (!string.IsNullOrEmpty(evaluatedParentFeatureName))
            {
                //element can be added to a feature
                //Trace.WriteLine("AddElementsDefinitionAction evaluatedParentFeatureName=" + evaluatedParentFeatureName);
                Helpers2.AddVSElementToVSFeature(dte, project, elementFolder, evaluatedParentFeatureName);
            }
            else
            {
                //no featurename is given, we add the element to the package
                //Trace.WriteLine("AddElementsDefinitionAction evaluatedParentFeatureName=null");
                Helpers2.AddVSElementToVSPackage(dte, project, elementFolder);
            }

            CreatedElementFolder = elementFolder;
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