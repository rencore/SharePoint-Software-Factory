#region Using Directives

using System;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using EnvDTE;
using System.Xml;
using System.Collections.Generic;
using EnvDTE80;
using System.Text;
using System.CodeDom.Compiler;
using System.Collections;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// Adds content to an existing XML file or creates the xml file (elements.xml)
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class SetFeatureReceiver : BaseItemAction
    {
        private string _ReceiverAssembly;  //xml
        private string _ParentFeatureName; //name of the featurefolder
        private string _ReceiverClass;

        [Input(Required = true)]
        public string ParentFeatureName
        {
            get { return _ParentFeatureName; }
            set { _ParentFeatureName = value; }
        }

        [Input(Required = true)]
        public string ReceiverClass
        {
            get { return _ReceiverClass; }
            set { _ReceiverClass = value; }
        }

        [Input(Required = true)]
        public string ReceiverAssembly
        {
            get { return _ReceiverAssembly; }
            set { _ReceiverAssembly = value; }
        }

        public override void Execute()
        {
            if (ExcludeCondition)
            {
                return;
            }
            if (!AdditionalCondition)
            {
                return;
            }

            DTE dte = (DTE)this.GetService(typeof(DTE));
            Project project = this.GetTargetProject(dte);

            try
            {
                //find the feature receiver code
                ProjectItem featureXMLFile = Helpers.GetFeatureXML(project, ParentFeatureName);

                if (featureXMLFile == null)
                {
                    throw new Exception("Feature with name " + ParentFeatureName + " not found");
                }
                string path = Helpers.GetFullPathOfProjectItem(featureXMLFile);

                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("ns", "http://schemas.microsoft.com/sharepoint/");

                XmlNode featureNode = doc.SelectSingleNode("/ns:Feature", nsmgr);
                if (featureNode == null)
                {
                    throw new Exception("XmlNode 'Feature' not found in file " + path);
                }

                Helpers.LogMessage(dte, this, "Setting feature receiver of feature '" + ParentFeatureName + "' to '" + _ReceiverClass + "', '" + _ReceiverAssembly + "'");

                //set properties ReceiverClass und ReceiverAssembly
                SetAttribute(featureNode, "ReceiverClass", this.ReceiverClass);
                SetAttribute(featureNode, "ReceiverAssembly", this.ReceiverAssembly);

                //save the feature.xml after the changes
                Helpers.EnsureCheckout(dte, path);

                XmlWriter xw = XmlWriter.Create(path, Helpers.GetXmlWriterSettings(path));
                doc.Save(xw);
                xw.Flush();
                xw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SetAttribute(XmlNode featureNode, string attributeName, string newValue)
        {
            if (featureNode.Attributes[attributeName] != null)
            {
                if (MessageBox.Show("Feature already contains a feature receiver ('" + featureNode.Attributes[attributeName].Value + "'). Replace?", "Warning", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    featureNode.Attributes[attributeName].Value = newValue;
                }
            }
            else
            {
                featureNode.Attributes.Append(featureNode.OwnerDocument.CreateAttribute(attributeName)).Value = newValue;
            }
        }

        public override void Undo()
        {
        }
    }
}