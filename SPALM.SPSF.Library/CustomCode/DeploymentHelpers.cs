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
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.CodeDom.Compiler;
using EnvDTE80;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using SPALM.SPSF.SharePointBridge;
using SPALM.SPSF.Library;
using System.Net;
#endregion
using Microsoft.VisualStudio.SharePoint;

namespace SPALM.SPSF.Library
{
    public static class DeploymentHelpers
    {
        internal static bool CheckRebuildForSelectedProjects(DTE dte)
        {
            Helpers.LogMessage(dte, dte, "*** Check selected projects if rebuild is required ***");           
            List<Project> projects = Helpers.GetSelectedDeploymentProjects(dte);
            foreach (Project project in projects)
            {
                if (!DeploymentHelpers.CheckRebuildForProject(dte, project))
                {
                    return false;
                }
            }
            return true;
        }

        internal static bool CheckRebuildForProject(DTE dte, Project project)
        {
            Helpers.LogMessage(dte, dte, "Check project '" + project.Name + "' if rebuild is required");         
            string wspfilename = GetWSPFilePath(dte, project);
            if ((Helpers.RebuildRequired(project)) || !File.Exists(wspfilename))
            {
                if (MessageBox.Show("Rebuild of project project " + project.Name + " is required. Rebuild?", "Rebuild?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    dte.Solution.SolutionBuild.BuildProject(project.ConfigurationManager.ActiveConfiguration.ConfigurationName, project.UniqueName, true);
                    if (dte.Solution.SolutionBuild.LastBuildInfo != 0)
                    {
                        Helpers.LogMessage(dte, dte, "Error: Build of project '" + project.Name + "' failed");   
                        return false;
                    }
                }
            }
            return true;
        }

        internal static void RedeploySolutions(DTE dte)
        {
            Helpers.ShowProgress(dte, "Redeploying solutions...", 30);
            Helpers.LogMessage(dte, dte, "*** Redeploying selected solutions ***");
            DeploymentHelpers.RedeployProject(dte, Helpers.GetSelectedDeploymentProjects(dte));
            Helpers.HideProgress(dte);
        }

        internal static void DeploySolutions(DTE dte)
        {
            Helpers.ShowProgress(dte, "Deploying solutions...", 30);
            Helpers.LogMessage(dte, dte, "*** Deploying selected solutions ***");
            DeploymentHelpers.DeployProject(dte, Helpers.GetSelectedDeploymentProjects(dte));
            Helpers.ShowProgress(dte, "Deploying solutions...", 60);
            Helpers.HideProgress(dte);
        }

        internal static void UndeploySolutions(DTE dte)
        {
            Helpers.ShowProgress(dte, "Undeploying solutions...", 30);
            Helpers.LogMessage(dte, dte, "*** Undeploying selected solutions ***");
            DeploymentHelpers.UndeployProject(dte, Helpers.GetSelectedDeploymentProjects(dte));
            Helpers.ShowProgress(dte, "Undeploying solutions...", 60);
            Helpers.HideProgress(dte);
        }

        internal static void UpgradeSolutions(DTE dte)
        {
            Helpers.ShowProgress(dte, "Upgrading solutions...", 30);
            Helpers.LogMessage(dte, dte, "*** Upgrading selected solutions ***");
            DeploymentHelpers.UpgradeProject(dte, Helpers.GetSelectedDeploymentProjects(dte));
            Helpers.HideProgress(dte);
        }

        internal static void UndeployProject(DTE dte, List<Project> projects)
        {
            PerformDeploymentOperation(dte, projects, "undeploy");
        }

        internal static void DeployProject(DTE dte, List<Project> projects)
        {
            PerformDeploymentOperation(dte, projects, "deploy");
        }

        internal static void RedeployProject(DTE dte, Project project)
        {
            List<Project> projects = new List<Project>();
            projects.Add(project);
            PerformDeploymentOperation(dte, projects, "redeploy");
        }

        internal static void RedeployProject(DTE dte, List<Project> projects)
        {
            PerformDeploymentOperation(dte, projects, "redeploy");
        }

        internal static void UpgradeProject(DTE dte, List<Project> projects)
        {
            PerformDeploymentOperation(dte, projects, "upgrade");
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


        /// <summary>
        /// Returns true if the template of the given project is a SharePoint Template
        /// </summary>
        /// <param name="dte"></param>
        /// <param name="selectedProject"></param>
        /// <returns></returns>
        internal static bool IsSharePointVSTemplate(DTE dte, Project selectedProject)
        {
            string projectTypeGuids = GetProjectTypeGuids(selectedProject);
            // SP14 BB1F664B-9266-4FD6-B973-E1E44974B511
            // SP15 C1CDDADD-2546-481F-9697-4EA41081F2FC 
            if (projectTypeGuids.ToUpper().Contains("{BB1F664B-9266-4FD6-B973-E1E44974B511}") ||
                projectTypeGuids.ToUpper().Contains("{C1CDDADD-2546-481F-9697-4EA41081F2FC}") )
            {
                return true;
            }
            return false;
        }

        internal static string GetOutputDLL(DTE dte, Project _CurrentProject)
        {
            string dllName = _CurrentProject.Properties.Item("OutputFileName").Value.ToString();
            string fullPath = Helpers.GetFullPathOfProjectItem(_CurrentProject);
            string outputPath = _CurrentProject.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value.ToString();
            string outputDir = Path.Combine(fullPath, outputPath);
            return Path.Combine(outputDir, dllName);
        }

        public static string GetWSPFilePath(DTE dte, Project _CurrentProject)
        {
            string wspname = GetWSPName(dte, _CurrentProject);

            //search wsp file e.g. is in /bin/Debug
            string fullPath = Helpers.GetFullPathOfProjectItem(_CurrentProject);
            string outputPath = _CurrentProject.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value.ToString();
            string outputDir = Path.Combine(fullPath, outputPath);
            string wspfile1 = Path.Combine(outputDir, wspname);
            return wspfile1;

            /*
            if(File.Exists(wspfile1))
            {
            


                string projectpath = Helpers.GetFullPathOfProjectItem(_CurrentProject);
                string projectfolder = projectpath.Substring(0, projectpath.LastIndexOf("\\"));
                return Path.Combine(Path.Combine(projectfolder, "Batches"), wspname);
            }
            */
        }

        internal static string GetWSPName(DTE dte, Project _CurrentProject)
        {
            string wspname = _CurrentProject.Properties.Item("OutputFileName").Value.ToString();
            wspname = Path.GetFileNameWithoutExtension(wspname) + ".wsp";
            return wspname;
        }

        internal static void PerformDeploymentOperation(DTE dte, List<Project> projects, string operation)
        {
            List<SharePointDeploymentJob> wspFiles = new List<SharePointDeploymentJob>();
            foreach (Project project in projects)
            {
                SharePointDeploymentJob job = new SharePointDeploymentJob();
                job.WSPName = GetWSPName(dte, project);
                job.WSPFilename = GetWSPFilePath(dte, project);
                job.TargetSiteUrl = GetSiteUrl(dte, project);
                job.IsSandBoxedSolution = Helpers.GetIsSandboxedSolution(project);

                if (!File.Exists(job.WSPFilename))
                {
                    Helpers.LogMessage(dte, dte, "Error: WSP file not found for project '" + project.Name + "' finished (using '" + job.WSPFilename + "')");
                }
                else
                {
                    wspFiles.Add(job);
                }
            }
            if (wspFiles.Count > 0)
            {
                WorkerObject workerObject = new WorkerObject();
                workerObject.DTE = dte;
                workerObject.Operation = operation;
                workerObject.WSPFiles = wspFiles;

                worker = new BackgroundWorker();
                worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                worker.RunWorkerAsync(workerObject);                             
            }
        }

        internal static BackgroundWorker worker = null;
        static void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }

        internal static void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            
            if (e.Argument is WorkerObject)
            {
                WorkerObject workerObject = e.Argument as WorkerObject;
                new SharePointBrigdeHelper(workerObject.DTE).PerformDeploymentOperation(workerObject.Operation, workerObject.WSPFiles);
            }
        }

        internal static void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
           // Helpers.LogMessage(dte, dte, "BackgroundWorker completed");

        }

        /// <summary>
        /// returns the deployment url for the project
        /// </summary>
        /// <param name="dte"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        internal static string GetSiteUrl(DTE dte, Project project)
        {         
            try
            {
                ISharePointProjectService projectService = Helpers2.GetSharePointProjectService(dte);

                if (project != null)
                {
                    try
                    {
                        //set url for current project
                        ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(project);
                        return sharePointProject.SiteUrl.ToString();
                    }
                    catch { }
                }
            }
            catch { }

            return Helpers.GetApplicationConfigValue(dte, "DebuggingWebApp", "").ToString();
        }

        internal static string GetStartupUrl(DTE dte, Project project)
        {
            try
            {
                ISharePointProjectService projectService = Helpers2.GetSharePointProjectService(dte);

                if (project != null)
                {
                    try
                    {
                        //set url for current project
                        ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(project);
                        if (sharePointProject.StartupUrl != null)
                        {
                            return sharePointProject.StartupUrl.ToString();
                        }
                        return sharePointProject.SiteUrl.ToString();
                    }
                    catch { }
                }
            }
            catch { }

            return Helpers.GetApplicationConfigValue(dte, "StartupUrl", Helpers.GetApplicationConfigValue(dte, "DebuggingWebApp", "").ToString()).ToString();
        }


        internal static void AddFiles(ProjectItem folder12, List<string> files)
        {
            foreach (ProjectItem item in folder12.ProjectItems)
            {
                try
                {
                    files.Add(Helpers.GetFullPathOfProjectItem(item));
                    AddFiles(item, files);
                }
                catch (Exception)
                {
                }
            }
        }

        internal static void ExecAdmSvcJobs(DTE dte)
        {
            Helpers.LogMessage(dte, dte, "running stsadm -o execadmsvcjobs");

            string STSADMPath = Helpers.GetSharePointHive() + "\\BIN\\stsadm.exe";

            if (File.Exists(STSADMPath))
            {
                //run wspbuilder.exe
                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = STSADMPath;
                    psi.Arguments = "-o execadmsvcjobs";
                    psi.CreateNoWindow = true;
                    psi.UseShellExecute = false;
                    psi.RedirectStandardInput = true;
                    psi.RedirectStandardOutput = true;
                    psi.RedirectStandardError = true;

                    // Create the process.
                    System.Diagnostics.Process p = new System.Diagnostics.Process();

                    // Associate process info with the process.
                    p.StartInfo = psi;

                    // Run the process.
                    bool fStarted = p.Start();

                    if (!fStarted)
                        throw new Exception("Unable to start process.");

                    while (!p.HasExited)
                    {
                        string text = p.StandardOutput.ReadLine();
                        if (!String.IsNullOrEmpty(text))
                        {
                            Helpers.LogMessage(dte, dte, text);
                        }
                        System.Threading.Thread.Sleep(100);
                    }

                    Helpers.LogMessage(dte, dte, p.StandardOutput.ReadToEnd());
                }
                catch (Exception ex)
                {
                    Helpers.LogMessage(dte, dte, ex.ToString());
                    System.Windows.Forms.MessageBox.Show(ex.Message, "Error");
                }
                finally
                {
                }
            }
        }

        internal static void QuickDeployItem(DTE dte, ProjectItem selectedItem, ref int successes, ref int failures, ref int overwritten)
        {
            //is file or folder?
            if (selectedItem.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFile)
            {
                //file                
                try
                {
                    string sourcefilename = Helpers.GetFullPathOfProjectItem(selectedItem);

                    if (!Helpers2.IsFileDeployableToHive(sourcefilename))
                    {
                        //ignore CS files
                        return;
                    }

                    string pathToHive = Helpers.GetSharePointHive(); //returns 'web server extensions/14', but we need without 14
                    string relativetargetFileName = GetDeploymentPathOfItem(dte, selectedItem); //starting with template

                    if (!string.IsNullOrEmpty(relativetargetFileName))
                    {
                        string fullTargetPath = pathToHive + relativetargetFileName;
                        
                        //replace SharePoint attributes
                        Helpers.LogMessage(dte, dte, "Copying " + sourcefilename);

                        if (File.Exists(fullTargetPath))
                        {
                            overwritten++;
                        }
                        File.Copy(sourcefilename, fullTargetPath, true);

                        //replace SharePoint Arguments
                        ReplaceTokens(fullTargetPath);

                        successes++;
                    }
                }
                catch (Exception ex2)
                {
                    failures++;
                    Helpers.LogMessage(dte, dte, ex2.Message);
                }
            }
            else
            {
                foreach (ProjectItem childItem in selectedItem.ProjectItems)
                {
                    QuickDeployItem(dte, childItem, ref successes, ref failures, ref overwritten);
                }
            }
        }

        private static void ReplaceTokens(string fullTargetPath)
        {
            /* XML, ASCX, ASPX, Webpart, DWP, SVC */
            if (fullTargetPath.EndsWith(""))
            {

                /*

                $SharePoint.Project.FileName$
                The name of the containing project file, such as, "NewProj.csproj".
                 
                $SharePoint.Project.FileNameWithoutExtension$
                The name of the containing project file without the file name extension. For example, "NewProj".
 
                $SharePoint.Project.AssemblyFullName$
                The display name (strong name) of the containing project’s output assembly.
 
                $SharePoint.Project.AssemblyFileName$
                The name of the containing project’s output assembly.
 
                $SharePoint.Project.AssemblyFileNameWithoutExtension$
                The name of the containing project’s output assembly, without the file name extension.
 
                $SharePoint.Project.AssemblyPublicKeyToken$
                The public key token of the containing project’s output assembly, converted to a string. (16-characters in "x2" hexadecimal format.)
 
                $SharePoint.Package.Name$
                The name of the containing package.
 
                $SharePoint.Package.FileName$
                The name of the containing package's definition file.
 
                $SharePoint.Package.FileNameWithoutExtension$
                The name (without extension) of the containing package's definition file.
                 
                $SharePoint.Package.Id$
                The SharePoint ID for the containing package. If a feature is used in more than one package, then this value will change.
 
                $SharePoint.Feature.FileName$
                The name of the definition file of the containing feature, such as Feature1.feature.
 
                $SharePoint.Feature.FileNameWithoutExtension$
                The name of the feature definition file, without the file name extension.
 
                $SharePoint.Feature.DeploymentPath$
                The name of the folder that contains the feature in the package. This token equates to the "Deployment Path" property in the Feature Designer. An example value is, "Project1_Feature1".
 
                $SharePoint.Feature.Id$
                The SharePoint ID of the containing feature. This token, as with all feature-level tokens, can be used only by files included in a package via a feature, not added directly to a package outside of a feature.
 
                $SharePoint.ProjectItem.Name$
                The name of the project item (not its file name), as obtained from ISharePointProjectItem.Name.
 
                $SharePoint.Type.<GUID>.AssemblyQualifiedName$
                The assembly qualified name of the type matching the GUID of the token. The format of the GUID is lowercase and corresponds to the Guid.ToString("D") format (that is, xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).
 
                $SharePoint.Type.<GUID>.FullName$
                The full name of the type matching the GUID in the token. The format of the GUID is lowercase and corresponds to the Guid.ToString("D") format (that is, xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).
 
                */
            }
        }

        /// <summary>
        /// returns the relative path of the item in the final sharepoint folder
        /// </summary>
        /// <param name="dte"></param>
        /// <param name="selectedItem"></param>
        /// <returns></returns>
        public static string GetDeploymentPathOfItem(DTE dte, ProjectItem selectedItem)
        {
            //1. trying to use VS 2010
            try
            {
                ISharePointProjectService projectService = Helpers2.GetSharePointProjectService(dte);

               
                try
                {
                    ISharePointProjectItemFile selectedSharePointItem = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItemFile>(selectedItem);
                    string path = selectedSharePointItem.DeploymentRoot + selectedSharePointItem.DeploymentPath + selectedSharePointItem.Name; //= {SharePointRoot}\\Template\\
                    if (path.StartsWith("{SharePointRoot}"))
                    {
                        path = path.Replace("{SharePointRoot}", "");
                    }
                    if (path.Contains("{FeatureName}"))
                    {
                        //ok, element is part of a feature, need to find the feature where the element is located
                        string parentFeatureName = "";
                        ISharePointProjectItem sharePointItem = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItem>(selectedItem);
                        if (sharePointItem == null)
                        {
                            if (selectedItem.Collection.Parent is ProjectItem)
                            {
                                sharePointItem = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItem>(selectedItem.Collection.Parent as ProjectItem);
                            }
                        }                       
                        foreach (ISharePointProject project in projectService.Projects)
                        {
                            foreach (ISharePointProjectFeature feature in project.Features)
                            {
                                if (feature.ProjectItems.Contains(sharePointItem))
                                {
                                    parentFeatureName = feature.Name;
                                }
                            }
                        }
                        path = path.Replace("{FeatureName}", parentFeatureName);
                    }
                    return path;
                }
                catch { }
                
            }
            catch { }

            //ok, we have HIVE format!
            //structure could be 12, 14, oder SharePointRoot
            string projectfolder = Path.GetDirectoryName(Helpers.GetFullPathOfProjectItem(selectedItem.ContainingProject));
            string itemFullPath = Helpers.GetFullPathOfProjectItem(selectedItem);
            string itemRelativePath = itemFullPath.Substring(projectfolder.Length+1);

            if (itemRelativePath.StartsWith("12"))
            {
                return itemRelativePath.Substring(2);
            }
            else if (itemRelativePath.StartsWith("14"))
            {
                return itemRelativePath.Substring(2);
            }
            else if (itemRelativePath.StartsWith("15"))
            {
                return itemRelativePath.Substring(2);
            }
            else if (itemRelativePath.StartsWith("SharePointRoot", StringComparison.InvariantCultureIgnoreCase))
            {
                return itemRelativePath.Substring(14);
            }

            return "";
        }

        internal static void QuickDeploy(DTE dte, Project _CurrentProject)
        {
            int success = 0;
            int failures = 0;
            int overwritten = 0;
            Helpers.LogMessage(dte, dte, "*** Starting Quick Deploy ***");
            foreach (ProjectItem childItem in _CurrentProject.ProjectItems)
            {
                QuickDeployItem(dte, childItem, ref success, ref failures, ref overwritten);
            }
            Helpers.LogMessage(dte, dte, "*** Quick Deploy finished: " + success.ToString() + " successfully (" + overwritten.ToString() + " overwrites), " + failures.ToString() + " failed ***" + Environment.NewLine);
        }

        internal static BackgroundWorker quickDeployWorker = null;

        internal static bool QuickDeployGAC(DTE dte)
        {
            Helpers.LogMessage(dte, dte, "*** Starting Quick Deploy of assemblies ***");
            
            //running separat thread for gacdeployment
            quickDeployWorker = new BackgroundWorker();
            quickDeployWorker.ProgressChanged += new ProgressChangedEventHandler(quickDeployWorker_ProgressChanged);
            quickDeployWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(quickDeployWorker_RunWorkerCompleted);
            quickDeployWorker.DoWork += new DoWorkEventHandler(quickDeployWorker_DoWork);
            quickDeployWorker.RunWorkerAsync(dte);

            return true;
        }

        static void quickDeployWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int successfullCopies = 0;
            int failedCopies = 0;

            if (e.Argument is DTE)
            {
                DTE dte = e.Argument as DTE;

                Helpers.ShowProgress(dte, "Running Quick Deployment...", 10);

                List<string> urls = new List<string>();
                List<Project> projects = Helpers.GetSelectedDeploymentProjects(dte);
                foreach (Project project in projects)
                {
                    string siteurl = GetSiteUrl(dte, project);
                    if (!string.IsNullOrEmpty(siteurl))
                    {
                        if (!urls.Contains(siteurl))
                        {
                            urls.Add(siteurl);
                        }
                    }
                    if (QuickDeployGAC(dte, project))
                    {
                        successfullCopies++;
                    }
                    else
                    {
                        failedCopies++;
                    }
                }

                Helpers.ShowProgress(dte, "Recycling Application Pools...", 40);
                
                //run recycle app pools for each site
                Helpers.LogMessage(dte, dte, "*** Recycling app pools for all servers ***");
                List<string> appPools = new List<string>();
                if (urls.Count > 0)
                {
                    //siteurls are given
                    foreach (string url in urls)
                    {
                        //getting apppool for a siteurl
                        string appPoolName = Helpers.GetAppPoolOfWebApp(dte, url);
                        if (!string.IsNullOrEmpty(appPoolName))
                        {
                            if (!appPools.Contains(appPoolName))
                            {
                                appPools.Add(appPoolName);
                            }
                        }
                    }
                    foreach (string appPool in appPools)
                    {
                        Helpers.RecycleAppPool(dte, appPool);
                    }
                }
                else
                {
                    //no site urls found, we recycle all app pools
                    Helpers.RecycleAllAppPools(dte);
                }

                Helpers.ShowProgress(dte, "Send ping to servers...", 80);

                //pinging all servers
                Helpers.LogMessage(dte, dte, "*** Send ping to all servers ***");
                foreach(string url in urls)
                {
                    PingServer(dte, url);
                }

                Helpers.LogMessage(dte, dte, "*** Quick Deploy finished, " + successfullCopies.ToString() + " successful, " + failedCopies.ToString() + " failed ***" + Environment.NewLine);
                Helpers.ShowProgress(dte, "Finished.", 100);
                Helpers.HideProgress(dte);
            }
        }

        internal static void PingServer(DTE dte, string url)
        {
            PingServer(dte, url, 2000);        
        }

        internal static void PingServer(DTE dte, string url, int timeout)
        {
            Helpers.LogMessage(dte, dte, "Send ping to " + url + "");
            try
            {
                //open the webpage
                WebRequest webRequest = WebRequest.Create(url);
                webRequest.Timeout = timeout;
                webRequest.Credentials = CredentialCache.DefaultCredentials;
                WebResponse response = webRequest.GetResponse();
                response.Close();
            }
            catch
            {
                //Helpers.LogMessage(dte, dte, "Exception from " + url + ": " + ex.Message);
            }
        }

        

        internal static bool QuickDeployGAC(DTE dte, Project project)
        {
            try
            {
                string projectpath = Helpers.GetFullPathOfProjectItem(project);
                string projectfolder = Directory.GetParent(projectpath).FullName;

                Helpers.LogMessage(dte, dte, "Running GAC deployment for project '" + project.Name + "'");

                //GACDeployment
                //if project contains assembly then gacutil or bin folder
                string GACFile = GetOutputDLL(dte, project);

                if (!File.Exists(GACFile))
                {
                    throw new Exception("Assembly in path " + GACFile + " not found.");
                }

                string gacutilpath = Helpers.GetGACUtil(dte);
                if (string.IsNullOrEmpty(gacutilpath))
                {
                    throw new Exception("Path to gacutil.exe not found: " + gacutilpath);
                }

                Helpers.RunProcess(dte, gacutilpath, "/if  \"" + GACFile + "\"", true, false);

                return true;
            }
            catch (Exception ex)
            {
                Helpers.LogMessage(dte, dte, "Error: " + ex.ToString());
                return false;
            }
        }

        static void quickDeployWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
        }

        static void quickDeployWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }

        internal static void DeleteFailedDeploymentJobs(DTE dte)
        {
            Helpers.ShowProgress(dte, "Deleting failed deployment jobs...", 30);
            Helpers.LogMessage(dte, dte, "Deleting failed deployment jobs");

            new SharePointBrigdeHelper(dte).DeleteFailedDeploymentJobs();

            Helpers.HideProgress(dte);
        }

        internal static void CheckBrokenFields(DTE dte, string siteCollectionUrl)
        {
            Helpers.LogMessage(dte, dte, "Checking for broken fields");
            Helpers.LogMessage(dte, dte, "Please wait...");

            new SharePointBrigdeHelper(dte).CheckBrokenFields(siteCollectionUrl);

            Helpers.HideProgress(dte);
        }

        public static string GetDefaultIISWebApp()
        {
            using (DirectoryEntry iisApps = new DirectoryEntry(string.Format("IIS://{0}/W3SVC", "localhost")))
            {
                foreach (DirectoryEntry iisApp in iisApps.Children)
                {
                    string n = iisApp.Name;
                    string name = "";
                    string ports = "";

                    PropertyValueCollection pvc1 = iisApp.Properties["ServerComment"];
                    foreach (object value in pvc1)
                    {
                        name = value.ToString();
                    }

                    PropertyValueCollection pvc = iisApp.Properties["ServerBindings"];
                    foreach (object value in pvc)
                    {
                        // Format is IPAddress:Port:HostHeader
                        string[] Bits = value.ToString().Split(':');
                        string IPAddress = Bits[0];
                        string TCPIPPort = Bits[1];
                        string HostHeader = Bits[2];


                        ports = String.Format("IP = {0}, Port = {1}, Header = {2}",
                        (IPAddress.Length == 0) ? "(All Unassigned)" : IPAddress,
                        TCPIPPort,
                        (HostHeader.Length == 0) ? "(No Host Header)" : HostHeader);
                    }

                    string a = name + "(" + ports + ")";
                }
            }

            /*
            DirectoryEntry entry = new DirectoryEntry("IIS://localhost/w3svc/1");
            PropertyValueCollection pvc = entry.Properties["ServerBindings"];
            foreach (object value in pvc)
            {
              // Format is IPAddress:Port:HostHeader
              string[] Bits = value.ToString().Split(':');
              string IPAddress = Bits[0];
              string TCPIPPort = Bits[1];
              string HostHeader = Bits[2];


              string s = String.Format("IP = {0}, Port = {1}, Header = {2}",
              (IPAddress.Length == 0) ? "(All Unassigned)" : IPAddress,
              TCPIPPort,
              (HostHeader.Length == 0) ? "(No Host Header)" : HostHeader);
            }
             * */


            return "localhost";
        }
    }
}