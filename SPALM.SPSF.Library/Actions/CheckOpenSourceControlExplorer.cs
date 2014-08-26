#region Using Directives

using System;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using EnvDTE;
using System.Xml;

#endregion

namespace SPALM.SPSF.Library.Actions
{
  /// <summary>
  /// Adds content to an existing XML file or creates the xml file (elements.xml)
  /// </summary>
  [ServiceDependency(typeof(DTE))]
  public class CheckOpenSourceControlExplorer : ConfigurableAction
  {

    private bool messageBoxShown = false;

    public override void Execute()
    {

    }

    public override void Undo()
    {

    }

    public object GetTarget(_DTE vs)
    {
      if ((vs.SelectedItems == null) || (vs.SelectedItems.Count <= 0))
      {
        throw new InvalidOperationException("invalid");
      }
      if (vs.SelectedItems.Count != 1)
      {
        return vs.SelectedItems;
      }
      SelectedItem item = vs.SelectedItems.Item(1);
      if (item.Project != null)
      {
        return item.Project;
      }
      if (item.ProjectItem != null)
      {
        return item.ProjectItem;
      }
      if (vs.Solution.Properties.Item("Name").Value.Equals(item.Name))
      {
        return vs.Solution;
      }
      return item;
    }

    protected override void OnSited()
    {
      base.OnSited();

      bool exceptionThrown = false;
      bool sourceExplorerVisible = false;
      bool teamExplorerVisible = false;

      if (!messageBoxShown)
      {
        DTE dte = GetService(typeof(DTE)) as DTE;
        if (dte != null)
        {
          if (dte.SelectedItems.Count > 0)
          {
            try
            {
              string x = dte.SelectedItems.Item(1).Name;
            }
            catch
            {
              exceptionThrown = true;
            }
          }

          foreach (Window window in dte.Windows)
          {
            if (window.Caption == "Source Control Explorer")
            {              
              if (window.Visible)
              {
                sourceExplorerVisible = true;
              }
            }

            if (window.Caption == "Team Explorer")
            {
              if (window.Visible)
              {
                teamExplorerVisible = true;
              }
            }
          }
        }

        if (exceptionThrown && sourceExplorerVisible && teamExplorerVisible)
        {
          messageBoxShown = true;
          MessageBox.Show("Problem encountered: The Microsoft Guidance Automation Extensions have a known problem during project creation if Source Control Explorer and Team Explorer are visible and an item in Source Control Explorer is selected.\n\nYou will receive an error message 'Member not found' if you proceed.\n\nPlease close Source Control Explorer and try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
      }
    }
  }
}