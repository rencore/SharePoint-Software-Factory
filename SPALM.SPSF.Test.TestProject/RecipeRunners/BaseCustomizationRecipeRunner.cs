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
using System.Collections;
using Microsoft.Practices.RecipeFramework.Configuration;
using System.Reflection;

namespace SharePointSoftwareFactory.Tests
{
	public class BaseCustomizationRecipeRunner: BaseRecipeRunner
	{
        public BaseCustomizationRecipeRunner(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, string recipeName, BaseTest parentTest) :
            base(guidancePackage, recipeName, parentTest)
		{
		}

		public override void Init(DTE dte)
		{
			base.Init(dte);

			SelectCustomizationProject(dte);
		}
	}

	public class BaseSiteFeatureRecipeRunner : BaseCustomizationRecipeRunner
	{
		public BaseSiteFeatureRecipeRunner(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, string recipeName, BaseTest parentTest) :
			base(guidancePackage, recipeName, parentTest)
		{
		}

		public override void Init(DTE dte)
		{
			base.Init(dte);
			
			SelectFeature("Site");
		}
	}

	public class BaseFarmFeatureRecipeRunner : BaseCustomizationRecipeRunner
	{
        public BaseFarmFeatureRecipeRunner(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, string recipeName, BaseTest parentTest) :
			base(guidancePackage, recipeName, parentTest)
		{
		}

		public override void Init(DTE dte)
		{
			base.Init(dte);

			SelectFeature("Farm");
		}
	}

  public class BaseWebAppFeatureRecipeRunner : BaseCustomizationRecipeRunner
  {
    public BaseWebAppFeatureRecipeRunner(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, string recipeName, BaseTest parentTest) :
      base(guidancePackage, recipeName, parentTest)
    {
    }

    public override void Init(DTE dte)
    {
      base.Init(dte);

      SelectFeature("WebApplication");
    }
  }

	public class BaseWebFeatureRecipeRunner : BaseCustomizationRecipeRunner
	{
        public BaseWebFeatureRecipeRunner(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, string recipeName, BaseTest parentTest) :
			base(guidancePackage, recipeName, parentTest)
		{
		}

		public override void Init(DTE dte)
		{
			base.Init(dte);

			SelectFeature("Web");
		}
	}
}
