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
using Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates;
using Microsoft.Practices.Common.Services;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ComponentModel.Design;
using System.Xml;
using System.Data;
using System.Collections.Specialized;
using System.Xml.XPath;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// Adds a template file as a child to the given project item
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class AddXmlToItemAction : BaseTemplateAction
    {
      public AddXmlToItemAction()
        : base()
      {
      }

        [Input(Required = false)]
        public ProjectItem AddToItem { get; set; }

        [Input(Required = false)]
        public ProjectItem SelectedItem { get; set; }

        [Input(Required = false)]
        public string XPath { get; set; } //xpath to the parent node if multiple elements are in the projectitem

        [Input(Required=false)]
        public string XPathNamespace { get; set; }

        [Input(Required = false)]
        public bool ReplaceExistingNode { get; set; } //TODO wegen doppelter DocumentTemplate z.B.

        [Output]
        public ProjectItem CreatedElementFolder { get; set; }

        [Output]
        public ProjectItem CreatedElementFile { get; set; }

        private string previousXmlContent = "";
        private string previousXmlPath = "";

        public override void Undo()
        {
            base.Undo();

            //write the old content back to the file
            if (!string.IsNullOrEmpty(previousXmlContent))
            {
                File.WriteAllText(previousXmlPath, previousXmlContent);
            }
        }

        public override void Execute()
        {           
            DTE dte = (DTE)this.GetService(typeof(DTE));
            Project project = this.GetTargetProject(dte);

            //1. transformiere Template
            string evaluatedTemplateFileName = EvaluateParameterAsString(dte, TemplateFileName);
            string contents = GenerateContent(evaluatedTemplateFileName, "");
            if (Helpers2.TemplateContentIsEmpty(contents))
            {
                return;
            }
            string evaluatedXPath = EvaluateParameterAsString(dte, XPath);
            
            //2. Finde das ProjectItem wo der XPath enthalten ist
            ProjectItem xmlElementManifestFile = null;
            if (SelectedItem != null)
            {
                if (SelectedItem.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFile)
                {
                    //is the xml file clicked?
                    try
                    {
                        if (Helpers2.IsXPathInFile(SelectedItem, evaluatedXPath, XPathNamespace))
                        {
                            xmlElementManifestFile = SelectedItem;
                        }
                    }
                    catch { }
                }
                else
                {
                    //folder is clicked, we search for xml files below the folder which contains the needed xpath
                    foreach (ProjectItem childItem in SelectedItem.ProjectItems)
                    {
                        if (Helpers2.IsXPathInFile(childItem, evaluatedXPath, XPathNamespace))
                        {
                            xmlElementManifestFile = childItem;
                        }
                        foreach (ProjectItem subchildItem in childItem.ProjectItems)
                        {
                            if (Helpers2.IsXPathInFile(subchildItem, evaluatedXPath, XPathNamespace))
                            {
                                xmlElementManifestFile = subchildItem;
                            }
                        }
                    }
                }

                if (xmlElementManifestFile != null)
                {
                    Helpers.EnsureCheckout(dte, xmlElementManifestFile);

                    //settings output parameters
                    CreatedElementFolder = xmlElementManifestFile.Collection.Parent as ProjectItem;
                    CreatedElementFile = xmlElementManifestFile;

                    string pathToElementsXmlFile = Helpers.GetFullPathOfProjectItem(xmlElementManifestFile);

                    //bestehendes Dokument laden
                    XmlDocument existingDoc = new XmlDocument();
                    existingDoc.Load(pathToElementsXmlFile);

                    //backup for undo action
                    previousXmlContent = existingDoc.OuterXml;
                    previousXmlPath = pathToElementsXmlFile;

                    //neues Xml laden
                    XmlDocument newdoc = new XmlDocument();
                    newdoc.LoadXml(contents);
                    
                    if (string.IsNullOrEmpty(evaluatedXPath))
                    {
                        //Fall 1. kein xpath angegeben, documente werden gemerged
                        XmlDocument d = AddXmlToDoc(existingDoc, newdoc);
                        string xml = d.InnerXml;

                        XmlWriter xw2 = XmlWriter.Create(previousXmlPath, Helpers.GetXmlWriterSettings(previousXmlPath));
                        existingDoc.Save(xw2);
                        xw2.Flush();
                        xw2.Close();
                    }
                    else
                    {
                        //Fall 2. ein xpath ist angegeben für die Node, an die etwas angehängt werden soll
                        existingDoc = Transform(existingDoc, newdoc, evaluatedXPath, XPathNamespace);

                        XmlWriter xw2 = XmlWriter.Create(previousXmlPath, Helpers.GetXmlWriterSettings(previousXmlPath));
                        existingDoc.Save(xw2);
                        xw2.Flush();
                        xw2.Close();

                    }                                    
                }
            }

            if (Open)
            {
                if (xmlElementManifestFile != null)
                {
                    Window window = xmlElementManifestFile.Open("{00000000-0000-0000-0000-000000000000}");
                    window.Visible = true;
                    window.Activate();

                    try
                    {
                        XPathDocument doc = new XPathDocument(previousXmlPath);
                        XPathNavigator navigator = doc.CreateNavigator();

                        XmlNamespaceManager newnsmgr = new XmlNamespaceManager(navigator.NameTable);
                        newnsmgr.AddNamespace("ns", XPathNamespace);
                        XPathNavigator iterator = navigator.SelectSingleNode(evaluatedXPath, newnsmgr);
                        IXmlLineInfo lineInfo = ((IXmlLineInfo)iterator);

                        ((TextDocument)dte.ActiveDocument.Object("TextDocument")).Selection.GotoLine(lineInfo.LineNumber);
                        ((TextDocument)dte.ActiveDocument.Object("TextDocument")).Selection.PadToColumn(lineInfo.LinePosition);
                    }
                    catch { }
                }
            }
        }

        private XmlDocument Transform(XmlDocument existingDocument, XmlDocument newDocument, string xPath, string XPathNamespace)
        {
            XmlNamespaceManager newnsmgr = new XmlNamespaceManager(existingDocument.NameTable);
            newnsmgr.AddNamespace("ns", XPathNamespace);
            XmlNode startNodeInExistingDoc = existingDocument.SelectSingleNode(xPath, newnsmgr);

            //selektiere die selbe node in newDocument

            string tempXPath = this.GetXPathToNodeWithNodeNames(startNodeInExistingDoc);

            XmlNamespaceManager newnsmgr2 = new XmlNamespaceManager(newDocument.NameTable);
            newnsmgr2.AddNamespace("ns", XPathNamespace);
            XmlNode startNodeInNewDoc = newDocument.SelectSingleNode(tempXPath, newnsmgr2);

            if (startNodeInNewDoc != null)
            {
                //ok, ich habe link und rechte node, erstelle jetzt temporär 2 xmldocuments davon
                XmlDocument tempExistingDoc = new XmlDocument();
                tempExistingDoc.LoadXml(startNodeInExistingDoc.OuterXml);

                XmlDocument tempNewDoc = new XmlDocument();
                tempNewDoc.LoadXml(startNodeInNewDoc.OuterXml);

                //so, habe jetzt beide nodes links und rechts in der Hand, jetzt beide Elemente verbinden
                XmlDocument mergedFragementDoc = AddXmlToDoc(tempExistingDoc, tempNewDoc);

                //danach das Element wieder im existingdoc einhängen
                XmlDocumentFragment docFrag = existingDocument.CreateDocumentFragment();
                docFrag.InnerXml = mergedFragementDoc.InnerXml;
                startNodeInExistingDoc.ParentNode.ReplaceChild(docFrag, startNodeInExistingDoc);

                //remove unneeded xmlns
                CheckNamespaces(existingDocument);

                return existingDocument;
            }
            else
            {
                //es wurde keine passende node in newDoc gefunden, dann wird versucht, das gesamt dokument zu mergen
                return AddXmlToDoc(existingDocument, newDocument);
            }
        }

        private string GetXPathToNodeWithNodeNames(XmlNode node)
        {
            if (node.NodeType == XmlNodeType.Attribute)
            {
                return "";                
            }
            if (node.ParentNode == null)
            {
                // the only node with no parent is the root node, which has no path
                return "";
            }
            // the path to a node is the path to its parent, plus "/node()[n]", where 
            // n is its position among its siblings.
            return String.Format(
                "{0}/ns:{1}",
                GetXPathToNodeWithNodeNames(node.ParentNode),
                node.Name
                );
        }

        private XmlDocument AddXmlToDoc(XmlDocument existingdoc, XmlDocument newdoc)
        {

          //Prüfe für jede node in xml2
          //1. Gibt es die Node in xml1 
          //ja, node ist schon da, dann weiter und nix machen
          //ist die node nicht da, dann komplett mit Kindern reinpacken
          foreach (XmlNode node2 in newdoc.ChildNodes)
          {
              CheckNode(node2, existingdoc, existingdoc);
          }

          //remove unneeded xmlns
          CheckNamespaces(existingdoc);

          return existingdoc;
        }

        private void CheckNamespaces(XmlNode node)
        {
          if (node.Attributes != null)
          {
            //hat die Node ein Attribute
            if (node.Attributes.Count > 0)
            {
              foreach (XmlAttribute attrib in node.Attributes)
              {
                if (attrib.Name == "xmlns")
                {
                  if (attrib.Value == node.NamespaceURI)
                  {
                    //duplicate
                    node.Attributes.Remove(attrib);
                    break;
                  }
                }
              }
            }
          }
          foreach (XmlNode subnode in node.ChildNodes)
          {
            CheckNamespaces(subnode);
          }
        }

        string GetXPathToNode(XmlNode node)
        {
            return GetXPathToNode(node, true);
        }

        string GetXPathToNode(XmlNode node, bool includeAttributes)
        {
          if (node.NodeType == XmlNodeType.Attribute)
          {
              return "";
              /*
            // attributes have an OwnerElement, not a ParentNode; also they have
            // to be matched by name, not found by position
            return String.Format(
                "{0}/@{1}",
                GetXPathToNode(((XmlAttribute)node).OwnerElement),
                node.Name
                );
               * */
          }
          if (node.ParentNode == null)
          {
            // the only node with no parent is the root node, which has no path
            return "";
          }
          // the path to a node is the path to its parent, plus "/node()[n]", where 
          // n is its position among its siblings.
          return String.Format(
              "{0}/node()[{1}]",
              GetXPathToNode(node.ParentNode),
              GetNodePosition(node)
              );
        }

        int GetNodePosition(XmlNode child)
        {
          for (int i = 0; i < child.ParentNode.ChildNodes.Count; i++)
          {
            if (child.ParentNode.ChildNodes[i] == child)
            {
              // tricksy XPath, not starting its positions at 0 like a normal language
              return i + 1;
            }
          }
          throw new InvalidOperationException("Child node somehow not found in its parent's ChildNodes property.");
        }


        private void CheckNode(XmlNode node2, XmlDocument doc1, XmlNode currentparentnode)
        {
            //achtung: wenn ReplaceExistingNode da ist, dann wird das erste auftreten einer Node mit gleichem Namen eine komplette ersetzung durchgeführt
            //speziell für DocumentTemplates
          //ist die node2 im doc1 enthalten???

          //erstelle ein Xslt für die node
          string xslt2 = FindXPath(node2);

          string xslt2b = GetXPathToNode(node2);

          XmlNode parentnode = null;

          if (xslt2 != "")
          {
              XmlNamespaceManager featurensmgr = new XmlNamespaceManager(doc1.NameTable);
              foreach (string key in namespaces.Keys)
              {
                  featurensmgr.AddNamespace(key, namespaces[key]);
              }

              XmlNodeList foundnodes = doc1.SelectNodes(xslt2, featurensmgr);
              

              //ich brauche den attributecount, aber ohne xmlns, 
              int attribcount = 0;
              foreach (XmlAttribute attrib in node2.Attributes)
              {
                  if (attrib.Name != "xmlns")
                  {
                      attribcount++;
                  }
              }

              if ((foundnodes.Count == 1) && (attribcount == 0))
              {
                  //wenn nur 1 node gefunden und node2 hat gar keine Attribute, dann nehmen wir diese
                  //dann will man vermutlich mergen
                  parentnode = foundnodes[0];
              }
              else
              {
                  foreach (XmlNode foundnode in foundnodes)
                  {
                      //die gefundene Node könnte aber auch mehr attribute haben, als 
                      //im xpath angegeben. deshalb hier überprüfung, ob der Attribute-Count
                      //gleich ist. Wenn nicht, nehmen wir die nächste Node, die passen könnte
                      if (foundnode.Attributes.Count == node2.Attributes.Count)
                      {
                          parentnode = foundnode;
                      }
                  }
              }
          }

          if (parentnode == null)
          {
            //meine neue node gibt es noch nicht
            //also rein damit und nicht weitermachen
            //namespaces entfernen vor dem Einfügen, da die Namespaces 
            //mit dem parent gleich sind

            XmlNamespaceManager xnm = new XmlNamespaceManager(node2.OwnerDocument.NameTable);
            xnm.RemoveNamespace(node2.Prefix, node2.NamespaceURI);

            XmlDocumentFragment docFrag = doc1.CreateDocumentFragment();
            docFrag.InnerXml = node2.OuterXml;
            currentparentnode.AppendChild(docFrag);
          }
          else
          {
            //die gleiche Node habe ich gefunden, 
            //ich brauche also nichts hineinmachen
            //jetzt hier die Kinder prüfe
            foreach (XmlNode node2a in node2.ChildNodes)
            {
              CheckNode(node2a, doc1, parentnode);
            }
          }
        }

        string FindXPath(XmlNode node)
        {
          StringBuilder builder = new StringBuilder();
          while (node != null)
          {
            switch (node.NodeType)
            {
              case XmlNodeType.Attribute:
                builder.Insert(0, "/@" + node.Name);
                node = ((XmlAttribute)node).OwnerElement;
                break;
              case XmlNodeType.Element:
                int index = FindElementIndex((XmlElement)node);
                string xpath = "";

                string namespaceuri = node.NamespaceURI;
                string prefix = GetPrefixForNamespace(namespaceuri);
                if (node.Attributes.Count > 0)
                {

                  bool firstelelement = true;
                  foreach (XmlAttribute attrib in node.Attributes)
                  {
                    if (attrib.NodeType == XmlNodeType.Attribute)
                    {
                      if (attrib.Name != "xmlns")
                      {
                        if (!firstelelement)
                        {
                          xpath += " and ";
                        }

                          //attrib.Value can contain special chars (')
                        string attribValue = attrib.Value;
                        attribValue = attribValue.Replace("'", "&apos;");
                        attribValue = attribValue.Replace("&", "&amp;");
                        attribValue = attribValue.Replace("\"", "&quot;");
                        attribValue = attribValue.Replace("<", "&lt;");
                        attribValue = attribValue.Replace(">", "&gt;");       
                        
                        xpath += "@" + attrib.Name + "='" + attribValue + "'";
                        firstelelement = false;
                      }
                    }
                  }
                  if (xpath != "")
                  {
                    xpath = "[" + xpath + "]";
                  }
                }
                if (prefix != "")
                {
                  builder.Insert(0, "/" + prefix + ":" + node.Name + xpath);
                }
                else
                {
                  builder.Insert(0, "/" + node.Name + xpath);
                }
                node = node.ParentNode;
                break;
              case XmlNodeType.Document:
                return builder.ToString();
              default:
                //do nothing
                return builder.ToString();
            }
          }
          throw new ArgumentException("Node was not in a document");
        }

        NameValueCollection namespaces = new NameValueCollection();
        private string GetPrefixForNamespace(string namespaceuri)
        {
          if (namespaceuri == "")
          {
            return "";
          }
          int itemcount = 1;
          foreach (string key in namespaces.Keys)
          {
            itemcount++;
            //gibt es die namespaceuri schon?
            if (namespaces[key] == namespaceuri)
            {
              return key;
            }
          }
          //nicht gefunden
          string newkey = "ns" + itemcount.ToString();
          namespaces.Add(newkey, namespaceuri);
          return newkey;
        }

        int FindElementIndex(XmlElement element)
        {
          XmlNode parentNode = element.ParentNode;
          if (parentNode is XmlDocument)
          {
            return 1;
          }
          XmlElement parent = (XmlElement)parentNode;
          int index = 1;
          foreach (XmlElement candidate in parent.GetElementsByTagName(element.Name))
          {
            if (candidate == element)
            {
              return index;
            }
            index++;
          }
          throw new ArgumentException("Couldn't find element within parent");
        }

    }
}