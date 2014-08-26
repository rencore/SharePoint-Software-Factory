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
using Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates;
using Microsoft.Practices.Common.Services;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ComponentModel.Design;
using System.Xml;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using SPALM.SPSF.Library.Actions;
using System.Security.Permissions;
using System.Security;
using Microsoft.VisualStudio.Shell;
using EnvDTE80;
using Microsoft.VisualStudio.SharePoint;

#endregion

namespace SPALM.SPSF.Library
{
    public class Helpers2
    {
        public static ProjectItem GetFeature(DTE dte, Project project, string featureName)
        {
            ProjectItem folderOfFeature = null;
            try
            {
                folderOfFeature = Helpers.GetFolder(Helpers.GetFeatureFolder(project, false), featureName, false);
            }
            catch { }

            if (folderOfFeature == null)
            {
                throw new Exception("Feature " + featureName + " not found. Could not add feature to package.");
            }
            return folderOfFeature;
        }

        /// <summary>
        /// returns the projectitems with the given deploymentpath, it creates a mapped folder if need and the needed subfolder
        /// </summary>
        /// <param name="project"></param>
        /// <param name="sPFileType"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static ProjectItems GetDeploymentPath(DTE dte, Project project, SPFileType sPFileType, string deploymentPath)
        {
            //parameters are e.g. SPFileType.TemplateFile + DeploymentPath = "/Layouts/APPNAME"
            //we create and return the project items of path /SharePointRoot/Templates/Layouts/AppName 
            ProjectItems mappedfolder = null;

            if (Helpers2.IsSharePointVSTemplate(dte, project))
            {
                //get the mapped folder an
                mappedfolder = Helpers2.GetMappedFolder(dte, project, sPFileType);
            }
            else
            {
                //create the needed folder structure for the deploymentpath
                mappedfolder = Helpers2.GetMappedFolder(dte, project, sPFileType);
            }

            if (mappedfolder != null)
            {
                if (!string.IsNullOrEmpty(deploymentPath))
                {
                    //subfolder within the mappedfolder needed
                    return Helpers.GetProjectItemsByPath(mappedfolder, deploymentPath);
                }
                else
                {
                    return mappedfolder;
                }
            }
            return null;
        }

        public static bool IsXPathInFile(ProjectItem pitem, string xpath, string xpathnamespace)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(Helpers.GetFullPathOfProjectItem(pitem));

                if (string.IsNullOrEmpty(xpathnamespace))
                {
                    if (xml.SelectNodes(xpath).Count > 0)
                    {
                        //xpath found
                        return true;
                    }
                }
                else
                {
                    XmlNamespaceManager newnsmgr = new XmlNamespaceManager(xml.NameTable);
                    newnsmgr.AddNamespace("ns", xpathnamespace);
                    if (xml.SelectNodes(xpath, newnsmgr).Count > 0)
                    {
                        //xpath found
                        return true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return false;
        }


        public static Guid GetProjectGuid(Project project)
        {
            Microsoft.VisualStudio.Shell.Interop.IVsHierarchy hierarchy = null;
            IVsSolution solution = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SVsSolution)) as IVsSolution;
            if (solution != null)
            {
                solution.GetProjectOfUniqueName(project.FullName, out hierarchy);

                if (hierarchy != null)
                {
                    Guid projectGuid;
                    hierarchy.GetGuidProperty(VSConstants.VSITEMID_ROOT, (int)__VSHPROPID.VSHPROPID_ProjectIDGuid, out projectGuid);
                    if (projectGuid != null)
                    {
                        return projectGuid;
                    }
                }
            }
            return Guid.Empty;
        }

        internal static string GetProjectTypeGuids(EnvDTE.Project proj)
        {

            string projectTypeGuids = "";
            object service = null;
            Microsoft.VisualStudio.Shell.Interop.IVsSolution solution = null;
            Microsoft.VisualStudio.Shell.Interop.IVsHierarchy hierarchy = null;
            Microsoft.VisualStudio.Shell.Interop.IVsAggregatableProject aggregatableProject = null;
            int result = 0;

            service = GetService(proj.DTE, typeof(Microsoft.VisualStudio.Shell.Interop.IVsSolution));
            solution = (Microsoft.VisualStudio.Shell.Interop.IVsSolution)service;

            result = solution.GetProjectOfUniqueName(proj.UniqueName, out hierarchy);

            if (result == 0)
            {
                aggregatableProject = (Microsoft.VisualStudio.Shell.Interop.IVsAggregatableProject)hierarchy;
                result = aggregatableProject.GetAggregateProjectTypeGuids(out projectTypeGuids);
            }

            return projectTypeGuids;

        }

        internal static object GetService(object serviceProvider, System.Type type)
        {
            return GetService(serviceProvider, type.GUID);
        }

        internal static object GetService(object serviceProviderObject, System.Guid guid)
        {
            object service = null;
            Microsoft.VisualStudio.OLE.Interop.IServiceProvider serviceProvider = null;
            IntPtr serviceIntPtr;
            int hr = 0;
            Guid SIDGuid;
            Guid IIDGuid;

            SIDGuid = guid;
            IIDGuid = SIDGuid;
            serviceProvider = (Microsoft.VisualStudio.OLE.Interop.IServiceProvider)serviceProviderObject;
            hr = serviceProvider.QueryService(ref SIDGuid, ref IIDGuid, out serviceIntPtr);

            if (hr != 0)
            {
                System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(hr);
            }
            else if (!serviceIntPtr.Equals(IntPtr.Zero))
            {
                service = System.Runtime.InteropServices.Marshal.GetObjectForIUnknown(serviceIntPtr);
                System.Runtime.InteropServices.Marshal.Release(serviceIntPtr);
            }

            return service;
        }

        public static bool BuildProject(Project project)
        {
            //can we cast the project to ISharePointSolution
            ISharePointProjectService projectService = Helpers2.GetSharePointProjectService(project.DTE);
            ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(project);
            sharePointProject.Package.Validate();
            return sharePointProject.Package.BuildPackage();
        }

        /// <summary>
        /// Returns true if the template of the given project is a SharePoint Template
        /// </summary>
        /// <param name="dte"></param>
        /// <param name="selectedProject"></param>
        /// <returns></returns>
        public static bool IsSharePointVSTemplate(DTE dte, Project selectedProject)
        {
            //can we cast the project to ISharePointSolution
            try
            {
                //can we convert the feature to a sharepointfeature and check the scope???
                ISharePointProjectService projectService = Helpers2.GetSharePointProjectService(selectedProject.DTE);
                ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(selectedProject);
                if (sharePointProject != null)
                {
                    return true;
                }
            }
            catch { }
            return false;
        }

        
        /// <summary>
        /// Returns the current selected project
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        internal static Project GetSelectedProject(DTE service)
        {
            if (service.SelectedItems.Count > 0)
            {
                try
                {
                    SelectedItem item = service.SelectedItems.Item(1);
                    Project project = null;

                    if (item is Project)
                    {
                        project = (Project)item;
                    }
                    else if (item is ProjectItem)
                    {
                        project = ((ProjectItem)item).ContainingProject;
                    }
                    else
                    {
                        if (item.ProjectItem != null)
                        {
                            project = item.ProjectItem.ContainingProject;
                        }
                        else if (item.Project != null)
                        {
                            project = item.Project;
                        }
                    }
                    if (project != null)
                    {
                        return project;
                    }
                }
                catch (Exception)
                {
                }
            }
            return null;
        }

        /// <summary>
        /// default it returns "SharePointRoot", if an older template is used it could return "12", "14", "RootFiles" 
        /// </summary>
        /// <param name="dte"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        internal static string GetCurrentSharePointRootName(DTE dte, Project project)
        {
            return "SharePointRoot";
        }


        /// <summary>
        /// Creates a file with the given filename and adds the new file to the given project at the given targetfolder
        /// </summary>
        /// <param name="dte"></param>
        /// <param name="project"></param>
        /// <param name="targetFolder"></param>
        /// <param name="targetFilename"></param>
        /// <param name="content"></param>
        /// <param name="overwrite"></param>
        /// <param name="open"></param>
        /// <returns>Created project item</returns>
        internal static ProjectItem AddFileToProject(DTE dte, Project project, string targetFolder, string targetFilename, string content, bool overwrite, bool open, out ProjectItems parentProjectItems)
        {
            ProjectItems whereToAdd = Helpers.GetProjectItemsByPath(project, targetFolder);
            parentProjectItems = whereToAdd;
            return AddFile(dte, whereToAdd, targetFilename, content, overwrite, open);
        }

        internal static ProjectItem AddFileToProject(DTE dte, Project project, string targetFolder, string targetFilename, string content, bool overwrite, bool open)
        {
            ProjectItems whereToAdd = Helpers.GetProjectItemsByPath(project, targetFolder);
            return AddFile(dte, whereToAdd, targetFilename, content, overwrite, open);
        }

        internal static ProjectItem AddFile(DTE dte, ProjectItems whereToAdd, string targetFilename, string content, bool overwrite, bool open)
        {
            //sometimes parent is a file, but it should be a folder, so we point to the parent whereToAdd
            ProjectItem createdProjectItem = null;
            try
            {
                if (targetFilename.ToUpper().EndsWith(".XML") || targetFilename.ToUpper().EndsWith(".BDCM"))
                {
                    try
                    {
                        //we place our comment there
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(content);
                        Helpers.CheckLicenseComment(doc);
                        content = doc.OuterXml;
                    }
                    catch (Exception)
                    {
                    }
                }

                if (targetFilename.ToUpper().EndsWith(".CS"))
                {
                    try
                    {
                        content = Helpers.CheckLicenseCommentCS(content);
                    }
                    catch (Exception)
                    {
                    }
                }


                string finalpath = "";
                if (whereToAdd.Parent is Project)
                {
                    finalpath = Path.Combine(Helpers.GetFullPathOfProjectItem(whereToAdd.Parent as Project), targetFilename);
                }
                else
                {
                    //project item
                    ProjectItem parentItem = whereToAdd.Parent as ProjectItem;
                    if (parentItem.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFile)
                    {
                        string projectItemFilePath = Helpers.GetFullPathOfProjectItem(parentItem);
                        finalpath = Path.Combine(Path.GetDirectoryName(projectItemFilePath), targetFilename);
                    }
                    else
                    {
                        finalpath = Path.Combine(Helpers.GetFullPathOfProjectItem(whereToAdd.Parent as ProjectItem), targetFilename);
                    }
                }
                
                ProjectItem existingItem = Helpers.GetProjectItemByName(whereToAdd, targetFilename);
                if ((existingItem != null) && !overwrite)
                {
                  //item exists, do not write the content
                  return existingItem;
                }                 

                using (StreamWriter writer = new StreamWriter(finalpath, false))
                {
                    if (content.Contains("<?xml") || targetFilename.ToUpper().EndsWith(".XML"))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(content);

                        XmlWriter xw = XmlWriter.Create(writer, Helpers.GetXmlWriterSettings(targetFilename));
                        doc.Save(xw);
                        xw.Flush();
                        xw.Close();
                    }
                    else
                    {
                        writer.WriteLine(content);
                    }
                }


                //is the item already there
                if (existingItem != null)
                {
                    createdProjectItem = existingItem;
                }
                else
                {
                    createdProjectItem = Helpers.AddFromFile(whereToAdd, finalpath);
                }

                //if the parent item is a ISharePointProjectItem we add the item as a child
                //TODO


                if (open)
                {
                    Window window = createdProjectItem.Open("{00000000-0000-0000-0000-000000000000}");
                    window.Visible = true;
                    window.Activate();
                }
            }
            catch (Exception ex)
            {
                Helpers.LogMessage(dte, dte, ex.ToString());
                MessageBox.Show(ex.ToString());
            }

            return createdProjectItem;
        }

        /// <summary>
        /// Adds a file to the mapped folder
        /// </summary>
        /// <param name="dte"></param>
        /// <param name="project"></param>
        /// <param name="sPFileType"></param>
        /// <param name="evaluatedSourceFileName"></param>
        /// <param name="evaluatedTargetFileName"></param>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        internal static ProjectItem AddFileToMappedFolder(DTE dte, Project project, SPFileType sPFileType, string targetFolder, string evaluatedSourceFileName, string evaluatedTargetFileName, bool overwrite, bool open)
        {
            string completeDestinationPath = GetMappedFolderPath(dte, project, sPFileType, targetFolder);
            ProjectItems whereToAdd = Helpers.GetProjectItemsByPath(project, completeDestinationPath);

            return Helpers.AddFromTemplate(whereToAdd, evaluatedSourceFileName, evaluatedTargetFileName);
        }

        internal static ProjectItem AddTemplateToMappedFolder(DTE dte, Project project, SPFileType sPFileType, string targetFolder, string templateContent, string evaluatedTargetFileName, bool overwrite, bool open)
        {
            string relativeDestinationPath = GetMappedFolderPath(dte, project, sPFileType, targetFolder);
            ProjectItems whereToAdd = Helpers.GetProjectItemsByPath(project, relativeDestinationPath);
            return AddFile(dte, whereToAdd, evaluatedTargetFileName, templateContent, overwrite, open);
        }

        internal static ProjectItem AddTemplateToProjectItem(DTE dte, Project project, ProjectItem parentProjectItem, string templateContent, string evaluatedTargetFileName, bool overwrite, bool open)
        {
            ProjectItems whereToAdd = parentProjectItem.ProjectItems;
            return AddFile(dte, whereToAdd, evaluatedTargetFileName, templateContent, overwrite, open);
        }

        internal static ProjectItem AddTemplateToProjectItem(DTE dte, Project project, ProjectItem parentProjectItem, string targetFolder, string templateContent, string evaluatedTargetFileName, bool overwrite, bool open)
        {
            ProjectItem whereToAdd = Helpers.GetProjectFolder(parentProjectItem.ProjectItems, targetFolder, true);
            return AddFile(dte, whereToAdd.ProjectItems, evaluatedTargetFileName, templateContent, overwrite, open);
        }



        internal static ProjectItem AddFileToProjectItem(DTE dte, Project project, ProjectItem parentProjectItem, string sourcefilename, string evaluatedTargetFileName, bool overwrite, bool open)
        {
            ProjectItems whereToAdd = parentProjectItem.ProjectItems;
            return Helpers.AddFromTemplate(whereToAdd, sourcefilename, evaluatedTargetFileName);
        }

        internal static ProjectItem AddFileToProjectItem(DTE dte, Project project, ProjectItem parentProjectItem, string targetFolder, string sourcefilename, string evaluatedTargetFileName, bool overwrite, bool open)
        {
            ProjectItem whereToAdd = Helpers.GetProjectFolder(parentProjectItem.ProjectItems, targetFolder, true);
            return Helpers.AddFromTemplate(whereToAdd.ProjectItems, sourcefilename, evaluatedTargetFileName);
        }

        /// <summary>
        /// returns the relative path within the project to the mapped folder
        /// </summary>
        /// <param name="dte"></param>
        /// <param name="project"></param>
        /// <param name="sPFileType"></param>
        /// <param name="subFolderPath"></param>
        /// <returns></returns>
        internal static string GetMappedFolderPath(DTE dte, Project project, SPFileType sPFileType, string subFolderPath)
        {
            ProjectItems mappedFolder = GetMappedFolder(dte, project, sPFileType);

            string currentPath = "";
            if (mappedFolder.Parent is Project)
            {
                currentPath = Helpers.GetFullPathOfProjectItem(mappedFolder.Parent as Project);
            }
            else if (mappedFolder.Parent is ProjectItem)
            {
                currentPath = Helpers.GetFullPathOfProjectItem(mappedFolder.Parent as ProjectItem);
            }

            string completeDestinationPath = currentPath + subFolderPath;
            string projectPath = Helpers.GetFullPathOfProjectItem(project);
            completeDestinationPath = completeDestinationPath.Substring(projectPath.Length); //we need the relative path of the mapped folder
            return completeDestinationPath;
        }

        /// <summary>
        /// creates or returns the projectitem which points to the given target sharepoint folder
        /// </summary>
        /// <param name="dte"></param>
        /// <param name="project"></param>
        /// <param name="sPFileType"></param>
        /// <returns></returns>
        internal static ProjectItems GetMappedFolder(DTE dte, Project project, SPFileType sPFileType)
        {
            if (IsSharePointVSTemplate(dte, project))
            {
                if (sPFileType == SPFileType.TemplateFile)
                {
                    //find a folder which is mapped to images
                    return Helpers2.GetOrCreateMappedFolder(project, "TemplateFile", "Template");
                }
                else if (sPFileType == SPFileType.RootFile)
                {
                    //find a folder which is mapped to images
                    return Helpers2.GetOrCreateMappedFolder(project, "RootFile", "SharePointRoot");
                }
            }

            if (sPFileType == SPFileType.CustomCode)
            {
                return Helpers.GetProjectItemsByPath(project, "CustomCode");
            }
            if (sPFileType == SPFileType.ClassResource)
            {
                return Helpers.GetProjectItemsByPath(project, @"80\wpresources\" + Helpers.GetOutputNameWithoutExtensions(project));
            }
            if (sPFileType == SPFileType.AppGlobalResource)
            {
                return Helpers.GetProjectItemsByPath(project, @"80\resources");
            }
            if (sPFileType == SPFileType.BINFile)
            {
                return Helpers.GetProjectItemsByPath(project, @"80\BIN");
            }
            if (sPFileType == SPFileType.GACFile)
            {
                return Helpers.GetProjectItemsByPath(project, @"GAC");
            }

            //if not found return root
            return null;
        }

        public static ProjectItem GetFolderOfFeature(Project project, string featurename)
        {
            try
            {
                if (IsSharePointVSTemplate(project.DTE, project))
                {
                    return Helpers.FindProjectItemByPath(project, @"Features\" + featurename);
                }
                else
                {
                    return Helpers.GetProjectItemByName(Helpers.GetProjectItemByName(Helpers.GetProjectItemByName(Helpers.GetProjectItemByName(project.ProjectItems, GetCurrentSharePointRootName(project.DTE, project)).ProjectItems, "template").ProjectItems, "Features").ProjectItems, featurename);
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <param name="mappedFolderType"></param>
        /// <returns></returns>
        internal static ProjectItems GetOrCreateMappedFolder(Project project, string mappedFolderType, string folderNameForCreate)
        {
            //find a file with the following content
            /*<?xml version="1.0" encoding="utf-8"?>
            <ProjectItem 
             * Type="Microsoft.VisualStudio.SharePoint.MappedFolder" 
             * SupportedTrustLevels="FullTrust" 
             * SupportedDeploymentScopes="Package" 
             * xmlns="http://schemas.microsoft.com/VisualStudio/2010/SharePointTools/SharePointProjectItemModel">
                <ProjectItemFolder Target="" Type="TemplateFile" />
              </ProjectItem>
             * */

            ProjectItem mappedFolder = FindSharePointProjectItemInProject(project, "/ns:ProjectItem[@Type='Microsoft.VisualStudio.SharePoint.MappedFolder']/ns:ProjectItemFolder[@Type='" + mappedFolderType + "' and @Target='']");
            if (mappedFolder != null)
            {
                return mappedFolder.ProjectItems;
            }

            //1. create mapped folder
            ProjectItem newMappedFolder = project.ProjectItems.AddFolder(folderNameForCreate, EnvDTE.Constants.vsProjectItemKindPhysicalFolder);

            //2. add an SharePointProjectItem.spdata to the folder
            string spdatacontents = "";
            spdatacontents += "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            spdatacontents += "<ProjectItem Type=\"Microsoft.VisualStudio.SharePoint.MappedFolder\" SupportedTrustLevels=\"FullTrust\" SupportedDeploymentScopes=\"Package\" xmlns=\"http://schemas.microsoft.com/VisualStudio/2010/SharePointTools/SharePointProjectItemModel\" >";
            spdatacontents += "<ProjectItemFolder Target=\"\" Type=\"" + mappedFolderType + "\" />";
            spdatacontents += "</ProjectItem>";

            string spdataFilename = Path.Combine(Helpers.GetFullPathOfProjectItem(newMappedFolder), "SharePointProjectItem.spdata");
            File.WriteAllText(spdataFilename, spdatacontents);
            newMappedFolder.ProjectItems.AddFromFile(spdataFilename);

            Helpers.LogMessage(project.DTE, project.DTE, "New mapped folder " + folderNameForCreate + " created");

            //3. add the mapped folder to the package
            AddVSMappedFolderToVSPackage(project.DTE, project, newMappedFolder);

            return newMappedFolder.ProjectItems;
        }

        /// <summary>
        /// adds a mapped folder to the package of the project
        /// </summary>
        /// <param name="dte"></param>
        /// <param name="project"></param>
        /// <param name="newMappedFolder"></param>
        private static void AddVSMappedFolderToVSPackage(DTE dte, Project project, ProjectItem newMappedFolder)
        {
            ISharePointProjectService projectService = GetSharePointProjectService(dte);
            ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(project);

            //1. new added items need to be validated before 
            sharePointProject.Synchronize();

            //1. Get the project item of the features
            ProjectItem packageItem = Helpers.GetProjectItemByName(project.ProjectItems, "Package");
            if (packageItem == null)
            {
                throw new Exception("Package not found. Could not add feature to package.");
            }

            try
            {
                IMappedFolder sharePointMappedFolder = projectService.Convert<EnvDTE.ProjectItem, IMappedFolder>(newMappedFolder);
                if (!sharePointProject.Package.ProjectItems.Contains(sharePointMappedFolder))
                {
                    sharePointProject.Package.ProjectItems.Add(sharePointMappedFolder);//sharePointFeature);
                    Helpers.LogMessage(dte, dte, "Mapped folder " + sharePointMappedFolder.Name + " added to package " + sharePointProject.Package.Name);

                    ProjectItem packageFile = projectService.Convert<ISharePointProjectMember, EnvDTE.ProjectItem>(sharePointProject.Package.PackageFile);
                    Helpers.EnsureCheckout(dte, packageFile);
                    packageFile.Save();
                }
            }
            catch
            {
                Helpers.LogMessage(dte, dte, "Warning: Could not add mapped folder " + newMappedFolder.Name + " to package in project " + project.Name + ". Please add folder manually.");
            }
        }

        private static ProjectItem FindSharePointProjectItemInProject(Project project, string xPath)
        {
            ProjectItem res = null;
            foreach (ProjectItem childItem in project.ProjectItems)
            {
                if (res == null)
                {
                    res = FindSharePointProjectItem(childItem, xPath);
                }
            }
            return res;
        }

        private static ProjectItem FindSharePointProjectItem(ProjectItem projectItem, string xPath)
        {
            ProjectItem res = null;
            foreach (ProjectItem childItem in projectItem.ProjectItems)
            {
                try
                {
                    if (childItem.Name.Equals("SharePointProjectItem.spdata"))
                    {
                        string filename = Helpers.GetFullPathOfProjectItem(childItem);
                        XmlDocument spdataDoc = new XmlDocument();
                        spdataDoc.Load(filename);

                        XmlNamespaceManager nsmgr = new XmlNamespaceManager(spdataDoc.NameTable);
                        nsmgr.AddNamespace("ns", "http://schemas.microsoft.com/VisualStudio/2010/SharePointTools/SharePointProjectItemModel");

                        XmlNode node = spdataDoc.SelectSingleNode(xPath, nsmgr);
                        if (node != null)
                        {
                            //found
                            return projectItem;
                        }
                    }
                }
                catch { }
                if (res == null)
                {
                    res = FindSharePointProjectItem(childItem, xPath);
                }
            }
            return res;
        }

        /// <summary>
        /// Takes the folder of a spdata item and adds the elements xml to that item
        /// </summary>
        /// <param name="dte"></param>
        /// <param name="project"></param>
        /// <param name="contentTypeFolder"></param>
        /// <param name="elementsXmlContent"></param>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        internal static void MergeElementsXmlToWithSPDataItem(DTE dte, Project project, ProjectItem contentTypeFolder, string elementsXmlContent, bool p, bool p_2)
        {
            //get the path to the spdatafile
            string pathToSpdataFile = Path.Combine(Helpers.GetFullPathOfProjectItem(contentTypeFolder), "SharePointProjectItem.spdata");
            if (!File.Exists(pathToSpdataFile))
            {
                throw new FileNotFoundException("SpData file not found", pathToSpdataFile);
            }

            /*
            XmlDocument spDataXml = new XmlDocument();
            spDataXml.Load(pathToSpdataFile);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(spDataXml.NameTable);
            nsmgr.AddNamespace("ns", "http://schemas.microsoft.com/VisualStudio/2010/SharePointTools/SharePointProjectItemModel");

            XmlNode node = spDataXml.SelectSingleNode(xPath, nsmgr);
            XmlNode projectItemNode = 
             * 
             * */
        }

        /*

        /// <summary>
        /// Adds a feature to the package, returns the folder of the feature
        /// </summary>
        /// <param name="dte"></param>
        /// <param name="project"></param>
        /// <param name="finalFeatureName"></param>
        internal static ProjectItem AddVSFeatureToVSPackage(DTE dte, Project project, string finalFeatureName)
        {
            Trace.WriteLine("AddVSFeatureToVSPackage");

            //2. Get the feature
            ProjectItem featureItem = Helpers.FindProjectItemByPath(project, @"Features\" + finalFeatureName + @"\" + finalFeatureName + ".feature");
            if (featureItem == null)
            {
                throw new Exception("Feature " + finalFeatureName + " not found. Could not add feature to package.");
            }


            ISharePointProjectService projectService = GetSharePointProjectService(dte);
            ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(project);
            
            try
            {
                //1. new added items need to be validated before 
                sharePointProject.Synchronize();

                ISharePointProjectFeature sharePointFeature = null;
                foreach (ISharePointProjectFeature existingFeature in sharePointProject.Features)
                {
                    if (existingFeature.Name.Equals(finalFeatureName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        sharePointFeature = existingFeature;
                    }
                }

                if (sharePointFeature == null)
                {
                    throw new Exception("Feature with name " + finalFeatureName + " not found in project");
                }

                ISharePointProjectPackage sharePointPackage = sharePointProject.Package;
                if (!sharePointPackage.Features.Contains(sharePointFeature))
                {
                    Helpers.LogMessage(dte, dte, "Feature " + finalFeatureName + " add to package in project " + project.Name);
                    sharePointPackage.Features.Add(sharePointFeature);

                    ProjectItem packageFile = projectService.Convert<ISharePointProjectMember, EnvDTE.ProjectItem>(sharePointPackage.PackageFile);
                    packageFile.Save();
                }
            }
            catch(Exception ex)
            {
                Trace.WriteLine("Exception " + ex.Message);
                Helpers.LogMessage(dte, dte, "Warning: Could not add feature " + finalFeatureName + " to package in project " + project.Name + ". Please add feature manually.");
            }

            return featureItem;
        }
         * */

        internal static ISharePointProjectService GetSharePointProjectService(DTE dte)
        {
            EnvDTE80.DTE2 dte2 = (EnvDTE80.DTE2)dte;

            Microsoft.VisualStudio.Shell.ServiceProvider serviceProvider = new Microsoft.VisualStudio.Shell.ServiceProvider(dte2 as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
            Debug.Assert(serviceProvider != null);

            Microsoft.VisualStudio.SharePoint.ISharePointProjectService projectService = serviceProvider.GetService(typeof(Microsoft.VisualStudio.SharePoint.ISharePointProjectService)) as Microsoft.VisualStudio.SharePoint.ISharePointProjectService;
            if (projectService == null)
            {
                projectService = Package.GetGlobalService(typeof(Microsoft.VisualStudio.SharePoint.ISharePointProjectService)) as Microsoft.VisualStudio.SharePoint.ISharePointProjectService;
            }

            Debug.Assert(projectService != null);


            return projectService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dte"></param>
        /// <param name="sourceproject"></param>
        /// <param name="targetFolder"></param>
        /// <param name="deploymentType"></param>
        internal static void AddProjectOutputReferenceToFolder(DTE dte, Project sourceproject, ProjectItem targetFolder, SPFileType deploymentType)
        {
            if (IsSharePointVSTemplate(dte, targetFolder.ContainingProject))
            {
                ISharePointProjectService projectService = GetSharePointProjectService(dte);
                ISharePointProjectItem sharePointItem = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItem>(targetFolder);
                sharePointItem.ProjectOutputReferences.Add(sourceproject.FullName, GetDeploymentTypeFromFileType(deploymentType));
            }
            else
            {
                //todo
                //1. build dependency to sourceproject
                // Make Project1 dependent on Project2.
                BuildDependency bd = dte.Solution.SolutionBuild.BuildDependencies.Item(targetFolder.ContainingProject.UniqueName);
                bd.AddProject(sourceproject.UniqueName);
            }
        }

        internal static void AddBuildDependency(DTE dte, Project dependProject, Project sourceProject )
        {
            if (dependProject == null || sourceProject == null)
            {
                return;
            }
            //todo
            //1. build dependency to sourceproject
            // Make Project1 dependent on Project2.
            BuildDependency bd = dte.Solution.SolutionBuild.BuildDependencies.Item(dependProject.UniqueName);
            bd.AddProject(sourceProject.UniqueName);
        }

        internal static void AddVSElementToVSFeature(DTE dte, Project project, ProjectItem elementFolder, string evaluatedParentFeatureName)
        {

            ISharePointProjectService projectService = GetSharePointProjectService(dte);
            ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(project);
          
            //1. new added items need to be validated before 
            sharePointProject.Synchronize();

            //1. Get the feature
            ProjectItem featureItem = Helpers.FindProjectItemByPath(project, @"Features\" + evaluatedParentFeatureName);
            if (featureItem == null)
            {
                throw new Exception("Feature " + evaluatedParentFeatureName + " not found. Could not add feature to package.");
            }

            ISharePointProjectFeature sharePointFeature = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectFeature>(featureItem);
            ISharePointProjectItem sharePointItem = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItem>(elementFolder);

            try
            {
              //sometimes the element for a feature is automatically added to the package, so we remove the element first from the package
              if (sharePointProject.Package.ProjectItems.Contains(sharePointItem))
              {
                Trace.WriteLine("Removed " + sharePointItem.Name + " sharePointItem from package");
                sharePointProject.Package.ProjectItems.Remove(sharePointItem);
                //sharePointProject.Synchronize();
              }
            }
            catch
            { 
            }

            if (!sharePointFeature.ProjectItems.Contains(sharePointItem))
            {
              //check in all features if it is already there
              foreach(ISharePointProjectFeature existingFeature in sharePointProject.Features)
              {
                if (existingFeature.ProjectItems.Contains(sharePointItem) && (existingFeature.Name != sharePointFeature.Name))
                {
                  Helpers.LogMessage(dte, dte, "Removed element '" + sharePointItem.Name + "' from feature '" + existingFeature.Name + "'");
                  existingFeature.ProjectItems.Remove(sharePointItem);
                }
              }

              try
              {
                Trace.WriteLine("Feature not contains " + sharePointItem.Name);
                sharePointFeature.ProjectItems.Add(sharePointItem);
                Helpers.LogMessage(dte, dte, "Added element '" + sharePointItem.Name + "' to feature '" + sharePointFeature.Name + "'");
                  
                sharePointProject.Synchronize();
              }
              catch
              {
              }
            }
            else
            {
                //Trace.WriteLine("AddElementsDefinitionAction " + evaluatedParentFeatureName + "already contains " + sharePointItem.Name);
                //Trace.WriteLine("  Following item are alreay in feature " + evaluatedParentFeatureName);
                foreach (ISharePointProjectItem item in sharePointFeature.ProjectItems)
                {
                    //Trace.WriteLine("  - " + item.Name);                    
                }
            }

            ProjectItem featureFile = projectService.Convert<ISharePointProjectMember, EnvDTE.ProjectItem>(sharePointFeature.FeatureFile);
            Helpers.EnsureCheckout(dte, featureFile);
            featureFile.Save();

            try
            {
              //TODO: nur gemacht für die Tests
              ProjectItem packageFile = projectService.Convert<ISharePointProjectMember, EnvDTE.ProjectItem>(sharePointProject.Package.PackageFile);
              packageFile.Save();
            }
            catch 
            { 
            }
        }

        internal static void AddVSElementToVSPackage(DTE dte, Project project, ProjectItem elementFolder)
        {
            ISharePointProjectService projectService = GetSharePointProjectService(dte);
            ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(project);

            //1. new added items need to be validated before 
            sharePointProject.Synchronize();

            ISharePointProjectItem sharePointItem = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItem>(elementFolder);

            //sometimes the element for a feature is automatically added to the package, so we remove the element first from the package
            if (!sharePointProject.Package.ProjectItems.Contains(sharePointItem))
            {
              try
              {
                sharePointProject.Package.ProjectItems.Add(sharePointItem);
                ProjectItem packageFile = projectService.Convert<ISharePointProjectMember, EnvDTE.ProjectItem>(sharePointProject.Package.PackageFile);

                Helpers.EnsureCheckout(dte, packageFile);
                packageFile.Save();
              }
              catch
              {
              }
            }
        }

        /// <summary>
        /// returns true if the content of the template is empty
        /// </summary>
        /// <param name="templateContent"></param>
        /// <returns></returns>
        internal static bool TemplateContentIsEmpty(string templateContent)
        {
            templateContent = templateContent.Trim(new char[] { ' ', '\t', '\n', '\r' });
            if ((templateContent == "") || (templateContent == "dummy"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a file to a project folder 
        /// </summary>
        /// <param name="dte"></param>
        /// <param name="project"></param>
        /// <param name="ParentElementFolder"></param>
        /// <param name="sPFileType"></param>
        /// <param name="evaluatedTargetFolder"></param>
        /// <param name="fullSourceFileName"></param>
        /// <param name="evaluatedTargetFileName"></param>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        /// <returns></returns>
        internal static ProjectItem AddFileToElementManifest(DTE dte, Project project, ProjectItem ParentElementFolder, SPFileType sPFileType, string evaluatedTargetFolder, string fullSourceFileName, string evaluatedTargetFileName, bool overwrite, bool open)
        {
            if (IsSharePointVSTemplate(dte, project))
            {
                //1. place the file in the folder project item
                ProjectItem addedFile = Helpers.AddFromTemplate(ParentElementFolder.ProjectItems, fullSourceFileName, evaluatedTargetFileName);

                try
                {
                  //2. in VS add a reference in the parent element
                  ISharePointProjectService projectService = GetSharePointProjectService(dte);
                  ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(project);

                  //1. new added items need to be validated before 
                  sharePointProject.Synchronize();

                  ISharePointProjectItem parentSharePointItem = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItem>(ParentElementFolder);

                  ISharePointProjectItemFile addedSharePointItem = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItemFile>(addedFile);
                  addedSharePointItem.DeploymentType = GetDeploymentTypeFromFileType(sPFileType);
                  addedSharePointItem.DeploymentPath = evaluatedTargetFolder;

                  parentSharePointItem.Files.AddFromFile(addedSharePointItem.FullPath);
                }
                catch
                { 
                }

                return addedFile;
            }
            else
            {
                if (sPFileType == SPFileType.ElementFile)
                {
                    //file needs to be palced inside a subfolder of a feature (parameter 'ParentElementFolder')
                    return Helpers.AddFromTemplate(ParentElementFolder.ProjectItems, fullSourceFileName, evaluatedTargetFileName);
                }
                else
                {
                    return AddFileToMappedFolder(dte, project, sPFileType, evaluatedTargetFolder, fullSourceFileName, evaluatedTargetFileName, overwrite, open);
                }
            }
        }

        internal static ProjectItem AddTemplateToElementManifest(DTE dte, Project project, ProjectItem parentProjectItem, SPFileType sPFileType, string evaluatedTargetFolder, string templateContent, string evaluatedTargetFileName, bool overwrite, bool open)
        {
            if (IsSharePointVSTemplate(dte, project))
            {
                //1. place the file in the folder project item
                ProjectItems whereToAdd = parentProjectItem.ProjectItems;
                ProjectItem addedFile = AddFile(dte, whereToAdd, evaluatedTargetFileName, templateContent, true, false);

                try
                {
                  //2. in VS add a reference in the parent element
                  ISharePointProjectService projectService = GetSharePointProjectService(dte);
                  ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(project);

                  //1. new added items need to be validated before 
                  sharePointProject.Synchronize();

                  ISharePointProjectItem parentSharePointItem = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItem>(parentProjectItem);

                  ISharePointProjectItemFile addedSharePointItem = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItemFile>(addedFile);
                  addedSharePointItem.DeploymentType = GetDeploymentTypeFromFileType(sPFileType);
                  addedSharePointItem.DeploymentPath = evaluatedTargetFolder;

                  parentSharePointItem.Files.AddFromFile(addedSharePointItem.FullPath);
                }
                catch
                {
                }
                return addedFile;
            }
            else
            {
                if (sPFileType == SPFileType.ElementFile)
                {
                    //file needs to be palced inside a subfolder of a feature (parameter 'ParentElementFolder')
                    return AddTemplateToProjectItem(dte, project, parentProjectItem, evaluatedTargetFolder, templateContent, evaluatedTargetFileName, overwrite, open);
                }
                else
                {
                    return AddTemplateToMappedFolder(dte, project, sPFileType, evaluatedTargetFolder, templateContent, evaluatedTargetFileName, overwrite, open);
                }
            }
        }

        internal static DeploymentType GetDeploymentTypeFromFileType(SPFileType sPFileType)
        {
            if (sPFileType == SPFileType.RootFile)
            {
                return DeploymentType.RootFile;
            }
            else if (sPFileType == SPFileType.TemplateFile)
            {
                return DeploymentType.TemplateFile;
            }
            else if (sPFileType == SPFileType.ElementFile)
            {
                return DeploymentType.ElementFile;
            }
            else if (sPFileType == SPFileType.ElementManifest)
            {
                return DeploymentType.ElementManifest;
            }
            else if (sPFileType == SPFileType.CustomCode)
            {
                return DeploymentType.NoDeployment;
            }
            else if (sPFileType == SPFileType.AppGlobalResource)
            {
              return DeploymentType.AppGlobalResource;
            }
            else if (sPFileType == SPFileType.ClassResource)
            {
              return DeploymentType.ClassResource;
            }
            else if (sPFileType == SPFileType.Resource)
            {
              return DeploymentType.Resource;
            }
            return DeploymentType.RootFile;
        }

        public static void AddExternalItems(DTE dte, List<NameValueItem> _list, string internalXPath, string internalNamespace, XmlNodeHandler nodeHandler)
        {
            string configfilename = "";

            if (dte != null)
            {
                string solutionpath = dte.Solution.FullName;
                solutionpath = solutionpath.Substring(0, solutionpath.LastIndexOf("\\"));

                configfilename = Path.Combine(solutionpath, "SharepointConfiguration.xml");

                if (!File.Exists(configfilename))
                {
                    //SharePointConfiguration not found at expected place
                    configfilename = Helpers.GetBasePath() + @"\Templates\Text\SharePointConfiguration.xml";
                }

                if (!File.Exists(configfilename))
                {
                    if (MessageBox.Show("No file 'SharePointConfiguration.xml' found in solution. This file contains a list of all feature, content types, list templates etc. in the target system. " + Environment.NewLine + "Do you want to create this file from your local SharePoint instalation?", "SharePoint Configuration", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        SharePointConfigurationHelper.CreateSharePointConfigurationFile(dte);
                        configfilename = Path.Combine(solutionpath, "SharepointConfiguration.xml");
                    }
                }

                if (File.Exists(configfilename))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(configfilename);

                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("ns", internalNamespace);
                    foreach (XmlNode node in doc.SelectNodes(internalXPath, nsmgr))
                    {
                        foreach(NameValueItem nvitem in nodeHandler.GetNameValueItems(node, null, null))
                        {
                            if (nvitem != null)
                            {
                                _list.Add(nvitem);
                            }
                        }
                    }
                }
                else
                {

                }
            }
        }

        public static void AddInternalItems(DTE dte, List<NameValueItem> _list, string internalXPath, string internalNamespace, XmlNodeHandler nodeHandler, string extension)
        {
          nodeHandler.SetGroupName(SPSFConstants.ThisSolutionNodeText);
          nodeHandler.SetGlobalResourcesDictionary(Helpers.GetResourcesInSolution(dte));
          if (dte != null)
          {
            foreach (Project project in dte.Solution.Projects)
            {
              Helpers.NavigateProjectItems(project.ProjectItems, _list, internalXPath, internalNamespace, nodeHandler, extension);
            }
          }
        }

        public static void AddInternalItems(DTE dte, List<NameValueItem> _list, string internalXPath, string internalNamespace, XmlNodeHandler nodeHandler)
        {
          AddInternalItems(dte, _list, internalXPath, internalNamespace, nodeHandler, ".xml");
        }

        public static void AddInternalItems(DTE dte, bool startFromSelecteditem, List<NameValueItem> _list, string internalXPath, string internalNamespace, XmlNodeHandler nodeHandler)
        {
            nodeHandler.SetGroupName(SPSFConstants.ThisSolutionNodeText);
            nodeHandler.SetGlobalResourcesDictionary(Helpers.GetResourcesInSolution(dte));
            if (dte != null)
            {
                if (startFromSelecteditem)
                {
                    if (dte.SelectedItems.Count > 0)
                    {
                        try
                        {
                            SelectedItem item = dte.SelectedItems.Item(1);
                            if (item.ProjectItem != null)
                            {
                                Helpers.NavigateProjectItem(item.ProjectItem, _list, internalXPath, internalNamespace, nodeHandler, ".xml");
                            }
                            else if (item.Project != null)
                            {
                                Helpers.NavigateProjectItems(item.Project.ProjectItems, _list, internalXPath, internalNamespace, nodeHandler, ".xml");
                            }
                        }
                        catch { }
                    }
                }
                else
                {
                    foreach (Project project in dte.Solution.Projects)
                    {
                        Helpers.NavigateProjectItems(project.ProjectItems, _list, internalXPath, internalNamespace, nodeHandler, ".xml");
                    }
                }
            }
        }

        /// <summary>
        /// returns true if the solution uses the SharePoint template
        /// </summary>
        /// <param name="dTE"></param>
        /// <returns></returns>
        internal static bool SolutionHasSharePointTemplate(DTE dTE)
        {
            try
            {
                ISharePointProjectService projectService = Helpers2.GetSharePointProjectService(dTE);

                foreach (ISharePointProject sharePointProject in projectService.Projects)
                {
                    try
                    {
                        if (sharePointProject != null)
                        {
                            return true;
                        }
                    }
                    catch
                    {

                    }
                }
            }
            catch { }

          // bugfix: if there is no project in the solution we could check for GenerateManifest.tt
            string solutionDirectory = Path.GetDirectoryName((string)dTE.Solution.Properties.Item("Path").Value);
            string generateManifest = Path.Combine(solutionDirectory, "GenerateManifest.tt");
            if (File.Exists(generateManifest))
            {
              //assume HIVE
              return false;
            }
            else
            {
              return true;
            }
        }

        internal static void DeleteDummyFile(DTE service, ProjectItem _ParentItem, bool showWarnings)
        {
            string _Filename = "dummy.txt";

            if ((_Filename == null) || (_Filename == ""))
            {
                return;
            }

          //if project has been reloaded the parent item is not null, but not valid anymore
            try
            {
              string test = _ParentItem.Name;
            }
            catch 
            { 
              //ok, item is not valid
              _ParentItem = null;
            }

            Project _CurrentProject = null;
            try
            {
                ProjectItem dummy = null;

                if (_ParentItem == null)
                {
                    //ok, the item should be in the project
                    _CurrentProject = Helpers.GetSelectedProject(service);
                    dummy = Helpers.GetProjectItemByName(_CurrentProject.ProjectItems, _Filename);

                    if (dummy == null)
                    {
                        ProjectItems items = Helpers.GetItemsOfSelectedItem(service);
                        dummy = Helpers.GetProjectItemByName(items, _Filename);
                        if (dummy == null)
                        {
                            if (showWarnings)
                            {
                                //Helpers.LogMessage(service, service, "Could not delete dummy file " + _Filename);
                            }
                        }
                    }
                }
                else
                {
                    if (_ParentItem.Name == "dummy.txt")
                    {
                        dummy = _ParentItem;
                    }
                    else
                    {
                        dummy = Helpers.GetProjectItemByName(_ParentItem.ProjectItems, _Filename);
                    }
                }


                if (dummy == null)
                {
                    if (showWarnings)
                    {
                        //Helpers.LogMessage(service, service, "Could not delete dummy file " + _Filename);
                    }
                }
                else
                {
                    string fullFilepath = Helpers.GetFullPathOfProjectItem(dummy);
                    //Helpers.LogMessage(service, this, "Deleting dummy file " + _Filename);
                    dummy.Remove();
                    File.Delete(fullFilepath);

                    try
                    {
                        //can we convert the feature to a sharepointfeature and check the scope???
                        ISharePointProjectService projectService = Helpers2.GetSharePointProjectService(service);
                        ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(_CurrentProject);
                        sharePointProject.Synchronize();
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                if (showWarnings)
                {
                    Helpers.LogMessage(service, service, "Could not delete dummy file " + _Filename + ": " + ex.ToString());
                }
            }
        }

        /// <summary>
        /// Adds a feature to the package, returns the folder of the feature
        /// </summary>
        /// <param name="dte"></param>
        /// <param name="project"></param>
        /// <param name="finalFeatureName"></param>
        internal static ProjectItem AddVSFeatureToVSPackage(DTE dte, Project project, string finalFeatureName)
        {
            
            //2. Get the feature
            ProjectItem featureItem = Helpers.FindProjectItemByPath(project, @"Features\" + finalFeatureName + @"\" + finalFeatureName + ".feature");
            if (featureItem == null)
            {
                throw new Exception("Feature " + finalFeatureName + " not found. Could not add feature to package.");
            }

            ISharePointProjectService projectService = GetSharePointProjectService(dte);
            ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(project);

            try
            {
                //1. new added items need to be validated before 
                sharePointProject.Synchronize();

                ISharePointProjectFeature sharePointFeature = null;
                foreach (ISharePointProjectFeature existingFeature in sharePointProject.Features)
                {
                    if (existingFeature.Name.Equals(finalFeatureName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        sharePointFeature = existingFeature;
                    }
                }

                if (sharePointFeature == null)
                {
                    throw new Exception("Feature with name " + finalFeatureName + " not found in project");
                }



                ISharePointProjectPackage sharePointPackage = sharePointProject.Package;
                if (!sharePointPackage.Features.Contains(sharePointFeature))
                {
                    Helpers.LogMessage(dte, dte, "Feature " + finalFeatureName + " add to package in project " + project.Name);
                    sharePointPackage.Features.Add(sharePointFeature);

                    try
                    {
                      ProjectItem packageFile = projectService.Convert<ISharePointProjectMember, EnvDTE.ProjectItem>(sharePointPackage.PackageFile);
                      packageFile.Save();
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Exception " + ex.Message);
                Helpers.LogMessage(dte, dte, "Warning: Could not add feature " + finalFeatureName + " to package in project " + project.Name + ". Please add feature manually.");
            }

            return featureItem;
        }

        /*
        /// <summary>
        /// Adds a new element of the given type to a vs2010 project
        /// </summary>
        /// <param name="dte"></param>
        /// <param name="project"></param>
        /// <param name="evaluatedElementsCategory"></param>
        /// <param name="evaluatedElementsName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static ProjectItem AddProjectItemToVSTemplate(DTE dte, Project project, string evaluatedElementsCategory, string evaluatedElementsName, string type)
        {
            ProjectItem result = null;

            ISharePointProjectService projectService = GetSharePointProjectService(dte);
            ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(project);

            if (!string.IsNullOrEmpty(evaluatedElementsCategory))
            {
                //we create the element in the project root
                ISharePointProjectItem newItem = sharePointProject.ProjectItems.Add(evaluatedElementsCategory, evaluatedElementsName, type, true);
                result = projectService.Convert<ISharePointProjectItem, EnvDTE.ProjectItem>(newItem); 
            }
            else
            {
                
                //we create the element in the project root
                ISharePointProjectItem newItem = sharePointProject.ProjectItems.Add(evaluatedElementsName, type, true);
                result = projectService.Convert<ISharePointProjectItem, EnvDTE.ProjectItem>(newItem); 
            }         

            return result;
        }
        */

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dte"></param>
        /// <param name="project"></param>
        /// <param name="finalFeatureName"></param>
        /// <param name="FeatureScope"></param>
        /// <param name="featureTemplateContent"></param>
        /// <returns>ProjectItem FeatureName.feature</returns>
        internal static ProjectItem AddFeatureToVSProject(DTE dte, Project project, string finalFeatureName, string featureScope, string featureId, string featureTitle, string featureDescription, string featureTemplateContent, out ProjectItem CreatedProjectFolder)
        {
            ProjectItem result = null;


            ISharePointProjectService projectService = GetSharePointProjectService(dte);
            ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(project);

            ISharePointProjectFeature featureItem = sharePointProject.Features.Add(finalFeatureName, false);

            //set the properties
            featureItem.Model.FeatureId = new Guid(featureId);
            featureItem.Model.Title = featureTitle;
            featureItem.Model.Description = featureDescription;

            //set the scope
            switch(featureScope)
            {
                case "Web":
                    featureItem.Model.Scope = Microsoft.VisualStudio.SharePoint.Features.FeatureScope.Web;
                    break;
                case "Site":
                    featureItem.Model.Scope = Microsoft.VisualStudio.SharePoint.Features.FeatureScope.Site;
                    break;
                case "WebApplication":
                    featureItem.Model.Scope = Microsoft.VisualStudio.SharePoint.Features.FeatureScope.WebApplication;
                    break;
                case "Farm":
                    featureItem.Model.Scope = Microsoft.VisualStudio.SharePoint.Features.FeatureScope.Farm;
                    break;
                default:
                    featureItem.Model.Scope = Microsoft.VisualStudio.SharePoint.Features.FeatureScope.Web;
                    break;
            }

            //set return values
            CreatedProjectFolder = projectService.Convert<ISharePointProjectFeature, EnvDTE.ProjectItem>(featureItem);
            result = projectService.Convert<ISharePointProjectMember, EnvDTE.ProjectItem>(featureItem.FeatureFile);

            if (!string.IsNullOrEmpty(featureTemplateContent))
            {
                //set the content of the FeatureName.template.feature
                string pathToTemplateFile = featureItem.ManifestTemplateFile.FullPath;
                File.WriteAllText(pathToTemplateFile, featureTemplateContent);
            }

            return result;
        }
         * */

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementFolder">Folder e.g. ContentType1 (which holds the SharePointProjectItem.spdata)</param>
        /// <param name="CreatedElementFile">Elements.xml file</param>
        internal static void AddElementsXmlToProjectItem(DTE dte, Project project, ProjectItem elementFolder, ProjectItem CreatedElementFile)
        {

            ISharePointProjectService projectService = GetSharePointProjectService(dte);
            ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(project);
            sharePointProject.Synchronize();

            ISharePointProjectItem elementFolderItem = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItem>(elementFolder);
            ISharePointProjectItemFile elementXmlFile = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItemFile>(CreatedElementFile);

            elementFolderItem.Files.AddFromFile(elementXmlFile.FullPath);

            elementFolderItem.DefaultFile = elementXmlFile;
            
        }
        */

        internal static void MoveProjectToSolutionFolder(DTE dte, string ProjectName, string SolutionFolder)
        {
            try
            {
                Solution2 soln = (Solution2)dte.Solution;
                Project targetSolutionFolder = Helpers.GetSolutionFolder(soln, SolutionFolder);
                if (targetSolutionFolder == null)
                {
                    targetSolutionFolder = soln.AddSolutionFolder(SolutionFolder);
                }

                SolutionFolder destinationSolutionFolder = targetSolutionFolder.Object as SolutionFolder;

                //find the project by name
                Project projectToBeMoved = Helpers.GetProjectByName(dte, ProjectName);
                string projectFileName = projectToBeMoved.FullName;

                //is the project already in this solution folder?
                foreach (ProjectItem pitem in destinationSolutionFolder.Parent.ProjectItems)
                {
                    if (pitem.Object != null)
                    {
                        if (pitem.Object is Project)
                        {
                            if ((pitem.Object as Project).Name.Equals(ProjectName, StringComparison.InvariantCultureIgnoreCase))
                            {
                                return;
                            }
                        }
                    }
                }

                Helpers.LogMessage(dte, dte, "Moving project '" + ProjectName + "' to solution folder '" + SolutionFolder + "'");

                //remove the project from the solution
                dte.Solution.Remove(projectToBeMoved);

                //add the project to the solution folder
                Project addedproject = destinationSolutionFolder.AddFromFile(projectFileName);

                dte.Solution.SaveAs(dte.Solution.FullName);
            }
            catch
            {
                Helpers.LogMessage(dte, dte, "Error: Could not move project '" + ProjectName + "' to solution folder '" + SolutionFolder + "'");

            }
        }

        internal static bool IsFileDeployableToHive(string sourcefilename)
        {
            if (sourcefilename.EndsWith(".cs", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            return true;
        }

        internal static void SetDeploymentType(ProjectItem CreatedElementFile, SPFileType sPFileType)
        {
            try
            {
                ISharePointProjectService projectService = GetSharePointProjectService(CreatedElementFile.DTE);
                ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(CreatedElementFile.ContainingProject);
                sharePointProject.Synchronize();
                ISharePointProjectItemFile sharePointItem = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItemFile>(CreatedElementFile);
              
                sharePointItem.DeploymentType = GetDeploymentTypeFromFileType(sPFileType);
            }
            catch (Exception)
            {
            }
        }

        internal static void SetDeploymentPath(ProjectItem CreatedElementFile, string DeploymentPath)
        {
          try
          {
            ISharePointProjectService projectService = GetSharePointProjectService(CreatedElementFile.DTE);
            ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(CreatedElementFile.ContainingProject);
            sharePointProject.Synchronize();
            ISharePointProjectItemFile sharePointItem = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItemFile>(CreatedElementFile);

            sharePointItem.DeploymentPath = DeploymentPath;
          }
          catch (Exception)
          {
          }
        }

        internal static void CopyDeploymentPath(ProjectItem SourceElementFile, ProjectItem TargetElementFile)
        {
            try
            {
                ISharePointProjectService projectService = GetSharePointProjectService(SourceElementFile.DTE);
                ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(SourceElementFile.ContainingProject);
                sharePointProject.Synchronize();

                ISharePointProjectItemFile sourceItem = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItemFile>(SourceElementFile);
                ISharePointProjectItemFile targetItem = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItemFile>(TargetElementFile);

                targetItem.DeploymentType = sourceItem.DeploymentType;
                targetItem.DeploymentPath = sourceItem.DeploymentPath;
                
            }
            catch (Exception ex)
            {
                Helpers.LogMessage(SourceElementFile.DTE, SourceElementFile.DTE, "Could not copy DeploymentPath of files: " + ex.Message);
            }
        }

        internal static string GetFeatureIdOfProjectItem(ProjectItem item)
        {
          if (Helpers2.IsSharePointVSTemplate(item.DTE, item.ContainingProject))
          {
            try
            {
              
              //get all features in all projects and iterate through all feature
              ISharePointProjectService projectService = GetSharePointProjectService(item.DTE);
              ISharePointProjectItem sharePointItem = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItem>(item);
              if(sharePointItem == null)
              {
                //unable to convert file to ISharePointProjectItem, try the parent
                if(item.Collection.Parent is ProjectItem)
                {
                  sharePointItem = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItem>(item.Collection.Parent as ProjectItem);
                }
              }
              foreach (Project project in Helpers.GetAllProjects(item.DTE))
              {
                try
                {
                  ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(project);
                  foreach (ISharePointProjectFeature existingFeature in sharePointProject.Features)
                  {
                    if (existingFeature.ProjectItems.Contains(sharePointItem))
                    {
                      return existingFeature.Model.FeatureId.ToString();
                    }
                  }
                }
                catch
                {
                }
              }           
            }
            catch (Exception)
            {
            }
          }
          else
          {
            try
            {
              ProjectItem featureFolder = Helpers.GetFeatureFolderByProjectItem(item);
              if(featureFolder != null)
              {
                ProjectItem featureXmlItem = Helpers.GetProjectItemByName(featureFolder.ProjectItems, "feature.xml");
                if(featureXmlItem != null)
                {
                  return Helpers.GetFeatureId(featureXmlItem);
                }
              }
            }
            catch
            {
            }            
          }
          return "";
        }
    }
}