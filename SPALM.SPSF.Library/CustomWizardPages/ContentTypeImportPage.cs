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
    public partial class ContentTypeImportPage : CustomWizardPage
    {
        public ContentTypeImportPage(WizardForm parent)
            : base(parent)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            treeView_nodes.Nodes.Clear();
            if (comboBox1.Text != "")
            {
                List<SharePointContentType> items = LoadContentTypes(comboBox1.Text);
                foreach (SharePointContentType item in items)
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

        public override bool IsDataValid
        {
            get
            {
                try
                {
                    IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
                    if (dictionaryService.GetValue("ContentTypeName") == null)
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

        private List<SharePointContentType> LoadContentTypes(string siteurl)
        {
            DTE dte = GetService(typeof(DTE)) as DTE;

            Cursor = Cursors.WaitCursor;

            SharePointBrigdeHelper helper = new SharePointBrigdeHelper(dte);
            List<SharePointContentType> result = helper.GetContentTypes(siteurl);

            Cursor = Cursors.Default;
            return result;
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
                MessageBox.Show(ex.Message);
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

        private void SetAsInvalid()
        {
            try
            {
                IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
                dictionaryService.SetValue("ContentTypeName", null);
            }
            catch (Exception)
            {
            }
        }

        private void treeView_nodes_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SetAsInvalid();
            if (e.Node != null)
            {
                if (e.Node.Tag != null)
                {
                    if (e.Node.Tag is SharePointContentType)
                    {
                        try
                        {
                            IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
                            SharePointContentType vct = (SharePointContentType)e.Node.Tag;
                            dictionaryService.SetValue("ContentTypeID", vct.Id);
                            dictionaryService.SetValue("ContentTypeName", vct.Name);
                            dictionaryService.SetValue("ContentTypeDescription", vct.Description);
                            dictionaryService.SetValue("ContentTypeGroup", vct.Group);
                            dictionaryService.SetValue("ContentTypeFieldSchema", vct.FieldSchema);
                            dictionaryService.SetValue("ContentTypeVersion", vct.Version);
                            dictionaryService.SetValue("ContentTypeHidden", vct.Hidden);
                            dictionaryService.SetValue("ContentTypeReadOnly", vct.ReadOnly);
                            dictionaryService.SetValue("ContentTypeSealed", vct.Sealed);
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