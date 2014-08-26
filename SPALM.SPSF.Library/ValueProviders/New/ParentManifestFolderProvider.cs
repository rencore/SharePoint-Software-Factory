using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.Common;
using System.ComponentModel.Design;
using System.IO;
using SPALM.SPSF.Library.Actions;
using Microsoft.Practices.RecipeFramework.Services;
using System.Xml;

namespace SPALM.SPSF.Library.ValueProviders
{
      /// <summary>
      /// for manifest recipes the parent elementmanifest folder is needed, this class returns the elements.xml ProjectItem
      /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class ParentManifestFolderProvider : ValueProvider
    {
      public override bool OnBeforeActions(object currentValue, out object newValue)
      {
        return SetValue(currentValue, out newValue);
      }

      public override bool OnBeginRecipe(object currentValue, out object newValue)
      {
        return SetValue(currentValue, out newValue);
      }

      private bool SetValue(object currentValue, out object newValue)
      {
        if (currentValue != null)
        {
          newValue = null;
          return false;
        }

        EnvDTE.DTE dte = this.GetService<EnvDTE.DTE>(true);

        //first we need the xpath, which is needed by the manifest-recipe
        string manifestXpath = "";
        string manifestXpathNamespace = "";
        IConfigurationService b = (IConfigurationService)GetService(typeof(IConfigurationService));
        Microsoft.Practices.RecipeFramework.Configuration.Recipe recipe = b.CurrentRecipe;
        if (recipe.HostData.Any.Attributes["XPath"] != null)
        {
            manifestXpath = recipe.HostData.Any.Attributes["XPath"].Value;
        }
        if (recipe.HostData.Any.Attributes["XPathNamespace"] != null)
        {
            manifestXpathNamespace = recipe.HostData.Any.Attributes["XPathNamespace"].Value;
        }

        //if file is selected we check, if the xpath can be found in this item
        //if folder is selected we search in this folder for the elements.xml which contains the xpath
        if (dte.SelectedItems.Count > 0)
        {
            SelectedItem item = dte.SelectedItems.Item(1);
            ProjectItem selectedItem = null;

            if (item is ProjectItem)
            {
                selectedItem = item as ProjectItem;
            }
            else if (item.ProjectItem is ProjectItem)
            {
                selectedItem = item.ProjectItem as ProjectItem;
            }

            if (selectedItem != null)
            {
                if (selectedItem.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFile)
                {
                    if (selectedItem.Collection.Parent is ProjectItem)
                    {
                        ProjectItem parentItem = selectedItem.Collection.Parent as ProjectItem;
                        newValue = parentItem;
                        return true;
                    }
                }
                else if (selectedItem.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFolder)
                {
                    newValue = selectedItem;
                    return true;
                }
            }
        }

        newValue = null;
        return false;
      }      
    }
}
