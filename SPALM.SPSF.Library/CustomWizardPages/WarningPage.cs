using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Practices.WizardFramework;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Xml;
using System.Collections.Generic;
using EnvDTE;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.ComponentModel;

namespace SPALM.SPSF.Library.CustomWizardPages
{
    /// <summary>
    /// Example of a class that is a custom wizard page
    /// </summary>
    /// 
    [ServiceDependency(typeof(IConfigurationService))]
    public partial class WarningPage : SPSFBaseWizardPage
    {
        private string WarningMessage = "";

        public WarningPage(WizardForm parent)
            : base(parent)
        {
            InitializeComponent();
        }

        public override bool OnActivate()
        {
            base.OnActivate();
            AddBrandingPanel();
            HasBeenActivated = true;

            IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
            WarningMessage = GetStringValue(dictionaryService, "WarningMessage");
            this.label1.Text = WarningMessage;
            return true;
        }

        private string GetStringValue(IDictionaryService dictionaryService, string name)
        {
            try
            {
                object v = dictionaryService.GetValue(name);
                if (v != null)
                {

                    return v.ToString();
                }
                return "";
            }
            catch (Exception)
            {
                throw new Exception("Could not find argument " + name);
            }
        }
    }   
}