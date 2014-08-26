using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using SPALM.SPSF.Library;
using Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates;
using System.IO;
using System.CodeDom.Compiler;
using System.Xml;

namespace SPALM.SPSF.Test.BCSTester
{
    public partial class Form1 : Form
    {
        private BCSModel bcsModel;

        public Form1()
        {
            InitializeComponent();

            bcsTreeControl1.BCSModelChanged += new BCSTreeControl.BCSModelChangedHandler(bcsTreeControl1_BCSModelChanged);
            //bcsTreeControl1.IsDesignMode = true;

            //Class1 test = new Class1();
        }

        void bcsTreeControl1_BCSModelChanged(BCSModel changedModel)
        {
            bcsModel = changedModel;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GenerateTemplate();
        }

        private void GenerateTemplate()
        {
            tabControl1.TabPages.Clear();

            Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngine engine = new Microsoft.VisualStudio.TextTemplating.Engine();
            Dictionary<string, PropertyData> arguments = new Dictionary<string, PropertyData>();

            PropertyData data = new PropertyData(bcsModel, typeof(BCSModel));
            arguments.Add("BCSModel", data);

            arguments.Add("ProjectNamespace", new PropertyData("BBS.Project1", typeof(String)));
            arguments.Add("BCSModelName", new PropertyData("Model1", typeof(String)));
            arguments.Add("BCSDatabase", new PropertyData("BCSTest", typeof(String)));
            arguments.Add("BCSModelDisplayName", new PropertyData("Model 1", typeof(String)));
            arguments.Add("BCSModelIsCached", new PropertyData(false, typeof(Boolean)));
            arguments.Add("BCSPermissions", new PropertyData("contoso\\Administrator", typeof(String)));
            arguments.Add("BCSServer", new PropertyData("demo2010a", typeof(String)));
            arguments.Add("BCSModelVersion", new PropertyData("1.0.0.0", typeof(String)));
            arguments.Add("BCSEstimatedCount", new PropertyData("10000", typeof(String)));
            arguments.Add("ProjectAssemblyName", new PropertyData("BCS.Test.Model", typeof(String)));
            arguments.Add("GeneratedFileName", new PropertyData("OutputFileName", typeof(String)));
            arguments.Add("CopyrightCompanyName", new PropertyData("Company", typeof(String)));

            arguments.Add("BCSType", new PropertyData("Wcf", typeof(String)));

            foreach (string templateFile in checkedListBox1.CheckedItems)
            {
                tabControl1.TabPages.Add(templateFile, templateFile);
                TextBox txtbox = new TextBox();
                txtbox.Multiline = true;
                txtbox.ScrollBars = ScrollBars.Both;
                txtbox.Dock = DockStyle.Fill;
                txtbox.WordWrap = false;
                tabControl1.TabPages[templateFile].Controls.Add(txtbox);

                string templateCode = File.ReadAllText(@"F:\TFS\SPSF\Dev\Source\SPALM.SPSF\Templates\Text\BCS\" + templateFile);
                StringBuilder templateCodeLines = new StringBuilder();
                StringReader reader = new StringReader(templateCode);
                string line = "";
                bool headerinserted = false;
                while ((line = reader.ReadLine()) != null)
                {
                    templateCodeLines.AppendLine(line);

                    if (!headerinserted)
                    {
                        headerinserted = true;
                        templateCodeLines.AppendLine("<#@ assembly name=\"System.dll\" #>");
                        templateCodeLines.AppendLine("<#@ assembly name=\"SPALM.SPSF.Library.dll\" #>");
                        templateCodeLines.AppendLine("<#@ import namespace=\"SPALM.SPSF.Library\" #>");
                        foreach (string argument in arguments.Keys)
                        {
                            templateCodeLines.AppendLine("<#@ property processor=\"PropertyProcessor\" name=\"" + argument + "\" #>");
                        }
                    }
                }

                templateCode = templateCodeLines.ToString();

                TemplateHost host = new TemplateHost(@"F:\TFS\SPSF\Dev\Source\SPALM.SPSF\", arguments);
                host.TemplateFile = @"Templates\Text\BCS\BCSModel_SQL.bdcm.t4";
                string str3 = engine.ProcessTemplate(templateCode, host);

                foreach (CompilerError error in host.Errors)
                {
                    txtbox.Text += error.ErrorText + "(" + error.Line + ")";
                }

                if (!host.Errors.HasErrors)
                {
                    if (str3.Contains("<?xml"))
                    {
                        XmlDocument result = new XmlDocument();
                        result.LoadXml(str3);

                        XmlWriterSettings wSettings = new XmlWriterSettings();
                        wSettings.Indent = true;
                        wSettings.NewLineOnAttributes = false;
                        wSettings.IndentChars = "\t";

                        string path = Path.GetTempFileName();

                        XmlWriter xw = XmlWriter.Create(path, wSettings);
                        result.Save(xw);
                        xw.Flush();
                        xw.Close();

                        txtbox.Text += File.ReadAllText(path);
                    }
                    else
                    {
                        txtbox.Text += str3;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bcsTreeControl1.LoadData("demo2010a", "BCSTest");
        }
    }
}
