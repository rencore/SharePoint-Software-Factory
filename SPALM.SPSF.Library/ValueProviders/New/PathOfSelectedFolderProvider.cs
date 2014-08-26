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
  /// returns the name of the current selected folder, if a file is selected, the parent folder name is returned
  /// used e.g. to return the name of a contenttype because this name is only be used as folder name
  /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class PathOfSelectedFolderProvider : ValueProvider
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
                        newValue = parentItem.Properties.Item("FullPath").Value;
                        return true;
                    }
                }
                else if (selectedItem.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFolder)
                {
                    newValue = selectedItem.Properties.Item("FullPath").Value;
                    return true;
                }
            }
        }
        //string projectId = currentProject.

        Project currentProject = Helpers.GetSelectedProject(dte);
        newValue = new FileInfo(currentProject.FullName).Directory.FullName;
        return true;
      }      
    }
}
