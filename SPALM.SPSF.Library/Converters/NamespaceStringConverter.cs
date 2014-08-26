using System;
using System.ComponentModel;
using Microsoft.Practices.Common;
using Microsoft.Practices.ComponentModel;
using EnvDTE;
using System.Xml;
using System.ComponentModel.Design;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Practices.RecipeFramework.Services;
using System.IO;
using System.Collections.ObjectModel;
using System.Management;
using System.DirectoryServices;

namespace SPALM.SPSF.Library.Converters
{
  /// <summary>
  /// returns a list of worker processes
  /// </summary>
  [ServiceDependency(typeof(DTE))]
  public class NamespaceStringConverter : StringConverter
  {
    public NamespaceStringConverter() : base()
    {
    }

    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
      return base.CanConvertFrom(context, sourceType);
    }

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {      
      return base.CanConvertTo(context, destinationType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
    {
      if (!Helpers.IsValidNamespace(value.ToString()))
      {
        return null;
      }     
      return base.ConvertFrom(context, culture, value);
    }

    public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
    {
      return base.ConvertTo(context, culture, value, destinationType);
    }

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
      return base.GetStandardValuesExclusive(context);
    }

    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
      return base.GetStandardValuesSupported(context);
    }
      public override bool IsValid(ITypeDescriptorContext context, object value)
      {
        return true;
      }
    }  
}
