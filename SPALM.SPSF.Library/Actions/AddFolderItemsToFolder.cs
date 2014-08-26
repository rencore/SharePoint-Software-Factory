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
    /// Adds all items in a specific template folder to a target folder
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class AddFolderItemsToFolder : ConfigurableAction
    {
        private string _SourceFolder = "";

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

        [Input(Required = true)]
        public string SourceFolder
        {
            get { return _SourceFolder; }
            set { _SourceFolder = value; }
        }

        private string destFolder = String.Empty;
        [Input(Required = true)]
        public string TargetFolder
        {
            get { return destFolder; }
            set { destFolder = value; }
        }

        [Input(Required = true)]
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
            string templatefolder = this.SourceFolder;
            string templateBasePath = GetTemplateBasePath();
            if (!Path.IsPathRooted(templatefolder))
            {
                templatefolder = Path.Combine(templateBasePath, templatefolder);
            }

            ProjectItems whereToAdd = WhereToAdd();

            if (Directory.Exists(templatefolder))
            {
                string[] files = Directory.GetFiles(templatefolder);

                foreach (string s in files)
                {
                    string filename = new FileInfo(s).Name;
                    ProjectItem _ProjectItem = whereToAdd.AddFromTemplate(s, filename);
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