using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Practices.WizardFramework;

namespace SPALM.SPSF.Library.CustomWizardPages
{
    partial class BCSSingleEntityPage : SPSFBaseWizardPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BCSSingleEntityPage));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBoxCreateOperation = new System.Windows.Forms.CheckBox();
            this.checkBoxDeleteOperation = new System.Windows.Forms.CheckBox();
            this.checkBoxUpdateOperation = new System.Windows.Forms.CheckBox();
            this.checkBoxReadOperation = new System.Windows.Forms.CheckBox();
            this.textBoxEntityDisplayName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxEntityName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.imageList_SharePointIcons = new System.Windows.Forms.ImageList(this.components);
            this.infoPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // infoPanel
            // 
            this.infoPanel.Controls.Add(this.panel1);
            this.infoPanel.Location = new System.Drawing.Point(0, 42);
            this.infoPanel.Size = new System.Drawing.Size(500, 309);
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
            this.panel2.Controls.Add(this.checkBoxCreateOperation);
            this.panel2.Controls.Add(this.checkBoxDeleteOperation);
            this.panel2.Controls.Add(this.checkBoxUpdateOperation);
            this.panel2.Controls.Add(this.checkBoxReadOperation);
            this.panel2.Controls.Add(this.textBoxEntityDisplayName);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.textBoxEntityName);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(497, 348);
            this.panel2.TabIndex = 0;
            // 
            // checkBoxCreateOperation
            // 
            this.checkBoxCreateOperation.AutoSize = true;
            this.checkBoxCreateOperation.Checked = true;
            this.checkBoxCreateOperation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCreateOperation.Location = new System.Drawing.Point(1, 94);
            this.checkBoxCreateOperation.Name = "checkBoxCreateOperation";
            this.checkBoxCreateOperation.Size = new System.Drawing.Size(145, 17);
            this.checkBoxCreateOperation.TabIndex = 7;
            this.checkBoxCreateOperation.Text = "Create Create Operations";
            this.checkBoxCreateOperation.UseVisualStyleBackColor = true;
            this.checkBoxCreateOperation.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // checkBoxDeleteOperation
            // 
            this.checkBoxDeleteOperation.AutoSize = true;
            this.checkBoxDeleteOperation.Checked = true;
            this.checkBoxDeleteOperation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDeleteOperation.Location = new System.Drawing.Point(1, 166);
            this.checkBoxDeleteOperation.Name = "checkBoxDeleteOperation";
            this.checkBoxDeleteOperation.Size = new System.Drawing.Size(145, 17);
            this.checkBoxDeleteOperation.TabIndex = 6;
            this.checkBoxDeleteOperation.Text = "Create Delete Operations";
            this.checkBoxDeleteOperation.UseVisualStyleBackColor = true;
            this.checkBoxDeleteOperation.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // checkBoxUpdateOperation
            // 
            this.checkBoxUpdateOperation.AutoSize = true;
            this.checkBoxUpdateOperation.Checked = true;
            this.checkBoxUpdateOperation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUpdateOperation.Location = new System.Drawing.Point(1, 142);
            this.checkBoxUpdateOperation.Name = "checkBoxUpdateOperation";
            this.checkBoxUpdateOperation.Size = new System.Drawing.Size(149, 17);
            this.checkBoxUpdateOperation.TabIndex = 5;
            this.checkBoxUpdateOperation.Text = "Create Update Operations";
            this.checkBoxUpdateOperation.UseVisualStyleBackColor = true;
            this.checkBoxUpdateOperation.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBoxReadOperation
            // 
            this.checkBoxReadOperation.AutoSize = true;
            this.checkBoxReadOperation.Checked = true;
            this.checkBoxReadOperation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxReadOperation.Location = new System.Drawing.Point(1, 118);
            this.checkBoxReadOperation.Name = "checkBoxReadOperation";
            this.checkBoxReadOperation.Size = new System.Drawing.Size(140, 17);
            this.checkBoxReadOperation.TabIndex = 4;
            this.checkBoxReadOperation.Text = "Create Read Operations";
            this.checkBoxReadOperation.UseVisualStyleBackColor = true;
            this.checkBoxReadOperation.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // textBoxEntityDisplayName
            // 
            this.textBoxEntityDisplayName.Location = new System.Drawing.Point(1, 62);
            this.textBoxEntityDisplayName.Name = "textBoxEntityDisplayName";
            this.textBoxEntityDisplayName.Size = new System.Drawing.Size(493, 20);
            this.textBoxEntityDisplayName.TabIndex = 3;
            this.textBoxEntityDisplayName.Text = "Entity 1";
            this.textBoxEntityDisplayName.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-1, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Entity Display Name";
            // 
            // textBoxEntityName
            // 
            this.textBoxEntityName.Location = new System.Drawing.Point(1, 17);
            this.textBoxEntityName.Name = "textBoxEntityName";
            this.textBoxEntityName.Size = new System.Drawing.Size(493, 20);
            this.textBoxEntityName.TabIndex = 1;
            this.textBoxEntityName.Text = "Entity1";
            this.textBoxEntityName.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-1, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Entity Name";
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
            // BCSSingleEntityPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.panel2);
            this.Headline = "Database";
            this.InfoRTBoxSize = new System.Drawing.Size(550, 50);
            this.InfoRTBoxText = "Select the tables and fields which should be imported into the BCS model";
            this.Name = "BCSSingleEntityPage";
            this.Size = new System.Drawing.Size(500, 351);
            this.StepTitle = "Model Entities";
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
        private ImageList imageList_SharePointIcons;
        private CheckBox checkBoxCreateOperation;
        private CheckBox checkBoxDeleteOperation;
        private CheckBox checkBoxUpdateOperation;
        private CheckBox checkBoxReadOperation;
        private TextBox textBoxEntityDisplayName;
        private Label label2;
        private TextBox textBoxEntityName;
        private Label label1;


    }
}