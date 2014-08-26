using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Reflection;
using EnvDTE;

namespace SPALM.SPSF.Library
{
  public class WSPExtractor
  {
    private string coretempFolder;
    private string sourceTempFolder;
    private string targetTempFolder;
    private string wspFilename;
    private string fullPathToExtractExe;

    public WSPExtractor(string _fullPathToExtractExe, string _wspFilename)
    {
      wspFilename = _wspFilename;
      fullPathToExtractExe = _fullPathToExtractExe;
    }

    private void CreateTempFolder()
    {
      coretempFolder = Path.Combine(Path.GetTempPath(), "WSPExtract" + Guid.NewGuid().ToString());
      if (Directory.Exists(coretempFolder))
      {
        Directory.Delete(coretempFolder, true);
      }
      Directory.CreateDirectory(coretempFolder);

      sourceTempFolder = Path.Combine(coretempFolder, "copy");
      if (Directory.Exists(sourceTempFolder))
      {
        Directory.Delete(sourceTempFolder, true);
      }
      Directory.CreateDirectory(sourceTempFolder);

      targetTempFolder = Path.Combine(coretempFolder, "extracted"); // Path.GetTempPath();
      if (Directory.Exists(targetTempFolder))
      {
        Directory.Delete(targetTempFolder, true);
      }
      Directory.CreateDirectory(targetTempFolder);
    }

    private void DeleteTempFolder()
    {
      Directory.Delete(coretempFolder, true);
    }

    private void AddFilesToSolution(DTE dte, string sourceDirectory, Project targetProject)
    {
        string projectFolder = Helpers.GetFullPathOfProjectItem(targetProject);
      CopyFolder(dte, sourceDirectory, projectFolder, targetProject.ProjectItems);
    }

    private void CopyFolder(DTE dte, string sourceFolder, string destFolder, ProjectItems parentProjectItems)
    {
      //first copy all files
      string[] files = Directory.GetFiles(sourceFolder);
      foreach (string file in files)
      {
        string name = Path.GetFileName(file);
        string dest = Path.Combine(destFolder, name);
        if (File.Exists(dest))
        {
          //if (MessageBox.Show("Project file " + name + " already exists. Overwrite?", "Overwrite file", MessageBoxButtons.YesNo) == DialogResult.Yes)
          {
            //Helpers.WriteToOutputWindow(dte, "Warning: File " + dest + " replaced.", false);
            File.Copy(file, dest, true);
          }
        }
        else
        {
          File.Copy(file, dest, true);
        }


        try
        { //add file to project items
          ProjectItem addItem = AddFromFile(parentProjectItems, dest);
          addItem.Properties.Item("BuildAction").Value = 2;
        }
        catch (Exception)
        { //file already existed
        }
      }

      //second create the directories in the project if they are not there
      string[] sourcefolders = Directory.GetDirectories(sourceFolder);
      foreach (string sourcefolder in sourcefolders)
      {
        string name = Path.GetFileName(sourcefolder);
        string dest = Path.Combine(destFolder, name);

        //create folder in the project if it not exists
        ProjectItem projectFolder = null;
        try
        {
          projectFolder = GetProjectItemByName(parentProjectItems, name);
        }
        catch (Exception)
        {
        }
        if (projectFolder == null)
        {
          projectFolder = parentProjectItems.AddFolder(name, EnvDTE.Constants.vsProjectItemKindPhysicalFolder);
        }
        CopyFolder(dte, sourcefolder, dest, projectFolder.ProjectItems);
      }
    }

    internal ProjectItem GetProjectItemByName(ProjectItems pitems, string name)
    {
      foreach (ProjectItem pitem in pitems)
      {
        if (pitem.Name.ToUpper() == name.ToUpper())
        {
          return pitem;
        }
      }
      return null;
    }

    internal ProjectItem AddFromFile(ProjectItems projectItems, string filename)
    {
      return projectItems.AddFromFile(filename);
    }

    public void ExtractToSolutionToProject(DTE dte, Project targetProject)
    {
      CreateTempFolder();

      ExtractSolution();

      AddFilesToSolution(dte, targetTempFolder, targetProject);

      DeleteTempFolder();
    }

    /// <summary>
    /// Extracts a wsp solution and returns the path to the directory where the files are located
    /// </summary>
    /// <returns></returns>
    public string ExtractSolution()
    {
      //DirectoryInfo binDebugFolder = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

      //1. Create a subfolder in the tempFolder to strore the contents of the wsp
      string wspfoldername = Path.GetFileNameWithoutExtension(wspFilename);
      sourceTempFolder = Path.Combine(sourceTempFolder, wspfoldername);
      targetTempFolder = Path.Combine(targetTempFolder, wspfoldername);

      if (!Directory.Exists(sourceTempFolder))
      {
        Directory.CreateDirectory(sourceTempFolder);
      }
      if (!Directory.Exists(targetTempFolder))
      {
        Directory.CreateDirectory(targetTempFolder);
      }

      string command = fullPathToExtractExe; // 

      if (!File.Exists(command))
      {
				command = Path.Combine(fullPathToExtractExe, "Tools\\extract.exe");
				if (!File.Exists(command))
				{
					throw new Exception("Extract.exe not found at location " + command);
				}
      }

      //run process to expand the wsp to the target folder
      string commandarguments = "\"" + wspFilename + "\" /E *.* /L \"" + sourceTempFolder + "\"";
      ProcessStartInfo startInfo = new ProcessStartInfo(command, commandarguments);
      startInfo.CreateNoWindow = true;
      startInfo.UseShellExecute = false;
      System.Diagnostics.Process snProcess = System.Diagnostics.Process.Start(startInfo);
      snProcess.WaitForExit(20000);


      //2. extract the wsp file to that wsptempfolder
      //CabLib.Extract i_Extract = new CabLib.Extract();
      //i_Extract.ExtractFile(wspFilename, sourceTempFolder);

      //3. load manifest file to reorganize the contents
      string manifestfile = Path.Combine(sourceTempFolder, "manifest.xml");
      XmlDocument wspdoc = new XmlDocument();
      wspdoc.Load(manifestfile);

      #region FeatureManifests

      //find all features in this wsp
      XmlNamespaceManager wspnsmgr = new XmlNamespaceManager(wspdoc.NameTable);
      wspnsmgr.AddNamespace("ns", "http://schemas.microsoft.com/sharepoint/");
      XmlNodeList featureManifests = wspdoc.SelectNodes("/ns:Solution/ns:FeatureManifests/ns:FeatureManifest", wspnsmgr);
      foreach (XmlNode featureManifest in featureManifests)
      {
        //read the feature manifest, get the name
        //<FeatureManifest Location="BSHActivateCaching\feature.xml" />
        //get folder from the location
        string featurefoldername = featureManifest.Attributes["Location"].Value;
        if (featurefoldername.Contains("\\"))
        {
          featurefoldername = featurefoldername.Substring(0, featurefoldername.IndexOf("\\"));
        }
        else if (featurefoldername.Contains("/"))
        {
            featurefoldername = featurefoldername.Substring(0, featurefoldername.IndexOf("/"));
        }

        //move the feature folder to \SharePointRoot\TEMPLATE\FEATURES
        string oldfolder = Path.Combine(sourceTempFolder, featurefoldername);
        string newfolder = targetTempFolder + "\\SharePointRoot\\TEMPLATE\\FEATURES\\";

        if (!Directory.Exists(oldfolder))
        {
          throw new Exception(oldfolder + " not found");
        }
        if (!Directory.Exists(newfolder))
        {
          Directory.CreateDirectory(newfolder);
        }
        Directory.Move(oldfolder, newfolder + featurefoldername);
      }

      #endregion

      #region TemplateFiles

      //<TemplateFiles>
      //<TemplateFile Location="CONTROLTEMPLATES\BSH\BSHLinkControl.ascx" />
      XmlNodeList templateFiles = wspdoc.SelectNodes("/ns:Solution/ns:TemplateFiles/ns:TemplateFile", wspnsmgr);
      foreach (XmlNode templateFile in templateFiles)
      {
        string templateFilename = templateFile.Attributes["Location"].Value;
        string templateFolderName = "";
        if (templateFilename.Contains("\\"))
        {
          templateFolderName = templateFilename.Substring(0, templateFilename.LastIndexOf("\\"));
        }
        else if (templateFilename.Contains("/"))
        {
          templateFolderName = templateFilename.Substring(0, templateFilename.LastIndexOf("/"));
        }

        string oldfolder = Path.Combine(sourceTempFolder, templateFolderName);
        string newfolder = Path.Combine(targetTempFolder, "SharePointRoot\\TEMPLATE\\" + templateFolderName);

        if (!Directory.Exists(newfolder))
        {
          Directory.CreateDirectory(newfolder);
        }

        File.Copy(Path.Combine(sourceTempFolder, templateFilename), Path.Combine(targetTempFolder, "SharePointRoot\\TEMPLATE\\" + templateFilename));
      }

      #endregion

      #region Assemblies

      /* <Assemblies>
          <Assembly Location="BSH.Intranet.Customization.CommonComponents.dll" DeploymentTarget="GlobalAssemblyCache">
            <SafeControls>
              <SafeControl Assembly="BSH.Intranet.Customization.CommonComponents, Version=1.0.0.0, Culture=neutral, PublicKeyToken=01269d1a06e75182" Namespace="BSH.Intranet.Customization.CommonComponents" TypeName="*" Safe="True" />
            </SafeControls>
            <ClassResources>
				      <ClassResource Location="Resources/ConfigFile.xml" />
				      <ClassResource Location="Resources/EditorToolPart.js" />
				      <ClassResource Location="Resources/ListConfigFile.xml" />
             classresource landet im 80\\wpresources\\assemblynameOhneExtension\\
           </ClassResources>
          </Assembly>
        </Assemblies>
      */

      XmlNodeList assemblyFiles = wspdoc.SelectNodes("/ns:Solution/ns:Assemblies/ns:Assembly", wspnsmgr);
      foreach (XmlNode assemblyFile in assemblyFiles)
      {
        string assemblyFilename = assemblyFile.Attributes["Location"].Value;
        string deploymentTarget = assemblyFile.Attributes["DeploymentTarget"].Value;

        if (deploymentTarget == "GlobalAssemblyCache")
        {
          //copy to GAC
          string gacfolder = Path.Combine(targetTempFolder, "GAC");
          if (!Directory.Exists(gacfolder))
          {
            Directory.CreateDirectory(gacfolder);
          }

          //assemblyFilename can be "GAC\OldVersion\Assembly.Name.dll"
          //or only "Assembly.Name.dll" without folder
          string targetAssemblyName = EnsureDirectory(gacfolder, assemblyFilename);

          File.Copy(Path.Combine(sourceTempFolder, assemblyFilename), Path.Combine(gacfolder, targetAssemblyName));
        }
        else if (deploymentTarget == "WebApplication")
        { //copy to 80\BIN
          string bin80folder = Path.Combine(targetTempFolder, "80\\BIN");
          if (!Directory.Exists(bin80folder))
          {
            Directory.CreateDirectory(bin80folder);
          }

          string targetAssemblyName = EnsureDirectory(bin80folder, assemblyFilename);

          File.Copy(Path.Combine(sourceTempFolder, assemblyFilename), Path.Combine(bin80folder, targetAssemblyName));
        }

        //get name of assembly without extension
        FileInfo info = new FileInfo(assemblyFilename);
        string wpresourceFolder = Path.GetFileNameWithoutExtension(info.Name);

        XmlNodeList classResources = assemblyFile.SelectNodes("ns:ClassResources/ns:ClassResource", wspnsmgr);
        foreach (XmlNode classResource in classResources)
        {
          //each classresource file will be from root copy folder copied to 80\\wpresources\\assemlbynameWithoutExt
          {
            string classResFilename = classResource.Attributes["Location"].Value;
            string classResFolderName = "";
            if (classResFilename.Contains("\\"))
            {
              classResFolderName = classResFilename.Substring(0, classResFilename.LastIndexOf("\\"));
            }
            else if (classResFilename.Contains("/"))
            {
                classResFolderName = classResFilename.Substring(0, classResFilename.LastIndexOf("/"));
            }

            string oldfolder = Path.Combine(sourceTempFolder, classResFolderName);
            string newfolder = Path.Combine(targetTempFolder, "80\\wpresources\\" + wpresourceFolder + "\\" + classResFolderName);

            if (!Directory.Exists(newfolder))
            {
              Directory.CreateDirectory(newfolder);
            }

            File.Copy(Path.Combine(sourceTempFolder, classResFilename), Path.Combine(targetTempFolder, "80\\wpresources\\" + wpresourceFolder + "\\" + classResFilename));
          }

        }
      }

      XmlNodeList classResFiles = wspdoc.SelectNodes("/ns:Solution/ns:Assemblies/ns:Assembly/ns:ClassResources/ns:ClassResource", wspnsmgr);
      foreach (XmlNode classResFile in classResFiles)
      {
        //BIG TODO
      }
      /*
       * 
       * <Assembly DeploymentTarget="GlobalAssemblyCache" Location="RadEditorSharePoint.dll" >
      <SafeControls>
        <SafeControl Namespace="Telerik.SharePoint" TypeName="*" Safe="True" />
        <SafeControl Namespace="Telerik.SharePoint.FieldEditor" TypeName="*" Safe="True" />
        <SafeControl Namespace="Telerik.SharePoint.ListFieldEditor" TypeName="*" Safe="True" />
      </SafeControls>
      
       * */

      #endregion

      #region RootFiles

      //<RootFiles>
      //<RootFile Location="12er.xml" />
      XmlNodeList rootFiles = wspdoc.SelectNodes("/ns:Solution/ns:RootFiles/ns:RootFile", wspnsmgr);
      foreach (XmlNode rootFile in rootFiles)
      {
        string rootFilename = rootFile.Attributes["Location"].Value;
        string rootFolderName = "";
        if (rootFilename.Contains("\\"))
        {
          rootFolderName = rootFilename.Substring(0, rootFilename.LastIndexOf("\\"));
        }
        else if (rootFilename.Contains("/"))
        {
            rootFolderName = rootFilename.Substring(0, rootFilename.LastIndexOf("/"));
        }

        string oldfolder = Path.Combine(sourceTempFolder, rootFolderName);
        string newfolder = Path.Combine(targetTempFolder, "SharePointRoot\\" + rootFolderName);

        if (!Directory.Exists(newfolder))
        {
          Directory.CreateDirectory(newfolder);
        }

        File.Copy(Path.Combine(sourceTempFolder, rootFilename), Path.Combine(targetTempFolder, "SharePointRoot\\" + rootFilename));
      }

      #endregion

      #region ApplicationResourceFiles

      //<ApplicationResourceFiles>
      //<ApplicationResourceFile Location="SMCSupernetWebWebServices.resx"
      XmlNodeList appresFiles = wspdoc.SelectNodes("/ns:Solution/ns:ApplicationResourceFiles/ns:ApplicationResourceFile", wspnsmgr);
      foreach (XmlNode appresFile in appresFiles)
      {
        string appresFilename = appresFile.Attributes["Location"].Value;
        string appresFolderName = "";
        if (appresFilename.Contains("\\"))
        {
          appresFolderName = appresFilename.Substring(0, appresFilename.LastIndexOf("\\"));
        }
        else if (appresFilename.Contains("/"))
        {
            appresFolderName = appresFilename.Substring(0, appresFilename.LastIndexOf("/"));
        }

        string oldfolder = Path.Combine(sourceTempFolder, appresFolderName);
        string newfolder = Path.Combine(targetTempFolder, "80\\wpresources\\" + appresFolderName);

        if (!Directory.Exists(newfolder))
        {
          Directory.CreateDirectory(newfolder);
        }

        try
        {
          File.Copy(Path.Combine(sourceTempFolder, appresFilename), Path.Combine(targetTempFolder, "80\\wpresources\\" + appresFilename));
        }
        catch (Exception)
        {
        }
      }

      #endregion

      #region CodeAccessSecurity TODO extract xml to the CustomCAS.xml in the solution

      //no extraction of data needed, its only xml and we extract it to the manifest.xml

      #endregion

      #region  DwpFiles

      /* landen im 80\wpcatalog
       * <DwpFiles>
            <DwpFile Location="webpart1.dwp" />
          </DwpFiles>
       */

      XmlNodeList dwpFiles = wspdoc.SelectNodes("/ns:Solution/ns:DwpFiles/ns:DwpFile", wspnsmgr);
      foreach (XmlNode dwpFile in dwpFiles)
      {
        string dwpFilename = dwpFile.Attributes["Location"].Value;
        string dwpFolderName = "";
        if (dwpFilename.Contains("\\"))
        {
          dwpFolderName = dwpFilename.Substring(0, dwpFilename.LastIndexOf("\\"));
        }
        else if (dwpFilename.Contains("/"))
        {
            dwpFolderName = dwpFilename.Substring(0, dwpFilename.LastIndexOf("/"));
        }

        string oldfolder = Path.Combine(sourceTempFolder, dwpFolderName);
        string newfolder = Path.Combine(targetTempFolder, "80\\wpcatalog\\" + dwpFolderName);

        if (!Directory.Exists(newfolder))
        {
          Directory.CreateDirectory(newfolder);
        }

        File.Copy(Path.Combine(sourceTempFolder, dwpFilename), Path.Combine(targetTempFolder, "80\\wpcatalog\\" + dwpFilename));
      }


      #endregion

      #region Resources

      //only feature specific resources
      //every resource is added to the corresponding feature folder
      /* <Resources>
          <Resource Location="MR.Prototype.Branding.Theme\Resources\Resources.resx" />
        </Resources>
       * */

      //WE DO NOTHING: The resx. files are contained in the featurefolder and copied with the folder completely

      #endregion

      #region SiteDefinitionManifests

      /*
      <SiteDefinitionManifests>
          <SiteDefinitionManifest Location="BSHGlobal"> --> 
       *      // es gibt den Folder BSHGlobal im extrahierten Folder, der muss komplett nach Folder SharePointRoot\TEMPLATE\SiteTemplates\BSHGlobal kopiert werden
            <WebTempFile Location="1033\xml\WEBTEMPBSHGlobal.xml" /> müssen nach SharePointRoot\TEMPLATE kopiert werden
          </SiteDefinitionManifest>
        </SiteDefinitionManifests>
      */

      XmlNodeList siteDefinitions = wspdoc.SelectNodes("/ns:Solution/ns:SiteDefinitionManifests/ns:SiteDefinitionManifest", wspnsmgr);
      foreach (XmlNode siteDefinition in siteDefinitions)
      {
        string siteDefinitionFolder = siteDefinition.Attributes["Location"].Value;

        string oldfolder = Path.Combine(sourceTempFolder, siteDefinitionFolder);
        string newfolder = Path.Combine(targetTempFolder, "SharePointRoot\\TEMPLATE\\SiteTemplates\\");

        if (!Directory.Exists(newfolder))
        {
          Directory.CreateDirectory(newfolder);
        }
        Directory.Move(oldfolder, newfolder + "\\" + siteDefinitionFolder);
      }

      XmlNodeList webTemps = wspdoc.SelectNodes("/ns:Solution/ns:SiteDefinitionManifests/ns:SiteDefinitionManifest/ns:WebTempFile", wspnsmgr);
      foreach (XmlNode webTemp in webTemps)
      {
        string templateFilename = webTemp.Attributes["Location"].Value;
        string templateFolderName = "";
        if (templateFilename.Contains("\\"))
        {
          templateFolderName = templateFilename.Substring(0, templateFilename.LastIndexOf("\\"));
        }
        else if (templateFilename.Contains("/"))
        {
            templateFolderName = templateFilename.Substring(0, templateFilename.LastIndexOf("/"));
        }

        string oldfolder = Path.Combine(sourceTempFolder, templateFolderName);
        string newfolder = Path.Combine(targetTempFolder, "SharePointRoot\\TEMPLATE\\" + templateFolderName);

        if (!Directory.Exists(newfolder))
        {
          Directory.CreateDirectory(newfolder);
        }

        File.Copy(Path.Combine(sourceTempFolder, templateFilename), Path.Combine(targetTempFolder, "SharePointRoot\\TEMPLATE\\" + templateFilename));
      }

      #endregion

      //copy manifest.xml if exists
      if (File.Exists(Path.Combine(sourceTempFolder, "manifest.xml")))
      {
        File.Copy(Path.Combine(sourceTempFolder, "manifest.xml"), Path.Combine(targetTempFolder, "manifest.xml"));
      }

      return targetTempFolder;
    }

    private string EnsureDirectory(string gacfolder, string targetAssemblyName)
    {
      if (targetAssemblyName.Contains(@"\"))
      {
        //foldername in assembly
        //if it is "GAC", remove it
        if (targetAssemblyName.StartsWith(@"GAC\", StringComparison.InvariantCultureIgnoreCase))
        {
          targetAssemblyName = targetAssemblyName.Replace(@"GAC\", "");
        }
        //ensure that the subfolder exists in the target directory
        if (targetAssemblyName.Contains(@"\"))
        {
          string folderInGacFilename = targetAssemblyName.Substring(0, targetAssemblyName.LastIndexOf(@"\"));
          string directoryForGacFile = Path.Combine(gacfolder, folderInGacFilename);
          if (!Directory.Exists(directoryForGacFile))
          {
            Directory.CreateDirectory(directoryForGacFile);
          }
        }
      }
      return targetAssemblyName;
    }
  }
}
