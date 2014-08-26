using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Practices.WizardFramework;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Xml;
using EnvDTE;
using SPALM.SPSF.SharePointBridge;
using System.Collections.Generic;

namespace SPALM.SPSF.Library.CustomWizardPages
{
    /// <summary>
    /// Example of a class that is a custom wizard page
    /// </summary>
    public partial class BCSSingleEntityPage : SPSFBaseWizardPage
    {
        private BCSEntity entity;
        private BCSModel bcsModel;

        public BCSSingleEntityPage(WizardForm parent)
            : base(parent)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
        }

        private void CreateInitialEntity()
        {
            bcsModel = new BCSModel();

            entity = new BCSEntity();
            bcsModel.Entities.Add(entity);

            BCSField fieldID = new BCSField();
            fieldID.DataType = typeof(System.Int32);
            fieldID.IsKey = true;
            fieldID.Name = "ID";
            fieldID.DisplayName = "ID";
            entity.Fields.Add(fieldID);

            BCSField fieldName = new BCSField();
            fieldName.DataType = typeof(System.String);
            fieldName.Name = "Name";
            fieldName.IsRequired = true;
            fieldName.DisplayName = "Name";
            entity.Fields.Add(fieldName);

            BCSField fieldProperty = new BCSField();
            fieldProperty.DataType = typeof(System.String);
            fieldProperty.Name = "Property1";
            fieldProperty.DisplayName = "Property 1";
            entity.Fields.Add(fieldProperty);
        }

        public override bool OnActivate()
        {
            base.OnActivate();
            AddBrandingPanel();

            if (!HasBeenActivated)
            {
                UpdateEntity();
                HasBeenActivated = true;
            }

            return true;
        }

        public override bool IsDataValid
        {
            get
            {
                try
                {
                    IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
                    if (dictionaryService.GetValue("BCSModel") == null)
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                }
                return base.IsDataValid;
            }
        }

        private void UpdateArgument()
        {
            IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
            if (bcsModel != null)
            {
                if (string.IsNullOrEmpty(entity.Name) || string.IsNullOrEmpty(entity.DisplayName))
                {
                    dictionaryService.SetValue("BCSModel", null);
                }
                else
                {
                    dictionaryService.SetValue("BCSModel", bcsModel);
                }
            }
            Wizard.OnValidationStateChanged(this);
        }


        private void UpdateEntity()
        {
            if (bcsModel == null)
            {
                CreateInitialEntity();
            }
            entity.Name = textBoxEntityName.Text;
            entity.DisplayName = textBoxEntityDisplayName.Text;
            entity.CreateReadOperation = checkBoxReadOperation.Checked;
            entity.CreateUpdateOperation = checkBoxUpdateOperation.Checked;
            entity.CreateDeleteOperation = checkBoxDeleteOperation.Checked;
            entity.CreateCreateOperation = checkBoxCreateOperation.Checked;

            UpdateArgument();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateEntity();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            UpdateEntity();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEntity();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEntity();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEntity();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEntity();
        }      
    }
}