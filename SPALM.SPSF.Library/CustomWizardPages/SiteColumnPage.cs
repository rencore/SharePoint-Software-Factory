using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Practices.WizardFramework;
using System.ComponentModel.Design;

namespace SPALM.SPSF.Library.CustomWizardPages
{
    /// <summary>
    /// Example of a class that is a custom wizard page
    /// </summary>
    public partial class SiteColumnPage : CustomWizardPage
    {
      public SiteColumnPage(WizardForm parent)
            : base(parent)
      {        
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            siteColumnWizard1.SchemaChanged += new SiteColumnWizard.SchemaChangedDelegate(siteColumnWizard1_SchemaChanged);
      }

      void siteColumnWizard1_SchemaChanged()
      {
        string SiteColumnSchema = "";
        SiteColumnSchema = siteColumnWizard1.UpdateXML();
        IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
        dictionaryService.SetValue("SiteColumnSchema", SiteColumnSchema);
      }            
    }
}