namespace SPALM.SPSF.SharePointBridge
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Runtime.Serialization; 
    using System.Reflection;
    using System.IO;
    using System.Net;
    using System.ServiceProcess;
    using System.Diagnostics;

   

    [Serializable()]
    public class SharePointExportSettings
    {
        public List<SharePointExportObject> ExportObjects { get; set; }
        public string SiteUrl { get; set; }

        public string ExportMethod { get; set; }
        public string IncludeSecurity { get; set; }
        public string IncludeVersions { get; set; }
        public bool ExcludeDependencies { get; set; }

        public SharePointExportSettings()
        {
            ExportObjects = new List<SharePointExportObject>();  
        }
    }

    [Serializable()]
    public class SharePointExportObject
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
    }

    [Serializable()]
    public class SharePointDeploymentJob
    {
        public string WSPName { get; set; }
        public string WSPFilename { get; set; }
        public string TargetSiteUrl { get; set; }
        public bool IsSandBoxedSolution { get; set; }
        public bool HasFailure { get; set; }
    }

    [Serializable()]
    public class SharePointItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid ID { get; set; }

        private List<SharePointItem> childItems;
        public List<SharePointItem> ChildItems
        {
            get
            {
                return childItems;
            }
            set
            {
                childItems = value;
            }
        }

        public SharePointItem(Microsoft.SharePoint.SPListItem item)
        {
            this.ID = item.UniqueId;

            if (item.FileSystemObjectType == Microsoft.SharePoint.SPFileSystemObjectType.File)
            {
                try
                {
                    Title = item.DisplayName;
                }
                catch (Exception)
                {
                    try
                    {
                        Title = item["Title"].ToString();
                    }
                    catch (Exception)
                    {
                        try
                        {
                            Title = item["Title0"].ToString();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                childItems = new List<SharePointItem>();
            }
            else if (item.FileSystemObjectType == Microsoft.SharePoint.SPFileSystemObjectType.Folder)
            {
                try
                {
                    Title = item.Title;
                }
                catch (Exception)
                {
                }
                childItems = ReadChildItems(item);
            }
        }

        private List<SharePointItem> ReadChildItems(Microsoft.SharePoint.SPListItem item)
        {
            List<SharePointItem> items = new List<SharePointItem>();
            Microsoft.SharePoint.SPQuery query = new Microsoft.SharePoint.SPQuery();
            query.Folder = item.Folder;
            Microsoft.SharePoint.SPListItemCollection col = item.ParentList.GetItems(query);
            foreach (Microsoft.SharePoint.SPListItem childItem in col)
            {
                items.Add(new SharePointItem(childItem));
            }
            return items;
        }
    }

    [Serializable()]
    public class SharePointWeb
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid ID { get; set; }
        public string Url { get; set; }

        public SharePointWeb(Microsoft.SharePoint.SPWeb spweb)
            : this(spweb, true, true)
        {
        }

        public SharePointWeb(Microsoft.SharePoint.SPWeb spweb, bool readSubWebs, bool readLists)
        {
            Title = spweb.Title;
            Description = spweb.Description;
            ID = spweb.ID;
            Url = spweb.Url;

            if (readSubWebs)
            {
                ChildWebs = ReadWebs(spweb);
            }
            if (readLists)
            {
                Lists = ReadLists(spweb);
            }            
        }

        private List<SharePointWeb> ReadWebs(Microsoft.SharePoint.SPWeb sPWeb)
        {
            List<SharePointWeb> res = new List<SharePointWeb>();
            foreach (Microsoft.SharePoint.SPWeb childsWeb in sPWeb.Webs)
            {
                SharePointWeb newWeb = new SharePointWeb(childsWeb);
                res.Add(newWeb);
            }
            return res;
        }

        private List<SharePointList> ReadLists(Microsoft.SharePoint.SPWeb sPWeb)
        {
            List<SharePointList> res = new List<SharePointList>();

            try
            {
                foreach (Microsoft.SharePoint.SPList spList in sPWeb.Lists)
                {
                    try
                    {
                        SharePointList list = new SharePointList(spList);
                        res.Add(list);


                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
            }

            return res;
        }

        private List<SharePointWeb> childWebs;
        public List<SharePointWeb> ChildWebs
        {
            get
            {
                return childWebs;
            }
            set
            {
                childWebs = value;
            }
        }

        private List<SharePointList> lists;
        public List<SharePointList> Lists
        {
            get
            {
                return lists;
            }
            set
            {
                lists = value;
            }
        }
    }

    [Serializable()]
    public class SharePointList
    {
        private List<SharePointItem> childItems;
        public List<SharePointItem> ChildItems
        {
            get
            {
                return childItems;
            }
            set
            {
                childItems = value;
            }
        }

        private List<SharePointContentType> contentTypes;
        public List<SharePointContentType> ContentTypes
        {
            get
            {
                return contentTypes;
            }
            set
            {
                contentTypes = value;
            }
        }

        public SharePointList(Microsoft.SharePoint.SPList spList)
        {

            AllowContentTypes = spList.AllowContentTypes;
            BaseTemplate = ((int)spList.BaseTemplate).ToString();
            BaseType = ((int)spList.BaseType).ToString();
            Description = spList.Description;
            EnableAttachments = spList.EnableAttachments;
            EnableFolderCreation = spList.EnableFolderCreation;
            EnableModeration = spList.EnableModeration;
            EnableVersioning = spList.EnableVersioning;
            Hidden = spList.Hidden;
            ID = spList.ID;
            ImageUrl = spList.ImageUrl;
            OnQuickLaunch = spList.OnQuickLaunch;
            ReadSecurity = spList.ReadSecurity.ToString();
            TemplateFeatureId = spList.TemplateFeatureId;
            Title = spList.Title;
            Url = spList.ParentWeb.Url;
            WriteSecurity = spList.WriteSecurity.ToString();
            SchemaXml = spList.SchemaXml;

            childItems = ReadItems(spList);

            contentTypes = ReadContenttypes(spList);
        }

        private List<SharePointContentType> ReadContenttypes(Microsoft.SharePoint.SPList spList)
        {
            List<SharePointContentType> items = new List<SharePointContentType>();

            foreach (Microsoft.SharePoint.SPContentType contentType in spList.ContentTypes)
            {
                SharePointContentType newCt = new SharePointContentType();
                newCt.SetValues(contentType);
                items.Add(newCt);
            }

            return items;
        }

        private List<SharePointItem> ReadItems(Microsoft.SharePoint.SPList spList)
        {
            List<SharePointItem> items = new List<SharePointItem>();

            Microsoft.SharePoint.SPQuery query = new Microsoft.SharePoint.SPQuery();
            query.Folder = spList.RootFolder;
            Microsoft.SharePoint.SPListItemCollection col = spList.GetItems(query);
            foreach (Microsoft.SharePoint.SPListItem item in col)
            {
                items.Add(new SharePointItem(item));
            }

            return items;
        }

        public string Url { get; set; }
        public Guid ID { get; set; }
        public Guid TemplateFeatureId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BaseTemplate { get; set; }
        public string BaseType { get; set; }
        public bool OnQuickLaunch { get; set; }
        public bool EnableAttachments { get; set; }
        public bool AllowContentTypes { get; set; }
        public bool EnableVersioning { get; set; }
        public bool EnableModeration { get; set; }
        public bool EnableFolderCreation { get; set; }
        public bool Hidden { get; set; }
        public string ReadSecurity { get; set; }
        public string WriteSecurity { get; set; }
        public string ImageUrl { get; set; }
        public string SchemaXml { get; set; }
    }

    [Serializable()]
    public class SharePointField
    {
        // Fields
        private string displayName;
        private string group;
        private Guid id;
        private int? maxLength;
        private string name;
        private string sourceID;
        private string staticName;
        private string description;

        public string SchemaXml { get; set; }

        // Methods
        public SharePointField(Microsoft.SharePoint.SPField field)
        {
            this.group = field.Group;
            this.id = field.Id;
            this.sourceID = field.SourceId;
            this.staticName = field.StaticName;
            this.name = field.InternalName;
            this.displayName = field.Title;
            this.description = field.Description;

            SchemaXml = field.SchemaXml;
        }

        public override string ToString()
        {
            return this.StaticName;
        }

        // Properties
        public string DisplayName
        {
            get
            {
                return this.displayName;
            }
            set
            {
                this.displayName = value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public string Group
        {
            get
            {
                return this.group;
            }
            set
            {
                this.group = value;
            }
        }

        public Guid Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public int? MaxLength
        {
            get
            {
                return this.maxLength;
            }
            set
            {
                this.maxLength = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public string SourceID
        {
            get
            {
                return this.sourceID;
            }
            set
            {
                this.sourceID = value;
            }
        }

        public string StaticName
        {
            get
            {
                return this.staticName;
            }
            set
            {
                this.staticName = value;
            }
        }
    }

    [Serializable()]
    public class SharePointContentType
    {
        // Fields
        private string description;
        private Guid featureID;
        private List<SharePointField> fields = new List<SharePointField>();
        private string group;
        private bool hidden;
        private string id;
        private string name;
        private bool readOnly;
        private string resourceFolder;
        private string version;
        private bool sealedField;

        public string SchemaXml { get; set; }

        public string FieldSchema;

        public SharePointContentType()
        {
        }

        public void SetValues(Microsoft.SharePoint.SPContentType contentType)
        {
            this.description = contentType.Description;
            this.group = contentType.Group;
            this.id = contentType.Id.ToString();
            this.name = contentType.Name;
            this.hidden = contentType.Hidden;
            this.readOnly = contentType.ReadOnly;
            this.sealedField = contentType.Sealed;
            this.resourceFolder = contentType.ResourceFolder.Name;
            this.version = contentType.Version.ToString();
            foreach (Microsoft.SharePoint.SPField field in contentType.Fields)
            {
                this.fields.Add(new SharePointField(field));
            }

            FieldSchema = "<FieldRefs>";
            foreach (Microsoft.SharePoint.SPFieldLink fieldLink in contentType.FieldLinks)
            {
                FieldSchema = FieldSchema + fieldLink.SchemaXml;
            }
            FieldSchema += "</FieldRefs>";

            this.SchemaXml = contentType.SchemaXml;

        }

        public override string ToString()
        {
            return this.Name;
        }

        // Properties
        public string Description
        {
            get
            {
                if (description == null)
                {
                    return "";
                }
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public Guid FeatureID
        {
            get
            {
                return this.featureID;
            }
            set
            {
                this.featureID = value;
            }
        }

        public List<SharePointField> Fields
        {
            get
            {
                return this.fields;
            }
            set
            {
                this.fields = value;
            }
        }

        public string Group
        {
            get
            {
                if (group == null)
                {
                    return "";
                }
                return this.group;
            }
            set
            {
                this.group = value;
            }
        }

        public string Version
        {
            get
            {
                if (version == null)
                {
                    return "";
                }
                return this.version;
            }
            set
            {
                this.version = value;
            }
        }

        public bool Hidden
        {
            get
            {
                return this.hidden;
            }
            set
            {
                this.hidden = value;
            }
        }

        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return this.readOnly;
            }
            set
            {
                this.readOnly = value;
            }
        }

        public string ResourceFolder
        {
            get
            {
                return this.resourceFolder;
            }
            set
            {
                this.resourceFolder = value;
            }
        }

        public bool Sealed
        {
            get
            {
                return this.sealedField;
            }
            set
            {
                this.sealedField = value;
            }
        }
    }

    [Serializable()]
    public class SharePointWebApplication
    {
        public string Name = "";
        public string Url = "";
        public string Description = "";
    }

    [Serializable()]
    public class SharePointSiteCollection
    {
        public Guid ID { get; set; }
        public string Url { get; set; }

        public SharePointSiteCollection()
        {
        }

        public SharePointSiteCollection(Microsoft.SharePoint.SPSite site)
        {
            Url = site.Url;
            ID = site.ID;
        }
    }

    [Serializable()]
    public class SharePointSolution
    {
        public string Name;
        public string DeploymentState;
        public string DisplayName;
        public Guid SolutionId;
        public long Version;
    }

    public class SharePointRemoteObject : MarshalByRefObject
    {
        public Version GetSharePointVersion()
        {
            LogMessage("SharePointRemoteObjects: Entering GetSharePointVersion");
            try
            {
                return Microsoft.SharePoint.Administration.SPFarm.Local.BuildVersion;
            }
            catch (Exception ex)
            {
                LogMessage("SharePointRemoteObjects: " + ex.ToString());
            }
            return new Version();
        }

        public string GetPathToTraceLogs()
        {
          ThrowIfFarmNotRunning();

            LogMessage("SharePointRemoteObjects: Entering GetPathToTraceLogs");
            try
            {
                string location = Microsoft.SharePoint.Administration.SPDiagnosticsService.Local.LogLocation;
                //result can contain e.g. %CommomProgramFolder%, we check this
                if (location.Contains("%"))
                {
                    string result = "";
                    char[] sep = new char[] { '\\' };
                    string[] folders = location.Split(sep);
                    foreach (string folder in folders)
                    {
                        if (!string.IsNullOrEmpty(folder))
                        {
                            string folderElement = folder;
                            if (folderElement.StartsWith("%") && folderElement.EndsWith("%"))
                            {
                                folderElement = Environment.GetEnvironmentVariable(folderElement.Replace("%", ""));
                            }
                            result += folderElement + "\\";
                        }
                    }
                    Console.WriteLine(result + " returned");
                    return result;
                }
                else
                {
                    Console.WriteLine(location + " returned");
                    return location;
                }
            }
            catch (Exception ex)
            {
                LogMessage("SharePointRemoteObjects: " + ex.ToString());
            }
            return "";

        }

        public string GetCentralAdministrationUrl()
        {
          ThrowIfFarmNotRunning();

            LogMessage("SharePointRemoteObjects: Entering GetCentralAdministrationUrl");
            Microsoft.SharePoint.Administration.SPWebApplication webApplication = Microsoft.SharePoint.Administration.SPAdministrationWebApplication.Local;
            return webApplication.GetResponseUri(Microsoft.SharePoint.Administration.SPUrlZone.Default).ToString();
        }

        /// <summary>
        /// gets all of the currently configured applications on the current sharepoint server
        /// </summary>
        /// <param name="applications">the collection to add it to</param>
        public static void AddAllWebApplications(Collection<Microsoft.SharePoint.Administration.SPWebApplication> applications)
        {
            LogMessage("SharePointRemoteObjects: Entering AddAllWebApplications");
            try
            {
                foreach (Microsoft.SharePoint.Administration.SPWebApplication app1 in Microsoft.SharePoint.Administration.SPWebService.AdministrationService.WebApplications)
                {
                    applications.Add(app1);
                }
            }
            catch (Exception ex)
            {
                LogMessage("SharePointRemoteObjects: " + ex.ToString());
            }
        }

        private static void LogMessage(string p)
        {
            string logFile = Path.Combine(GetAssemblyDirectory(), "SharePointBridge.log");
            File.AppendAllText(logFile, p + Environment.NewLine);
        }

        private static string GetAssemblyDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        /// <summary>
        ///  gets all of the currently configured applications on the current sharepoint content server
        /// </summary>
        /// <param name="applications">the collection to add it to</param>
        public static void AddAllContentWebApplications(Collection<Microsoft.SharePoint.Administration.SPWebApplication> applications)
        {
            LogMessage("SharePointRemoteObjects: Entering AddAllContentWebApplications");
            try
            {
                foreach (Microsoft.SharePoint.Administration.SPWebApplication app2 in Microsoft.SharePoint.Administration.SPWebService.ContentService.WebApplications)
                {
                    applications.Add(app2);
                }
            }
            catch (Exception ex)
            {
                LogMessage("SharePointRemoteObjects: " + ex.ToString());
            }
        }

        private static List<string> GetWebAppList(string webAppUrls)
        {
            List<string> webAppList = new List<string>();
            if (webAppUrls.Contains(";"))
            {
                char[] sep = new char[] { ';' };
                foreach (string strSep in webAppUrls.Split(sep))
                {
                    if (strSep != "")
                    {
                        webAppList.Add(strSep);
                    }
                }
            }
            else
            {
                //nur 1 element drin
                webAppList.Add(webAppUrls);
            }
            return webAppList;
        }

        private static void AddSpecificWebApplication(Collection<Microsoft.SharePoint.Administration.SPWebApplication> applications, string webAppUrls)
        {
            List<string> webapplist = GetWebAppList(webAppUrls);
            foreach (string webappurl in webapplist)
            {
                string url = webappurl;
                if (url.EndsWith("/"))
                {
                    url = url.Substring(0, url.Length - 1);
                }

                foreach (Microsoft.SharePoint.Administration.SPWebApplication app1 in Microsoft.SharePoint.Administration.SPWebService.AdministrationService.WebApplications)
                {
                    foreach (Microsoft.SharePoint.Administration.SPAlternateUrl alterurl in app1.AlternateUrls)
                    {
                        string checkurl = alterurl.IncomingUrl;
                        if (checkurl == url)
                        {
                            applications.Add(app1);
                        }
                    }
                }

                foreach (Microsoft.SharePoint.Administration.SPWebApplication app2 in Microsoft.SharePoint.Administration.SPWebService.ContentService.WebApplications)
                {
                    foreach (Microsoft.SharePoint.Administration.SPAlternateUrl alterurl in app2.AlternateUrls)
                    {
                        string checkurl = alterurl.IncomingUrl;
                        if (checkurl == url)
                        {
                            applications.Add(app2);
                        }
                    }
                }
            }
        }

        private static void RemoveExistingJob(Microsoft.SharePoint.Administration.SPSolution solution)
        {
            if (solution.JobExists)
            {
                if (solution.JobStatus == Microsoft.SharePoint.Administration.SPRunningJobStatus.Initialized)
                {
                    return;
                }

                //get all running jbs
                Microsoft.SharePoint.Administration.SPFarm localFarm = Microsoft.SharePoint.Administration.SPFarm.Local;
                Microsoft.SharePoint.Administration.SPTimerService service = localFarm.TimerService;
                foreach (Microsoft.SharePoint.Administration.SPJobDefinition definition in service.JobDefinitions)
                {
                    if (definition.Title != null && definition.Title.Contains(solution.Name))
                    {
                        Console.WriteLine("Deleting running job for solution " + solution.Name);
                        definition.Delete();
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
        }

        private static bool IsTimerServiceRunning()
        {
            try
            {
                if (CheckAndStartService("SPTimerV3"))
                {
                    return true;
                }
                if (CheckAndStartService("SPTimerV4"))
                {
                    return true;
                }
                if (CheckAndStartService("WSSTimerV3"))
                {
                    return true;
                }
                if (CheckAndStartService("WSSTimerV4"))
                {
                    return true;
                }
            }
            catch { }
            return false;
        }

        private static bool CheckAndStartService(string serviceName)
        {
            ServiceController sc = null;

            try
            {
                sc = new ServiceController(serviceName);
            }
            catch (Exception)
            {
                //service is not installed, ignore
                return false;
            }

            if (sc == null)
            {
                return false;
            }

            try
            {
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    return true;
                }

                Console.WriteLine("Service '" + serviceName + "' not running. Trying to start service...");

                //if not running try restarting
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 0, 10));

                if (sc.Status == ServiceControllerStatus.Running)
                {
                    return true;
                }

                Console.WriteLine("Service '" + serviceName + "' could not be started within 10 seconds");
            }
            catch
            {
            }
            return false;
        }

        public void PerformDeploymentOperation(string operation, List<SharePointDeploymentJob> deploymentJobs)
        {

            if (Microsoft.SharePoint.Administration.SPFarm.Local == null)
            {
                throw new Exception("Error: Local SharePoint farm not running");
            }

            Console.WriteLine("20%*** Running Deployment ***");

            if (!IsTimerServiceRunning())
            {
                throw new Exception("Error: Timer service not running, deployment will fail");
            }

            if (operation == "redeploy")
            {
                if (UndeploySolutions(deploymentJobs))
                {
                    Console.WriteLine("30%*** Wait for Successful Undeployment ***");
                    WaitForSuccessfulSolutionDeployment(deploymentJobs);
                }
                Console.WriteLine("40%*** Deleting Solutions ***");
                DeleteSolutions(deploymentJobs);
                Console.WriteLine("50%*** Adding Solutions ***");
                AddSolutions(deploymentJobs);
                Console.WriteLine("60%*** Deploying Solutions ***");
                DeploySolutions(deploymentJobs);
                Console.WriteLine("70%*** Wait for Successful Deployment ***");
                WaitForSuccessfulSolutionDeployment(deploymentJobs);
            }
            else if (operation == "deploy")
            {
                if (SolutionsExist(deploymentJobs))
                {
                  if (UndeploySolutions(deploymentJobs))
                  {
                    Console.WriteLine("30%*** Wait for Successful Undeployment ***");
                    WaitForSuccessfulSolutionDeployment(deploymentJobs);
                  }
                  Console.WriteLine("40%*** Deleting Solutions ***");
                  DeleteSolutions(deploymentJobs);                
                }

                Console.WriteLine("30%*** Adding Solutions ***");
                AddSolutions(deploymentJobs);
                Console.WriteLine("50%*** Deploying Solutions ***");
                DeploySolutions(deploymentJobs);
                Console.WriteLine("70%*** Wait for Successful Deployment ***");
                WaitForSuccessfulSolutionDeployment(deploymentJobs);
            }
            else if (operation == "undeploy")
            {
                Console.WriteLine("30%*** Undeploying Solutions ***");
                if (UndeploySolutions(deploymentJobs))
                {
                    Console.WriteLine("50%*** Wait for Successful Undeployment ***");
                    WaitForSuccessfulSolutionDeployment(deploymentJobs);
                }
                Console.WriteLine("70%*** Deleting Solutions ***");
                DeleteSolutions(deploymentJobs);
            }
            else if (operation == "upgrade")
            {
                Console.WriteLine("50%*** Upgrading Solutions ***");
                UpgradeSolutions(deploymentJobs);
            }

            CheckSuccessfullSolutionDeploymentOperations(operation, deploymentJobs);

            PingServers(deploymentJobs);
         

            int failures = 0;
            int successes = 0;
            foreach (SharePointDeploymentJob deploymentJob in deploymentJobs)
            {
                if (deploymentJob.HasFailure)
                {
                    failures++;
                }
                else
                {
                    successes++;
                }
            }
            Console.WriteLine("*** Deployment operation '" + operation + "' finished: " + successes.ToString() + " successful, " + failures.ToString() + " failed ***" + Environment.NewLine);
        }

        private bool SolutionsExist(List<SharePointDeploymentJob> deploymentJobs)
        {

          foreach (SharePointDeploymentJob deploymentJob in deploymentJobs)
          {
            try
            {
              if (IsSharePoint14() && deploymentJob.IsSandBoxedSolution)
              {
                Microsoft.SharePoint.SPSite site = new Microsoft.SharePoint.SPSite(deploymentJob.TargetSiteUrl);
                if (site != null)
                {
                  return SPUserSolutionsExist(deploymentJob, site);
                }
              }
              else
              {
                Microsoft.SharePoint.Administration.SPSolution solution = Microsoft.SharePoint.Administration.SPFarm.Local.Solutions[deploymentJob.WSPName];
                if (solution != null)
                {
                  Console.WriteLine("Found solution '" + solution.Name + "' in farm");                      
                  return true;
                }
              }
            }
            catch (Exception)
            {              
            }
          }
          return false;
        }

        private bool SPUserSolutionsExist(SharePointDeploymentJob deploymentJob, Microsoft.SharePoint.SPSite site)
        {
          foreach (Microsoft.SharePoint.SPUserSolution _userSolution in site.Solutions)
          {
            if (_userSolution.Name == deploymentJob.WSPName)
            {
              Console.WriteLine("Found solution '" + _userSolution.Name + "' in site '" + site.Url + "'");
              return true;
            }
          }
          return false;
        }

        private bool UndeploySolutions(List<SharePointDeploymentJob> deploymentJobs)
        {
            bool waitingForRetractNeeded = false;
            foreach (SharePointDeploymentJob deploymentJob in deploymentJobs)
            {
                try
                {
                    if (IsSharePoint14() && deploymentJob.IsSandBoxedSolution)
                    {
                        UndeployUserSolution(deploymentJob);
                    }
                    else
                    {
                        Microsoft.SharePoint.Administration.SPSolution solution = Microsoft.SharePoint.Administration.SPFarm.Local.Solutions[deploymentJob.WSPName];
                        if (solution != null)
                        {
                            if (solution.Deployed)
                            {
                                waitingForRetractNeeded = true;
                                if (solution.ContainsWebApplicationResource)
                                {
                                    Collection<Microsoft.SharePoint.Administration.SPWebApplication> webApplications = GetAllWebApplicationsForSolution();
                                    Console.WriteLine("Retracing solution '" + deploymentJob.WSPName + "'");
                                    solution.Retract(DateTime.Now - TimeSpan.FromDays(1), webApplications);
                                }
                                else
                                {
                                    Console.WriteLine("Retracing solution '" + deploymentJob.WSPName + "'");
                                    solution.Retract(DateTime.Now - TimeSpan.FromDays(1));
                                }
                            }
                        }
                        else
                        {
                            deploymentJob.HasFailure = true;
                            Console.WriteLine("Error retracting solution '" + deploymentJob.WSPName + "': Solution not found in Solution Store");
                        }
                    }
                }
                catch (Exception ex)
                {
                    deploymentJob.HasFailure = true;
                    Console.WriteLine("Error retracting solution '" + deploymentJob.WSPName + "': " + ex.Message);
                }
            }
            return waitingForRetractNeeded;
        }

        private bool IsSharePoint14()
        {
            if (Microsoft.SharePoint.Administration.SPFarm.Local.BuildVersion.ToString().StartsWith("14."))
            {
                return true;
            }
            return false;
        }

        private void UndeployUserSolution(SharePointDeploymentJob deploymentJob)
        {            
            Microsoft.SharePoint.SPSite site = new Microsoft.SharePoint.SPSite(deploymentJob.TargetSiteUrl);
            if (site != null)
            {
                Microsoft.SharePoint.SPUserSolution userSolution = null;
                foreach (Microsoft.SharePoint.SPUserSolution _userSolution in site.Solutions)
                {
                    if (_userSolution.Name == deploymentJob.WSPName)
                    {
                        userSolution = _userSolution;
                    }
                }
                if (userSolution != null)
                {
                    Console.WriteLine("Retracting user solution '" + deploymentJob.WSPName + "'");
                    site.Solutions.Remove(userSolution);
                }
                else
                {
                    deploymentJob.HasFailure = false;
                    Console.WriteLine("Error retracting user solution '" + deploymentJob.WSPName + "': Solution not activated");
                }
            }
            else
            {
                deploymentJob.HasFailure = true;
                Console.WriteLine("Error retracting user solution '" + deploymentJob.WSPName + "': Site '" + deploymentJob.TargetSiteUrl + "' not found ");
            }
            
        }

        private void DeleteUserSolution(SharePointDeploymentJob deploymentJob)
        { 
            Microsoft.SharePoint.SPSite site = new Microsoft.SharePoint.SPSite(deploymentJob.TargetSiteUrl);
            if (site != null)
            {
                bool solutionDeleted = false;
                Microsoft.SharePoint.SPList solutionsList = site.GetCatalog(Microsoft.SharePoint.SPListTemplateType.SolutionCatalog);
                foreach (Microsoft.SharePoint.SPListItem solutionItem in solutionsList.Items)
                {
                    if (solutionItem.File.Name.Equals(deploymentJob.WSPName))
                    {
                        Console.WriteLine("Deleting user solution '" + deploymentJob.WSPName + "' in Site '" + deploymentJob.TargetSiteUrl + "'");
                        solutionsList.Items.DeleteItemById(solutionItem.ID);
                        solutionDeleted = true;
                        break;
                    }
                }
                if (!solutionDeleted)
                {
                    deploymentJob.HasFailure = true;
                    Console.WriteLine("Error deleting user solution '" + deploymentJob.WSPName + "': Site '" + deploymentJob.TargetSiteUrl + "' not found ");
                }
            }
            else
            {
                deploymentJob.HasFailure = true;
                Console.WriteLine("Error deleting user solution '" + deploymentJob.WSPName + "': Site '" + deploymentJob.TargetSiteUrl + "' not found ");
            }
           
        }

        private void DeployUserSolution(SharePointDeploymentJob deploymentJob)
        {
            Console.WriteLine("Starting deployment of user solution '" + deploymentJob.WSPName + "' to '" + deploymentJob.TargetSiteUrl + "'");
            Microsoft.SharePoint.SPSite site = new Microsoft.SharePoint.SPSite(deploymentJob.TargetSiteUrl);
            if (site != null)
            {
                bool solutionDeployed = false;
                Microsoft.SharePoint.SPList solutionList = site.GetCatalog(Microsoft.SharePoint.SPListTemplateType.SolutionCatalog);
                foreach (Microsoft.SharePoint.SPListItem solutionItem in solutionList.Items)
                {
                    if (solutionItem.File.Name.Equals(deploymentJob.WSPName))
                    {
                        Console.WriteLine("Deploying user solution '" + deploymentJob.WSPName + "'");
                        site.Solutions.Add(solutionItem.ID);
                        solutionDeployed = true;
                        break;
                    }
                }
                if (!solutionDeployed)
                {
                    deploymentJob.HasFailure = true;
                    Console.WriteLine("Error deploying user solution '" + deploymentJob.WSPName + "'");
                }
            }
            else
            {
                deploymentJob.HasFailure = true;
                Console.WriteLine("Error removing user solution '" + deploymentJob.WSPName + "': Site '" + deploymentJob.TargetSiteUrl + "' not found ");
            }
 
        }

        private void AddUserSolution(SharePointDeploymentJob deploymentJob)
        {
            Console.WriteLine("Uploading sandboxed solution '" + deploymentJob.WSPName + "' to Site '" + deploymentJob.TargetSiteUrl + "'");

            Microsoft.SharePoint.SPSite site = new Microsoft.SharePoint.SPSite(deploymentJob.TargetSiteUrl);
            if (site != null)
            {
                Microsoft.SharePoint.SPList solutionGallery = site.GetCatalog(Microsoft.SharePoint.SPListTemplateType.SolutionCatalog);

                Byte[] fileContent = null;
                FileStream filestream = null;

                try
                {
                    filestream = File.OpenRead(deploymentJob.WSPFilename);
                    fileContent = new byte[Convert.ToInt32(filestream.Length)];
                    filestream.Read(fileContent, 0, Convert.ToInt32(filestream.Length));

                    solutionGallery.RootFolder.Files.Add(solutionGallery.RootFolder.Url + "/" + deploymentJob.WSPName, fileContent, true);
                    solutionGallery.Update();
                }
                catch (Exception)
                {
                    deploymentJob.HasFailure = true;
                    Console.WriteLine("Error uploading user solution '" + deploymentJob.WSPName + "' to Site '" + deploymentJob.TargetSiteUrl + "'");
                }
                finally
                {
                    if (filestream != null)
                    {
                        filestream.Close();
                    }
                }

            }
            else
            {
                deploymentJob.HasFailure = true;
                Console.WriteLine("Error adding user solution '" + deploymentJob.WSPName + "' to Site '" + deploymentJob.TargetSiteUrl + "' not found ");
            }
            
        }

        private void AddSolutions(List<SharePointDeploymentJob> deploymentJobs)
        {
            foreach (SharePointDeploymentJob deploymentJob in deploymentJobs)
            {
                
                try
                {
                    if (IsSharePoint14() && deploymentJob.IsSandBoxedSolution)
                    {
                        AddUserSolution(deploymentJob);
                    }
                    else
                    {
                        Console.WriteLine("Adding solution '" + deploymentJob.WSPName + "'");
                        Microsoft.SharePoint.Administration.SPSolution solution = Microsoft.SharePoint.Administration.SPFarm.Local.Solutions.Add(deploymentJob.WSPFilename);
                    }
                }
                catch (Exception ex)
                {
                    deploymentJob.HasFailure = true;
                    Console.WriteLine("Error adding solution '" + deploymentJob.WSPName + "': " + ex.Message);
                }

            }
        }

        private void DeploySolutions(List<SharePointDeploymentJob> deploymentJobs)
        {
            foreach (SharePointDeploymentJob deploymentJob in deploymentJobs)
            {
                try
                {
                    if (IsSharePoint14() && deploymentJob.IsSandBoxedSolution)
                    {
                        DeployUserSolution(deploymentJob);
                    }
                    else
                    {
                        Microsoft.SharePoint.Administration.SPSolution solution = Microsoft.SharePoint.Administration.SPFarm.Local.Solutions[deploymentJob.WSPName];
                        if (solution != null)
                        {
                            if (solution.ContainsWebApplicationResource)
                            {
                                Collection<Microsoft.SharePoint.Administration.SPWebApplication> webApplications = GetWebApplicationsForSolution(deploymentJob);
                                string webapps = "";
                                foreach (Microsoft.SharePoint.Administration.SPWebApplication webapp in webApplications)
                                {
                                    webapps += webapp.Name + ", ";
                                }
                                Console.WriteLine("Deploy solution '" + deploymentJob.WSPName + "' to " + webapps);
                                solution.Deploy(DateTime.Now - TimeSpan.FromDays(1), true, webApplications, true);
                            }
                            else
                            {
                                Console.WriteLine("Deploy solution '" + deploymentJob.WSPName + "' to all content urls");
                                solution.Deploy(DateTime.Now - TimeSpan.FromDays(1), true, true);
                            }
                        }
                        else
                        {
                            deploymentJob.HasFailure = true;
                            Console.WriteLine("Error deploying solution '" + deploymentJob.WSPName + "': Solution not found in Solution Store");
                        }
                    }
                }
                catch (Exception ex)
                {
                    deploymentJob.HasFailure = true;
                    Console.WriteLine("Error deploying solution '" + deploymentJob.WSPName + "': " + ex.Message);
                }
            }
        }

        private void UpgradeSolutions(List<SharePointDeploymentJob> deploymentJobs)
        {
            foreach (SharePointDeploymentJob deploymentJob in deploymentJobs)
            {
                try
                {
                    if (IsSharePoint14() && deploymentJob.IsSandBoxedSolution)
                    {
                        UndeployUserSolution(deploymentJob);
                        AddUserSolution(deploymentJob);
                        DeployUserSolution(deploymentJob);
                    }
                    else
                    {

                        Microsoft.SharePoint.Administration.SPSolution solution = Microsoft.SharePoint.Administration.SPFarm.Local.Solutions[deploymentJob.WSPName];
                        if (solution != null)
                        {
                            Console.WriteLine("Upgrading solution '" + deploymentJob.WSPName + "'");
                            solution.Upgrade(deploymentJob.WSPFilename, DateTime.Now - TimeSpan.FromDays(1));
                        }
                        else
                        {
                            deploymentJob.HasFailure = true;
                            Console.WriteLine("Error upgrading solution '" + deploymentJob.WSPName + "': Solution not found in Solution Store");
                        }
                    }
                }
                catch (Exception ex)
                {
                    deploymentJob.HasFailure = true;
                    Console.WriteLine("Error upgrading solution '" + deploymentJob.WSPName + "': " + ex.Message);
                }
            }
        }

        private void DeleteSolutions(List<SharePointDeploymentJob> deploymentJobs)
        {
            foreach (SharePointDeploymentJob deploymentJob in deploymentJobs)
            {
                try
                {
                    if (IsSharePoint14() && deploymentJob.IsSandBoxedSolution)
                    {
                        DeleteUserSolution(deploymentJob);
                    }
                    else
                    {
                        Microsoft.SharePoint.Administration.SPSolution solution = Microsoft.SharePoint.Administration.SPFarm.Local.Solutions[deploymentJob.WSPName];
                        if (solution != null)
                        {
                            Console.WriteLine("Deleting solution '" + deploymentJob.WSPName + "'");
                            solution.Delete();
                        }
                        else
                        {
                            deploymentJob.HasFailure = true;
                            Console.WriteLine("Error deploying solution '" + deploymentJob.WSPName + "': Solution not found in Solution Store");
                        }
                    }
                }
                catch (Exception ex)
                {
                    deploymentJob.HasFailure = true;
                    Console.WriteLine("Error deleting solution '" + deploymentJob.WSPName + "': " + ex.Message);
                }
            }
        }

        private Collection<Microsoft.SharePoint.Administration.SPWebApplication> GetAllWebApplicationsForSolution()
        {
            Collection<Microsoft.SharePoint.Administration.SPWebApplication> webApplications = new Collection<Microsoft.SharePoint.Administration.SPWebApplication>();
            if (true) AddAllWebApplications(webApplications);
            if (true) AddAllContentWebApplications(webApplications);
            return webApplications;
        }

        private Collection<Microsoft.SharePoint.Administration.SPWebApplication> GetWebApplicationsForSolution(SharePointDeploymentJob deploymentJob)
        {
            Collection<Microsoft.SharePoint.Administration.SPWebApplication> webApplications = new Collection<Microsoft.SharePoint.Administration.SPWebApplication>();
            Microsoft.SharePoint.Administration.SPSolution solution = Microsoft.SharePoint.Administration.SPFarm.Local.Solutions[deploymentJob.WSPName];
            if (solution.ContainsWebApplicationResource)
            {
                if (deploymentJob.TargetSiteUrl == "")
                {
                    if (true) AddAllWebApplications(webApplications);
                    if (true) AddAllContentWebApplications(webApplications);

                    if (webApplications.Count == 0) // try to make sure we have at least one
                    {
                        Microsoft.SharePoint.Administration.SPWebApplication app = Microsoft.SharePoint.Administration.SPWebService.AdministrationService.WebApplications.GetEnumerator().Current;
                        if (app == null) app = Microsoft.SharePoint.Administration.SPWebService.ContentService.WebApplications.GetEnumerator().Current;
                    }
                }
                else
                {
                    //getwebappwith the publishUrl
                    AddSpecificWebApplication(webApplications, deploymentJob.TargetSiteUrl);
                    if (webApplications.Count == 0) // try to make sure we have at least one
                    {
                        throw new Exception("Webapplication with name " + deploymentJob.TargetSiteUrl + " not found.");
                    }
                }
            }
            return webApplications;
        }

        private void PingServers(List<SharePointDeploymentJob> deploymentJobs)
        {
            Console.WriteLine("90%*** Send ping to SharePoint to refresh pages ***");

            //collecting all urls
            List<string> urls = new List<string>();
            foreach (SharePointDeploymentJob deploymentJob in deploymentJobs)
            {
                if (!string.IsNullOrEmpty(deploymentJob.TargetSiteUrl))
                {
                    if (!urls.Contains(deploymentJob.TargetSiteUrl))
                    {
                        urls.Add(deploymentJob.TargetSiteUrl);
                    }
                }
            }

            foreach (string url in urls)
            {
                Console.WriteLine("Send ping to " + url + ".");
                try
                {
                    //open the webpage
                    WebRequest webRequest = WebRequest.Create(url);
                    webRequest.Timeout = 2000;
                    webRequest.Credentials = CredentialCache.DefaultCredentials;
                    WebResponse response = webRequest.GetResponse();
                    response.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void CheckSuccessfullSolutionDeploymentOperations(string operation, List<SharePointDeploymentJob> deploymentJobs)
        {
            bool waitForDeployment = false;
            foreach(SharePointDeploymentJob job in deploymentJobs)
            {
                if(!job.IsSandBoxedSolution)
                {
                    waitForDeployment = true;
                }
            }
            if(!waitForDeployment)
            {
                return;
            }

            Console.WriteLine("80%*** Checking wsp solution for last operation result ***");

            if (Microsoft.SharePoint.Administration.SPFarm.Local == null)
            {
                throw new Exception("Error: Local SharePoint farm not running");
            }

            foreach (SharePointDeploymentJob deploymentJob in deploymentJobs)
            {
                if (operation.Equals("undeploy"))
                {
                    //check that solutions are not available
                    try
                    {
                        Microsoft.SharePoint.Administration.SPSolution solution = Microsoft.SharePoint.Administration.SPFarm.Local.Solutions[deploymentJob.WSPName];
                        if (solution == null)
                        {
                            Console.WriteLine("Undeployment operation for solution " + deploymentJob.WSPName + " successfully");
                        }
                        else
                        {
                            deploymentJob.HasFailure = true;
                            Console.WriteLine("Warning: Undeployment operation failed for solution " + deploymentJob.WSPName);
                        }
                    }
                    catch (Exception ex)
                    {
                        deploymentJob.HasFailure = true;
                        Console.WriteLine("Exception for " + deploymentJob.WSPName + ": " + ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        Microsoft.SharePoint.Administration.SPSolution solution = Microsoft.SharePoint.Administration.SPFarm.Local.Solutions[deploymentJob.WSPName];
                        if (solution != null)
                        {
                            if (DeploymentIsSuccessfully(solution))
                            {
                                Console.WriteLine("Deployment operation for solution " + deploymentJob.WSPName + " successfully");
                            }
                            else if (DeploymentHasWarnings(solution))
                            {
                                Console.WriteLine("Warning: Solution " + deploymentJob.WSPName + " has warnings (Message: " + solution.LastOperationDetails + ")");
                            }
                            else
                            {
                                deploymentJob.HasFailure = true;
                                Console.WriteLine("Error: Deployment operation failed for solution " + deploymentJob.WSPName + " (Message: " + solution.LastOperationDetails + ")");
                            }
                        }
                        else
                        {
                            //deploymentJob.HasFailure = true;
                            //Console.WriteLine("Solution '" + deploymentJob.WSPName + "'  not found in Solution Store");
                        }
                    }
                    catch (Exception ex)
                    {
                        deploymentJob.HasFailure = true;
                        Console.WriteLine("Exception for " + deploymentJob.WSPName + ": " + ex.Message);
                    }
                }
            }

        }

        private bool DeploymentIsSuccessfully(Microsoft.SharePoint.Administration.SPSolution solution)
        {
            if (solution.LastOperationResult == Microsoft.SharePoint.Administration.SPSolutionOperationResult.NoOperationPerformed)
            {
                return true;
            }
            if (solution.LastOperationResult == Microsoft.SharePoint.Administration.SPSolutionOperationResult.DeploymentSucceeded)
            {
                return true;
            }
            if (solution.LastOperationResult == Microsoft.SharePoint.Administration.SPSolutionOperationResult.RetractionSucceeded)
            {
                return true;
            }
            return false;
        }

        private bool WaitForSuccessfulSolutionDeployment(List<SharePointDeploymentJob> deploymentJobs)
        {
            //is there a solution which is not a sandboxed solution
            bool waitForDeployment = false;
            foreach(SharePointDeploymentJob job in deploymentJobs)
            {
                if(!job.IsSandBoxedSolution)
                {
                    waitForDeployment = true;
                }
            }
            if(!waitForDeployment)
            {
                return false;
            }

            ExecAdmSvcJobs();
            string message = "";
            bool throwException = false;
            int maxWaiting = 20;
            int currentWaiting = 0;
            try
            {
                bool deployFinished = false;
                while (!deployFinished)
                {
                    deployFinished = true;

                    foreach (SharePointDeploymentJob deploymentJob in deploymentJobs)
                    {
                        Microsoft.SharePoint.Administration.SPSolution solution = Microsoft.SharePoint.Administration.SPFarm.Local.Solutions[deploymentJob.WSPName];
                        if (DeploymentFailed(solution))
                        {
                            throwException = true;
                            Console.WriteLine("Solution deployment for '" + solution.Name + "' failed with '" + solution.LastOperationDetails + "'");
                        }
                        else if (DeploymentHasWarnings(solution))
                        {
                            //waiting
                            Console.WriteLine("Solution '" + solution.Name + "' has warnings:'" + solution.LastOperationDetails + "'");
                        }

                        if (DeploymentIsInProgress(solution))
                        {
                            deployFinished = false;
                            Console.WriteLine("Waiting for solution '" + solution.Name + "'");
                        }
                    }

                    if (currentWaiting > maxWaiting)
                    {
                        throw new Exception("Solution deployment failed. Timeout reached.");
                    }

                    if (!deployFinished)
                    {
                        System.Threading.Thread.Sleep(5000);
                        currentWaiting++;
                    }
                }
            }
            catch (Exception ex)
            {
                message += ex.Message;
                return false;
            }

            if (throwException)
            {
                Console.WriteLine(message);
                return false;
            }
            else
            {
                Console.WriteLine("Solutions successfully deployed.");
                return true;
            }
        }

        private static bool DeploymentHasWarnings(Microsoft.SharePoint.Administration.SPSolution solution)
        {
            if (solution.LastOperationResult == Microsoft.SharePoint.Administration.SPSolutionOperationResult.DeploymentWarningsOccurred)
            {
                return true;
            }
            if (solution.LastOperationResult == Microsoft.SharePoint.Administration.SPSolutionOperationResult.RetractionWarningsOccurred)
            {
                return true;
            }
            return false;
        }

        private static bool DeploymentIsInProgress(Microsoft.SharePoint.Administration.SPSolution solution)
        {
            if (!solution.JobExists)
            {
                return false;
            }
            if (solution.LastOperationResult == Microsoft.SharePoint.Administration.SPSolutionOperationResult.NoOperationPerformed)
            {
                return true;
            }
            return false;
        }


        private static bool DeploymentFailed(Microsoft.SharePoint.Administration.SPSolution solution)
        {
            if (solution.LastOperationResult == Microsoft.SharePoint.Administration.SPSolutionOperationResult.DeploymentFailedFeatureInstall)
            {
                return true;
            }
            if (solution.LastOperationResult == Microsoft.SharePoint.Administration.SPSolutionOperationResult.DeploymentFailedFileCopy)
            {
                return true;
            }
            if (solution.LastOperationResult == Microsoft.SharePoint.Administration.SPSolutionOperationResult.DeploymentSolutionValidationFailed)
            {
                return true;
            }
            if (solution.LastOperationResult == Microsoft.SharePoint.Administration.SPSolutionOperationResult.DeploymentFailedCabExtraction)
            {
                return true;
            }

            if (solution.LastOperationResult == Microsoft.SharePoint.Administration.SPSolutionOperationResult.RetractionFailedCouldNotRemoveFeature)
            {
                return true;
            }
            if (solution.LastOperationResult == Microsoft.SharePoint.Administration.SPSolutionOperationResult.RetractionFailedCouldNotRemoveFile)
            {
                return true;
            }

            return false;
        }

        private void ExecAdmSvcJobs()
        {
            Console.WriteLine("70%*** Running deployment jobs ***");

            string stsadm = Path.Combine(Microsoft.SharePoint.Utilities.SPUtility.GetGenericSetupPath("BIN"), "stsadm.exe"); ;
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = stsadm;
            psi.Arguments = "-o execadmsvcjobs";
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo = psi;
            bool fStarted = p.Start();

            if (!fStarted)
            {
                Console.WriteLine("Unable to start stsadm.");
            }

            while (!p.HasExited)
            {
                string text = p.StandardOutput.ReadLine();
                if (!String.IsNullOrEmpty(text))
                {
                    Console.WriteLine(text);
                }
                System.Threading.Thread.Sleep(100);
            }

            Console.WriteLine(p.StandardOutput.ReadToEnd());

        }

        private static bool CheckApplicableRunningJobs(Microsoft.SharePoint.Administration.SPServer server, bool quiet)
        {
            foreach (KeyValuePair<Guid, Microsoft.SharePoint.Administration.SPService> current in GetProvisionedServices(server))
            {
                Microsoft.SharePoint.Administration.SPService service = current.Value;
                SPAdministrationServiceJobDefinitionCollection definitions = new SPAdministrationServiceJobDefinitionCollection(service);
                if (CheckApplicableRunningJobs(server, definitions, quiet))
                    return true; // We've found running jobs so no point looking any further.

                Microsoft.SharePoint.Administration.SPWebService service2 = service as Microsoft.SharePoint.Administration.SPWebService;
                if (service2 != null)
                {
                    foreach (Microsoft.SharePoint.Administration.SPWebApplication webApplication in service2.WebApplications)
                    {
                        definitions = new SPAdministrationServiceJobDefinitionCollection(webApplication);
                        if (CheckApplicableRunningJobs(server, definitions, quiet))
                            return true;
                    }
                }
            }
            return false;
        }

        private static bool CheckApplicableRunningJobs(Microsoft.SharePoint.Administration.SPServer server, SPAdministrationServiceJobDefinitionCollection jds, bool quiet)
        {
            bool stillExecuting = false;

            foreach (Microsoft.SharePoint.Administration.SPJobDefinition definition in jds)
            {
                if (string.IsNullOrEmpty(definition.Name))
                    continue;

                bool isApplicable = false;
                if (!definition.IsDisabled)
                    isApplicable = ((definition.Server == null) || definition.Server.Id.Equals(server.Id));

                if (!isApplicable)
                {
                    // If it's not applicable then we don't really care if it's running or not.
                    continue;
                }

                Console.Write("Waiting on {0}.\r\n", definition.Name);

                stillExecuting = true;
            }
            return stillExecuting;
        }

        private static Dictionary<Guid, Microsoft.SharePoint.Administration.SPService> GetProvisionedServices(Microsoft.SharePoint.Administration.SPServer server)
        {
            Dictionary<Guid, Microsoft.SharePoint.Administration.SPService> dictionary = new Dictionary<Guid, Microsoft.SharePoint.Administration.SPService>(8);
            foreach (Microsoft.SharePoint.Administration.SPServiceInstance serviceInstance in server.ServiceInstances)
            {
                Microsoft.SharePoint.Administration.SPService service = serviceInstance.Service;
                if (serviceInstance.Status == Microsoft.SharePoint.Administration.SPObjectStatus.Online)
                {
                    if (dictionary.ContainsKey(service.Id))
                        continue;
                    dictionary.Add(service.Id, service);
                }
            }
            return dictionary;
        }

        public void DeleteFailedDeploymentJobs()
        {
          ThrowIfFarmNotRunning();

            foreach (Microsoft.SharePoint.Administration.SPService service in Microsoft.SharePoint.Administration.SPFarm.Local.Services)
            {
                foreach (Microsoft.SharePoint.Administration.SPRunningJob runningJob in service.RunningJobs)
                {
                    try
                    {
                        if (runningJob.JobDefinition != null)
                        {
                            if (runningJob.JobDefinition.Name.StartsWith("solution-deployment-"))
                            {
                                if ((runningJob.Status == Microsoft.SharePoint.Administration.SPRunningJobStatus.Failed) ||
                                    (runningJob.Status == Microsoft.SharePoint.Administration.SPRunningJobStatus.Aborted))
                                {
                                    Console.WriteLine("Deleting deployment job " + runningJob.JobDefinition);
                                    runningJob.JobDefinition.Delete();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception for deployment job " + runningJob.JobDefinition + ", " + ex.ToString());
                    }
                }
            }
        }

        public List<SharePointSolution> GetAllSharePointSolutions()
        {
          ThrowIfFarmNotRunning();

            List<SharePointSolution> result = new List<SharePointSolution>();

            Microsoft.SharePoint.Administration.SPSolutionCollection solutions = Microsoft.SharePoint.Administration.SPFarm.Local.Solutions;
            foreach (Microsoft.SharePoint.Administration.SPSolution solution in solutions)
            {
                SharePointSolution sharePointSolution = new SharePointSolution();
                sharePointSolution.Name = solution.Name;
                sharePointSolution.DeploymentState = solution.DeploymentState.ToString();
                sharePointSolution.DisplayName = solution.DisplayName;
                sharePointSolution.SolutionId = solution.SolutionId;
                sharePointSolution.Version = solution.Version;

                result.Add(sharePointSolution);
            }

            return result;
        }

        public List<SharePointWebApplication> GetAllWebApplications()
        {
          ThrowIfFarmNotRunning();

            List<SharePointWebApplication> result = new List<SharePointWebApplication>();

            Collection<Microsoft.SharePoint.Administration.SPWebApplication> webApplications = new Collection<Microsoft.SharePoint.Administration.SPWebApplication>();
            AddAllWebApplications(webApplications);
            AddAllContentWebApplications(webApplications);

            foreach (Microsoft.SharePoint.Administration.SPWebApplication spwebapp in webApplications)
            {
                try
                {
                    if (spwebapp.Sites.Count > 0)
                    {
                        foreach (Microsoft.SharePoint.SPSite site in spwebapp.Sites)
                        {
                            try
                            {
                                SharePointWebApplication sharePointwebapp = new SharePointWebApplication();
                                sharePointwebapp.Name = site.Url;
                                sharePointwebapp.Url = site.Url;
                                sharePointwebapp.Description = site.RootWeb.Description;
                                result.Add(sharePointwebapp);

                                LogMessage("SharePointRemoteObjects: Adding WebApp " + sharePointwebapp.Name);
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    else
                    {
                        //webap without a site                     
                        SharePointWebApplication sharePointwebapp = new SharePointWebApplication();
                        sharePointwebapp.Name = spwebapp.AlternateUrls[0].IncomingUrl;
                        sharePointwebapp.Url = spwebapp.AlternateUrls[0].IncomingUrl;
                        sharePointwebapp.Description = spwebapp.DisplayName;
                        result.Add(sharePointwebapp);
                        LogMessage("SharePointRemoteObjects: Adding WebApp " + sharePointwebapp.Name);
                    }
                }
                catch (Exception ex)
                {
                    LogMessage(ex.ToString());
                }
            }

            LogMessage("SharePointRemoteObjects: Returning " + result.Count + " webapps");
            return result;
        }

        public void CheckBrokenFields(string siteCollectionUrl)
        {
            using (Microsoft.SharePoint.SPSite site = new Microsoft.SharePoint.SPSite(siteCollectionUrl))
            {
                using (Microsoft.SharePoint.SPWeb rootWeb = site.RootWeb)
                {
                    try
                    {
                        foreach (Microsoft.SharePoint.SPField f in rootWeb.AvailableFields)
                        {
                            try
                            {
                                Console.WriteLine(f.Title);
                            }
                            catch (Exception ex2)
                            {
                                Console.WriteLine(ex2.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        public List<SharePointContentType> GetContentTypes(string siteUrl)
        {
          ThrowIfFarmNotRunning();

            List<SharePointContentType> result = new List<SharePointContentType>();

            Microsoft.SharePoint.SPSite site = new Microsoft.SharePoint.SPSite(siteUrl);
            foreach (Microsoft.SharePoint.SPContentType ct in site.RootWeb.ContentTypes)
            {
                SharePointContentType newCt = new SharePointContentType();
                newCt.SetValues(ct);
                result.Add(newCt);
            }

            return result;
        }


        public List<SharePointField> GetSiteColumns(string siteUrl)
        {
          ThrowIfFarmNotRunning();

            List<SharePointField> result = new List<SharePointField>();

            Microsoft.SharePoint.SPSite site = new Microsoft.SharePoint.SPSite(siteUrl);
            foreach (Microsoft.SharePoint.SPField ct in site.RootWeb.Fields)
            {
                result.Add(new SharePointField(ct));
            }

            return result;
        }

        /// <summary>
        /// Returns a SharePointWeb with all core data
        /// </summary>
        /// <param name="siteCollection"></param>
        /// <returns></returns>
        public SharePointWeb GetRootWebOfSite(string siteCollection)
        {
          ThrowIfFarmNotRunning();

            Microsoft.SharePoint.SPSite site = new Microsoft.SharePoint.SPSite(siteCollection);

            SharePointWeb rootWeb = ReadCompleteWeb(site.RootWeb);

            return rootWeb;
        }

        /// <summary>
        /// Returns a SharePointWeb with all core data
        /// </summary>
        /// <param name="siteCollection"></param>
        /// <returns></returns>
        public SharePointWeb GetWeb(string webUrl)
        {
            ThrowIfFarmNotRunning();

            using (Microsoft.SharePoint.SPSite siteCollection = new Microsoft.SharePoint.SPSite(webUrl))
            {

                Microsoft.SharePoint.SPWeb web = siteCollection.OpenWeb();
                SharePointWeb spweb = ReadCompleteWeb(web, false, true);
                return spweb;
            }            
        }

        private void ThrowIfFarmNotRunning()
        {
            try
            {
                Microsoft.SharePoint.Administration.SPFarm farm = Microsoft.SharePoint.Administration.SPFarm.Local;
                Version x = farm.BuildVersion;

                if (Microsoft.SharePoint.Administration.SPFarm.Local == null)
                {
                    throw new Exception("The local SharePoint farm is not available. Ensure that all necessary services are up and running.");
                }
            }
            catch
            {
                throw new Exception("The local SharePoint farm is not available. Ensure that all necessary services are up and running.");
            }
        }

        private SharePointWeb ReadCompleteWeb(Microsoft.SharePoint.SPWeb sPWeb, bool readSubwebs, bool readLists)
        {
            SharePointWeb web = new SharePointWeb(sPWeb, readSubwebs, readLists);

            return web;
        }

        private SharePointWeb ReadCompleteWeb(Microsoft.SharePoint.SPWeb sPWeb)
        {
            SharePointWeb web = new SharePointWeb(sPWeb);

            return web;
        }

        public List<SharePointSiteCollection> GetAllSiteCollections()
        {
          ThrowIfFarmNotRunning();

            List<SharePointSiteCollection> result = new List<SharePointSiteCollection>();

            Microsoft.SharePoint.Administration.SPFarm thisFarm = Microsoft.SharePoint.Administration.SPFarm.Local;
            Microsoft.SharePoint.Administration.SPWebService service = thisFarm.Services.GetValue<Microsoft.SharePoint.Administration.SPWebService>("");

            foreach (Microsoft.SharePoint.Administration.SPWebApplication webApp in service.WebApplications)
            {
                foreach (Microsoft.SharePoint.SPSite siteCollection in webApp.Sites)
                {
                    result.Add(new SharePointSiteCollection(siteCollection));
                }
            }

            return result;
        }


        public void ExportSolutionAsFile(string solutionName, string saveAsfilename)
        {
          ThrowIfFarmNotRunning();

            Microsoft.SharePoint.Administration.SPSolutionCollection sc = Microsoft.SharePoint.Administration.SPFarm.Local.Solutions;

            foreach (Microsoft.SharePoint.Administration.SPSolution s in sc)
            {
                if (s.Name == solutionName)
                {
                    Microsoft.SharePoint.Administration.SPPersistedFile file = s.SolutionFile;
                    file.SaveAs(saveAsfilename);
                    break;
                }
            }
        }

        //Exports a list as template
        public void ExportListAsTemplate(string weburl, Guid listId, string tempPath)
        {
            ExportListHelper.Export(new Uri(weburl), listId, tempPath);
        }

        public void ExportContent(SharePointExportSettings exportSettings, string tempExportDir, string tempFilename, string tempLogFilePath)
        {
            Microsoft.SharePoint.Deployment.SPExportSettings spExportsettings = new Microsoft.SharePoint.Deployment.SPExportSettings();
            spExportsettings.SiteUrl = exportSettings.SiteUrl;

            if (exportSettings.ExportMethod == "ExportChanges")
            {
                spExportsettings.ExportMethod = Microsoft.SharePoint.Deployment.SPExportMethodType.ExportChanges;
            }
            else
            {
                spExportsettings.ExportMethod = Microsoft.SharePoint.Deployment.SPExportMethodType.ExportAll;
            }

            if (exportSettings.IncludeSecurity == "All")
            {
                spExportsettings.IncludeSecurity = Microsoft.SharePoint.Deployment.SPIncludeSecurity.All;
            }
            else if (exportSettings.IncludeSecurity == "WssOnly")
            {
                spExportsettings.IncludeSecurity = Microsoft.SharePoint.Deployment.SPIncludeSecurity.WssOnly;
            }
            else
            {
                spExportsettings.IncludeSecurity = Microsoft.SharePoint.Deployment.SPIncludeSecurity.None;
            }

            if (exportSettings.IncludeVersions == "All")
            {
                spExportsettings.IncludeVersions = Microsoft.SharePoint.Deployment.SPIncludeVersions.All;
            }
            else if (exportSettings.IncludeVersions == "LastMajor")
            {
                spExportsettings.IncludeVersions = Microsoft.SharePoint.Deployment.SPIncludeVersions.LastMajor;
            }
            else if (exportSettings.IncludeVersions == "LastMajorAndMinor")
            {
                spExportsettings.IncludeVersions = Microsoft.SharePoint.Deployment.SPIncludeVersions.LastMajorAndMinor;
            }
            else
            {
                spExportsettings.IncludeVersions = Microsoft.SharePoint.Deployment.SPIncludeVersions.CurrentVersion;
            }

            spExportsettings.ExcludeDependencies = exportSettings.ExcludeDependencies;



            foreach (SharePointExportObject exObj in exportSettings.ExportObjects)
            {
                Microsoft.SharePoint.Deployment.SPExportObject exportObject = new Microsoft.SharePoint.Deployment.SPExportObject();
                exportObject.Id = new Guid(exObj.Id);
                exportObject.Url = exObj.Url;
                exportObject.IncludeDescendants = Microsoft.SharePoint.Deployment.SPIncludeDescendants.All;
                if (exObj.Type == "Web")
                {
                    exportObject.Type =Microsoft.SharePoint.Deployment.SPDeploymentObjectType.Web;
                }
                else if (exObj.Type == "List")
                {
                    exportObject.Type = Microsoft.SharePoint.Deployment.SPDeploymentObjectType.List;
                }
                else if (exObj.Type == "Item")
                {
                    exportObject.Type = Microsoft.SharePoint.Deployment.SPDeploymentObjectType.ListItem;
                }
                spExportsettings.ExportObjects.Add(exportObject);
            }

            spExportsettings.FileLocation = tempExportDir;
            spExportsettings.BaseFileName = tempFilename;
            spExportsettings.LogFilePath = tempLogFilePath;
            spExportsettings.OverwriteExistingDataFile = true;

            Microsoft.SharePoint.Deployment.SPExport export = new Microsoft.SharePoint.Deployment.SPExport(spExportsettings);
            export.Completed += new EventHandler<Microsoft.SharePoint.Deployment.SPDeploymentEventArgs>(export_Completed);
            export.Run();
        }

        void export_Completed(object sender, Microsoft.SharePoint.Deployment.SPDeploymentEventArgs e)
        {

        }
    }

    internal enum DeploymentOperation
    {
        Retract,
        Deploy,
        Delete,
        AddSolution
    }

    internal class SPAdministrationServiceJobDefinitionCollection : Microsoft.SharePoint.Administration.SPPersistedChildCollection<Microsoft.SharePoint.Administration.SPAdministrationServiceJobDefinition>
    {

        internal SPAdministrationServiceJobDefinitionCollection(Microsoft.SharePoint.Administration.SPService service)
            : base(service)
        {
        }

        internal SPAdministrationServiceJobDefinitionCollection(Microsoft.SharePoint.Administration.SPWebApplication webApplication)
            : base(webApplication)
        {

        }
    }
}
