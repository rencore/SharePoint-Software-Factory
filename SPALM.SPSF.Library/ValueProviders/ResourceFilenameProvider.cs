using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common;
using System.ComponentModel.Design;

namespace SPALM.SPSF.Library.ValueProviders
{
  [ServiceDependency(typeof(DTE))]
  public class ResourceFilenameProvider : ValueProvider
  {
    public override bool OnBeforeActions(object currentValue, out object newValue)
    {
      if (currentValue != null)
      {
        // Do not assign a new value, and return false to flag that 
        // we don't want the current value to be changed.
        newValue = null;
        return false;
      }

      DTE dte = (DTE)this.GetService(typeof(DTE));
      string resourcefilename = "";
      Project project = Helpers.GetSelectedProject(dte);
      if (project != null)
      {
        resourcefilename = Helpers.GetApplicationName(dte) + "." + project.Name;
        newValue = resourcefilename;
        return true;
      }

      newValue = "Resources.resx";
      return true;
    }
  }
}
