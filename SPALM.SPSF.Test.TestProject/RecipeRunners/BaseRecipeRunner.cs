using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnvDTE;
using Microsoft.Win32;
using System.IO;
using System.Xml;
using Microsoft.Practices.Common.Services;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Collections;
using Microsoft.Practices.RecipeFramework.Configuration;
using System.Reflection;
using SPALM.SPSF.Library;
using Microsoft.VSSDK.Tools.VsIdeTesting;

namespace SharePointSoftwareFactory.Tests
{
  public class BaseRecipeRunner
  {
    public string recipeName = "";
    public bool NotSandboxSupported = false;

    internal string testDataRootDir = "";

    internal BaseTest parentTest = null;

    internal Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage = null;

    public BaseRecipeRunner(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, string recipeName, BaseTest parentTest)
    {
      this.guidancePackage = guidancePackage;
      this.recipeName = recipeName;
      this.parentTest = parentTest;

      DirectoryInfo binDebugFolder = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
      DirectoryInfo solutionDir = binDebugFolder.Parent.Parent.Parent;
      testDataRootDir = Path.Combine(solutionDir.FullName, "TestData");
    }

    public virtual void Init(DTE dte)
    {
    }

    public void AddExpectedDeployResult(string result)
    {
      if (parentTest != null)
      {
        parentTest.AddExpectedDeployResult(this.recipeName + ": " + result);
      }
    }

    private EnvDTE.DTE GetDTE()
    {
      IServiceProvider sp = VsIdeTestHostContext.ServiceProvider;
      return (EnvDTE.DTE)sp.GetService(typeof(EnvDTE.DTE));
    }

    public void SelectProjectByName(string projectName)
    {
      Project CoreRecipes = null;
      foreach (Project p in GetDTE().Solution.Projects)
      {
        if (p.Name == projectName)
        {
          CoreRecipes = p;
        }
      }

      if (CoreRecipes != null)
      {
        UIHierarchy tree = GetDTE().Windows.Item(EnvDTE.Constants.vsWindowKindSolutionExplorer).Object as UIHierarchy;
        SelectProject(tree, CoreRecipes);
      }
    }

    public void SelectProject(UIHierarchy tree, Project CoreRecipes)
    {
      foreach (UIHierarchyItem subnode in tree.UIHierarchyItems)
      {
        subnode.UIHierarchyItems.Expanded = true;
        SelectProject(subnode, CoreRecipes);
      }
    }

    public void SelectProject(UIHierarchyItem node, Project CoreRecipes)
    {
      foreach (UIHierarchyItem subnode in node.UIHierarchyItems)
      {        
        

        if (subnode.Object is Project)
        {
          Project p = subnode.Object as Project;
          if (p.Name == CoreRecipes.Name)
          {
            subnode.UIHierarchyItems.Expanded = true;
            subnode.Select(vsUISelectionType.vsUISelectionTypeSelect);
          }
        }
        if (subnode.Object is ProjectItem)
        {
          ProjectItem p = subnode.Object as ProjectItem;
          if (p.SubProject != null)
          {
            if (p.SubProject.Name == CoreRecipes.Name)
            {
              subnode.UIHierarchyItems.Expanded = true;
              subnode.Select(vsUISelectionType.vsUISelectionTypeSelect);
            }
          }
        }
        SelectProject(subnode, CoreRecipes);
      }
    }

    internal void SelectFeature(string featureScope)
    {
      Project customizationProject = SelectCustomizationProject(GetDTE());

      ProjectItem featureFolder = Helpers.GetFirstFeatureWithScope(GetDTE(), customizationProject, featureScope);
      if (featureFolder == null)
      {
        //create a feature with featureScope

        Dictionary<string, object> arguments = new Dictionary<string, object>();
        arguments.Add("FeatureName", featureScope + "Scoped");
        arguments.Add("FeatureScope", featureScope);
        new RecipeRunner_EmptyFeature(guidancePackage, parentTest).RunRecipeWithSpecifiedParameters(arguments);

        featureFolder = Helpers.GetFirstFeatureWithScope(GetDTE(), customizationProject, featureScope);

        if (featureFolder == null)
        {
          throw new Exception("Feature with scope " + featureScope + " not found in project");
        }
      }

      SelectProjectItem(featureFolder);

    }

    internal Project SelectCustomizationProject(DTE dte)
    {
      this.parentTest.TestContext.WriteLine("SelectCustomizationProject");
      foreach (Project project in dte.Solution.Projects)
      {
        //expand all first level nodes
        
        if (Helpers.IsCustomizationProject(project))
        {
          this.parentTest.TestContext.WriteLine("Yes IsCustomizationProject " + project.Name);
          SelectProject(project);
          return project;
        }
        if (project.Object is EnvDTE80.SolutionFolder)
        {

          EnvDTE80.SolutionFolder x = (EnvDTE80.SolutionFolder)project.Object;
          foreach (ProjectItem pitem in x.Parent.ProjectItems)
          {
            if (pitem.Object is Project)
            {
              Project p2 = pitem.Object as Project;
              if (Helpers.IsCustomizationProject(p2))
              {
                this.parentTest.TestContext.WriteLine("Yes IsCustomizationProject " + project.Name);
                SelectProject(p2);
                return p2;
              }
            }
          }
        }
      }

      throw new Exception("No CustomizationProject found in solution");
    }

    internal void SelectProjectItem(ProjectItem projectItem)
    {
      UIHierarchy tree = projectItem.DTE.Windows.Item(EnvDTE.Constants.vsWindowKindSolutionExplorer).Object as UIHierarchy;

      UIHierarchyItems solutionItems = tree.UIHierarchyItems;

      // 
      // ExpandView dont (always) work it item is inside solution folders. 
      // The hack below worked, but it expanded all solution folders, and since we walk the solution explorer 
      // in FindHierarchyItem we might as well expand at the same time 
      // 
      //try 
      //{ 
      //    proItem.ExpandView(); 
      //} 
      //catch (Exception) 
      //{ 
      //    //bug: expand project dont work if a soultion folder is parent and it hasnt been expanded yet 
      //    LoadSolutionItems(items); 
      //    proItem.ExpandView(); 
      //} 

      //check if we have a solution 
      if (solutionItems.Count != 1)
        return;

      //FindHierarchyItem expands nodes as well (it must do so, because children arent loaded until expanded) 
      UIHierarchyItem uiItem = FindHierarchyItem(solutionItems.Item(1).UIHierarchyItems, projectItem);

      if (uiItem != null)
      {
        //if we called DoDefaultAction in FindHierarchyItem, solution explorer will have focus 
        //set it back to  the dicument 
        //projectItem.Document.Activate();

        //didnt find another way to make the item gray (while not having focus) 
        //dte.ExecuteCommand("View.TrackActivityinSolutionExplorer", "True");

        //if item is already selected, View.TrackActivityinSolutionExplorer wont make the item grey, 
        //so deselect it first 
        if (uiItem.IsSelected)
          uiItem.Select(vsUISelectionType.vsUISelectionTypeToggle);

        //selecting while View.TrackActivityinSolutionExplorer selects and makes it grey 
        uiItem.Select(vsUISelectionType.vsUISelectionTypeSelect);
        //done with it now 
        //dte.ExecuteCommand("View.TrackActivityinSolutionExplorer", "False");

      }
      else
      {

      }
    }

    internal UIHierarchyItem FindHierarchyItem(UIHierarchyItems items, object item)
    {
      // 
      // Enumerating children recursive would work, but it may be slow on large solution. 
      // This tries to be smarter and faster 
      // 

      Stack s = new Stack();
      CreateItemsStack(s, item);

      UIHierarchyItem last = null;
      while (s.Count != 0)
      {
        if (!items.Expanded)
          items.Expanded = true;
        if (!items.Expanded)
        {
          //bug: expand dont always work... 
          UIHierarchyItem parent = ((UIHierarchyItem)items.Parent);
          parent.Select(vsUISelectionType.vsUISelectionTypeSelect);

          UIHierarchy tree = items.DTE.Windows.Item(EnvDTE.Constants.vsWindowKindSolutionExplorer).Object as UIHierarchy;
          tree.DoDefaultAction();
          //_DTE.ToolWindows.SolutionExplorer.DoDefaultAction();
        }

        object o = s.Pop();

        last = null;
        foreach (UIHierarchyItem child in items)
          if (child.Object == o)
          {
            last = child;
            items = child.UIHierarchyItems;
            break;
          }
      }

      return last;
    }

    internal void CreateItemsStack(Stack s, object item)
    {
      if (item is ProjectItem)
      {
        ProjectItem pi = (ProjectItem)item;
        s.Push(pi);
        CreateItemsStack(s, pi.Collection.Parent);
      }
      else if (item is Project)
      {
        Project p = (Project)item;
        s.Push(p);
        if (p.ParentProjectItem != null)
        {
          //top nodes dont have solution as parent, but is null 
          CreateItemsStack(s, p.ParentProjectItem);
        }
      }
      else if (item is Solution)
      {
        //doesnt seem to ever happend... 
        Solution sol = (Solution)item;
      }
      else
      {
        throw new Exception("unknown item");
      }
    }

    internal void SelectProject(Project project)
    {
      this.parentTest.TestContext.WriteLine("trying to select project " + project.Name);

      UIHierarchy tree = project.DTE.Windows.Item(EnvDTE.Constants.vsWindowKindSolutionExplorer).Object as UIHierarchy;
      SelectProject(tree, project);
      //tree.DoDefaultAction();

      GetDTE().Windows.Item(Constants.vsWindowKindSolutionExplorer).Activate();

      //project.DTE.ExecuteCommand("View.TrackActivityinSolutionExplorer", "True");

      System.Threading.Thread.Sleep(2000);
    }

    public virtual Dictionary<string, object> GetInitialArguments()
    {
        return new Dictionary<string, object>(); ;
    }

    public void RunRecipeWithDefaultValues()
    {
        Dictionary<string, object> arguments = GetInitialArguments(); 
      RunRecipe(arguments);
    }

    /// <summary>
    /// Runs the recipe with all non-required arguments set to null to check whether the text templates can handle this
    /// </summary>
    /// <param name="recipeName"></param>
    /// <param name="dte"></param>
    public void RunRecipeWithoutRequiredValues()
    {
        Dictionary<string, object> arguments = GetInitialArguments(); 
      Recipe recipe = GetRecipe(recipeName);
      foreach (Argument arg in recipe.Arguments)
      {
        if (arg.Required == false)
        {
          arguments.Add(arg.Name, null);
        }
      }
      RunRecipe(arguments);
    }

    /// <summary>
    /// Runs the recipe with all non-required arguments set to null to check whether the text templates can handle this
    /// </summary>
    /// <param name="recipeName"></param>
    /// <param name="dte"></param>
    public void RunRecipeWithSpecifiedParameters(Dictionary<string, object> arguments)
    {
      RunRecipe(arguments);
    }

    public bool CurrentTestIsSandboxed()
    {
      //if solution is sandboxed and the recipe is NotSandboxSupported then do not run the recipe
      if (GetDTE().Solution.FullName.Contains(".SandBoxed."))
      {
        
          this.parentTest.testContextInstance.WriteLine("Recipe '" + this.recipeName + "' ignored in sandbox ");
          return true;
        
      }
      return false;
    }

    public void RunRecipe(Dictionary<string, object> arguments)
    {
      //check if the recipe needs a customization project
      this.Init(GetDTE());
      if (NotSandboxSupported)
      {
        if (CurrentTestIsSandboxed())
        {
          return;
        }
      }

      Microsoft.Practices.Common.ExecutionResult exResult = guidancePackage.Execute(recipeName, arguments);
      Assert.IsTrue(exResult == Microsoft.Practices.Common.ExecutionResult.Finish);

      if (GetDTE().Solution.IsDirty)
      {
        GetDTE().Solution.SaveAs(GetDTE().Solution.FullName);
      }

      GetDTE().Documents.CloseAll(EnvDTE.vsSaveChanges.vsSaveChangesNo);

    }

    private Recipe GetRecipe(string recipeName)
    {
      foreach (Recipe recipe in guidancePackage.Configuration.Recipes)
      {
        if (recipe.Name == recipeName)
        {
          return recipe;
        }
      }
      throw new Exception("Recipe with name '" + recipeName + "' not found in package '" + guidancePackage.Configuration.Name + "'");
    }
  }
}
