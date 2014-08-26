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
    [ServiceDependency(typeof(DTE))]
    public class MSBuildTargetEditor : UITypeEditor
    {

        public MSBuildTargetEditor() : base()
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
                    string targetsfilename = Helpers.GetFullPathOfProjectItem(item.ProjectItem);

                    
                    XmlDocument doc = new XmlDocument();
                    doc.Load(targetsfilename);

                    XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
                    manager.AddNamespace("ns", "http://schemas.microsoft.com/developer/msbuild/2003");
                    foreach(XmlNode targetnode in doc.SelectNodes("/ns:Project/ns:Target", manager))
                    {
                        if(targetnode.Attributes["Name"] != null)
                        {
                            box.Items.Add(targetnode.Attributes["Name"].Value);
                        } 
                    }
                }                       
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           

            IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            box.SelectedIndexChanged += delegate(object sender, EventArgs e)
            {
                editorService.CloseDropDown();
            };
            editorService.DropDownControl(box);

            
            if (box.SelectedItem != null)
            {
                return box.SelectedItem;
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
                return true;
            }
        }
    }
  
}
