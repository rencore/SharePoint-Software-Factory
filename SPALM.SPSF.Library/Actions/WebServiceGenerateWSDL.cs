#region Using Directives

using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// Exports a given
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class WebServiceGenerateWSDL : ConfigurableAction
    {
        private ProjectItem _WebServiceFile = null;

        [Input(Required = true)]
        public ProjectItem WebServiceFile
        {
            get { return _WebServiceFile; }
            set { _WebServiceFile = value; }
        }

        public override void Execute()
        {
            DTE service = (DTE)this.GetService(typeof(DTE));
            Helpers.ShowProgress(service, "Generating WSDL...", 10);

            string defaultIISAPP = Helpers.GetDefaultIISWebApp();

            string workingDirectory = Path.Combine(Path.GetTempPath(), "SPALMWSDL" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(workingDirectory);

            ProjectItem asmxfileParentFolder = (ProjectItem)WebServiceFile.ProjectItems.Parent;

            //build if necessary
            Helpers.WriteToOutputWindow(service, "Compile to get actual version of dll");
            Project project = WebServiceFile.ContainingProject;
            service.Solution.SolutionBuild.BuildProject(service.Solution.SolutionBuild.ActiveConfiguration.Name, project.UniqueName, true);

            //get the asmx file and copy to /_layouts
            string projectpath = Helpers.GetFullPathOfProjectItem(project);
            string projectfolder = Path.GetDirectoryName(projectpath);

            bool ASMXFileExisted = false;
            string fullasmxtarget = "";
            string asmxfilename = WebServiceFile.Name;
            string asmxfilenamewithoutext = asmxfilename.Substring(0, asmxfilename.LastIndexOf("."));
            string asmxfullPath = Helpers.GetFullPathOfProjectItem(WebServiceFile);
            if (File.Exists(asmxfullPath))
            {


                string targetfolder = Helpers.GetSharePointHive() + @"\TEMPLATE\LAYOUTS";
                Helpers.WriteToOutputWindow(service, "Copying asmx file to _layouts folder");
                fullasmxtarget = Path.Combine(targetfolder, asmxfilename);
                if (File.Exists(fullasmxtarget))
                {
                    ASMXFileExisted = true;
                }
                File.Copy(asmxfullPath, fullasmxtarget, true);
            }

            //add the assembly to the gac
            string OutputFileName = project.Properties.Item("OutputFileName").Value.ToString();
            string OutputPath = project.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value.ToString();
            string assemblypath = Path.Combine(Path.Combine(projectfolder, OutputPath), OutputFileName);

            if (!File.Exists(assemblypath))
            {
                //check GAC folder of project
                assemblypath = Path.Combine(projectfolder, "GAC");
                if (Directory.Exists(assemblypath))
                {
                    assemblypath = Path.Combine(assemblypath, OutputFileName);
                }
            }

            if (!File.Exists(assemblypath))
            {
                string message = "Warning: No assembly in project found";
                Helpers.LogMessage(service, this, message);
                MessageBox.Show(message);
            }

            if (File.Exists(assemblypath))
            {
                string gacutilpath = Helpers.GetGACUtil(service);
                //sear
                if (File.Exists(gacutilpath))
                {
                    Helpers.ShowProgress(service, "Generating WSDL...", 30);
                    Helpers.WriteToOutputWindow(service, "Install dll in GAC", false);
                    Helpers.RunProcess(service, gacutilpath, "/if " + assemblypath, true, workingDirectory, false);
                    Helpers.WriteToOutputWindow(service, "IISReset to force reload of dll", false);
                    Helpers.ShowProgress(service, "Generating WSDL...", 60);
                    Helpers.LogMessage(service, this, "IISRESET...");
                    Helpers.RunProcess(service, "iisreset", "", true, workingDirectory, false);
                }
                else
                {
                    string message =
                        "GACUTIL.exe not found on your system.\nPlease install .net or Windows SDK.\ni.e. Windows SDK 7.1 http://www.microsoft.com/download/en/details.aspx?id=8442";
                    Helpers.LogMessage(service, this, message);
                    MessageBox.Show(message);
                }
            }


            //call disco.exe 
            Helpers.LogMessage(service, this, "Getting path to disco.exe...");
            string discopath = Helpers.GetDiscoPath();
            if (discopath != "")
            {
                if (!defaultIISAPP.StartsWith("http:"))
                {
                    defaultIISAPP = "http://" + defaultIISAPP;
                }

                Helpers.ShowProgress(service, "Generating WSDL...", 80);
                string discoargument = " " + defaultIISAPP + "/_layouts" + Helpers.GetVersionedFolder(service) + "/" + asmxfilename;

                Helpers.LogMessage(service, this, "Ping server...");
                DeploymentHelpers.PingServer(service, discoargument, 20000);

                Helpers.LogMessage(service, this, "Running disco.exe...");
                Helpers.RunProcess(service, discopath, discoargument, true, workingDirectory, false);
            }
            else
            {
                string message = "Disco.exe not found on your system.\nPlease install .net or Windows SDK.\ni.e. Windows SDK 7.1 http://www.microsoft.com/download/en/details.aspx?id=8442";
                Helpers.LogMessage(service, this, message);
                MessageBox.Show(message);
            }
            //adding results to the project
            //WebService1.disco
            string finalwsdlpath = "";
            string finaldiscopath = "";
            string[] wsdls = Directory.GetFiles(workingDirectory, "*.wsdl");
            if (wsdls.Length > 0)
            {
                finalwsdlpath = wsdls[0];
            }
            string[] discos = Directory.GetFiles(workingDirectory, "*.disco");
            if (discos.Length > 0)
            {
                finaldiscopath = discos[0];
            }
            if (File.Exists(finalwsdlpath) && File.Exists(finaldiscopath))
            {
                string SharePointVersion = Helpers.GetInstalledSharePointVersion();

                //replace text in the files
                /*To register namespaces of the Windows SharePoint Services object model, open both the .disco and .wsdl files and replace the opening XML processing instruction -- <?xml version="1.0" encoding="utf-8"?> -- with instructions such as the following:
                <%@ Page Language="C#" Inherits="System.Web.UI.Page" %> 
                <%@ Assembly Name="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
                <%@ Import Namespace="Microsoft.SharePoint.Utilities" %> 
                <%@ Import Namespace="Microsoft.SharePoint" %>
                <% Response.ContentType = "text/xml"; %>
                */
                Helpers.ShowProgress(service, "Generating WSDL...", 90);

                StringBuilder wsdlreplaced = new StringBuilder();
                TextReader wsdlreader = new StreamReader(finalwsdlpath);
                string input = null;
                while ((input = wsdlreader.ReadLine()) != null)
                {
                    if (input.TrimStart(null).StartsWith("<?xml version="))
                    {
                        wsdlreplaced.AppendLine("<%@ Page Language=\"C#\" Inherits=\"System.Web.UI.Page\" %>");
                        wsdlreplaced.AppendLine("<%@ Assembly Name=\"Microsoft.SharePoint, Version=" + SharePointVersion + ".0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c\" %>");
                        wsdlreplaced.AppendLine("<%@ Import Namespace=\"Microsoft.SharePoint.Utilities\" %> ");
                        wsdlreplaced.AppendLine("<%@ Import Namespace=\"Microsoft.SharePoint\" %>");
                        wsdlreplaced.AppendLine("<% Response.ContentType = \"text/xml\"; %>");
                    }
                    else if (input.TrimStart(null).StartsWith("<soap:address"))
                    {
                        wsdlreplaced.AppendLine("<soap:address location=<% SPHttpUtility.AddQuote(SPHttpUtility.HtmlEncode(SPWeb.OriginalBaseUrl(Request)),Response.Output); %> />");
                    }
                    else if (input.TrimStart(null).StartsWith("<soap12:address"))
                    {
                        wsdlreplaced.AppendLine("<soap12:address location=<% SPHttpUtility.AddQuote(SPHttpUtility.HtmlEncode(SPWeb.OriginalBaseUrl(Request)),Response.Output); %> />");
                    }
                    else
                    {
                        wsdlreplaced.AppendLine(input);
                    }
                }
                wsdlreader.Close();
                TextWriter wsdlwriter = new StreamWriter(finalwsdlpath);
                wsdlwriter.Write(wsdlreplaced.ToString());
                wsdlwriter.Close();

                StringBuilder discoreplaced = new StringBuilder();
                TextReader discoreader = new StreamReader(finaldiscopath);
                string discoinput = null;
                while ((discoinput = discoreader.ReadLine()) != null)
                {
                    if (discoinput.TrimStart(null).StartsWith("<?xml version="))
                    {
                        discoreplaced.AppendLine("<%@ Page Language=\"C#\" Inherits=\"System.Web.UI.Page\" %>");
                        discoreplaced.AppendLine("<%@ Assembly Name=\"Microsoft.SharePoint, Version=" + SharePointVersion + ".0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c\" %>");
                        discoreplaced.AppendLine("<%@ Import Namespace=\"Microsoft.SharePoint.Utilities\" %> ");
                        discoreplaced.AppendLine("<%@ Import Namespace=\"Microsoft.SharePoint\" %>");
                        discoreplaced.AppendLine("<% Response.ContentType = \"text/xml\"; %>");
                    }
                    else if (discoinput.TrimStart(null).StartsWith("<contractRef"))
                    {
                        discoreplaced.AppendLine("<contractRef ref=<% SPHttpUtility.AddQuote(SPHttpUtility.HtmlEncode(SPWeb.OriginalBaseUrl(Request) + \"?wsdl\"),Response.Output); %> docRef=<% SPHttpUtility.AddQuote(SPHttpUtility.HtmlEncode(SPWeb.OriginalBaseUrl(Request)),Response.Output); %> xmlns=\"http://schemas.xmlsoap.org/disco/scl/\" />");
                    }
                    else if (discoinput.TrimStart(null).StartsWith("<soap address="))
                    {
                        //before
                        //<soap address="http://tfsrtm08/_layouts/WebService1.asmx" xmlns:q1="http://SMC.Supernet.Web.WebServices/" binding="q1:WebService1Soap" xmlns="http://schemas.xmlsoap.org/disco/soap/" />

                        //replaced
                        //<soap address=<% SPHttpUtility.AddQuote(SPHttpUtility.HtmlEncode(SPWeb.OriginalBaseUrl(Request)),Response.Output); %> xmlns:q1="http://tempuri.org/" binding="q1:HelloWorld" xmlns="http://schemas.xmlsoap.org/disco/soap/" />

                        //we replace the field adress
                        string originalstring = discoinput;

                        string beforeaddress = originalstring.Substring(0, originalstring.IndexOf(" address=") + 9);
                        string afteraddress = originalstring.Substring(originalstring.IndexOf("\"", originalstring.IndexOf(" address=") + 11));

                        //skip the quot
                        afteraddress = afteraddress.Substring(1);
                        discoreplaced.AppendLine(beforeaddress + "<% SPHttpUtility.AddQuote(SPHttpUtility.HtmlEncode(SPWeb.OriginalBaseUrl(Request)),Response.Output); %>" + afteraddress);
                    }
                    else
                    {
                        discoreplaced.AppendLine(discoinput);
                    }
                }
                discoreader.Close();
                TextWriter discowriter = new StreamWriter(finaldiscopath);
                discowriter.Write(discoreplaced.ToString());
                discowriter.Close();

                //files renaming needed
                //WebService.wsdl -> WebServiceWSDL.aspx
                //WebService.disco -> WebServiceDisco.aspx
                string renamedwsdlpath = Path.Combine(workingDirectory, asmxfilenamewithoutext + "WSDL.aspx");
                string renameddiscopath = Path.Combine(workingDirectory, asmxfilenamewithoutext + "Disco.aspx");

                File.Copy(finalwsdlpath, renamedwsdlpath);
                File.Copy(finaldiscopath, renameddiscopath);

                //add the files to the project
                ProjectItem wsdlItem = Helpers.AddFile(asmxfileParentFolder, renamedwsdlpath);
                ProjectItem discoItem = Helpers.AddFile(asmxfileParentFolder, renameddiscopath);

                //set the deployment target of the files to the same as the parent
                if (Helpers2.IsSharePointVSTemplate(service, project))
                {
                    Helpers2.CopyDeploymentPath(WebServiceFile, wsdlItem);
                    Helpers2.CopyDeploymentPath(WebServiceFile, discoItem);
                }
            }
            else
            {
                string message = "Created WSDL and DISCO files not found. Creation failed.";
                Helpers.LogMessage(service, this, message);
                MessageBox.Show(message);
            }

            try
            {
                //delete temp folder
                Directory.Delete(workingDirectory, true);

                //clean up everything what we have copied to the layouts folder
                if (ASMXFileExisted)
                {
                    File.Delete(fullasmxtarget);
                }
            }
            catch (Exception)
            {
            }

            Helpers.HideProgress(service);
        }

        /// <summary>
        /// Removes the previously added reference, if it was created
        /// </summary>
        public override void Undo()
        {
        }
    }
}