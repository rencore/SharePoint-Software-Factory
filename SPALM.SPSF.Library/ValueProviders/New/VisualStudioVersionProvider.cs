using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.ComponentModel;
using EnvDTE;
using SPALM.SPSF.Library.Actions;

namespace SPALM.SPSF.Library.ValueProviders
{
    /// <summary>
    /// Provides the version of Visual Studio
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class VisualStudioVersionProvider : ValueProvider
    {
        public override bool OnBeforeActions(object currentValue, out object newValue)
        {
            return SetValue(currentValue, out newValue);
        }

        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            return SetValue(currentValue, out newValue);
        }

        private bool SetValue(object currentValue, out object newValue)
        {
            EnvDTE.DTE dte = this.GetService<EnvDTE.DTE>(true);
            
            newValue = dte.Version;
            return true;
        }  
    }
}
