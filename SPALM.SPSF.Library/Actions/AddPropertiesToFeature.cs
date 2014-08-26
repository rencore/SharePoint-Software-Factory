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
  public class AddPropertiesToFeature : BaseTemplateAction
  {
    private string _ParentFeatureName; //name of the featurefolder

    [Input(Required = true)]
    public string ParentFeatureName
    {
      get { return _ParentFeatureName; }
      set { _ParentFeatureName = value; }
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

        //1. get correct parameters ("$(FeatureName)" as "FeatureX")    

        string targetFilename = Path.GetTempFileName();
        string evaluatedTemplateFileName = EvaluateParameterAsString(dte, TemplateFileName);
        string Content = GenerateContent(evaluatedTemplateFileName, targetFilename);
        if (Helpers2.TemplateContentIsEmpty(Content))
        {
            return;
        }

        try
      {
        //loading the generated xml content
          string codeToBeAdded = Content;

        //find the feature receiver code
          ProjectItem featureXMLFile = Helpers.GetFeatureXML(project, ParentFeatureName);

        if (featureXMLFile == null)
        {
          throw new Exception("Feature with name " +  ParentFeatureName + " not found");
        }
        string path = Helpers.GetFullPathOfProjectItem(featureXMLFile);  //Helpers.GetFullPathOfProjectItem(featureXMLFile);

        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
        nsmgr.AddNamespace("ns", "http://schemas.microsoft.com/sharepoint/");

        XmlNode featureNode = doc.SelectSingleNode("/ns:Feature", nsmgr);
        if (featureNode == null)
        {
          throw new Exception("XmlNode 'Feature' not found in file " + path);
        }

        //is there already a propertiesNode
        XmlNode propertiesNode = doc.SelectSingleNode("/ns:Feature/ns:Properties", nsmgr);
        if (propertiesNode == null)
        {
          propertiesNode = doc.CreateNode(XmlNodeType.Element, "Properties", "http://schemas.microsoft.com/sharepoint/");
          featureNode.AppendChild(propertiesNode);
        }

        //now add the new properties
        XmlDocument newPropertiesdoc = new XmlDocument();
        newPropertiesdoc.LoadXml(Content);
        XmlNodeList newPropertiesNodes = newPropertiesdoc.SelectNodes("/Properties/Property");
        foreach (XmlNode newPropNode in newPropertiesNodes)
        {
          XmlNode copiedNode = doc.CreateNode(XmlNodeType.Element, "Property", "http://schemas.microsoft.com/sharepoint/");
          copiedNode.Attributes.Append(doc.CreateAttribute("Key")).Value = newPropNode.Attributes["Key"].Value;
          copiedNode.Attributes.Append(doc.CreateAttribute("Value")).Value = newPropNode.Attributes["Value"].Value;
          propertiesNode.AppendChild(copiedNode);
        }

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

    private void AttachCodeInRegion(List<string> codelines, string regionname, string code)
    {
      bool regionstartfound = false;
      string tabs = "";
      for (int i = 0; i < codelines.Count; i++)
      {
        string s = codelines[i];
        if (!regionstartfound)
        {
          if (s.Contains("#region " + regionname))
          {
            regionstartfound = true;
            tabs = s.Substring(0, s.IndexOf("#region"));
          }
        }
        else
        {
          //region start found, search for the end
          if (s.Contains("#endregion"))
          {
            //zeilenweise den code einfügen, damit wir einen Tab davor machen können
            string finalcode = "";
            string line = "";
            StringReader reader = new StringReader(code);
            line = reader.ReadLine();
            while (line != null)
            {
              finalcode += tabs + line + Environment.NewLine;
              line = reader.ReadLine();
            }
            //place method call here
            codelines.Insert(i, finalcode);
            break;
          }
        }
      }
    }

    public override void Undo()
    {
    }
  }
}