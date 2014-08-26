using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Security.Cryptography;
using System.Collections;
using System.Diagnostics;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.Common;
using Microsoft.Practices.ComponentModel;
using System.Windows.Forms;

namespace SPALM.SPSF.Library.ValueProviders
{
  /// <summary>
  /// checks all projects and checks what sharepoint version is used in the projects
  /// </summary>
    [ServiceDependency(typeof(DTE))]
  public class CurrentSharePointVersionValueProvider : ValueProvider
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

            DTE dte = (DTE)this.GetService(typeof(DTE));
            
            newValue = Helpers.GetApplicationConfigValue(dte, "SharePointVersion", "15");
            return true;
        }
    }
}

