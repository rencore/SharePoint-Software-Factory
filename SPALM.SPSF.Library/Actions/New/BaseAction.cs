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

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class BaseAction :  Microsoft.Practices.RecipeFramework.VisualStudio.Library.DynamicInputAction
    {
        internal bool DeploymentTypeIsSet = false;
        internal bool DeploymentPathIsSet = false;

        public BaseAction()
        {
          AdditionalCondition = true;
        }

        protected string GetBasePath()
        {
            return base.GetService<IConfigurationService>(true).BasePath;
        }

        protected string GetTemplateBasePath()
        {
            return new DirectoryInfo(this.GetBasePath() + @"\Templates").FullName;
        }

        [Input(Required = false)]
        public string ProjectName { get; set; }

        [Input(Required = false)]
        public Project Project { get; set; }
       
        protected Project GetTargetProject(DTE dte)
        {
            if (Project != null)
            {
                return Project;
            }
            if (!string.IsNullOrEmpty(ProjectName))
            {
                Project destinationProject = Helpers.GetProjectByName(dte, ProjectName);
                if (destinationProject != null)
                {
                    return destinationProject;
                }
            }
            return Helpers2.GetSelectedProject(dte);
        }

        /// <summary>
        /// Type of file (e.g. RootFile, TemplateFile, Image, LayoutsFile etc.
        /// </summary>
        [Input(Required = true)]
        public SPFileType DeploymentType
        {
            get
            {
                return this.deploymentType;
            }
            set
            {
                this.deploymentType = value;
                DeploymentTypeIsSet = true;
            }
        }
        private SPFileType deploymentType = SPFileType.RootFile;

        /// <summary>
        /// Path within the deployment location, if empty the file is placed in the root of the deployment location
        /// </summary>
        [Input(Required = false)]
        public string DeploymentPath
        {
          get
          {
            return this.deploymentPath;
          }
          set
          {
            this.deploymentPath = value;
            DeploymentPathIsSet = true;
          }
        }
        private string deploymentPath = "";

        /// Targetfilename, if empty the source filename is used
        /// </summary>
        [Input(Required = false)]
        public string TargetFileName { get; set; }

        /// Parent project item; if empty the current project is used
        /// </summary>
        [Input(Required = false)]
        public ProjectItem ParentProjectItem { get; set; }

        /// Parent project item; if empty the current project is used
        /// </summary>
        [Input(Required = false)]
        public ProjectItem ParentProjectFolder { get; set; }

        /// newly created projectItem to allow others to add elements as childs to this file
        /// </summary>
        [Output]
        public ProjectItem CreatedProjectItem { get; set; }

        /// parent folder of the new created projectItem to allow others to add elements to this folder
        /// </summary>
        [Output]
        public ProjectItem CreatedProjectFolder { get; set; }

        [Input(Required = false)]
        public bool ExcludeCondition { get; set; }

        [Input(Required = false)]
        public bool AdditionalCondition { get; set; }

        public override void Execute()
        {
        }

        public override void Undo()
        {
            //TODO: delete created project item
        }

        /// <summary>
        /// takes a parameter like "$(FeatureName)$(NameSeparator)$(ID)" and returns the current value "FeatureX_34"
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        internal string EvaluateParameterAsString(DTE dte, string parameterName)
        {
            try
            {
                IDictionaryService serviceToAdapt = (IDictionaryService)this.GetService(typeof(IDictionaryService));
                ServiceAdapterDictionary serviceAdaptor = new ServiceAdapterDictionary(serviceToAdapt);
                ExpressionEvaluationService2 expressionService2 = new ExpressionEvaluationService2();
                string evaluatedValue = expressionService2.Evaluate(parameterName, serviceAdaptor).ToString();
                if (string.IsNullOrEmpty(evaluatedValue))
                {
                    return "";
                }
                return evaluatedValue;
            }
            catch (NullReferenceException)
            {
            }
            return "";
        }
    }   
}