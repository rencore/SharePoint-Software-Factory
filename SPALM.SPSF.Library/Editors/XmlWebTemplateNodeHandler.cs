using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using EnvDTE;

namespace SPALM.SPSF.Library
{
  /// <summary>
  /// used to read the sharePoint configuration xml. For web templates a special
  /// processing is needed.
  /// </summary>
  public class XmlWebTemplateNodeHandler : XmlNodeHandler
  {
    public XmlWebTemplateNodeHandler()
      : base("", "", "", "")
    {
    }

    public override NameValueItem GetNameValueItem(XmlNode node, XmlDocument resdoc, ProjectItem item)
    {
      try
      {
        string title = node.Attributes["Title"].Value;

        string ConfigID = node.Attributes["ID"].Value;
        string ConfigName = node.ParentNode.Attributes["Name"].Value;
        string value = ConfigName + "#" + ConfigID;

        NameValueItem nvitem = new NameValueItem();
        nvitem.ItemType = node.Name;
        nvitem.Name = title;
        nvitem.Value = value;
        return nvitem;
      }
      catch (Exception)
      {
      }
      return null;
    }
  }    
}
