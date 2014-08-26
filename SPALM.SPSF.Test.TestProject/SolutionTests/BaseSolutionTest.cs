using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnvDTE;
using Microsoft.Win32;
using System.IO;
using System.Xml;
using Microsoft.Practices.Common.Services;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace SharePointSoftwareFactory.Tests
{
	[TestClass()]
	public class BaseSolutionTest : BaseTest
	{
		[TestInitialize]
		public new void TestInitialize()
		{			
			//StartVisualStudio();
		}

		[TestCleanup]
        public new void TestCleanup()
		{
			//CloseVisualStudio();
		}
	}
}
