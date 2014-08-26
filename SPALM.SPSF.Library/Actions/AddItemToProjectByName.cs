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
	/// Adds an item to the solution
	/// </summary>
	[ServiceDependency(typeof(DTE))]
	public class AddItemToProjectByName : ConfigurableAction
	{
		private string _SourceFileName = "";
		private string content = "";

		[Input(Required = false)]
		public string SourceFileName
		{
			get { return _SourceFileName; }
			set { _SourceFileName = value; }
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

		[Input(Required = true)]
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
            if (string.IsNullOrEmpty(Content))
            {
                return;
            }

			DTE dte = base.GetService<DTE>(true);

			string fileWithContent = "";

			string testcontent = Content.Trim(new char[]{' ','\t','\n','\r'});
      if ((testcontent == "") || (testcontent == "dummy"))
      {
				//no content, check for sourcefile
        if (SourceFileName == null)
				{
					return;
				}
				if (SourceFileName == "")
				{
					return;
				}

				fileWithContent = this.SourceFileName;
				string templateBasePath = GetTemplateBasePath();
				if (!Path.IsPathRooted(fileWithContent))
				{
					fileWithContent = Path.Combine(templateBasePath, fileWithContent);
					fileWithContent = new FileInfo(fileWithContent).FullName;
				}
      }		
			else
			{
				fileWithContent = Path.GetTempFileName();
				using (StreamWriter writer = new StreamWriter(fileWithContent, false))
				{
					writer.Write(content);
					writer.Close();
				}
			}	

			if ((TargetFileName == null) || (TargetFileName == ""))
			{
				if (File.Exists(fileWithContent))
				{
					TargetFileName = Path.GetFileName(fileWithContent);
				}
			}

			Project projectByName = Helpers.GetProjectByName(dte, projectName);
			if (projectByName != null)
			{
				ProjectItems whereToAdd = projectByName.ProjectItems;

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

				//if targetfilename ends with .tt, we remove any file with the same start
				if (TargetFileName.EndsWith(".tt"))
				{
					//is there any file with the same name, but different extension
					foreach (ProjectItem pitem in whereToAdd)
					{
						if (Path.GetFileNameWithoutExtension(pitem.Name.ToUpper()) == Path.GetFileNameWithoutExtension(TargetFileName.ToUpper()))
						{
                            string deletedFilepath = Helpers.GetFullPathOfProjectItem(pitem);  //pHelpers.GetFullPathOfProjectItem(item);
							//delete the file
							pitem.Remove();
							File.Delete(deletedFilepath);
							break;
						}
					}
				}

				ProjectItem _ProjectItem = Helpers.AddFromTemplate(whereToAdd, fileWithContent, this.TargetFileName);
			}
			else
			{
				//do nothing if project is not found
				//throw new Exception("Project with name " + projectName + " not found in solution");
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