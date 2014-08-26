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

namespace SPALM.SPSF.Library.ValueProviders
{
    [ServiceDependency(typeof(DTE))]
    public class SafeProjectNameProvider : ValueProvider
    {
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            if (currentValue != null)
            {
                newValue = null;
                return false;
            }

            DTE dte = (DTE)this.GetService(typeof(DTE));
            string resourcefilename = "";
            Project project = Helpers.GetSelectedProject(dte);
            if (project != null)
            {
                resourcefilename = Helpers.GetSaveApplicationName(dte) + SPSFConstants.NameSeparator + Helpers.GetSaveProjectName(project);
                newValue = resourcefilename;
                return true;
            }

            newValue = "Resources.resx";
            return true;
        }
    }
}

