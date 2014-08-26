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
using System.ComponentModel;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// Adds a new SharePoint feature to the project and registers the element in the parent package
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class AddFeatureAction : BaseTemplateAction
    {
      public AddFeatureAction()
        : base()
      {
      }
        /// <summary>
        /// Required for Visual Studio 2010 project template 
        /// </summary>
        [Input(Required = true)]
        public string SPDataTemplate
        {
            get
            {
                return this.sPDataTemplate;
            }
            set
            {
                this.sPDataTemplate = value;
            }
        }
        private string sPDataTemplate = "";

        /// <summary>
        /// Required feature name, needed to create all necessary files and folders with this name
        /// </summary>
        [Input(Required = true)]
        public string FeatureName
        {
            get
            {
                return this.featureName;
            }
            set
            {
                this.featureName = value;
            }
        }
        private string featureName = "";

        //[Output]
        //public ProjectItem CreatedProjectItem { get; set; }

        //[Output]
        //public ProjectItem CreatedProjectFolder { get; set; }

        /// <summary>
        /// returns the created feature projectitem (folder of the created feature)
        /// </summary>
        [Output]
        public ProjectItem FeatureProjectFolder
        {
            get
            {
                return this.featureProjectFolder;
            }
            set
            {
                this.featureProjectFolder = value;
            }
        }
        private ProjectItem featureProjectFolder = null;

        public override void Execute()
        {
            DTE dte = (DTE)this.GetService(typeof(DTE));
            Project project = this.GetTargetProject(dte);

            //Microsoft.Practices.RecipeFramework.GuidancePackage p = (Microsoft.Practices.RecipeFramework.GuidancePackage)GetService(typeof(Microsoft.Practices.RecipeFramework.Services.IExecutionService));
            //ISite x = p.Site;

            //1. get correct parameters ("$(FeatureName)" as "FeatureX")       
            string evaluatedFeatureName = EvaluateParameterAsString(dte, featureName);

            //2. create the Feature
            if (Helpers2.IsSharePointVSTemplate(dte, project))
            {
                AddFeatureToVSTemplate(dte, project, evaluatedFeatureName);
            }
        }

        /// <summary>
        /// Adds a new feature
        /// </summary>
        private void AddFeatureToVSTemplate(DTE dte, Project project, string finalFeatureName)
        {
            string targetFolder = @"Features\" + finalFeatureName;
            string targetFilename1 = finalFeatureName + ".feature";
            string targetFilename2 = finalFeatureName + ".Template.xml";

            //1. add folder with featurename to folder /Features and place FeatureName.feature in the folder.
            //2. add FeatureName.Template.xml in the same folder
            string featureContent = GenerateContent(this.sPDataTemplate, targetFilename1);
            string featureTemplateContent = GenerateContent(this.TemplateFileName, targetFilename2);

            CreatedProjectItem = Helpers2.AddFileToProject(dte, project, targetFolder, targetFilename1, featureContent, true, false);
            //CreatedProjectFolder = CreatedProjectItem.Collection.Parent as ProjectItem;
            Helpers2.AddFileToProject(dte, project, targetFolder, targetFilename2, featureTemplateContent, true, false);

            CreatedProjectFolder = Helpers2.AddVSFeatureToVSPackage(dte, project, finalFeatureName);
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