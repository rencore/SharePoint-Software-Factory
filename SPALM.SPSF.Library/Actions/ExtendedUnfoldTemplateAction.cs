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

#endregion

namespace SPALM.SPSF.Library.Actions
{
    [ServiceDependency(typeof(DTE)), ServiceDependency(typeof(ITypeResolutionService))]
    public class ExtendedUnfoldTemplateAction : ExtendedUnfoldTemplateActionBase
    {
        /*
         * 
         * FileIOPermission(SecurityAction.Demand, Write = @"C:\Program Files\")] 
         * FileIOPermission FIOP = new FileIOPermission(PermissionState.Unrestricted);
            FIOP.AllFiles = FileIOPermissionAccess.AllAccess;
         * 
         */

        private IConfigurationService configurationService;
        private List<TemplateItem> templateItemLists;

        [Input(Required = true)]
        public string ProjectName
        {
            get { return _ProjectName; }
            set
            {
                _ProjectName = value;
                ItemName = value;
            }
        }
        private string _ProjectName = "";

        [Input(Required = false)]
        public bool AdditionalCondition
        {
            get { return _AdditionalCondition; }
            set { _AdditionalCondition = value; }
        }
        private bool _AdditionalCondition = true;


        [Input(Required = false)]
        public string SolutionFolder
        {
            get { return _SolutionFolder; }
            set { _SolutionFolder = value; }
        }
        private string _SolutionFolder = "";

        [Input(Required = false)]
        public bool UseSolutionFolder
        {
            get { return _UseSolutionFolder; }
            set { _UseSolutionFolder = value; }
        }

        private bool _UseSolutionFolder = false;


        public override void Execute()
        {
            if (!_AdditionalCondition)
            {
                return;
            }
            EnvDTE.DTE vs = this.GetService<EnvDTE.DTE>(true);

            if ((_SolutionFolder != "") && (_UseSolutionFolder))
            {
                Solution2 soln = (Solution2)vs.Solution;
                Project p = Helpers.GetSolutionFolder(soln, _SolutionFolder);
                if (p == null)
                {
                    p = soln.AddSolutionFolder(_SolutionFolder);
                }
                this.Root = p;
            }
            else
            {
                this.Root = vs.Solution;
            }

            string solutionPath = (string)vs.Solution.Properties.Item("Path").Value;
            string solutionDir = System.IO.Path.GetDirectoryName(solutionPath);

            //solutionpath + ProjectName
            //string solutionfolder = vs.Solution.FullName;
            //solutionfolder = solutionfolder.Substring(0,solutionfolder.LastIndexOf("\\"));
            DestinationFolder = solutionDir + "\\" + ProjectName;

            //ProjectName
            ItemName = ProjectName;

            templateItemLists = new List<TemplateItem>();
            configurationService = GetService<IConfigurationService>(true);

            if (!System.IO.Path.IsPathRooted(this.Template) && configurationService != null)
            {
                this.Template = new FileInfo(System.IO.Path.Combine(configurationService.BasePath + @"\Templates\", this.Template)).FullName;
            }

            //Helpers.LogMessage(vs, this, "Unfolding template " + this.Template);

            string templateDirectory = Directory.GetParent(this.Template).FullName;

            FileIOPermission FIOP = new FileIOPermission(FileIOPermissionAccess.Read, templateDirectory);
            FIOP.Demand();

            IDictionaryService dictionaryService = GetService<IDictionaryService>();


            // * !!! 
            //remove wizard not possible because dictionary with recipe arguments is
            //already loaded and we have conflicts :-(
            // vsnet loads the vstemplate file from the internal cached zip file
            // later update impossible
            //remove recipe from the vstemplate file
            /*
            string vstemplateOriginal = this.Template;
            string vstemplatBackupFileName = System.IO.Path.GetTempFileName();
            File.Copy(vstemplateOriginal, vstemplatBackupFileName, true);
            RemoveWizards(vstemplateOriginal);
            //this.Template = vstemplateOriginal; //set the template again
            */
            ReadTemplate();

            foreach (TemplateItem templateItem in templateItemLists)
            {
                if (templateItem.OriginalFileName.ToLower().Contains(".dll") || templateItem.OriginalFileName.ToLower().Contains(".exe"))
                {
                    //donothing
                }
                else
                {
                    templateItem.ParseItem(dictionaryService);
                }
            }

            try
            {
                base.Execute();
            }
            finally
            {
                //copy vstemplate back 
                //File.Copy(vstemplatBackupFileName, vstemplateOriginal, true);

                foreach (TemplateItem templateItem in templateItemLists)
                {
                    templateItem.Restore();
                }
            }
        }

        /// <summary>
        /// Removes the wizard from a .vstemplate file
        /// </summary>
        /// <param name="templateContent"></param>
        /// <returns></returns>
        internal void RemoveWizards(string vstemplateFilepath)
        {
            XmlDocument vsTemplateDocument = new XmlDocument();
            vsTemplateDocument.Load(vstemplateFilepath);

            //search for wizard WizardData
            XmlNamespaceManager manager = new XmlNamespaceManager(vsTemplateDocument.NameTable);
            manager.AddNamespace("ns", "http://schemas.microsoft.com/developer/vstemplate/2005");

            XmlNode templateContentNode = vsTemplateDocument.SelectSingleNode("//ns:VSTemplate/ns:WizardData", manager);

            if (templateContentNode != null)
            {
                templateContentNode.ParentNode.RemoveChild(templateContentNode);
            }

            vsTemplateDocument.Save(vstemplateFilepath);
        }

        public override void Undo()
        {
            // nothing to do.
        }

        #region Private Helpers
        private void ReadTemplate()
        {
            XmlDocument vsTemplateDocument = new XmlDocument();

            XmlNamespaceManager manager = new XmlNamespaceManager(vsTemplateDocument.NameTable);
            manager.AddNamespace("ns", "http://schemas.microsoft.com/developer/vstemplate/2005");

            vsTemplateDocument.Load(this.Template);

            XmlNode templateContentNode = vsTemplateDocument.SelectSingleNode("//ns:VSTemplate/ns:TemplateContent", manager);

            if (templateContentNode != null)
            {
                foreach (XmlNode projectNode in templateContentNode.SelectNodes("ns:Project", manager))
                {
                    if (projectNode.Attributes["File"] != null)
                    {
                        string projectFilename = GetFullPath(projectNode.Attributes["File"].Value);

                        if (File.Exists(projectFilename))
                        {
                            templateItemLists.Add(new TemplateItem(projectFilename));
                        }
                    }

                    FindProjecItems(projectNode, manager, System.IO.Path.GetDirectoryName(this.Template));
                }

                FindProjecItems(templateContentNode, manager, System.IO.Path.GetDirectoryName(this.Template));
            }
        }

        private string GetProjectFolderName(XmlNode foldernode)
        {
            if (foldernode.Attributes["TargetFolderName"] != null)
            {
                return foldernode.Attributes["TargetFolderName"].Value;
            }
            if (foldernode.Attributes["FolderName"] != null)
            {
                return foldernode.Attributes["TargetFolderName"].Value;
            }
            return "";
        }

        private void FindProjecItems(XmlNode parentNode, XmlNamespaceManager manager, string path)
        {
            //Ordner
            foreach (XmlNode projectFolderNode in parentNode.SelectNodes("ns:Folder", manager))
            {
                FindProjecItems(projectFolderNode, manager, path + "/" + GetProjectFolderName(projectFolderNode));
            }

            foreach (XmlNode projectItemNode in parentNode.SelectNodes("ns:ProjectItem", manager))
            {
                string projectItemFilename = path + "/" + projectItemNode.InnerText;

                if (File.Exists(projectItemFilename))
                {
                    templateItemLists.Add(new TemplateItem(projectItemFilename));
                }
            }
        }

        private string GetFullPath(string file)
        {
            if (!System.IO.Path.IsPathRooted(file))
            {
                return new FileInfo(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.Template), file)).FullName;
            }

            return file;
        }

        private class TemplateItem
        {
            private string originalFilename;

            public string OriginalFileName
            {
                get { return originalFilename; }
                set { originalFilename = value; }
            }

            private string backupFileName;

            public TemplateItem(string fileName)
            {
                this.originalFilename = fileName;
            }

            public void Backup()
            {
                backupFileName = System.IO.Path.GetTempFileName();
                File.Copy(originalFilename, backupFileName, true);
            }

            public void Restore()
            {
                if (!string.IsNullOrEmpty(backupFileName) && File.Exists(backupFileName))
                {
                    File.Copy(backupFileName, originalFilename, true);
                }
            }

            public void ParseItem(IDictionaryService dictionaryService)
            {
                string templateContent = File.ReadAllText(this.originalFilename);

                Regex expression = new Regex("\\$(?<argumentName>\\S+?)\\$", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.ExplicitCapture);

                if (dictionaryService != null)
                {
                    foreach (Match match in expression.Matches(templateContent))
                    {
                        Group matchGroup = match.Groups["argumentName"];

                        if (matchGroup.Success)
                        {
                            object value = dictionaryService.GetValue(matchGroup.Value);

                            if (value != null && value is string)
                            {
                                templateContent = templateContent.Replace(match.Value, (string)value);
                            }
                            else if (value != null)
                            {
                                templateContent = templateContent.Replace(match.Value, value.ToString());
                            }
                        }
                    }
                }

                Backup();


                File.WriteAllText(originalFilename, templateContent);


            }
        }
        #endregion
    }
}
