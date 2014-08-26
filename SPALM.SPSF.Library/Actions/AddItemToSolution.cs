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
using Microsoft.VisualStudio.Shell.Interop;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// Adds an item to the solution
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class AddItemToSolution : ConfigurableAction
    {
        private string _sourceFile = String.Empty;

        /// <summary>
        /// Specifies the filename name of the template 
        /// we are going to reference
        /// </summary>
        [Input(Required = false)]
        public string SourceFileName
        {
            get { return _sourceFile; }
            set { _sourceFile = value; }
        }

        [Input(Required = true)]
        public string TargetFileName
        {
            get { return _TargetFilename; }
            set { _TargetFilename = value; }
        }
        private string _TargetFilename = "";

        [Input(Required = false)]
        public string Content
        {
            get { return _Content; }
            set { _Content = value; }
        }
        private string _Content = "";

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
            EnvDTE.DTE dte = this.GetService<EnvDTE.DTE>(true);

  
            try
            {
                IVsSolutionPersistence pers = this.GetService<IVsSolutionPersistence>(true);

                //pers.LoadPackageUserOpts(
            }
            catch (Exception)
            {
            }

            string template = this.SourceFileName;

            if (Content != "")
            {
                template = Path.GetTempFileName();
                using (StreamWriter writer = new StreamWriter(template, false))
                {
                    writer.WriteLine(Content);
                }
            }

            string templateBasePath = GetTemplateBasePath();
            if (!Path.IsPathRooted(template))
            {
                template = Path.Combine(templateBasePath, template);
            }

            template = new FileInfo(template).FullName;

            string solutionPath = (string)dte.Solution.Properties.Item("Path").Value;
            string solutionDir = Path.GetDirectoryName(solutionPath);
            string keyFilePath = Path.Combine(solutionDir, this.TargetFileName);

            File.Copy(template, keyFilePath);

            try
            {
                Helpers.LogMessage(dte, this, keyFilePath + ": File added");

                DteHelper.SelectSolution(dte);
                dte.ItemOperations.AddExistingItem(keyFilePath);
                dte.ActiveWindow.Close(EnvDTE.vsSaveChanges.vsSaveChangesNo);
            }
            catch (Exception ex)
            {
                Helpers.LogMessage(dte, this, ex.ToString());
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