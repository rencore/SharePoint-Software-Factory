using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Practices.WizardFramework;

namespace SPALM.SPSF.Library.CustomWizardPages
{
  partial class SPExportPage : CustomWizardPage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
					this.components = new System.ComponentModel.Container();
					System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPExportPage));
					this.panel1 = new System.Windows.Forms.Panel();
					this.panel2 = new System.Windows.Forms.Panel();
					this.treeView_nodes = new System.Windows.Forms.TreeView();
					this.imageList_SharePointIcons = new System.Windows.Forms.ImageList(this.components);
					this.button1 = new System.Windows.Forms.Button();
					this.label2 = new System.Windows.Forms.Label();
					this.comboBox1 = new System.Windows.Forms.ComboBox();
					this.label1 = new System.Windows.Forms.Label();
					this.infoPanel.SuspendLayout();
					this.panel2.SuspendLayout();
					((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
					this.SuspendLayout();
					// 
					// infoPanel
					// 
					this.infoPanel.Controls.Add(this.panel1);
					this.infoPanel.Location = new System.Drawing.Point(0, 41);
					this.infoPanel.Size = new System.Drawing.Size(503, 309);
					this.infoPanel.Controls.SetChildIndex(this.panel1, 0);
					// 
					// panel1
					// 
					this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
					this.panel1.Location = new System.Drawing.Point(0, 0);
					this.panel1.Name = "panel1";
					this.panel1.Size = new System.Drawing.Size(503, 205);
					this.panel1.TabIndex = 9;
					// 
					// panel2
					// 
					this.panel2.Controls.Add(this.treeView_nodes);
					this.panel2.Controls.Add(this.button1);
					this.panel2.Controls.Add(this.label2);
					this.panel2.Controls.Add(this.comboBox1);
					this.panel2.Controls.Add(this.label1);
					this.panel2.Location = new System.Drawing.Point(0, 0);
					this.panel2.Name = "panel2";
					this.panel2.Size = new System.Drawing.Size(500, 260);
					this.panel2.TabIndex = 0;
					// 
					// treeView_nodes
					// 
					this.treeView_nodes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
											| System.Windows.Forms.AnchorStyles.Left)
											| System.Windows.Forms.AnchorStyles.Right)));
					this.treeView_nodes.CheckBoxes = true;
					this.treeView_nodes.ImageIndex = 0;
					this.treeView_nodes.ImageList = this.imageList_SharePointIcons;
					this.treeView_nodes.Location = new System.Drawing.Point(0, 56);
					this.treeView_nodes.Name = "treeView_nodes";
					this.treeView_nodes.SelectedImageIndex = 0;
					this.treeView_nodes.Size = new System.Drawing.Size(500, 201);
					this.treeView_nodes.TabIndex = 4;
					this.treeView_nodes.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView_nodes_AfterSelect);
					// 
					// imageList_SharePointIcons
					// 
					this.imageList_SharePointIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList_SharePointIcons.ImageStream")));
					this.imageList_SharePointIcons.TransparentColor = System.Drawing.Color.Transparent;
					this.imageList_SharePointIcons.Images.SetKeyName(0, "SPSite");
					this.imageList_SharePointIcons.Images.SetKeyName(1, "SPList");
					this.imageList_SharePointIcons.Images.SetKeyName(2, "SPFolder");
					this.imageList_SharePointIcons.Images.SetKeyName(3, "SPFile");
					this.imageList_SharePointIcons.Images.SetKeyName(4, "SPDocumentLibrary");
					this.imageList_SharePointIcons.Images.SetKeyName(5, "SPListItem");
					// 
					// button1
					// 
					this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
					this.button1.Location = new System.Drawing.Point(356, 16);
					this.button1.Name = "button1";
					this.button1.Size = new System.Drawing.Size(144, 23);
					this.button1.TabIndex = 3;
					this.button1.Text = "Load Structure";
					this.button1.UseVisualStyleBackColor = true;
					this.button1.Click += new System.EventHandler(this.button1_Click);
					// 
					// label2
					// 
					this.label2.AutoSize = true;
					this.label2.Location = new System.Drawing.Point(-1, 40);
					this.label2.Name = "label2";
					this.label2.Size = new System.Drawing.Size(164, 13);
					this.label2.TabIndex = 2;
					this.label2.Text = "Select the elements for the import";
					// 
					// comboBox1
					// 
					this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
											| System.Windows.Forms.AnchorStyles.Right)));
					this.comboBox1.FormattingEnabled = true;
					this.comboBox1.Location = new System.Drawing.Point(0, 16);
					this.comboBox1.Name = "comboBox1";
					this.comboBox1.Size = new System.Drawing.Size(350, 21);
					this.comboBox1.TabIndex = 1;
					this.comboBox1.DropDown += new System.EventHandler(this.comboBox1_DropDown);
					// 
					// label1
					// 
					this.label1.AutoSize = true;
					this.label1.Location = new System.Drawing.Point(-1, 0);
					this.label1.Name = "label1";
					this.label1.Size = new System.Drawing.Size(191, 13);
					this.label1.TabIndex = 0;
					this.label1.Text = "Enter URL of SharePoint site collection";
					// 
					// SPExportPage
					// 
					this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
					this.Controls.Add(this.panel2);
					this.Headline = "Source Data";
					this.InfoRTBoxSize = new System.Drawing.Size(550, 50);
					this.InfoRTBoxText = "Connect to a existing SharePoint site collection. ";
					this.Name = "SPExportPage";
					this.ShowInfoPanel = true;
					this.Size = new System.Drawing.Size(500, 350);
					this.StepTitle = "Source Data";
					this.Controls.SetChildIndex(this.infoPanel, 0);
					this.Controls.SetChildIndex(this.panel2, 0);
					this.infoPanel.ResumeLayout(false);
					this.infoPanel.PerformLayout();
					this.panel2.ResumeLayout(false);
					this.panel2.PerformLayout();
					((System.ComponentModel.ISupportInitialize)(this)).EndInit();
					this.ResumeLayout(false);
					this.PerformLayout();

        }
        #endregion

        private Panel panel1;
        private Panel panel2;
        private ComboBox comboBox1;
        private Label label1;
        private Label label2;
        private Button button1;
        private TreeView treeView_nodes;
        private ImageList imageList_SharePointIcons;


    }
}