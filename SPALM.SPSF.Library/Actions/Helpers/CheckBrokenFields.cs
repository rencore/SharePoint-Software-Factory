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
	public class CheckBrokenFields : ConfigurableAction
  {
			private string _SiteCollectionUrl;

			[Input(Required = true)]
			public string SiteCollectionUrl
			{
				get { return _SiteCollectionUrl; }
				set { _SiteCollectionUrl = value; }
			}

      public override void Execute()
      {
        DTE dte = GetService<DTE>(true);
				DeploymentHelpers.CheckBrokenFields(dte, _SiteCollectionUrl);
      }

      public override void Undo()
      {

      }
	}   
}