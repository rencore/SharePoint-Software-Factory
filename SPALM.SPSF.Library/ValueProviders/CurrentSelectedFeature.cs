using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using System.Windows.Forms;

namespace SPALM.SPSF.Library.ValueProviders
{
    [ServiceDependency(typeof(DTE))]
    public class CurrentSelectedFeature : ValueProvider
    {
        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
          if (currentValue != null)
          {
            // Do not assign a new value, and return false to flag that 
            // we don't want the current value to be changed.
            newValue = null;
            return false;
          }

            DTE service = (DTE)this.GetService(typeof(DTE));
            
            try
            {
              if (service.SelectedItems.Count > 0)
              {
                SelectedItem item = service.SelectedItems.Item(1);
                ProjectItem pitem = null;
                if (item is ProjectItem)
                {
                  pitem = ((ProjectItem)item);
                }
                else if (item.ProjectItem != null)
                {
                  pitem = item.ProjectItem;
                }


                Project selectedProject = Helpers.GetSelectedProject(service);
                ProjectItem featureFolder = null;
                
                try
                {
                    featureFolder = Helpers.GetFeatureFolder(selectedProject, false);
                }
                catch(Exception)
                {
                }

                if (pitem != null)
                {
                    //is the current select item a child of the feature folder???

                  string path = Helpers.GetFullPathOfProjectItem(pitem);
                  string featurefolderPath = Helpers.GetFullPathOfProjectItem(featureFolder);
                  if(path.StartsWith(featurefolderPath))
                  {
                    //ok we are below features, it is worth to search for the current feature
                    //Find the feature name in the path of the item
                    path = path.Substring(featurefolderPath.Length);

                    if (path.Contains("\\"))
                    {
                      path = path.Substring(0, path.IndexOf("\\"));
                    }
                  }
                  newValue = path;
                  return true;
                }
              }
            }
            catch (Exception)
            {
            }
            newValue = currentValue;
            return true;
        }
    }
}
