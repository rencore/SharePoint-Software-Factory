using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.ComponentModel;
using EnvDTE;

namespace SPALM.SPSF.Library.ValueProviders
{
  public class DefaultCompanyNameProvider : ValueProvider
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
        string org = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion", "RegisteredOrganization", "");
        if (!string.IsNullOrEmpty(org))
        {
            newValue = org;
            return true;
        }
      }
      catch(Exception)
      {
      }

      newValue = "MyCompany";
      return true;
    }
  }
}
