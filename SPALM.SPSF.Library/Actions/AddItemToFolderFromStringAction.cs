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

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// Add a newly created item to a folder in the project
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class AddItemToFolderFromStringAction : ConfigurableAction
    {
        private string destFolder = String.Empty;
        private bool doOverwrite = true;

        // Fields
        private string content;
        private bool open = true;
        private Project project;
        private ProjectItem projectItem;
        private string targetFileName;


        /// <summary>
        /// Specifies the filename name of the template 
        /// we are going to reference
        /// </summary>
        [Input(Required = false)]
        public string TargetFolder
        {
            get { return destFolder; }
            set { destFolder = value; }
        }

        [Input(Required = true)]
        public bool Overwrite
        {
            get { return doOverwrite; }
            set { doOverwrite = value; }
        }

        [Input(Required = false)]
        public string Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
            }
        }

        [Input]
        public bool Open
        {
            get
            {
                return this.open;
            }
            set
            {
                this.open = value;
            }
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

        [Input(Required = false)]
        public string ProjectName { get; set; }

        [Output]
        public ProjectItem ProjectItem
        {
            get
            {
                return this.projectItem;
            }
            set
            {
                this.projectItem = value;
            }
        }

        [Input(Required = true)]
        public string TargetFileName
        {
            get
            {
                return this.targetFileName;
            }
            set
            {
                this.targetFileName = value;
            }
        }


        public override void Execute()
        {
            DTE dte = base.GetService<DTE>(true);

            Project targetProject = Project;
            if (targetProject == null)
            {
                targetProject = Helpers.GetProjectByName(dte, this.ProjectName);
            }

            string testcontent = Content.Trim(new char[] { ' ', '\t', '\n', '\r' });
            if ((testcontent == "") || (testcontent == "dummy"))
            {
                return;
            }

            try
            {
                if (TargetFileName.ToUpper().EndsWith(".XML"))
                {
                    try
                    {
                        //we place our comment there
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(Content);
                        Helpers.CheckLicenseComment(doc);
                        Content = doc.OuterXml;
                    }
                    catch (Exception)
                    {
                    }
                }

                if (TargetFileName.ToUpper().EndsWith(".CS"))
                {
                    try
                    {
                        Content = Helpers.CheckLicenseCommentCS(Content);
                    }
                    catch (Exception)
                    {
                    }
                }

                ProjectItems whereToAdd = Helpers.GetProjectItemsByPath(targetProject, destFolder);

                string finalpath = "";
                if (whereToAdd.Parent is Project)
                {
                    finalpath = Path.Combine(Helpers.GetFullPathOfProjectItem(whereToAdd.Parent as Project), TargetFileName);
                }
                else
                {
                    finalpath = Path.Combine(Helpers.GetFullPathOfProjectItem(whereToAdd.Parent as ProjectItem), TargetFileName);
               }

                if (File.Exists(finalpath) && !Overwrite)
                {
                    return;
                }


                using (StreamWriter writer = new StreamWriter(finalpath, false))
                {
                    if (TargetFileName.ToUpper().EndsWith(".XML"))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(Content);

                        XmlWriter xw = XmlWriter.Create(writer, Helpers.GetXmlWriterSettings(finalpath));
                        doc.Save(xw);
                        xw.Flush();
                        xw.Close();
                    }
                    else
                    {
                        writer.WriteLine(Content);
                    }
                }


                //is the item already there
                ProjectItem existingItem = Helpers.GetProjectItemByName(whereToAdd, TargetFileName);
                if (existingItem != null)
                {
                    this.ProjectItem = existingItem;
                }
                else
                {
                    this.ProjectItem = Helpers.AddFromFile(whereToAdd, finalpath);
                }

                if (this.Open)
                {
                    Window window = this.ProjectItem.Open("{00000000-0000-0000-0000-000000000000}");
                    window.Visible = true;
                    window.Activate();
                }
            }
            catch (Exception ex)
            {
                Helpers.LogMessage(dte, this, ex.ToString());

                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Undo()
        {
            if (this.projectItem != null)
            {
                this.projectItem.Delete();
            }
        }
    }
}