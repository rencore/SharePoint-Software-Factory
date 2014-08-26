using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Practices.WizardFramework;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Xml;
using System.Collections.Generic;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.ComponentModel;
using System.Reflection;
using System.IO;

namespace SPALM.SPSF.Library.CustomWizardPages
{
    /// <summary>
    /// Example of a class that is a custom wizard page
    /// </summary>
    /// 
    [ServiceDependency(typeof(IConfigurationService))]
    public partial class AboutPage : SPSFBaseWizardPage
    {
        public AboutPage(WizardForm parent)
            : base(parent)
        {
            InitializeComponent();
        }

        public override bool OnActivate()
        {
            base.OnActivate();
            AddBrandingPanel();
            HasBeenActivated = true;
            this.Cancellable = false;
            this.ShowInfoPanel = false;

            //get title, description and version
            string path = Path.Combine(GetBasePath(), "SPALM.SPSF.xml");
            if (File.Exists(path))
            {
                XmlDocument guidanceDoc = new XmlDocument();
                guidanceDoc.Load(path);

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(guidanceDoc.NameTable);
                nsmgr.AddNamespace("ns", "http://schemas.microsoft.com/pag/gax-core");
                nsmgr.AddNamespace("spsf", "http://spsf.codeplex.com");
                nsmgr.AddNamespace("wiz", "http://schemas.microsoft.com/pag/gax-wizards");

                XmlNode rootNode = guidanceDoc.SelectSingleNode("/ns:GuidancePackage", nsmgr);
                if (rootNode != null)
                {
                    label_title.Text = rootNode.Attributes["Caption"].Value.ToString();
                    label_description.Text = rootNode.Attributes["Description"].Value.ToString();
                    label_version.Text = rootNode.Attributes["Version"].Value.ToString();
                }
            }

            //get fileversion of assembly
            label_fileversion.Text = System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            
            //TODO: get contributors from Authors.xml
            return true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }   
}