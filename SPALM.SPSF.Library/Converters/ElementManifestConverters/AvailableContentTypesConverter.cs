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
    public class AvailableContentTypesConverter : StringConverter
  {
      private bool acceptFreeText = false;
      private List<NameValueItem> list = null;

      public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
      {
        Cursor.Current = Cursors.WaitCursor;

        list = new List<NameValueItem>();

        if (context != null)
        {
            DTE dte = (DTE)context.GetService(typeof(_DTE));
            Helpers2.AddInternalItems(dte, true, list, "/ns:Elements/ns:ContentType", "http://schemas.microsoft.com/sharepoint/", new XmlNodeHandler("Name", "ID", "Group", "Description"));
        }

        Cursor.Current = Cursors.Default;

        return new StandardValuesCollection(list.ToArray());
      }

      public override bool IsValid(ITypeDescriptorContext context, object value)
      {
        return true;
      }

      public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
      {
        if (sourceType == typeof(NameValueItem))
        {
          return true;
        }
        return base.CanConvertFrom(context, sourceType);
      }

      public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
      {
        if (destinationType == typeof(System.String))
        {
          return true;
        }
        return base.CanConvertTo(context, destinationType);
      }

      public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
      {
        if (context.PropertyDescriptor.PropertyType == typeof(System.String))
        {
          return value.ToString();
        }
        return base.ConvertFrom(context, culture, value);
      }

      public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
      {
        if (destinationType == typeof(System.String))
        {
          return value.ToString();
        }
        return base.ConvertTo(context, culture, value, destinationType);
      }

      public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
      {
        if (acceptFreeText)
        {
          return false;
        }
        return true;
      }

      public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
      {
        return true;
      }
    }  
}
