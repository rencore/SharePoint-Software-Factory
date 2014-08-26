namespace SPALM.SPSF.Library
{
    partial class BCSTreeControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BCSTreeControl));
            this.panel2 = new System.Windows.Forms.Panel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBoxField = new System.Windows.Forms.GroupBox();
            this.comboBoxReferencedField = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBoxReferencedEntity = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.checkBoxIsKey = new System.Windows.Forms.CheckBox();
            this.comboBoxDataType = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxFieldName = new System.Windows.Forms.TextBox();
            this.comboBoxOfficeProperty = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxRequired = new System.Windows.Forms.CheckBox();
            this.checkBoxReadOnly = new System.Windows.Forms.CheckBox();
            this.checkBoxShowInPicker = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBoxTable = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxTableName = new System.Windows.Forms.TextBox();
            this.comboBoxOffice = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxTitleField = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxTableDisplayName = new System.Windows.Forms.TextBox();
            this.checkBoxCreate = new System.Windows.Forms.CheckBox();
            this.checkBoxDelete = new System.Windows.Forms.CheckBox();
            this.checkBoxUpdate = new System.Windows.Forms.CheckBox();
            this.checkBoxRead = new System.Windows.Forms.CheckBox();
            this.panelDesign = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAddEntity = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDeleteEntity = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonAddField = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDeleteField = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonUp = new System.Windows.Forms.ToolStripButton();
            this.buttonDown = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxInfo = new System.Windows.Forms.TextBox();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBoxField.SuspendLayout();
            this.groupBoxTable.SuspendLayout();
            this.panelDesign.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.treeView1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(613, 607);
            this.panel2.TabIndex = 13;
            // 
            // treeView1
            // 
            this.treeView1.CheckBoxes = true;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.HideSelection = false;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(333, 607);
            this.treeView1.TabIndex = 7;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Table");
            this.imageList1.Images.SetKeyName(1, "Field");
            this.imageList1.Images.SetKeyName(2, "Key");
            this.imageList1.Images.SetKeyName(3, "Foreign");
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBoxField);
            this.panel3.Controls.Add(this.groupBoxTable);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(333, 0);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(8, 0, 2, 8);
            this.panel3.Size = new System.Drawing.Size(280, 607);
            this.panel3.TabIndex = 8;
            // 
            // groupBoxField
            // 
            this.groupBoxField.Controls.Add(this.comboBoxReferencedField);
            this.groupBoxField.Controls.Add(this.label13);
            this.groupBoxField.Controls.Add(this.label12);
            this.groupBoxField.Controls.Add(this.comboBoxReferencedEntity);
            this.groupBoxField.Controls.Add(this.label11);
            this.groupBoxField.Controls.Add(this.label9);
            this.groupBoxField.Controls.Add(this.checkBoxIsKey);
            this.groupBoxField.Controls.Add(this.comboBoxDataType);
            this.groupBoxField.Controls.Add(this.label8);
            this.groupBoxField.Controls.Add(this.label7);
            this.groupBoxField.Controls.Add(this.textBoxFieldName);
            this.groupBoxField.Controls.Add(this.comboBoxOfficeProperty);
            this.groupBoxField.Controls.Add(this.label5);
            this.groupBoxField.Controls.Add(this.checkBoxRequired);
            this.groupBoxField.Controls.Add(this.checkBoxReadOnly);
            this.groupBoxField.Controls.Add(this.checkBoxShowInPicker);
            this.groupBoxField.Controls.Add(this.label1);
            this.groupBoxField.Controls.Add(this.textBox1);
            this.groupBoxField.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxField.Location = new System.Drawing.Point(8, 216);
            this.groupBoxField.Name = "groupBoxField";
            this.groupBoxField.Size = new System.Drawing.Size(270, 260);
            this.groupBoxField.TabIndex = 1;
            this.groupBoxField.TabStop = false;
            this.groupBoxField.Text = "Field Settings";
            this.groupBoxField.Visible = false;
            // 
            // comboBoxReferencedField
            // 
            this.comboBoxReferencedField.Enabled = false;
            this.comboBoxReferencedField.FormattingEnabled = true;
            this.comboBoxReferencedField.Location = new System.Drawing.Point(105, 232);
            this.comboBoxReferencedField.Name = "comboBoxReferencedField";
            this.comboBoxReferencedField.Size = new System.Drawing.Size(157, 21);
            this.comboBoxReferencedField.TabIndex = 33;
            this.comboBoxReferencedField.SelectedIndexChanged += new System.EventHandler(this.comboBoxReferencedField_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 235);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(88, 13);
            this.label13.TabIndex = 32;
            this.label13.Text = "Referenced Field";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 210);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(92, 13);
            this.label12.TabIndex = 31;
            this.label12.Text = "Referenced Entity";
            // 
            // comboBoxReferencedEntity
            // 
            this.comboBoxReferencedEntity.Enabled = false;
            this.comboBoxReferencedEntity.FormattingEnabled = true;
            this.comboBoxReferencedEntity.Location = new System.Drawing.Point(105, 207);
            this.comboBoxReferencedEntity.Name = "comboBoxReferencedEntity";
            this.comboBoxReferencedEntity.Size = new System.Drawing.Size(157, 21);
            this.comboBoxReferencedEntity.TabIndex = 30;
            this.comboBoxReferencedEntity.SelectedIndexChanged += new System.EventHandler(this.comboBoxReferencedEntity_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 116);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 13);
            this.label11.TabIndex = 29;
            this.label11.Text = "Options";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(8, 189);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 13);
            this.label9.TabIndex = 28;
            this.label9.Text = "Foreign Key";
            // 
            // checkBoxIsKey
            // 
            this.checkBoxIsKey.AutoSize = true;
            this.checkBoxIsKey.Location = new System.Drawing.Point(105, 115);
            this.checkBoxIsKey.Name = "checkBoxIsKey";
            this.checkBoxIsKey.Size = new System.Drawing.Size(69, 17);
            this.checkBoxIsKey.TabIndex = 27;
            this.checkBoxIsKey.Text = "Key Field";
            this.checkBoxIsKey.UseVisualStyleBackColor = true;
            this.checkBoxIsKey.CheckedChanged += new System.EventHandler(this.checkBoxIsKey_CheckedChanged);
            // 
            // comboBoxDataType
            // 
            this.comboBoxDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDataType.FormattingEnabled = true;
            this.comboBoxDataType.Items.AddRange(new object[] {
            "System.Boolean",
            "System.Byte",
            "System.DateTime",
            "System.Decimal",
            "System.Double",
            "System.Exception",
            "System.Guid",
            "System.Int16",
            "System.Int32",
            "System.Object",
            "System.SByte",
            "System.String",
            "System.Single",
            "System.TimeSpan",
            "System.UInt64",
            "System.UInt32",
            "System.UInt16"});
            this.comboBoxDataType.Location = new System.Drawing.Point(105, 64);
            this.comboBoxDataType.Name = "comboBoxDataType";
            this.comboBoxDataType.Size = new System.Drawing.Size(157, 21);
            this.comboBoxDataType.TabIndex = 26;
            this.comboBoxDataType.SelectedIndexChanged += new System.EventHandler(this.comboBoxDataType_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Data Type";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Name*";
            // 
            // textBoxFieldName
            // 
            this.textBoxFieldName.Location = new System.Drawing.Point(105, 16);
            this.textBoxFieldName.Name = "textBoxFieldName";
            this.textBoxFieldName.Size = new System.Drawing.Size(157, 20);
            this.textBoxFieldName.TabIndex = 23;
            this.textBoxFieldName.TextChanged += new System.EventHandler(this.textBoxFieldName_TextChanged);
            // 
            // comboBoxOfficeProperty
            // 
            this.comboBoxOfficeProperty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOfficeProperty.FormattingEnabled = true;
            this.comboBoxOfficeProperty.Items.AddRange(new object[] {
            "Generic List",
            "Appointment",
            "Contact",
            "Task",
            "Post"});
            this.comboBoxOfficeProperty.Location = new System.Drawing.Point(105, 89);
            this.comboBoxOfficeProperty.Name = "comboBoxOfficeProperty";
            this.comboBoxOfficeProperty.Size = new System.Drawing.Size(157, 21);
            this.comboBoxOfficeProperty.TabIndex = 22;
            this.comboBoxOfficeProperty.SelectedIndexChanged += new System.EventHandler(this.comboBoxOfficeProperty_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Office Property";
            // 
            // checkBoxRequired
            // 
            this.checkBoxRequired.AutoSize = true;
            this.checkBoxRequired.Location = new System.Drawing.Point(105, 175);
            this.checkBoxRequired.Name = "checkBoxRequired";
            this.checkBoxRequired.Size = new System.Drawing.Size(69, 17);
            this.checkBoxRequired.TabIndex = 17;
            this.checkBoxRequired.Text = "Required";
            this.checkBoxRequired.UseVisualStyleBackColor = true;
            this.checkBoxRequired.CheckedChanged += new System.EventHandler(this.checkBoxRequired_CheckedChanged);
            // 
            // checkBoxReadOnly
            // 
            this.checkBoxReadOnly.AutoSize = true;
            this.checkBoxReadOnly.Location = new System.Drawing.Point(105, 155);
            this.checkBoxReadOnly.Name = "checkBoxReadOnly";
            this.checkBoxReadOnly.Size = new System.Drawing.Size(76, 17);
            this.checkBoxReadOnly.TabIndex = 16;
            this.checkBoxReadOnly.Text = "Read Only";
            this.checkBoxReadOnly.UseVisualStyleBackColor = true;
            this.checkBoxReadOnly.CheckedChanged += new System.EventHandler(this.checkBoxReadOnly_CheckedChanged);
            // 
            // checkBoxShowInPicker
            // 
            this.checkBoxShowInPicker.AutoSize = true;
            this.checkBoxShowInPicker.Location = new System.Drawing.Point(105, 135);
            this.checkBoxShowInPicker.Name = "checkBoxShowInPicker";
            this.checkBoxShowInPicker.Size = new System.Drawing.Size(97, 17);
            this.checkBoxShowInPicker.TabIndex = 15;
            this.checkBoxShowInPicker.Text = "Show in Picker";
            this.checkBoxShowInPicker.UseVisualStyleBackColor = true;
            this.checkBoxShowInPicker.CheckedChanged += new System.EventHandler(this.checkBoxShowInPicker_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Display Name*";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(105, 40);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(157, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // groupBoxTable
            // 
            this.groupBoxTable.Controls.Add(this.label10);
            this.groupBoxTable.Controls.Add(this.label6);
            this.groupBoxTable.Controls.Add(this.textBoxTableName);
            this.groupBoxTable.Controls.Add(this.comboBoxOffice);
            this.groupBoxTable.Controls.Add(this.label4);
            this.groupBoxTable.Controls.Add(this.comboBoxTitleField);
            this.groupBoxTable.Controls.Add(this.label3);
            this.groupBoxTable.Controls.Add(this.label2);
            this.groupBoxTable.Controls.Add(this.textBoxTableDisplayName);
            this.groupBoxTable.Controls.Add(this.checkBoxCreate);
            this.groupBoxTable.Controls.Add(this.checkBoxDelete);
            this.groupBoxTable.Controls.Add(this.checkBoxUpdate);
            this.groupBoxTable.Controls.Add(this.checkBoxRead);
            this.groupBoxTable.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxTable.Location = new System.Drawing.Point(8, 0);
            this.groupBoxTable.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxTable.Name = "groupBoxTable";
            this.groupBoxTable.Size = new System.Drawing.Size(270, 216);
            this.groupBoxTable.TabIndex = 0;
            this.groupBoxTable.TabStop = false;
            this.groupBoxTable.Text = "Entity Settings";
            this.groupBoxTable.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 116);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "Operations";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Name*";
            // 
            // textBoxTableName
            // 
            this.textBoxTableName.Location = new System.Drawing.Point(105, 16);
            this.textBoxTableName.Name = "textBoxTableName";
            this.textBoxTableName.Size = new System.Drawing.Size(158, 20);
            this.textBoxTableName.TabIndex = 21;
            this.textBoxTableName.TextChanged += new System.EventHandler(this.textBoxTableName_TextChanged);
            // 
            // comboBoxOffice
            // 
            this.comboBoxOffice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOffice.FormattingEnabled = true;
            this.comboBoxOffice.Items.AddRange(new object[] {
            "Generic List",
            "Appointment",
            "Contact",
            "Task",
            "Post"});
            this.comboBoxOffice.Location = new System.Drawing.Point(105, 89);
            this.comboBoxOffice.Name = "comboBoxOffice";
            this.comboBoxOffice.Size = new System.Drawing.Size(157, 21);
            this.comboBoxOffice.TabIndex = 20;
            this.comboBoxOffice.SelectedIndexChanged += new System.EventHandler(this.comboBoxOffice_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Office Type";
            // 
            // comboBoxTitleField
            // 
            this.comboBoxTitleField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTitleField.FormattingEnabled = true;
            this.comboBoxTitleField.Location = new System.Drawing.Point(105, 64);
            this.comboBoxTitleField.Name = "comboBoxTitleField";
            this.comboBoxTitleField.Size = new System.Drawing.Size(157, 21);
            this.comboBoxTitleField.TabIndex = 18;
            this.comboBoxTitleField.SelectedIndexChanged += new System.EventHandler(this.comboBoxTitleField_SelectedIndexChanged);
            this.comboBoxTitleField.SelectedValueChanged += new System.EventHandler(this.comboBoxTitleField_SelectedValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Title Field";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Display Name*";
            // 
            // textBoxTableDisplayName
            // 
            this.textBoxTableDisplayName.Location = new System.Drawing.Point(105, 40);
            this.textBoxTableDisplayName.Name = "textBoxTableDisplayName";
            this.textBoxTableDisplayName.Size = new System.Drawing.Size(158, 20);
            this.textBoxTableDisplayName.TabIndex = 15;
            this.textBoxTableDisplayName.TextChanged += new System.EventHandler(this.textBoxTableDisplayName_TextChanged);
            // 
            // checkBoxCreate
            // 
            this.checkBoxCreate.AutoSize = true;
            this.checkBoxCreate.Location = new System.Drawing.Point(105, 175);
            this.checkBoxCreate.Name = "checkBoxCreate";
            this.checkBoxCreate.Size = new System.Drawing.Size(140, 17);
            this.checkBoxCreate.TabIndex = 14;
            this.checkBoxCreate.Text = "Create Create Operation";
            this.checkBoxCreate.UseVisualStyleBackColor = true;
            this.checkBoxCreate.CheckedChanged += new System.EventHandler(this.checkBoxCreate_CheckedChanged);
            // 
            // checkBoxDelete
            // 
            this.checkBoxDelete.AutoSize = true;
            this.checkBoxDelete.Location = new System.Drawing.Point(105, 155);
            this.checkBoxDelete.Name = "checkBoxDelete";
            this.checkBoxDelete.Size = new System.Drawing.Size(140, 17);
            this.checkBoxDelete.TabIndex = 13;
            this.checkBoxDelete.Text = "Create Delete Operation";
            this.checkBoxDelete.UseVisualStyleBackColor = true;
            this.checkBoxDelete.CheckedChanged += new System.EventHandler(this.checkBoxDelete_CheckedChanged);
            // 
            // checkBoxUpdate
            // 
            this.checkBoxUpdate.AutoSize = true;
            this.checkBoxUpdate.Location = new System.Drawing.Point(105, 135);
            this.checkBoxUpdate.Name = "checkBoxUpdate";
            this.checkBoxUpdate.Size = new System.Drawing.Size(144, 17);
            this.checkBoxUpdate.TabIndex = 12;
            this.checkBoxUpdate.Text = "Create Update Operation";
            this.checkBoxUpdate.UseVisualStyleBackColor = true;
            this.checkBoxUpdate.CheckedChanged += new System.EventHandler(this.checkBoxUpdate_CheckedChanged);
            // 
            // checkBoxRead
            // 
            this.checkBoxRead.AutoSize = true;
            this.checkBoxRead.Location = new System.Drawing.Point(105, 115);
            this.checkBoxRead.Name = "checkBoxRead";
            this.checkBoxRead.Size = new System.Drawing.Size(135, 17);
            this.checkBoxRead.TabIndex = 11;
            this.checkBoxRead.Text = "Create Read Operation";
            this.checkBoxRead.UseVisualStyleBackColor = true;
            this.checkBoxRead.CheckedChanged += new System.EventHandler(this.checkBoxRead_CheckedChanged);
            // 
            // panelDesign
            // 
            this.panelDesign.Controls.Add(this.toolStrip1);
            this.panelDesign.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDesign.Location = new System.Drawing.Point(0, 0);
            this.panelDesign.Name = "panelDesign";
            this.panelDesign.Size = new System.Drawing.Size(613, 24);
            this.panelDesign.TabIndex = 14;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAddEntity,
            this.toolStripButtonDeleteEntity,
            this.toolStripSeparator1,
            this.toolStripButtonAddField,
            this.toolStripButtonDeleteField,
            this.toolStripSeparator2,
            this.buttonUp,
            this.buttonDown});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(613, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonAddEntity
            // 
            this.toolStripButtonAddEntity.Image = global::SPALM.SPSF.Library.Properties.Resources.plus;
            this.toolStripButtonAddEntity.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddEntity.Name = "toolStripButtonAddEntity";
            this.toolStripButtonAddEntity.Size = new System.Drawing.Size(77, 22);
            this.toolStripButtonAddEntity.Text = "Add Entity";
            this.toolStripButtonAddEntity.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButtonDeleteEntity
            // 
            this.toolStripButtonDeleteEntity.Image = global::SPALM.SPSF.Library.Properties.Resources.cross;
            this.toolStripButtonDeleteEntity.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDeleteEntity.Name = "toolStripButtonDeleteEntity";
            this.toolStripButtonDeleteEntity.Size = new System.Drawing.Size(89, 22);
            this.toolStripButtonDeleteEntity.Text = "Delete Entity";
            this.toolStripButtonDeleteEntity.Click += new System.EventHandler(this.toolStripButtonDeleteEntity_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonAddField
            // 
            this.toolStripButtonAddField.Image = global::SPALM.SPSF.Library.Properties.Resources.plus;
            this.toolStripButtonAddField.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddField.Name = "toolStripButtonAddField";
            this.toolStripButtonAddField.Size = new System.Drawing.Size(71, 22);
            this.toolStripButtonAddField.Text = "Add Field";
            this.toolStripButtonAddField.Click += new System.EventHandler(this.toolStripButtonAddField_Click);
            // 
            // toolStripButtonDeleteField
            // 
            this.toolStripButtonDeleteField.Image = global::SPALM.SPSF.Library.Properties.Resources.cross;
            this.toolStripButtonDeleteField.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDeleteField.Name = "toolStripButtonDeleteField";
            this.toolStripButtonDeleteField.Size = new System.Drawing.Size(83, 22);
            this.toolStripButtonDeleteField.Text = "Delete Field";
            this.toolStripButtonDeleteField.Click += new System.EventHandler(this.toolStripButtonDeleteField_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // buttonUp
            // 
            this.buttonUp.Image = ((System.Drawing.Image)(resources.GetObject("buttonUp.Image")));
            this.buttonUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(69, 22);
            this.buttonUp.Text = "Move Up";
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            this.buttonDown.Image = ((System.Drawing.Image)(resources.GetObject("buttonDown.Image")));
            this.buttonDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(83, 22);
            this.buttonDown.Text = "Move Down";
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBoxInfo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 631);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.panel1.Size = new System.Drawing.Size(613, 50);
            this.panel1.TabIndex = 15;
            // 
            // textBoxInfo
            // 
            this.textBoxInfo.BackColor = System.Drawing.SystemColors.Info;
            this.textBoxInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxInfo.Location = new System.Drawing.Point(0, 5);
            this.textBoxInfo.Multiline = true;
            this.textBoxInfo.Name = "textBoxInfo";
            this.textBoxInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxInfo.Size = new System.Drawing.Size(613, 45);
            this.textBoxInfo.TabIndex = 0;
            // 
            // BCSTreeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelDesign);
            this.Controls.Add(this.panel1);
            this.Name = "BCSTreeControl";
            this.Size = new System.Drawing.Size(613, 681);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBoxField.ResumeLayout(false);
            this.groupBoxField.PerformLayout();
            this.groupBoxTable.ResumeLayout(false);
            this.groupBoxTable.PerformLayout();
            this.panelDesign.ResumeLayout(false);
            this.panelDesign.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBoxField;
        private System.Windows.Forms.CheckBox checkBoxRequired;
        private System.Windows.Forms.CheckBox checkBoxReadOnly;
        private System.Windows.Forms.CheckBox checkBoxShowInPicker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBoxTable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxTableDisplayName;
        private System.Windows.Forms.CheckBox checkBoxCreate;
        private System.Windows.Forms.CheckBox checkBoxDelete;
        private System.Windows.Forms.CheckBox checkBoxUpdate;
        private System.Windows.Forms.CheckBox checkBoxRead;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ComboBox comboBoxTitleField;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxOffice;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxOfficeProperty;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panelDesign;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddEntity;
        private System.Windows.Forms.ToolStripButton toolStripButtonDeleteEntity;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddField;
        private System.Windows.Forms.ToolStripButton toolStripButtonDeleteField;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxTableName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxFieldName;
        private System.Windows.Forms.CheckBox checkBoxIsKey;
        private System.Windows.Forms.ComboBox comboBoxDataType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBoxReferencedField;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox comboBoxReferencedEntity;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton buttonUp;
        private System.Windows.Forms.ToolStripButton buttonDown;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxInfo;
    }
}
