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
using EnvDTE80;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// Adds a new Elements definition, e.g. a contenttype and registers the element in the parent feature
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class AddProjectOutputReference : ConfigurableAction
    {
        public AddProjectOutputReference()
        {

        }

        /// <summary>
        /// Contains the name of the source project
        /// </summary>
        [Input(Required = true)]
        public string SourceProjectName { get; set; }

        /// <summary>
        /// contains the folder where the element should be added
        /// </summary>
        [Input(Required = true)]
        public ProjectItem TargetFolder { get; set; }

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
            }
        }
        private SPFileType deploymentType = SPFileType.RootFile;

        public override void Execute()
        {
            EnvDTE.DTE dte = this.GetService<EnvDTE.DTE>(true);

            string evaluatedSourceProjectName = EvaluateParameterAsString(dte, SourceProjectName);

            Project sourceProject = Helpers.GetProjectByName(dte, evaluatedSourceProjectName);
            Helpers2.AddProjectOutputReferenceToFolder(dte, sourceProject, TargetFolder, DeploymentType);
            
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

        /// <summary>
        /// 
        /// </summary>
        public override void Undo()
        {
            //TODO: Delete the created projectitems
        }
    }
}