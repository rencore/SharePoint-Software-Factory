namespace SPALM.SPSF.Test.BCSTester
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.bcsTreeControl1 = new SPALM.SPSF.Library.BCSTreeControl();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(718, 21);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Table");
            this.imageList1.Images.SetKeyName(1, "Field");
            this.imageList1.Images.SetKeyName(2, "Key");
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(799, 21);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "Generate";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // bcsTreeControl1
            // 
            this.bcsTreeControl1.Location = new System.Drawing.Point(15, 12);
            this.bcsTreeControl1.Name = "bcsTreeControl1";
            this.bcsTreeControl1.Size = new System.Drawing.Size(613, 379);
            this.bcsTreeControl1.TabIndex = 15;
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "BCSModel_SQL.bdcm.t4",
            "BCSModel_Classes.bdcm.t4",
            "BCSModel_Classes.cs.t4",
            "BCSModel_ClassesService.cs.t4",
            "BCSModel_Classes.bdcm.diagram.t4",
            "BCSModel_DBML.dbml.t4",
            "BCSModel_ClassesService_LINQ.cs.t4"});
            this.checkedListBox1.Location = new System.Drawing.Point(718, 84);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(175, 124);
            this.checkedListBox1.TabIndex = 17;
            // 
            // tabControl1
            // 
            this.tabControl1.Location = new System.Drawing.Point(13, 398);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(880, 372);
            this.tabControl1.TabIndex = 18;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 782);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.bcsTreeControl1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "BCS Tester";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button button2;
        private SPALM.SPSF.Library.BCSTreeControl bcsTreeControl1;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.TabControl tabControl1;
    }
}

