namespace SPALM.SPSF.Library
{
    partial class SelectionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectionForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.searchgroup = new System.Windows.Forms.Panel();
            this.button_clear = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button_find = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.Panel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.selectedItem = new System.Windows.Forms.Panel();
            this.selectedGroup = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.selectedDescription = new System.Windows.Forms.Label();
            this.selectedValue = new System.Windows.Forms.Label();
            this.selectedName = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button_cancel = new System.Windows.Forms.Button();
            this.buton_ok = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.searchgroup.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.selectedItem.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.searchgroup);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(528, 38);
            this.panel1.TabIndex = 0;
            // 
            // searchgroup
            // 
            this.searchgroup.Controls.Add(this.button_clear);
            this.searchgroup.Controls.Add(this.label1);
            this.searchgroup.Controls.Add(this.button_find);
            this.searchgroup.Controls.Add(this.textBox1);
            this.searchgroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchgroup.Location = new System.Drawing.Point(5, 5);
            this.searchgroup.Name = "searchgroup";
            this.searchgroup.Size = new System.Drawing.Size(518, 28);
            this.searchgroup.TabIndex = 0;
            this.searchgroup.Text = "Search";
            // 
            // button_clear
            // 
            this.button_clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_clear.Location = new System.Drawing.Point(456, 1);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(61, 23);
            this.button_clear.TabIndex = 3;
            this.button_clear.Text = "Clear";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Search";
            // 
            // button_find
            // 
            this.button_find.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_find.Location = new System.Drawing.Point(389, 1);
            this.button_find.Name = "button_find";
            this.button_find.Size = new System.Drawing.Size(61, 23);
            this.button_find.TabIndex = 1;
            this.button_find.Text = "Find";
            this.button_find.UseVisualStyleBackColor = true;
            this.button_find.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(64, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(313, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.splitContainer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 38);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(528, 357);
            this.panel2.TabIndex = 1;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(5, 5);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.selectedItem);
            this.splitContainer1.Size = new System.Drawing.Size(518, 347);
            this.splitContainer1.SplitterDistance = 241;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.treeView);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(518, 241);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.Text = "Available Items";
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.ShowNodeToolTips = true;
            this.treeView.Size = new System.Drawing.Size(518, 241);
            this.treeView.TabIndex = 0;
            this.treeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterCheck);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            this.treeView.DoubleClick += new System.EventHandler(this.treeView_DoubleClick);
            // 
            // selectedItem
            // 
            this.selectedItem.Controls.Add(this.selectedGroup);
            this.selectedItem.Controls.Add(this.label5);
            this.selectedItem.Controls.Add(this.label4);
            this.selectedItem.Controls.Add(this.label3);
            this.selectedItem.Controls.Add(this.label2);
            this.selectedItem.Controls.Add(this.selectedDescription);
            this.selectedItem.Controls.Add(this.selectedValue);
            this.selectedItem.Controls.Add(this.selectedName);
            this.selectedItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectedItem.Location = new System.Drawing.Point(0, 0);
            this.selectedItem.Name = "selectedItem";
            this.selectedItem.Size = new System.Drawing.Size(518, 102);
            this.selectedItem.TabIndex = 0;
            this.selectedItem.Text = "Selected Item";
            // 
            // selectedGroup
            // 
            this.selectedGroup.Location = new System.Drawing.Point(74, 46);
            this.selectedGroup.Name = "selectedGroup";
            this.selectedGroup.Size = new System.Drawing.Size(437, 14);
            this.selectedGroup.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Group:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Description:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Value:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Name:";
            // 
            // selectedDescription
            // 
            this.selectedDescription.Location = new System.Drawing.Point(74, 66);
            this.selectedDescription.Name = "selectedDescription";
            this.selectedDescription.Size = new System.Drawing.Size(437, 36);
            this.selectedDescription.TabIndex = 2;
            // 
            // selectedValue
            // 
            this.selectedValue.Location = new System.Drawing.Point(74, 27);
            this.selectedValue.Name = "selectedValue";
            this.selectedValue.Size = new System.Drawing.Size(437, 13);
            this.selectedValue.TabIndex = 1;
            // 
            // selectedName
            // 
            this.selectedName.Location = new System.Drawing.Point(74, 5);
            this.selectedName.Name = "selectedName";
            this.selectedName.Size = new System.Drawing.Size(438, 15);
            this.selectedName.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button_cancel);
            this.panel3.Controls.Add(this.buton_ok);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 395);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(528, 37);
            this.panel3.TabIndex = 2;
            // 
            // button_cancel
            // 
            this.button_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_cancel.Location = new System.Drawing.Point(447, 6);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            // 
            // buton_ok
            // 
            this.buton_ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buton_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buton_ok.Location = new System.Drawing.Point(366, 6);
            this.buton_ok.Name = "buton_ok";
            this.buton_ok.Size = new System.Drawing.Size(75, 23);
            this.buton_ok.TabIndex = 0;
            this.buton_ok.Text = "OK";
            this.buton_ok.UseVisualStyleBackColor = true;
            this.buton_ok.Click += new System.EventHandler(this.button1_Click);
            // 
            // SelectionForm
            // 
            this.AcceptButton = this.button_find;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_cancel;
            this.ClientSize = new System.Drawing.Size(528, 432);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Selection";
            this.panel1.ResumeLayout(false);
            this.searchgroup.ResumeLayout(false);
            this.searchgroup.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.selectedItem.ResumeLayout(false);
            this.selectedItem.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Button buton_ok;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel groupBox3;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Panel selectedItem;
        private System.Windows.Forms.Button button_find;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label selectedName;
        private System.Windows.Forms.Label selectedValue;
        private System.Windows.Forms.Label selectedDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Label selectedGroup;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel searchgroup;
    }
}