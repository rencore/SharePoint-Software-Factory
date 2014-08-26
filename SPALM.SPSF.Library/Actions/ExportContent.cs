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

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// Exports a given
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class ExportContent : ConfigurableAction
    {
      private Project _TargetProject = null;
      private string _TargetFolder = "";
      private string _ExportObjects = "";
      private string _CabFilename = "";
      private string _IncludeSecurity = "";
      private string _IncludeVersions = "";
      private bool _ExcludeDependencies = false;

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
        public string ExportObjects
        {
          get { return _ExportObjects; }
          set { _ExportObjects = value; }
        }

        [Input(Required = true)]
        public string CabFilename
        {
          get { return _CabFilename; }
          set { _CabFilename = value; }
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

      
        public override void Execute()
        {
          //deserialize ExportObjects
          VirtualExportObject[] objects = null;

          XmlSerializer ser = new XmlSerializer(typeof(VirtualExportObject[]));
          StringReader stringReader = new StringReader(_ExportObjects);
          XmlTextReader xmlReader = new XmlTextReader(stringReader);
          object obj = ser.Deserialize(xmlReader);
          xmlReader.Close();
          stringReader.Close();
          objects = (VirtualExportObject[])obj;

          /*
            DTE dte = GetService<DTE>(true);
            SharePointSoftwareFactory.Base.Helpers.WriteToOutputWindow(dte, "Starting export");

            if(TargetProject != null)
            {
          
   
            SPExportSettings exportsettings = new SPExportSettings();
            //exportsettings.SiteUrl = "http://";
            exportsettings.BaseFileName = cabfilename;
            exportsettings.FileLocation = exportfolder;
            exportsettings.CommandLineVerbose = true;
            exportsettings.IncludeSecurity = SPIncludeSecurity.All;
            exportsettings.IncludeVersions = SPIncludeVersions.All;
            exportsettings.ExcludeDependencies = false;
            exportsettings.ExportMethod = SPExportMethodType.ExportAll;
            exportsettings.FileCompression = true;
            exportsettings.OverwriteExistingDataFile = true;
            exportsettings.LogExportObjectsTable = true;
            exportsettings.LogFilePath = exportlogfile;
        */
        }

      /*
        void snProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (OWP != null)
            {
                OWP.OutputString(e.Data + "\n");
            }
        }

        void snProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (OWP != null)
            {
                OWP.OutputString(e.Data + "\n");
            }
        }
      */
        public void RunSkript()
        {
        }

        /// <summary>
        /// Removes the previously added reference, if it was created
        /// </summary>
        public override void Undo()
        {
        }
    }    
}