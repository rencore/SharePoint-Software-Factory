#region Using Directives

using System;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.Services;
using System.IO;
using Microsoft.Practices.Common;
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using System.Collections.Generic;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Library.Templates.Actions;
using System.Text.RegularExpressions;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.Common.Services;
using System.Xml;
using EnvDTE80;
using Microsoft.Win32;
using System.Collections;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.VisualStudio;
using System.Globalization;
using System.Security.Permissions;
using System.Windows.Forms;

#endregion

namespace SPALM.SPSF.Library.Actions
{
	[ServiceDependency(typeof(DTE)), ServiceDependency(typeof(ITypeResolutionService))]
	public class ExtendedUnfoldTemplateActionBase : ConfigurableAction
	{
		// Fields
		private string destFolder;
		private string itemName;
		private object newItem;
		private string path = string.Empty;
		private object root;
		private string template;

		// Methods
		private void AddItemTemplate(Project rootproject)
		{
			DTE service = (DTE)this.GetService(typeof(DTE));
			string str2 = DteHelper.BuildPath(rootproject);
			if (!string.IsNullOrEmpty(this.Path))
			{
				str2 = System.IO.Path.Combine(str2, this.Path);
			}
			ProjectItem item = DteHelper.FindItemByPath(service.Solution, str2);
			if (item != null)
			{
				this.NewItem = item.ProjectItems.AddFromTemplate(this.Template, this.ItemName);
			}
			else
			{
				Project project = DteHelper.FindProjectByPath(service.Solution, str2);
				if (project == null)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture, "InsertionPointException", new object[] { str2 }));
				}
				project.ProjectItems.AddFromTemplate(this.Template, this.ItemName);
			}
		}

		private void AddProjectTemplate(Project project)
		{
			try
			{
				IRecipeManagerService provider = (IRecipeManagerService)this.GetService(typeof(IRecipeManagerService));

				GuidancePackage p = provider.GetPackage("SharePointSoftwareFactory.Base");
				GuidancePackage p2 = provider.EnablePackage("SharePointSoftwareFactory.Base");
			}
			catch (Exception)
			{

			}

			DTE service = (DTE)this.GetService(typeof(DTE));
			SolutionFolder folder = null;
			if (project == null)
			{
				if (string.IsNullOrEmpty(this.Path))
				{
                    //char[] invalidedChars = System.IO.Path.GetInvalidPathChars();
                    //foreach (char c in invalidedChars)
                    //{
                    //    if (this.Template.IndexOf(c) > 0)
                    //    {
                    //    }
                    //    if (this.DestinationFolder.IndexOf(c) > 0)
                    //    {
                    //    }
                    //}
					this.NewItem = service.Solution.AddFromTemplate(this.Template, this.DestinationFolder, this.ItemName, false);
				}
				else
				{
					folder = (SolutionFolder)DteHelper.FindProjectByPath(service.Solution, this.Path).Object;
					this.NewItem = folder.AddFromTemplate(this.Template, this.DestinationFolder, this.ItemName);
				}
			}
			else
			{
                //sometimes in the solutionfolder a project already exists but is not part of the project
                //so we delete the folder if it already exists
                if (Directory.Exists(this.DestinationFolder))
                {
                    if (MessageBox.Show("Directory '" + this.DestinationFolder + "' already exists in the solution. Delete directory? If you choose 'No' the directory will be renamed to '" + this.DestinationFolder + "_Backup'", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Directory.Delete(this.DestinationFolder, true);
                    }
                    else
                    {
                        string backupDirectoryName = this.DestinationFolder + "_Backup";
                        int count = 1;
                        while (Directory.Exists(backupDirectoryName))
                        {
                            backupDirectoryName = this.DestinationFolder + "_Backup" + count.ToString();
                            count++;
                        }
                        Directory.Move(this.DestinationFolder, backupDirectoryName);
                    }
                }

				folder = (SolutionFolder)project.Object;
				this.NewItem = folder.AddFromTemplate(this.Template, this.DestinationFolder, this.ItemName);
			}
			if (this.newItem == null)
			{
				ProjectItems projectItems;
				if (folder != null)
				{
					projectItems = folder.Parent.ProjectItems;
				}
				else
				{
					projectItems = service.Solution.Projects as ProjectItems;
				}
				if (projectItems != null)
				{
					foreach (ProjectItem item in projectItems)
					{
						if (item.Name.Contains(this.ItemName))
						{
							this.NewItem = item.Object as Project;
							break;
						}
					}
				}
				else
				{
					this.NewItem = FindProjectByName(service, this.ItemName, false);
				}
			}
		}

        public Project FindProject(_DTE vs, Predicate<Project> match)
        {
            Guard.ArgumentNotNull(vs, "vs");
            Guard.ArgumentNotNull(match, "match");
            foreach (Project project in vs.Solution.Projects)
            {
                if (match(project))
                {
                    return project;
                }
                Project project2 = FindProjectInternal(project.ProjectItems, match);
                if (project2 != null)
                {
                    return project2;
                }
            }
            return null;
        }

        private Project FindProjectInternal(ProjectItems items, Predicate<Project> match)
        {
            if (items != null)
            {
                foreach (ProjectItem item in items)
                {
                    Project project = item.SubProject ?? (item.Object as Project);
                    if (project != null)
                    {
                        if (match(project))
                        {
                            return project;
                        }
                        project = FindProjectInternal(project.ProjectItems, match);
                        if (project != null)
                        {
                            return project;
                        }
                    }
                }
            }
            return null;
        }

        public Project FindProjectByName(DTE dte, string name, bool isWeb)
        {
            Predicate<Project> match = null;
            if (!isWeb)
            {
                if (match == null)
                {
                    match = delegate(Project internalProject)
                    {
                        return internalProject.Name == name;
                    };
                }
                return FindProject(dte, match);
            }
            foreach (Project project2 in dte.Solution.Projects)
            {
                if (project2.Name.Contains(name))
                {
                    return project2;
                }
                if (project2.ProjectItems != null)
                {
                    Project project3 = FindProjectByName(project2.ProjectItems, name);
                    if (project3 != null)
                    {
                        return project3;
                    }
                }
            }
            return null;
        }


        private Project FindProjectByName(ProjectItems items, string name)
        {
            foreach (ProjectItem item in items)
            {
                if ((item.Object is Project) && ((Project)item.Object).Name.Contains(name))
                {
                    return (item.Object as Project);
                }
                if (item.ProjectItems != null)
                {
                    Project project = FindProjectByName(item.ProjectItems, name);
                    if (project != null)
                    {
                        return project;
                    }
                }
            }
            return null;
        }



		public override void Execute()
		{
			DTE service = (DTE)this.GetService(typeof(DTE));
			Project project = FindProjectByName(service, this.ItemName, false);
			if (project != null)
			{
				this.NewItem = project;
			}
			else
			{
				if (string.IsNullOrEmpty(this.DestinationFolder))
				{
					this.DestinationFolder = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(service.Solution.FileName), this.ItemName);
					if (Directory.Exists(this.DestinationFolder))
					{
						Directory.Delete(this.DestinationFolder, true);
					}
				}
				this.InternalExecute();
			}
		}

		private void InternalExecute()
		{
			if ((this.Root == null) || (this.Root is Solution))
			{
				this.AddProjectTemplate(null);
			}
			else if ((this.Root is Project) && (((Project)this.Root).Object is SolutionFolder))
			{
				this.AddProjectTemplate((Project)this.Root);
			}
			else if (this.Root is SolutionFolder)
			{
				this.AddProjectTemplate(((SolutionFolder)this.Root).Parent);
			}
			else if (this.Root is Project)
			{
				this.AddItemTemplate((Project)this.Root);
			}
		}

		public override void Undo()
		{
		}

		// Properties
		[Input]
		public string DestinationFolder
		{
			get
			{
				return this.destFolder;
			}
			set
			{
				this.destFolder = value;
			}
		}

		[Input(Required = true)]
		public string ItemName
		{
			get
			{
				return this.itemName;
			}
			set
			{
				this.itemName = value;
			}
		}

		[Output]
		public object NewItem
		{
			get
			{
				return this.newItem;
			}
			set
			{
				this.newItem = value;
			}
		}

        [Output]
        public string CreatedProjectID
        {
            get
            {
                if(newItem is Project)
                {
                    return Helpers.GetProjectGuid(newItem as Project);
                }
                else if(newItem is ProjectItem)
                {
                    return Helpers.GetProjectGuid((newItem as ProjectItem).ContainingProject);
                }
                return "";
            }
        }

		[Input]
		public string Path
		{
			get
			{
				return this.path;
			}
			set
			{
				this.path = value;
			}
		}

		[Input]
		public object Root
		{
			get
			{
				return this.root;
			}
			set
			{
				this.root = value;
			}
		}

		[Input(Required = true)]
		public string Template
		{
			get
			{
				if (!File.Exists(this.template))
				{
					TypeResolutionService service = (TypeResolutionService)this.GetService(typeof(ITypeResolutionService));
					if (service != null)
					{
						this.template = new FileInfo(System.IO.Path.Combine(System.IO.Path.Combine(service.BasePath, @"Templates\"), this.template)).FullName;
					}
				}
				return this.template;
			}
			set
			{
				this.template = value;
			}
		}
	}
}
