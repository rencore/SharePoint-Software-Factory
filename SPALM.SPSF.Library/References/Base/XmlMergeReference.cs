using System;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using EnvDTE80;
using EnvDTE;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.Common;
using System.Security.Permissions;
using System.Xml;
using SPALM.SPSF.Library.Actions;

namespace SPALM.SPSF.Library.References
{
    [Serializable]
    public class XmlMergeReference : CustomizationReference, IAttributesConfigurable
    {
        public string XPath { get; set; }

        public string XPathNamespace { get; set; }

        public XmlMergeReference(string template)
            : base(template)
        {
        }

        #region ISerializable Members

        protected XmlMergeReference(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            try
            {
                this.XPath = info.GetString("XPath");
                this.XPathNamespace = info.GetString("XPathNamespace");
            }
            catch { }
        }


        #endregion ISerializable Members

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("XPath", this.XPath);
            info.AddValue("XPathNamespace", this.XPathNamespace);
            base.GetObjectData(info, context);
        }

        public override bool IsEnabledFor(object target)
        {
            Helpers.Log("XmlMergeReference");

            if (!(target is ProjectItem))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(this.SharePointVersions))
            {
                try
                {
                    if (!Helpers.SolutionHasVersion(Helpers.GetDTEFromTarget(target), SharePointVersions))
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                }
            }

            try
            {
                if (XPath != "")
                { 
                    //ok, item is clicked, we search now for a child element which contains the needed xpath, 
                    //if we find a element, we return true
                    if (target is ProjectItem)
                    {
                        ProjectItem pitem = target as ProjectItem;
                        if (pitem.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFile)
                        {
                            //is the xml file clicked?
                            try
                            {
                                if (pitem.Name.EndsWith(".XML", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    return Helpers2.IsXPathInFile(pitem, this.XPath, this.XPathNamespace);
                                }
                            }
                            catch { }
                        }
                        else
                        {
                            if (pitem.ProjectItems != null)
                            {
                                //folder is clicked, we search for xml files below the folder which contains the needed xpath
                                foreach (ProjectItem childItem in pitem.ProjectItems)
                                {
                                    if (childItem.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFile)
                                    {
                                        if (childItem.Name.EndsWith(".XML", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            if (Helpers2.IsXPathInFile(childItem, this.XPath, this.XPathNamespace))
                                            {
                                                return true;
                                            }
                                        }
                                    }
                                    //sometimes files have childs
                                    foreach (ProjectItem subchildItem in childItem.ProjectItems)
                                    {
                                        if (subchildItem.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFile)
                                        {
                                            if (subchildItem.Name.EndsWith(".XML",
                                                                           StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                if (Helpers2.IsXPathInFile(subchildItem, this.XPath, this.XPathNamespace))
                                                {
                                                    return true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        
        public override string AppliesTo
        {
            get { return "Xml files with xpath " + this.XPath; }
        }

        #region IAttributesConfigurable Members

        public new void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            base.Configure(attributes);

            if (attributes.ContainsKey("XPathNamespace"))
            {
                XPathNamespace = attributes["XPathNamespace"];
            }
            if (attributes.ContainsKey("XPath"))
            {
                XPath = attributes["XPath"];
            } 
        }

        #endregion
    }
}
