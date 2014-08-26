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
    /// Created a project dependency
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class MakeProjectDependentOnOther : ConfigurableAction
    {

        [Input(Required = true)]
        public string DependingProject { get; set; }

        [Input(Required = true)]
        public string SourceProject { get; set; }


        public override void Execute()
        {
            DTE dte = base.GetService<DTE>(true);

            Project _dependingProject = Helpers.GetProjectByName(dte, this.DependingProject);
            Project _sourceProject = Helpers.GetProjectByName(dte, this.SourceProject);

            if (_dependingProject == null || _sourceProject == null)
            {
                return;
            }

            try
            {
                Helpers2.AddBuildDependency(dte, _dependingProject, _sourceProject);
            }
            catch (Exception ex)
            {
                Helpers.LogMessage(dte, this, ex.ToString());

                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Undo()
        {

        }
    }
}