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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml.Serialization;
using System.Xml;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    [ServiceDependency(typeof(DTE))]
    public class AddLocalListDefinition : AddFileToItemAction
    {
        private NameValueItem _BaseListTemplate = null;
        private NameValueItem[] _ContentTypes = null;

        [Input(Required = true)]
        public NameValueItem BaseListTemplate
        {
            get { return _BaseListTemplate; }
            set { _BaseListTemplate = value; }
        }

        [Input(Required = false)]
        public NameValueItem[] ContentTypes
        {
            get { return _ContentTypes; }
            set { _ContentTypes = value; }
        }

        public override void Execute()
      {
        DTE dte = GetService<DTE>(true);
        
        //search in features folder for the selected template type
        //read the contents of the list template and import them to a folder in the project
        //create the folder in the project feature

        string existingTemplatePath = "";
        string featuresfolder = Helpers.GetSharePointHive() + @"\TEMPLATE\FEATURES";
        foreach (string s in Directory.GetFiles(featuresfolder, "*.xml", SearchOption.AllDirectories))
        {
          try
          {
            XmlDocument doc = new XmlDocument();
            doc.Load(s);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("ns", "http://schemas.microsoft.com/sharepoint/");

            foreach (XmlNode node in doc.SelectNodes("/ns:Elements/ns:ListTemplate", nsmgr))
            {
              try
              {
                if (node.Attributes["Type"].Value == BaseListTemplate.Value)
                {
                  //found the list template definition
                  //get the name and add this to the current folder
                  string templatename = node.Attributes["Name"].Value;

                  //structure of feature is often so
                  //feature/FeatureName/ListTemplates/listTemp.xml <-- here we are so we need to go up to the featurename
                  //feature/FeatureName/doclib/..doclibfiles
                  string featurepath = Helpers.GetLocalFeatureDirectory(s);
                  string templatefolder = Path.Combine(featurepath, templatename);
                  if (Directory.Exists(templatefolder))
                  {
                    existingTemplatePath = templatefolder;
                  }
                  break;
                }

              }
              catch (Exception)
              {
              }
            }
          }
          catch (Exception)
          {
          }
        }

        if (existingTemplatePath == "")
        {
          throw new Exception("List template with ID " + BaseListTemplate.Value + " not found");
        }

        //collect all files from the list definition which need to be added to the project (e.g. schema.xml etc.)
          //and put them in a temp folder so that we can modify them before import
          SourceFileName = "";

          string tempFolder = Path.Combine(Path.GetTempPath(), "SPSF" + Guid.NewGuid().ToString());
          Directory.CreateDirectory(tempFolder);

          //add the exported items to the project
          foreach (string s in Directory.GetFiles(existingTemplatePath))
          {
              string copiedFileName = Path.Combine(tempFolder, Path.GetFileName(s));
              File.Copy(s, copiedFileName);

              if(SourceFileName != "")
              {
                  SourceFileName += ";";
              }
                SourceFileName += copiedFileName;

              //if schema.xml und contenttypes, dann noch content types in schema.xml eintragen
                bool contentTypesAdded = false;
                try
                {
                    if (Path.GetFileName(s).Equals("schema.xml", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (_ContentTypes != null)
                        {
                            if (_ContentTypes.Length > 0)
                            {
                                //open schemaxml, add content type nodes and close
                                XmlDocument schemaDoc = new XmlDocument();
                                schemaDoc.Load(copiedFileName);

                                XmlNamespaceManager nsmgr = new XmlNamespaceManager(schemaDoc.NameTable);
                                nsmgr.AddNamespace("ns", "http://schemas.microsoft.com/sharepoint/");

                                XmlNode contentTypesNode = schemaDoc.SelectSingleNode("/List/MetaData/ContentTypes");

                                /*
                                <ContentTypes>
                                  <ContentTypeRef ID="0x0102">
                                    <Folder TargetName="Event" />
                                  </ContentTypeRef>
                                </ContentTypes>
                                 * */

                                if (contentTypesNode != null)
                                {
                                    foreach (NameValueItem newContentType in _ContentTypes)
                                    {
                                        XmlNode newNode = schemaDoc.CreateNode(XmlNodeType.Element, "ContentTypeRef", "");
                                        XmlAttribute idAttribute = schemaDoc.CreateAttribute("ID");
                                        newNode.Attributes.Append(idAttribute).Value = newContentType.Value;

                                        XmlNode newFolderNode = schemaDoc.CreateNode(XmlNodeType.Element, "Folder", "");
                                        XmlAttribute folderAttribute = schemaDoc.CreateAttribute("TargetName");
                                        newFolderNode.Attributes.Append(folderAttribute).Value = newContentType.Name;
                                        newNode.AppendChild(newFolderNode);

                                        contentTypesNode.AppendChild(newNode);

                                        contentTypesAdded = true;
                                    }
                                }

                                if (contentTypesAdded)
                                {
                                    //if we have added content types then we set EnableContentTypes in the schema.xml to true
                                    XmlNode listNode = schemaDoc.SelectSingleNode("/List");
                                    if (listNode != null)
                                    {
                                        if (listNode.Attributes["EnableContentTypes"] != null)
                                        {
                                            //attribute EnableContentTypes is already there
                                            listNode.Attributes["EnableContentTypes"].Value = "True";
                                        }
                                        else
                                        {
                                            //attribute EnableContentTypes is NOT there
                                            XmlAttribute enableCTAttribute = schemaDoc.CreateAttribute("EnableContentTypes");
                                            listNode.Attributes.Append(enableCTAttribute).Value = "True";
                                        }
                                    }
                                }
                                schemaDoc.Save(copiedFileName);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    Helpers.LogMessage(dte, this, "Warning: could not add all content types to schema.xml");
                }
                if (contentTypesAdded)
                {


                }
          }

          //now add all collected files to the project as element files
          base.Execute();

          //delete temp folder
          Directory.Delete(tempFolder, true);

      }

        public override void Undo()
        {
        }
    }
}