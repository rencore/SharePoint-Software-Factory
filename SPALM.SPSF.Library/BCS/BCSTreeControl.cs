using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Collections;

namespace SPALM.SPSF.Library
{
    public partial class BCSTreeControl : UserControl
    {
        private string lastServer = "";
        private string lastDatabase = "";

        public delegate void BCSModelChangedHandler(BCSModel changedModel);
        public event BCSModelChangedHandler BCSModelChanged;

        public BCSTreeControl()
        {
            InitializeComponent();

            UpdateToolbar();
        }

        public void UpdateControls()
        {
            if (IsDesignMode)
            {
                toolStripButtonAddEntity.Visible = true;
                toolStripButtonAddField.Visible = true;
                toolStripButtonDeleteEntity.Visible = true;
                toolStripButtonDeleteField.Visible = true;
                treeView1.CheckBoxes = false;
            }
            else
            {
                toolStripButtonAddEntity.Visible = false;
                toolStripButtonAddField.Visible = false;
                toolStripButtonDeleteEntity.Visible = false;
                toolStripButtonDeleteField.Visible = false;
                treeView1.CheckBoxes = true;
            }
        }

        public void UpdateResult()
        {
            string warnings = "";
            string errors = "";
            bool dataIsValid = true;

            BCSModel model = new BCSModel();
            model.Entities.Clear();
            
            foreach (TreeNode tableNode in treeView1.Nodes)
            {
                if (IsNodeChecked(tableNode))
                {
                    BCSEntity generateEntity = tableNode.Tag as BCSEntity;
                    model.Entities.Add(generateEntity);
                    generateEntity.Fields.Clear();

                    foreach (TreeNode fieldNode in tableNode.Nodes)
                    {
                        if (IsNodeChecked(fieldNode))
                        {
                            BCSField generateField = fieldNode.Tag as BCSField;                      
                            generateEntity.Fields.Add(generateField);
                        }
                    }
                }
            }

            //check the model
            foreach (BCSEntity entity in model.Entities)
            {
                if (string.IsNullOrEmpty(entity.Name) || string.IsNullOrEmpty(entity.DisplayName))
                {
                    errors += "Error: The name or the display name of an entity is empty." + Environment.NewLine;
                    dataIsValid = false;
                }
                if (entity.Name.Contains(" "))
                {
                    errors += "Error: The name of entity '" + entity.Name + "' contains a space. This is not supported by the recipe." + Environment.NewLine;
                    dataIsValid = false;
                }

                bool hasKey = false;
                foreach (BCSField field in entity.Fields)
                {
                    SetAssocationName(field, entity);

                    if (field.IsKey)
                    {
                        hasKey = true;
                    }
                    if (string.IsNullOrEmpty(field.Name) || string.IsNullOrEmpty(field.DisplayName))
                    {
                        errors += "Error: The name or the display name of a field in entity '" + field.Name + "' is empty." + Environment.NewLine;
                        dataIsValid = false;
                    }
                    if (field.Name.Contains(" "))
                    {
                        errors += "Error: The name of field '" + field.Name + "' contains a space. This is not supported by the recipe." + Environment.NewLine;
                        dataIsValid = false;
                    }
                    if ((field.DataType == typeof(System.Int64)) || (field.DataType == typeof(System.Byte)) || (field.DataType == typeof(System.Byte[])))
                    {
                        warnings += "Error: The type of field '" + field.Name + "' is not supported in external lists." + Environment.NewLine;
                    }

                    if (IsDesignMode)
                    {
                        if (!string.IsNullOrEmpty(field.ReferencedEntity))
                        {
                            //check if Reference is valid
                            if (!model.IsValidAssociation(field))
                            {

                                errors += "Error: The association of '" + field.Name + "' to entity '" + field.ReferencedEntity + "' is not valid." + Environment.NewLine;
                                dataIsValid = false;
                            }
                        }
                    }
                }
                if (!hasKey)
                {
                    errors += "Error: The entity '" + entity.Name + "' contains to key field." + Environment.NewLine;
                    dataIsValid = false;
                }
                if (string.IsNullOrEmpty(entity.TitleField))
                {
                    warnings += "Warning: The entity '" + entity.Name + "' has no title field." + Environment.NewLine;
                }
            }

            textBoxInfo.Text = errors + warnings;
            
            if ((model.Entities.Count == 0) || !dataIsValid)
            {
                //if no elements, then return null
                model = null;
            }

            if (BCSModelChanged != null)
            {
                BCSModelChanged(model);
                // MessageBox.Show("Event raised");
            }
        }

        private bool IsNodeChecked(TreeNode tableNode)
        {
            if (IsDesignMode)
            {
                return true;
            }
            if (tableNode.Checked)
            {
                return true;
            }
            return false;
        }

        public void LoadData(string server, string database)
        {
            if (IsDesignMode)
            {
                return;
            }

            if ((server == lastServer) && (database == lastDatabase))
            {
                //same server, do nothing
                return;
            }

            lastServer = server;
            lastDatabase = database;

            string connString = "Provider=SQLOLEDB;Data Source=" + server + ";Initial Catalog=" + database + ";Integrated Security=SSPI;";
            treeView1.Nodes.Clear();

            string datatypes = "";

            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                try
                {
                    // open the connection to the database
                    conn.Open();

                    ArrayList arrViews = new ArrayList();
                    ArrayList arrTables = new ArrayList();

                    // Get the Tables
                    DataTable SchemaTable =
                    conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new
                    Object[] { null, null, null, "TABLE" });

                    // Store the table names in the class scoped array
                    // list of table names
                    for (int i = 0; i < SchemaTable.Rows.Count; i++)
                    {
                        BCSEntity entity = new BCSEntity();
                        entity.Name = MakeSafe(SchemaTable.Rows[i].ItemArray[2].ToString());
                        entity.DisplayName = SchemaTable.Rows[i].ItemArray[2].ToString();
                        entity.Schema =  MakeSafe(SchemaTable.Rows[i].ItemArray[1].ToString());

                        datatypes += "-------------------------------------------------------" + Environment.NewLine;
                        datatypes += entity.Name + Environment.NewLine;

                        TreeNode tableNode = new TreeNode();
                        tableNode.Text = entity.Name;
                        tableNode.ImageKey = "Table";
                        tableNode.SelectedImageKey = "Table";
                        tableNode.Tag = entity;
                        treeView1.Nodes.Add(tableNode);

                        ///////////////
                        List<string> autoIncs = new List<string>();
                        OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT SCHEMA_NAME( OBJECTPROPERTY( OBJECT_ID, 'SCHEMAID' )) AS SCHEMA_NAME, OBJECT_NAME( OBJECT_ID ) AS TABLE_NAME, NAME AS COLUMN_NAME FROM  SYS.COLUMNS WHERE COLUMNPROPERTY(OBJECT_ID, NAME, 'IsIdentity') = 1 AND SCHEMA_NAME( OBJECTPROPERTY( OBJECT_ID, 'SCHEMAID' )) = '" + entity.Schema + "' AND OBJECT_NAME( OBJECT_ID ) = '" + entity.Name + "'", conn);
                        DataTable tableSchema = new DataTable();
                        adapter.Fill(tableSchema);
                        foreach (DataRow dr in tableSchema.Rows)
                        {
                            autoIncs.Add(MakeSafe(dr["COLUMN_NAME"].ToString()));
                        }

                        DataTable mySchema = (conn as OleDbConnection).
                            GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys,
                            new Object[] { null, null, SchemaTable.Rows[i].ItemArray[2].ToString() });

                        int columnOrdinalForName = mySchema.Columns["COLUMN_NAME"].Ordinal;

                        List<string> keys = new List<string>();
                        foreach (DataRow r in mySchema.Rows)
                        {
                            keys.Add(MakeSafe(r.ItemArray[columnOrdinalForName].ToString()));
                        }

                        DataTable dtField = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new
                            object[] { null, null, SchemaTable.Rows[i].ItemArray[2].ToString() });

                        foreach (DataRow dr in dtField.Rows)
                        {
                            foreach (DataColumn columnName in dtField.Columns)
                            {
                                datatypes += dr[columnName].ToString() + "\t";
                            }

                            BCSField field = new BCSField();

                            field.Name = MakeSafe(dr["COLUMN_NAME"].ToString());

                            if (field.Name.StartsWith("Timestamp"))
                            {
                            }

                            field.OleDbType = ((OleDbType)dr[11]).ToString();
                            field.DbType = ConvertToDbType((OleDbType)dr[11], dr["CHARACTER_MAXIMUM_LENGTH"].ToString());
                            field.DataType = ConvertType((OleDbType)dr[11]);
                            field.DisplayName = dr["COLUMN_NAME"].ToString();
                            field.DataTypeSize = dr["CHARACTER_MAXIMUM_LENGTH"].ToString();
                            field.IsNullAllowed = Boolean.Parse(dr["IS_NULLABLE"].ToString());
                            if (autoIncs.Contains(field.Name))
                            {
                                field.IsIdentity = true;
                            }
                            if (keys.Contains(field.Name))
                            {
                                field.IsKey = true;
                            }

                            //exception: "Date" in SQL kommt als WChar bzw NChar(10) an
                            if (field.DbType == "NChar(10)")
                            {
                                field.DataType = typeof(System.DateTime);
                            }

                            datatypes += field.Name + "\t" + field.OleDbType + "\t" + field.DbType + "\t" + field.DataType + Environment.NewLine;

                            string columnNodeText = dr["COLUMN_NAME"].ToString();
                            //append type
                            columnNodeText += " (" + ((OleDbType)dr[11]).ToString();
                            if (!field.IsNullAllowed)
                            {
                                columnNodeText += ", NOT NULL";
                            }
                            columnNodeText += ")";

                            TreeNode columnNode = new TreeNode();
                            columnNode.Tag = field;
                            UpdateNodeText(columnNode);

                            tableNode.Nodes.Add(columnNode);
                        }

                        //get all assocations (where fields in this entity references other entities)
                        DataTable foreigns = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, new object[] { null, null, null, null, null, entity.Name });
                        foreach (DataRow foreign in foreigns.Rows)
                        {
                            //stay in same database
                            if (foreign["FK_TABLE_CATALOG"].ToString() == foreign["PK_TABLE_CATALOG"].ToString())
                            {
                                string fieldInCurrentEntity = foreign["FK_COLUMN_NAME"].ToString();
                                string referencedEntityName = foreign["PK_TABLE_NAME"].ToString();
                                string refrencedEntityField = foreign["PK_COLUMN_NAME"].ToString();

                                foreach (TreeNode fieldNode in tableNode.Nodes)
                                {
                                    BCSField field = fieldNode.Tag as BCSField;
                                    if (field.Name == fieldInCurrentEntity)
                                    {
                                        field.ReferencedEntity = referencedEntityName;
                                        field.ReferencedField = refrencedEntityField;
                                        SetAssocationName(field, entity);

                                        fieldNode.ImageKey = "Foreign";
                                        fieldNode.SelectedImageKey = "Foreign";
                                    }
                                }
                            }
                        }
                    }

                    // Get the Views
                    SchemaTable =
                        conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new
                    Object[] { null, null, null, "VIEW" });

                    // Store the view names in the class scoped array
                    // list of view names
                    for (int i = 0; i < SchemaTable.Rows.Count; i++)
                    {
                        arrViews.Add(SchemaTable.Rows[i].
                        ItemArray[2].ToString());
                    }
                }
                catch (Exception ex)
                {
                    // break and notify if the connection fails
                    MessageBox.Show(ex.Message, "Connection Error");
                }
            }

            string a = datatypes;
            string b = a;
        }

        private void SetAssocationName(BCSField field, BCSEntity entity)
        {
            field.AssociationName = field.ReferencedField + "Of" + field.ReferencedEntity + "To" + field.Name + "Of" + entity.Name;
            field.AssociationDisplayName = field.ReferencedEntity + " to " + entity.Name + "(" + field.Name + ")"; 
        }

        private string MakeSafe(string unsafecontent)
        {
            unsafecontent = unsafecontent.Replace(" ", "");
            return unsafecontent;
        }

        private string ConvertToDbType(OleDbType oleType, string maxsize)
        {
            string result = "";
            switch (oleType)
            {
                case OleDbType.Boolean: result = "Bit"; break;
                case OleDbType.UnsignedTinyInt: result = "TinyInt"; break;
                case OleDbType.VarBinary: result = "Binary"; break;
                case OleDbType.Binary: result = "Binary"; break;
                case OleDbType.Char: result = "Char"; break;
                case OleDbType.DBTimeStamp: result = "DateTime"; break;
                case OleDbType.Decimal: result = "Numeric"; break;
                case OleDbType.Double: result = "Double"; break;
                case OleDbType.Single: result = "Real"; break;
                case OleDbType.Guid: result = "UniqueIdentifier"; break;
                case OleDbType.SmallInt: result = "SmallInt"; break;
                case OleDbType.Integer: result = "Int"; break;
                case OleDbType.BigInt: result = "BigInt"; break;
                case OleDbType.VarWChar: result = "NVarChar"; break;
                case OleDbType.DBTime: result = "Time"; break;
                case OleDbType.UnsignedSmallInt: result = "Int"; break;
                case OleDbType.UnsignedInt: result = "BigInt"; break;
                case OleDbType.UnsignedBigInt: result = "Numeric"; break;
                case OleDbType.VarChar: result = "VarChar"; break;
                case OleDbType.DBDate: result = "Date"; break;
                case OleDbType.WChar: result = "NChar"; break;
                default: result = "VarChar"; break;
            }
            if (!string.IsNullOrEmpty(maxsize))
            {
                result += "(" + maxsize + ")";
            }
            return result;
        }

        private Type ConvertType(OleDbType oleType)
        {
            switch (oleType)
            {
                case OleDbType.BigInt:
                    return typeof(System.Int64);
                case OleDbType.Binary:
                    return typeof(System.Byte[]);
                case OleDbType.Boolean:
                    return typeof(System.Boolean);
                case OleDbType.BSTR:
                    return typeof(System.String);
                case OleDbType.Char:
                    return typeof(System.String);
                case OleDbType.Currency:
                    return typeof(System.Decimal);
                case OleDbType.Date:
                    return typeof(System.DateTime);
                case OleDbType.DBDate:
                    return typeof(System.DateTime);
                case OleDbType.DBTime:
                    return typeof(System.TimeSpan);
                case OleDbType.DBTimeStamp:
                    return typeof(System.DateTime);
                case OleDbType.Decimal:
                    return typeof(System.Decimal);
                case OleDbType.Double:
                    return typeof(System.Double);
                case OleDbType.Error:
                    return typeof(System.Exception);
                case OleDbType.Filetime:
                    return typeof(System.DateTime);
                case OleDbType.Guid:
                    return typeof(System.Guid);
                case OleDbType.IDispatch:
                    return typeof(System.Object);
                case OleDbType.Integer:
                    return typeof(System.Int32);
                case OleDbType.IUnknown:
                    return typeof(System.Object);
                case OleDbType.LongVarBinary:
                    return typeof(System.Byte);
                case OleDbType.LongVarChar:
                    return typeof(System.String);
                case OleDbType.LongVarWChar:
                    return typeof(System.String);
                case OleDbType.Numeric:
                    return typeof(System.Decimal);
                case OleDbType.PropVariant:
                    return typeof(System.Object);
                case OleDbType.Single:
                    return typeof(System.Single);
                case OleDbType.SmallInt:
                    return typeof(System.Int16);
                case OleDbType.TinyInt:
                    return typeof(System.SByte);
                case OleDbType.UnsignedBigInt:
                    return typeof(System.UInt64);
                case OleDbType.UnsignedInt:
                    return typeof(System.UInt32);
                case OleDbType.UnsignedSmallInt:
                    return typeof(System.UInt16);
                case OleDbType.UnsignedTinyInt:
                    return typeof(System.Byte);
                case OleDbType.VarBinary:
                    return typeof(System.Byte);
                case OleDbType.VarChar:
                    return typeof(System.String);
                case OleDbType.Variant:
                    return typeof(System.Object);
                case OleDbType.VarNumeric:
                    return typeof(System.Decimal);
                case OleDbType.VarWChar:
                    return typeof(System.String);
                case OleDbType.WChar:
                    return typeof(System.String);
            }

            return typeof(System.Object);
        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ShowDetailsForNode(e.Node);

            UpdateToolbar();
        }

        private void ShowDetailsForNode(TreeNode node)
        {
            groupBoxTable.Visible = false;
            groupBoxField.Visible = false;

            if (IsNodeChecked(node))
            {
                //if first level, then show Generator Options, if second then show field options
                if (node.Tag is BCSEntity)
                {
                    groupBoxTable.Visible = true;

                    BCSEntity entity = node.Tag as BCSEntity;
                    checkBoxCreate.Checked = entity.CreateCreateOperation;
                    checkBoxUpdate.Checked = entity.CreateUpdateOperation;
                    checkBoxDelete.Checked = entity.CreateDeleteOperation;
                    checkBoxRead.Checked = entity.CreateReadOperation;
                    textBoxTableDisplayName.Text = entity.DisplayName;
                    textBoxTableName.Text = entity.Name;

                    comboBoxTitleField.SelectedIndexChanged -= comboBoxTitleField_SelectedIndexChanged;
                    comboBoxTitleField.ValueMember = "Name";
                    comboBoxTitleField.DisplayMember = "Name";
                    List<string> objects = node.Nodes.OfType<TreeNode>().Select(tnode => tnode.Tag).Cast<BCSField>().Select(item => item.Name).ToList();
                    objects.Insert(0, "");
                    comboBoxTitleField.DataSource = objects;
                    comboBoxTitleField.SelectedItem = entity.TitleField;
                    comboBoxTitleField.SelectedIndexChanged += comboBoxTitleField_SelectedIndexChanged;

                    comboBoxOffice.SelectedIndexChanged -= comboBoxOffice_SelectedIndexChanged;
                    if (string.IsNullOrEmpty(entity.OfficeItemType))
                    {
                        comboBoxOffice.SelectedIndex = 0;
                    }
                    else
                    {
                        comboBoxOffice.SelectedItem = entity.OfficeItemType;
                    }
                    comboBoxOffice.SelectedIndexChanged += comboBoxOffice_SelectedIndexChanged;

                    if (IsDesignMode)
                    {
                        textBoxTableName.Enabled = true;
                    }
                    else
                    {
                        textBoxTableName.Enabled = false;
                    }
                }
                else if (node.Tag is BCSField)
                {
                    groupBoxField.Visible = true;

                    BCSField field = node.Tag as BCSField;
                    textBox1.Text = field.DisplayName;
                    textBoxFieldName.Text = field.Name;

                    checkBoxIsKey.Checked = field.IsKey;
                    checkBoxShowInPicker.Checked = field.ShowInPicker;
                    checkBoxRequired.Checked = field.IsRequired;
                    checkBoxReadOnly.Checked = field.IsReadOnly;

                    //set the available office fields for this node, depending on the type of the parent
                    SetAvailableOfficeProperties(node);

                    SetButtons(node);

                    comboBoxDataType.SelectedIndexChanged -= comboBoxDataType_SelectedIndexChanged;
                    comboBoxDataType.SelectedItem = field.DataType.ToString();
                    comboBoxDataType.SelectedIndexChanged += comboBoxDataType_SelectedIndexChanged;

                    //add all entities to the possible referenced entities
                    comboBoxReferencedEntity.Items.Clear();
                    comboBoxReferencedEntity.Items.Add("[None]");
                    foreach (TreeNode entityNode in treeView1.Nodes)
                    {
                        comboBoxReferencedEntity.Items.Add((entityNode.Tag as BCSEntity).Name);
                    }

                    if (!string.IsNullOrEmpty(field.ReferencedEntity))
                    {
                        comboBoxReferencedEntity.SelectedItem = field.ReferencedEntity;
                        if (!string.IsNullOrEmpty(field.ReferencedField))
                        {
                            comboBoxReferencedField.SelectedItem = field.ReferencedField;
                        }
                    }

                    if (IsDesignMode)
                    {
                        textBoxFieldName.Enabled = true;
                        checkBoxIsKey.Enabled = true;
                        comboBoxDataType.Enabled = true;
                        comboBoxReferencedEntity.Enabled = true;
                        comboBoxReferencedField.Enabled = true;
                    }
                    else
                    {
                        textBoxFieldName.Enabled = false;
                        checkBoxIsKey.Enabled = false;
                        comboBoxDataType.Enabled = false;
                        comboBoxReferencedEntity.Enabled = false;
                        comboBoxReferencedField.Enabled = false;
                    }
                }
            }
        }

        private void SetAvailableOfficeProperties(TreeNode node)
        {
            comboBoxOfficeProperty.Enabled = true;
            comboBoxOfficeProperty.Items.Clear();

            if (node.Tag is BCSField)
            {
                //get parent BCSEntity a mapped office type
                string mappedOfficeType = (node.Parent.Tag as BCSEntity).OfficeItemType;
                if (string.IsNullOrEmpty(mappedOfficeType))
                {
                    comboBoxOfficeProperty.Enabled = false;
                    return;
                }

                BCSField currentField = node.Tag as BCSField;
 
                //add default empty propertiy
                comboBoxOfficeProperty.Items.Add("Custom Property");

                //add string properties
                Dictionary<string, string> allProps = GetOfficeProperties(mappedOfficeType);
                foreach (string propName in allProps.Keys)
                {
                    if (allProps[propName] == currentField.DataType.ToString())
                    {
                        //are other fields of the parent already using the node, then do not provide it
                        bool addProp = true;
                        foreach (TreeNode otherFields in node.Parent.Nodes)
                        {
                            if (otherFields != node)
                            {
                                if ((otherFields.Tag as BCSField).OfficeProperty == propName)
                                {
                                    addProp = false;
                                }
                            }
                        }
                        if (addProp)
                        {
                            comboBoxOfficeProperty.Items.Add(propName);
                        }
                    }
                }

                comboBoxOfficeProperty.SelectedIndexChanged -= comboBoxOfficeProperty_SelectedIndexChanged;
                if (!string.IsNullOrEmpty(currentField.OfficeProperty))
                {
                    comboBoxOfficeProperty.SelectedItem = currentField.OfficeProperty;
                }
                else
                {
                    comboBoxOfficeProperty.SelectedIndex = 0;
                }
                comboBoxOfficeProperty.SelectedIndexChanged += comboBoxOfficeProperty_SelectedIndexChanged;
            }
        }

        private void SetButtons(TreeNode node)
        {
            buttonDown.Enabled = true;
            buttonUp.Enabled = true;

            if (IsLastNode(node))
            {
                buttonDown.Enabled = false;
            }
            if (IsFirstNode(node))
            {
                buttonUp.Enabled = false;
            }
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is BCSField)
            {
                //if child is checked, parent must be checked
                if (e.Node.Checked)
                {
                    if (!e.Node.Parent.Checked)
                    {
                        //also check parent
                        CheckNodeWithoutEvents(e.Node.Parent);
                    }
                }
                else
                {
                    //if not checked but field is notnullable, then show warning and check node again
                    if (!(e.Node.Tag as BCSField).IsNullAllowed)
                    {
                        if ((e.Node.Parent.Tag as BCSEntity).CreateCreateOperation || (e.Node.Parent.Tag as BCSEntity).CreateUpdateOperation)
                        {
                            MessageBox.Show(string.Format("Field '{0}' must be included in Update and Create operations because the field cannot be null in the data source", (e.Node.Tag as BCSField).Name));
                            CheckNodeWithoutEvents(e.Node);
                        }
                    }
                }
            }

            //check childs
            CheckChilds(e.Node);

            //select the checked node
            if (!e.Node.IsSelected)
            {
                treeView1.SelectedNode = e.Node;
            }

            //update the selected result
            ShowDetailsForNode(e.Node);

            UpdateResult();
        }

        private void CheckNodeWithoutEvents(TreeNode treeNode)
        {
            treeView1.AfterCheck -= treeView1_AfterCheck;
            treeNode.Checked = true;
            treeView1.AfterCheck += treeView1_AfterCheck;
        }

        private void CheckChilds(TreeNode treeNode)
        {
            treeView1.AfterCheck -= treeView1_AfterCheck;

            foreach (TreeNode child in treeNode.Nodes)
            {
                child.Checked = treeNode.Checked;
            }

            treeView1.AfterCheck += treeView1_AfterCheck;
        }

        private void checkBoxRead_CheckedChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSEntity)
                {
                    (treeView1.SelectedNode.Tag as BCSEntity).CreateReadOperation = (sender as CheckBox).Checked;
                }
            }
            UpdateResult();
        }

        private void checkBoxUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSEntity)
                {
                    (treeView1.SelectedNode.Tag as BCSEntity).CreateUpdateOperation = (sender as CheckBox).Checked;
                }
            }
            UpdateResult();
        }

        private void checkBoxDelete_CheckedChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSEntity)
                {
                    (treeView1.SelectedNode.Tag as BCSEntity).CreateDeleteOperation = (sender as CheckBox).Checked;
                }
            }
            UpdateResult();
        }

        private void checkBoxCreate_CheckedChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSEntity)
                {
                    (treeView1.SelectedNode.Tag as BCSEntity).CreateCreateOperation = (sender as CheckBox).Checked;
                }
            }
            UpdateResult();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSField)
                {
                    (treeView1.SelectedNode.Tag as BCSField).DisplayName = (sender as TextBox).Text;
                }
            }
            UpdateResult();
        }

        private void checkBoxShowInPicker_CheckedChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSField)
                {
                    (treeView1.SelectedNode.Tag as BCSField).ShowInPicker = (sender as CheckBox).Checked;
                }
            }
            UpdateResult();
        }

        private void checkBoxReadOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSField)
                {
                    (treeView1.SelectedNode.Tag as BCSField).IsReadOnly = (sender as CheckBox).Checked;
                }
            }
            UpdateResult();
        }

        private void checkBoxRequired_CheckedChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSField)
                {
                    (treeView1.SelectedNode.Tag as BCSField).IsRequired = (sender as CheckBox).Checked;
                }
            }
            UpdateResult();
        }

        private void textBoxTableDisplayName_TextChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSEntity)
                {
                    (treeView1.SelectedNode.Tag as BCSEntity).DisplayName = (sender as TextBox).Text;
                }
            }
            UpdateResult();
        }

        private void checkBoxIsTitle_CheckedChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSField)
                {
                    (treeView1.SelectedNode.Tag as BCSField).IsRequired = (sender as CheckBox).Checked;
                }
            }
            UpdateResult();
        }

        private void comboBoxTitleField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSEntity)
                {
                    (treeView1.SelectedNode.Tag as BCSEntity).TitleField = (sender as ComboBox).Text;
                }
            }
            UpdateResult();
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            MoveNodeUp(treeView1.SelectedNode);
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            MoveNodeDown(treeView1.SelectedNode);
        }

        void MoveNodeUp(TreeNode node)
        {
            TreeNode parentNode = node.Parent;
            int originalIndex = node.Index;
            if (IsFirstNode(node)) return;
            node.Remove();
            parentNode.Nodes.Insert(originalIndex - 1, node);
            parentNode.TreeView.SelectedNode = node;

            SetButtons(node);
        }

        void MoveNodeDown(TreeNode node)
        {
            TreeNode parentNode = node.Parent;
            int originalIndex = node.Index;
            if (IsLastNode(node)) return;

            node.Remove();
            parentNode.Nodes.Insert(originalIndex + 1, node);
            parentNode.TreeView.SelectedNode = node;

            SetButtons(node);
        }

        private bool IsFirstNode(TreeNode node)
        {
            if (node.Index == 0)
            {
                return true;
            }
            return false;
        }

        private bool IsLastNode(TreeNode node)
        {
            if (node.Index == (node.Parent.Nodes.Count - 1))
            {
                return true;
            }
            return false;
        }

        private void comboBoxTitleField_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSEntity)
                {
                    if ((sender as ComboBox).SelectedIndex == 0)
                    {
                        (treeView1.SelectedNode.Tag as BCSEntity).OfficeItemType = "";
                    }
                    else
                    {
                        (treeView1.SelectedNode.Tag as BCSEntity).OfficeItemType = (sender as ComboBox).Text;
                    }
                }
            }
            UpdateResult();
        }

        private void comboBoxOfficeProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSField)
                {
                    if ((sender as ComboBox).SelectedIndex == 0)
                    {
                        (treeView1.SelectedNode.Tag as BCSField).OfficeProperty = "";
                    }
                    else
                    {
                        (treeView1.SelectedNode.Tag as BCSField).OfficeProperty = (sender as ComboBox).Text;
                    }
                }
            }
            UpdateResult();
        }

        private Dictionary<string, string> GetOfficeProperties(string officeItemType)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();

            //Basic properties

            properties.Add("BillingInformation", "System.String");
            properties.Add("Body", "System.String");
            properties.Add("Categories", "System.String");
            properties.Add("CreationTime", "System.DateTime");
            properties.Add("Importance", "System.Int32");
            properties.Add("Mileage", "System.String");
            properties.Add("ReminderOverrideDefault", "System.Boolean");
            properties.Add("ReminderOverridePlaySound", "System.Boolean");
            properties.Add("ReminderOverrideSet", "System.Boolean");
            properties.Add("Sensitivity", "System.Int32");
            properties.Add("Subject", "System.String");

            if (officeItemType == "Appointment")
            {
                properties.Add("BusyStatus", "System.In32");
                properties.Add("End", "System.DateTime");
                properties.Add("Location", "System.String");
                properties.Add("ReminderMinutesBeforeStart", "System.Int32");
                properties.Add("Start", "System.DateTime");
            }

            if (officeItemType == "Contact")
            {
                properties.Add("Account", "System.String");
                properties.Add("AssistantName", "System.String");
                properties.Add("AssisantTelephoneNumber", "System.String");
                properties.Add("Anniversary", "System.DateTime");
                properties.Add("Birthday", "System.DateTime");
                properties.Add("Business2TelephoneNumber", "System.String");
                properties.Add("BusinessAddress", "System.String");
                properties.Add("BusinessAddressCity", "System.String");
                properties.Add("BusinessAddressCountry", "System.String");
                properties.Add("BusinessAddressPostalCode", "System.String");
                properties.Add("BusinessAddressPostOfficeBox", "System.String");
                properties.Add("BusinessAddressState", "System.String");
                properties.Add("BusinessAddressStreet", "System.String");
                properties.Add("BusinessFaxNumber", "System.String");
                properties.Add("BusinessHomePage", "System.String");
                properties.Add("BusinessTelephoneNumber", "System.String");
                properties.Add("CallbackTelephoneNumber", "System.String");
                properties.Add("CarTelephoneNumber", "System.String");
                properties.Add("CompanyMainTelephoneNumber", "System.String");
                properties.Add("CompanyName", "System.String");
                properties.Add("CustomerId", "System.String");
                properties.Add("Department", "System.String");
                properties.Add("Email1Address", "System.String");
                properties.Add("Email1DisplayName", "System.String");
                properties.Add("Email1AddressType", "System.String");
                properties.Add("Email2Address", "System.String");
                properties.Add("Email2DisplayName", "System.String");
                properties.Add("Email2AddressType", "System.String");
                properties.Add("Email3Address", "System.String");
                properties.Add("Email3DisplayName", "System.String");
                properties.Add("Email3AddressType", "System.String");
                properties.Add("FileAs", "System.String");
                properties.Add("FirstName", "System.String");
                properties.Add("FullName", "System.String");
                properties.Add("Gender", "System.Int32");
                properties.Add("GovernmentIDNumber", "System.String");
                properties.Add("Home2TelephoneNumber", "System.String");
                properties.Add("HomeAddress", "System.String");
                properties.Add("HomeAddressCity", "System.String");
                properties.Add("HomeAddressCountry", "System.String");
                properties.Add("HomeAddressPostalCode", "System.String");
                properties.Add("HomeAddressPostOfficeBox", "System.String");
                properties.Add("HomeAddressState", "System.String");
                properties.Add("HomeAddressStreet", "System.String");
                properties.Add("HomeFaxNumber", "System.String");
                properties.Add("HomeTelephoneNumber", "System.String");
                properties.Add("IMAddress", "System.String");
                properties.Add("Initials", "System.String");
                properties.Add("JobTitle", "System.String");
                properties.Add("LastName", "System.String");
                properties.Add("MailingAddress", "System.String");
                properties.Add("MailingAddressCity", "System.String");
                properties.Add("MailingAddressCountry", "System.String");
                properties.Add("MailingAddressPostalCode", "System.String");
                properties.Add("MailingAddressPostOfficeBox", "System.String");
                properties.Add("MailingAddressState", "System.String");
                properties.Add("MailingAddressStreet", "System.String");
                properties.Add("ManagerName", "System.String");
                properties.Add("MiddleName", "System.String");
                properties.Add("MobileTelephoneNumber", "System.String");
                properties.Add("NickName", "System.String");
                properties.Add("OfficeLocation", "System.String");
                properties.Add("OrganizationalIDNumber", "System.String");
                properties.Add("OtherAddress", "System.String");
                properties.Add("OtherAddressPostalCode", "System.String");
                properties.Add("OtherAddressPostOfficeBox", "System.String");
                properties.Add("OtherAddressState", "System.String");
                properties.Add("OtherAddressStreet", "System.String");
                properties.Add("OtherFaxNumber", "System.String");
                properties.Add("OtherTelephoneNumber", "System.String");
                properties.Add("PagerNumber", "System.String");
                properties.Add("PrimaryTelephoneNumber", "System.String");
                properties.Add("Profession", "System.String");
                properties.Add("ReferredBy", "System.String");
                properties.Add("ReminderTime", "System.DateTime");
                properties.Add("Spouse", "System.String");
                properties.Add("Suffix", "System.String");
                properties.Add("TelexNumber", "System.String");
                properties.Add("Title", "System.String");
                properties.Add("TTYTDDTelephoneNumber", "System.String");
                properties.Add("WebPage", "System.String");
                properties.Add("YomiCompanyName", "System.String");
                properties.Add("YomiFirstName", "System.String");
                properties.Add("YomiLastName", "System.String");
            }
            if (officeItemType == "Task")
            {

                properties.Add("ActualWork", "System.Int32");
                properties.Add("Complete", "System.Boolean");
                properties.Add("ContactNames", "System.String");
                properties.Add("DateCompleted", "System.DateTime");
                properties.Add("DueDate", "System.DateTime");
                properties.Add("PercentComplete", "System.Double");
                properties.Add("ReminderTime", "System.DateTime");
                properties.Add("StartDate", "System.DateTime");
                properties.Add("Status", "System.Int32");
                properties.Add("TotalWork", "System.Int32");
            }

            return properties;
        }

        private bool _IsDesignMode = false;
        public bool IsDesignMode
        {
            get
            {
                return _IsDesignMode;
            }
            set
            {
                _IsDesignMode = value;
                UpdateControls();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            int index = treeView1.Nodes.Count + 1;
            BCSEntity entity = new BCSEntity();
            entity.Name = "Entity" + index.ToString();
            entity.DisplayName = "Entity " + index.ToString();
            entity.Schema = "";

            TreeNode tableNode = new TreeNode();
            tableNode.Text = entity.Name;
            tableNode.ImageKey = "Table";
            tableNode.SelectedImageKey = "Table";
            tableNode.Tag = entity;
            treeView1.Nodes.Add(tableNode);

            treeView1.SelectedNode = tableNode;

            UpdateToolbar();

            UpdateResult();
        }

        private void toolStripButtonDeleteEntity_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSEntity)
                {
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                    UpdateToolbar();
                }
            }
            UpdateResult();
        }

        private void toolStripButtonDeleteField_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSField)
                {
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                    UpdateToolbar();
                }
            }
            UpdateResult();
        }

        private void toolStripButtonAddField_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSEntity)
                {
                    TreeNode tableNode = treeView1.SelectedNode;

                    BCSField field = new BCSField();

                    int index = tableNode.Nodes.Count + 1;

                    field.Name = "Field" + index.ToString();
                    field.DisplayName = "Field " + index.ToString();

                    field.OleDbType = null;
                    field.DbType = null;
                    field.DataType = typeof(System.String);
                    
                    field.DataTypeSize = null;
                    field.IsNullAllowed = false;
                    field.IsIdentity = false;
                    field.IsKey = false;

                    TreeNode columnNode = new TreeNode();
                    columnNode.Text = field.Name;
                    columnNode.ImageKey = "Field";
                    columnNode.SelectedImageKey = "Field";
                    if (field.IsKey)
                    {
                        columnNode.ImageKey = "Key";
                        columnNode.SelectedImageKey = "Key";
                    }
                    columnNode.Tag = field;
                    tableNode.Nodes.Add(columnNode);

                    treeView1.SelectedNode = columnNode;
                }
            }
            UpdateToolbar();
            UpdateResult();
        }

        private void textBoxTableName_TextChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSEntity)
                {
                    (treeView1.SelectedNode.Tag as BCSEntity).Name = (sender as TextBox).Text;
                    UpdateNodeText(treeView1.SelectedNode);
                }
            }
            UpdateResult();
        }

        private void UpdateNodeText(TreeNode treeNode)
        {
            //TODO
            if (treeNode.Tag is BCSEntity)
            {
                treeNode.Text = (treeNode.Tag as BCSEntity).Name;
            }
            else if (treeNode.Tag is BCSField)
            {
                BCSField field = treeNode.Tag as BCSField;

                treeNode.Text = field.Name;
                treeNode.ImageKey = "Field";
                treeNode.SelectedImageKey = "Field";
                if (field.IsKey)
                {
                    treeNode.ImageKey = "Key";
                    treeNode.SelectedImageKey = "Key";
                }
                if (!string.IsNullOrEmpty(field.ReferencedEntity) && !string.IsNullOrEmpty(field.ReferencedField))
                {
                    treeNode.ImageKey = "Foreign";
                    treeNode.SelectedImageKey = "Foreign";
                }
            }
        }

        private void textBoxFieldName_TextChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSField)
                {
                    (treeView1.SelectedNode.Tag as BCSField).Name = (sender as TextBox).Text;
                    UpdateNodeText(treeView1.SelectedNode);
                }
            }
            UpdateResult();
        }

        private void checkBoxIsKey_CheckedChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSField)
                {
                    (treeView1.SelectedNode.Tag as BCSField).IsKey = (sender as CheckBox).Checked;
                    UpdateNodeText(treeView1.SelectedNode);
                }
            }
            UpdateResult();
        }

        private void comboBoxDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSField)
                {
                    (treeView1.SelectedNode.Tag as BCSField).DataType = Type.GetType((sender as ComboBox).Text);
                }
            }
            UpdateResult();
        }

        private void UpdateToolbar()
        {
            TreeNode node = treeView1.SelectedNode;

            toolStripButtonDeleteEntity.Enabled = false;
            toolStripButtonAddField.Enabled = false;
            toolStripButtonDeleteField.Enabled = false;
            buttonDown.Enabled = false;
            buttonUp.Enabled = false;

            if (node != null)
            {
                //if first level, then show Generator Options, if second then show field options
                if (node.Tag is BCSEntity)
                {                    
                    toolStripButtonDeleteEntity.Enabled = true;
                    toolStripButtonAddField.Enabled = true;
                    toolStripButtonDeleteField.Enabled = false;
                }
                else if (node.Tag is BCSField)
                {                    
                    toolStripButtonDeleteEntity.Enabled = false;
                    toolStripButtonAddField.Enabled = false;
                    toolStripButtonDeleteField.Enabled = true;
                    buttonDown.Enabled = true;
                    buttonUp.Enabled = true;
                }
            }
        }

        private void comboBoxReferencedEntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSField)
                {
                    comboBoxReferencedField.Items.Clear();
                    if(comboBoxReferencedEntity.SelectedIndex == 0)
                    {
                        (treeView1.SelectedNode.Tag as BCSField).ReferencedEntity = "";
                        (treeView1.SelectedNode.Tag as BCSField).ReferencedField = "";
                        comboBoxReferencedField.Enabled = false;
                        UpdateNodeText(treeView1.SelectedNode);
                        return;
                    }
                                        
                    string referencedEntity = comboBoxReferencedEntity.Text;
                    (treeView1.SelectedNode.Tag as BCSField).ReferencedEntity = referencedEntity;
                    foreach (TreeNode entityNode in treeView1.Nodes)
                    {
                        if ((entityNode.Tag as BCSEntity).Name == referencedEntity)
                        {
                            foreach (TreeNode fieldNode in entityNode.Nodes)
                            {
                                comboBoxReferencedField.Items.Add((fieldNode.Tag as BCSField).Name);
                            }
                        }
                    }
                    comboBoxReferencedField.Enabled = true;

                    UpdateNodeText(treeView1.SelectedNode);
                }
            }
            UpdateResult();
        }

        private void comboBoxReferencedField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BCSField)
                {
                    (treeView1.SelectedNode.Tag as BCSField).ReferencedField = (sender as ComboBox).Text;
                    UpdateNodeText(treeView1.SelectedNode);
                }
            }
            UpdateResult();
        }
    }
}
