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
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.SharePoint;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class ShowProperties : ConfigurableAction
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

        public object GetService(object serviceProvider, System.Type type)
        {
            return GetService(serviceProvider, type.GUID);
        }

        public object GetService(object serviceProviderObject, System.Guid guid)
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

        public override void Execute()
        {
            DTE service = (DTE)this.GetService(typeof(DTE)); 

            try
            {
                if (projectItem != null)
                {
                    Helpers.LogMessage(service, this, "*********** SharePointService Properties ********************");
                    try
                    {
                        ISharePointProjectService projectService = Helpers2.GetSharePointProjectService(service);
                        ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(project);

                        try
                        {   //is it a feature
                            ISharePointProjectFeature sharePointFeature = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectFeature>(projectItem);

                        }
                        catch { }

                        try
                        {   //is it a feature
                            ISharePointProjectItem sharePointItem = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItem>(projectItem);
                            
                        }
                        catch { }

                        try
                        {   //is it a feature
                            ISharePointProjectItemFile sharePointItemFile = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItemFile>(projectItem);
                            
                        }
                        catch { }

                        try
                        {   //is it a mapped folder
                            IMappedFolder sharePointMappedFolder = projectService.Convert<EnvDTE.ProjectItem, IMappedFolder>(projectItem);

                        }
                        catch { }

                        try
                        {   //is it a mapped folder
                            ISharePointProjectPackage sharePointProjectPackage = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectPackage>(projectItem);

                        }
                        catch { }
                    }
                    catch{}

                    //codelements
                    Helpers.LogMessage(service, this, "*********** EnvDTE CodeElements ********************");
                    
                    foreach (CodeElement codeElement in projectItem.FileCodeModel.CodeElements)
                    {
                        try
                        {
                            Helpers.LogMessage(service, this, "codeElement.FullName: " + codeElement.FullName);
                            Helpers.LogMessage(service, this, "codeElement.Type: " + codeElement.GetType().ToString());
                            Helpers.LogMessage(service, this, "codeElement.Kind: " + codeElement.Kind.ToString());
                        }
                        catch {}
                    }

                    Helpers.LogMessage(service, this, "*********** EnvDTE Properties ********************");
                    for (int i = 0; i < projectItem.Properties.Count; i++)
                    {
                        try
                        {
                            string name = projectItem.Properties.Item(i).Name;
                            string value = "";
                            try
                            {
                                value = projectItem.Properties.Item(i).Value.ToString();
                            }
                            catch (Exception)
                            {
                            }
                            Helpers.LogMessage(service, this, name + "=" + value);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                else if (project != null)
                {
                    for (int i = 0; i < project.Properties.Count; i++)
                    {
                        try
                        {
                            string name = project.Properties.Item(i).Name;
                            string value = "";
                            try
                            {
                                value = project.Properties.Item(i).Value.ToString();
                            }
                            catch (Exception)
                            {
                            }
                            Helpers.LogMessage(service, this, name + "=" + value);

                            if (project.Properties.Item(i).Collection.Count > 0)
                            {
                            }
                        }
                        catch (Exception)
                        {
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