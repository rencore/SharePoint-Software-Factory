using System;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using EnvDTE80;
using EnvDTE;
using Microsoft.Practices.RecipeFramework;
using Microsoft.VisualStudio.SharePoint;

namespace SPALM.SPSF.Library.References
{
    /// <summary>
    /// every item in 12 Hive or Projects containing 12 hive or a solutionfolder
    /// </summary>
    [Serializable]
    public class SolutionHiveReference : UnboundRecipeReference
    {
        public SolutionHiveReference(string template)
            : base(template)
        {
        }

        public override bool IsEnabledFor(object target)
        {
            try
            {
                if (target is Project)
                {
                    if (Helpers.IsCustomizationProject((Project)target))
                    {
                        return true;
                    }
                    else if (((Project)target).Object is SolutionFolder)
                    {
                        //is there any Customization Project in this solutionfolder
                        SolutionFolder f = ((Project)target).Object as SolutionFolder;
                        return Helpers.ContainsCustomizationProject(f);
                    }
                }
                else if (target is Solution)
                {
                    return true;
                }
                else if (target is ProjectItem)
                {
                    //check if item is deployable
                    ProjectItem pitem = target as ProjectItem;

                    if (pitem.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFile)
                    {
                        //ok, is file
                        return ItemIsDeployable(pitem);
                    }
                    else
                    {
                        //has the folder a deployable child item
                        return ContainsDeployableItem(pitem);
                    }
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        private bool ContainsDeployableItem(ProjectItem pitem)
        {
            if (pitem != null && pitem.ProjectItems != null)
            {
                foreach (ProjectItem childItem in pitem.ProjectItems)
                {
                    if (childItem.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFile)
                    {
                        //ok, is file
                        if (ItemIsDeployable(childItem))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (ContainsDeployableItem(childItem))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool ItemIsDeployable(ProjectItem pitem)
        {
            string sourcefilename = Helpers.GetFullPathOfProjectItem(pitem);
            if (!Helpers2.IsFileDeployableToHive(sourcefilename))
            {
                return false;
            }

            try
            {
                ISharePointProjectService projectService = Helpers2.GetSharePointProjectService(pitem.DTE);

                try
                {
                    ISharePointProjectItemFile selectedSharePointItem = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItemFile>(pitem);
                    if (selectedSharePointItem != null && selectedSharePointItem.DeploymentPath != "")
                    {
                        return true;
                    }
                }
                catch { }

            }
            catch { }

            //check if the parent is folder 12
            try
            {
                string itemPath = Helpers.GetFullPathOfProjectItem(pitem);
                string projectPath = Helpers.GetProjectFolder(pitem.ContainingProject);
                itemPath = itemPath.Substring(projectPath.Length + 1);
                if (itemPath.StartsWith(@"12") || itemPath.StartsWith(@"14") || itemPath.StartsWith(@"15") || itemPath.StartsWith(@"SharePointRoot", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            catch { }

            return false;
        }
        
        public override string AppliesTo
        {
            get { return "All Customization projects or solution folders with customization projects"; }
        }

        #region ISerializable Members

        /// <summary>
        /// Required constructor for deserialization.
        /// </summary>
        protected SolutionHiveReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion ISerializable Members
    }
}
