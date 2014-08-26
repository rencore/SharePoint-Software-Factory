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
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using SPALM.SPSF.SharePointBridge;

#endregion

namespace SPALM.SPSF.Library.Actions
{
	/// <summary>
	/// Exports a given
	/// </summary>
	[ServiceDependency(typeof(DTE))]
	public class RunContentExport : ConfigurableAction
	{
		private Project _TargetProject = null;
		private string _TargetFolder = "";
		private SharePointExportSettings _ExportSettings;
		private string _ExportDataFolder = "";
		private bool _ExtractAfterExport = false;

		private string _IncludeSecurity = "";
		private string _IncludeVersions = "";
		private string _ExportMethod = "";
		private bool _ExcludeDependencies = false;

		private const string EXPORTFILENAME = "exportedData.cab";

		private DTE dte = null;

		[Input(Required = true)]
		public Project TargetProject
		{
			get { return _TargetProject; }
			set { _TargetProject = value; }
		}

		[Input(Required = true)]
		public string TargetFolder
		{
			get { return _TargetFolder; }
			set { _TargetFolder = value; }
		}

		[Input(Required = true)]
		public string ExportDataFolder
		{
			get { return _ExportDataFolder; }
			set { _ExportDataFolder = value; }
		}

		[Input(Required = false)]
		public bool ExtractAfterExport
		{
			get { return _ExtractAfterExport; }
			set { _ExtractAfterExport = value; }
		}

		[Input(Required = true)]
		public string ExportMethod
		{
			get { return _ExportMethod; }
			set { _ExportMethod = value; }
		}

		[Input(Required = true)]
		public string IncludeSecurity
		{
			get { return _IncludeSecurity; }
			set { _IncludeSecurity = value; }
		}

		[Input(Required = true)]
		public string IncludeVersions
		{
			get { return _IncludeVersions; }
			set { _IncludeVersions = value; }
		}

		[Input(Required = true)]
		public bool ExcludeDependencies
		{
			get { return _ExcludeDependencies; }
			set { _ExcludeDependencies = value; }
		}

		[Input(Required = true)]
		public SharePointExportSettings ExportSettings
		{
			get { return _ExportSettings; }
			set { _ExportSettings = value; }
		}

		private string tempFilename = "";
		private string tempExportDir = "";
		private string tempLogFilePath = "";

		public override void Execute()
		{
			dte = GetService<DTE>(true);

			//set the directory where to place the output
			tempExportDir = Path.Combine(Path.GetTempPath(), "SPSFExport" + Guid.NewGuid().ToString());
			Directory.CreateDirectory(tempExportDir);
			tempFilename = EXPORTFILENAME;
			tempLogFilePath = Path.Combine(tempExportDir, "SPSFExport.log");

			_ExportSettings.ExportMethod = _ExportMethod;
			_ExportSettings.IncludeSecurity = _IncludeSecurity;
			_ExportSettings.IncludeVersions = _IncludeVersions;
			_ExportSettings.ExcludeDependencies = _ExcludeDependencies;

			//start bridge 
			SharePointBrigdeHelper helper = new SharePointBrigdeHelper(dte);
			helper.ExportContent(_ExportSettings, tempExportDir, tempFilename, tempLogFilePath);

			if (_ExtractAfterExport)
			{
				//should they be extracted before
				string tempExtractFolder = tempExportDir; // Path.Combine(tempExportDir, "extracted");
				//Directory.CreateDirectory(tempExtractFolder);
				string fileToExtract = Path.Combine(tempExportDir, EXPORTFILENAME);

				// Launch the process and wait until it's done (with a 10 second timeout).
				string commandarguments = "\"" + fileToExtract + "\" \"" + tempExtractFolder + "\" -F:*";
				ProcessStartInfo startInfo = new ProcessStartInfo("expand ", commandarguments);
				startInfo.CreateNoWindow = true;
				startInfo.UseShellExecute = false;
				System.Diagnostics.Process snProcess = System.Diagnostics.Process.Start(startInfo);
				snProcess.WaitForExit(20000);

				//rename
				bool renameFilesAfterExport = true;
				if (renameFilesAfterExport)
				{
					//get the manifest.xml
					string manifestFilename = Path.Combine(tempExportDir, "Manifest.xml");
					if (File.Exists(manifestFilename))
					{
						XmlDocument doc = new XmlDocument();
						doc.Load(manifestFilename);

						//get the contents of the elements node in the new xml 
						XmlNamespaceManager newnsmgr = new XmlNamespaceManager(doc.NameTable);
						newnsmgr.AddNamespace("ns", "urn:deployment-manifest-schema");

						//read all 
						XmlNodeList datFiles = doc.SelectNodes("/ns:SPObjects/ns:SPObject/ns:File", newnsmgr);
						foreach (XmlNode datFile in datFiles)
						{
							//DispForm.aspx
							if (datFile.Attributes["Name"] != null)
							{
								if (datFile.Attributes["FileValue"] != null)
								{
									//is there a file with name 00000001.dat
									string datFilename = datFile.Attributes["FileValue"].Value;
									string datFilePath = Path.Combine(tempExportDir, datFilename);
									if (File.Exists(datFilePath))
									{
										//ok, lets change the name
										string newfilename = Path.GetFileNameWithoutExtension(datFilePath);
										string itemname = datFile.Attributes["Name"].Value;
										itemname = itemname.Replace(".", "_");
										itemname = itemname.Replace(" ", "_");
                                        newfilename = newfilename + SPSFConstants.NameSeparator + itemname + ".dat";

										string newfilePath = Path.Combine(tempExportDir, newfilename);
										//rename the file
										File.Move(datFilePath, newfilePath);

										//update the manifest.xml
										datFile.Attributes["FileValue"].Value = newfilename;
									}
								}
							}
						}
						doc.Save(manifestFilename);
					}
				}

				//todo delete the cmp file
				if (File.Exists(fileToExtract))
				{
					File.Delete(fileToExtract);
				}

				//create the ddf out of the contents (except the logfile);
				StringBuilder ddfcontent = new StringBuilder();
				ddfcontent.AppendLine(";*** Sample Source Code MakeCAB Directive file example");
				ddfcontent.AppendLine(";");
				ddfcontent.AppendLine(".OPTION EXPLICIT     ; Generate errors");
				ddfcontent.AppendLine(".Set CabinetNameTemplate=exportedData.cab");
				ddfcontent.AppendLine(".set DiskDirectoryTemplate=CDROM ; All cabinets go in a single directory");
				ddfcontent.AppendLine(".Set CompressionType=MSZIP;** All files are compressed in cabinet files");
				ddfcontent.AppendLine(".Set UniqueFiles=\"OFF\"");
				ddfcontent.AppendLine(".Set Cabinet=on");
				ddfcontent.AppendLine(".Set DiskDirectory1=.");
				ddfcontent.AppendLine(".Set CabinetFileCountThreshold=0");
				ddfcontent.AppendLine(".Set FolderFileCountThreshold=0");
				ddfcontent.AppendLine(".Set FolderSizeThreshold=0");
				ddfcontent.AppendLine(".Set MaxCabinetSize=0");
				ddfcontent.AppendLine(".Set MaxDiskFileCount=0");
				ddfcontent.AppendLine(".Set MaxDiskSize=0");

				foreach (string s in Directory.GetFiles(tempExportDir, "*.*", SearchOption.AllDirectories))
				{
					if (!s.EndsWith(".log"))
					{
						FileInfo info = new FileInfo(s);
						ddfcontent.AppendLine(info.Name);
					}
				}

				ddfcontent.AppendLine("");
				ddfcontent.AppendLine(";*** The end");

				//write the ddf file
				string ddfFilename = Path.Combine(tempExportDir, "_exportedData.ddf");
				TextWriter writer = new StreamWriter(ddfFilename);
				writer.Write(ddfcontent.ToString());
				writer.Close();
			}

			//add all elements in temp folder to the folder exported objects in the project
			ProjectItem targetFolder = Helpers.GetProjectItemByName(TargetProject.ProjectItems, TargetFolder);
			if (targetFolder != null)
			{
				//is there already a folder named "ExportedData"
				ProjectItem exportedDataFolder = null;
				try
				{
					exportedDataFolder = Helpers.GetProjectItemByName(targetFolder.ProjectItems, "ExportedData");
				}
				catch (Exception)
				{
				}
				if (exportedDataFolder == null)
				{
					exportedDataFolder = targetFolder.ProjectItems.AddFolder("ExportedData", EnvDTE.Constants.vsProjectItemKindPhysicalFolder);
				}

				//empty the exportedDataFolder
				foreach (ProjectItem subitem in exportedDataFolder.ProjectItems)
				{
					subitem.Delete();
				}

				//add all files in temp folder to the project (except log file
				foreach (string s in Directory.GetFiles(tempExportDir, "*.*", SearchOption.AllDirectories))
				{
					if (!s.EndsWith(".log"))
					{
						exportedDataFolder.ProjectItems.AddFromFileCopy(s);
					}
				}

				//add log file to the parent folder
				if (File.Exists(tempLogFilePath))
				{
					targetFolder.ProjectItems.AddFromFileCopy(tempLogFilePath);
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