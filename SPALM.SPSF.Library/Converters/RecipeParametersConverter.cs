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
  /// reads RecipeParameters.xml and returns the found parameters als NameValueItem list
  /// </summary>
  [ServiceDependency(typeof(DTE))]
  public class RecipeParametersConverter : StringConverter, IAttributesConfigurable
  {
      private string xPath = "";
      private bool acceptFreeText = false;
      private List<NameValueItem> list = null;

      public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
      {
        if(list != null)
        {
          return new StandardValuesCollection(list.ToArray());
        }

				
        list = new List<NameValueItem>();

        if (context != null)
        {
          DTE service = (DTE)context.GetService(typeof(DTE));

					string sharePointVersion = Helpers.GetSharePointVersion(service);
					string recipeParameterFile = "RecipeParameters" + sharePointVersion + ".xml";

          IConfigurationService configService = (IConfigurationService)context.GetService(typeof(IConfigurationService));
          string basepath = configService.BasePath;
					string configfilename = Path.Combine(basepath, recipeParameterFile);
          if (File.Exists(configfilename))
          {
            XmlDocument doc = new XmlDocument();
            doc.Load(configfilename);

            foreach (XmlNode node in doc.SelectNodes(xPath))
            {
              NameValueItem nvitem = GetNameValueItem(node);
              if (nvitem != null)
              {
                list.Add(nvitem);
              }
            }            
          }
        }

        return new StandardValuesCollection(list.ToArray());
      }

      private NameValueItem GetNameValueItem(XmlNode node)
      {
        string Name = "";
        string Value = "";
        string Description = "";
        string Group = "";

        if(node.Attributes["Value"] != null)
        {
          Value = node.Attributes["Value"].Value;
        }
        if(node.Attributes["Name"] != null)
        {
          Name = node.Attributes["Name"].Value;
        }
        else
        {
          Name = node.Attributes["Value"].Value;
        }
        if(node.Attributes["Description"] != null)
        {
          Description = node.Attributes["Description"].Value;
        }
        if(node.Attributes["Group"] != null)
        {
          Group = node.Attributes["Group"].Value;
        }

        NameValueItem item = new NameValueItem();
        item.ItemType = node.Name;
        item.Name = Name;
        item.Description = Description;
        item.Value = Value;
        item.Group = Value;
        return item;
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
