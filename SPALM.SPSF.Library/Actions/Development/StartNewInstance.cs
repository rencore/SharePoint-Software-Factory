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
    public class StartNewInstance : ConfigurableAction
    {
        public override void Execute()
        {
            DTE dte = GetService<DTE>(true);
            Helpers.StartNewInstance(dte);
        }

        /// <summary>
        /// Removes the previously added reference, if it was created
        /// </summary>
        public override void Undo()
        {
        }
    }
}