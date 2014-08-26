using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using System.ComponentModel;
using SPALM.SPSF.Library.Editors;

namespace SPALM.SPSF.Library
{
  public class WebTemplateEditor : TreeViewEditor
  {
    public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
    {
      List<NameValueItem> list = new List<NameValueItem>();
      Helpers2.AddInternalItems(dte, list, "/Templates/Template/Configuration", "", new XmlWebTemplateNodeHandler());
      Helpers2.AddExternalItems(dte, list, "/SharePointConfiguration/SiteTemplates/SiteTemplate", "", new XmlNodeHandler("Title", "Id", "DisplayCategory", "Description"));
      return list;
    }
  }

  public class ListTemplateEditor : ListEditor
  {
    public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
    {
      List<NameValueItem> list = new List<NameValueItem>();
      Helpers2.AddInternalItems(dte, list, "/ns:Elements/ns:ListTemplate", "http://schemas.microsoft.com/sharepoint/", new XmlInternalListTemplateNodeHandler("DisplayName", "Type", "FeatureId", "Description"));
      Helpers2.AddExternalItems(dte, list, "/SharePointConfiguration/ListTemplates/ListTemplate", "", new XmlNodeHandler("DisplayName", "Type", "FeatureId", "Description"));
      return list;
    }
  }

  public class ListInstanceEditor : ListEditor
  {
    public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
    {
      List<NameValueItem> list = new List<NameValueItem>();
      Helpers2.AddInternalItems(dte, list, "/ns:Elements/ns:ListTemplate", "http://schemas.microsoft.com/sharepoint/", new XmlInternalListTemplateNodeHandler("DisplayName", "Type", "FeatureId", "Description"));
      Helpers2.AddExternalItems(dte, list, "/SharePointConfiguration/ListTemplates/ListTemplate", "", new XmlNodeHandler("DisplayName", "Type", "FeatureId", "Description"));
      return list;
    }
  }

  public class BCSModelEditor : ListEditor
  {
      public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
      {
          List<NameValueItem> list = new List<NameValueItem>();
          Helpers2.AddInternalItems(dte, list, "/ns:Model/ns:LobSystems/ns:LobSystem/ns:Entities/ns:Entity", "http://schemas.microsoft.com/windows/2007/BusinessDataCatalog", new BCSNodeHandler(), ".bdcm");
          return list;
      }
  }

  public class FeatureEditor : TreeViewEditor
  {
    public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
    {
      List<NameValueItem> list = new List<NameValueItem>();
      Helpers2.AddInternalItems(dte, list, "/ns:Feature", "http://schemas.microsoft.com/sharepoint/", new XmlNodeHandler("Title", "Id", "Scope", "Description"));
      Helpers2.AddInternalItems(dte, list, "/ns:feature", "http://schemas.microsoft.com/VisualStudio/2008/SharePointTools/FeatureModel", new XmlNodeHandler("Title", "Id", "Scope", "Description"), ".feature");
      Helpers2.AddExternalItems(dte, list, "/SharePointConfiguration/Features/Feature", "", new XmlNodeHandler("Title", "Id", "Scope", "Description"));
      return list;
    }
  }

  public class SiteColumnEditor : TreeViewEditor
  {
    public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
    {
      List<NameValueItem> list = new List<NameValueItem>();
      Helpers2.AddInternalItems(dte, list, "/ns:Elements/ns:Field", "http://schemas.microsoft.com/sharepoint/", new XmlNodeHandler("Name", "ID", "", "Description"));
      Helpers2.AddExternalItems(dte, list, "/SharePointConfiguration/Fields/Field", "", new XmlNodeHandler("DisplayName", "ID", "Group", "Description"));
      return list;
    }
  }

  public class ContentTypeEditor : TreeViewEditor
  {
    public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
    {
      List<NameValueItem> list = new List<NameValueItem>();
      Helpers2.AddInternalItems(dte, list, "/ns:Elements/ns:ContentType", "http://schemas.microsoft.com/sharepoint/", new XmlNodeHandler("Name", "ID", "Group", "Description"));
      Helpers2.AddExternalItems(dte, list, "/SharePointConfiguration/ContentTypes/ContentType", "", new XmlNodeHandler("Name", "ID", "Group", "Description"));
      return list;
    }
  }

  public class ContentTypeEditorInternal : ListEditor
  {
    public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
    {
      List<NameValueItem> list = new List<NameValueItem>();
      Helpers2.AddInternalItems(dte, true, list, "/ns:Elements/ns:ContentType", "http://schemas.microsoft.com/sharepoint/", new XmlNodeHandler("Name", "ID", "Group", "Description"));
      return list;
    }
  }

  public class ContentTypeGroupEditor : ListEditor
  {
    public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
    {
      List<NameValueItem> list = new List<NameValueItem>();
      Helpers2.AddInternalItems(dte, list, "/ns:Elements/ns:ContentType", "http://schemas.microsoft.com/sharepoint/", new XmlNodeHandler("Group", "Group", "", ""));
      Helpers2.AddExternalItems(dte, list, "/SharePointConfiguration/ContentTypes/ContentType", "", new XmlNodeHandler("Group", "Group", "Group", ""));
      return list;
    }
  }

  public class CustomActionSitesEditor : TreeViewEditor
  {
    public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
    {
      List<NameValueItem> list = new List<NameValueItem>();
      AddRecipeParameters(provider, list, "/RecipeParameters/CustomActionLocations/ActionGroup", "", new XmlNodeHandler("GroupID", "Location", "Location", "Description"));
      Helpers2.AddInternalItems(dte, list, "/ns:Elements/ns:CustomActionGroup", "http://schemas.microsoft.com/sharepoint/", new XmlNodeHandler("Id", "Location", "", "Title"));
      return list;
    }
  }

  public class DelegateControlEditor : ListEditor
  {
    public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
    {
      List<NameValueItem> list = new List<NameValueItem>();
      Helpers2.AddInternalItems(dte, list, "/ns:Elements/ns:Control", "http://schemas.microsoft.com/sharepoint/", new XmlNodeHandler("Id", "Id", "", ""));
      AddRecipeParameters(provider, list, "/RecipeParameters/DelegateControlIds/DelegateControlId", "", new XmlNodeHandler("Value", "Value", "", "Description"));
      return list;
    }
  }


  public class CustomActionRightsEditor : ListEditor
  {
    public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
    {
      List<NameValueItem> list = new List<NameValueItem>();
      AddRecipeParameters(provider, list, "/RecipeParameters/SPBasePermissions/SPBasePermission", "", new XmlNodeHandler("Name", "Name", "", "Description"));
      return list;
    }
  }

  public class CustomActionLocationsEditor : ListEditor
  {
    public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
    {
      List<NameValueItem> list = new List<NameValueItem>();
      AddRecipeParameters(provider, list, "/RecipeParameters/CustomActionLocations/ActionGroup", "", new XmlNodeHandler("Location", "Location", "", "Location"));
      return list;
    }
  }

  public class CustomActionGroupLocationsEditor : ListEditor
  {
    public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
    {
      List<NameValueItem> list = new List<NameValueItem>();
      AddRecipeParameters(provider, list, "/RecipeParameters/CustomActionGroupLocations/ActionGroup", "", new XmlNodeHandler("Location", "Location", "", "Location"));
      return list;
    }
  }

  public class CustomActionListToolBarsEditor : ListEditor
  {
    public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
    {
      List<NameValueItem> list = new List<NameValueItem>();
      AddRecipeParameters(provider, list, "/RecipeParameters/CustomActionListToolBars/ActionGroup", "", new XmlNodeHandler("Location", "Location", "Location", "Description"));
      return list;
    }
  }

  public class CustomActionListEditor : TreeViewEditor
  {
    public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
    {
      List<NameValueItem> list = new List<NameValueItem>();
      AddRecipeParameters(provider, list, "/RecipeParameters/CustomActionList/ActionGroup", "", new XmlNodeHandler("GroupID", "Location", "Location", "Description"));
      return list;
    }
  }

  public class HideCustomActionLocationsEditor : TreeViewEditor
  {
    public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
    {
      List<NameValueItem> list = new List<NameValueItem>();
      AddRecipeParameters(provider, list, "/RecipeParameters/HideCustomActionIDs/ActionGroup", "", new XmlNodeHandler("ID", "Location", "GroupID", "Description"));
      return list;
    }
  }

  public class RibbonGroupEditor : TreeViewEditor
  {
    public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
    {
      List<NameValueItem> list = new List<NameValueItem>();
      AddRecipeParameters(provider, list, "/RecipeParameters/RibbonGroups/RibbonGroup", "", new XmlNodeHandler("Group", "Group", "Tab", "Group"));
      return list;
    }
  }

  public class RibbonControlEditor : TreeViewEditor
  {
    public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
    {
      List<NameValueItem> list = new List<NameValueItem>();
      AddRecipeParameters(provider, list, "/RecipeParameters/RibbonControls/RibbonControl", "", new XmlNodeHandler("ID", "ID", "Group", "Tab"));
      return list;
    }
  }

  public class RibbonTabEditor : ListEditor
  {
    public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
    {
      List<NameValueItem> list = new List<NameValueItem>();
      AddRecipeParameters(provider, list, "/RecipeParameters/RibbonTabs/RibbonTab", "", new XmlNodeHandler("Tab", "Tab", "Tab", "Tab"));
      return list;
    }
  }






  public class ProgIdEditor : ListEditor
  {
    public override List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
    {
      List<NameValueItem> list = new List<NameValueItem>();
      AddRecipeParameters(provider, list, "/RecipeParameters/ProgIds/ProgId", "", new XmlNodeHandler("Value", "Value", "", "Description"));
      return list;
    }
  }
}
