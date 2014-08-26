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
using System.Collections.Specialized;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    /// <summary>
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class BaseTemplateAction : BaseItemAction
    {
      public BaseTemplateAction()
        : base()
      {
      }

        /// <summary>
        /// template file name in the templates folder of SPSF 
        /// </summary>
        [Input(Required = true)]
        public string TemplateFileName { get; set; }

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
            
            //1. get correct parameters ("$(FeatureName)" as "FeatureX")       
            string evaluatedTargetFileName = EvaluateParameterAsString(dte, TargetFileName);
            string evaluatedTemplateFileName = EvaluateParameterAsString(dte, TemplateFileName);
            string contents = GenerateContent(evaluatedTemplateFileName, evaluatedTargetFileName);
            if (Helpers2.TemplateContentIsEmpty(contents))
            {
                return;
            }

            try
            {
                //2. save the file to a temporary folder
                SourceFileName = Path.GetTempFileName();
                WriteContentToTempFile(SourceFileName, contents, evaluatedTargetFileName);

                //now run parent action which adds the file to the project
                base.Execute();

                //now we can delete the temp file
                File.Delete(SourceFileName);
            }
            catch (Exception ex)
            {
                Helpers.LogMessage(dte, dte, ex.ToString());
                throw ex;
            }
        }

        private void WriteContentToTempFile(string tempFileName, string content, string targetFilename)
        {
            //sometimes parent is a file, but it should be a folder, so we point to the parent whereToAdd

            if (targetFilename.ToUpper().EndsWith(".XML"))
            {
                try
                {
                    //we place our comment there
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(content);
                    Helpers.CheckLicenseComment(doc);
                    content = doc.OuterXml;
                }
                catch (Exception)
                {
                }
            }

            if (targetFilename.ToUpper().EndsWith(".CS"))
            {
                try
                {
                    content = Helpers.CheckLicenseCommentCS(content);
                }
                catch (Exception)
                {
                }
            }

            using (StreamWriter writer = new StreamWriter(tempFileName, false))
            {
                if (content.Contains("<?xml") || targetFilename.ToUpper().EndsWith(".XML"))
                {
                    try
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(content);

                        XmlWriter xw = XmlWriter.Create(writer, Helpers.GetXmlWriterSettings(targetFilename.ToUpper()));
                        doc.Save(xw);
                        xw.Flush();
                        xw.Close();
                    }
                    catch
                    {
                        writer.WriteLine(content);
                    }
                }
                else
                {
                    writer.WriteLine(content);
                }
            }
        }
    }
}