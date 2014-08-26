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

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// Add a new item to a given folder
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class AddItemToFolder : ConfigurableAction
    {
        private string _SourceFileName = "";

        private ProjectItems WhereToAdd()
        {
            ProjectItems whereToAdd;
            if ((destFolder != String.Empty) && (destFolder != null))
            {
                //only works for a single foldername
                ProjectItem _folder = DteHelper.FindItemByName(this.Project.ProjectItems, destFolder, true);
                if (_folder != null)
                {
                    whereToAdd = _folder.ProjectItems;
                }
                else
                {
                    ProjectItems pitems = this.Project.ProjectItems;

                    string projectpath = Helpers.GetFullPathOfProjectItem(Project);

                    //folder doesnt exist
                    //create the folder
                    char[] sep = { '\\' };
                    string[] folders = destFolder.Split(sep);
                    for (int i = 0; i < folders.Length; i++)
                    {
                        projectpath += folders[i] + "\\";
                        //does the folder already exist?
                        _folder = DteHelper.FindItemByName(pitems, folders[i], true);
                        if (_folder != null)
                        {
                            //folder exists
                            pitems = _folder.ProjectItems;
                        }
                        else
                        {   //create folder
                            try
                            {
                                _folder = pitems.AddFolder(folders[i], EnvDTE.Constants.vsProjectItemKindPhysicalFolder);
                                pitems = _folder.ProjectItems;
                            }
                            catch (Exception)
                            {
                                _folder = pitems.AddFromDirectory(projectpath);
                                pitems = _folder.ProjectItems;
                            }
                        }
                    }
                    whereToAdd = _folder.ProjectItems;
                }
            }
            else
            {
                whereToAdd = this.Project.ProjectItems;
            }
            return whereToAdd;
        }

        [Input(Required = false)]
        public bool AdditionalCondition
        {
            get { return _AdditionalCondition; }
            set { _AdditionalCondition = value; }
        }
        private bool _AdditionalCondition = true;

        [Input(Required = false)]
        public string SourceFileName
        {
            get { return _SourceFileName; }
            set { _SourceFileName = value; }
        }

        private string destFolder = String.Empty;
        [Input(Required = false)]
        public string TargetFolder
        {
            get { return destFolder; }
            set { destFolder = value; }
        }

        private string _TargetFileName = String.Empty;
        [Input(Required = false)]
        public string TargetFileName
        {
            get { return _TargetFileName; }
            set { _TargetFileName = value; }
        }

        private bool _Overwrite = false;
        [Input(Required = false)]
        public bool Overwrite
        {
            get { return _Overwrite; }
            set { _Overwrite = value; }
        }

        [Input(Required = false)]
        public Project Project
        {
            get
            {
                return this.project;
            }
            set
            {
                this.project = value;
            }
        }
        private Project project;

        [Input(Required = false)]
        public string ProjectName
        {
            get
            {
                return this.projectName;
            }
            set
            {
                this.projectName = value;
            }
        }
        private string projectName;


        protected string GetBasePath()
        {
            return base.GetService<IConfigurationService>(true).BasePath;
        }

        private string GetTemplateBasePath()
        {
            return new DirectoryInfo(this.GetBasePath() + @"\Templates").FullName;
        }

        public override void Execute()
        {
            if (!_AdditionalCondition)
            {
                return;
            }
            DTE dte = base.GetService<DTE>(true);

            if (project == null)
            {
                project = Helpers.GetProjectByName(dte, projectName);
            }

            if (SourceFileName == null)
            {
                return;
            }
            if (SourceFileName == "")
            {
                return;
            }

            string template = this.SourceFileName;
            string templateBasePath = GetTemplateBasePath();
            if (!Path.IsPathRooted(template))
            {
                template = Path.Combine(templateBasePath, template);
                template = new FileInfo(template).FullName;
            }

            if ((TargetFileName == null) || (TargetFileName == ""))
            {
                if (File.Exists(template))
                {
                    TargetFileName = Path.GetFileName(template);
                }
            }

            ProjectItems whereToAdd = null;
            if ((TargetFolder != null) && (TargetFolder != ""))
            {
                //get the folder TargetFolder in the project
                //Helpers.LogMessage(dte, this, "Adding file '" + TargetFolder);
                whereToAdd = WhereToAdd();
            }
            else
            {
                //no TargetFolder given, then add to project root
                //Helpers.LogMessage(dte, this, "Adding file '" + Project.Name);
                whereToAdd = this.Project.ProjectItems;
            }

            //check if item exists
            ProjectItem existingFile = null;
            try
            {
                existingFile = Helpers.GetProjectItemByName(whereToAdd, TargetFileName);
            }
            catch (Exception)
            {
            }

            if (existingFile != null && !Overwrite)
            {
                Helpers.LogMessage(dte, this, "File " + TargetFileName + " exists and will not be overwritten");
                return;
            }

            ProjectItem _ProjectItem = Helpers.AddFromTemplate(whereToAdd, template, this.TargetFileName);
        }

        /// <summary>
        /// Removes the previously added reference, if it was created
        /// </summary>
        public override void Undo()
        {
        }
    }
}