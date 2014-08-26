using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.Common;

namespace SPALM.SPSF.Library.ValueProviders
{
    public class DefaultNameValueCollectionProvider : ValueProvider
    {
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            if (currentValue != null)
            {                
                newValue = null;
                return false;
            }
            
            newValue = new NameValueItem[0];
            return true;
            
        }
    }
}
