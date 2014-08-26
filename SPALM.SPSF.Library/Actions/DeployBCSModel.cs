#region Using Directives

using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;
using System.Xml;
using Microsoft.Practices.RecipeFramework.Services;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// Exports a given
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class DeployBCSModel : ConfigurableAction
    {
        private ProjectItem _BCSModelFile = null;

        [Input(Required = true)]
        public ProjectItem BCSModelFile
        {
            get { return _BCSModelFile; }
            set { _BCSModelFile = value; }
        }

        private string GetPowerShell()
        {
            if (File.Exists(Environment.ExpandEnvironmentVariables("%systemroot%\\sysnative") + @"\WindowsPowerShell\v1.0\PowerShell.exe"))
            {
                return Environment.ExpandEnvironmentVariables("%systemroot%\\sysnative") + @"\WindowsPowerShell\v1.0\PowerShell.exe";
            }
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\WindowsPowerShell\v1.0\PowerShell.exe"))
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.System) + @"\WindowsPowerShell\v1.0\PowerShell.exe";
            }
            return "";
        }

        protected string GetBasePath()
        {
            return base.GetService<IConfigurationService>(true).BasePath;
        }

        public override void Execute()
        {
            DTE service = (DTE)this.GetService(typeof(DTE));
            Helpers.ShowProgress(service, "Deploying BCS Model to local farm...", 10);

            if(BCSModelFile == null)
            {
                return;
            }

            try
            {
                //find the name of the model
                string bcsModelName = "";
                string bdcmFilename = Helpers.GetFullPathOfProjectItem(BCSModelFile);  //BCSModelFile.Properties.Item("FullPath").Value.ToString();
                string siteName = Helpers.GetDebuggingWebApp(service, GetBasePath());

                XmlDocument bdcmXml = new XmlDocument();
                bdcmXml.Load(bdcmFilename);

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(bdcmXml.NameTable);
                nsmgr.AddNamespace("ns", "http://schemas.microsoft.com/windows/2007/BusinessDataCatalog");

                XmlNode modelNode = bdcmXml.SelectSingleNode("/ns:Model", nsmgr);
                if(modelNode != null)
                {
                    bcsModelName = modelNode.Attributes["Name"].Value;
                }

                string command = GetPowerShell(); // @"C:\Windows\System32\..\System32\WindowsPowerShell\v1.0\PowerShell.exe";

                //string arguments = "-psconsolefile \"" + Helpers.GetSharePointHive() + "\\CONFIG\\POWERSHELL\\Registration\\psconsole.psc1\" -command \"";
                string arguments = "-command \"Add-PsSnapin Microsoft.SharePoint.PowerShell;";
                arguments += "Start-SPAssignment -Global;";              
                arguments += "Write 'Getting WebApplication of site " + siteName + "...'; ";
                arguments += "$s = Get-SPSiteAdministration -Identity '" + siteName + "'; ";
                arguments += "Write 'Getting Catalog...'; ";
                arguments += "$bdc = Get-SPBusinessDataCatalogMetadataObject -BdcObjectType Catalog -ServiceContext $s; ";
                arguments += "Write 'Getting existing model " + bcsModelName + "...'; ";
                arguments += "$modelFile = Get-SPBusinessDataCatalogMetadataObject -Name '" + bcsModelName + "' -BdcObjectType Model -ServiceContext $s; ";
                arguments += "if($modelFile) { Write 'Removing model...'; Remove-SPBusinessDataCatalogModel -Identity $modelFile -Confirm:$false } ; ";
                arguments += "Import-SPBusinessDataCatalogModel -Identity $bdc -Path '" + bdcmFilename + "' -force -ModelsIncluded -PropertiesIncluded -PermissionsIncluded -Verbose -ErrorAction Stop -ErrorVariable $err; ";
                arguments += "Stop-SPAssignment -Global; if($err) { Write $err; Exit(1) }; ";
                arguments += "Exit(0)";
                arguments += " \"";

                Helpers.RunProcessAsync(service, command, arguments, null);

            }
            catch(Exception ex)
            {
                Helpers.LogMessage(service, this, ex.ToString());
            }
                /*
             
             * 
            $serviceContext = Get-SPSiteAdministration -Identity $SiteUrl
 
Write-Host “Connecting to DBC”
 
$bdc = Get-SPBusinessDataCatalogMetadataObject -BdcObjectType Catalog -ServiceContext $serviceContext
 
Write-Host “Importing …”
 
Import-SPBusinessDataCatalogModel -Identity $bdc -Path “.\MyModelExport.xml” -force -ModelsIncluded -PropertiesIncluded -PermissionsIncluded -Verbose -ErrorAction Stop -ErrorVariable $err

             * 
             * 
$modelFile = Get-SPBusinessDataCatalogMetadataObject -Name "ContosoModel" -BdcObjectType Model -ServiceContext http://contoso 

Remove-SPBusinessDataCatalogModel -Identity $modelFile
             * 
             * 
             * 
            $MetadataStore = Get-SPBusinessDataCatalogMetadataObject -BdcObjectType "Catalog" -ServiceConext http://contoso 





Copy Code 
Import-SPBusinessDataCatalogModel -Path "C:\folder\model.bdcm" -Identity $MetadataStore
                 * */
        }

        /// <summary>
        /// Removes the previously added reference, if it was created
        /// </summary>
        public override void Undo()
        {
        }
    }
}