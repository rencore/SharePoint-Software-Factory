using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Practices.WizardFramework;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Xml;
using EnvDTE;
using SPALM.SPSF.SharePointBridge;
using System.Collections.Generic;

namespace SPALM.SPSF.Library.CustomWizardPages
{
    /// <summary>
    /// Example of a class that is a custom wizard page
    /// </summary>
    public partial class LINQImportPage : SPSFBaseWizardPage
    {
        public LINQImportPage(WizardForm parent)
            : base(parent)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
        }

        public override bool OnActivate()
        {
            base.OnActivate();
            AddBrandingPanel();
            HasBeenActivated = true;

            IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
            if (dictionaryService.GetValue("LINQSourceWeb") != null)
            {
                textBox1.Text = dictionaryService.GetValue("LINQSourceWeb").ToString();
            }

            return true;
        }

        public override bool IsDataValid
        {
            get
            {
                try
                {
                    IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
                    if (dictionaryService.GetValue("LINQSourceWeb") == null)
                    {
                        return false;
                    }
                    if ((dictionaryService.GetValue("LINQIncludedLists") as NameValueItem[]).Length == 0)
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
            Cursor = Cursors.WaitCursor;

            try
            {
                DTE dte = GetService(typeof(DTE)) as DTE;

                SharePointBrigdeHelper helper = new SharePointBrigdeHelper(dte);
                SharePointWeb rootweb = helper.GetWeb(textBox1.Text);

                treeView1.Nodes.Clear();
                foreach (SharePointList childList in rootweb.Lists)
                {
                    TreeNode listnode = new TreeNode();
                    listnode.Text = childList.Title + " (List)";
                    listnode.Name = childList.Title;
                    listnode.ImageKey = "SPList";
                    listnode.SelectedImageKey = "SPList";
                    treeView1.Nodes.Add(listnode);

                    foreach (SharePointContentType ct in childList.ContentTypes)
                    {
                        TreeNode ctnode = new TreeNode();
                        ctnode.Text = ct.Name + " (ContentType)";
                        ctnode.Name = ct.Name;
                        ctnode.ImageKey = "SPContentType";
                        ctnode.SelectedImageKey = "SPContentType";
                        listnode.Nodes.Add(ctnode);

                        foreach (SharePointField field in ct.Fields)
                        {
                            TreeNode fieldNode = new TreeNode();
                            fieldNode.Text = field.Name + " (Field)";
                            fieldNode.Name = field.Name;
                            fieldNode.ImageKey = "SPField";
                            fieldNode.SelectedImageKey = "SPField";
                            ctnode.Nodes.Add(fieldNode);
                        }
                        /*
                        listboxLists.Items.Add(childList.Title, false);
                         * */
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            Cursor = Cursors.Default;
        }

        private void SetAsInvalid()
        {
            try
            {
                IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
                dictionaryService.SetValue("LINQIncludedLists", null);
            }
            catch (Exception)
            {
            }
        }

        private void UpdateExcludedLists()
        {
            //LINQIncludedLists
            SetAsInvalid();

            List<NameValueItem> includedLists = new List<NameValueItem>();
            RebuildHierachy(treeView1.Nodes, includedLists);

            IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
            dictionaryService.SetValue("LINQIncludedLists", includedLists.ToArray());
        }

        private void RebuildHierachy(TreeNodeCollection treeNodeCollection, List<NameValueItem> includedLists)
        {
            foreach (TreeNode node in treeNodeCollection)
            {
                if (node.Checked)
                {
                    NameValueItem nvitem = new NameValueItem("node", node.Name, node.Name);
                    RebuildHierachy(node.Nodes, nvitem.Childs);
                    includedLists.Add(nvitem);
                }
            }
        }
 
        private void listboxLists_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            UpdateExcludedLists();
            Wizard.OnValidationStateChanged(this);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
            dictionaryService.SetValue("LINQSourceWeb", textBox1.Text);

            if (textBox1.Text != "")
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //update all childs with same status
            UpdateChildNodes(e.Node);

            UpdateExcludedLists();
            Wizard.OnValidationStateChanged(this);
        }

        private void UpdateChildNodes(TreeNode treeNode)
        {
            foreach (TreeNode childNode in treeNode.Nodes)
            {
                childNode.Checked = treeNode.Checked;
                UpdateChildNodes(childNode);
            }
        }
    }
}