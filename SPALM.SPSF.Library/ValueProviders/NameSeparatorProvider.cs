using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common;
using System.ComponentModel.Design;
using System.IO;

namespace SPALM.SPSF.Library.ValueProviders
{
  /// <summary>
  /// returns the separator used for file naming
  /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class NameSeparatorProvider : ValueProvider
    {
      public override bool OnBeforeActions(object currentValue, out object newValue)
      {
          newValue = SPSFConstants.NameSeparator;
		return true;
      }

    }
}
