using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using System.ComponentModel;
using System.Xml;
using System.IO;
using SPALM.SPSF.Library.Editors;

namespace SPALM.SPSF.Library
{
  /// <summary>
  /// Provides a list of local installed list templates
  /// </summary>
  public class CustomActionUrlParameterEditor : TreeViewEditor
  {
    public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
    {
      List<NameValueItem> list = new List<NameValueItem>();

      AddItem("ItemId","{ItemId}","ID (Integer value) of the item being acted upon (if applicable).", list);
      AddItem("ItemUrl","{ItemUrl}","URL of the item being acted upon (only works in document libraries).", list);
      AddItem("ListId","{ListId}","GUID of the list", list);
      AddItem("SiteUrl","{SiteUrl}","absolute URL of the current web site", list);
      AddItem("RecurrenceId","{RecurrenceId}","Recurrence index (not supported in the context menu of list items). ", list);

      return list;
    }

    private void AddItem(string Name, string Value, string Description, List<NameValueItem> list)
    {
      NameValueItem item = new NameValueItem();
      item.ItemType = "CustomAction";
      item.Name = Name;
      item.Description = Description;
      item.Value = Value;
      list.Add(item); 
    }
  }
}
