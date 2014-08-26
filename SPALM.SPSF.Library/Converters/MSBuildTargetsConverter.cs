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
    public class MSBuildTargetsConverter : StringConverter
  {
      private bool acceptFreeText = false;
      private List<string> list = null;

      public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
      {
        Cursor.Current = Cursors.WaitCursor;

        list = new List<string>();

        if (context != null)
        {
          DTE service = (DTE)context.GetService(typeof(DTE));
          try
          {
              if (service.SelectedItems.Count > 0)
              {
                  SelectedItem item = service.SelectedItems.Item(1);
                  ProjectItem selectedItem = null;

                  if (item is ProjectItem)
                  {
                      selectedItem = item as ProjectItem;
                  }
                  else if(item.ProjectItem is ProjectItem)
                  {
                      selectedItem = item.ProjectItem as ProjectItem;
                  }

                  if(selectedItem != null)
                  {
                      string itemPath = Helpers.GetFullPathOfProjectItem(selectedItem);

                      XmlDocument msbuildDoc = new XmlDocument();
                      msbuildDoc.Load(itemPath);

                      //what is the scope of the feature
                      XmlNamespaceManager msbuildnsmgr = new XmlNamespaceManager(msbuildDoc.NameTable);
                      msbuildnsmgr.AddNamespace("ns", "http://schemas.microsoft.com/developer/msbuild/2003");

                      foreach (XmlNode targetNode in msbuildDoc.SelectNodes("/ns:Project/ns:Target", msbuildnsmgr))
                      {
                          try
                          {
                            list.Add(targetNode.Attributes["Name"].Value);
                          }
                          catch(Exception)
                          {
                          }
                      }
                  }
              }
          }
          catch (Exception)
          {
          }
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
        if (sourceType == typeof(string))
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
