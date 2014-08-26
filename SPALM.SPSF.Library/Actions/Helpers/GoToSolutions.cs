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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    [ServiceDependency(typeof(DTE))]
    public class GoToSolutions : ConfigurableAction
    {
      public override void Execute()
      {
        DTE dte = GetService<DTE>(true);

        try
        {
            Helpers.LogMessage(dte, this, "Retrieving Central Administration");
            string centralAdminUrl = new SharePointBrigdeHelper(dte).GetCentralAdministrationUrl();

            centralAdminUrl += "_admin/Solutions.aspx";

            Helpers.OpenWebPage(dte, centralAdminUrl);
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