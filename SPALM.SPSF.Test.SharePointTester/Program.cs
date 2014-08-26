using System;
using System.Xml;

namespace SPALM.SPSF.Test.SharePointTester
{
    class Program
    {
        static void Main(string[] args)
        {
            MigrateFile(@"c:\WSPBuilderProject1.csproj");
        }

        private static void MigrateFile(string path)
        {
            string removedContent = "";

            XmlDocument csprojfile = new XmlDocument();
            csprojfile.Load(path);

            XmlNamespaceManager newnsmgr = new XmlNamespaceManager(csprojfile.NameTable);
            newnsmgr.AddNamespace("ns", "http://schemas.microsoft.com/developer/msbuild/2003");

            XmlNode nodeProject = csprojfile.SelectSingleNode("/ns:Project", newnsmgr);

            //check for nodeimport

            CheckImportNode(csprojfile, nodeProject, @"$(SolutionDir)\SPSF.targets", @"!Exists('$(MSBuildProjectDirectory)\..\SPSF.targets')");
            CheckImportNode(csprojfile, nodeProject, @"$(MSBuildProjectDirectory)\..\SPSF.targets", @"Exists('$(MSBuildProjectDirectory)\..\SPSF.targets')");
            
            /*
             <Import Condition="!Exists('$(MSBuildProjectDirectory)\..\SPSF.targets')" Project="$(SolutionDir)\SPSF.targets" />
             <Import Condition=" Exists('$(MSBuildProjectDirectory)\..\SPSF.targets')" Project="$(MSBuildProjectDirectory)\..\SPSF.targets" />
    
             */
            

            XmlNode nodeBeforeBuild = csprojfile.SelectSingleNode("/ns:Project/ns:Target[@Name='BeforeBuild' and @DependsOnTargets='$(BeforeBuildDependsOn)']", newnsmgr);
            if (nodeBeforeBuild == null)
            {
                XmlNode nodeBeforeBuildOther = csprojfile.SelectSingleNode("/ns:Project/ns:Target[@Name='BeforeBuild']", newnsmgr);
                if (nodeBeforeBuildOther != null)
                {   //remove this node with same name
                    removedContent += nodeBeforeBuildOther.OuterXml + Environment.NewLine;
                    nodeProject.RemoveChild(nodeBeforeBuildOther);
                }
                AddBeforeBuildNode(csprojfile, nodeProject);
            }
            
            XmlNode nodeAfterBuild = csprojfile.SelectSingleNode("/ns:Project/ns:Target[@Name='AfterBuild' and @DependsOnTargets='$(AfterBuildDependsOn)']", newnsmgr);
            if (nodeAfterBuild == null)
            {
                XmlNode nodeAfterBuildOther = csprojfile.SelectSingleNode("/ns:Project/ns:Target[@Name='AfterBuild']", newnsmgr);
                if (nodeAfterBuildOther != null)
                {   //remove this node with same name
                    removedContent += nodeAfterBuildOther.OuterXml +Environment.NewLine;                    
                    nodeProject.RemoveChild(nodeAfterBuildOther);
                }
                AddAfterBuildNode(csprojfile, nodeProject);
            } 

            if (removedContent != "")
            {
                XmlComment comment = csprojfile.CreateComment("Following content has been removed during migration with SPSF" + Environment.NewLine + removedContent);
                nodeProject.AppendChild(comment);
            }

            /*
            <Import Project="$(SolutionDir)\SPSF.targets" />
  <Target Name="BeforeBuild" DependsOnTargets="$(BeforeBuildDependsOn)" />
  <Target Name="AfterBuild" DependsOnTargets="$(AfterBuildDependsOn)" />
             * */

            /*
            foreach (SPSolution solution in SPFarm.Local.Solutions)
            {
                Console.WriteLine(solution.Name);
                Console.WriteLine("Deployed: " + solution.Deployed.ToString());
                Console.WriteLine("JobExists: " + solution.JobExists.ToString());
                Console.WriteLine("LastOperationResult: " + solution.LastOperationResult.ToString());
                Console.WriteLine("LastOperationDetails: " + solution.LastOperationDetails);
                Console.WriteLine("----");
            }
             * */

            csprojfile.Save(@"C:\WSPBuilderProject1_transformed.csproj");

            Console.ReadLine();
        }

        private static void CheckImportNode(XmlDocument csprojfile, XmlNode nodeProject, string projectString,string conditionString)
        {
            XmlNamespaceManager newnsmgr = new XmlNamespaceManager(csprojfile.NameTable);
            newnsmgr.AddNamespace("ns", "http://schemas.microsoft.com/developer/msbuild/2003");
            XmlNode nodeImport = csprojfile.SelectSingleNode("/ns:Project/ns:Import[@Project='" + projectString + "']", newnsmgr);
            if (nodeImport == null)
            {
                //ok, die 1. node ist noch nicht da
                XmlElement importNode = csprojfile.CreateElement("Import", "http://schemas.microsoft.com/developer/msbuild/2003");
                nodeProject.AppendChild(importNode);                
                XmlAttribute importAttribute = csprojfile.CreateAttribute("Project"); //, "http://schemas.microsoft.com/developer/msbuild/2003");
                importAttribute.Value = projectString;
                importNode.Attributes.Append(importAttribute);
                XmlAttribute condiAttribute = csprojfile.CreateAttribute("Condition"); //, "http://schemas.microsoft.com/developer/msbuild/2003");
                condiAttribute.Value = conditionString;
                importNode.Attributes.Append(condiAttribute);
            }
            else
            {
                //ok, die node ist da, ist aber auch die condition richtig?
                if ((nodeImport.Attributes["Condition"] != null) && (nodeImport.Attributes["Condition"].Value.Trim() == conditionString))
                {
                    //ok, alles ist korrekt, wir machen nix
                }
                else
                {
                    //ok, wenn condition da, dann wert setzen, ansonsten Conditionattribute erzeugen
                    if (nodeImport.Attributes["Condition"] != null)
                    {
                        nodeImport.Attributes["Condition"].Value = conditionString;
                    }
                    else
                    {
                        XmlAttribute condiAttribute = csprojfile.CreateAttribute("Condition"); //, "http://schemas.microsoft.com/developer/msbuild/2003");
                        condiAttribute.Value = conditionString;
                        nodeImport.Attributes.Append(condiAttribute);
                    }
                }
            }
        }

        private static void AddAfterBuildNode(XmlDocument csprojfile, XmlNode nodeProject)
        {
            XmlElement afterBuildNode = csprojfile.CreateElement("Target", "http://schemas.microsoft.com/developer/msbuild/2003");
            nodeProject.AppendChild(afterBuildNode);
            XmlAttribute nameAttribute2 = csprojfile.CreateAttribute("Name"); //, "http://schemas.microsoft.com/developer/msbuild/2003");
            nameAttribute2.Value = "AfterBuild";
            afterBuildNode.Attributes.Append(nameAttribute2);
            XmlAttribute dependsAttribute2 = csprojfile.CreateAttribute("DependsOnTargets");//, "http://schemas.microsoft.com/developer/msbuild/2003");
            dependsAttribute2.Value = "$(AfterBuildDependsOn)";
            afterBuildNode.Attributes.Append(dependsAttribute2);
        }

        private static void AddBeforeBuildNode(XmlDocument csprojfile, XmlNode nodeProject)
        {
            XmlElement beforeBuildNode = csprojfile.CreateElement("Target", "http://schemas.microsoft.com/developer/msbuild/2003");
            nodeProject.AppendChild(beforeBuildNode);
            XmlAttribute nameAttribute = csprojfile.CreateAttribute("Name");//, "http://schemas.microsoft.com/developer/msbuild/2003");
            nameAttribute.Value = "BeforeBuild";
            beforeBuildNode.Attributes.Append(nameAttribute);
            XmlAttribute dependsAttribute = csprojfile.CreateAttribute("DependsOnTargets");//, "http://schemas.microsoft.com/developer/msbuild/2003");
            dependsAttribute.Value = "$(BeforeBuildDependsOn)";
            beforeBuildNode.Attributes.Append(dependsAttribute);
        }
    }
}
