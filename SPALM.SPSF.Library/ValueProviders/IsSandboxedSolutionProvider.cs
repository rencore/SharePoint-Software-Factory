using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;

namespace SPALM.SPSF.Library.ValueProviders
{
    [ServiceDependency(typeof(DTE))]
    public class IsSandboxedSolutionProvider : ValueProvider
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
            Project project = Helpers.GetSelectedProject(service);
            if (project != null)
            {
                newValue = Helpers.GetIsSandboxedSolution(project);
                return true;
            }
            newValue = false;
            return true;
        }
    }


}
