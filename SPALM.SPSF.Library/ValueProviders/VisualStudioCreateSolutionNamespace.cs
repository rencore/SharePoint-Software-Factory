using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using System.ComponentModel.Design;

namespace SPALM.SPSF.Library.ValueProviders
{
  public class VisualStudioCreateSolutionNamespace : ValueProvider
  {
    public override bool OnBeginRecipe(object currentValue, out object newValue)
    {
      if (currentValue != null)
      {
        // Do not assign a new value, and return false to flag that 
        // we don't want the current value to be changed.
        newValue = null;
        return false;
      }

      IDictionaryService dictionaryService = (IDictionaryService)this.GetService(typeof(IDictionaryService));
      try
      {
        string solutionName = dictionaryService.GetValue("SolutionName").ToString();
        if (solutionName != "")
        {
          if (solutionName.Contains("."))
          {
            //take the last element 
            solutionName = solutionName.Substring(0, solutionName.LastIndexOf("."));
          }
          newValue = solutionName;
          return true;
        }
      }
      catch (Exception)
      {
      }

      newValue = "DefaultNamespace";
      return true;
    }
  }
}
