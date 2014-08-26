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
using EnvDTE80;
using System.ComponentModel.Design;

#endregion

namespace SPALM.SPSF.Library.Actions
{
  /// <summary>
  /// Add a new item to a given folder
  /// </summary>
  [ServiceDependency(typeof(DTE))]
  public class RemoveItemFromSolutionFolder : ConfigurableAction
  {
    private string _FileName = "";
    public string FileName
    {
      get { return _FileName; }
      set { _FileName = value; }
    }

    private string destFolder = String.Empty;
    [Input(Required = true)]
    public string TargetFolder
    {
      get { return destFolder; }
      set { destFolder = value; }
    }

    internal string EvaluateParameterAsString(DTE dte, string parameterName)
    {
      try
      {
        IDictionaryService serviceToAdapt = (IDictionaryService)this.GetService(typeof(IDictionaryService));
        ServiceAdapterDictionary serviceAdaptor = new ServiceAdapterDictionary(serviceToAdapt);
        ExpressionEvaluationService2 expressionService2 = new ExpressionEvaluationService2();
        string evaluatedValue = expressionService2.Evaluate(parameterName, serviceAdaptor).ToString();
        if (string.IsNullOrEmpty(evaluatedValue))
        {
          return "";
        }
        return evaluatedValue;
      }
      catch (NullReferenceException)
      {
      }
      return "";
    }

    public override void Execute()
    {
      EnvDTE.DTE dte = this.GetService<EnvDTE.DTE>(true);
      try
      {
        Solution2 soln = (Solution2)dte.Solution;
        Project projecttargetfolder = Helpers.GetSolutionFolder(soln, TargetFolder);
        ProjectItem projectItem = Helpers.GetProjectItemByName(projecttargetfolder.ProjectItems, _FileName);

        if (projectItem != null)
        {
          Helpers.EnsureCheckout(dte, projectItem);
          Helpers.LogMessage(dte, dte, "Deleting '" + _FileName + "'");
          projectItem.Delete();          
        }
      }
      catch
      {
        Helpers.LogMessage(dte, dte, "Could not delete file '" + _FileName + "'");
      }
    }

    /// <summary>
    /// Removes the previously added reference, if it was created
    /// </summary>
    public override void Undo()
    {
    }
  }
}