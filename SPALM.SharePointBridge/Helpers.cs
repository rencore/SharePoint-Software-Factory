using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SPALM.SharePointBridge
{
	class Helpers
	{
		public static string GetCommonProgramsFolder()
		{
			string commonProgramFolder = System.Environment.GetEnvironmentVariable("CommonProgramW6432");
			if (string.IsNullOrEmpty(commonProgramFolder))
			{
				commonProgramFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
			}
			return commonProgramFolder;
		}

		internal static string GetInstalledSharePointVersion()
		{
			//is there a folder /14/Template, check only for 12 or 14 is not enough because 
			//folder 12 is sometimes also on 14er installations available (with bin folder)
			string check15Folder = GetCommonProgramsFolder() + "\\Microsoft Shared\\web server extensions\\" + 15 + "\\Template";
			if (Directory.Exists(check15Folder))
			{
				return "15";
			}
			return "14";
		}

		internal static string GetSharePointHive()
		{
			return GetCommonProgramsFolder() + "\\Microsoft Shared\\web server extensions\\" + GetInstalledSharePointVersion();
		}
	}
}
