#region Using Directives

using System;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using EnvDTE;
using System.Xml;
using EnvDTE80;
using System.Collections.Specialized;
using System.Collections;

#endregion

namespace SPALM.SPSF.Library.Actions
{
  //Recreates the SharePointConfiguration.xml with the local information on the computer (features, contenttypes etc.)
    [ServiceDependency(typeof(DTE))]
    public class LoadSharePointConfiguration : ConfigurableAction
    {        
        public override void Execute()
        {
            DTE dte = GetService<DTE>();
            Helpers.LogMessage(dte, this, "Reading local features, site templates, content types etc. into file SharePointConfiguration.xml");
            try
            {
                SharePointConfigurationHelper.CreateSharePointConfigurationFile(dte);
                Helpers.LogMessage(dte, this, "Finished");
            }
            catch (Exception ex)
            {
                Helpers.LogMessage(dte, this, ex.Message);
            }
        }
        
        /// <summary>
        /// Removes the previously added reference, if it was created
        /// </summary>
        public override void Undo()
        {
        }
    }
}