using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;

namespace SPALM.SPSF.Library.ValueProviders
{
    public class GuidValueProvider : ValueProvider
    {
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            if (currentValue != null)
            {
                newValue = null;
                return false;
            }
            newValue = Guid.NewGuid().ToString().ToUpper();
            return true;
        }
    }
}
