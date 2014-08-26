using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using System.ComponentModel.Design;
using Microsoft.Practices.Common;

namespace SPALM.SPSF.Library.ValueProviders
{
    //takes a boolean recipeargument and returns the opposite value (needed because ConditionalCoordinator) can not evaluate "NOT"
    [ServiceDependency(typeof(DTE))]
  public class BooleanOppositeProvider : ValueProvider, IAttributesConfigurable
    {
        private string RecipeArgument = "";

        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
          
          newValue = null;
          return this.Evaluate(out newValue);
        }

        public override bool OnBeforeActions(object currentValue, out object newValue)
        {
          
          newValue = null;
          return this.Evaluate(out newValue);
        }

        private bool Evaluate(out object newValue)
        {          
          IDictionaryService dictionaryService = (IDictionaryService)ServiceHelper.GetService(this, typeof(IDictionaryService));
          try
          {
              string argumentvalue = dictionaryService.GetValue(RecipeArgument).ToString();
              bool value1 = Boolean.Parse(argumentvalue);
              newValue = !value1;
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
