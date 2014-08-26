using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Practices.WizardFramework;

namespace SPALM.SPSF.Library.CustomWizardPages
{
  partial class SiteColumnPage : CustomWizardPage
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
          this.siteColumnWizard1 = new SPALM.SPSF.Library.CustomWizardPages.SiteColumnWizard();
          this.infoPanel.SuspendLayout();
          ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
          this.SuspendLayout();
          // 
          // infoPanel
          // 
          this.infoPanel.Controls.Add(this.panel1);
          this.infoPanel.Location = new System.Drawing.Point(0, 66);
          this.infoPanel.Size = new System.Drawing.Size(656, 309);
          this.infoPanel.Controls.SetChildIndex(this.panel1, 0);
          // 
          // panel1
          // 
          this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
          this.panel1.Location = new System.Drawing.Point(0, 0);
          this.panel1.Name = "panel1";
          this.panel1.Size = new System.Drawing.Size(656, 249);
          this.panel1.TabIndex = 9;
          // 
          // siteColumnWizard1
          // 
          this.siteColumnWizard1.Location = new System.Drawing.Point(0, 0);
          this.siteColumnWizard1.Name = "siteColumnWizard1";
          this.siteColumnWizard1.Size = new System.Drawing.Size(520, 284);
          this.siteColumnWizard1.TabIndex = 0;
          // 
          // SiteColumnPage
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.Controls.Add(this.siteColumnWizard1);
          this.Headline = "Site Column Settings";
          this.InfoRTBoxSize = new System.Drawing.Size(550, 50);
          this.InfoRTBoxText = "Configure the site column.";
          this.Name = "SiteColumnPage";
          this.ShowInfoPanel = true;
          this.Size = new System.Drawing.Size(656, 375);
          this.StepTitle = "Site Column Settings";
          this.Controls.SetChildIndex(this.infoPanel, 0);
          this.Controls.SetChildIndex(this.siteColumnWizard1, 0);
          this.infoPanel.ResumeLayout(false);
          this.infoPanel.PerformLayout();
          ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
          this.ResumeLayout(false);
          this.PerformLayout();

        }
        #endregion

        private Panel panel1;
        private SiteColumnWizard siteColumnWizard1;


    }
}