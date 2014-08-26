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
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.VisualStudio;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

#endregion

namespace SPALM.SPSF.Library
{
  public class ErrorList : IServiceProvider
  {

    private ErrorListProvider _errorListProvider;
    private EnvDTE.DTE dte;

    public ErrorList(DTE _dte)
    {
      dte = _dte;
      _errorListProvider = new ErrorListProvider(this);
      _errorListProvider.ProviderName = "SPSF Generator";
      _errorListProvider.ProviderGuid = new Guid("{051F078C-B363-4d08-B351-206E9E62BBEF}");
      _errorListProvider.Show();
    }

    public void AddError(Project project, string path, string errorText, TaskErrorCategory category, int iLine, int iColumn)
    {
      ErrorTask task;
      IVsSolution solution;
      IVsHierarchy hierarchy;

      if (project != null)
      {
          try
          {
              solution = (IVsSolution)GetService(typeof(IVsSolution));
              ErrorHandler.ThrowOnFailure(solution.GetProjectOfUniqueName(project.UniqueName, out hierarchy));

              task = new ErrorTask();
              task.ErrorCategory = category;
              task.HierarchyItem = hierarchy;
              task.Document = path;

              // VS uses indexes starting at 0 while the automation model uses indexes starting at 1
              task.Line = iLine - 1;
              task.Column = iColumn;
              task.Text = errorText;
              task.Navigate += ErrorTaskNavigate;
              if (ContainsLink(errorText))
              {
                  task.Help += new EventHandler(task_Help);
              }
              _errorListProvider.Tasks.Add(task);
          }
          catch (Exception ex)
          {
              MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
      }
    }

    bool ContainsLink(string text)
    {
      if (text == null)
      {
        throw new ArgumentNullException("text");
      }

      Match urlMatches = Regex.Match(text,
                  @"((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)");
      return urlMatches.Success;
    }

    /// <summary>  
    /// Handles the Help event of the task control.  
    /// </summary>  
    /// <param name="sender">The Task to parse for a guidance link.</param>  
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>  
    void task_Help(object sender, EventArgs e)
    {
      Microsoft.VisualStudio.Shell.Task task = sender as Microsoft.VisualStudio.Shell.Task;

      if (task == null)
      {
        throw new ArgumentException("sender");
      }

      string url = null;
      Match urlMatches = Regex.Match(task.Text,
                  @"((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)");
      if (urlMatches.Success)
      {
        url = urlMatches.Captures[0].Value;
      }

      if (url != null)
      {
        Window win = dte.Windows.Item(EnvDTE.Constants.vsWindowKindCommandWindow);
        CommandWindow comwin = (CommandWindow)win.Object;
        comwin.SendInput("nav \"" + url + "\"", true);
      }
    }  

    public void ClearErrors()
    {
      _errorListProvider.Tasks.Clear();
    }

    public void ClearErrors(string atgFile)
    {
      for (int i = _errorListProvider.Tasks.Count - 1; i >= 0; i--)
      {
        if (_errorListProvider.Tasks[i].Document.ToLower() == atgFile.ToLower())
        {
          _errorListProvider.Tasks.RemoveAt(i);
        }
      }
    }

    public object GetService(Type serviceType)
    {
      try
      {
        return Package.GetGlobalService(serviceType);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return null;
      }
    }

    private void ErrorTaskNavigate(object sender, EventArgs e)
    {
      ErrorTask task;
      try
      {
        task = (ErrorTask)sender;
        task.Line += 1;
        _errorListProvider.Navigate(task, new Guid(EnvDTE.Constants.vsViewKindTextView));
        task.Line -= 1;
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

  }
}