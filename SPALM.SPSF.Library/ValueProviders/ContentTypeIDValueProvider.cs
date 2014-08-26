using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using System.ComponentModel.Design;

namespace SPALM.SPSF.Library.ValueProviders
{
    public class ContentTypeIDValueProvider : ValueProvider
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
            string guid = Guid.NewGuid().ToString();
            guid = guid.ToUpper();
            guid = guid.Replace("-","");
            newValue = guid.ToString();
            return true;
        }
    }
}
