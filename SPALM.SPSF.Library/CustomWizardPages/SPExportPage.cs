using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Practices.WizardFramework;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using EnvDTE;
using SPALM.SPSF.SharePointBridge;

namespace SPALM.SPSF.Library.CustomWizardPages
{
    /// <summary>
    /// Example of a class that is a custom wizard page
    /// </summary>
    public partial class SPExportPage : CustomWizardPage
    {
      private SharePointExportSettings exportSettings = null;

      public SPExportPage(WizardForm parent)
            : base(parent)
      {        
        // This call is required by the Windows Form Designer.
        InitializeComponent();
      }

      public override void OnActivated()
      {
        base.OnActivated();

        //get the current exportsettings
        try
        {
          IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
					exportSettings = (SharePointExportSettings)dictionaryService.GetValue("ExportSettings");
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.ToString());
        }
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
					DisplayList(childList, node.Nodes);
				}
			}

			private void DisplayList(SharePointList childList, TreeNodeCollection treeNodeCollection)
			{
				TreeNode listnode = new TreeNode(childList.Title);
				listnode.Tag = childList;
				listnode.ImageKey = "SPList";
				listnode.SelectedImageKey = "SPList";
				treeNodeCollection.Add(listnode);

				foreach(SharePointItem childItem in childList.ChildItems)
				{
					DisplayItem(childItem, listnode.Nodes);
				}
			}

			private void DisplayItem(SharePointItem listitem, TreeNodeCollection treeNodeCollection)
			{
				TreeNode itemnode = new TreeNode(listitem.Title);
				itemnode.Tag = listitem;
				itemnode.ImageKey = "SPItem";
				itemnode.SelectedImageKey = "SPItem";
				treeNodeCollection.Add(itemnode);

				foreach(SharePointItem childItem in listitem.ChildItems)
				{
					DisplayItem(childItem, itemnode.Nodes);
				}
			}

      private void button1_Click(object sender, EventArgs e)
      {
        Cursor = Cursors.WaitCursor;
        treeView_nodes.Nodes.Clear();

				try
				{
					DTE dte = GetService(typeof(DTE)) as DTE;

					exportSettings.SiteUrl = comboBox1.Text;

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

      private void AddSelectedNodes(TreeNodeCollection nodes)
      {
        foreach (TreeNode tnode in nodes)
        {
          if (tnode.Checked)
          {
            Type t = tnode.Tag.GetType();
            if (tnode.Tag.GetType() == typeof(SharePointWeb))
            {
							SharePointExportObject exportObject = new SharePointExportObject();
							exportObject.Id = ((SharePointWeb)tnode.Tag).ID.ToString();
							exportObject.Url = ((SharePointWeb)tnode.Tag).Url;
							exportObject.Type = "Web"; // Microsoft.SharePoint.Deployment.SPDeploymentObjectType.Web;
              exportSettings.ExportObjects.Add(exportObject);
            }
            else if (tnode.Tag.GetType() == typeof(SharePointList))
            {

							SharePointExportObject exportObject = new SharePointExportObject();
							exportObject.Id = ((SharePointList)tnode.Tag).ID.ToString();
              exportObject.Type = "List"; // Microsoft.SharePoint.Deployment.SPDeploymentObjectType.List;
              exportSettings.ExportObjects.Add(exportObject);
            }
            else if (tnode.Tag.GetType() == typeof(SharePointItem))
            {
							SharePointExportObject exportObject = new SharePointExportObject();
							exportObject.Id = ((SharePointItem)tnode.Tag).ID.ToString();
              exportObject.Type = "ListItem"; // Microsoft.SharePoint.Deployment.SPDeploymentObjectType.ListItem;
              exportSettings.ExportObjects.Add(exportObject);
            }
          }

          AddSelectedNodes(tnode.Nodes);
        }
      }

      private void treeView_nodes_AfterSelect(object sender, TreeViewEventArgs e)
      {
        try
        {
          exportSettings.ExportObjects.Clear();
          AddSelectedNodes(treeView_nodes.Nodes);
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.ToString());
        }
      }
    }
}