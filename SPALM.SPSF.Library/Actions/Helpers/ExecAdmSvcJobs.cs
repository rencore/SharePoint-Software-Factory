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
using System.ComponentModel;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    [ServiceDependency(typeof(DTE))]
    public class ExecAdmSvcJobs : ConfigurableAction
    {
      DTE dte = null;

      public override void Execute()
      {
        dte = GetService<DTE>(true);
        string STSAdmPath = Helpers.GetSharePointHive() + "\\BIN\\stsadm.exe";
        ProcessExec exec = new ProcessExec(dte, STSAdmPath, "-o execadmsvcjobs", "Running stsadm -o execadmsvcjobs");
        exec.RunProcess();
      }

      public override void Undo()
      {

      }
  }    
}