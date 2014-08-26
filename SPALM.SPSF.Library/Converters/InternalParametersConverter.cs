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

namespace SPALM.SPSF.Library.Converters
{
  /// <summary>
  /// finds items in the project which could contain the xpath (.xml files)
  /// </summary>
  [ServiceDependency(typeof(DTE))]
  public class InternalParametersConverter : StringConverter, IAttributesConfigurable
  {
      private string xPath = "";
      private string nameSpace = ""; //optional namespace for the xpath
      private bool acceptFreeText = false;
      private List<NameValueItem> list = null;

      public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
      {
        if (list != null)
        {
          return new StandardValuesCollection(list.ToArray());
        }
        
        list = new List<NameValueItem>();

        if (context != null)
        {
          _DTE service = (_DTE)context.GetService(typeof(_DTE));

          Project project = Helpers.GetSelectedProject(service.DTE);
          
          XmlNodeHandler handler = new XmlNodeHandler("", "", "", "");

          //find all items in project with .xml extension
          Helpers.NavigateProjectItems(project.ProjectItems, list, xPath, nameSpace, handler, ".xml");
        }

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

      #region IAttributesConfigurable Members

      public void Configure(System.Collections.Specialized.StringDictionary attributes)
      {
        if (!attributes.ContainsKey("XPath"))
        {
          throw new ArgumentException("Attribute XPath is missing in Converter");
        }
        this.xPath = attributes["XPath"];

        if (attributes.ContainsKey("Namespace"))
        {
          this.nameSpace = attributes["Namespace"];
        }
        if (attributes.ContainsKey("AcceptFreeText"))
        {
          try
          {
            this.acceptFreeText = Boolean.Parse(attributes["AcceptFreeText"]);
          }
          catch (Exception)
          {
          }
        }
       

      }

      #endregion
    }  
}
