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
    /// Adds a template file as a child to the given project item
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class AddFileToItemAction : BaseItemAction
    {
      public AddFileToItemAction()
        : base()
      {
      }

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

            base.Execute();            
        }      
    }    
}