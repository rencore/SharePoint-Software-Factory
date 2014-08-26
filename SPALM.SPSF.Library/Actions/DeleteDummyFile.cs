#region Using Directives

using System;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.Services;
using System.IO;
using Microsoft.Practices.Common;
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using System.Collections.Generic;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Library.Templates.Actions;
using System.Text.RegularExpressions;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.Common.Services;
using System.Xml;
using System.Windows.Forms;
using Microsoft.VisualStudio.SharePoint;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    [ServiceDependency(typeof(DTE)), ServiceDependency(typeof(ITypeResolutionService))]
  public class DeleteDummyFile : ConfigurableAction
    {
      private string _Filename = "";
      private ProjectItem _ParentItem = null;

      [Input(Required = false)]
      public string Filename
      {
        get { return _Filename; }
        set { _Filename = value; }
      }

      [Input(Required = false)]
      public ProjectItem ParentItem
      {
        get { return _ParentItem; }
        set { _ParentItem = value; }
      }
      
      public override void Execute()
      {
        EnvDTE.DTE service = this.GetService<EnvDTE.DTE>(true);

        Helpers2.DeleteDummyFile(service, ParentItem, true);        
      }

      public override void Undo()
      {
          // nothing to do.
      }      
    }
}
