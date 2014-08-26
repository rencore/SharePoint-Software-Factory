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
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Services;

namespace SPALM.SPSF.Library.ValueProviders
{
    [ServiceDependency(typeof(DTE))]
    public class InstalledSilverlightVersionProvider : ValueProvider
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

            bool silverlightIsInstalled = false;
            if (!silverlightIsInstalled)
            {
                throw new Exception("Silverlight SDK is not installed. Please install runtime and SDK.");
            }

            DTE service = (DTE)this.GetService(typeof(DTE));
            string sharePointVersionOfApplication = Helpers.GetSharePointVersion(service);
            newValue = false;
            return true;
        }
    }
}

