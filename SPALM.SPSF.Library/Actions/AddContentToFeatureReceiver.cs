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
  public class AddContentToFeatureReceiver : ConfigurableAction
  {
    private Project _CurrentProject;
    private string _Content;  //xml
    private string _ParentFeatureName; //name of the featurefolder
    private string _Method; //e.g. FeatureActivated
    private bool _Open = false;
    private string _DefaultNamespace = "";

		private string _AssemblyName = "";
		private string _FeatureReceiverClass = "";
		private string _FeatureReceiverContent = "";
		private string _FeatureReceiverGeneratedContent = "";

    /// <summary>
    /// Specifies the filename name of the xml file in the project 
    /// </summary>
    [Input(Required = true)]
    public Project Project
    {
      get { return _CurrentProject; }
      set { _CurrentProject = value; }
    }

    [Input(Required = true)]
    public string Content
    {
      get { return _Content; }
      set { _Content = value; }
    }

		[Input(Required = true)]
    public string FeatureReceiverContent
    {
      get { return _FeatureReceiverContent; }
      set { _FeatureReceiverContent = value; }
    }

		[Input(Required = true)]
    public string FeatureReceiverGeneratedContent
    {
      get { return _FeatureReceiverGeneratedContent; }
      set { _FeatureReceiverGeneratedContent = value; }
    }

    [Input(Required = true)]
    public string ParentFeatureName
    {
      get { return _ParentFeatureName; }
      set { _ParentFeatureName = value; }
    }

    [Input(Required = true)]
    public string Method
    {
      get { return _Method; }
      set { _Method = value; }
    }

    [Input(Required = false)]
    public bool Open
    {
      get { return _Open; }
      set { _Open = value; }
    }

    [Input(Required = false)]
    public string DefaultNamespace
    {
      get { return _DefaultNamespace; }
      set { _DefaultNamespace = value; }
    }

		[Input(Required = true)]
		public string AssemblyName
    {
			get { return _AssemblyName; }
			set { _AssemblyName = value; }
    }

		[Input(Required = true)]
		public string FeatureReceiverClass
    {
			get { return _FeatureReceiverClass; }
			set { _FeatureReceiverClass = value; }
    }

		


    protected string GetBasePath()
    {
        return base.GetService<IConfigurationService>(true).BasePath;
    }

    private string GetTemplateBasePath()
    {
      return new DirectoryInfo(this.GetBasePath() + @"\Templates").FullName;
    }

    public override void Execute()
    {
      EnvDTE.DTE dte = this.GetService<EnvDTE.DTE>(true);

      try
      {
        //loading the generated xml content
        string codeToBeAdded = Content;

        //find the feature receiver code
        ProjectItem featureXMLFile = Helpers.GetFeatureXML(_CurrentProject, ParentFeatureName);

        string featureName = ParentFeatureName;
        if (featureName.Contains(SPSFConstants.NameSeparator))
        {
            featureName = featureName.Substring(featureName.LastIndexOf(SPSFConstants.NameSeparator) + 1);
        }
        
        //check if feature already contains feature receiver
        if (featureXMLFile != null)
        {
          string receiverClassName = Helpers.GetFeatureReceiverClass(featureXMLFile);
          string receiverNamespace = Helpers.GetFeatureReceiverNamespace(featureXMLFile);

          if (receiverClassName == "")
          {
            //there is no feature receiver, we need to create one :-(
              string path = Helpers.GetFullPathOfProjectItem(featureXMLFile); // Helpers.GetFullPathOfProjectItem(featureXMLFile);

            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("ns", "http://schemas.microsoft.com/sharepoint/");

            XmlNode node = doc.SelectSingleNode("/ns:Feature", nsmgr);
            if (node != null)
            {
              //add files to the project
              //CustomCode/FeatureName
              ProjectItem CodeFolder = Helpers.GetProjectFolder(_CurrentProject.ProjectItems, "CustomCode", true);
              ProjectItem FeatureCodeFolder = Helpers.GetProjectFolder(CodeFolder.ProjectItems, featureName, true);

              string featurecodeFolder = Helpers.GetFullPathOfProjectItem(FeatureCodeFolder); //FeatureCodeFolder.Properties.Item("FullPath").Value.ToString();
							string featureReceiverFilename = Path.Combine(featurecodeFolder, FeatureReceiverClass + ".cs");
							string featureReceiverGenFilename = Path.Combine(featurecodeFolder, FeatureReceiverClass + ".generated.cs");

              if (!File.Exists(featureReceiverFilename))
              {
                File.Create(featureReceiverFilename).Close();
              }
              File.WriteAllText(featureReceiverFilename, this.FeatureReceiverContent);

              if (!File.Exists(featureReceiverGenFilename))
              {
                File.Create(featureReceiverGenFilename).Close();
              }
              File.WriteAllText(featureReceiverGenFilename, this.FeatureReceiverGeneratedContent);

              Helpers.AddFromFile(FeatureCodeFolder, featureReceiverFilename);
              Helpers.AddFromFile(FeatureCodeFolder, featureReceiverGenFilename);

              XmlAttribute receiverAssemblyAttrib = doc.CreateAttribute("ReceiverAssembly");
              receiverAssemblyAttrib.Value = this.AssemblyName;
              node.Attributes.Append(receiverAssemblyAttrib);

              XmlAttribute receiverClassAttrib = doc.CreateAttribute("ReceiverClass");
							receiverClassAttrib.Value = _DefaultNamespace + "." + FeatureReceiverClass;
              node.Attributes.Append(receiverClassAttrib);

              Helpers.EnsureCheckout(dte, path);

              XmlWriter xw = XmlWriter.Create(path, Helpers.GetXmlWriterSettings(path));
              doc.Save(xw);
              xw.Flush();
              xw.Close();

              this.MergeNewCodeWithClass(featureReceiverGenFilename);
            }

          }
          else
          {
            //yes, there is a feature receiver, but we need to find the class in the project
            //find any .cs item which contains the name of the featureclass and namespace
            List<string> patterns = new List<string>();
            patterns.Add(receiverClassName);
            patterns.Add(receiverNamespace);
            patterns.Add(" FeatureActivated(");  //to ensure that we get the correct partial class
            ProjectItem existingCode = Helpers.FindItem(_CurrentProject, ".cs", patterns);

            if (existingCode != null)
            {
                string featureFilename = Helpers.GetFullPathOfProjectItem(existingCode); // existingCode.Properties.Item("FullPath").Value.ToString();
              this.MergeNewCodeWithClass(featureFilename);
            }          
          }
        }  
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.ToString());
      }
    }

    /// <summary>
    /// takes the existing feature receiver code, finds the method a puts the new code in there
    /// </summary>
    /// <param name="existingClassFilename"></param>
    private void MergeNewCodeWithClass(string existingClassFilename)
    {
      //read the existing content line by line
      List<string> codelines = new List<string>();
      if (File.Exists(existingClassFilename))
      {
        TextReader reader = new StreamReader(existingClassFilename);
        string input = null;
        while ((input = reader.ReadLine()) != null)
        {
          codelines.Add(input);
        }
        reader.Close();
      }

      //ok, Content is xml in the following form
      /*
       * <FeatureReceiverCode>
	      <UsingStatement>using Microsoft.SharePoint.Administration;</UsingStatement>
	      <FeatureActivatedMethod>ActivateTimerJob_<#= TimerJobClass #></FeatureActivatedMethod>
	      <FeatureActivatedCode>
      */
      //this needs to be added;

      XmlDocument allCode = new XmlDocument();
      allCode.LoadXml(Content);

      
      //check: is the code already there
      string checkActivateFunctionCall = "";
      XmlNode activateFunctionNode = allCode.SelectSingleNode("/FeatureReceiverCode/FeatureActivatedMethod");
      if (activateFunctionNode != null)
      {
        checkActivateFunctionCall = activateFunctionNode.InnerText;
      }
      string checkDeactivateFunctionCall = "";
      XmlNode deactivateFunctionNode = allCode.SelectSingleNode("/FeatureReceiverCode/FeatureDeactivatingMethod");
      if (deactivateFunctionNode != null)
      {
        checkDeactivateFunctionCall = activateFunctionNode.InnerText;
      }
      foreach (string s in codelines)
      {
        if ((checkActivateFunctionCall != "") && s.Contains(checkActivateFunctionCall))
        {
          return;
        }
        if ((checkDeactivateFunctionCall != "") && s.Contains(checkDeactivateFunctionCall))
        {
          return;
        }
      }

      //placing the using statements in the existing file
      XmlNodeList usingNodes = allCode.SelectNodes("/FeatureReceiverCode/UsingStatements/UsingStatement");
      foreach (XmlNode usingNode in usingNodes)
      {
        //check if the using is already there
        string usingStatement = usingNode.InnerText;
        bool usingStatementfound = false;
        bool usingsectionfound = false;
        int indent = 0;
        for(int i = 0; i < codelines.Count; i++)
        {
          string s = codelines[i].TrimStart();
          
          if (s.StartsWith("using "))
          {
            usingsectionfound = true;
            indent = codelines[i].Length - s.Length;
          }
          if (s == usingStatement)
          {
            usingStatementfound = true;
          }
          //bin ich am ende der using section
          if (usingsectionfound)
          {
            if (!s.StartsWith("using"))
            {
              if (!usingStatementfound)
              {
                //ok rein damit
                codelines.Insert(i, usingStatement.PadLeft(indent + usingStatement.Length));
                break;
              }
            }
          }
        }
      }

      //next we need to add the method call to the existing methods
      //FeatureActivatedMethod and add it near the region #region FeatureActivatedGeneratedCode
      XmlNode methodNode = allCode.SelectSingleNode("/FeatureReceiverCode/FeatureActivatedMethod");
      if (methodNode != null)
      {
        XmlNode methodCodeNode = allCode.SelectSingleNode("/FeatureReceiverCode/FeatureActivatedCode");
        if (methodCodeNode != null)
        {
          AttachCodeInRegion(codelines, "FeatureActivatedGeneratedCode", methodNode.InnerText);
          AttachCodeInRegion(codelines, "FeatureGeneratedMethods", methodCodeNode.InnerText);
        }
      }

      //next we need to add the method call to the existing methods
      //FeatureDeActivatedMethod and add it near the region #region FeatureActivatedGeneratedCode
      XmlNode methodNodeDe = allCode.SelectSingleNode("/FeatureReceiverCode/FeatureDeactivatingMethod");
      if (methodNodeDe != null)
      {
        XmlNode methodCodeNodeDe = allCode.SelectSingleNode("/FeatureReceiverCode/FeatureDeactivatingCode");
        if (methodCodeNodeDe != null)
        {
          AttachCodeInRegion(codelines, "FeatureDeactivatingGeneratedCode", methodNodeDe.InnerText);
          AttachCodeInRegion(codelines, "FeatureGeneratedMethods", methodCodeNodeDe.InnerText);
        }
      }

      //finished, write content
      TextWriter writer = new StreamWriter(existingClassFilename);
      foreach (string s in codelines)
      {
        writer.WriteLine(s);
      }
      writer.Close();
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