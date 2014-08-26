using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using EnvDTE;

namespace SPALM.SPSF.Library
{
  public class XmlNodeHandler
  {
    private string _name = "";
    private string _value = "";
    private string _group = "";
    private string _description = "";
    private string fixedGroupName = "";
    private Dictionary<string, CustomResXResourceReader> dictionary = null;

    public XmlNodeHandler(string name, string value, string group, string description)
    {
      _name = name;
      _value = value;
      _group = group;
      _description = description;
    }

    public virtual NameValueItem[] GetNameValueItems(XmlNode node, XmlDocument resDocument, ProjectItem item)
    {
        NameValueItem nvitem = GetNameValueItem(node, resDocument, item);
        if (nvitem != null)
        {
            return new NameValueItem[] { nvitem };
        }
        return new NameValueItem[0];
    }

    public virtual NameValueItem GetNameValueItem(XmlNode node, XmlDocument resDocument, ProjectItem item)
    {
      try
      {
        string title = "";
        string value = "";
        string group = "";
        string description = "";

        if (_name == "")
        {
          //keine attribute angegeben, wir können nur den innerText nehmen
          title = node.InnerText;
          value = node.InnerText;
        }
        else
        {
            title = GetNodeAttribute(node,_name);
            value = GetNodeAttribute(node,_value);


        if (fixedGroupName != "")
        {
            group = fixedGroupName;
        }
        else if (_group != "")
        {
            group = GetNodeAttribute(node,_group);
        }

          
          if (_description != "")
          {
              description = GetNodeAttribute(node,_description);
          }
        }

        //wenn resourcen drin sind
        if (title.StartsWith("$Resources:"))
        {
          title = GetResourceValue(title);
        }
        if (description.StartsWith("$Resources:"))
        {
          description = GetResourceValue(description);
        }
        if (group.StartsWith("$Resources:"))
        {
          group = GetResourceValue(group);
        }

        NameValueItem nvitem = new NameValueItem();
        nvitem.ItemType = node.Name;
        nvitem.Name = title;
        nvitem.Value = value;
        nvitem.Group = group;
        nvitem.Description = description;

        return nvitem;
      }
      catch (Exception)
      {
      }
      return null;
    }

    private static string GetNodeAttribute(XmlNode node, string attr)
    {
        if (node.Attributes[attr] != null)
        {
            return node.Attributes[attr].Value;
        }
        // maybe casing doesn't fit anymore
        foreach (XmlAttribute attribute in node.Attributes)
        {
            if (attribute.Name.Equals(attr, StringComparison.InvariantCultureIgnoreCase))
            {
                return attribute.Value;
            }
        }
        return "";
    }

    private string GetResourceValue(string title)
    {
      if (!title.StartsWith("$Resources:"))
      {
        return title;
      }

      if (dictionary != null)
      {
        return Helpers.GetResourceString(title, "", dictionary);
      }
      return title;
    }

    internal void SetGroupName(string groupName)
    {
      this.fixedGroupName = groupName;
    }

    internal void SetGlobalResourcesDictionary(Dictionary<string, CustomResXResourceReader> dictionary)
    {
      this.dictionary = dictionary;
    }
  }    
}
