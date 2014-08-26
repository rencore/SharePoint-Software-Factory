namespace SPALM.SPSF.SharePointBridge
{

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;
    using System.IO;
    using System.Configuration;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Collections;
    using System.Net;
    using Microsoft.Win32;

	public static class ExportListHelper
	{
		// Fields
		public const string InternalVersion = "1";

		// Methods
		private static void _Export(Uri siteUrl, Guid listId, string exportLocation)
		{
            using (Microsoft.SharePoint.SPSite site = new Microsoft.SharePoint.SPSite(siteUrl.ToString()))
      {
          using (Microsoft.SharePoint.SPWeb web = site.OpenWeb())
        {
          ExportListSchema(listId, exportLocation, web);
          ExportListFiles(listId, exportLocation, web);
          UpdateContentTypes(listId, exportLocation, web);
          UpdateSummaryView(exportLocation);
          CommentOutExportUnsupportedFields(exportLocation);
          UpdateListSchema(listId, exportLocation, web);
          ApplyFixes(exportLocation);
        }
      }
		}

		private static void CommentOutExportUnsupportedFields(string path)
		{
			string filename = Path.Combine(path, "schema.xml");
			XmlDocument document = new XmlDocument();
			document.Load(filename);
			if (document != null)
			{
				List<string> list = new List<string>();
				list.AddRange(GetNamedEntries("UnsupportedFieldType"));
				using (List<string>.Enumerator enumerator = list.GetEnumerator())
				{
					string str2;
					XmlNodeList list2;
					Regex regex;
					IEnumerator enumerator2;
					goto Label_00F6;
				Label_0041:
					try
					{
						string xpath = "MetaData/Fields/Field[@Type=\"" + str2 + "\" and (@Hidden=\"FALSE\" or count(@Hidden)=0)]";
						list2 = document.DocumentElement.SelectNodes(xpath);
						goto Label_013E;
					}
					catch (XmlException)
					{
						goto Label_00F6;
					}
				Label_006E:
					try
					{
						while (enumerator2.MoveNext())
						{
							XmlNode current = (XmlNode)enumerator2.Current;
							if ((current.Attributes["List"] == null) || regex.IsMatch(current.Attributes["List"].Value))
							{
								XmlNode newChild = current.OwnerDocument.CreateComment(current.OuterXml);
								current.ParentNode.ReplaceChild(newChild, current);
							}
						}
					}
					finally
					{
						IDisposable disposable = enumerator2 as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				Label_00F6:
					if (enumerator.MoveNext())
					{
						goto Label_012D;
					}
					document.Save(filename);
					return;
				Label_0108:
					if (list2.Count <= 0)
					{
						goto Label_00F6;
					}
					regex = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);
					enumerator2 = list2.GetEnumerator();
					goto Label_006E;
				Label_012D:
					str2 = enumerator.Current;
					list2 = null;
					goto Label_0041;
				Label_013E:
					if (list2 != null)
					{
						goto Label_0108;
					}
					goto Label_00F6;
				}
			}
		}

		public static void Export(Uri siteUrl, Guid listId, string exportLocation)
		{
			_Export(siteUrl, listId, exportLocation);
		}

        private static void ExportListFiles(Guid listId, string exportLocation, Microsoft.SharePoint.SPWeb site)
		{
            Microsoft.SharePoint.SPList list = site.Lists[listId];
			if (list.RootFolder != null)
			{
                foreach (Microsoft.SharePoint.SPFile file in list.RootFolder.Files)
				{
					string path = exportLocation + @"\" + file.Name;
					Stream stream = file.OpenBinaryStream();
					if (!IsText(file.Name))
					{
						SaveAsFile(ReadAsByte(stream), path);
					}
					else
					{
						SaveAsFile(ReadAsText(stream), path);
					}
					stream.Close();
				}
			}
		}

        private static void ExportListSchema(Guid listId, string exportLocation, Microsoft.SharePoint.SPWeb site)
		{
			string listSchema = GetListSchema(site, listId);
			FileStream stream = File.Open(exportLocation + @"\schema.xml", FileMode.Create, FileAccess.Write);
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(listSchema);
			writer.Flush();
			writer.Close();
			stream.Close();
		}

		private static string GetFeatureDirectoryPath(Guid featureId)
		{
			string str = string.Empty;
			foreach (string str2 in Directory.GetDirectories(FeaturesDirectoryPath))
			{
				string path = str2.TrimEnd(new char[] { '\\' }) + @"\feature.xml";
				if (File.Exists(path))
				{
					XmlDocument document = new XmlDocument();
					document.Load(path);
					string g = document.DocumentElement.Attributes.GetNamedItem("Id").Value;
					Guid guid = new Guid(g);
					if (featureId.Equals(guid))
					{
						return str2;
					}
				}
			}
			return str;
		}

        private static string GetListSchema(Microsoft.SharePoint.SPWeb web, Guid listId)
		{
			string str = string.Empty;
			HttpWebRequest request = WebRequest.Create(web.Url + "/_vti_bin/owssvr.dll?Cmd=ExportList&List=" + listId.ToString("D")) as HttpWebRequest;
			request.Credentials = CredentialCache.DefaultCredentials;
			HttpWebResponse response = null;
			try
			{
				response = request.GetResponse() as HttpWebResponse;
			}
			catch (WebException)
			{
				throw;
			}
			str = new StreamReader(response.GetResponseStream()).ReadToEnd();
			response.Close();
			return str;
		}

		private static string GetListSchemaFilePath(Guid featureId)
		{
			string path = string.Empty;
			string str2 = string.Empty;
			string featureDirectoryPath = GetFeatureDirectoryPath(featureId);
			string str4 = featureDirectoryPath.TrimEnd(new char[] { '\\' }) + @"\feature.xml";
			if (File.Exists(str4))
			{
				XmlDocument document = new XmlDocument();
				document.Load(str4);
				XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
				nsmgr.AddNamespace("spns", document.DocumentElement.NamespaceURI);
				string str5 = document.DocumentElement.SelectSingleNode("spns:ElementManifests/spns:ElementManifest", nsmgr).Attributes.GetNamedItem("Location").Value;
				str2 = featureDirectoryPath.TrimEnd(new char[] { '\\' }) + @"\" + str5;
			}
			if (!string.IsNullOrEmpty(str2))
			{
				if (!File.Exists(str2))
				{
					return path;
				}
				XmlDocument document2 = new XmlDocument();
				document2.Load(str2);
				XmlNamespaceManager manager2 = new XmlNamespaceManager(document2.NameTable);
				manager2.AddNamespace("spns", document2.DocumentElement.NamespaceURI);
				string str6 = document2.DocumentElement.SelectSingleNode("spns:ListTemplate", manager2).Attributes.GetNamedItem("Name").Value;
				path = featureDirectoryPath.TrimEnd(new char[] { '\\' }) + @"\" + str6 + @"\schema.xml";
				if (!File.Exists(path))
				{
					path = string.Empty;
				}
			}
			return path;
		}

		private static string[] GetNamedEntries(string name)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < ConfigurationManager.AppSettings.Count; i++)
			{
				string str = ConfigurationManager.AppSettings.Keys[i];
				string item = ConfigurationManager.AppSettings[i];
				if (str.StartsWith(name, StringComparison.InvariantCultureIgnoreCase))
				{
					list.Add(item);
				}
			}
			return list.ToArray();
		}

		public static bool IsText(string path)
		{
			List<string> list = null;
			if (list == null)
			{
				string[] collection = new string[] { ".txt", ".htm", ".html", ".asp", ".aspx", ".xml", ".css", ".js" };
				list = new List<string>();
				list.AddRange(collection);
			}
			FileInfo info = new FileInfo(Path.GetFileName(path));
			string item = info.Extension.ToLower(CultureInfo.InvariantCulture);
			if (!list.Contains(item))
			{
				return false;
			}
			return true;
		}

		public static byte[] ReadAsByte(Stream stream)
		{
			BinaryReader reader = new BinaryReader(stream);
			List<byte> list = new List<byte>();
			long num = 0L;
			while (true)
			{
				if (num >= reader.BaseStream.Length)
				{
					break;
				}
				byte item = reader.ReadByte();
				list.Add(item);
				num += 1L;
			}
			reader.Close();
			return list.ToArray();
		}

		public static string ReadAsText(Stream stream)
		{
			StreamReader reader = new StreamReader(stream);
			string str = reader.ReadToEnd();
			reader.Close();
			return str;
		}

		public static string ReplaceUnsafeChar(string source)
		{
			string str = string.Empty;
			foreach (char ch in source.ToCharArray())
			{
				char ch2 = ch;
				if (((('0' > ch) || (ch > '9')) && (('A' > ch) || (ch > 'Z'))) && ((('a' > ch) || (ch > 'z')) && ((ch != '_') && (ch != '-'))))
				{
					string str2 = string.Format(CultureInfo.InvariantCulture, "u{0:X}", new object[] { Convert.ToInt32(ch) });
					str = str + str2;
				}
				else
				{
					str = str + ch2;
				}
			}
			return str;
		}

		public static void SaveAsFile(string contents, string path)
		{
			FileStream stream = File.Create(path);
			StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
			writer.Write(contents);
			writer.Flush();
			writer.Close();
			stream.Close();
		}

		public static void SaveAsFile(byte[] contents, string path)
		{
			FileStream output = File.Create(path);
			BinaryWriter writer = new BinaryWriter(output);
			writer.Write(contents);
			writer.Flush();
			writer.Close();
			output.Close();
		}

        private static void UpdateContentTypes(Guid listId, string path, Microsoft.SharePoint.SPWeb site)
		{
			string filename = Path.Combine(path, "schema.xml");
			XmlDocument document = new XmlDocument();
			document.Load(filename);
			if (document != null)
			{
				XmlNode node = document.DocumentElement.SelectSingleNode("MetaData/ContentTypes");
				if (node != null)
				{
					Guid uniqueId = new Guid(document.DocumentElement.Attributes["Name"].Value);
                    foreach (Microsoft.SharePoint.SPContentType type in site.Lists.GetList(uniqueId, true).ContentTypes)
					{
						string strA = type.Parent.Id.ToString();
						if ((type.Version <= 0) && ((string.Compare(strA, "0x0120", StringComparison.CurrentCulture) != 0) || (string.Compare(strA, "0x0107", StringComparison.CurrentCulture) == 0)))
						{
							string xpath = "ContentType[@ID=\"" + type.Id.ToString() + "\"]";
							XmlNode oldChild = node.SelectSingleNode(xpath);
							XmlNode newChild = oldChild.OwnerDocument.CreateComment(oldChild.OuterXml);
							oldChild.ParentNode.ReplaceChild(newChild, oldChild);
							XmlNode node4 = oldChild.OwnerDocument.CreateElement("ContentTypeRef");
							XmlAttribute attribute = oldChild.OwnerDocument.CreateAttribute("ID");
							attribute.Value = strA;
							node4.Attributes.Append(attribute);
							newChild.ParentNode.InsertBefore(node4, newChild);
						}
					}
					document.Save(filename);
				}
			}
		}

		private static void ApplyFixes(string exportLocation)
		{
			XmlNode documentElement;
			string filename = exportLocation + @"\schema.xml";
			XmlDocument document = new XmlDocument();
			document.Load(filename);
			if (document != null)
			{
				documentElement = document.DocumentElement;

				XmlNodeList colnamefixes = document.DocumentElement.SelectNodes("MetaData/Fields/Field");
				foreach (XmlNode colnamefix in colnamefixes)
				{
					if (colnamefix.Attributes["ColName"] != null)
					{
						string coltype = colnamefix.Attributes["ColName"].Value;
						if (coltype.StartsWith("varchar") ||
							coltype.StartsWith("text") ||
							coltype.StartsWith("ntext") ||
							coltype.StartsWith("nvarchar") ||
							coltype.StartsWith("datetime"))
						{
							XmlAttribute attrib = colnamefix.Attributes["ColName"];
							colnamefix.Attributes.Remove(attrib);
						}
					}
				}

				//http://msmvps.com/blogs/laflour/archive/2008/05/14/export-list-definition-with-ocdexportlist-and-reusing-standard-views.aspx
				XmlNodeList formfixes = document.DocumentElement.SelectNodes("MetaData/Forms/Form");
				foreach (XmlNode formfix in formfixes)
				{
					if (formfix.Attributes["SetupPath"] == null)
					{
						XmlAttribute attrib = document.CreateAttribute("SetupPath");
						attrib.Value = "pages\\form.aspx";
						formfix.Attributes.Append(attrib);
					}
				}

				//<View DefaultView="TRUE" Type="HTML" OrderedView="TRUE" DisplayName="All Links" Url="AllItems.aspx"  
				XmlNodeList viewfixes = document.DocumentElement.SelectNodes("MetaData/Views/View");
				foreach (XmlNode viewfix in viewfixes)
				{
					if (viewfix.Attributes["Url"] != null)
					{
						if (viewfix.Attributes["Url"].Value == "AllItems.aspx")
						{

							if (viewfix.Attributes["SetupPath"] == null)
							{
								XmlAttribute attrib = document.CreateAttribute("SetupPath");
								attrib.Value = "pages\\form.aspx";
								viewfix.Attributes.Append(attrib);

								if (viewfix.Attributes["DisplayName"] != null)
								{
									viewfix.Attributes["DisplayName"].Value = "$Resources:core,objectiv_schema_mwsidcamlidC24;";
								}
							}
						}
					}
				}
			}
			//mandelkow fixes end
			document.Save(filename);
		}

        private static void UpdateListSchema(Guid listId, string exportLocation, Microsoft.SharePoint.SPWeb site)
		{
			XmlNode documentElement;
            Microsoft.SharePoint.SPList list;
			string filename = exportLocation + @"\schema.xml";
			XmlDocument document = new XmlDocument();
			document.Load(filename);
			if (document != null)
			{
				documentElement = document.DocumentElement;
				new Guid(documentElement.Attributes["Name"].Value);
				list = site.Lists.GetList(listId, true);
				if (list.EnableModeration && ((documentElement.Attributes["ModeratedList"] == null) || (documentElement.Attributes["ModeratedList"].Value.ToLower(CultureInfo.CurrentCulture) == "false")))
				{
					XmlAttribute node = document.CreateAttribute("ModeratedList");
					node.Value = "TRUE";
					documentElement.Attributes.Append(node);
				}
			}
			else
			{
				return;
			}
			XmlAttribute attribute2 = document.CreateAttribute("Type");
			attribute2.Value = documentElement.Attributes["ServerTemplate"].Value;
			documentElement.Attributes.Append(attribute2);
			documentElement.Attributes.RemoveNamedItem("FeatureId");
			documentElement.Attributes.RemoveNamedItem("ServerTemplate");
			documentElement.Attributes.RemoveNamedItem("Version");
			documentElement.Attributes["Name"].Value = ReplaceUnsafeChar(documentElement.Attributes["Title"].Value);
			XmlAttribute attribute4 = document.CreateAttribute("Id");
			attribute4.Value = Guid.NewGuid().ToString();
			documentElement.Attributes.Append(attribute4);

			XmlNode node4 = document.DocumentElement.SelectSingleNode("MetaData/Views");
			List<string> list2 = new List<string>();
			IEnumerator enumerator = node4.GetEnumerator();

			XmlNode node5;
			string str2;
		Label_022C:
			if (enumerator.MoveNext())
			{
				goto Label_061D;
			}
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			IEnumerator enumerator2 = node4.GetEnumerator();
			XmlNode node6;
			string str3;
			string str4;
			XmlAttribute attribute5;
			int num;
			goto Label_0493;
		Label_0254:
			node6.Attributes.RemoveNamedItem("Name");
			goto Label_03E3;
		Label_026B:
			if (node6.Attributes["Name"] != null)
			{
				goto Label_0254;
			}
			goto Label_03E3;
		Label_0283:
			num = -1;
			int num2 = 1;
			goto Label_045C;
		Label_028E:
			node6 = (XmlNode)enumerator2.Current;
			if (node6.Attributes["Url"] != null)
			{
				goto Label_038C;
			}
			goto Label_026B;
		Label_02B4:
			attribute5 = document.CreateAttribute("WebPartZoneID");
			attribute5.Value = "Main";
			node6.Attributes.Append(attribute5);
			if (list3.Contains(str4))
			{
				goto Label_0283;
			}
			goto Label_0361;
		Label_02EA:
			if (node6.Attributes["Url"] != null)
			{
				goto Label_03CC;
			}
			goto Label_0493;
		Label_0305:
			if (node6.Attributes["Hidden"].Value.ToLower(CultureInfo.CurrentCulture) == "true")
			{
				goto Label_03B7;
			}
			goto Label_0361;
		Label_0336:
			str4 = node6.Attributes["BaseViewID"].Value;
			if (str4 == "0")
			{
				goto Label_02EA;
			}
			goto Label_02B4;
		Label_0361:
			list3.Add(str4);
			goto Label_0493;
		Label_036F:
			node6.Attributes["Url"].Value = str3;
			goto Label_026B;
		Label_038C:
			str3 = Path.GetFileName(node6.Attributes["Url"].Value);
        if (list.BaseType == Microsoft.SharePoint.SPBaseType.DocumentLibrary)
			{
				goto Label_0449;
			}
			goto Label_036F;
		Label_03B7:
			list4.Add(num.ToString(CultureInfo.CurrentCulture));
			goto Label_0361;
		Label_03CC:
			node6.Attributes.RemoveNamedItem("Url");
			goto Label_0493;
		Label_03E3:
			if (node6.Attributes["BaseViewID"] != null)
			{
				goto Label_0336;
			}
			goto Label_0493;
		Label_03FE:
			node6.Attributes["BaseViewID"].Value = Convert.ToString(num, CultureInfo.CurrentCulture);
			str4 = num.ToString(CultureInfo.CurrentCulture);
			if (node6.Attributes["Hidden"] != null)
			{
				goto Label_0305;
			}
			goto Label_0361;
		Label_0449:
			str3 = "Forms/" + str3;
			goto Label_036F;
		Label_045C:
			if (!list2.Contains(num2.ToString(CultureInfo.CurrentCulture)))
			{
				goto Label_0479;
			}
		Label_0471:
			num2++;
			goto Label_045C;
		Label_0479:
			if (!list3.Contains(num2.ToString(CultureInfo.CurrentCulture)))
			{
				goto Label_05D9;
			}
			goto Label_0471;
		Label_0493:
			if (enumerator2.MoveNext())
			{
				goto Label_028E;
			}
			foreach (string str5 in list4)
			{
				XmlNode oldChild = node4.SelectSingleNode("View[@BaseViewID='" + str5 + "']");
				node4.RemoveChild(oldChild);
			}
			IEnumerator enumerator4 = document.DocumentElement.SelectSingleNode("MetaData/Forms").GetEnumerator();

			XmlNode current;
			string fileName;
			goto Label_05AE;
		Label_04FD:
			fileName = "Forms/" + fileName;
		Label_050B:
			current.Attributes["Url"].Value = fileName;
			current.Attributes.RemoveNamedItem("Name");
			current.Attributes.RemoveNamedItem("Default");
			XmlAttribute attribute6 = document.CreateAttribute("WebPartZoneID");
			attribute6.Value = "Main";
			current.Attributes.Append(attribute6);
		Label_05AE:
			while (enumerator4.MoveNext())
			{
				current = (XmlNode)enumerator4.Current;
				fileName = Path.GetFileName(current.Attributes["Url"].Value);
                if (list.BaseType == Microsoft.SharePoint.SPBaseType.DocumentLibrary)
				{
					goto Label_04FD;
				}
				goto Label_050B;
			}
			if (document.DocumentElement.SelectSingleNode("MetaData/Toolbar") == null)
			{
				XmlNode node2 = document.DocumentElement.SelectSingleNode("MetaData");
				XmlNode newChild = document.CreateElement("Toolbar");
				node2.AppendChild(newChild);
				XmlAttribute attribute3 = document.CreateAttribute("Type");
				attribute3.Value = "RelatedTasks";
				newChild.Attributes.Append(attribute3);
			}
			document.Save(filename);
			return;
		Label_05D9:
			num = num2;
			goto Label_03FE;
		Label_05F7:
			str2 = node5.Attributes["BaseViewID"].Value;
			list2.Add(str2);
			goto Label_022C;
		Label_061D:
			node5 = (XmlNode)enumerator.Current;
			if (node5.Attributes["BaseViewID"] != null)
			{
				goto Label_05F7;
			}
			goto Label_022C;

		}

		private static void UpdateSummaryView(string path)
		{
			string filename = path + @"\schema.xml";
			XmlDocument document = new XmlDocument();
			document.Load(filename);
			if (document != null)
			{
				XmlNode node = document.DocumentElement.SelectSingleNode("MetaData/Views");
				IEnumerator enumerator = node.ChildNodes.GetEnumerator();

				XmlNode node3;
				int num;
			Label_003D:
				if (enumerator.MoveNext())
				{
					goto Label_0172;
				}
				if (document.DocumentElement.Attributes["FeatureId"] != null)
				{
					string g = document.DocumentElement.Attributes.GetNamedItem("FeatureId").Value;
					Guid featureId = new Guid(g);
					string listSchemaFilePath = GetListSchemaFilePath(featureId);
					XmlDocument document2 = new XmlDocument();
					document2.Load(listSchemaFilePath);
					if (document2 == null)
					{
						return;
					}
					XmlNode node2 = null;
					foreach (XmlNode node4 in document2.DocumentElement.SelectSingleNode("MetaData/Views").ChildNodes)
					{
						if (node4.Attributes["BaseViewID"] != null)
						{
							int num2 = 0;
							if (node4.Attributes["BaseViewID"].Value == num2.ToString(CultureInfo.CurrentCulture))
							{
								node2 = node4;
								break;
							}
						}
					}
					if (node2 != null)
					{
						node.InnerXml = node2.OuterXml + node.InnerXml;
					}
					document.Save(filename);
				}
				return;
			Label_0141:
				num = 0;
				if (!(node3.Attributes["BaseViewID"].Value == num.ToString(CultureInfo.CurrentCulture)))
				{
					goto Label_003D;
				}
				return;
			Label_0172:
				node3 = (XmlNode)enumerator.Current;
				if (node3.Attributes["BaseViewID"] != null)
				{
					goto Label_0141;
				}
				goto Label_003D;
			}

			//mandelkow fixes
			//http://social.msdn.microsoft.com/Forums/en-US/sharepointdevelopment/thread/f5a30776-6dde-4422-bc0b-65d0a80f1a56
			//text, varchar, ntext,nvarchar, colname=datetime


		}

		// Properties
		private static string FeaturesDirectoryPath
		{
			get
			{
				string name = @"SOFTWARE\Microsoft\Shared Tools\Web Server Extensions\12.0";
				string str3 = "Location";
				string str4 = string.Empty;
				RegistryKey key = Registry.LocalMachine.OpenSubKey(name);
				str4 = key.GetValue(str3) as string;
				key.Close();
				return (str4.TrimEnd(new char[] { '\\' }) + @"\template\features");
			}
		}
	}
}
