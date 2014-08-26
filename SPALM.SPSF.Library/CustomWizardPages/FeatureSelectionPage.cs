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
    public partial class FeatureSelectionPage : SPSFBaseWizardPage
    {
			private string AllowedScopes = "";
			private Project CurrentProject = null;

			private string SafeAppName = "";
			private string ParentFeatureName = "";
			private string ParentFeatureId = "";
			private string FeatureName = "";
			private string FeatureTitle = "";
			private string FeatureDescription = "";
			private string FeatureID = "";
			private string FeatureScope = "";
			private bool FeatureCreateReceiver = false;
			private bool FeatureHidden = false;
					

			public FeatureSelectionPage(WizardForm parent)
            : base(parent)
      {        
            InitializeComponent();
      }

            

			public override bool OnActivate()
			{
				base.OnActivate();
				ReadArguments();
				LoadFeatures();
				AddBrandingPanel();
				HasBeenActivated = true;
				return true;
			}

			private void LoadFeatures()
			{
				if (HasBeenActivated)
				{
					return;
				}

				IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
				CurrentProject = dictionaryService.GetValue("CurrentProject") as Project;
				AllowedScopes = dictionaryService.GetValue("AllowedScopes").ToString();
				
                List<string> allowedScopesList = new List<string>();
                if (AllowedScopes == "")
				{
					allowedScopesList.Add("Web");
					allowedScopesList.Add("Site");
					allowedScopesList.Add("WebApplication");
					allowedScopesList.Add("Farm");
				}
				else
				{
					char[] sep = new char[] { ';' };
					string[] scopes = AllowedScopes.Split(sep);
					foreach (string scope in scopes)
					{
						allowedScopesList.Add(scope);
					}
				}

                this.comboBox_Scopes.Items.Clear();
                foreach(string allowedScope in allowedScopesList)
                {
                    comboBox_Scopes.Items.Add(allowedScope);
				}
				comboBox_Scopes.SelectedIndex = 0;

				this.comboBox1.Items.Clear();
				comboBox1.Items.Add("<New...>");
				int selectedIndex = 0;
				if (CurrentProject != null)
				{
					List<NameValueItem> list = new List<NameValueItem>();
					Helpers.GetAllFeatures(list, CurrentProject);
					foreach (NameValueItem item in list)
					{
						FeatureItem newItem = new FeatureItem();
						newItem.FeatureName = item.Name;
						newItem.FeatureId = item.Value;
						newItem.FeatureScope = item.Description;
						int index = comboBox1.Items.Add(newItem);
                        if (ParentFeatureName != "")
                        {
                            if (item.Name.Equals(ParentFeatureName, StringComparison.InvariantCultureIgnoreCase))
                            {
                                selectedIndex = index;
                            }
                        }
                        else
                        {
                            //if feature has allowed scope then we select it first
                            if (allowedScopesList.Contains(newItem.FeatureScope))
                            {
                                selectedIndex = index;
                            }
                        }
					}
				}
				comboBox1.SelectedIndex = selectedIndex;

                UpdateArguments();
			}

			public override bool IsDataValid
			{
				get
				{
          string featureName = textBox_FeatureName.Text;
          if(!Helpers.IsValidNamespace(textBox_FeatureName.Text))
          {
            return false;
          }
					return base.IsDataValid;
				}
			}

			private bool CreateNewFeature()
			{
				if (comboBox1.SelectedItem != null)
				{
					if (comboBox1.SelectedItem is FeatureItem)
					{
						return false;
					}
				}
				return true;
			}

			private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
			{
				if (CreateNewFeature())
				{
					groupBox1.Enabled = true;
				}
				else
				{
					groupBox1.Enabled = false;
				}
				UpdateArguments();
			}

			protected void UpdateArguments()
			{
        this.Wizard.OnValidationStateChanged(this);

				IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
				if (CreateNewFeature())
				{
					dictionaryService.SetValue("ParentFeatureName", SafeAppName + "_" + this.textBox_FeatureName.Text);
					dictionaryService.SetValue("ParentFeatureId", this.FeatureID);
                    dictionaryService.SetValue("ParentFeatureScope", this.comboBox_Scopes.Text);
					dictionaryService.SetValue("CreateNewFeature", true);

					dictionaryService.SetValue("FeatureName", this.textBox_FeatureName.Text);
					dictionaryService.SetValue("FeatureTitle", this.textBox_FeatureTitle.Text);
					dictionaryService.SetValue("FeatureDescription", this.textBox_FeatureDescription.Text);
					dictionaryService.SetValue("FeatureScope", this.comboBox_Scopes.Text);
					dictionaryService.SetValue("FeatureHidden", this.checkBox_Hidden.Checked);
					dictionaryService.SetValue("FeatureCreateReceiver", this.checkBox_CreateReceiver.Checked);
				}
				else
				{
					//use the selected feature
					if (comboBox1.SelectedItem != null)
					{
						FeatureItem selectedFeature = comboBox1.SelectedItem as FeatureItem;

						dictionaryService.SetValue("ParentFeatureName", selectedFeature.FeatureName);
						dictionaryService.SetValue("ParentFeatureId", selectedFeature.FeatureId);
                        dictionaryService.SetValue("ParentFeatureScope", selectedFeature.FeatureScope);
					
						dictionaryService.SetValue("CreateNewFeature", false);
						dictionaryService.SetValue("FeatureCreateReceiver", false);

					}
				}			
			}

			protected void ReadArguments()
			{
				if (HasBeenActivated)
				{
					return;
				}
				//called once at the beginning by the system

				IDictionaryService dictionaryService = GetService(typeof(IDictionaryService)) as IDictionaryService;
				SafeAppName = GetStringValue(dictionaryService, "SafeAppName");
				ParentFeatureName = GetStringValue(dictionaryService, "ParentFeatureName");
				ParentFeatureId = GetStringValue(dictionaryService, "ParentFeatureId");
				FeatureID = GetStringValue(dictionaryService, "FeatureID");
				FeatureName = GetStringValue(dictionaryService, "FeatureName");
				FeatureTitle = GetStringValue(dictionaryService, "FeatureTitle");
				FeatureDescription = GetStringValue(dictionaryService, "FeatureDescription");
				FeatureScope = GetStringValue(dictionaryService, "FeatureScope");
				FeatureHidden = GetBooleanValue(dictionaryService, "FeatureHidden");
				FeatureCreateReceiver = GetBooleanValue(dictionaryService, "FeatureCreateReceiver");

				this.textBox_FeatureName.Text = FeatureName;
				this.textBox_FeatureTitle.Text = FeatureTitle;
				this.textBox_FeatureDescription.Text = FeatureDescription;
				checkBox_CreateReceiver.Checked = FeatureCreateReceiver;
				checkBox_Hidden.Checked = FeatureHidden;
			}

			private bool GetBooleanValue(IDictionaryService dictionaryService, string name)
			{
                try
                {
                    return Boolean.Parse(GetStringValue(dictionaryService, name));
                }
                catch (Exception)
                {
                }
                return false;
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
			private void groupBox1_Enter(object sender, EventArgs e)
			{

			}

			private void comboBox_Scopes_SelectedIndexChanged(object sender, EventArgs e)
			{
				UpdateArguments();
			}

			private void checkBox_CreateReceiver_CheckedChanged(object sender, EventArgs e)
			{
				UpdateArguments();
			}

			private void textBox_FeatureDescription_TextChanged(object sender, EventArgs e)
			{
				UpdateArguments();
			}
    }

		public class FeatureItem
		{
			public string FeatureName = "";
			public string FeatureId = "";
			public string FeatureScope = "";

			public override string ToString()
			{
				return FeatureName + " (" + FeatureScope  +")";
			}
		}
}