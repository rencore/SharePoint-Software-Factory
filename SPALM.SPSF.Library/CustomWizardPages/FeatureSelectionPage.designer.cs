using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Practices.WizardFramework;

namespace SPALM.SPSF.Library.CustomWizardPages
{
    partial class FeatureSelectionPage : SPSFBaseWizardPage
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
					this.groupBox1 = new System.Windows.Forms.GroupBox();
					this.checkBox_Hidden = new System.Windows.Forms.CheckBox();
					this.textBox_FeatureTitle = new System.Windows.Forms.TextBox();
					this.label5 = new System.Windows.Forms.Label();
					this.checkBox_CreateReceiver = new System.Windows.Forms.CheckBox();
					this.comboBox_Scopes = new System.Windows.Forms.ComboBox();
					this.textBox_FeatureName = new System.Windows.Forms.TextBox();
					this.label2 = new System.Windows.Forms.Label();
					this.textBox_FeatureDescription = new System.Windows.Forms.TextBox();
					this.label3 = new System.Windows.Forms.Label();
					this.label4 = new System.Windows.Forms.Label();
					this.comboBox1 = new System.Windows.Forms.ComboBox();
					this.label1 = new System.Windows.Forms.Label();
					this.infoPanel.SuspendLayout();
					this.panel2.SuspendLayout();
					this.groupBox1.SuspendLayout();
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
					this.panel2.Controls.Add(this.groupBox1);
					this.panel2.Controls.Add(this.comboBox1);
					this.panel2.Controls.Add(this.label1);
					this.panel2.Location = new System.Drawing.Point(0, 0);
					this.panel2.Name = "panel2";
					this.panel2.Size = new System.Drawing.Size(495, 298);
					this.panel2.TabIndex = 0;
					// 
					// groupBox1
					// 
					this.groupBox1.Controls.Add(this.checkBox_Hidden);
					this.groupBox1.Controls.Add(this.textBox_FeatureTitle);
					this.groupBox1.Controls.Add(this.label5);
					this.groupBox1.Controls.Add(this.checkBox_CreateReceiver);
					this.groupBox1.Controls.Add(this.comboBox_Scopes);
					this.groupBox1.Controls.Add(this.textBox_FeatureName);
					this.groupBox1.Controls.Add(this.label2);
					this.groupBox1.Controls.Add(this.textBox_FeatureDescription);
					this.groupBox1.Controls.Add(this.label3);
					this.groupBox1.Controls.Add(this.label4);
					this.groupBox1.Location = new System.Drawing.Point(3, 54);
					this.groupBox1.Name = "groupBox1";
					this.groupBox1.Size = new System.Drawing.Size(489, 236);
					this.groupBox1.TabIndex = 8;
					this.groupBox1.TabStop = false;
					this.groupBox1.Text = "New feature settings";
					this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
					// 
					// checkBox_Hidden
					// 
					this.checkBox_Hidden.AutoSize = true;
					this.checkBox_Hidden.Location = new System.Drawing.Point(10, 203);
					this.checkBox_Hidden.Name = "checkBox_Hidden";
					this.checkBox_Hidden.Size = new System.Drawing.Size(96, 17);
					this.checkBox_Hidden.TabIndex = 12;
					this.checkBox_Hidden.Text = "Hidden feature";
					this.checkBox_Hidden.UseVisualStyleBackColor = true;
					this.checkBox_Hidden.CheckedChanged += new System.EventHandler(this.checkBox_CreateReceiver_CheckedChanged);
					// 
					// textBox_FeatureTitle
					// 
					this.textBox_FeatureTitle.Location = new System.Drawing.Point(10, 74);
					this.textBox_FeatureTitle.Name = "textBox_FeatureTitle";
					this.textBox_FeatureTitle.Size = new System.Drawing.Size(469, 20);
					this.textBox_FeatureTitle.TabIndex = 11;
					this.textBox_FeatureTitle.TextChanged += new System.EventHandler(this.textBox_FeatureDescription_TextChanged);
					// 
					// label5
					// 
					this.label5.AutoSize = true;
					this.label5.Location = new System.Drawing.Point(7, 58);
					this.label5.Name = "label5";
					this.label5.Size = new System.Drawing.Size(27, 13);
					this.label5.TabIndex = 10;
					this.label5.Text = "Title";
					// 
					// checkBox_CreateReceiver
					// 
					this.checkBox_CreateReceiver.AutoSize = true;
					this.checkBox_CreateReceiver.Location = new System.Drawing.Point(10, 180);
					this.checkBox_CreateReceiver.Name = "checkBox_CreateReceiver";
					this.checkBox_CreateReceiver.Size = new System.Drawing.Size(139, 17);
					this.checkBox_CreateReceiver.TabIndex = 9;
					this.checkBox_CreateReceiver.Text = "Create FeatureReceiver";
					this.checkBox_CreateReceiver.UseVisualStyleBackColor = true;
					this.checkBox_CreateReceiver.CheckedChanged += new System.EventHandler(this.checkBox_CreateReceiver_CheckedChanged);
					// 
					// comboBox_Scopes
					// 
					this.comboBox_Scopes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
					this.comboBox_Scopes.FormattingEnabled = true;
					this.comboBox_Scopes.Location = new System.Drawing.Point(10, 153);
					this.comboBox_Scopes.Name = "comboBox_Scopes";
					this.comboBox_Scopes.Size = new System.Drawing.Size(469, 21);
					this.comboBox_Scopes.TabIndex = 7;
					this.comboBox_Scopes.SelectedIndexChanged += new System.EventHandler(this.comboBox_Scopes_SelectedIndexChanged);
					// 
					// textBox_FeatureName
					// 
					this.textBox_FeatureName.Location = new System.Drawing.Point(10, 33);
					this.textBox_FeatureName.Name = "textBox_FeatureName";
					this.textBox_FeatureName.Size = new System.Drawing.Size(469, 20);
					this.textBox_FeatureName.TabIndex = 5;
					this.textBox_FeatureName.TextChanged += new System.EventHandler(this.textBox_FeatureDescription_TextChanged);
					// 
					// label2
					// 
					this.label2.AutoSize = true;
					this.label2.Location = new System.Drawing.Point(7, 17);
					this.label2.Name = "label2";
					this.label2.Size = new System.Drawing.Size(35, 13);
					this.label2.TabIndex = 2;
					this.label2.Text = "Name";
					// 
					// textBox_FeatureDescription
					// 
					this.textBox_FeatureDescription.Location = new System.Drawing.Point(10, 113);
					this.textBox_FeatureDescription.Name = "textBox_FeatureDescription";
					this.textBox_FeatureDescription.Size = new System.Drawing.Size(469, 20);
					this.textBox_FeatureDescription.TabIndex = 6;
					this.textBox_FeatureDescription.TextChanged += new System.EventHandler(this.textBox_FeatureDescription_TextChanged);
					// 
					// label3
					// 
					this.label3.AutoSize = true;
					this.label3.Location = new System.Drawing.Point(7, 97);
					this.label3.Name = "label3";
					this.label3.Size = new System.Drawing.Size(60, 13);
					this.label3.TabIndex = 3;
					this.label3.Text = "Description";
					// 
					// label4
					// 
					this.label4.AutoSize = true;
					this.label4.Location = new System.Drawing.Point(7, 136);
					this.label4.Name = "label4";
					this.label4.Size = new System.Drawing.Size(38, 13);
					this.label4.TabIndex = 4;
					this.label4.Text = "Scope";
					// 
					// comboBox1
					// 
					this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
											| System.Windows.Forms.AnchorStyles.Right)));
					this.comboBox1.BackColor = System.Drawing.SystemColors.Window;
					this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
					this.comboBox1.FormattingEnabled = true;
					this.comboBox1.Location = new System.Drawing.Point(2, 19);
					this.comboBox1.Name = "comboBox1";
					this.comboBox1.Size = new System.Drawing.Size(490, 21);
					this.comboBox1.TabIndex = 1;
					this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
					// 
					// label1
					// 
					this.label1.AutoSize = true;
					this.label1.Location = new System.Drawing.Point(-1, 2);
					this.label1.Name = "label1";
					this.label1.Size = new System.Drawing.Size(106, 13);
					this.label1.TabIndex = 0;
					this.label1.Text = "Select parent feature";
					// 
					// FeatureSelectionPage
					// 
					this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
					this.Controls.Add(this.panel2);
					this.Headline = "Parent Feature";
					this.InfoRTBoxSize = new System.Drawing.Size(550, 50);
					this.InfoRTBoxText = "Select an existing feature or create a new feature.";
					this.Name = "FeatureSelectionPage";
					this.Size = new System.Drawing.Size(500, 350);
					this.StepTitle = "Parent Feature";
					this.Controls.SetChildIndex(this.infoPanel, 0);
					this.Controls.SetChildIndex(this.panel2, 0);
					this.infoPanel.ResumeLayout(false);
					this.infoPanel.PerformLayout();
					this.panel2.ResumeLayout(false);
					this.panel2.PerformLayout();
					this.groupBox1.ResumeLayout(false);
					this.groupBox1.PerformLayout();
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
				private TextBox textBox_FeatureDescription;
				private TextBox textBox_FeatureName;
				private Label label4;
				private Label label3;
				private GroupBox groupBox1;
				private ComboBox comboBox_Scopes;
				private CheckBox checkBox_CreateReceiver;
				private TextBox textBox_FeatureTitle;
				private Label label5;
				private CheckBox checkBox_Hidden;


    }
}