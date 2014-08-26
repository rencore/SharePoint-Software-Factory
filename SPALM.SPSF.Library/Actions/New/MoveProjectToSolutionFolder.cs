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
    public class MoveProjectToSolutionFolder : ConfigurableAction
    {
        public MoveProjectToSolutionFolder()
        {

        }

        /// <summary>
        /// Contains the name of the solution folder
        /// </summary>
        [Input(Required = true)]
        public string SolutionFolder { get; set; }

        /// <summary>
        /// Contains the name of the project
        /// </summary>
        [Input(Required = true)]
        public string ProjectName { get; set; }

        public override void Execute()
        {
            EnvDTE.DTE dte = this.GetService<EnvDTE.DTE>(true);

            //Helpers2.MoveProjectToSolutionFolder(dte, ProjectName, SolutionFolder);
            
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