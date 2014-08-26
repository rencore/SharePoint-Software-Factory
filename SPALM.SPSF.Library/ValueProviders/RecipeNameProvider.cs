using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework.Services;

namespace SPALM.SPSF.Library.ValueProviders
{
    public class RecipeNameProvider : ValueProvider
    {
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            if (currentValue != null)
            {
                newValue = null;
                return false;
            }

            IConfigurationService b = (IConfigurationService)GetService(typeof(IConfigurationService));
            Microsoft.Practices.RecipeFramework.Configuration.Recipe recipe = b.CurrentRecipe;

            newValue = recipe.Name;
            return true;
        }
    }
}
