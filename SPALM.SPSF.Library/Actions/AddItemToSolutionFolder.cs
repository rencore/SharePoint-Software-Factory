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
using EnvDTE80;
using System.ComponentModel.Design;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// Add a new item to a given folder
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class AddItemToSolutionFolder : ConfigurableAction
    {
        private string _SourceFileName = "";
        public string SourceFileName
        {
            get { return _SourceFileName; }
            set { _SourceFileName = value; }
        }

        private string destFolder = String.Empty;
        [Input(Required = true)]
        public string TargetFolder
        {
            get { return destFolder; }
            set { destFolder = value; }
        }

        private string _TargetFilename = String.Empty;
        [Input(Required = true)]
        public string TargetFilename
        {
            get { return _TargetFilename; }
            set { _TargetFilename = value; }
        }

        [Input(Required = false)]
        public string Content
        {
            get { return _Content; }
            set { _Content = value; }
        }
        private string _Content = "";

        [Input(Required = false)]
        public bool Overwrite
        {
            get { return _Overwrite; }
            set { _Overwrite = value; }
        }
        private bool _Overwrite = false;

        protected string GetBasePath()
        {
            return base.GetService<IConfigurationService>(true).BasePath;
        }

        private string GetTemplateBasePath()
        {
            return new DirectoryInfo(this.GetBasePath() + @"\Templates").FullName;
        }

        internal string EvaluateParameterAsString(DTE dte, string parameterName)
        {
          try
          {
            IDictionaryService serviceToAdapt = (IDictionaryService)this.GetService(typeof(IDictionaryService));
            ServiceAdapterDictionary serviceAdaptor = new ServiceAdapterDictionary(serviceToAdapt);
            ExpressionEvaluationService2 expressionService2 = new ExpressionEvaluationService2();
            object evaluatedValue = expressionService2.Evaluate(parameterName, serviceAdaptor);
            if (evaluatedValue == null || string.IsNullOrEmpty(evaluatedValue.ToString()))
            {
              return "";
            }
            return evaluatedValue.ToString();
          }
          catch (NullReferenceException)
          {
          }
          return "";
        }

        public override void Execute()
        {
            EnvDTE.DTE dte = this.GetService<EnvDTE.DTE>(true);

            string evaluatedSourceFileName = EvaluateParameterAsString(dte, SourceFileName);

            string template = "";

            if (Content != "")
            {
                template = Path.GetTempFileName();
                using (StreamWriter writer = new StreamWriter(template, false))
                {
                    writer.WriteLine(Content);
                }
            }
            else
            {
              template = evaluatedSourceFileName;
            }

            if (template != "")
            {
                string templateBasePath = GetTemplateBasePath();
                if (!Path.IsPathRooted(template))
                {
                    template = Path.Combine(templateBasePath, template);
                }

                template = new FileInfo(template).FullName;

                string solutionPath = (string)dte.Solution.Properties.Item("Path").Value;
                string solutionDir = Path.GetDirectoryName(solutionPath);
                string keyFilePath = Path.Combine(solutionDir, this.TargetFolder);

                Solution2 soln = (Solution2)dte.Solution;
                Project projecttargetfolder = Helpers.GetSolutionFolder(soln, TargetFolder);
                if (projecttargetfolder == null)
                {
                    projecttargetfolder = soln.AddSolutionFolder(TargetFolder);
                }

                if (projecttargetfolder != null)
                {
                    string filename = Path.Combine(solutionDir, _TargetFilename);

                    if (File.Exists(filename))
                    {

                        if (Overwrite || dte.SuppressUI)
                        {
                            Helpers.EnsureCheckout(dte, filename);
                            Helpers.LogMessage(dte, dte, filename + ": File overwritten");
                            File.Copy(template, filename, true);
                        }
                        else
                        {
                            if(MessageBox.Show("File '" + _TargetFilename + "' already exists. Overwrite?", "Overwrite existing file?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                Helpers.EnsureCheckout(dte, filename);
                                Helpers.LogMessage(dte, dte, filename + ": File overwritten");
                                File.Copy(template, filename, true);
                            }
                        }
                    }
                    else
                    {
                        Helpers.LogMessage(dte, dte, filename + ": File added");
                        File.Copy(template, filename);
                    }

                    try
                    {
                        //is item already in project
                        ProjectItem existingItem = Helpers.GetProjectItemByName(projecttargetfolder.ProjectItems, _TargetFilename);
                        if(existingItem == null)
                        {
                            Helpers.AddFromFile(projecttargetfolder, filename);
                            if (dte.ActiveWindow.Document != null)
                            {
                                if (dte.ActiveWindow.Document.FullName == filename)
                                {
                                    //only close window if the document has been openend otherwise there are problems in recipe migrate application
                                    dte.ActiveWindow.Close(EnvDTE.vsSaveChanges.vsSaveChangesNo);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Helpers.LogMessage(dte, this, ex.ToString());
                    }
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