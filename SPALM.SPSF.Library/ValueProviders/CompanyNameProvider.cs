using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.ComponentModel;
using EnvDTE;

namespace SPALM.SPSF.Library.ValueProviders
{
  [ServiceDependency(typeof(DTE))]
  public class CompanyNameProvider : ValueProvider
  {
    public override bool OnBeginRecipe(object currentValue, out object newValue)
    {
      if (currentValue != null)
      {
          newValue = null;
          return false;
      }

      DTE service = (DTE)this.GetService(typeof(DTE));

      //check if there is already a config value for "Company"
      string org = "";
      try
      {
        org = Helpers.GetApplicationConfigValue(service, "Company", "");
        if (!string.IsNullOrEmpty(org))
        {
            newValue = org;
            return true;
        }
      }
      catch(Exception)
      {
      }

      try
      {
        org = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion", "RegisteredOrganization", "");
        if (!string.IsNullOrEmpty(org))
        {
            newValue = org;
            return true;
        }
      }
      catch(Exception)
      {
      }

      newValue = "Company";
      return true;
    }
  }
}
