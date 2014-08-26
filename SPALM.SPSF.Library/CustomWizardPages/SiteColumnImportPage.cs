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
using SPALM.SPSF.SharePointBridge;

namespace SPALM.SPSF.Library.CustomWizardPages
{
    /// <summary>
    /// Example of a class that is a custom wizard page
    /// </summary>
    public partial class SiteColumnImportPage : CustomWizardPage
    {
        public SiteColumnImportPage(WizardForm parent)
            : base(parent)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
        }

        public override bool IsDataValid
        {
            get
            {
                try
                {
                    IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
                    if (dictionaryService.GetValue("SiteColumnSchema") == null)
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                }
                return base.IsDataValid;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            treeView_nodes.Nodes.Clear();
            if (comboBox1.Text != "")
            {
                foreach (SharePointField item in LoadSiteColumns(comboBox1.Text))
                {
                    string key = item.Group;
                    if ((key == null) || (key == ""))
                    {
                        key = "[nogroup]";
                    }

                    TreeNode groupnode = null;
                    if (!treeView_nodes.Nodes.ContainsKey(key))
                    {
                        //create new group
                        groupnode = new TreeNode();
                        groupnode.Text = key;
                        groupnode.Name = key;
                        treeView_nodes.Nodes.Add(groupnode);
                    }
                    else
                    {
                        groupnode = treeView_nodes.Nodes[key];
                    }

                    TreeNode node = new TreeNode(item.Name);
                    node.Tag = item;
                    groupnode.Nodes.Add(node);
                }
            }
        }

        private List<SharePointField> LoadSiteColumns(string siteurl)
        {
            Cursor = Cursors.WaitCursor;

            DTE dte = GetService(typeof(DTE)) as DTE;
            SharePointBrigdeHelper helper = new SharePointBrigdeHelper(dte);
            List<SharePointField> siteColumns = helper.GetSiteColumns(siteurl);

            Cursor = Cursors.Default;

            return siteColumns;
        }

        private void GetAllSiteCollections()
        {
            Cursor = Cursors.WaitCursor;
            this.comboBox1.Items.Clear();

            try
            {
                DTE dte = GetService(typeof(DTE)) as DTE;

                SharePointBrigdeHelper helper = new SharePointBrigdeHelper(dte);
                foreach (SharePointSiteCollection sitecoll in helper.GetAllSiteCollections())
                {
                    comboBox1.Items.Add(sitecoll.Url);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            Cursor = Cursors.Default;
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count == 0)
            {
                GetAllSiteCollections();
            }
        }

        private void treeView_nodes_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SetAsInvalid();

            if (e.Node != null)
            {
                if (e.Node.Tag != null)
                {
                    if (e.Node.Tag is SharePointField)
                    {
                        try
                        {
                            IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
                            SharePointField vct = (SharePointField)e.Node.Tag;

                            dictionaryService.SetValue("SiteColumnID", vct.Id.ToString());
                            dictionaryService.SetValue("SiteColumnName", vct.Name);
                            dictionaryService.SetValue("SiteColumnDisplayName", vct.DisplayName);
                            dictionaryService.SetValue("SiteColumnDescription", vct.Description);
                            dictionaryService.SetValue("SiteColumnGroup", vct.Group);
                            dictionaryService.SetValue("SiteColumnSchema", vct.SchemaXml);

                            /*
                             * 
                             * 
                             *     XmlElement dummyNode = xmlDocument.CreateElement("dummy");
                dummyNode.InnerXml = virtualField.OriginalField.SchemaXml;
                XmlElement fieldNode = (XmlElement) dummyNode.LastChild;
                fieldNode.SetAttribute("xmlns", "http://schemas.microsoft.com/sharepoint/");
                if (virtualField.Name != string.Empty)
                {
                    fieldNode.SetAttribute("Name", virtualField.Name);
                }
                else
                {
                    if (virtualField.StaticName == string.Empty)
                    {
                        throw new ApplicationException("No Name or StaticName for VirtualField: " + virtualField.OriginalField.SchemaXml);
                    }
                    fieldNode.SetAttribute("Name", virtualField.StaticName);
                }
                if (virtualField.DisplayName != string.Empty)
                {
                    fieldNode.SetAttribute("DisplayName", virtualField.DisplayName);
                }
                if (virtualField.StaticName != string.Empty)
                {
                    fieldNode.SetAttribute("StaticName", virtualField.StaticName);
                }
                if (virtualField.Group != string.Empty)
                {
                    fieldNode.SetAttribute("Group", virtualField.Group);
                }
                fieldNode.SetAttribute("ID", virtualField.Id.ToString("B"));
                if (virtualField.MaxLength.HasValue)
                {
                    fieldNode.SetAttribute("MaxLength", virtualField.MaxLength.ToString());
                }
                if (virtualField.SourceID != string.Empty)
                {
                    fieldNode.SetAttribute("SourceID", virtualField.SourceID);
                }
                fieldNode.RemoveAttribute("WebId");
                fieldNode.RemoveAttribute("Version");
                fieldNode.RemoveAttribute("UserSelectionMode");
                fieldNode.RemoveAttribute("UserSelectionScope");
                return CleanFeatureXml(dummyNode.InnerXml);

                             * 
                             * */
                            //vct.OriginalField.SchemaXml
                            /*
                            string fieldschema = "<FieldRefs>";
                            foreach (SPFieldLink fieldLink in vct.OriginalContentType.FieldLinks)
                            {
                              fieldschema = fieldschema + fieldLink.SchemaXml;
                            }
                            fieldschema += "</FieldRefs>";

                            dictionaryService.SetValue("ContentTypeID", vct.Id);
                            dictionaryService.SetValue("ContentTypeName", vct.Name);
                            dictionaryService.SetValue("ContentTypeDescription", vct.Description);
                            dictionaryService.SetValue("ContentTypeGroup", vct.Group);
                            dictionaryService.SetValue("ContentTypeFieldSchema", fieldschema);
                            dictionaryService.SetValue("ContentTypeVersion", vct.Version);
                            dictionaryService.SetValue("ContentTypeHidden", vct.Hidden);
                            dictionaryService.SetValue("ContentTypeReadOnly", vct.ReadOnly);
                            dictionaryService.SetValue("ContentTypeSealed", vct.Sealed);
                          */
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }
            }

            Wizard.OnValidationStateChanged(this);
        }

        private void SetAsInvalid()
        {
            try
            {
                IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
                dictionaryService.SetValue("SiteColumnSchema", null);
            }
            catch (Exception)
            {
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }
    }


}