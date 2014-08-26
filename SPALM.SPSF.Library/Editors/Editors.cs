using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using System.IO;
using System.Xml;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Services;

namespace SPALM.SPSF.Library.Editors
{
    [ServiceDependency(typeof(DTE))]
    public class BaseEditor : UITypeEditor
    {
        public static int SortByName(NameValueItem x, NameValueItem y)
        {
            return String.Compare(x.Name, y.Name);
        }

        public virtual List<NameValueItem> GetItems(DTE dte, IServiceProvider provider)
        {
            return new List<NameValueItem>();
        }

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public void SortItems(List<NameValueItem> _list)
        {
            _list.Sort(SortByName);
        }


        private ProjectItem GetProjectItemByName(ProjectItems pitems, string name)
        {
            foreach (ProjectItem pitem in pitems)
            {
                if (pitem.Name.ToUpper() == name.ToUpper())
                {
                    return pitem;
                }
            }
            throw new Exception("ProjectItem " + name + " not found");
        }

        /// <summary>
        /// Read RecipeParameters.xml from directory of Framework 
        /// </summary>
        /// <param name="dte"></param>
        /// <param name="_list"></param>
        /// <param name="internalXPath"></param>
        /// <param name="internalNamespace"></param>
        /// <param name="nodeHandler"></param>
        public void AddRecipeParameters(IServiceProvider provider, List<NameValueItem> _list, string internalXPath, string internalNamespace, XmlNodeHandler nodeHandler)
        {
            string configFileName = "RecipeParameters15.xml";

            try
            {
                DTE dte = (DTE)provider.GetService(typeof(DTE));
                string version = Helpers.GetSharePointVersion(dte);
                if (version != "")
                {
                    configFileName = "RecipeParameters" + version + ".xml";
                }
            }
            catch (Exception)
            {
            }

            IConfigurationService service = (IConfigurationService)provider.GetService(typeof(IConfigurationService));
            string basepath = service.BasePath;
            string configfilename = Path.Combine(basepath, configFileName);
            if (File.Exists(configfilename))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(configfilename);

                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("ns", internalNamespace);
                foreach (XmlNode node in doc.SelectNodes(internalXPath, nsmgr))
                {
                    foreach(NameValueItem nvitem in nodeHandler.GetNameValueItems(node, null, null))
                    {
                        if (nvitem != null)
                        {
                            _list.Add(nvitem);
                        }
                    }
                }
            }
        }

        


    }

    [ServiceDependency(typeof(DTE))]
    public class TreeViewEditor : BaseEditor
    {
        public override object EditValue(ITypeDescriptorContext context,
            IServiceProvider provider, object value)
        {
            EnvDTE.DTE dte = (EnvDTE.DTE)provider.GetService(typeof(EnvDTE.DTE));

            try
            {
                List<NameValueItem> nvitems = this.GetItems(dte, provider);

                SortItems(nvitems);

                object svc = provider.GetService(typeof(IWindowsFormsEditorService));
                if (svc == null)
                {
                    return base.EditValue(context, provider, value);
                }

                SelectionForm form = new SelectionForm(nvitems, value, true);
                if (((IWindowsFormsEditorService)svc).ShowDialog(form) == DialogResult.OK)
                {
                    if (value is NameValueItem[])
                    {
                        return form.SelectedNameValueItems;
                    }
                    else
                    {
                        return form.SelectedNameValueItem;
                    }
                }
                else
                {
                    return value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return null;
        }
    }

    [ServiceDependency(typeof(DTE))]
    public class ListEditor : BaseEditor
    {
        public override object EditValue(ITypeDescriptorContext context,
            IServiceProvider provider, object value)
        {
            EnvDTE.DTE dte = (EnvDTE.DTE)provider.GetService(typeof(EnvDTE.DTE));

            try
            {
                List<NameValueItem> nvitems = this.GetItems(dte, provider);

                SortItems(nvitems);

                object svc = provider.GetService(typeof(IWindowsFormsEditorService));
                if (svc == null)
                {
                    return base.EditValue(context, provider, value);
                }

                SelectionForm form = new SelectionForm(nvitems, value, false);
                if (((IWindowsFormsEditorService)svc).ShowDialog(form) == DialogResult.OK)
                {
                    if (context.Instance != null)
                    {
                        if (context.Instance is System.String)
                        {
                            if (context.Instance.ToString() == "NameValueItem")
                            {
                                return form.SelectedNameValueItem;
                            }
                        }
                    }
                    if (value is NameValueItem[])
                    {
                        return form.SelectedNameValueItems;
                    }
                    else if (value is NameValueItem)
                    {
                        return form.SelectedNameValueItem;
                    }
                    else
                    {
                        if (form.SelectedNameValueItem != null)
                        {
                            return form.SelectedNameValueItem.Value;
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
                else
                {
                    return value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return null;
        }
    }

}
