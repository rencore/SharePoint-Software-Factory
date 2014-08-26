using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Microsoft.Practices.WizardFramework;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Drawing;
using Microsoft.Practices.ComponentModel;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Collections;
using Microsoft.Practices.Common.Services;
using Microsoft.Practices.RecipeFramework.Services;
using SPALM.SPSF.Library.Editors;

namespace SPALM.SPSF.Library
{

    [ServiceDependency(typeof(IServiceProvider)), ServiceDependency(typeof(IContainer))]
    public class NameValuePanel : ArgumentPanelTypeEditor
    {
        public string ExpectedType = "System.String";

        public object GetAService(Type serviceType)
        {
            return base.GetService(serviceType);
        }

        public void CancelWizard()
        {

            this.WizardPage.Wizard.Shown += new EventHandler(Wizard_Shown);
        }

        void Wizard_Shown(object sender, EventArgs e)
        {
            //service = GetService(typeof(IRecipeManagerService)) as IRecipeManagerService;
            //service.AfterRecipeExecution += new RecipeEventHandler(service_AfterRecipeExecution);

            //pkg = (Microsoft.Practices.RecipeFramework.GuidancePackage)GetService(typeof(Microsoft.Practices.RecipeFramework.Services.IExecutionService));

            //this.WizardPage.Wizard.DialogResult = DialogResult.Cancel;
            this.WizardPage.Wizard.OnCancel();
            // this.WizardPage.Wizard.Disposed += new EventHandler(Wizard_Disposed);
            //run other recipe

        }

        void Wizard_Disposed(object sender, EventArgs e)
        {
            //pkg.ExecuteFromTemplate("EmptyFeature", null);
        }

        void service_AfterRecipeExecution(object sender, RecipeEventArgs e)
        {
            //pkg.ExecuteFromTemplate("EmptyFeature", null);
        }
    }
    public class RuntimeServiceProvider : IServiceProvider, ITypeDescriptorContext
    {
        public NameValuePanel parent = null;
        public RuntimeServiceProvider(NameValuePanel parent)
        {
            this.parent = parent;
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            if (serviceType == typeof(IWindowsFormsEditorService))
                return new EditorService();
            return parent.GetAService(serviceType);
        }

        class EditorService : IWindowsFormsEditorService
        {
            public void DropDownControl(Control control) { }
            public void CloseDropDown() { }

            public System.Windows.Forms.DialogResult ShowDialog(Form dialog)
            {
                return dialog.ShowDialog();
            }
        }


        public void OnComponentChanged() { }
        public IContainer Container { get { return null; } }
        public bool OnComponentChanging()
        {
            return true; //um Änderungen zu behalten ..
        }
        public object Instance
        {
            get
            {
                return parent.ExpectedType;
            }
        }
        public PropertyDescriptor PropertyDescriptor
        {
            get { return null; }
        }
    }

    [ServiceDependency(typeof(IServiceProvider)), ServiceDependency(typeof(IContainer))]
    public class NameValueStringPanel : NameValuePanel
    {
        //public TextBox textbox;
        public Button button;
        public Object Value;
        public Panel panel;
        public TextBox textbox;

        public NameValueStringPanel()
        {
            this.InitializeComponent();
        }

        private int panelHeight = 22;
        public int PanelHeight
        {
            get
            {
                return panelHeight;
            }
            set
            {
                panelHeight = value;
            }
        }

        public override void BeginInit()
        {
            base.BeginInit();
        }

        public override void EndInit()
        {
            base.EndInit();
            if (!base.IsInitializing)
            {
                //this.textbox.Text = base.FieldConfig.Label;
                base.toolTip.SetToolTip(this.textbox, base.FieldConfig.Tooltip);
                IDictionaryService service = this.GetService(typeof(IDictionaryService)) as IDictionaryService;
                if (base.FieldConfig.ReadOnly)
                {
                    this.textbox.Enabled = false;
                }
            }
        }

        public void InitializeComponent()
        {
            this.panel = new Panel();
            this.textbox = new TextBox();
            this.button = new Button();

            this.BeginInit();
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i] is ValueEditor)
                {
                    ValueEditor vale = (ValueEditor)this.Controls[i];
                    this.Controls.RemoveAt(i);
                    break;
                }
            }

            base.SuspendLayout();

            this.panel.Anchor = AnchorStyles.Right | AnchorStyles.Left;
            this.panel.BorderStyle = BorderStyle.None;
            this.panel.Location = new Point(3, 0x12);
            this.panel.MinimumSize = new Size(100, 0x12);
            this.panel.Name = "valueEditor";
            this.panel.Size = new Size(200, panelHeight);

            textbox.Size = new Size(177, 12);
            textbox.Location = new Point(0, 0);
            textbox.Anchor = AnchorStyles.Right | AnchorStyles.Left;
            textbox.Name = "textbox";
            textbox.Leave += new EventHandler(textbox_Leave);
            panel.Controls.Add(this.textbox);

            button.Text = "...";
            button.Click += new EventHandler(button_Click);
            button.Size = new Size(20, 20);
            button.Location = new Point(180, 0);
            button.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panel.Controls.Add(this.button);

            base.invalidValuePictureBox.Location = new Point(0xca, 1);
            base.Controls.Add(panel);
            base.Size = new Size(0xda, PanelHeight + 25);
            this.EndInit();
            base.ResumeLayout(false);
        }

        void textbox_Leave(object sender, EventArgs e)
        {
            string currentTextValue = textbox.Text;

            if (currentTextValue == "")
            {
                UpdateValue(null);
            }

            //text could be
            //convert the value to namevalueitem
            if (this.ExpectedType == "NameValueItem")
            {
                //is the current value in format "0x01 (Item)", then we extract the name and value part
                if (currentTextValue.Contains("("))
                {
                    try
                    {
                        string value = currentTextValue.Substring(0, currentTextValue.IndexOf("(") - 1);
                        string name = currentTextValue.Substring(currentTextValue.IndexOf("(") + 1, currentTextValue.LastIndexOf(")") - currentTextValue.IndexOf("(") - 1);
                        NameValueItem newitem = new NameValueItem("", name, value);

                        //has the user changed the existing value
                        if (this.Value is NameValueItem)
                        {
                            NameValueItem existingItem = this.Value as NameValueItem;
                            if (existingItem.Value == newitem.Value)
                            {
                                //value has not changed, 
                                //do not update,
                                return;
                            }
                        }

                        UpdateValue(newitem);
                        return;
                    }
                    catch (Exception)
                    {
                        NameValueItem newitem = new NameValueItem("", textbox.Text, textbox.Text);
                        UpdateValue(newitem);
                        return;
                    }
                }

                NameValueItem newTextitem = new NameValueItem("", textbox.Text, textbox.Text);
                UpdateValue(newTextitem);
                return;
            }
            else
            {
                UpdateValue(textbox.Text);
                return;
            }
        }

        void textbox_TextChanged(object sender, EventArgs e)
        {

            //base.UpdateValue(textbox.Text);
            //base.SetValue(this.checkBox.Checked);
        }

        public void button_Click(object sender, EventArgs e)
        {
            SPALM.SPSF.Library.RuntimeServiceProvider serviceProvider = new SPALM.SPSF.Library.RuntimeServiceProvider(this);
            object returnvalue = this.EditorInstance.EditValue(serviceProvider, serviceProvider, Value);
            UpdateValue(returnvalue);
        }

        protected override void UpdateValue(object newValue)
        {
            string t = this.ExpectedType;
            Value = newValue;
            if (newValue == null)
            {
                textbox.Text = "";
            }
            else if (newValue is NameValueItem)
            {
                this.textbox.Text = ((NameValueItem)newValue).Value + " (" + ((NameValueItem)newValue).Name + ")";
            }
            else
            {
                textbox.Text = newValue.ToString();
            }
            if (this.ExpectedType == "System.String")
            {
                if (newValue != null)
                {
                    base.SetValue(newValue.ToString());
                }
                else
                {
                    base.SetValue(null);
                }
            }
            else if (this.ExpectedType == "System.Int32")
            {
                base.SetValue(GetInteger(newValue));
            }
            else
            {
                base.SetValue(newValue);
            }
        }

        public int GetInteger(object newValue)
        {
            try
            {
                return Int32.Parse(newValue.ToString());
            }
            catch (Exception)
            {
            }
            return 0;
        }
    }

    [ServiceDependency(typeof(IServiceProvider)), ServiceDependency(typeof(IContainer))]
    public class NameValueItemPanel : NameValueStringPanel
    {
        public NameValueItemPanel()
            : base()
        {
            this.ExpectedType = "NameValueItem";
        }
    }

    [ServiceDependency(typeof(IServiceProvider)), ServiceDependency(typeof(IContainer))]
    public class NameValueListPanel : NameValuePanel
    {
        //public TextBox textbox;
        public Button button;
        public Button removebutton;
        public Object Value;
        public Panel panel;
        public ListBox listbox;

        public NameValueListPanel()
        {
            this.InitializeComponent();
        }

        private int panelHeight = 90;
        public int PanelHeight
        {
            get
            {
                return panelHeight;
            }
            set
            {
                panelHeight = value;
            }
        }

        //public object GetAService(Type serviceType)
        //{
        //    return base.GetService(serviceType);
        //}

        public override void BeginInit()
        {
            base.BeginInit();
        }

        public override void EndInit()
        {
            base.EndInit();
            if (!base.IsInitializing)
            {
                //this.textbox.Text = base.FieldConfig.Label;
                base.toolTip.SetToolTip(this.listbox, base.FieldConfig.Tooltip);
                IDictionaryService service = this.GetService(typeof(IDictionaryService)) as IDictionaryService;
                if (base.FieldConfig.ReadOnly)
                {
                    this.listbox.Enabled = false;
                }
            }
        }

        public void InitializeComponent()
        {
            this.panel = new Panel();
            this.listbox = new ListBox();
            this.button = new Button();
            this.removebutton = new Button();

            this.BeginInit();
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i] is ValueEditor)
                {
                    ValueEditor vale = (ValueEditor)this.Controls[i];
                    this.Controls.RemoveAt(i);
                    break;
                }
            }

            base.SuspendLayout();

            this.panel.Anchor = AnchorStyles.Right | AnchorStyles.Left;
            this.panel.BorderStyle = BorderStyle.None;
            this.panel.Location = new Point(3, 0x12);
            this.panel.MinimumSize = new Size(100, 0x12);
            this.panel.Name = "valueEditor";
            this.panel.Size = new Size(200, panelHeight);

            listbox.Size = new Size(172, 75);
            listbox.Location = new Point(0, 0);
            listbox.Anchor = AnchorStyles.Right | AnchorStyles.Left;
            listbox.Name = "textbox";
            listbox.SelectionMode = SelectionMode.MultiSimple;
            listbox.DisplayMember = "DisplayName";
            listbox.SelectedIndexChanged += new EventHandler(listbox_SelectedIndexChanged);
            panel.Controls.Add(this.listbox);

            button.Text = "";
            button.Image = ResourceIcons.plus;
            button.Click += new EventHandler(button_Click);
            button.Size = new Size(26, 21);
            button.ImageAlign = ContentAlignment.MiddleCenter;
            button.Location = new Point(175, 0);
            button.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panel.Controls.Add(this.button);

            removebutton.Text = "";
            removebutton.Enabled = false;
            removebutton.Image = ResourceIcons.cross;
            removebutton.ImageAlign = ContentAlignment.MiddleCenter;
            removebutton.Click += new EventHandler(button_removeClick);
            removebutton.Size = new Size(26, 21);
            removebutton.Location = new Point(175, 23);
            removebutton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panel.Controls.Add(this.removebutton);

            base.invalidValuePictureBox.Location = new Point(0xca, 1);
            base.Controls.Add(panel);
            base.Size = new Size(0xda, PanelHeight + 25);
            this.EndInit();
            base.ResumeLayout(false);
        }

        void listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        public void button_Click(object sender, EventArgs e)
        {
            RuntimeServiceProvider serviceProvider = new RuntimeServiceProvider(this);
            object returnvalue = this.EditorInstance.EditValue(serviceProvider, serviceProvider, Value);
            UpdateValue(returnvalue);
        }

        private void UpdateButtons()
        {
            if (listbox.SelectedItem != null)
            {
                removebutton.Enabled = true;
            }
            else
            {
                removebutton.Enabled = false;
            }
        }

        public void button_removeClick(object sender, EventArgs e)
        {
            if (listbox.SelectedItem != null)
            {
                List<NameValueItem> temp = new List<NameValueItem>();

                foreach (object o in listbox.Items)
                {
                    if (o is NameValueItem)
                    {
                        if (listbox.SelectedItems.Contains(o))
                        {
                            //do nothing
                        }
                        else
                        {
                            temp.Add(o as NameValueItem);
                        }
                    }
                }

                //set the new value
                UpdateValue(temp.ToArray());
            }
        }

        protected override void UpdateValue(object newValue)
        {
            Type targettype = this.ValueType;
            Value = newValue;
            if (newValue == null)
            {
                this.listbox.Items.Clear();
            }
            else if (newValue is NameValueItem)
            {
                this.listbox.Items.Clear();
                listbox.Items.Add(newValue);
                //this.textbox.Text = ((NameValueItem)newValue).Value + "; " + ((NameValueItem)newValue).Name;
            }
            else if (newValue is NameValueItem[])
            {
                NameValueItem[] items = (NameValueItem[])newValue;
                this.listbox.Items.Clear();

                foreach (NameValueItem item in items)
                {
                    listbox.Items.Add(item);
                }
            }
            else
            {
                listbox.Items.Add(newValue.ToString());
            }
            base.SetValue(newValue);
        }
    }

    [ServiceDependency(typeof(IServiceProvider)), ServiceDependency(typeof(IContainer))]
    public class NameValueCheckBoxPanel : NameValueListViewPanel
    {
        public NameValueCheckBoxPanel()
            : base(true)
        {
        }
    }

    [ServiceDependency(typeof(EnvDTE.DTE)), ServiceDependency(typeof(IServiceProvider)), ServiceDependency(typeof(IDictionaryService)), ServiceDependency(typeof(ITypeResolutionService)), ServiceDependency(typeof(IValueInfoService))]
    public class NameValueListViewPanel : NameValuePanel
    {
        //public TextBox textbox;
        public Button button;
        public Object Value;
        public Panel panel;
        public ListView listview;
        private bool withCheckboxes = false;

        public NameValueListViewPanel()
        {
            this.InitializeComponent();
        }

        public NameValueListViewPanel(bool _withCheckboxes)
        {
            withCheckboxes = _withCheckboxes;
            this.InitializeComponent();
        }

        private int panelHeight = 200;
        public int PanelHeight
        {
            get
            {
                return panelHeight;
            }
            set
            {
                panelHeight = value;
            }
        }

        //public object GetAService(Type serviceType)
        //{
        //    return base.GetService(serviceType);
        //}

        public override void BeginInit()
        {
            base.BeginInit();
        }

        public override void EndInit()
        {
            base.EndInit();
            if (!base.IsInitializing)
            {
                //this.textbox.Text = base.FieldConfig.Label;
                //base.toolTip.SetToolTip(this.listbox, base.FieldConfig.Tooltip);
                IDictionaryService service = this.GetService(typeof(IDictionaryService)) as IDictionaryService;
                if (base.FieldConfig.ReadOnly)
                {
                    this.listview.Enabled = false;
                }
            }
        }

        public void InitializeComponent()
        {
            this.panel = new Panel();
            this.listview = new ListView();


            this.BeginInit();
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i] is ValueEditor)
                {
                    ValueEditor vale = (ValueEditor)this.Controls[i];
                    vale.Visible = false;
                    //this.Controls.RemoveAt(i);
                    break;
                }
            }

            base.SuspendLayout();

            this.panel.Anchor = AnchorStyles.Right | AnchorStyles.Left;
            this.panel.BorderStyle = BorderStyle.None;
            this.panel.Location = new Point(3, 0x12);
            this.panel.MinimumSize = new Size(100, 0x12);
            this.panel.Name = "valueEditor";
            this.panel.Size = new Size(200, panelHeight);

            if (withCheckboxes)
            {
                listview.CheckBoxes = true;
                listview.MultiSelect = true;
            }
            else
            {
                listview.CheckBoxes = false;
                listview.MultiSelect = false;
                listview.HideSelection = false;
            }
            listview.FullRowSelect = true;
            listview.View = View.Details;
            listview.Size = new Size(200, panelHeight - 60);
            listview.Dock = DockStyle.Fill;
            listview.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(listview_ItemSelectionChanged);
            listview.ItemChecked += new ItemCheckedEventHandler(listview_ItemChecked);
            listview.Location = new Point(0, 0);
            listview.SmallImageList = ImageListProvider.GetIcons();
            listview.LargeImageList = ImageListProvider.GetIcons();

            ColumnHeader header1 = listview.Columns.Add("Name");
            header1.Width = 250;
            ColumnHeader header2 = listview.Columns.Add("Description");
            header2.Width = 150;

            //listview.Anchor = AnchorStyles.Right | AnchorStyles.Left;
            listview.Name = "textbox";
            panel.Controls.Add(this.listview);

            listview.SizeChanged += new EventHandler(listview_SizeChanged);

            base.invalidValuePictureBox.Location = new Point(0xca, 1);
            base.Controls.Add(panel);
            base.Size = new Size(0xda, PanelHeight + 25);
            this.EndInit();
            base.ResumeLayout(false);
        }

        void listview_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            bool isValid = false;
            List<NameValueItem> items = new List<NameValueItem>();
            foreach (ListViewItem litem in listview.Items)
            {
                if (litem.Checked)
                {
                    isValid = true;
                    items.Add(litem.Tag as NameValueItem);
                }
                this.InternalSetValue(items.ToArray());
            }
            if (!isValid)
            {
                this.InternalSetValue(null);
            }
            return;
        }

        private void InternalSetValue(object newValue)
        {
            Type exptectedType = this.ValueEditor.ValueType;
            if (exptectedType == typeof(String))
            {
                if (newValue != null)
                {
                    if (newValue.GetType().IsArray)
                    {
                        string tempValue = "";
                        Array array = (Array)newValue;
                        for (int i = 0; i < array.Length; i++)
                        {
                            if (tempValue != "")
                            {
                                tempValue += ";";
                            }
                            tempValue += array.GetValue(i).ToString();
                        }
                        this.SetValue(tempValue);
                    }
                    else
                    {
                        this.SetValue(newValue.ToString());
                    }
                }
                else
                {
                    this.SetValue(newValue);

                }
            }
            else if (exptectedType == typeof(Int32))
            {
                this.SetValue(GetInteger(newValue));
            }
            else
            {
                //stimmt der typ des wert setzens mit dem erwarteten typ des feldes überein
                this.SetValue(newValue);
            }
        }

        public int GetInteger(object newValue)
        {
            try
            {
                return Int32.Parse(newValue.ToString());
            }
            catch (Exception)
            {
            }
            return 0;
        }

        void listview_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            bool isValid = false;
            if (listview.CheckBoxes)
            {
                return;
            }
            else
            {
                if (listview.SelectedItems.Count > 0)
                {
                    if (this.ValueEditor.ValueType == typeof(NameValueItem[]))
                    {
                        NameValueItem[] items = new NameValueItem[listview.SelectedItems.Count];
                        for (int i = 0; i < listview.SelectedItems.Count; i++)
                        {
                            isValid = true;
                            items[i] = listview.SelectedItems[i].Tag as NameValueItem;
                        }
                        this.InternalSetValue(items);
                        return;
                    }
                    else
                    {
                        object x = listview.SelectedItems[0].Tag;
                        if (x is NameValueItem)
                        {
                            NameValueItem nvi = x as NameValueItem;
                            if (this.ValueEditor.ValueType == typeof(NameValueItem))
                            {
                                this.InternalSetValue(nvi);
                                return;
                            }
                            else
                            {
                                this.InternalSetValue(nvi.Value);
                                return;
                            }
                        }
                        this.InternalSetValue(x.ToString());
                        return;
                    }
                }
            }
            if (!isValid)
            {
                this.InternalSetValue(null);
            }
        }

        void listview_SizeChanged(object sender, EventArgs e)
        {
            if (listview.Columns.Count == 2)
            {
                ColumnHeader header1 = listview.Columns[0];
                header1.Width = 250;
                ColumnHeader header2 = listview.Columns[1];
                header2.Width = listview.Width - 258;
            }
        }

        private bool dataLoaded = false;
        protected override void UpdateValue(object newValue)
        {
            if (this.ConverterInstance != null)
            {
                if (!dataLoaded)
                {
                    //selected items??
                    List<string> selectedStrings = new List<string>();
                    if (newValue != null)
                    {
                        if (newValue.GetType() == typeof(string))
                        {
                            if (!string.IsNullOrEmpty(newValue.ToString()))
                            {
                                string strValue = newValue.ToString();
                                if (strValue.Contains(";"))
                                {
                                    char[] sep = new char[] { ';' };
                                    foreach (string strSep in strValue.Split(sep))
                                    {
                                        if (strSep != "")
                                        {
                                            selectedStrings.Add(strSep);
                                        }
                                    }
                                }
                                else
                                {
                                    //nur 1 element drin
                                    selectedStrings.Add(strValue);
                                }
                            }
                        }
                    }

                    dataLoaded = true;
                    RuntimeServiceProvider serviceProvider = new RuntimeServiceProvider(this);
                    foreach (object o in this.ConverterInstance.GetStandardValues(serviceProvider))
                    {
                        if (o is NameValueItem)
                        {
                            NameValueItem nvi = o as NameValueItem;
                            ListViewItem lvi = new ListViewItem(nvi.Name);
                            lvi.Tag = nvi;
                            lvi.ImageKey = nvi.ItemType;
                            listview.Items.Add(lvi);

                            ListViewItem.ListViewSubItem slvi = new ListViewItem.ListViewSubItem();
                            slvi.Text = nvi.Description;
                            lvi.SubItems.Add(slvi);

                            if (ConverterInstance != null)
                            {
                                if (!ConverterInstance.IsValid(serviceProvider, nvi))
                                {
                                    lvi.ForeColor = Color.Gray;
                                }

                                //select a default value, if only one selection is allowed
                                if ((selectedStrings.Count == 0) && !withCheckboxes)
                                {
                                    if (listview.SelectedItems.Count == 0)
                                    {
                                        //ITypeDescriptorContext context = serviceProvider.GetService(typeof(ITypeDescriptorContext)) as ITypeDescriptorContext;

                                        if (ConverterInstance.IsValid(serviceProvider, nvi))
                                        {
                                            lvi.Selected = true;
                                        }
                                    }
                                }
                            }

                            if (selectedStrings.Contains(nvi.ToString()))
                            {
                                if (withCheckboxes)
                                {
                                    lvi.Checked = true;
                                }
                                else
                                {
                                    lvi.Selected = true;
                                }
                            }
                        }
                        else
                        {
                            NameValueItem nvi = new NameValueItem();
                            nvi.Name = o.ToString();
                            nvi.Value = o.ToString();

                            ListViewItem lvi = new ListViewItem(o.ToString());
                            lvi.Tag = nvi;
                            lvi.ImageKey = o.ToString();
                            listview.Items.Add(lvi);

                            if (selectedStrings.Contains(nvi.ToString()))
                            {
                                if (withCheckboxes)
                                {
                                    lvi.Checked = true;
                                }
                                else
                                {
                                    lvi.Selected = true;
                                }
                            }
                        }
                    }

                    //if only one item is available, go to the next wizard page, if there is only 1 visible control on the wizard page
                    if (listview.Items.Count == 1)
                    {
                        int controlcount = 0;
                        foreach (Control control in this.WizardPage.Controls)
                        {
                            if (control.Visible)
                            {
                                if (control.GetType() == typeof(Panel))
                                {
                                    //ignore the button panel
                                }
                                else
                                {
                                    controlcount++;
                                }
                            }
                        }
                        if (controlcount == 1)
                        {
                            //I'm the only one control on the wizard page 
                            if (this.WizardPage.Wizard.NextPage != null)
                            {
                                this.WizardPage.Wizard.OnNext();

                                //this.WizardPage.Wizard.GotoPage(this.WizardPage.Wizard.NextPageFromPage(this.WizardPage));
                            }
                        }
                    }
                }
            }

            base.SetValue(newValue);
        }
    }

    [ServiceDependency(typeof(EnvDTE.DTE)), ServiceDependency(typeof(IServiceProvider)), ServiceDependency(typeof(IDictionaryService)), ServiceDependency(typeof(ITypeResolutionService)), ServiceDependency(typeof(IValueInfoService))]
    public class NameValueDropDownPanel : NameValuePanel
    {
        //public TextBox textbox;
        public Button button;
        public Object Value;
        public Panel panel;
        public ComboBox dropDown;

        public NameValueDropDownPanel()
        {
            this.InitializeComponent();
        }

        private int panelHeight = 30;
        public int PanelHeight
        {
            get
            {
                return panelHeight;
            }
            set
            {
                panelHeight = value;
            }
        }

        //public object GetAService(Type serviceType)
        //{
        //    return base.GetService(serviceType);
        //}

        public override void BeginInit()
        {
            base.BeginInit();
        }

        public override void EndInit()
        {
            base.EndInit();
            if (!base.IsInitializing)
            {
                //this.textbox.Text = base.FieldConfig.Label;
                //base.toolTip.SetToolTip(this.listbox, base.FieldConfig.Tooltip);
                IDictionaryService service = this.GetService(typeof(IDictionaryService)) as IDictionaryService;
                if (base.FieldConfig.ReadOnly)
                {
                    this.dropDown.Enabled = false;
                }
            }
        }

        public void InitializeComponent()
        {
            this.panel = new Panel();
            this.dropDown = new ComboBox();


            this.BeginInit();
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i] is ValueEditor)
                {
                    ValueEditor vale = (ValueEditor)this.Controls[i];
                    vale.Visible = false;
                    //this.Controls.RemoveAt(i);
                    break;
                }
            }

            base.SuspendLayout();

            this.panel.Anchor = AnchorStyles.Right | AnchorStyles.Left;
            this.panel.BorderStyle = BorderStyle.None;
            this.panel.Location = new Point(3, 0x12);
            this.panel.MinimumSize = new Size(100, 0x12);
            this.panel.Name = "valueEditor";
            this.panel.Size = new Size(200, panelHeight);

            dropDown.DisplayMember = "DisplayName";
            dropDown.DropDownStyle = ComboBoxStyle.DropDown;
            dropDown.Size = new Size(200, 20);
            dropDown.SelectedIndexChanged += new EventHandler(dropDown_SelectedIndexChanged);
            dropDown.Location = new Point(0, 0);
            dropDown.Dock = DockStyle.Top;
            dropDown.Name = "textbox";
            dropDown.DropDown += new EventHandler(dropDown_DropDown);
            dropDown.Leave += new EventHandler(dropDown_Leave);
            panel.Controls.Add(this.dropDown);

            base.invalidValuePictureBox.Location = new Point(0xca, 1);
            base.Controls.Add(panel);
            base.Size = new Size(0xda, PanelHeight + 25);
            this.EndInit();
            base.ResumeLayout(false);
        }

        void dropDown_Leave(object sender, EventArgs e)
        {
            if (dropDown.SelectedItem is NameValueItem)
            {
                if (this.ExpectedType == "NameValueItem")
                {
                    UpdateValue(dropDown.SelectedItem as NameValueItem);
                }
                else
                {
                    UpdateValue((dropDown.SelectedItem as NameValueItem).Value);
                }
                return;
            }

            string currentTextValue = dropDown.Text;

            if (currentTextValue == "")
            {
                UpdateValue(null);
            }

            //text could be
            //convert the value to namevalueitem
            if (this.ExpectedType == "NameValueItem")
            {
                //is the current value in format "0x01 (Item)", then we extract the name and value part
                if (currentTextValue.Contains("("))
                {
                    try
                    {
                        string value = currentTextValue.Substring(0, currentTextValue.IndexOf("(") - 1);
                        string name = currentTextValue.Substring(currentTextValue.IndexOf("(") + 1, currentTextValue.LastIndexOf(")") - currentTextValue.IndexOf("(") - 1);
                        NameValueItem newitem = new NameValueItem("", name, value);
                        UpdateValue(newitem);
                        return;
                    }
                    catch (Exception)
                    {
                        NameValueItem newitem = new NameValueItem("", dropDown.Text, dropDown.Text);
                        UpdateValue(newitem);
                        return;
                    }
                }

                NameValueItem newTextitem = new NameValueItem("", dropDown.Text, dropDown.Text);
                UpdateValue(newTextitem);
                return;
            }
            else
            {
                UpdateValue(dropDown.Text);
                return;
            }
        }

        void dropDown_DropDown(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (this.ConverterInstance != null)
                {
                    if (!dataLoaded)
                    {
                        dropDown.Items.Clear();

                        //selected items??
                        dataLoaded = true;
                        RuntimeServiceProvider serviceProvider = new RuntimeServiceProvider(this);
                        foreach (object o in this.ConverterInstance.GetStandardValues(serviceProvider))
                        {
                            NameValueItem nvi = null;

                            if (o is NameValueItem)
                            {
                                nvi = o as NameValueItem;
                            }
                            else
                            {
                                nvi = new NameValueItem();
                                nvi.Name = o.ToString();
                                nvi.Value = o.ToString();

                            }
                            int pos = dropDown.Items.Add(nvi);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            this.Cursor = Cursors.Default;
        }

        void dropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            object x = dropDown.SelectedItem;
            if (x is NameValueItem)
            {
                NameValueItem nvi = x as NameValueItem;
                if (this.ValueEditor.ValueType == typeof(NameValueItem))
                {
                    this.InternalSetValue(nvi);
                    return;
                }
                else
                {
                    this.InternalSetValue(nvi.Value);
                    return;
                }
            }
            this.InternalSetValue(x.ToString());
            return;

            throw new NotImplementedException();
        }

        private void InternalSetValue(object newValue)
        {
            Type exptectedType = this.ValueEditor.ValueType;
            if (exptectedType == typeof(String))
            {
                if (newValue != null)
                {
                    if (newValue.GetType().IsArray)
                    {
                        string tempValue = "";
                        Array array = (Array)newValue;
                        for (int i = 0; i < array.Length; i++)
                        {
                            if (tempValue != "")
                            {
                                tempValue += ";";
                            }
                            tempValue += array.GetValue(i).ToString();
                        }
                        this.SetValue(tempValue);
                    }
                    else
                    {
                        this.SetValue(newValue.ToString());
                    }
                }
                else
                {
                    this.SetValue(newValue);
                }
            }
            else if (exptectedType == typeof(Int32))
            {
                this.SetValue(GetInteger(newValue));
            }
            else
            {
                //stimmt der typ des wert setzens mit dem erwarteten typ des feldes überein
                this.SetValue(newValue);
            }
        }

        public int GetInteger(object newValue)
        {
            try
            {
                return Int32.Parse(newValue.ToString());
            }
            catch (Exception)
            {
            }
            return 0;
        }

        private bool dataLoaded = false;
        protected override void UpdateValue(object newValue)
        {
            /*
            List<string> selectedStrings = new List<string>();
            if (newValue != null)
            {
                if (newValue.GetType() == typeof(string))
                {
                    if (newValue != "")
                    {
                        string strValue = newValue.ToString();
                        if (strValue.Contains(";"))
                        {
                            char[] sep = new char[] { ';' };
                            foreach (string strSep in strValue.Split(sep))
                            {
                                if (strSep != "")
                                {
                                    selectedStrings.Add(strSep);
                                }
                            }
                        }
                        else
                        {
                            //nur 1 element drin
                            selectedStrings.Add(strValue);
                        }
                    }
                }
            }
             * */

            if (!dataLoaded)
            {
                //add dummy item with the value
                dropDown.Items.Clear();
                if (newValue != null)
                {
                    if (newValue.GetType() == typeof(string))
                    {
                        if (newValue.ToString() != "")
                        {
                            NameValueItem nvi = new NameValueItem();
                            nvi.Name = newValue.ToString();
                            nvi.Value = newValue.ToString();
                            dropDown.Items.Add(nvi);
                            dropDown.SelectedIndex = 0;
                        }
                    }
                    else if (newValue.GetType() == typeof(NameValueItem))
                    {
                        dropDown.Items.Add(newValue);
                        dropDown.SelectedIndex = 0;
                    }
                }
            }
            else
            {
            }
            base.SetValue(newValue);
        }
    }
}