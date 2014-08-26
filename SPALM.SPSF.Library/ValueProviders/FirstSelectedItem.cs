using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using Microsoft.Practices.ComponentModel;

namespace SPALM.SPSF.Library.ValueProviders
{
    [ServiceDependency(typeof(DTE))]
    public class FirstSelectedItem : ValueProvider
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

            DTE service = (DTE)this.GetService(typeof(DTE));
            
            if(service.SelectedItems.Count > 0)
            {
                SelectedItem item = service.SelectedItems.Item(1);
                newValue = item.ProjectItem;
                return true;
            }
            newValue = null;
            return false;
        }
    }
}
