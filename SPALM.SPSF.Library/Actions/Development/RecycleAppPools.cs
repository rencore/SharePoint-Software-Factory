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
using System.DirectoryServices;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    [ServiceDependency(typeof(DTE))]
    public class RecycleAppPools : ConfigurableAction
    {
      private string _AppPoolName = "";
      
      [Input(Required = true)]
      public string AppPoolName
      {
        get { return _AppPoolName; }
        set { _AppPoolName = value; }
      }
      
      public override void Execute()
      {
        DTE dte = GetService<DTE>(true);
        Helpers.RecycleAppPool(dte, _AppPoolName);
        Helpers.LogMessage(dte, dte, "*** Recycling finished ***");
        Helpers.LogMessage(dte, dte, "");
      }

        /// <summary>
        /// Removes the previously added reference, if it was created
        /// </summary>
        public override void Undo()
        {
        }
    }    
}