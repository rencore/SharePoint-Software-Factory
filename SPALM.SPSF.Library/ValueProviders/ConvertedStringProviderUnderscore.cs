using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using System.ComponentModel.Design;
using Microsoft.Practices.Common;

namespace SPALM.SPSF.Library.ValueProviders
{
    //removes all special characters from a string so that the returned string could be used e.g. as a resource key
    [ServiceDependency(typeof(DTE))]
  public class ConvertedStringProviderUnderscore : ValueProvider, IAttributesConfigurable
    {
        private string RecipeArgument = "";

        public override bool OnBeforeActions(object currentValue, out object newValue)
        {
          if (currentValue != null)
          {
            // Do not assign a new value, and return false to flag that 
            // we don't want the current value to be changed.
            newValue = null;
            return false;
          }

            try
            {
                IDictionaryService dictionaryService = (IDictionaryService)ServiceHelper.GetService(this, typeof(IDictionaryService));
                string argumentvalue = dictionaryService.GetValue(RecipeArgument).ToString();
                argumentvalue = argumentvalue.Replace(" ","");
                argumentvalue = argumentvalue.Replace("-","");
                argumentvalue = argumentvalue.Replace("(","");
                argumentvalue = argumentvalue.Replace(")","");
                argumentvalue = argumentvalue.Replace("&","");
                argumentvalue = argumentvalue.Replace("_", "");
                argumentvalue = argumentvalue.Replace(".", "_");
                newValue = argumentvalue;
                return true;
            }
            catch (Exception)
            {
            }

            newValue = null;
            return false;
        }

        #region IAttributesConfigurable Members

        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes["RecipeArgument"] != null)
            {
                RecipeArgument = attributes["RecipeArgument"];
            }
        }

        #endregion
    }
}
