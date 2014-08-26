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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

#endregion

namespace SPALM.SPSF.Library.Actions
{
	/// <summary>
	/// </summary>
	[ServiceDependency(typeof(DTE))]
    public class ManifestMakeStatic : ConfigurableAction
	{
		private ProjectItem projectItem;
		private Project project;

		[Input(Required = false)]
		public ProjectItem SelectedItem
		{
			get { return projectItem; }
			set { projectItem = value; }
		}

		[Input(Required = false)]
		public Project SelectedProject
		{
			get { return project; }
			set { project = value; }
		}

		protected string GetBasePath()
		{
			return base.GetService<IConfigurationService>(true).BasePath;
		}

		public override void Execute()
		{
			DTE service = (DTE)this.GetService(typeof(DTE));
			
			try
			{
                string content = "";
                string generatedFilename = "";                
                ProjectItems parentProjectItems = null;
                bool removeTT = false;

				if(projectItem != null)
				{
                    if(projectItem.Collection.Parent is Project)
                    {
                        parentProjectItems = (projectItem.Collection.Parent as Project).ProjectItems;
                    }
                    else if (projectItem.Collection.Parent is ProjectItem)
                    {
                        ProjectItem parentItem = projectItem.Collection.Parent as ProjectItem;
                        parentProjectItems = parentItem.ProjectItems;
                    }

                    if(projectItem.Name.EndsWith(".tt", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Helpers.EnsureCheckout(service, Helpers.GetFullPathOfProjectItem(projectItem));

                        //find the child item 
                        if (projectItem.ProjectItems.Count > 0)
                        {
                            //take the first child
                            foreach (ProjectItem childItem in projectItem.ProjectItems)
                            {
                                generatedFilename = Helpers.GetFullPathOfProjectItem(childItem); //childHelpers.GetFullPathOfProjectItem(item);
                                content = File.ReadAllText(generatedFilename);
                                removeTT = true;
                                break;
                            }

                            if (removeTT)
                            {
                                //delete dependent childs
                                try
                                {
                                    for (int i = projectItem.ProjectItems.Count; i > 0; i--)
                                    {
                                        projectItem.ProjectItems.Item(i).Delete();
                                    }
                                }
                                catch (Exception)
                                {
                                }
                                string ttfilePath = Helpers.GetFullPathOfProjectItem(projectItem); //projectHelpers.GetFullPathOfProjectItem(item);
                                projectItem.Delete();

                                File.WriteAllText(generatedFilename, content);

                                parentProjectItems.AddFromFile(generatedFilename);
                                Helpers.LogMessage(service, this, "Item converted successfully");
                            }
                        }
                    }					
				}	
			}
			catch (Exception ex)
			{
				Helpers.LogMessage(service, this, ex.ToString());
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