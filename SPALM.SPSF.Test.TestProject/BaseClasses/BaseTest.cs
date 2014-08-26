using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;
using EnvDTE;
using EnvDTE80;
using Microsoft.Practices.Common.Services;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VSSDK.Tools.VsIdeTesting;
using SPALM.SPSF.Library;


namespace SharePointSoftwareFactory.Tests
{
    /// <summary>
    /// Base class for initializing a test and cleaning up
    /// </summary>
    ///
    [TestClass]
    public class BaseTest
    {
        internal delegate void ThreadInvoker();

        internal string testDataRootDir = "";
        internal Microsoft.Practices.RecipeFramework.GuidancePackage coreRecipeGuidancePackage;
        internal string solutionFile = @"SolutionTemplates\SP2010.Hive\SP2010.Hive.sln";

        internal List<string> expectedCodeResults = new List<string>();
        internal List<string> expectedDeployResults = new List<string>();

        public BaseTest()
        {
            DirectoryInfo binDebugFolder = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            DirectoryInfo solutionDir = binDebugFolder.Parent.Parent.Parent;
            testDataRootDir = Path.Combine(solutionDir.FullName, "TestData");


        }

        public BaseTest(string solutionFile)
        {
            this.solutionFile = solutionFile;

            DirectoryInfo binDebugFolder = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            DirectoryInfo solutionDir = binDebugFolder.Parent.Parent.Parent;
            testDataRootDir = Path.Combine(solutionDir.FullName, "TestData");
        }

        public Microsoft.Practices.RecipeFramework.GuidancePackage GetGuidancePackage(string guidancePackageName)
        {
            //HKEY_USERS\S-1-5-21-1438320660-4206812534-2228856397-1000\Software\Microsoft\VisualStudio\11.0Exp_Config\Menus
            /*
            RegistryKey startupKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\11.0Exp\Menus\", false);
            object value = startupKey.GetValue(guidancePackageName);
            if (value == null)
            {
                throw new Exception("Guidance Package with name '" + guidancePackageName + "' not found in registry");
            }
            */

            string guidancePackageRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\VisualStudio\11.0Exp\Extensions\SPALM\SPSF\4.1\SPALM.SPSF.xml"); //value.ToString();
            string guidancePackageRootDir = Directory.GetParent(guidancePackageRoot).FullName;
            string guidancePackageXml = Path.Combine(guidancePackageRootDir, guidancePackageName + ".xml");

            if (!File.Exists(guidancePackageXml))
            {
                throw new FileNotFoundException("Guidance Package '" + guidancePackageXml + "' not found", guidancePackageXml);
            }

            return InitGuidancePackage(guidancePackageXml);
        }

        public Microsoft.Practices.RecipeFramework.GuidancePackage InitGuidancePackage(string packageXmlFilename)
        {
            string basePath = Directory.GetParent(packageXmlFilename).FullName + "\\";

            XmlReader configReader = XmlReader.Create(packageXmlFilename);
            //Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage curentConfig = Microsoft.Practices.RecipeFramework.GuidancePackage.ReadConfiguration(configReader);
            //Microsoft.Practices.RecipeFramework.GuidancePackage curentGPckg = new Microsoft.Practices.RecipeFramework.GuidancePackage(curentConfig, basePath);

            Microsoft.Practices.RecipeFramework.GuidancePackage curentGPckg = new Microsoft.Practices.RecipeFramework.GuidancePackage(configReader);

            System.IServiceProvider sp = VsIdeTestHostContext.ServiceProvider;

            TypeResolutionService _trs = new TypeResolutionService(basePath);
            TestValueGatheringService _tvs = new TestValueGatheringService();
            TestPersistenceService _tps = new TestPersistenceService();

            DTE test = GetDTE();
            testContextInstance.WriteLine("Mode: " + test.Mode);

            //curentGPckg.SetupTemporaryService(typeof(EnvDTE.DTE), GetDTE());

            //curentGPckg.AddService(typeof(ITypeResolutionService), _trs);
            curentGPckg.AddService(typeof(EnvDTE.DTE), GetDTE());
            curentGPckg.AddService(typeof(IValueGatheringService), _tvs);
            curentGPckg.AddService(typeof(IPersistenceService), _tps);


            //curentGPckg.AddService(typeof(Microsoft.VisualStudio.OLE.Interop.IOleComponentManager), sp.GetService(typeof(Microsoft.VisualStudio.OLE.Interop.IOleComponentManager)));

            curentGPckg.Site = new Microsoft.Practices.ComponentModel.Site(sp, new Microsoft.Practices.RecipeFramework.RecipeManager(), "SharePointSoftwareFactory");
            //curentGPckg.Site = (en)sp.GetService(typeof(ISite));
            configReader.Close();
            testContextInstance.WriteLine("Site: " + curentGPckg.Site.GetType().ToString());
            //testContextInstance.WriteLine("Container: " + curentGPckg.Site.Container.GetType().ToString());
            //testContextInstance.WriteLine("Component: " + curentGPckg.Site.Component.GetType().ToString());

            return curentGPckg;
        }

        private EnvDTE.DTE GetDTE()
        {
            IServiceProvider sp = VsIdeTestHostContext.ServiceProvider;
            return (EnvDTE.DTE)sp.GetService(typeof(EnvDTE.DTE));
        }

        [TestInitialize]
        public void TestInitialize()
        {
            string configFileWhereSolutionNameIsStored = testContextInstance.TestDir + "\\Out\\testSolution.txt";
            if (!File.Exists(configFileWhereSolutionNameIsStored))
            {
                throw new Exception("File not found " + configFileWhereSolutionNameIsStored);
            }
            string solutionName = File.ReadAllText(configFileWhereSolutionNameIsStored).Trim();
            solutionFile = @"SolutionTemplates\" + solutionName + @"\" + solutionName + ".sln";

            testContextInstance.WriteLine("Using config value " + solutionName);

            coreRecipeGuidancePackage = GetGuidancePackage("SPALM.SPSF");
            OpenSolution(solutionFile);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            TestContext.WriteLine(" ");
            TestContext.WriteLine("*** Expected code results");
            foreach (string s in this.expectedCodeResults)
            {
                TestContext.WriteLine(s);
            }

            TestContext.WriteLine(" ");
            TestContext.WriteLine("*** Expected deploy results");
            foreach (string s in this.expectedDeployResults)
            {
                TestContext.WriteLine(s);
            }

            bool hasErrors = false;
            string errorMessage = "";

            DTE dte = GetDTE();

            TestContext.WriteLine(" ");
            TestContext.WriteLine("*** Content of output window:");
            ReadOutput(dte, "SharePoint Software Factory");

            TestContext.WriteLine(" ");
            TestContext.WriteLine("*** Errors before build:");
            ReadErrorsAndWarnings(dte);
            TestContext.WriteLine("***");

            foreach (SolutionConfiguration2 config in dte.Solution.SolutionBuild.SolutionConfigurations)
            {
                if (config.Name == "Release")
                {
                    config.Activate();
                }
            }


            //save all
            foreach (Project p in Helpers.GetAllProjects(dte))
            {
                /*
                if (Helpers2.IsVSTemplate(p.DTE, p))
                {
                    TestContext.WriteLine("Building package file");
                    if (!Helpers2.BuildProject(p))
                    {
                        TestContext.WriteLine("Build failed " + p.Name);
                    }
                }
                */


                TestContext.WriteLine("Saving project " + p.Name);
                p.Save();

                ProjectItem packageItem = Helpers.FindProjectItemByPath(p, "Package\\package.package");
                if (packageItem != null)
                {
                    TestContext.WriteLine("Saving package file");
                    try
                    {
                        packageItem.Save();
                    }
                    catch { }
                }
            }

            //wait a second
            System.Threading.Thread.Sleep(2000);

            //build
            //Project project = Helpers.GetSelectedProject(dte);
            dte.Solution.SolutionBuild.Build(true);

            //wait a second
            System.Threading.Thread.Sleep(2000);

            //dte.Solution.SolutionBuild.BuildProject("Release", project.UniqueName, true);
            if (dte.Solution.SolutionBuild.LastBuildInfo != 0)
            {
                TestContext.WriteLine("Build of solution failed for " + dte.Solution.SolutionBuild.LastBuildInfo.ToString() + " projects");
                hasErrors = true;
                errorMessage += "Build failed for " + dte.Solution.SolutionBuild.LastBuildInfo.ToString() + " projects; ";
            }
            else
            {
                TestContext.WriteLine("Build of solution successfully");
            }

            TestContext.WriteLine(" ");
            TestContext.WriteLine("*** Errors after build:");
            //ReadOutput(dte, "Build");
            int numberOfErrors = ReadErrorsAndWarnings(dte);
            if (numberOfErrors != 0)
            {
                hasErrors = true;
                errorMessage += "Contains build " + numberOfErrors.ToString() + "warnings; ";
            }
            TestContext.WriteLine("***");
            TestContext.WriteLine(" ");

            //get all manifest.xmls and show them in the results
            foreach (Project p in Helpers.GetAllProjects(dte))
            {
                TestContext.WriteLine("Extracting project " + p.Name);
                try
                {
                    string pathToWSP = DeploymentHelpers.GetWSPFilePath(dte, p);
                    TestContext.WriteLine("- search wsp file " + pathToWSP);
                    if (File.Exists(pathToWSP))
                    {
                        TestContext.WriteLine("- wsp file found at " + pathToWSP);
                        string tempFolder = Path.Combine(Path.GetTempPath(), "SPSFTestExpand");
                        Directory.CreateDirectory(tempFolder);

                        // Launch the process and wait until it's done (with a 10 second timeout).
                        string commandarguments = "\"" + pathToWSP + "\" \"" + tempFolder + "\" -F:*";
                        ProcessStartInfo startInfo = new ProcessStartInfo("expand ", commandarguments);
                        startInfo.CreateNoWindow = true;
                        startInfo.UseShellExecute = false;
                        System.Diagnostics.Process snProcess = System.Diagnostics.Process.Start(startInfo);
                        snProcess.WaitForExit(20000);

                        string manifestFile = Path.Combine(tempFolder, "manifest.xml");
                        if (File.Exists(manifestFile))
                        {
                            TestContext.WriteLine("manifest of project " + p.Name);
                            TestContext.WriteLine(File.ReadAllText(manifestFile));
                        }
                        else
                        {
                            TestContext.WriteLine("- manifest not found at  " + manifestFile);
                        }

                        Directory.Delete(tempFolder, true);
                    }
                }
                catch (Exception ex)
                {
                    TestContext.WriteLine("Exception: " + ex.Message);
                }
            }

            string currentSolution = dte.Solution.FullName;


            dte.Solution.Close(true);

            if (coreRecipeGuidancePackage != null)
            {
                coreRecipeGuidancePackage.Dispose();
            }

            /*
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = @"C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe";
            process.StartInfo.Arguments = "/rootsuffix Exp " + currentSolution;
            process.Start();
            */

            if (hasErrors)
            {
                //open Visual Studio
                Assert.Fail(errorMessage);
            }
            //CloseVisualStudio();
        }

        private bool ReadOutput(DTE dte, string windowName)
        {
            Window win = dte.Windows.Item(EnvDTE.Constants.vsWindowKindOutput);
            OutputWindow comwin = (OutputWindow)win.Object;
            OutputWindowPane window = null;
            foreach (OutputWindowPane w in comwin.OutputWindowPanes)
            {
                if (w.Name == windowName)
                {
                    window = w;
                }
            }
            if (window != null)
            {
                window.TextDocument.Selection.SelectAll();
                string contents = window.TextDocument.Selection.Text;
                window.TextDocument.Selection.EndOfDocument();

                try
                {
                    TestContext.WriteLine(contents);
                }
                catch { }
            }
            return true;
        }

        private int ReadErrorsAndWarnings(DTE dte)
        {
            int numberOfErrors = 0;

            DTE2 dte2 = (DTE2)dte;
            dte2.ExecuteCommand("View.ErrorList", " ");


            EnvDTE80.ErrorList errors = dte2.ToolWindows.ErrorList;
            errors.Parent.Activate();
            errors.ShowWarnings = true;
            errors.ShowErrors = true;
            errors.ShowMessages = true;
            errors.Parent.Activate();

            dte2.ExecuteCommand("View.ErrorList", " ");

            for (int i = 1; i <= errors.ErrorItems.Count; i++)
            {
                try
                {
                    TestContext.WriteLine(errors.ErrorItems.Item(i).Description);

                    if (!IgnoreWarning(errors.ErrorItems.Item(i).Description.ToString()))
                    {
                        numberOfErrors++;
                        TestContext.WriteLine("BuildError: " + errors.ErrorItems.Item(i).Description.ToString() + " in " + errors.ErrorItems.Item(i).FileName);
                    }
                }
                catch { }
            }
            return numberOfErrors;
        }

        private bool IgnoreWarning(string p)
        {
            if (p.StartsWith("CA2109"))
            {
                //Consider making something private
                return true;
            }
            if (p.StartsWith("CA1704"))
            {
                //Consider renaming solution name
                return true;
            }             
            if (p.Contains(" exited with code 512"))
            {
                //general error that fxcop failed
                return true;
            }
            if (p.StartsWith("CA0060") || p.Contains("The indirectly-referenced assembly"))
            {
                //indirect referenced assembly could not be foundSA1210
                return true;
            }
            if (p.Contains("SA1210"))
            {
                //sorted using statements
                return true;
            }

            return false;
        }

        public TestContext testContextInstance;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        private void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it’s new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                //Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        internal void OpenSolution(string slnSourceFilename)
        {
            if (!Path.IsPathRooted(slnSourceFilename))
            {
                slnSourceFilename = Path.Combine(testDataRootDir, slnSourceFilename);
            }

            //copy the solution to the testdeploymentdir

            string targetDirName = testContextInstance.TestName + "_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            //string targetDirectory = Path.Combine(testContextInstance.TestDir, targetDirName);
            string targetDirectory = Path.Combine(@"C:\SPALMTests", targetDirName);
            if (Directory.Exists(targetDirectory))
            {
                Directory.Delete(targetDirectory, true);
            }
            DirectoryInfo targetDir = Directory.CreateDirectory(targetDirectory);
            DirectoryInfo sourceDir = Directory.GetParent(slnSourceFilename);
            string slnFilename = Path.Combine(targetDir.FullName, Path.GetFileName(slnSourceFilename));

            CopyAll(sourceDir, targetDir);

            testContextInstance.WriteLine("Opening " + slnFilename);
            GetDTE().Solution.Open(slnFilename);
        }

        public Hashtable GetIDEInstances(bool openSolutionsOnly)
        {
            Hashtable runningIDEInstances = new Hashtable();
            Hashtable runningObjects = GetRunningObjectTable();

            IDictionaryEnumerator rotEnumerator = runningObjects.GetEnumerator();
            while (rotEnumerator.MoveNext())
            {
                string candidateName = (string)rotEnumerator.Key;
                if (!candidateName.StartsWith("!VisualStudio.DTE"))
                    continue;

                _DTE ide = rotEnumerator.Value as _DTE;
                if (ide == null)
                    continue;

                if (openSolutionsOnly)
                {
                    try
                    {
                        string solutionFile = ide.Solution.FullName;
                        if (solutionFile != String.Empty)
                        {
                            runningIDEInstances[candidateName] = ide;
                        }
                    }
                    catch { }
                }
                else
                {
                    runningIDEInstances[candidateName] = ide;
                }
            }
            return runningIDEInstances;
        }

        [DllImport("ole32.dll")]
        public static extern int GetRunningObjectTable(int reserved,
                                  out UCOMIRunningObjectTable prot);

        [DllImport("ole32.dll")]
        public static extern int CreateBindCtx(int reserved,
                                      out UCOMIBindCtx ppbc);

        /// <summary>
        /// Get a snapshot of the running object table (ROT).
        /// </summary>
        /// <returns>A hashtable mapping the name of the object
        //     in the ROT to the corresponding object</returns>
        public Hashtable GetRunningObjectTable()
        {
            Hashtable result = new Hashtable();

            int numFetched;
            UCOMIRunningObjectTable runningObjectTable;
            UCOMIEnumMoniker monikerEnumerator;
            UCOMIMoniker[] monikers = new UCOMIMoniker[1];

            GetRunningObjectTable(0, out runningObjectTable);
            runningObjectTable.EnumRunning(out monikerEnumerator);
            monikerEnumerator.Reset();

            while (monikerEnumerator.Next(1, monikers, out numFetched) == 0)
            {
                UCOMIBindCtx ctx;
                CreateBindCtx(0, out ctx);

                string runningObjectName;
                monikers[0].GetDisplayName(ctx, null, out runningObjectName);

                object runningObjectVal;
                runningObjectTable.GetObject(monikers[0], out runningObjectVal);

                result[runningObjectName] = runningObjectVal;
            }

            return result;
        }


        internal void AddExpectedDeployResult(string p)
        {
            this.expectedDeployResults.Add(p);
        }
    }
}
