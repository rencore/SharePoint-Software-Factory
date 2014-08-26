using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Practices.WizardFramework;

namespace SPALM.SPSF.Library.CustomWizardPages
{
    partial class AboutPage : SPSFBaseWizardPage
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label_authors = new System.Windows.Forms.Label();
            this.label_fileversion = new System.Windows.Forms.Label();
            this.label_version = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label_description = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_title = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.infoPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // infoPanel
            // 
            this.infoPanel.Controls.Add(this.panel1);
            this.infoPanel.Location = new System.Drawing.Point(0, 41);
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
            this.panel2.Controls.Add(this.linkLabel1);
            this.panel2.Controls.Add(this.label_authors);
            this.panel2.Controls.Add(this.label_fileversion);
            this.panel2.Controls.Add(this.label_version);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label_description);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label_title);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(495, 298);
            this.panel2.TabIndex = 0;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(18, 204);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(126, 13);
            this.linkLabel1.TabIndex = 12;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://spsf.codeplex.com";
            // 
            // label_authors
            // 
            this.label_authors.AutoSize = true;
            this.label_authors.Location = new System.Drawing.Point(88, 271);
            this.label_authors.Name = "label_authors";
            this.label_authors.Size = new System.Drawing.Size(173, 13);
            this.label_authors.TabIndex = 9;
            this.label_authors.Text = "Torsten Mandelkow, Matthias Einig";
            // 
            // label_fileversion
            // 
            this.label_fileversion.AutoSize = true;
            this.label_fileversion.Location = new System.Drawing.Point(88, 248);
            this.label_fileversion.Name = "label_fileversion";
            this.label_fileversion.Size = new System.Drawing.Size(87, 13);
            this.label_fileversion.TabIndex = 8;
            this.label_fileversion.Text = "Fileversion Value";
            // 
            // label_version
            // 
            this.label_version.AutoSize = true;
            this.label_version.Location = new System.Drawing.Point(88, 226);
            this.label_version.Name = "label_version";
            this.label_version.Size = new System.Drawing.Size(72, 13);
            this.label_version.TabIndex = 7;
            this.label_version.Text = "Version Value";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 271);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Authors:";
            // 
            // label_description
            // 
            this.label_description.Location = new System.Drawing.Point(18, 22);
            this.label_description.Name = "label_description";
            this.label_description.Size = new System.Drawing.Size(266, 167);
            this.label_description.TabIndex = 5;
            this.label_description.Text = "Description";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 248);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Fileversion:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 226);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Version:";
            // 
            // label_title
            // 
            this.label_title.AutoSize = true;
            this.label_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_title.Location = new System.Drawing.Point(18, 2);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(32, 13);
            this.label_title.TabIndex = 2;
            this.label_title.Text = "Title";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SPALM.SPSF.Library.Properties.Resources.spsfid;
            this.pictureBox1.Location = new System.Drawing.Point(297, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(195, 286);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // AboutPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.panel2);
            this.Headline = "Info";
            this.InfoRTBoxSize = new System.Drawing.Size(550, 50);
            this.InfoRTBoxText = "\tSelect an existing feature or create a new feature.";
            this.Name = "AboutPage";
            this.Size = new System.Drawing.Size(500, 350);
            this.StepTitle = "Info";
            this.Controls.SetChildIndex(this.infoPanel, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.infoPanel.ResumeLayout(false);
            this.infoPanel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private Panel panel1;
        private Panel panel2;
        private PictureBox pictureBox1;
        private Label label_description;
        private Label label3;
        private Label label2;
        private Label label_title;
        private Label label5;
        private Label label_authors;
        private Label label_fileversion;
        private Label label_version;
        private LinkLabel linkLabel1;


    }
}