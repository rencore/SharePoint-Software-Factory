using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using System.Xml.Resolvers;
using System.Xml.Linq;

namespace SPALM.SPSF.Test.TestProject
{
    public class MyXmlResolver : XmlResolver
    {
        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            return new MemoryStream(System.Text.Encoding.Default.GetBytes(File.ReadAllText(@"F:\SPALM\SPSF\Dev\Source\TestProject\xhtml11.dtd")));
        }

        public override Uri ResolveUri(Uri baseUri, string relativeUri)
        {
            return base.ResolveUri(baseUri, relativeUri);
        }

        public override System.Net.ICredentials Credentials
        {
            set { throw new NotImplementedException(); }
        }
    }

    [TestClass()]
    public class Help3Validation
    {
        [TestMethod()]
        [TestCategory("Build Test")]
        [Description("Checks that create help files are valid xml")]
        public void CheckHelp3Files()
        {
            string testDir = testContextInstance.TestDir; //F:\SPALM\SPSF\Dev\Source\TestResults\Administrator_DEMO2010A 2010-08-25 09_11_37
            DirectoryInfo info = new DirectoryInfo(testDir);
            string solutionDir = info.Parent.Parent.FullName;
            testContextInstance.WriteLine("Solution Dir: " + solutionDir);

            //go to the help file directory
            string help3Directory = Path.Combine(solutionDir, @"SPALM.SPSF\Help\OutputHelp3");
             testContextInstance.WriteLine("Help3 Dir: " + help3Directory);

            foreach (string helpFile in Directory.GetFiles(help3Directory, "*.html"))
            {
                try
                {
                    //http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd

                    XmlReaderSettings settings = new XmlReaderSettings();
                    //settings.DtdProcessing = DtdProcessing.Parse;
                    settings.DtdProcessing = DtdProcessing.Ignore;
                    settings.XmlResolver = new XmlPreloadedResolver(new MyXmlResolver(), XmlKnownDtds.Xhtml10);
                    string fileContents = File.ReadAllText(helpFile);
                    fileContents = fileContents.Replace("http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd", @"F:\SPALM\SPSF\Dev\Source\TestProject\xhtml11.dtd");
                    settings.CloseInput = true;

                    using (StringReader sr = new StringReader(fileContents))
                    {
                        using (XmlReader reader = XmlReader.Create(sr, settings))
                        {
                            XDocument document = XDocument.Load(reader);
                            document.ToString();
                            testContextInstance.WriteLine("Checking " + helpFile + " OK");
                            reader.Close();
                        }
                    }                    
                }
                catch (Exception ex)
                {
                    Assert.Fail("Checking " + helpFile + " Failed with: " + ex.ToString());
                }
            }
        }

        public TestContext testContextInstance;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
    }
}
