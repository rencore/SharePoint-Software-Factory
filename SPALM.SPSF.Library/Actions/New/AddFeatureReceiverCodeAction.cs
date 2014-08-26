#region Using Directives

using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using System.Collections.Generic;
using System.Xml;
using EnvDTE80;
using System.Collections.Specialized;
using Microsoft.VisualStudio.SharePoint;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// Adds code the feature receiver of the given feature. If a feature receiver is not available in the given feature
    /// a feature receiver is added to the feature.
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class AddFeatureReceiverCodeAction : BaseTemplateAction
    {
        private const string FEATURERECEIVERTEMPLATE = @"Text\EmptyFeature\FeatureReceiver.cs.t4";

        public AddFeatureReceiverCodeAction()
          : base()
        {
        }
        
        [Input(Required = true)]
        public string ParentFeatureName { get; set; }

        [Input(Required = true)]
        public string AssemblyName { get; set; }

        [Input(Required = true)]
        public string Namespace { get; set; }

        //[Output]
        //public ProjectItem CreatedProjectItem { get; set; }

        //[Output]
        //public ProjectItem CreatedProjectFolder { get; set; }

        /// <summary>
        /// returns the path to the featureName.EventReceiver.Generated.cs
        /// </summary>
        /// <param name="featureName"></param>
        /// <param name="parentFolder"></param>
        /// <param name="parentItem"></param>
        /// <returns></returns>
        private ProjectItem CreateNewFeatureReceiver(string featureName, ProjectItem parentFolder, ProjectItem parentItem)
        {
            //add .cs files to the feature.xml project item as childs                            
            string featurecodeFolder = Helpers.GetFullPathOfProjectItem(parentFolder);

            string featureReceiverFilename = Path.Combine(featurecodeFolder, featureName + ".EventReceiver.cs");

            //replace featurename and namespace in the dictionary especially FeatureReceiverClass
            NameValueCollection overrideArguments = new NameValueCollection();
            overrideArguments.Add("FeatureReceiverClass", featureName + "EventReceiver");

            //generate the code for the featurereceiver
            string featureReceiverCode = GenerateContent(FEATURERECEIVERTEMPLATE, featureName + ".EventReceiver.cs", overrideArguments);
            
            if (!File.Exists(featureReceiverFilename))
            {
                File.Create(featureReceiverFilename).Close();
            }
            else
            {
                //file exists, ensure checkout
                ProjectItem fileItem = Helpers.GetProjectItemByName(parentItem.ProjectItems, featureReceiverFilename);
                Helpers.EnsureCheckout(parentItem.DTE, fileItem);
            }
            File.WriteAllText(featureReceiverFilename, featureReceiverCode); //this.FeatureReceiverContent);
            
            return Helpers.AddFromFile(parentItem, featureReceiverFilename);           
        }

        public override void Execute()
        {
            DTE dte = (DTE)this.GetService(typeof(DTE));
            Project Project = this.GetTargetProject(dte);

            string evaluatedTemplateFileName = EvaluateParameterAsString(dte, TemplateFileName);
            string evaluatedParentFeatureName = EvaluateParameterAsString(dte, ParentFeatureName);
            string evaluatedAssemblyName = EvaluateParameterAsString(dte, AssemblyName);
            string evaluatedNamespace = EvaluateParameterAsString(dte, Namespace);

            string xmlContent = GenerateContent(evaluatedTemplateFileName, "ReceiverCode.xml");

            try
            {
                //feature name consists of application name + SPSFConstants.NameSeparator + featurename, but we need to strip off the feature name from that
                string appName = Helpers.GetSaveApplicationName(dte);
                string featureName = evaluatedParentFeatureName;
                if (featureName.StartsWith(appName, StringComparison.InvariantCultureIgnoreCase))
                {
                    featureName = featureName.Substring(appName.Length + 1);
                }

                string FeatureReceiverClass = featureName + "EventReceiver";

                if (Helpers2.IsSharePointVSTemplate(dte, Project))
                {
                    //can we cast the project to ISharePointSolution
                    ISharePointProjectService projectService = Helpers2.GetSharePointProjectService(Project.DTE);
                    ISharePointProject sharePointProject = projectService.Convert<EnvDTE.Project, ISharePointProject>(Project);
                    sharePointProject.Synchronize();
                    
                    ISharePointProjectFeature parentfeature = null;
                    foreach (ISharePointProjectFeature feature in sharePointProject.Features)
                    {
                        if (feature.Name == evaluatedParentFeatureName)
                        {
                            parentfeature = feature;
                        }
                    }

                    if (parentfeature != null)
                    {
                        if (string.IsNullOrEmpty(parentfeature.Model.ReceiverClass))
                        {
                            if (dte.SuppressUI || (MessageBox.Show("Feature '" + evaluatedParentFeatureName + "' contains no feature receiver. Add feature receiver?", "Add Feature Receiver?", MessageBoxButtons.YesNo) == DialogResult.Yes))
                            {
                                
                                ProjectItem projectItem = projectService.Convert<ISharePointProjectFeature, EnvDTE.ProjectItem>(parentfeature);
                                ProjectItem featureXmlFile = projectService.Convert<ISharePointProjectMember, EnvDTE.ProjectItem>(parentfeature.FeatureFile);
                                
                                //no feature receiver there, we create one
                                ProjectItem createdFeatureReceiverItem = CreateNewFeatureReceiver(featureName, projectItem, featureXmlFile);
                                parentfeature.Model.ReceiverAssembly = evaluatedAssemblyName;
                                parentfeature.Model.ReceiverClass = evaluatedNamespace + "." + FeatureReceiverClass;
                                this.MergeNewCodeWithClass(createdFeatureReceiverItem, FeatureReceiverClass, xmlContent);
                                sharePointProject.Synchronize();

                                //is test run save file
                                if (dte.SuppressUI)
                                {
                                    featureXmlFile.Save();
                                    //createdFeatureReceiverItem.Save();
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            //1. we check if there is a filename with .generated in it
                            //we need the class name of the featurereceiver, but in VS-Feature the name contains a ID
                            string className = parentfeature.Model.ReceiverClass;
                            if (className.StartsWith("$SharePoint.Type"))
                            {
                                //not supported???
                                if (parentfeature.EventReceiverFile != null)
                                {

                                    ProjectItem existingCode = projectService.Convert<ISharePointProjectMember, EnvDTE.ProjectItem>(parentfeature.EventReceiverFile);
                                    this.MergeNewCodeWithClass(existingCode, "", xmlContent);
                                }
                            }
                            else
                            {
                                string foundReceiverClassName = parentfeature.Model.ReceiverClass;

                                string receiverNamespace = foundReceiverClassName.Substring(0, foundReceiverClassName.LastIndexOf(".") - 1);
                                string receiverClassName = foundReceiverClassName.Substring(foundReceiverClassName.LastIndexOf(".") + 1);

                                //ok, class name found
                                //yes, there is a feature receiver, but we need to find the class in the project
                                //find any .cs item which contains the name of the featureclass and namespace
                                List<string> patterns = new List<string>();
                                patterns.Add(receiverNamespace);
                                patterns.Add(receiverClassName);
                                patterns.Add(" FeatureActivated(");  //to ensure that we get the correct partial class
                                ProjectItem existingCode = Helpers.FindItem(Project, ".cs", patterns);

                                if (existingCode != null)
                                {
                                    this.MergeNewCodeWithClass(existingCode, receiverClassName, xmlContent);
                                }
                            }
                        }                        
                    }
                }
                else
                {

                    //find the feature receiver code
                    ProjectItem featureXMLFile = Helpers.GetFeatureXML(Project, evaluatedParentFeatureName);
                    ProjectItem featureFolder = Helpers2.GetFeature(Project.DTE, Project, evaluatedParentFeatureName);

                    //check if feature already contains feature receiver
                    if (featureXMLFile != null)
                    {
                        string receiverClassName = Helpers.GetFeatureReceiverClass(featureXMLFile);
                        string receiverNamespace = Helpers.GetFeatureReceiverNamespace(featureXMLFile);

                        if (receiverClassName == "")
                        {
                            if (dte.SuppressUI || (MessageBox.Show("Feature '" + evaluatedParentFeatureName + "' contains no feature receiver. Add feature receiver?", "Add Feature Receiver?", MessageBoxButtons.YesNo) == DialogResult.Yes))
                            {
                                //there is no feature receiver, we need to create one :-(
                                string path = Helpers.GetFullPathOfProjectItem(featureXMLFile); 

                                XmlDocument doc = new XmlDocument();
                                doc.Load(path);
                                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                                nsmgr.AddNamespace("ns", "http://schemas.microsoft.com/sharepoint/");

                                XmlNode node = doc.SelectSingleNode("/ns:Feature", nsmgr);
                                if (node != null)
                                {
                                    ProjectItem createdFeatureReceiverItem = CreateNewFeatureReceiver(featureName, featureFolder, featureXMLFile);

                                    XmlAttribute receiverAssemblyAttrib = doc.CreateAttribute("ReceiverAssembly");
                                    receiverAssemblyAttrib.Value = evaluatedAssemblyName;
                                    node.Attributes.Append(receiverAssemblyAttrib);

                                    XmlAttribute receiverClassAttrib = doc.CreateAttribute("ReceiverClass");
                                    receiverClassAttrib.Value = evaluatedNamespace + "." + FeatureReceiverClass;
                                    node.Attributes.Append(receiverClassAttrib);

                                    Helpers.EnsureCheckout(dte, path);

                                    XmlWriter xw = XmlWriter.Create(path, Helpers.GetXmlWriterSettings(path));
                                    doc.Save(xw);
                                    xw.Flush();
                                    xw.Close();

                                    this.MergeNewCodeWithClass(createdFeatureReceiverItem, FeatureReceiverClass, xmlContent);
                                }
                            }
                            else
                            {
                                return;
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
                            ProjectItem existingCode = Helpers.FindItem(Project, ".cs", patterns);

                            if (existingCode != null)
                            {
                                //find the activated Function
                                string featureFilename = Helpers.GetFullPathOfProjectItem(existingCode); 
                                this.MergeNewCodeWithClass(existingCode, receiverClassName, xmlContent);
                            }
                        }
                    }

                    if (featureXMLFile != null)
                    {
                        CreatedProjectItem = featureXMLFile;
                    }
                    else
                    {
                        CreatedProjectItem = Helpers.GetFeatureXML(Project, evaluatedParentFeatureName);
                    }

                    if (featureFolder != null)
                    {
                        CreatedProjectFolder = featureFolder;
                    }
                    else
                    {
                        CreatedProjectFolder = Helpers2.GetFeature(Project.DTE, Project, evaluatedParentFeatureName);
                    }
                }
            }
            catch (Exception ex)
            {
                if (dte.SuppressUI)
                {
                    throw new Exception(ex.ToString());
                }
                else
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        /// <summary>
        /// takes the existing feature receiver code, finds the method a puts the new code in there
        /// </summary>
        /// <param name="existingClassFilename"></param>
        private void MergeNewCodeWithClass(ProjectItem existingClassProjectItem, string featureReceiverClassName, string xmlContent)
        {
            FileCodeModel2 model = (FileCodeModel2)existingClassProjectItem.FileCodeModel;

            //ok, Content is xml in the following form
            /*
             * <FeatureReceiverCode>
                <UsingStatement>using Microsoft.SharePoint.Administration;</UsingStatement>
                <FeatureActivatedMethod>ActivateTimerJob_<#= TimerJobClass #></FeatureActivatedMethod>
                <FeatureActivatedCode>
            */
            //this needs to be added;

            XmlDocument allCode = new XmlDocument();
            allCode.LoadXml(xmlContent);

            //placing the using statements in the existing file
            XmlNodeList usingNodes = allCode.SelectNodes("/FeatureReceiverCode/UsingStatements/UsingStatement");
            foreach (XmlNode usingNode in usingNodes)
            {
                AddUsingStatement(model, model.CodeElements, usingNode.InnerText);
            }

            //next we need to add the method call to the existing methods
            //FeatureActivatedMethod and add it near the region #region FeatureActivatedGeneratedCode
            XmlNode methodNode = allCode.SelectSingleNode("/FeatureReceiverCode/FeatureActivatedMethod");
            if (methodNode != null)
            {
                XmlNode methodCodeNode = allCode.SelectSingleNode("/FeatureReceiverCode/FeatureActivatedCode");
                if (methodCodeNode != null)
                {
                    //add the method itself to the class
                    AddMethodCall(model.CodeElements, featureReceiverClassName, "FeatureActivated", methodNode.InnerText);

                    //add the call to our method to FeatureActivated
                    AddMethodToClass(model.CodeElements, featureReceiverClassName, methodCodeNode.InnerText);                    
                 }
            }

            XmlNode methodNode2 = allCode.SelectSingleNode("/FeatureReceiverCode/FeatureDeactivatingMethod");
            if (methodNode2 != null)
            {
                XmlNode methodCodeNode2 = allCode.SelectSingleNode("/FeatureReceiverCode/FeatureDeactivatingCode");
                if (methodCodeNode2 != null)
                {
                    //add the method itself to the class
                    AddMethodCall(model.CodeElements, featureReceiverClassName, "FeatureDeactivating", methodNode2.InnerText);

                    //add the call to our method to FeatureActivated
                    AddMethodToClass(model.CodeElements, featureReceiverClassName, methodCodeNode2.InnerText);
                }
            }

            model.Synchronize();

            //save automatically during tests
            if (model.DTE.SuppressUI)
            {
                existingClassProjectItem.Save();
            }
        }

        private void AddMethodToClass(CodeElements codeElements, string className, string methodCode)
        {
            CodeClass featureReceiverClass = GetClassByName(codeElements, className);

            //add the method to the class
            if (featureReceiverClass != null)
            {
                EditPoint2 editPoint = (EditPoint2)featureReceiverClass.GetEndPoint(vsCMPart.vsCMPartBody).CreateEditPoint();

                StringReader reader = new StringReader(methodCode);
                string line = reader.ReadLine();
                while (line != null)
                {
                    editPoint.InsertNewLine(1);
                    editPoint.Indent(null, 2);
                    editPoint.Insert(line);
                    line = reader.ReadLine();
                }

                editPoint.InsertNewLine(1);

                Helpers.LogMessage(featureReceiverClass.DTE, featureReceiverClass.DTE,  Helpers.GetFullPathOfProjectItem(featureReceiverClass.ProjectItem) + ": Added new method");

            }
            else
            {
                throw new Exception("Class " + className + " not found");
            }
        }

        private CodeClass GetClassByName(CodeElements codeElements, string className)
        {
            //if className is empty we will take the first class in the file

            CodeClass result = null;

            //add the method to the class
            foreach (CodeElement codeElement in codeElements)
            {
                if (codeElement.Kind == vsCMElement.vsCMElementClass)
                {
                    CodeClass featureReceiverClass = codeElement as CodeClass;
                    if ((featureReceiverClass.Name == className) || (className == ""))
                    {
                        result = featureReceiverClass;
                    }
                }
                if (result == null)
                {
                    result = GetClassByName(codeElement.Children, className);
                }
            }
            return result;
        }

        private bool AddMethodCall(CodeElements codeElements, string className, string targetMethodName, string methodCall)
        {
            CodeClass featureReceiverClass = GetClassByName(codeElements, className);
            CodeFunction function = null;
            bool result = false;

            if (featureReceiverClass != null)
            {

                //try to find the targetMethodName and if found then add the methodCall at the end
                foreach (CodeElement codeElement in featureReceiverClass.Members)
                {
                    if (codeElement.Kind == vsCMElement.vsCMElementFunction)
                    {
                        if (codeElement.Name == targetMethodName)
                        {
                            function = codeElement as CodeFunction;
                        }
                    }
                }

                if (function == null)
                {
                    //method not found (SPFeatureReceiverProperties properties)
                    function = featureReceiverClass.AddFunction(targetMethodName, vsCMFunction.vsCMFunctionFunction, "void", 0, vsCMAccess.vsCMAccessPublic, null);
                    CodeFunction2 function2 = function as CodeFunction2;
                    function2.OverrideKind = vsCMOverrideKind.vsCMOverrideKindOverride;
                    function.AddParameter("properties", "SPFeatureReceiverProperties", -1);
                    function.AddAttribute("SharePointPermission", "(SecurityAction.LinkDemand, ObjectModel = true)");
                    Helpers.LogMessage(function.DTE, function.DTE,  Helpers.GetFullPathOfProjectItem(function.ProjectItem) + ": Added method '" + methodCall + "'");

                }

                if (function != null)
                {                   
                    EditPoint2 editPoint = (EditPoint2)function.GetEndPoint(vsCMPart.vsCMPartBody).CreateEditPoint();

                    //get indent of editpoint (at the end of the function
                    int charsBefore = editPoint.AbsoluteCharOffset;
                    int lineAdded = editPoint.Line;

                    if (!methodCall.StartsWith("this."))
                    {
                        //add this. to be StyleCop conform
                        methodCall = "this." + methodCall;
                    }

                    editPoint.InsertNewLine(1);
                    editPoint.Insert("// Call to method " + methodCall);
                    editPoint.InsertNewLine(1);
                    editPoint.Indent(null, 2);
                    editPoint.Insert(methodCall);
                    editPoint.InsertNewLine(1);
                    editPoint.Indent(null, 2);

                    Helpers.LogMessage(function.DTE, function.DTE, Helpers.GetFullPathOfProjectItem(function.ProjectItem) + "(" + lineAdded.ToString() + ",0): Added code to method '" + methodCall + "'");

                    result = true;
                }
            }
            else
            {
                throw new Exception("Class " + className + " not found");
            }
            return result;
        }

        private void AddUsingStatement(FileCodeModel model, CodeElements codeElements, string usingStatement)
        {
            bool usingStatementFound = false;
            CodeImport lastCodeElement = null;

            foreach (CodeElement codeElement in codeElements)
            {
                if (codeElement.Kind == vsCMElement.vsCMElementImportStmt)
                {
                    CodeImport codeImport = codeElement as CodeImport;
                    if (codeImport.Namespace == usingStatement)
                    {
                        usingStatementFound = true;
                    }
                    lastCodeElement = codeImport;
                }

                AddUsingStatement(model, codeElement.Children, usingStatement);
            }

            if (!usingStatementFound)
            {
                if (lastCodeElement != null)
                {
                    //FileCodeModel2 model2 = model as FileCodeModel2;
                    //model2.AddImport(usingStatement);

                    EditPoint2 editPoint = (EditPoint2)lastCodeElement.GetEndPoint().CreateEditPoint();
                    editPoint.InsertNewLine(1);
                    editPoint.Indent(null, 1);
                    editPoint.Insert("using " + usingStatement + ";");

                    Helpers.LogMessage(model.DTE, model.DTE, Helpers.GetFullPathOfProjectItem(lastCodeElement.ProjectItem) + ": Added using statement '" + usingStatement + "'");
                }
            }
        }        
    }
}