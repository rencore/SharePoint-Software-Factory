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
using System.Collections.Generic;
using EnvDTE80;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    [ServiceDependency(typeof(DTE))]
    public class QuickDeploy : ConfigurableAction
    {

        public override void Execute()
        {
            DTE dte = GetService<DTE>(true);

            int success = 0;
            int failures = 0;
            int overwritten = 0;

            //is item selected, then copy only the item
            if (dte.SelectedItems.Count > 0)
            {
                foreach (SelectedItem item in dte.SelectedItems)
                {
                    if (item is ProjectItem)
                    {
                        DeploymentHelpers.QuickDeployItem(dte, item as ProjectItem, ref success, ref failures, ref overwritten);
                        Helpers.LogMessage(dte, dte, "*** Quick Deploy finished: " + success.ToString() + " successfully (" + overwritten.ToString() + " overwrites), " + failures.ToString() + " failed ***" + Environment.NewLine);
                    }
                    else if (item.ProjectItem is ProjectItem)
                    {
                        DeploymentHelpers.QuickDeployItem(dte, item.ProjectItem as ProjectItem, ref success, ref failures, ref overwritten);
                        Helpers.LogMessage(dte, dte, "*** Quick Deploy finished: " + success.ToString() + " successfully (" + overwritten.ToString() + " overwrites), " + failures.ToString() + " failed ***" + Environment.NewLine);
                    }
                    else if (item.Project != null)
                    {
                        if (item.Project.Object is SolutionFolder)
                        {
                            //solution folder selected
                            SolutionFolder sfolder = item.Project.Object as SolutionFolder;
                            foreach (ProjectItem pitem in sfolder.Parent.ProjectItems)
                            {
                                if (pitem.Object is Project)
                                {
                                    DeploymentHelpers.QuickDeploy(dte, pitem.Object as Project);
                                }
                            }                            
                        }
                        else
                        {
                            //project selected
                            DeploymentHelpers.QuickDeploy(dte, item.Project);
                        }                        
                    }
                    else if ((item.Project == null) && (item.ProjectItem == null))
                    {
                        //solution selected
                        List<Project> projects = Helpers.GetSelectedDeploymentProjects(dte);
                        foreach (Project project in projects)
                        {
                            //deployEachProject
                            DeploymentHelpers.QuickDeploy(dte, project);
                        }
                    }
                }
            }
            else
            {
                List<Project> projects = Helpers.GetSelectedDeploymentProjects(dte);
                foreach (Project project in projects)
                {
                    //deployEachProject
                    DeploymentHelpers.QuickDeploy(dte, project);
                }
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