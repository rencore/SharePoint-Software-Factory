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

namespace SPALM.SPSF.Library.CustomWizardPages
{
    /// <summary>
    /// Example of a class that is a custom wizard page
    /// </summary>
    public partial class ListTemplateImportPage : CustomWizardPage
    {
        public ListTemplateImportPage(WizardForm parent)
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
                    if (dictionaryService.GetValue("ListTemplateType") == null)
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
            treeView_nodes.Nodes.Clear();

            try
            {
                DTE dte = GetService(typeof(DTE)) as DTE;

                SharePointBrigdeHelper helper = new SharePointBrigdeHelper(dte);
                SharePointWeb rootweb = helper.GetRootWebOfSite(comboBox1.Text);

                DisplayWeb(rootweb, treeView_nodes.Nodes);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            Cursor = Cursors.Default;
        }

        private void DisplayWeb(SharePointWeb rootweb, TreeNodeCollection treeNodeCollection)
        {
            TreeNode node = new TreeNode(rootweb.Title);
            node.Tag = rootweb;
            node.ImageKey = "SPWeb";
            node.SelectedImageKey = "SPWeb";
            treeNodeCollection.Add(node);

            foreach (SharePointWeb childWeb in rootweb.ChildWebs)
            {
                DisplayWeb(childWeb, node.Nodes);
            }

            foreach (SharePointList childList in rootweb.Lists)
            {
                TreeNode listnode = new TreeNode(childList.Title);
                listnode.Tag = childList;
                listnode.ImageKey = "SPList";
                listnode.SelectedImageKey = "SPList";
                node.Nodes.Add(listnode);
            }
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
            //erstmal ListTemplateType auf leer setzen, damit der Dialog als NotValid angezeigt wird.
            SetAsInvalid();

            if (e.Node != null)
            {
                if (e.Node.Tag != null)
                {
                    if (e.Node.Tag is SharePointList)
                    {
                        try
                        {
                            IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;

                            SharePointList list = (SharePointList)e.Node.Tag;

                            dictionaryService.SetValue("SiteUrl", list.Url);
                            dictionaryService.SetValue("ListId", list.ID);
                            dictionaryService.SetValue("ListTemplateFeatureId", list.TemplateFeatureId.ToString());

                            dictionaryService.SetValue("ListTemplateDisplayName", list.Title);
                            dictionaryService.SetValue("ListTemplateDescription", list.Description);


                            dictionaryService.SetValue("ListTemplateType", list.BaseTemplate);
                            dictionaryService.SetValue("ListTemplateBaseType", list.BaseType);
                            dictionaryService.SetValue("ListTemplateName", list.Title.Replace(" ", "").ToLower());
                            //dictionaryService.SetValue("ListTemplateType", ((int)list.BaseTemplate).ToString());
                            //dictionaryService.SetValue("ListTemplateCategory", list.templ);
                            //dictionaryService.SetValue("ListTemplateSequence", list.Id);

                            dictionaryService.SetValue("ListTemplateOnQuickLaunch", list.OnQuickLaunch);
                            dictionaryService.SetValue("ListTemplateDisableAttachments", list.EnableAttachments);
                            dictionaryService.SetValue("ListTemplateDisallowContentTypes", list.AllowContentTypes);
                            dictionaryService.SetValue("ListTemplateVersioningEnabled", list.EnableVersioning);
                            dictionaryService.SetValue("ListTemplateFolderCreation", list.EnableFolderCreation);
                            dictionaryService.SetValue("ListTemplateEnableModeration", list.EnableModeration);
                            dictionaryService.SetValue("ListTemplateHiddenList", list.Hidden);

                            dictionaryService.SetValue("ListTemplateSecurityBitsRead", list.ReadSecurity);
                            dictionaryService.SetValue("ListTemplateSecurityBitsEdit", list.WriteSecurity);
                            dictionaryService.SetValue("ListTemplateImage", list.ImageUrl);

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
                dictionaryService.SetValue("ListTemplateType", null);
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