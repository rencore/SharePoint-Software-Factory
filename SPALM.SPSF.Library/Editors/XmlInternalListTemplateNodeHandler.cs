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
  public class XmlInternalListTemplateNodeHandler : XmlNodeHandler
  {
    public XmlInternalListTemplateNodeHandler(string name, string value, string group, string description)
      : base(name, value, group, description)
    {
    }

    public override NameValueItem GetNameValueItem(XmlNode node, XmlDocument resdoc, ProjectItem item)
    {
      NameValueItem res = base.GetNameValueItem(node, resdoc, item);
      if (res != null)
      {
        //set the featureid of the listtemplate in field group (important for list instances
        //find the parent feature of item (in the item the listtemplate is defined
        string featureId = Helpers2.GetFeatureIdOfProjectItem(item);
        res.Group = featureId;
      }
      return res;
    }
  }    
}
