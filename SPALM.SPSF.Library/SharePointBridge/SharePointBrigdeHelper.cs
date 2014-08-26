using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Remoting;
using EnvDTE;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using SPALM.SPSF.SharePointBridge;
using Microsoft.Win32;

namespace SPALM.SPSF.Library
{
  public class SharePointBrigdeHelper
  {
    private System.Diagnostics.Process bridgeProcess = null;
    private SharePointRemoteObject remoteObj = null;
    private DTE dte = null;
    private string toolsPath = "";

    public SharePointBrigdeHelper(DTE dte)
    {

      this.dte = dte;
      string version = Helpers.GetInstalledSharePointVersion();
      string sharePointBridgeExe = "SharePointBridge" + version + ".exe";
      DirectoryInfo assemblyFolder = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
      toolsPath = assemblyFolder + @"\" + sharePointBridgeExe;

      if (!File.Exists(toolsPath))
      {
        //ok, file is not in the current directory (issue that executing assembly is located in a different folder
        //let's check the registry for the installation location of SPSF

        RegistryKey packageKey = Registry.LocalMachine.OpenSubKey(dte.RegistryRoot + @"\BindingPaths\{93c68916-6e2b-43fb-9940-bf7c943cf0d9}", true);
        if (packageKey != null)
        {
          foreach (string s in packageKey.GetValueNames())
          {
            toolsPath = s + @"\" + sharePointBridgeExe;
            if (File.Exists(s))
            {
              break;
            }
          }
          if (!File.Exists(toolsPath))
          {
            Helpers.LogMessage(dte, dte, "Error: SharePointBridge '" + sharePointBridgeExe + "' not found ('" + toolsPath + "')");
            throw new Exception("File not found " + toolsPath);
          }
        }
      }
      Helpers.ShowProgress(dte, string.Format("Connecting to SharePoint {0}...", version == "14"?"2010":"2013"), 10);
      Helpers.LogMessage(dte, dte, string.Format("Connecting to SharePoint {0}. Please wait...", version == "14" ? "2010" : "2013"));

      if (IsBridgeNeeded)
      {
        StartBridge();
      }
      else
      {
        remoteObj = new SharePointRemoteObject();
      }
    }

    private int GetOSArchitecture()
    {
      string pa =
          Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
      return ((String.IsNullOrEmpty(pa) ||
               String.Compare(pa, 0, "x86", 0, 3, true) == 0) ? 32 : 64);
    }

    public bool IsBridgeNeeded
    {
      get
      {
        return true;
        //is the environment 64bit but the process is 32bit?
        /*
if (IntPtr.Size == 4)
{
  if (GetOSArchitecture() == 64)
  {
    Helpers.LogMessage(dte, dte, "Detected: Running as 32bit process on 64bit machine");
    return true;
  }
}
Helpers.LogMessage(dte, dte, "Detected: No bridge needed");
return false;
         * */
      }
    }

    ~SharePointBrigdeHelper()
    {
      StopBridge();
    }

    public Version GetSharePointVersion()
    {
      Version x = new Version();
      try
      {
        Helpers.ShowProgress(dte, "Retrieving SharePoint version...", 60);
        x = remoteObj.GetSharePointVersion();
      }
      catch (Exception ex)
      {
        Helpers.LogMessage(dte, dte, ex.Message);
      }
      finally
      {
        Helpers.HideProgress(dte);
        StopBridge();
      }
      return x;
    }

    public string GetPathToTraceLogs()
    {
      string path = "";
      try
      {
        Helpers.ShowProgress(dte, "Retrieving SharePoint Trace Log Location...", 60);
        path = remoteObj.GetPathToTraceLogs();
      }
      catch (Exception ex)
      {
        Helpers.LogMessage(dte, dte, ex.Message);
      }
      finally
      {
        Helpers.HideProgress(dte);
        StopBridge();
      }
      return path;
    }

    public void ExportContent(SharePointExportSettings exportSettings, string tempExportDir, string tempFilename, string tempLogFilePath)
    {
      try
      {
        Helpers.ShowProgress(dte, "Exporting SharePoint solution...", 60);
        remoteObj.ExportContent(exportSettings, tempExportDir, tempFilename, tempLogFilePath);
      }
      catch (Exception ex)
      {
        Helpers.LogMessage(dte, dte, ex.Message);
      }
      finally
      {
        Helpers.HideProgress(dte);
        StopBridge();
      }
    }

    public void ExportSolutionAsFile(string solutionName, string saveAsfilename)
    {

      try
      {
        Helpers.ShowProgress(dte, "Exporting SharePoint solution...", 60);
        remoteObj.ExportSolutionAsFile(solutionName, saveAsfilename);
      }
      catch (Exception ex)
      {
        Helpers.LogMessage(dte, dte, ex.Message);
      }
      finally
      {
        Helpers.HideProgress(dte);
        StopBridge();
      }
    }

    public void DeleteFailedDeploymentJobs()
    {
      try
      {
        remoteObj.DeleteFailedDeploymentJobs();
      }
      catch (Exception ex)
      {
        Helpers.LogMessage(dte, dte, ex.Message);
      }
      finally
      {
        Helpers.HideProgress(dte);
        StopBridge();
      }
    }

    private void StartBridge()
    {
      try
      {

        //IChannel ichannel = new IpcChannel("myClient");
        //ChannelServices.RegisterChannel(ichannel, false);
        string channelName = "SPSF" + Guid.NewGuid().ToString();

        bridgeProcess = new System.Diagnostics.Process();
        bridgeProcess.StartInfo.FileName = toolsPath;
        bridgeProcess.StartInfo.CreateNoWindow = true;
        bridgeProcess.StartInfo.Arguments = channelName;
        bridgeProcess.StartInfo.UseShellExecute = false;
        bridgeProcess.StartInfo.RedirectStandardInput = true;
        bridgeProcess.StartInfo.RedirectStandardOutput = true;
        bridgeProcess.StartInfo.RedirectStandardError = true;
        bridgeProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(toolsPath);
        bridgeProcess.OutputDataReceived += new DataReceivedEventHandler(bridgeProcess_OutputDataReceived);
        bridgeProcess.ErrorDataReceived += new DataReceivedEventHandler(bridgeProcess_ErrorDataReceived);
        bridgeProcess.EnableRaisingEvents = true;
        bridgeProcess.Start();
        bridgeProcess.BeginOutputReadLine();

        TimeSpan waitTime = new TimeSpan(0, 0, 5);
        System.Threading.Thread.Sleep(waitTime);

        try
        {
          remoteObj = Activator.GetObject(typeof(SharePointRemoteObject), "ipc://" + channelName + "/RemoteObj") as SharePointRemoteObject;
        }
        catch { }

        if (remoteObj == null)
        {
          for (int i = 0; i < 5; i++)
          {
            System.Threading.Thread.Sleep(waitTime);
            remoteObj = Activator.GetObject(typeof(SharePointRemoteObject), "ipc://" + channelName + "/RemoteObj") as SharePointRemoteObject;

            if (remoteObj == null)
            {
              break;
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Could not start SPSF SharePointBridge. Make sure that IIS and SharePoint are running (" + ex.Message + ")");
      }
      if (remoteObj == null)
      {
        throw new Exception("Could not access SharePointRemoteObject");
      }

      //Helpers.LogMessage(dte, dte, "SharePointBridge started");
    }

    void bridgeProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
      Helpers.LogMessage(dte, dte, "SharePointBridge error: " + e.Data);
    }

    void bridgeProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
      if (string.IsNullOrEmpty(e.Data))
      {
        return;
      }

      string message = e.Data;
      int percentage = 0;
      //to retrieve progress we have pipe as separator
      if (message.Contains("%"))
      {
        try
        {
          percentage = Int32.Parse(message.Substring(0, message.IndexOf("%")));
          message = message.Substring(message.IndexOf("%") + 1);
        }
        catch { }
      }
      if (percentage > 0)
      {
        Helpers.ShowProgress(dte, message, percentage);
      }
      Helpers.LogMessage(dte, dte, message);
    }

    private void StopBridge()
    {
      if (bridgeProcess != null)
      {
        if (!bridgeProcess.HasExited)
        {
          //Helpers.LogMessage(dte, dte, "Closing connection to SharePoint");
          bridgeProcess.Kill();
        }
      }
    }

    public string GetCentralAdministrationUrl()
    {
      string res = "";
      try
      {
        Helpers.ShowProgress(dte, "Retrieving url of Central Administration...", 60);
        res = remoteObj.GetCentralAdministrationUrl();
      }
      catch (Exception ex)
      {
        Helpers.LogMessage(dte, dte, ex.Message);
      }
      finally
      {
        Helpers.HideProgress(dte);
        StopBridge();
      }
      return res;
    }

    public void PerformDeploymentOperation(string operation, List<SharePointDeploymentJob> deploymentJobs)
    {
      try
      {
        remoteObj.PerformDeploymentOperation(operation, deploymentJobs);
      }
      catch (Exception ex)
      {
        Helpers.LogMessage(dte, dte, ex.Message);
      }
      finally
      {
        Helpers.HideProgress(dte);
        StopBridge();
      }
    }

    public void ExportListAsTemplate(string weburl, Guid listId, string tempPath)
    {
      try
      {
        Helpers.ShowProgress(dte, "Exporting list template...", 60);
        remoteObj.ExportListAsTemplate(weburl, listId, tempPath);
      }
      catch (Exception ex)
      {
        Helpers.LogMessage(dte, dte, ex.Message);
      }
      finally
      {
        Helpers.HideProgress(dte);
        StopBridge();
      }
    }

    public List<SharePointWebApplication> GetAllWebApplications()
    {
      List<SharePointWebApplication> res = new List<SharePointWebApplication>();
      try
      {
        Helpers.ShowProgress(dte, "Retrieving webapplication list...", 60);
        res = remoteObj.GetAllWebApplications();
        LogMessage("SharePointBrigdeHelper: GetAllWebApplications retrieved " + res.Count.ToString() + " items");
      }
      catch (Exception ex)
      {
        LogMessage("SharePointBrigdeHelper: " + ex.ToString());
        Helpers.LogMessage(dte, dte, ex.Message);
        Helpers.LogMessage(dte, dte, "Ensure that WWW Service and SharePoint database are running on your machine");
      }
      finally
      {
        Helpers.HideProgress(dte);
        StopBridge();
      }
      return res;
    }

    private static void LogMessage(string p)
    {
      //string logFile = Path.Combine(GetAssemblyDirectory(), "SharePointBridge.log");
      //File.AppendAllText(logFile, p + Environment.NewLine);
    }

    private static string GetAssemblyDirectory()
    {
      string codeBase = Assembly.GetExecutingAssembly().CodeBase;
      UriBuilder uri = new UriBuilder(codeBase);
      string path = Uri.UnescapeDataString(uri.Path);
      return Path.GetDirectoryName(path);
    }

    public SharePointWeb GetRootWebOfSite(string siteCollection)
    {
      SharePointWeb res = null;
      try
      {
        Helpers.ShowProgress(dte, "Retrieving webs of site " + siteCollection + "...", 60);
        res = remoteObj.GetRootWebOfSite(siteCollection);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
        Helpers.LogMessage(dte, dte, ex.ToString());
      }
      finally
      {
        Helpers.HideProgress(dte);
        StopBridge();
      }
      return res;
    }

    public SharePointWeb GetWeb(string webUrl)
    {
        SharePointWeb res = null;
        try
        {
            Helpers.ShowProgress(dte, "Retrieving web " + webUrl + "...", 60);
            res = remoteObj.GetWeb(webUrl);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            Helpers.LogMessage(dte, dte, ex.ToString());
        }
        finally
        {
            Helpers.HideProgress(dte);
            StopBridge();
        }
        return res;
    }

    public List<SharePointSiteCollection> GetAllSiteCollections()
    {
      List<SharePointSiteCollection> res = new List<SharePointSiteCollection>();
      try
      {
        Helpers.ShowProgress(dte, "Retrieving site collections list...", 60);
        res = remoteObj.GetAllSiteCollections();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
        Helpers.LogMessage(dte, dte, ex.ToString());
      }
      finally
      {
        Helpers.HideProgress(dte);
        StopBridge();
      }
      return res;
    }

    public List<SharePointField> GetSiteColumns(string siteUrl)
    {
      List<SharePointField> res = new List<SharePointField>();
      try
      {
        Helpers.ShowProgress(dte, "Retrieving content type list...", 60);
        res = remoteObj.GetSiteColumns(siteUrl);
      }
      catch (Exception ex)
      {
        Helpers.LogMessage(dte, dte, ex.Message);
      }
      finally
      {
        Helpers.HideProgress(dte);
        StopBridge();
      }
      return res;
    }

    public List<SharePointContentType> GetContentTypes(string siteUrl)
    {
      List<SharePointContentType> res = new List<SharePointContentType>();
      try
      {
        Helpers.ShowProgress(dte, "Retrieving content type list...", 60);
        res = remoteObj.GetContentTypes(siteUrl);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
        Helpers.LogMessage(dte, dte, ex.ToString());
      }
      finally
      {
        Helpers.HideProgress(dte);
        StopBridge();
      }
      return res;
    }

    public List<SharePointSolution> GetAllSharePointSolutions()
    {
      List<SharePointSolution> res = new List<SharePointSolution>();
      try
      {
        Helpers.ShowProgress(dte, "Retrieving solutions list...", 60);
        res = remoteObj.GetAllSharePointSolutions();
      }
      catch (Exception ex)
      {
        Helpers.LogMessage(dte, dte, ex.Message);
      }
      finally
      {
        Helpers.HideProgress(dte);
        StopBridge();
      }
      return res;
    }

    public void CheckBrokenFields(string siteCollectionUrl)
    {
      try
      {
        Helpers.ShowProgress(dte, "Checking for broken fields in site collection " + siteCollectionUrl, 60);
        remoteObj.CheckBrokenFields(siteCollectionUrl);
      }
      catch (Exception ex)
      {
        Helpers.LogMessage(dte, dte, ex.Message);
      }
      finally
      {
        Helpers.HideProgress(dte);
        StopBridge();
      }
    }
  }
}
