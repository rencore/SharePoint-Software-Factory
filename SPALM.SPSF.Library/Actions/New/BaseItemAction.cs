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
using System.Collections.Specialized;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.SharePoint;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// Base Action which adds an item to VS
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class BaseItemAction : BaseAction
    {
        public BaseItemAction() : base()
        {            
        }

        /// <summary>
        /// template file name in the templates folder of SPSF 
        /// </summary>
        [Input(Required = false)]
        public string SourceFileName { get; set; }

        /// <summary>
        /// template file name in the templates folder of SPSF 
        /// </summary>
        [Input(Required = false)]
        public string TargetFolder { get; set; }

        /// <summary>
        /// if true the created file will be opened after creation 
        /// </summary>
        [Input(Required = false)]
        public bool Open { get; set; }

        /// <summary>
        /// if true the created file will be overwritten
        /// </summary>
        [Input(Required = false)]
        public bool Overwrite { get; set; }

        /// <summary>
        /// Adds the item to ParentProjectItem
        /// </summary>
        public override void Execute()
        {
            if (ExcludeCondition)
            {
                return;
            }
            if (!AdditionalCondition)
            {
            }

            DTE dte = (DTE)this.GetService(typeof(DTE));
            Project project = this.GetTargetProject(dte);

            if (SourceFileName.Contains(";"))
            {
                string evaluatedDeploymentPath = EvaluateParameterAsString(dte, DeploymentPath);
                char[] sep = new char[] { ';' };
                string[] allFiles = SourceFileName.Split(sep);
                foreach(string file in allFiles)
                {
                    string fullFileName = file;
                    fullFileName = fullFileName.Trim();
                    if (!string.IsNullOrEmpty(fullFileName))
                    {
                        string sourceFilePath = fullFileName;
                        AddFileInternal(dte, project, sourceFilePath, "", evaluatedDeploymentPath);
                    }
                }
            }
            else
            {

                string evaluatedTargetFileName = EvaluateParameterAsString(dte, TargetFileName);
                string evaluatedSourceFileName = EvaluateParameterAsString(dte, SourceFileName);
                string evaluatedDeploymentPath = EvaluateParameterAsString(dte, DeploymentPath);

                AddFileInternal(dte, project, evaluatedSourceFileName, evaluatedTargetFileName, evaluatedDeploymentPath);
            }
        }

        internal string GenerateContent(string templateFilename, string targetFilename)
        {
          return GenerateContent(templateFilename, targetFilename, new NameValueCollection());
        }

        internal string GenerateContent(string templateFilename, string targetFilename, NameValueCollection overrideArguments)
        {
          EnvDTE.DTE vs = this.GetService<EnvDTE.DTE>(true);

          string templateCode = string.Empty;
          if (templateFilename == null)
          {
            throw new ArgumentNullException("Template");
          }
          string templateBasePath = this.GetTemplateBasePath();
          if (!Path.IsPathRooted(templateFilename))
          {
            templateFilename = Path.Combine(templateBasePath, templateFilename);
          }
          if (!File.Exists(templateFilename))
          {
            throw new FileNotFoundException(templateFilename);
          }
          templateFilename = new FileInfo(templateFilename).FullName;
          if (!templateFilename.StartsWith(templateBasePath))
          {
            throw new ArgumentException("Starts not with " + templateBasePath);
          }
          templateCode = File.ReadAllText(templateFilename);

          //jetzt alle properties rein
          StringBuilder templateCodeLines = new StringBuilder();
          StringReader reader = new StringReader(templateCode);
          string line = "";
          bool firstlinefound = false;
          bool itemadded = false;
          while ((line = reader.ReadLine()) != null)
          {
            if (firstlinefound && !itemadded)
            {
              itemadded = true;
              AddTargetFileNameArgument(templateCodeLines, targetFilename);
              AddAllArguments(templateCodeLines, overrideArguments);
            }
            if (line.StartsWith("<#@ template language="))
            {
              firstlinefound = true;
            }
            templateCodeLines.AppendLine(line);
          }

          return this.Render(templateCodeLines.ToString(), templateFilename);
        }


        private void AddTargetFileNameArgument(StringBuilder templateCodeLines, string targetFilename)
        {
          if (!string.IsNullOrEmpty(targetFilename))
          {
            //remove existing value for generatefilename
            if (base.additionalArguments.Contains("GeneratedFileName"))
            {
              base.additionalArguments.Remove("GeneratedFileName");
            }

            base.additionalArguments.Add("GeneratedFileName", targetFilename);
            templateCodeLines.AppendLine("<#@ property processor=\"PropertyProcessor\" name=\"GeneratedFileName\" #>");
          }
        }

        /// <summary>
        /// Adds all arguments to the template code and adds all arguments with values to the arguments list
        /// </summary>
        /// <param name="templateCodeLines"></param>
        private void AddAllArguments(StringBuilder templateCodeLines, NameValueCollection overrideArguments)
        {
          IDictionaryService dictionaryService = GetService<IDictionaryService>();
          IConfigurationService b = (IConfigurationService)GetService(typeof(IConfigurationService));
          Microsoft.Practices.RecipeFramework.Configuration.Recipe recipe = b.CurrentRecipe;

          foreach (Microsoft.Practices.RecipeFramework.Configuration.Argument argument in recipe.Arguments)
          {
            if (!argument.Type.StartsWith("EnvDTE", StringComparison.InvariantCultureIgnoreCase))
            {
              object currentValue = dictionaryService.GetValue(argument.Name);

              //is there a override for this key
              if (overrideArguments[argument.Name] != null)
              {
                currentValue = overrideArguments[argument.Name];
                if (!base.additionalArguments.Contains(argument.Name))
                {
                  base.additionalArguments.Add(argument.Name, currentValue);
                }
                else
                {
                  //force override of that value
                  base.additionalArguments[argument.Name] = currentValue;
                }
              }
              else
              {
                if (!base.additionalArguments.Contains(argument.Name))
                {
                  base.additionalArguments.Add(argument.Name, currentValue);
                }
              }

              templateCodeLines.AppendLine("<#@ property processor=\"PropertyProcessor\" name=\"" + argument.Name + "\" #>");
            }
          }

          templateCodeLines.AppendLine("<#@ assembly name=\"System.dll\" #>");
          templateCodeLines.AppendLine("<#@ assembly name=\"SPALM.SPSF.Library.dll\" #>");
          templateCodeLines.AppendLine("<#@ import namespace=\"SPALM.SPSF.Library\" #>");
        }

        private string Render(string templateCode, string templateFile)
        {
          EnvDTE.DTE vs = this.GetService<EnvDTE.DTE>(true);
          string basePath = this.GetBasePath();
          Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngine engine = new Microsoft.VisualStudio.TextTemplating.Engine();
          IValueInfoService service = (IValueInfoService)this.GetService(typeof(IValueInfoService));
          Dictionary<string, PropertyData> arguments = new Dictionary<string, PropertyData>();
          foreach (string str2 in base.additionalArguments.Keys)
          {
            Type type = null;
            try
            {
              type = service.GetInfo(str2).Type;
            }
            catch (ArgumentException)
            {
              if (base.additionalArguments[str2] != null)
              {
                type = base.additionalArguments[str2].GetType();
              }
              else
              {
                continue;
              }
            }
            PropertyData data = new PropertyData(base.additionalArguments[str2], type);
            arguments.Add(str2, data);
          }
          TemplateHost host = new TemplateHost(basePath, arguments);
          host.TemplateFile = templateFile;
          Helpers.LogMessage(vs, this, templateFile);

          string str3 = engine.ProcessTemplate(templateCode, host);
          if (host.Errors.HasErrors)
          {
            string errors = "";
            foreach (CompilerError error in host.Errors)
            {
              Helpers.LogMessage(vs, this, error.ErrorText);
              errors += error.ErrorText + Environment.NewLine;
            }
            throw new TemplateException(host.Errors);
          }
          if (host.Errors.HasWarnings)
          {
            StringBuilder builder = new StringBuilder();
            foreach (CompilerError error in host.Errors)
            {
              builder.AppendLine(error.ErrorText);
            }
            //Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "CompilationWarnings", new object[] { templateFile, builder.ToString() }));
          }
          return str3;
        }
        private void AddFileInternal(DTE dte, Project project, string evaluatedSourceFileName, string evaluatedTargetFileName, string evaluatedDeploymentPath)
        {
            if (!Path.IsPathRooted(evaluatedSourceFileName))
            {
                evaluatedSourceFileName = Path.Combine(GetTemplateBasePath(), evaluatedSourceFileName);
            }

            //ok, check the parameters
            if (!File.Exists(evaluatedSourceFileName))
            {
                //ignore this action if no source file is found, used e.g. in contenttype when no file is given
                return;
            }
            if (string.IsNullOrEmpty(evaluatedTargetFileName))
            {
                evaluatedTargetFileName = Path.GetFileName(evaluatedSourceFileName);
            }

            //targetfolder specified, find the project item
            if (!string.IsNullOrEmpty(TargetFolder) && (ParentProjectFolder == null))
            {
                //overwrite target folder                
                ParentProjectFolder = Helpers.GetFolder(project, TargetFolder, true);
                CreatedProjectFolder = this.ParentProjectFolder;
            }

            if (Helpers2.IsSharePointVSTemplate(dte, project))
            {
                if (this.ParentProjectItem != null)
                {
                    //used to add code files to a parent file
                    CreatedProjectItem = Helpers.AddFromTemplate(ParentProjectItem.ProjectItems, evaluatedSourceFileName, evaluatedTargetFileName, Overwrite);
                    if (DeploymentTypeIsSet)
                    {
                        SetDeploymentPath(dte, project, CreatedProjectItem, this.DeploymentType, evaluatedDeploymentPath);
                    }
                }
                else if (this.ParentProjectFolder != null)
                {
                    //we place the file directly in the given folder 
                    //and mapped the item to the deployment location
                    CreatedProjectItem = Helpers.AddFromTemplate(this.ParentProjectFolder.ProjectItems, evaluatedSourceFileName, evaluatedTargetFileName, Overwrite);
                    if (DeploymentTypeIsSet)
                    {
                        SetDeploymentPath(dte, project, CreatedProjectItem, this.DeploymentType, evaluatedDeploymentPath);
                    }
                }
                else if (DeploymentTypeIsSet)
                {
                    //place the file in a mapped folder
                    ProjectItems whereToAdd = Helpers2.GetDeploymentPath(dte, project, this.DeploymentType, evaluatedDeploymentPath);
                    CreatedProjectItem = Helpers.AddFromTemplate(whereToAdd, evaluatedSourceFileName, evaluatedTargetFileName, Overwrite);
                    //do not set the deploymentpath as we already placed the file in the mapped folder location
                }
                else if (project != null)
                {
                    CreatedProjectItem = Helpers.AddFromTemplate(project.ProjectItems, evaluatedSourceFileName, evaluatedTargetFileName, Overwrite);
                }
                else
                {
                    throw new Exception("Don't know where to place the file");
                }
            }

            if (CreatedProjectItem != null)
            {
                //set the build action
                if (CreatedProjectItem.Name.EndsWith(".resx", StringComparison.InvariantCultureIgnoreCase))
                {
                    CreatedProjectItem.Properties.Item("BuildAction").Value = 2;
                }                
            }


            if (this.Open)
            {
                if (this.CreatedProjectItem != null)
                {
                    Window window = this.CreatedProjectItem.Open("{00000000-0000-0000-0000-000000000000}");
                    window.Visible = true;
                    window.Activate();
                }
            }

        }

        private void SetDeploymentPath(DTE dte, Project project, ProjectItem CreatedProjectItem, SPFileType sPFileType, string evaluatedDeploymentPath)
        {


          //set to content
          if ((sPFileType != SPFileType.CustomCode))
          {
            CreatedProjectItem.Properties.Item("BuildAction").Value = 2;
          }

            //ok, file is placed, but we need set the deployment path
            ISharePointProjectService projectService = Helpers2.GetSharePointProjectService(dte);
            ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(project);

            sharePointProject.Synchronize();

            if (CreatedProjectItem.Collection.Parent is ProjectItem)
            {
                ProjectItem parentItem = CreatedProjectItem.Collection.Parent as ProjectItem;
                string name = parentItem.Name;

                //is the parent element a feature?
                try
                {
                    ISharePointProjectFeature parentIsFeature = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectFeature>(parentItem);
                    if (parentIsFeature != null)
                    {
                        ISharePointProjectItem addedSharePointItem = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItem>(CreatedProjectItem);
                        if (addedSharePointItem != null)
                        {
                            parentIsFeature.ProjectItems.Add(addedSharePointItem);
                        }
                    }
                }
                catch { }
            }

            try
            {
              //sometimes property deploymentpath is readonly
              //1. new added items need to be validated before 
              ISharePointProjectItemFile newaddedSharePointItem = projectService.Convert<EnvDTE.ProjectItem, ISharePointProjectItemFile>(CreatedProjectItem);
              newaddedSharePointItem.DeploymentType = Helpers2.GetDeploymentTypeFromFileType(this.DeploymentType);
              if (!string.IsNullOrEmpty(evaluatedDeploymentPath))
              {
                newaddedSharePointItem.DeploymentPath = evaluatedDeploymentPath;
              }
            }
            catch { }
        }

        public override void Undo()
        {
            //in error case try delete dummy file
            Helpers2.DeleteDummyFile((DTE)this.GetService(typeof(DTE)), null, false);
        }
    }   
}