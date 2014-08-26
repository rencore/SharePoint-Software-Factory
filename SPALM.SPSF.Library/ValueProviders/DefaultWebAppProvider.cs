using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.ComponentModel;
using EnvDTE;

namespace SPALM.SPSF.Library.ValueProviders
{
    public class DefaultWebAppProvider : ValueProvider
    {
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            if (currentValue != null)
            {
                newValue = null;
                return false;
            }

            try
            {
                //newValue = Helpers.GetDefaultIISWebApp();
                newValue = "http://" + Environment.MachineName.ToLower();
                return true;
            }
            catch (Exception)
            {
            }

            newValue = "";
            return true;
        }
    }
}
