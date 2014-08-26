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
using System.Text.RegularExpressions;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.Win32;
using Microsoft.VisualStudio.Shell;
using Microsoft.Practices.Common.Services;
using System.Collections.Generic;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates;
using System.CodeDom.Compiler;
using System.Text;
using System.Xml;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// Renames a project
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class AnalyzeSelectedProjects : ConfigurableAction
    {
        private int ignored = 0;
        private int solved = 0;

        public override void Execute()
        {
            DTE service = (DTE)this.GetService(typeof(DTE));
            
            try
            {
                Project selectedProject = Helpers.GetSelectedProject(service);
                if (selectedProject != null)
                {
                    Helpers.LogMessage(service, this, "*** Analyzing selected projects ***");
                    AnalyzeProject(service, selectedProject);
                }
                else
                {
                    Helpers.LogMessage(service, this, "*** Analyzing contained projects ***");
                    foreach (Project project in Helpers.GetAllProjects(service))
                    {
                        AnalyzeProject(service, project);
                    } 
                }                              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void AnalyzeProject(DTE service, Project project)
        {
            Helpers.LogMessage(service, this, "*** Analyzing project '" + project.Name + "' ***");
            
            try
            {
                SearchForHiddenFiles(service, project);

                CheckThatTTOutputIsNotUnderSCC(service, project);
            }
            catch (Exception ex)
            {
                Helpers.LogMessage(service, this, "Error: Analyzing of project '" + project.Name + "' failed: " + ex.Message);
            }

            Helpers.LogMessage(service, this, "Analysis finished, " + solved.ToString() + " conflicts solved, " + ignored.ToString() + " conflicts ignored");
            Helpers.LogMessage(service, this, " ");
        }

        private void CheckThatTTOutputIsNotUnderSCC(DTE service, Project project)
        {
            CheckThatTTOutputIsNotUnderSCC(service, project, project.ProjectItems);
            
        }

        private void CheckThatTTOutputIsNotUnderSCC(DTE service, Project project, ProjectItems projectItems)
        {
            foreach (ProjectItem childItem in projectItems)
            {
                if (childItem.Name.EndsWith(".tt"))
                {
                    //ok, tt-File found, check, if the child is under source control
                    if (childItem.ProjectItems.Count > 0)
                    {
                        foreach (ProjectItem ttOutputItem in childItem.ProjectItems)
                        {
                            string itemname = Helpers.GetFullPathOfProjectItem(ttOutputItem); // ttOutputHelpers.GetFullPathOfProjectItem(item);
                            if(service.SourceControl.IsItemUnderSCC(itemname))
                            {
                                Helpers.LogMessage(service, this, "Warning: File " + itemname + " should not be under source control");
                                if (MessageBox.Show("Warning: File " + itemname + " should not be under source control. " + Environment.NewLine + Environment.NewLine + "Exclude file from source control?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    Helpers.ExcludeItemFromSCC(service, project, ttOutputItem);
                                    solved++;
                                }
                                else
                                {
                                    ignored++;
                                }
                            }
                        }
                    }
                }
                CheckThatTTOutputIsNotUnderSCC(service, project, childItem.ProjectItems);
            }
        }

        private void SearchForHiddenFiles(DTE service, Project project)
        {
            ProjectItem hiveItem = Helpers.GetProjectItemByName(project.ProjectItems, "SharePointRoot");
            if (hiveItem == null)
            {
                hiveItem = Helpers.GetProjectItemByName(project.ProjectItems, "12");
                if (hiveItem == null)
                {
                    hiveItem = Helpers.GetProjectItemByName(project.ProjectItems, "14");
                }
                if (hiveItem == null)
                {
                    hiveItem = Helpers.GetProjectItemByName(project.ProjectItems, "15");
                }
            }
            if (hiveItem != null)
            {
                SearchHiddenFilesInFolder(service, project, hiveItem);
            }
        }

        private void SearchHiddenFilesInFolder(DTE service, Project project, ProjectItem hiveItem)
        {
            Helpers.LogMessage(service, this, "Searching for hidden files in folder '" + hiveItem.Name + "'");
            InternalSearchHiddenFilesInFolder(service, project, hiveItem);
        }

        private void InternalSearchHiddenFilesInFolder(DTE service, Project project, ProjectItem hiveItem)
        {
            string pathToProject = Helpers.GetFullPathOfProjectItem(project);
            if (hiveItem.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFolder)
            {
                string itemFolderPath = Helpers.GetFullPathOfProjectItem(hiveItem); 
                foreach (string childFile in Directory.GetFiles(itemFolderPath))
                {
                    //check, if each file in that folder is in the project
                    ProjectItem foundItem = DteHelper.FindItemByPath(service.Solution, childFile);
                    if (foundItem == null)
                    {
                        string relativeFileName = childFile.Substring(pathToProject.Length);
                        Helpers.LogMessage(service, this, "Warning: File " + relativeFileName + " not found in project");
                        AnalyzeProjectForm form = new AnalyzeProjectForm(relativeFileName);
                        DialogResult result = form.ShowDialog();
                        if (result == DialogResult.No)
                        {
                            //deletefile
                            Helpers.LogMessage(service, this, "Deleted File " + relativeFileName);
                            File.Delete(childFile);
                            solved++;
                        }
                        else if (result == DialogResult.Yes)
                        {
                            //include in project
                            Helpers.LogMessage(service, this, "Included File " + relativeFileName + " in project");
                            Helpers.AddFile(hiveItem, childFile);
                            solved++;
                        }
                        else
                        {
                            ignored++;
                        }
                    }
                }
            }
            foreach (ProjectItem childItem in hiveItem.ProjectItems)
            {
                InternalSearchHiddenFilesInFolder(service, project, childItem);
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