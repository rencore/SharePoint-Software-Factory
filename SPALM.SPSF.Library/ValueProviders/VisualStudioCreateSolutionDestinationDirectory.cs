using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace SPALM.SPSF.Library.ValueProviders
{
  public class VisualStudioCreateSolutionDestinationDirectory : ValueProvider
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

      string finalpath = "";
      IDictionaryService dictionaryService = (IDictionaryService)this.GetService(typeof(IDictionaryService));
      try
      {
        string solutionName = dictionaryService.GetValue("SolutionName").ToString();
        string destinationDir = dictionaryService.GetValue("destinationdirectory").ToString();
        finalpath = destinationDir + "\\" + solutionName;
      }
      catch (Exception)
      {
      }

      //checks if the length may be longer than 256 characters
      if (finalpath.Length > 110)
      {
        MessageBox.Show("The exptected path " + finalpath + " of the solution would be too long. This is not supported by Visual Studio. Please choose a different directory or shorter solution name.");
        throw new Exception("Cannot create a solution with a long filename");
      }
      
      //(Microsoft.Practices.ComponentModel.Site)
      //Site.Container.con
      //((Microsoft.Practices.RecipeFramework.Recipe)(((Microsoft.Practices.ComponentModel.Site)((Microsoft.Practices.ComponentModel.SitedComponent)(((Microsoft.Practices.RecipeFramework.Services.ReadOnlyDictionaryService)(dictionaryService)).innerService)).Site).Container)).Configuration

      newValue = finalpath;
      return true;
    }
  }
}
