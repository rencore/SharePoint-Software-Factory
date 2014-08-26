using System;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using System.Collections.Specialized;

namespace SPALM.SPSF.Library.Editors
{
    //returns a dropdown with all features in the current WSP Project
    [ServiceDependency(typeof(DTE))]
    public class FeatureNameEditor : UITypeEditor
    {

        public FeatureNameEditor()
            : base()
        {
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context,
            IServiceProvider provider, object value)
        {
            ListBox box = new ListBox();
            box.Sorted = true;
            
            DTE service = (DTE)provider.GetService(typeof(DTE));

            try
            {
                if (service.SelectedItems.Count > 0)
                {
                    SelectedItem item = service.SelectedItems.Item(1);
                    Project project = null;

                    if (item is Project)
                    {
                        project = (Project)item;
                    }
                    else if (item is ProjectItem)
                    {
                        project = ((ProjectItem)item).ContainingProject;
                    }
                    else
                    {                        
                        if (item.ProjectItem != null)
                        {
                            project = item.ProjectItem.ContainingProject;
                        }
                        if (item.Project != null)
                        {
                            project = item.Project;
                        }
                    }
                    if (project != null)
                    {

                        //geht all subfolders in folder 12/templates/features/ of the project
                        ProjectItem featuresFolder = null;
                        try
                        {
                          featuresFolder = Helpers.GetProjectItemByName(Helpers.GetProjectItemByName(Helpers.GetProjectItemByName(project.ProjectItems, Helpers.GetSharePointHive(project.DTE)).ProjectItems, "template").ProjectItems, "Features");
                        }
                        catch (Exception)
                        {
                        }

                        if (featuresFolder != null)
                        {
                          foreach (ProjectItem feature in featuresFolder.ProjectItems)
                          {
                            string scope = "";
                            ProjectItem featureXML = Helpers.GetProjectItemByName(feature.ProjectItems, "feature.xml");
                            if (featureXML != null)
                            {
                              string featurepath = Helpers.GetFullPathOfProjectItem(featureXML);

                              XmlDocument featuredoc = new XmlDocument();
                              featuredoc.Load(featurepath);

                              //what is the scope of the feature
                              XmlNamespaceManager featurensmgr = new XmlNamespaceManager(featuredoc.NameTable);
                              featurensmgr.AddNamespace("ns", "http://schemas.microsoft.com/sharepoint/");

                              XmlNode featurenode = featuredoc.SelectSingleNode("/ns:Feature", featurensmgr);
                              if (featurenode != null)
                              {
                                if (featurenode.Attributes["Scope"] != null)
                                {
                                  scope = featurenode.Attributes["Scope"].Value;
                                }
                              }
                            }
                            box.Items.Add(feature.Name + " (" + scope + ")");
                          }
                        }
                    }                    
                }                       
            }
            catch (Exception)
            {
                MessageBox.Show("There are no features in the project. Please create an empty feature first.");
            }

            IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            box.SelectedIndexChanged += delegate(object sender, EventArgs e)
            {
                editorService.CloseDropDown();
            };
            editorService.DropDownControl(box);

            
            if (box.SelectedItem != null)
            {
                string featurename = box.SelectedItem.ToString();
                featurename = featurename.Substring(0, featurename.IndexOf(" "));
                featurename.Trim();
                return featurename;
            }
            else
            {
                return null;
            }
            
        }

        public override bool IsDropDownResizable
        {
            get
            {
                return false;
            }
        }
    }
  
}
