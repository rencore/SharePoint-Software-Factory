using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Xml.XPath;

namespace SPALM.SPSF.Test.BCSTester
{
    public class Class1
    {
        public Class1()
        {
            string[] paths = new string[] {@"Microsoft SDKs\Windows\v6.0A\bin", @"\Microsoft SDKs\Windows\v7.0A\bin", @"Microsoft SDKs\Windows\v7.0A\bin\NETFX 4.0 Tools" };
            string sqlmetalexe = "";

            foreach(string checkpath in paths)
            {
                string testpath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), checkpath);
                testpath = Path.Combine(testpath, "sqlmetal.exe");
                if(File.Exists(testpath))
                {
                    sqlmetalexe = testpath;
                    break;
                }
                else
                {
                    //check x86 folder
                    testpath = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), checkpath);
                    testpath = Path.Combine(testpath, "sqlmetal.exe");
                    if(File.Exists(testpath))
                    {
                        sqlmetalexe = testpath;
                        break;
                    }
                }

            }

            //<Program Files>\Microsoft SDKs\Windows\v6.0A\bin
            if (File.Exists(sqlmetalexe))
            {
                string output = Path.GetTempFileName();

                //run sqlmetal.exe
                string arguments = "/server:demo2010a /database:BCSTest /dbml:\"" + output + "\" /namespace:thenamespace /context:ContextName /serialization:Unidirectional";

                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = sqlmetalexe;
                psi.Arguments = arguments;
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;

                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                

                // Create the process.
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.OutputDataReceived += new DataReceivedEventHandler(snProcess_OutputDataReceived);
                p.ErrorDataReceived += new DataReceivedEventHandler(snProcess_ErrorDataReceived);
                p.StartInfo = psi;                
                p.Start();
                p.WaitForExit();    

                XmlDocument doc = new XmlDocument();
                doc.Load(output);

                XmlNamespaceManager mgrMerge = new XmlNamespaceManager(doc.NameTable);
                mgrMerge.AddNamespace("ns", "http://schemas.microsoft.com/linqtosql/dbml/2007");

                XmlNode dbNode = doc.SelectSingleNode("/ns:Database", mgrMerge);
                for (int i = dbNode.ChildNodes.Count - 1; i >= 0; i--)
                {
                    bool removeNode = true;
                    if (dbNode.ChildNodes[i].Name == "Table")
                    {
                        if (dbNode.ChildNodes[i].Attributes["Name"].Value == "dbo.ProductTable")
                        {
                            //remove
                            removeNode = false;
                        }
                    }

                    if (removeNode)
                    {
                        dbNode.RemoveChild(dbNode.ChildNodes[i]);
                    }
                }

                doc.Save("C:\\test2.dbml");
            }
        }

        void snProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            
        }

        void snProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            
        }
    }
}
