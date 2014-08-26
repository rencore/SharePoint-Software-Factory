using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace SPALM.SPSF.Library
{
    public partial class SelectionForm : Form
    {
        List<NameValueItem> nvitems = null;
        private bool multipleItems = false;
        private bool groupItems = true;
        private object selection;
        public SelectionForm(List<NameValueItem> _nvitems, object _selection, bool grouped) 
        {
            InitializeComponent();

            nvitems = _nvitems;
            groupItems = grouped;
            selection = _selection;

            GenerateTree();
        }

        private void GenerateTree()
        {
            treeView.Nodes.Clear();

            //if in all items the groups are empty then grouping should be disabled
            CheckGrouped();

            if (selection is NameValueItem)
            {
                InitiallySelectedNameValueItem = (NameValueItem)selection;
            }
            else if (selection is NameValueItem[])
            {
                multipleItems = true;
                InitiallySelectedNameValueItems = (NameValueItem[])selection;
            }

            if (multipleItems)
            {
                treeView.CheckBoxes = true;
            }

            if (nvitems != null)
            {
                if (groupItems)
                {
                    //first grouping
                    foreach (NameValueItem nvitem in nvitems)
                    {
                        string key = nvitem.Group;
                        if ((key == null) || (key == ""))
                        {
                          key = "[nogroup]";
                        }
                        TreeNode groupnode = null;
                        if (!treeView.Nodes.ContainsKey(key))
                        {
                            //create new group
                            groupnode = new TreeNode();
                            groupnode.Text = key;
                            groupnode.Name = key;
                            //if(key.Equals(SPSFConstants.ThisSolutionNodeText, StringComparison.InvariantCultureIgnoreCase)){
                            //    groupnode.NodeFont = new Font(treeView.Font, FontStyle.Bold);
                            //}
                            treeView.Nodes.Add(groupnode);
                        }
                        else
                        {
                            groupnode = treeView.Nodes[key];
                        }
                        if (groupnode != null && 
                            !string.IsNullOrEmpty(nvitem.Name) && 
                            (!nvitem.Name.StartsWith("$Resources:",StringComparison.InvariantCultureIgnoreCase) || 
                             key.Equals(SPSFConstants.ThisSolutionNodeText, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            TreeNode tnode = new TreeNode();
                            tnode.Text = nvitem.Name;
                            tnode.Tag = nvitem;
                            tnode.ToolTipText = nvitem.Description;
                            groupnode.Nodes.Add(tnode);

                            if (InitiallySelectedNameValueItem != null)
                            {
                                if (nvitem.Value == InitiallySelectedNameValueItem.Value)
                                {
                                    groupnode.Expand();
                                }
                            }
                            if (InitiallySelectedNameValueItems != null)
                            {
                                foreach (NameValueItem nvi in InitiallySelectedNameValueItems)
                                {
                                    if (nvi.Value == nvitem.Value)
                                    {
                                        tnode.Checked = true;
                                        groupnode.Expand();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (NameValueItem nvitem in nvitems)
                    {
                        //gibt es das item schon auf 1. ebene?
                      if (!NodeExists(nvitem) &&
                          !string.IsNullOrEmpty(nvitem.Name) )
                          /*&& 
                          (!nvitem.Name.StartsWith("$Resources:",StringComparison.InvariantCultureIgnoreCase) || 
                             key.Equals(SPSFConstants.ThisSolutionNodeText, StringComparison.InvariantCultureIgnoreCase)))*/
                      {
                        TreeNode tnode = new TreeNode();
                        tnode.Text = nvitem.Name;
                        tnode.Tag = nvitem;
                        tnode.ToolTipText = nvitem.Description;
                        treeView.Nodes.Add(tnode);
                      }
                    }
                }
            }
            treeView.Sort();
            ValidateButtons();
        }

        private void SearchTree()
        {          
          string searchstring = this.textBox1.Text;
          if(searchstring != "")
          {          
            treeView.Nodes.Clear();
            searchstring = searchstring.ToUpper();
            foreach (NameValueItem nvitem in nvitems)
            { 
              bool additem = false;
              if (nvitem.Name != null)
              {
                if (nvitem.Name.ToUpper().Contains(searchstring))
                {
                  additem = true;
                }
              }
              else if (nvitem.Value != null)
              {
                if (nvitem.Value.ToUpper().Contains(searchstring))
                {
                  additem = true;
                }
              }
              if (additem)
              {
                //gibt es das item schon auf 1. ebene?
                if (!NodeExists(nvitem))
                {
                  TreeNode tnode = new TreeNode();
                  tnode.Text = nvitem.Name;
                  tnode.Tag = nvitem;
                  tnode.ToolTipText = nvitem.Description;
                  treeView.Nodes.Add(tnode);
                }
              }
            }
          }
          treeView.Sort();
        }

        private bool NodeExists(NameValueItem nvitem)
        {
          foreach (TreeNode node in treeView.Nodes)
          {
            if (node.Tag != null)            
            {
              if ((((NameValueItem)node.Tag).Name == nvitem.Name) && (((NameValueItem)node.Tag).Value == nvitem.Value))
              {
                return true;
              }
            }
          }
          return false;
        }

        private void CheckGrouped()
        {
          bool nonEmptyGroupFound = false;
          foreach (NameValueItem nvitem in nvitems)
          {
            if (nvitem.Group.Trim() != "")
            {
              nonEmptyGroupFound = true;
            }
          }
          if (!nonEmptyGroupFound)
          {
            groupItems = false;
          }
        }

        public NameValueItem InitiallySelectedNameValueItem = null;
        public NameValueItem[] InitiallySelectedNameValueItems = null;
        public NameValueItem SelectedNameValueItem = null;
        public NameValueItem[] SelectedNameValueItems = new NameValueItem[0];
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView.SelectedNode != null)
            {
              if (treeView.SelectedNode.Tag != null)
              {
                NameValueItem item = (NameValueItem)treeView.SelectedNode.Tag;
                selectedValue.Text = item.Value;
                selectedName.Text = item.Name;
                selectedDescription.Text = item.Description;
                selectedGroup.Text = item.Group;
              }
              else
              {
                selectedValue.Text = "";
                selectedName.Text = "";
                selectedDescription.Text = "";
                selectedGroup.Text = "";
              }
            }
            ValidateButtons();
        }

        private void FindSelectedNodes(TreeNodeCollection nodes, ArrayList items)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Checked)
                {
                    if (node.Tag != null)
                    {
                        if (node.Tag is NameValueItem)
                        {
                            items.Add(node.Tag);
                        }
                    }
                }
                FindSelectedNodes(node.Nodes, items);
            }
        }

        private void ValidateButtons()
        {
          buton_ok.Enabled = false;
          if ((treeView.SelectedNode != null) && (treeView.SelectedNode.Tag != null))
          {
            buton_ok.Enabled = true;
          }
          else if (treeView.CheckBoxes)
          {
            ArrayList items = new ArrayList();
            FindSelectedNodes(treeView.Nodes, items);
            if (items.Count > 0)
            {
              buton_ok.Enabled = true;
            }
          }
        }

        private void button1_Click(object sender, EventArgs e)
        {
          CloseForm();
        }

      private void CloseForm()
      {
            if (multipleItems)
            {
                ArrayList items = new ArrayList();
                FindSelectedNodes(treeView.Nodes, items);

                SelectedNameValueItems = new NameValueItem[items.Count];
                for (int i = 0; i < items.Count; i++)
                {
                    SelectedNameValueItems[i] = (NameValueItem)items[i];
                }
            }
            else
            {
                if (treeView.SelectedNode != null)
                {
                  if (treeView.SelectedNode.Tag != null)
                  {
                    SelectedNameValueItem = (NameValueItem)treeView.SelectedNode.Tag;
                    selectedValue.Text = SelectedNameValueItem.Value;
                    selectedName.Text = SelectedNameValueItem.Name;
                    selectedDescription.Text = SelectedNameValueItem.Description;
                    selectedGroup.Text = SelectedNameValueItem.Group;
                  }
                  else
                  {
                    selectedValue.Text = "";
                    selectedName.Text = "";
                    selectedDescription.Text = "";
                    selectedGroup.Text = "";
                  }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
          SearchTree();
          ValidateButtons();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
          textBox1.Text = "";
          GenerateTree();
          ValidateButtons();
        }

        private void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                if (e.Node.Nodes.Count > 0)
                {
                    foreach(TreeNode childNode in e.Node.Nodes)
                    {
                        childNode.Checked = e.Node.Checked;
                    }
                }
            }
          ValidateButtons();
        }

        private void treeView_DoubleClick(object sender, EventArgs e)
        {
          if (buton_ok.Enabled)
          {
            CloseForm();
          }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
          if (e.KeyCode == Keys.Enter)
          {
            SearchTree();
            ValidateButtons();
          }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
