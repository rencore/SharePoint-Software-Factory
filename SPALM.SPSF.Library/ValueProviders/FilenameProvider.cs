using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common;
using System.ComponentModel.Design;
using System.IO;

namespace SPALM.SPSF.Library.ValueProviders
{
  /// <summary>
  /// extracts the filename from a full filepath, optionally changes the name but leaves the extension the same
  /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class FilenameProvider : ValueProvider, IAttributesConfigurable
    {
      private string RecipeArgument = "";
      private string FilePath = "";
      private string ReplaceNameWithArgument = "";
      
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
        
        //in RecipeArgument we find the Argument, which holds the featurename
        try
        {
          string replacename = "";
          IDictionaryService dictionaryservice = this.GetService(typeof(IDictionaryService)) as IDictionaryService;
          FilePath = dictionaryservice.GetValue(RecipeArgument).ToString();
          if (ReplaceNameWithArgument != "")
          {
            replacename = dictionaryservice.GetValue(ReplaceNameWithArgument).ToString();
          }
          if (replacename != "")
          {
            string extension = Path.GetExtension(FilePath);
            string filename = Path.GetFileName(FilePath);

            newValue = filename + "." + extension;
            return true;
          }

          newValue = Path.GetFileName(FilePath);
          return true;
        }
        catch (Exception)
        {
        }

				newValue = Path.GetFileName(FilePath);
				return true;
      }

      #region IAttributesConfigurable Members

      public void Configure(System.Collections.Specialized.StringDictionary attributes)
      {
        if (attributes["RecipeArgument"] != null)
        {
          RecipeArgument = attributes["RecipeArgument"];
        }
        if (attributes["ReplaceNameWithArgument"] != null)
        {
          ReplaceNameWithArgument = attributes["ReplaceNameWithArgument"];
        }        
      }

      #endregion
    }
}
