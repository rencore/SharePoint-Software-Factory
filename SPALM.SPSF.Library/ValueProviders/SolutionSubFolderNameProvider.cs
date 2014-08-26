using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common;
using System.ComponentModel.Design;

namespace SPALM.SPSF.Library.ValueProviders
{
    [ServiceDependency(typeof(DTE))]
    public class SolutionSubFolderNameProvider : ValueProvider
    {
      public override bool OnBeforeActions(object currentValue, out object newValue)
      {
        if (currentValue != null)
        {
          // Do not assign a new value, and return false to flag that 
          // we don't want the current value to be changed.
          newValue = null;
          return false;
        }

        //is there a separate Project for resources?
        EnvDTE.DTE dte = this.GetService<EnvDTE.DTE>(true);
        
        newValue = Helpers.GetSaveApplicationName(dte);
        return true;
      }
    }
}
