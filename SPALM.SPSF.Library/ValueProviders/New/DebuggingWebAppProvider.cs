using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common;
using System.ComponentModel.Design;
using System.IO;
using SPALM.SPSF.Library.Actions;

namespace SPALM.SPSF.Library.ValueProviders
{
  /// <summary>
  /// returns the id of the current selected project
  /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class DebuggingWebAppProvider : ValueProvider
    {
      public override bool OnBeforeActions(object currentValue, out object newValue)
      {
        return SetValue(currentValue, out newValue);
      }

      public override bool OnBeginRecipe(object currentValue, out object newValue)
      {
        return SetValue(currentValue, out newValue);
      }

      private bool SetValue(object currentValue, out object newValue)
      {
        if (currentValue != null)
        {
          newValue = null;
          return false;
        }

        EnvDTE.DTE dte = this.GetService<EnvDTE.DTE>(true);

        newValue = Helpers.GetDebuggingWebApp(dte, Helpers.GetBasePath());       
        return true;
      }      
    }
}
