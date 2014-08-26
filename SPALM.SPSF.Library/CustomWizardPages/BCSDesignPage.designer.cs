using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Practices.WizardFramework;

namespace SPALM.SPSF.Library.CustomWizardPages
{
    partial class BCSDesignPage : SPSFBaseWizardPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BCSDesignPage));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.bcsTreeControl1 = new SPALM.SPSF.Library.BCSTreeControl();
            this.imageList_SharePointIcons = new System.Windows.Forms.ImageList(this.components);
            this.infoPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // infoPanel
            // 
            this.infoPanel.Controls.Add(this.panel1);
            this.infoPanel.Location = new System.Drawing.Point(0, 145);
            this.infoPanel.Size = new System.Drawing.Size(500, 205);
            this.infoPanel.Controls.SetChildIndex(this.panel1, 0);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(500, 205);
            this.panel1.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.bcsTreeControl1);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(497, 348);
            this.panel2.TabIndex = 0;
            // 
            // bcsTreeControl1
            // 
            this.bcsTreeControl1.IsDesignMode = false;
            this.bcsTreeControl1.Location = new System.Drawing.Point(0, 0);
            this.bcsTreeControl1.Name = "bcsTreeControl1";
            this.bcsTreeControl1.Size = new System.Drawing.Size(494, 345);
            this.bcsTreeControl1.TabIndex = 0;
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
            this.imageList_SharePointIcons.Images.SetKeyName(6, "SPContentType");
            this.imageList_SharePointIcons.Images.SetKeyName(7, "SPField");
            // 
            // BCSDesignPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.panel2);
            this.Headline = "Database";
            this.InfoRTBoxSize = new System.Drawing.Size(550, 50);
            this.InfoRTBoxText = "Select the tables and fields which should be imported into the BCS model";
            this.Name = "BCSDesignPage";
            this.Size = new System.Drawing.Size(500, 351);
            this.StepTitle = "Model Entities";
            this.Controls.SetChildIndex(this.infoPanel, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.infoPanel.ResumeLayout(false);
            this.infoPanel.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private Panel panel1;
        private Panel panel2;
        private ImageList imageList_SharePointIcons;
        private BCSTreeControl bcsTreeControl1;


    }
}