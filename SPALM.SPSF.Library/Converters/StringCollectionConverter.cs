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
  /// provides an array of the given values
  /// </summary>
  [ServiceDependency(typeof(DTE))]
    public class StringCollectionConverter : StringConverter, IAttributesConfigurable
  {
      private string values = "";

      public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
      {
          if (values.Contains("|"))
          {
              //return name value collection
              List<NameValueItem> list = new List<NameValueItem>();

              char[] sep = new char[] { ';' };
              char[] sep2 = new char[] { '|' };
              string[] validValues = values.Split(sep);
              foreach (string s in validValues)
              {
                  NameValueItem item = new NameValueItem();
                  if (s.Contains("|"))
                  {
                      string[] nameValue = s.Split(sep2);
                      item.Name = nameValue[0];
                      item.Value = nameValue[0];
                      item.ItemType = nameValue[0];
                      item.Description = nameValue[1];
                  }
                  else
                  {
                      item.Name = s;
                      item.Value = s;
                      item.ItemType = s;
                      item.Description = "";
                  }
                  list.Add(item);
              }

              return new StandardValuesCollection(list.ToArray());
          }
          else
          {
              char[] sep = new char[] { ';' };
              string[] validValues = values.Split(sep);
              return new StandardValuesCollection(validValues);
          }
      }

      public override bool IsValid(ITypeDescriptorContext context, object value)
      {
          return true;
      }

      public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
      {
          return true; //no other values allowed
      }

      public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
      {
          return true;
      }

      #region IAttributesConfigurable Members

      public void Configure(System.Collections.Specialized.StringDictionary attributes)
      {
        if (!attributes.ContainsKey("Values"))
        {
            throw new ArgumentException("Attribute Values is missing in Converter");
        }
        this.values = attributes["Values"];
      }

      #endregion
    }  
}
