using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;

namespace SPALM.SPSF.Library.ValueProviders
{
    [ServiceDependency(typeof(DTE))]
    public class CurrentItemProvider : ValueProvider
    {
        // Methods
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            DTE service = (DTE)this.GetService(typeof(DTE));
            newValue = service.SelectedItems.Item(1).ProjectItem;
            return true;
        }
    }


}
