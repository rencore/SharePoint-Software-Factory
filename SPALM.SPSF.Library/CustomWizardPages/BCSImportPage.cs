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
    public partial class BCSImportPage : SPSFBaseWizardPage
    {
        public BCSImportPage(WizardForm parent)
            : base(parent)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            bcsTreeControl1.BCSModelChanged += new BCSTreeControl.BCSModelChangedHandler(bcsTreeControl1_BCSModelChanged); 
        }

        void bcsTreeControl1_BCSModelChanged(BCSModel changedModel)
        {
            //MessageBox.Show("Event received");

            IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
            dictionaryService.SetValue("BCSModel", changedModel);

            Wizard.OnValidationStateChanged(this);
        }

        public override bool OnActivate()
        {
            base.OnActivate();
            AddBrandingPanel();
            HasBeenActivated = true;

            IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
            if ((dictionaryService.GetValue("BCSServer") != null) && (dictionaryService.GetValue("BCSDatabase") != null))
            {
                bcsTreeControl1.LoadData(dictionaryService.GetValue("BCSServer").ToString(), dictionaryService.GetValue("BCSDatabase").ToString());
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
    }
}