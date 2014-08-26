
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
using SPALM.SPSF.Library;
using Microsoft.VSSDK.Tools.VsIdeTesting;

namespace SharePointSoftwareFactory.Tests
{ 
		public partial class RecipeRunner_AdministrationPage : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_AdministrationPage(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "AdministrationPage", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_DefaultAdministrationPage()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"DefaultAdministrationPage";
						arguments.Add("ApplicationPageName", argObject);					
					
						argObject = @"Custom Administration Page";
						arguments.Add("ApplicationPageTitle", argObject);					
					
						argObject = @"Description of Custom Administration Page";
						arguments.Add("ApplicationPageDescription", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("Administration Page with title 'Administration Page' exists in central administration /_layouts/SolutionName/DefaultAdministrationPage.aspx");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultAdministrationPage();

								}
		}
				public partial class RecipeRunner_ApplicationPage : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_ApplicationPage(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "ApplicationPage", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_DefaultApplicationPage()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"DefaultApplicationPage";
						arguments.Add("ApplicationPageName", argObject);					
					
						argObject = @"Custom Application Page";
						arguments.Add("ApplicationPageTitle", argObject);					
					
						argObject = @"Description of Custom Application Page";
						arguments.Add("ApplicationPageDescription", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("Application Page with title 'Application Page' exists in site /_layouts/SolutionName/DefaultApplicationPage.aspx");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultApplicationPage();

								}
		}
				public partial class RecipeRunner_UnsecuredApplicationPage : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_UnsecuredApplicationPage(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "UnsecuredApplicationPage", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_DefaultApplicationPage()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"DefaultUnsecuredApplicationPage";
						arguments.Add("ApplicationPageName", argObject);					
					
						argObject = @"Custom Unsecured Application Page";
						arguments.Add("ApplicationPageTitle", argObject);					
					
						argObject = @"Description of Custom Unsecured Application Page";
						arguments.Add("ApplicationPageDescription", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("Application Page with title 'Unsecured Application Page' exists in site /_layouts/SolutionName/DefaultUnsecuredApplicationPage.aspx");
										}
								public void RunTestCase_AnonymousApplicationPage()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"AnonymousApplicationPage";
						arguments.Add("ApplicationPageName", argObject);					
					
						argObject = @"Anonymous Application Page";
						arguments.Add("ApplicationPageTitle", argObject);					
					
						argObject = @"Description of Anonymous Application Page";
						arguments.Add("ApplicationPageDescription", argObject);					
					
						argObject = @"True";
						arguments.Add("ApplicationPageAllowAnonymous", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("Application Page with AllowAnonymous = true and title 'Anonymous Application Page' exists in site /_layouts/SolutionName/DefaultUnsecuredApplicationPage.aspx");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultApplicationPage();

										RunTestCase_AnonymousApplicationPage();

								}
		}
				public partial class RecipeRunner_AdministrationWebService : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_AdministrationWebService(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "AdministrationWebService", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_DefaultAdministrationWebService()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"AdministrationWebService";
						arguments.Add("WebServiceName", argObject);					
					
						argObject = @"Description of AdministrationWebService";
						arguments.Add("WebServiceDescription", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("Webservice exists in site /_vti_adm/SolutionName/DefaultAdministrationWebService.asmx");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultAdministrationWebService();

								}
		}
				public partial class RecipeRunner_DelegateControlASCX : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_DelegateControlASCX(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "DelegateControlASCX", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_DefaultDelegateControlASCX()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"DelegateControlASCX";
						arguments.Add("DelegateControlName", argObject);					
					
						argObject = @"AdditionalPageHead";
						arguments.Add("DelegateControlId", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("In page content of page http://demo2010a/SitePages/Home.aspx a delegate in header is available");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultDelegateControlASCX();

								}
		}
				public partial class RecipeRunner_DelegateControlCS : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_DelegateControlCS(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "DelegateControlCS", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_DefaultDelegateControlCS()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"DelegateControlCS";
						arguments.Add("DelegateControlName", argObject);					
					
						argObject = @"AdditionalPageHead";
						arguments.Add("DelegateControlId", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("In page content of page http://demo2010a/SitePages/Home.aspx a delegate in header is available");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultDelegateControlCS();

								}
		}
				public partial class RecipeRunner_HttpHandler : BaseWebAppFeatureRecipeRunner		{		
			public RecipeRunner_HttpHandler(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "HttpHandler", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_DefaultHttpHandler()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"DefaultHttpHandler";
						arguments.Add("HttpHandlerClass", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultHttpHandler();

								}
		}
				public partial class RecipeRunner_HttpModule : BaseWebAppFeatureRecipeRunner		{		
			public RecipeRunner_HttpModule(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "HttpModule", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_DefaultHttpModule()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"DefaultHttpModule";
						arguments.Add("HttpModuleClass", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultHttpModule();

								}
		}
				public partial class RecipeRunner_WCFWebService : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_WCFWebService(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "WCFWebService", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_DefaultWebService()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"WebService";
						arguments.Add("WebServiceName", argObject);					
					
						argObject = @"Description of WebService";
						arguments.Add("WebServiceDescription", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("Webservice exists in site /_vti_adm/SolutionName/WebService.asmx");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultWebService();

								}
		}
				public partial class RecipeRunner_WebService : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_WebService(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "WebService", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_DefaultWebService()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"WebService";
						arguments.Add("WebServiceName", argObject);					
					
						argObject = @"Description of WebService";
						arguments.Add("WebServiceDescription", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("Webservice exists in site /_vti_adm/SolutionName/WebService.asmx");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultWebService();

								}
		}
				public partial class RecipeRunner_BCSDesignWithAssembly : BCSBaseTest		{		
			public RecipeRunner_BCSDesignWithAssembly(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "BCSDesignWithAssembly", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_BCSListInstance : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_BCSListInstance(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "BCSListInstance", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_BCSModelFromDatabaseWithClasses : BCSBaseTest		{		
			public RecipeRunner_BCSModelFromDatabaseWithClasses(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "BCSModelFromDatabaseWithClasses", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_BCSModelFromDatabaseWithLINQ : BCSBaseTest		{		
			public RecipeRunner_BCSModelFromDatabaseWithLINQ(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "BCSModelFromDatabaseWithLINQ", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_BCSModelFromDatabaseWithSQLQueries : BCSBaseTest		{		
			public RecipeRunner_BCSModelFromDatabaseWithSQLQueries(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "BCSModelFromDatabaseWithSQLQueries", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_BCSQuickDeploy : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_BCSQuickDeploy(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "BCSQuickDeploy", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_BCSQuickDeployIncAssembly : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_BCSQuickDeployIncAssembly(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "BCSQuickDeployIncAssembly", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_EmptyBCSModel : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_EmptyBCSModel(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "EmptyBCSModel", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_CustomActionContentType : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_CustomActionContentType(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "CustomActionContentType", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_CustomActionContentTypeWithUrlAction()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"UrlAction";
						arguments.Add("CustomActionName", argObject);					
					
						argObject = @"UrlActionTag";
						arguments.Add("CustomActionType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_CustomActionContentTypeWithControlClass()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ControlClass";
						arguments.Add("CustomActionName", argObject);					
					
						argObject = @"ClassFile";
						arguments.Add("CustomActionType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_CustomActionContentTypeWithControlTemplate()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ControlTemplate";
						arguments.Add("CustomActionName", argObject);					
					
						argObject = @"WebControl";
						arguments.Add("CustomActionType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_CustomActionContentTypeWithUrlAction();

										RunTestCase_CustomActionContentTypeWithControlClass();

										RunTestCase_CustomActionContentTypeWithControlTemplate();

								}
		}
				public partial class RecipeRunner_CustomActionFileType : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_CustomActionFileType(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "CustomActionFileType", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_CustomActionFileTypeWithUrlAction()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"FileTypeActionUrlAction";
						arguments.Add("CustomActionName", argObject);					
					
						argObject = @"UrlActionTag";
						arguments.Add("CustomActionType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_CustomActionFileTypeWithControlClass()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"FileTypeActionControlClass";
						arguments.Add("CustomActionName", argObject);					
					
						argObject = @"ClassFile";
						arguments.Add("CustomActionType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_CustomActionFileTypeWithControlTemplate()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"FileTypeActionControlTemplate";
						arguments.Add("CustomActionName", argObject);					
					
						argObject = @"WebControl";
						arguments.Add("CustomActionType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_CustomActionFileTypeWithUrlAction();

										RunTestCase_CustomActionFileTypeWithControlClass();

										RunTestCase_CustomActionFileTypeWithControlTemplate();

								}
		}
				public partial class RecipeRunner_CustomActionGroup : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_CustomActionGroup(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "CustomActionGroup", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_CustomActionGroup()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"CustomActionGroupSiteSettings";
						arguments.Add("CustomActionName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_CustomActionGroup();

								}
		}
				public partial class RecipeRunner_CustomActionList : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_CustomActionList(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "CustomActionList", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_CustomActionListWithUrlAction()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ListActionUrlAction";
						arguments.Add("CustomActionName", argObject);					
					
						argObject = @"UrlActionTag";
						arguments.Add("CustomActionType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_CustomActionListWithControlClass()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ListActionControlClass";
						arguments.Add("CustomActionName", argObject);					
					
						argObject = @"ClassFile";
						arguments.Add("CustomActionType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_CustomActionListWithControlTemplate()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ListActionControlTemplate";
						arguments.Add("CustomActionName", argObject);					
					
						argObject = @"WebControl";
						arguments.Add("CustomActionType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_CustomActionListWithUrlAction();

										RunTestCase_CustomActionListWithControlClass();

										RunTestCase_CustomActionListWithControlTemplate();

								}
		}
				public partial class RecipeRunner_CustomActionListToolbar : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_CustomActionListToolbar(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "CustomActionListToolbar", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_CustomActionListToolbarWithUrlAction()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ToolbarActionUrlAction";
						arguments.Add("CustomActionName", argObject);					
					
						argObject = @"UrlActionTag";
						arguments.Add("CustomActionType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_CustomActionListToolbarWithControlClass()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ToolbarActionControlClass";
						arguments.Add("CustomActionName", argObject);					
					
						argObject = @"ClassFile";
						arguments.Add("CustomActionType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_CustomActionListToolbarWithControlTemplate()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ToolbarActionControlTemplate";
						arguments.Add("CustomActionName", argObject);					
					
						argObject = @"WebControl";
						arguments.Add("CustomActionType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_CustomActionListToolbarWithUrlAction();

										RunTestCase_CustomActionListToolbarWithControlClass();

										RunTestCase_CustomActionListToolbarWithControlTemplate();

								}
		}
				public partial class RecipeRunner_CustomActionProgId : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_CustomActionProgId(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "CustomActionProgId", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_CustomActionProgIdWithUrlAction()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ProgIdActionUrlAction";
						arguments.Add("CustomActionName", argObject);					
					
						argObject = @"UrlActionTag";
						arguments.Add("CustomActionType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_CustomActionProgIdWithControlClass()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ProgIdActionControlClass";
						arguments.Add("CustomActionName", argObject);					
					
						argObject = @"ClassFile";
						arguments.Add("CustomActionType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_CustomActionProgIdWithControlTemplate()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ProgIdActionControlTemplate";
						arguments.Add("CustomActionName", argObject);					
					
						argObject = @"WebControl";
						arguments.Add("CustomActionType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_CustomActionProgIdWithUrlAction();

										RunTestCase_CustomActionProgIdWithControlClass();

										RunTestCase_CustomActionProgIdWithControlTemplate();

								}
		}
				public partial class RecipeRunner_CustomActionSite : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_CustomActionSite(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "CustomActionSite", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_CustomActionSiteWithUrlAction()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"SiteActionUrlAction";
						arguments.Add("CustomActionName", argObject);					
					
						argObject = @"UrlActionTag";
						arguments.Add("CustomActionType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_CustomActionSiteWithControlClass()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"SiteActionControlClass";
						arguments.Add("CustomActionName", argObject);					
					
						argObject = @"ClassFile";
						arguments.Add("CustomActionType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_CustomActionSiteWithControlTemplate()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"SiteActionControlTemplate";
						arguments.Add("CustomActionName", argObject);					
					
						argObject = @"WebControl";
						arguments.Add("CustomActionType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_CustomActionSiteWithUrlAction();

										RunTestCase_CustomActionSiteWithControlClass();

										RunTestCase_CustomActionSiteWithControlTemplate();

								}
		}
				public partial class RecipeRunner_CustomActionSiteActionsMenu : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_CustomActionSiteActionsMenu(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "CustomActionSiteActionsMenu", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_CustomSiteActionWithUrlAction()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"SiteActionsMenuUrlAction";
						arguments.Add("CustomActionName", argObject);					
					
						argObject = @"UrlActionTag";
						arguments.Add("CustomActionType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_CustomSiteActionWithControlClass()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"SiteActionsMenuControlClass";
						arguments.Add("CustomActionName", argObject);					
					
						argObject = @"ClassFile";
						arguments.Add("CustomActionType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_CustomSiteActionWithControlTemplate()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"SiteActionsMenuControlTemplate";
						arguments.Add("CustomActionName", argObject);					
					
						argObject = @"WebControl";
						arguments.Add("CustomActionType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_CustomSiteActionWithUrlAction();

										RunTestCase_CustomSiteActionWithControlClass();

										RunTestCase_CustomSiteActionWithControlTemplate();

								}
		}
				public partial class RecipeRunner_HideCustomAction : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_HideCustomAction(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "HideCustomAction", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_HideCustomActionWithUrlAction()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"HideCustomActionDeleteWeb";
						arguments.Add("CustomActionName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_HideCustomActionWithUrlAction();

								}
		}
				public partial class RecipeRunner_EmailEventReceiver : BaseWebFeatureRecipeRunner		{		
			public RecipeRunner_EmailEventReceiver(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "EmailEventReceiver", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_Default()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"EmailEvent";
						arguments.Add("EventReceiverName", argObject);					
					
						List<NameValueItem> tempArray = new List<NameValueItem>();
							tempArray.Add(new NameValueItem("EmailReceived","EmailReceived","EmailReceived"));
																	argObject = tempArray.ToArray();
							
						arguments.Add("EventReceiverTypes", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_Default();

								}
		}
				public partial class RecipeRunner_ItemEventReceiver : BaseWebFeatureRecipeRunner		{		
			public RecipeRunner_ItemEventReceiver(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "ItemEventReceiver", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_Default()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ItemEvent";
						arguments.Add("EventReceiverName", argObject);					
					
						List<NameValueItem> tempArray = new List<NameValueItem>();
							tempArray.Add(new NameValueItem("ItemAdding","ItemAdding","ItemAdding"));
										tempArray.Add(new NameValueItem("ItemDeleting","ItemDeleting","ItemDeleting"));
																	argObject = tempArray.ToArray();
							
						arguments.Add("EventReceiverTypes", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_ItemEventWithScopeSite()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ItemEventWithScopeSite";
						arguments.Add("EventReceiverName", argObject);					
					
						List<NameValueItem> tempArray = new List<NameValueItem>();
							tempArray.Add(new NameValueItem("ItemAdding","ItemAdding","ItemAdding"));
										tempArray.Add(new NameValueItem("ItemDeleting","ItemDeleting","ItemDeleting"));
																	argObject = tempArray.ToArray();
							
						arguments.Add("EventReceiverTypes", argObject);					
					
						argObject = @"Site";
						arguments.Add("EventReceiverScope", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_ItemEventWithRootWebOnly()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ItemEventWithRootWebOnly";
						arguments.Add("EventReceiverName", argObject);					
					
						List<NameValueItem> tempArray = new List<NameValueItem>();
							tempArray.Add(new NameValueItem("ItemAdding","ItemAdding","ItemAdding"));
										tempArray.Add(new NameValueItem("ItemDeleting","ItemDeleting","ItemDeleting"));
																	argObject = tempArray.ToArray();
							
						arguments.Add("EventReceiverTypes", argObject);					
					
						argObject = @"True";
						arguments.Add("EventReceiverRootWebOnly", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_Default();

										RunTestCase_ItemEventWithScopeSite();

										RunTestCase_ItemEventWithRootWebOnly();

								}
		}
				public partial class RecipeRunner_ListEventReceiver : BaseWebFeatureRecipeRunner		{		
			public RecipeRunner_ListEventReceiver(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "ListEventReceiver", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_Default()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ListEvent";
						arguments.Add("EventReceiverName", argObject);					
					
						List<NameValueItem> tempArray = new List<NameValueItem>();
							tempArray.Add(new NameValueItem("FieldDeleted","FieldDeleted","FieldDeleted"));
										tempArray.Add(new NameValueItem("FieldUpdated","FieldUpdated","FieldUpdated"));
																	argObject = tempArray.ToArray();
							
						arguments.Add("EventReceiverTypes", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_ListEventWithScopeSite()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ListEventWithScopeSite";
						arguments.Add("EventReceiverName", argObject);					
					
						List<NameValueItem> tempArray = new List<NameValueItem>();
							tempArray.Add(new NameValueItem("FieldDeleted","FieldDeleted","FieldDeleted"));
										tempArray.Add(new NameValueItem("FieldUpdated","FieldUpdated","FieldUpdated"));
																	argObject = tempArray.ToArray();
							
						arguments.Add("EventReceiverTypes", argObject);					
					
						argObject = @"Site";
						arguments.Add("EventReceiverScope", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_ListEventWithRootWebOnly()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ListEventWithRootWebOnly";
						arguments.Add("EventReceiverName", argObject);					
					
						List<NameValueItem> tempArray = new List<NameValueItem>();
							tempArray.Add(new NameValueItem("FieldDeleted","FieldDeleted","FieldDeleted"));
										tempArray.Add(new NameValueItem("FieldUpdated","FieldUpdated","FieldUpdated"));
																	argObject = tempArray.ToArray();
							
						arguments.Add("EventReceiverTypes", argObject);					
					
						argObject = @"True";
						arguments.Add("EventReceiverRootWebOnly", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_Default();

										RunTestCase_ListEventWithScopeSite();

										RunTestCase_ListEventWithRootWebOnly();

								}
		}
				public partial class RecipeRunner_WebEventReceiver : BaseWebFeatureRecipeRunner		{		
			public RecipeRunner_WebEventReceiver(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "WebEventReceiver", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_Default()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"WebEvent";
						arguments.Add("EventReceiverName", argObject);					
					
						List<NameValueItem> tempArray = new List<NameValueItem>();
							tempArray.Add(new NameValueItem("SiteDeleted","SiteDeleted","SiteDeleted"));
										tempArray.Add(new NameValueItem("WebDeleted","WebDeleted","WebDeleted"));
																	argObject = tempArray.ToArray();
							
						arguments.Add("EventReceiverTypes", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_WebEventWithScopeSite()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"WebEventWithScopeSite";
						arguments.Add("EventReceiverName", argObject);					
					
						List<NameValueItem> tempArray = new List<NameValueItem>();
							tempArray.Add(new NameValueItem("SiteDeleted","SiteDeleted","SiteDeleted"));
										tempArray.Add(new NameValueItem("WebDeleted","WebDeleted","WebDeleted"));
																	argObject = tempArray.ToArray();
							
						arguments.Add("EventReceiverTypes", argObject);					
					
						argObject = @"Site";
						arguments.Add("EventReceiverScope", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_WebEventWithRootWebOnly()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"WebEventWithRootWebOnly";
						arguments.Add("EventReceiverName", argObject);					
					
						List<NameValueItem> tempArray = new List<NameValueItem>();
							tempArray.Add(new NameValueItem("SiteDeleted","SiteDeleted","SiteDeleted"));
										tempArray.Add(new NameValueItem("WebDeleted","WebDeleted","WebDeleted"));
																	argObject = tempArray.ToArray();
							
						arguments.Add("EventReceiverTypes", argObject);					
					
						argObject = @"True";
						arguments.Add("EventReceiverRootWebOnly", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_Default();

										RunTestCase_WebEventWithScopeSite();

										RunTestCase_WebEventWithRootWebOnly();

								}
		}
				public partial class RecipeRunner_EmptyFeature : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_EmptyFeature(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "EmptyFeature", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_SiteFeature()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"SiteFeature";
						arguments.Add("FeatureName", argObject);					
					
						argObject = @"Site";
						arguments.Add("FeatureScope", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_FeatureWithReceiver()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"SiteFeatureWithReceiver";
						arguments.Add("FeatureName", argObject);					
					
						argObject = @"Site";
						arguments.Add("FeatureScope", argObject);					
					
						argObject = @"True";
						arguments.Add("FeatureCreateReceiver", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_WebFeature()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"WebFeature";
						arguments.Add("FeatureName", argObject);					
					
						argObject = @"Web";
						arguments.Add("FeatureScope", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_FeatureWithDependencies()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"WebFeature";
						arguments.Add("FeatureName", argObject);					
					
						argObject = @"Web";
						arguments.Add("FeatureScope", argObject);					
					
						List<NameValueItem> tempArray = new List<NameValueItem>();
							tempArray.Add(new NameValueItem("Dependency","External Lists","00BFEA71-9549-43f8-B978-E47E54A10600"));
										tempArray.Add(new NameValueItem("Dependency","Resources List","58160a6b-4396-4d6e-867c-65381fb5fbc9"));
																	argObject = tempArray.ToArray();
							
						arguments.Add("FeatureActivationDependencies", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_SiteFeature();

										RunTestCase_FeatureWithReceiver();

										RunTestCase_WebFeature();

										RunTestCase_FeatureWithDependencies();

								}
		}
				public partial class RecipeRunner_EmptyFeatureActivationDependency14 : BaseRecipeRunner		{		
			public RecipeRunner_EmptyFeatureActivationDependency14(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "EmptyFeatureActivationDependency14", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_EmptyFeatureProperty : BaseRecipeRunner		{		
			public RecipeRunner_EmptyFeatureProperty(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "EmptyFeatureProperty", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_EmptyFeatureUpgradeActionContentType : BaseRecipeRunner		{		
			public RecipeRunner_EmptyFeatureUpgradeActionContentType(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "EmptyFeatureUpgradeActionContentType", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_FieldType : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_FieldType(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "FieldType", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_FieldTypeDefault()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"DefaultFieldType";
						arguments.Add("FieldTypeTypeName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_FieldTypeDefault();

								}
		}
				public partial class RecipeRunner_FieldTypeChoice : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_FieldTypeChoice(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "FieldTypeChoice", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_FieldTypeChoice()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ChoiceFieldType";
						arguments.Add("FieldTypeTypeName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_FieldTypeChoice();

								}
		}
				public partial class RecipeRunner_FieldTypeComplexValue : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_FieldTypeComplexValue(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "FieldTypeComplexValue", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_FieldTypeComplexValue()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ComplexFieldType";
						arguments.Add("FieldTypeTypeName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_FieldTypeComplexValue();

								}
		}
				public partial class RecipeRunner_FieldTypeFlash : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_FieldTypeFlash(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "FieldTypeFlash", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_FieldTypeFlash()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"FlashFieldType";
						arguments.Add("FieldTypeTypeName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_FieldTypeFlash();

								}
		}
				public partial class RecipeRunner_FieldTypeISBN10 : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_FieldTypeISBN10(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "FieldTypeISBN10", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_FieldTypeISBN()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ISBNFieldType";
						arguments.Add("FieldTypeTypeName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_FieldTypeISBN();

								}
		}
				public partial class RecipeRunner_FieldTypeMultiColumn : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_FieldTypeMultiColumn(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "FieldTypeMultiColumn", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_FieldTypeMultiColumn()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"MultiColumnFieldType";
						arguments.Add("FieldTypeTypeName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_FieldTypeMultiColumn();

								}
		}
				public partial class RecipeRunner_FieldTypeRatings : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_FieldTypeRatings(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "FieldTypeRatings", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_FieldTypeRatings()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"RatingFieldType";
						arguments.Add("FieldTypeTypeName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_FieldTypeRatings();

								}
		}
				public partial class RecipeRunner_FieldTypeSSN : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_FieldTypeSSN(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "FieldTypeSSN", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_FieldTypeSSN()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"SSNFieldType";
						arguments.Add("FieldTypeTypeName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_FieldTypeSSN();

								}
		}
				public partial class RecipeRunner_ApplicationResourceFile : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_ApplicationResourceFile(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "ApplicationResourceFile", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_Default()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"F:\SPALM\SPSF\Dev\Source\TestData\TestFiles\ApplicationResourceFile.txt";
						arguments.Add("SourceFiles", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_Default();

								}
		}
				public partial class RecipeRunner_ClassResourceFile : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_ClassResourceFile(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "ClassResourceFile", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_Default()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"F:\SPALM\SPSF\Dev\Source\TestData\TestFiles\ClassResourceFile.txt";
						arguments.Add("SourceFiles", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_Default();

								}
		}
				public partial class RecipeRunner_GlobalResxFile : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_GlobalResxFile(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "GlobalResxFile", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_Default()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"F:\SPALM\SPSF\Dev\Source\TestData\TestFiles\Module.txt";
						arguments.Add("SourceFiles", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_Default();

								}
		}
				public partial class RecipeRunner_ImageFile : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_ImageFile(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "ImageFile", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_Default()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"F:\SPALM\SPSF\Dev\Source\TestData\TestFiles\Module.txt";
						arguments.Add("SourceFiles", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_Default();

								}
		}
				public partial class RecipeRunner_LayoutsFile : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_LayoutsFile(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "LayoutsFile", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_Default()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"F:\SPALM\SPSF\Dev\Source\TestData\TestFiles\Module.txt";
						arguments.Add("SourceFiles", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_Default();

								}
		}
				public partial class RecipeRunner_ContentType : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_ContentType(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "ContentType", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_DefaultContentType()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ContentTypeDefault";
						arguments.Add("ContentTypeName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_ContentTypeWithReceiver()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ContentTypeWithReceiver";
						arguments.Add("ContentTypeName", argObject);					
					
						argObject = @"True";
						arguments.Add("ContentTypeEventItemAdding", argObject);					
					
						argObject = @"True";
						arguments.Add("ContentTypeEventItemAdded", argObject);					
					
						argObject = @"True";
						arguments.Add("ContentTypeEventItemDeleting", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_ContentTypeWithCustomForm()
				{
				    						if(CurrentTestIsSandboxed())
						{
							this.parentTest.TestContext.WriteLine("Testcase ContentTypeWithCustomForm skipped because it is NotSandboxSupported");
							return;
						}
											Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ContentTypeWithCustomForm";
						arguments.Add("ContentTypeName", argObject);					
					
						argObject = @"True";
						arguments.Add("ContentTypeCustomFormsDisplay", argObject);					
					
						argObject = @"True";
						arguments.Add("ContentTypeCustomFormsNew", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultContentType();

										RunTestCase_ContentTypeWithReceiver();

										RunTestCase_ContentTypeWithCustomForm();

								}
		}
				public partial class RecipeRunner_ContentTypeBinding : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_ContentTypeBinding(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "ContentTypeBinding", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_ContentTypeBindingAnnouncements()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"Lists/Announcements";
						arguments.Add("ContentTypeBindingListUrl", argObject);					
					
						argObject = new NameValueItem("Doc","0x0101","0x0101");
						arguments.Add("ContentTypeBindingContentType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_ContentTypeBindingAnnouncements();

								}
		}
				public partial class RecipeRunner_ContentTypeCustomForm : BaseRecipeRunner		{		
			public RecipeRunner_ContentTypeCustomForm(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "ContentTypeCustomForm", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_ContentTypeDocumentTemplate : BaseRecipeRunner		{		
			public RecipeRunner_ContentTypeDocumentTemplate(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "ContentTypeDocumentTemplate", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_ContentTypeEventReceiver : BaseRecipeRunner		{		
			public RecipeRunner_ContentTypeEventReceiver(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "ContentTypeEventReceiver", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_ContentTypeFieldRef : BaseRecipeRunner		{		
			public RecipeRunner_ContentTypeFieldRef(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "ContentTypeFieldRef", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_LinqToSharePoint : LinqToSharePointTest		{		
			public RecipeRunner_LinqToSharePoint(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "LinqToSharePoint", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_ListDefinition : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_ListDefinition(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "ListDefinition", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_ListInstance : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_ListInstance(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "ListInstance", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_DefaultListInstance()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ListInstanceTest";
						arguments.Add("ListInstanceTitle", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultListInstance();

								}
		}
				public partial class RecipeRunner_Module : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_Module(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "Module", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_DefaultModule()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ModuleDefault";
						arguments.Add("ModuleName", argObject);					
					
						argObject = @"F:\SPALM\SPSF\Dev\Source\TestData\TestFiles\Module.txt; F:\SPALM\SPSF\Dev\Source\TestData\TestFiles\ClassResourceFile.txt";
						arguments.Add("FilesToAdd", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultModule();

								}
		}
				public partial class RecipeRunner_SiteColumn : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_SiteColumn(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "SiteColumn", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_PowerShellCmdLet : BaseRecipeRunner		{		
			public RecipeRunner_PowerShellCmdLet(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "PowerShellCmdLet", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_PowerShellPSCmdLet : BaseRecipeRunner		{		
			public RecipeRunner_PowerShellPSCmdLet(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "PowerShellPSCmdLet", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_PowerShellScript : BaseRecipeRunner		{		
			public RecipeRunner_PowerShellScript(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "PowerShellScript", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_SPCmdLet : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_SPCmdLet(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "SPCmdLet", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_DefaultCommand()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"Start-MyCustomJob";
						arguments.Add("SPCmdName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultCommand();

								}
		}
				public partial class RecipeRunner_SPCmdLetWithPipeBind : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_SPCmdLetWithPipeBind(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "SPCmdLetWithPipeBind", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_DefaultCommand()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"Get-SPSiteByName";
						arguments.Add("SPCmdName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_GetSPListByName()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"Get-MyCustomList";
						arguments.Add("SPCmdName", argObject);					
					
						argObject = @"SPList";
						arguments.Add("SPCmdLetPipeObjectType", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultCommand();

										RunTestCase_GetSPListByName();

								}
		}
				public partial class RecipeRunner_MasterPage : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_MasterPage(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "MasterPage", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_DefaultMasterPage()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"DefaultMasterPage";
						arguments.Add("MasterPageName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultMasterPage();

								}
		}
				public partial class RecipeRunner_PageLayout : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_PageLayout(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "PageLayout", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_DefaultPageLayout()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"DefaultPageLayout";
						arguments.Add("PageLayoutName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultPageLayout();

								}
		}
				public partial class RecipeRunner_PubSiteDef : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_PubSiteDef(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "PubSiteDef", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_PublishingSiteDefinitionDefault()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"PUBSITE1";
						arguments.Add("PubSiteDefName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_PublishingSiteDefinitionWithProvisioningXML()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"PUBSITE2";
						arguments.Add("PubSiteDefName", argObject);					
					
						argObject = @"True";
						arguments.Add("PubSiteDefCreateProvisioningXML", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_PublishingSiteDefinitionWithProvisioningHandler()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"PUBSITE3";
						arguments.Add("PubSiteDefName", argObject);					
					
						argObject = @"True";
						arguments.Add("PubSiteDefCreateProvisioningXML", argObject);					
					
						argObject = @"True";
						arguments.Add("PubSiteDefCreateProvisioningHandler", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_PublishingSiteDefinitionDefault();

										RunTestCase_PublishingSiteDefinitionWithProvisioningXML();

										RunTestCase_PublishingSiteDefinitionWithProvisioningHandler();

								}
		}
				public partial class RecipeRunner_HideRibbonControl : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_HideRibbonControl(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "HideRibbonControl", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_DefaultRibbonControl()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"HideRibbonControl";
						arguments.Add("RibbonActionName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_Hide()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"HideLibraryCreateView";
						arguments.Add("RibbonActionName", argObject);					
					
						argObject = new NameValueItem("Ribbon.Library.CustomViews.CreateView","Ribbon.Library.CustomViews.CreateView","Ribbon.Library.CustomViews.CreateView");
						arguments.Add("RibbonCommandUIDefinitionLocation", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("In tab 'Library' button 'CreateView' is not visible in document library page http://demo2010a/Shared%20Documents/Forms/AllItems.aspx");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultRibbonControl();

										RunTestCase_Hide();

								}
		}
				public partial class RecipeRunner_RibbonControl : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_RibbonControl(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "RibbonControl", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_DefaultRibbonControl()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"DefaultRibbonControl";
						arguments.Add("RibbonActionName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_RibbonControlWithPermissions()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"RibbonControlWithPermissions";
						arguments.Add("RibbonActionName", argObject);					
					
						List<NameValueItem> tempArray = new List<NameValueItem>();
							tempArray.Add(new NameValueItem("ManagePermissions","ManagePermissions","ManagePermissions"));
										tempArray.Add(new NameValueItem("ViewLists","ViewLists","ViewLists"));
																	argObject = tempArray.ToArray();
							
						arguments.Add("CustomActionRights", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_RibbonControlWithButton()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"RibbonControlWithButton";
						arguments.Add("RibbonActionName", argObject);					
					
						argObject = @"Ribbon Button";
						arguments.Add("RibbonActionTitle", argObject);					
					
						argObject = @"Button";
						arguments.Add("RibbonControlType", argObject);					
					
						argObject = new NameValueItem("Ribbon.Library.Actions","Ribbon.Library.Actions","Ribbon.Library.Actions");
						arguments.Add("RibbonCommandUIDefinitionLocation", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("Document library page contains custom ribbon button in group actions http://demo2010a/Shared%20Documents/Forms/AllItems.aspx");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultRibbonControl();

										RunTestCase_RibbonControlWithPermissions();

										RunTestCase_RibbonControlWithButton();

								}
		}
				public partial class RecipeRunner_RibbonGroup : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_RibbonGroup(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "RibbonGroup", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_DefaultRibbonGroup()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"DefaultRibbonGroup";
						arguments.Add("RibbonActionName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_RibbonGroupWithPermissions()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"RibbonGroupWithPermissions";
						arguments.Add("RibbonActionName", argObject);					
					
						List<NameValueItem> tempArray = new List<NameValueItem>();
							tempArray.Add(new NameValueItem("ManagePermissions","ManagePermissions","ManagePermissions"));
										tempArray.Add(new NameValueItem("ViewLists","ViewLists","ViewLists"));
																	argObject = tempArray.ToArray();
							
						arguments.Add("CustomActionRights", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_RibbonGroupForList()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"RibbonGroupForDocLib";
						arguments.Add("RibbonActionName", argObject);					
					
						argObject = @"Ribbon Group";
						arguments.Add("RibbonActionTitle", argObject);					
					
						argObject = @"List";
						arguments.Add("CustomActionRegistrationType", argObject);					
					
						argObject = @"101";
						arguments.Add("CustomActionRegistrationId", argObject);					
					
						argObject = new NameValueItem("Ribbon.Library","Ribbon.Library","Ribbon.Library");
						arguments.Add("RibbonCommandUIDefinitionLocation", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("In tab 'Library' new Ribbon Group 'Ribbon Group' with 2 buttons is available in document library page http://demo2010a/Shared%20Documents/Forms/AllItems.aspx");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultRibbonGroup();

										RunTestCase_RibbonGroupWithPermissions();

										RunTestCase_RibbonGroupForList();

								}
		}
				public partial class RecipeRunner_RibbonSampleContentTypeButton : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_RibbonSampleContentTypeButton(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "RibbonSampleContentTypeButton", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_DefaultRibbonGroup()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"DefaultContentTypeRibbon";
						arguments.Add("RibbonActionName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_RibbonGroupForList()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"RibbonForContentType";
						arguments.Add("RibbonActionName", argObject);					
					
						argObject = @"ContentType Button";
						arguments.Add("RibbonActionTitle", argObject);					
					
						argObject = @"ContentType";
						arguments.Add("CustomActionRegistrationType", argObject);					
					
						argObject = @"0x0101";
						arguments.Add("CustomActionRegistrationId", argObject);					
					
						argObject = @"CommandUI.Ribbon";
						arguments.Add("CustomActionLocation", argObject);					
					
						argObject = new NameValueItem("Ribbon.DocLibListForm.Edit.Actions","Ribbon.DocLibListForm.Edit.Actions","Ribbon.DocLibListForm.Edit.Actions");
						arguments.Add("RibbonCommandUIDefinitionLocation", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("In dialog 'Edit Properties' for content type 'document' is in tab 'Action' a new Ribbon Button 'ContentType Button' http://demo2010a/Shared%20Documents/Forms/AllItems.aspx");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultRibbonGroup();

										RunTestCase_RibbonGroupForList();

								}
		}
				public partial class RecipeRunner_RibbonSampleItemFormButton : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_RibbonSampleItemFormButton(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "RibbonSampleItemFormButton", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_RibbonSampleListViewButton : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_RibbonSampleListViewButton(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "RibbonSampleListViewButton", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_RibbonSampleTab : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_RibbonSampleTab(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "RibbonSampleTab", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_DefaultRibbonTab()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"RibbonSample";
						arguments.Add("RibbonActionName", argObject);					
					
						argObject = @"Custom Tab";
						arguments.Add("RibbonActionTitle", argObject);					
					
						argObject = @"List";
						arguments.Add("CustomActionRegistrationType", argObject);					
					
						argObject = @"101";
						arguments.Add("CustomActionRegistrationId", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_SampleRibbonTab()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"RibbonTabWith3Buttons";
						arguments.Add("RibbonActionName", argObject);					
					
						argObject = @"Sample Tab";
						arguments.Add("RibbonActionTitle", argObject);					
					
						argObject = @"List";
						arguments.Add("CustomActionRegistrationType", argObject);					
					
						argObject = @"101";
						arguments.Add("CustomActionRegistrationId", argObject);					
					
						argObject = @"CommandUI.Ribbon.ListView";
						arguments.Add("CustomActionLocation", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("In document library 'Sample Tab' a custom tab 'DocLib Tab' is available http://demo2010a/Shared%20Documents/Forms/AllItems.aspx");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultRibbonTab();

										RunTestCase_SampleRibbonTab();

								}
		}
				public partial class RecipeRunner_RibbonTab : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_RibbonTab(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "RibbonTab", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_DefaultRibbonTab()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"RibbonTabDefault";
						arguments.Add("RibbonActionName", argObject);					
					
						argObject = @"Custom Tab 1";
						arguments.Add("RibbonActionTitle", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_RibbonTabWithPermissions()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"RibbonTabWithPermissions";
						arguments.Add("RibbonActionName", argObject);					
					
						argObject = @"Custom Tab With Permission";
						arguments.Add("RibbonActionTitle", argObject);					
					
						List<NameValueItem> tempArray = new List<NameValueItem>();
							tempArray.Add(new NameValueItem("ManagePermissions","ManagePermissions","ManagePermissions"));
										tempArray.Add(new NameValueItem("ViewLists","ViewLists","ViewLists"));
																	argObject = tempArray.ToArray();
							
						arguments.Add("CustomActionRights", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
								public void RunTestCase_RibbonTabForList()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"RibbonTabForList";
						arguments.Add("RibbonActionName", argObject);					
					
						argObject = @"DocLib Tab";
						arguments.Add("RibbonActionTitle", argObject);					
					
						argObject = @"List";
						arguments.Add("CustomActionRegistrationType", argObject);					
					
						argObject = @"101";
						arguments.Add("CustomActionRegistrationId", argObject);					
					
						argObject = @"CommandUI.Ribbon.ListView";
						arguments.Add("CustomActionLocation", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("In document library 'Shared Documents' a custom tab 'DocLib Tab' is available http://demo2010a/Shared%20Documents/Forms/AllItems.aspx");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultRibbonTab();

										RunTestCase_RibbonTabWithPermissions();

										RunTestCase_RibbonTabForList();

								}
		}
				public partial class RecipeRunner_AspNetHostingPermission : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_AspNetHostingPermission(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "AspNetHostingPermission", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_DnsPermission : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_DnsPermission(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "DnsPermission", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_EnvironmentPermission : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_EnvironmentPermission(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "EnvironmentPermission", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_FileIOPermission : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_FileIOPermission(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "FileIOPermission", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_MediumCAS : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_MediumCAS(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "MediumCAS", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_MinimalCAS : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_MinimalCAS(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "MinimalCAS", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_PrintingPermission : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_PrintingPermission(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "PrintingPermission", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_ReflectionPermission : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_ReflectionPermission(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "ReflectionPermission", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_RegistryPermission : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_RegistryPermission(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "RegistryPermission", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_SecurityPermission : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_SecurityPermission(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "SecurityPermission", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_SharePointPermission : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_SharePointPermission(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "SharePointPermission", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_SmtpPermission : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_SmtpPermission(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "SmtpPermission", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_SqlClientPermission : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_SqlClientPermission(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "SqlClientPermission", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_WebPartPermission : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_WebPartPermission(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "WebPartPermission", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_SilverlightApplication : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_SilverlightApplication(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "SilverlightApplication", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_EmptySilverlightApplication()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"EmptySilverlightApp";
						arguments.Add("SilverlightApplicationName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("A webpart 'EmptySilverlightApp' is available in site http://demo2010a/");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_EmptySilverlightApplication();

								}
		}
				public partial class RecipeRunner_SilverlightApplicationSampleDataBinding : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_SilverlightApplicationSampleDataBinding(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "SilverlightApplicationSampleDataBinding", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_DataBindingSilverlightApplication()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"SilverlightDataBind";
						arguments.Add("SilverlightApplicationName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("A webpart 'DataBindingSilverlightApp' is available in site http://demo2010a/");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DataBindingSilverlightApplication();

								}
		}
				public partial class RecipeRunner_SilverlightApplicationSampleListViewer : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_SilverlightApplicationSampleListViewer(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "SilverlightApplicationSampleListViewer", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_ListViewerSilverlightApplication()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"SilverlightListViewer";
						arguments.Add("SilverlightApplicationName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("A webpart 'ListViewerSilverlightApp' is available in site http://demo2010a/");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_ListViewerSilverlightApplication();

								}
		}
				public partial class RecipeRunner_SilverlightApplicationSampleTaskAdder : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_SilverlightApplicationSampleTaskAdder(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "SilverlightApplicationSampleTaskAdder", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_TaskAdderSilverlightApplication()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"SilverlightTaskAdder";
						arguments.Add("SilverlightApplicationName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("A webpart 'TaskAdderSilverlightApp' is available in site http://demo2010a/");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_TaskAdderSilverlightApplication();

								}
		}
				public partial class RecipeRunner_BlankSiteDefinition : BaseRecipeRunner		{		
			public RecipeRunner_BlankSiteDefinition(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "BlankSiteDefinition", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_PublishingSiteDefinitionDefault()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"BLANKSITE1";
						arguments.Add("SiteDefinitionName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_PublishingSiteDefinitionDefault();

								}
		}
				public partial class RecipeRunner_FeatureStapling : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_FeatureStapling(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "FeatureStapling", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_FeatureAssocation1()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"FeatureAssocation1";
						arguments.Add("FeatureStaplingName", argObject);					
					
						argObject = new NameValueItem("Asset Library","Asset Library","4BCCCD62-DCAF-46dc-A7D4-E38277EF33F4");
						arguments.Add("FeatureStaplingFeature", argObject);					
					
						List<NameValueItem> tempArray = new List<NameValueItem>();
							tempArray.Add(new NameValueItem("SPS#0","SPS#0","SPS#0"));
										tempArray.Add(new NameValueItem("SPS#1","SPS#1","SPS#1"));
																	argObject = tempArray.ToArray();
							
						arguments.Add("FeatureStaplingSiteTemplates", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_FeatureAssocation1();

								}
		}
				public partial class RecipeRunner_TeamSiteDefinition : BaseCustomizationRecipeRunner		{		
			public RecipeRunner_TeamSiteDefinition(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "TeamSiteDefinition", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_PublishingSiteDefinitionDefault()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"TEAMSITE1";
						arguments.Add("SiteDefinitionName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_PublishingSiteDefinitionDefault();

								}
		}
				public partial class RecipeRunner_AjaxWebPart : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_AjaxWebPart(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "AjaxWebPart", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_AjaxWebPart()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"AjaxWebPart";
						arguments.Add("WebPartName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("A webpart 'AjaxWebPart' is available in site http://demo2010a/");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_AjaxWebPart();

								}
		}
				public partial class RecipeRunner_ASPWebPart : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_ASPWebPart(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "ASPWebPart", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_ASPWebPart()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"ASPWebPart";
						arguments.Add("WebPartName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("A webpart 'ASPWebPart' is available in site http://demo2010a/");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_ASPWebPart();

								}
		}
				public partial class RecipeRunner_FilterConsumerWebPart : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_FilterConsumerWebPart(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "FilterConsumerWebPart", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_FilterProviderWebPart : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_FilterProviderWebPart(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "FilterProviderWebPart", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_SharePointWebPart : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_SharePointWebPart(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "SharePointWebPart", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_SharePointWebPart()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"SharePointWebPart";
						arguments.Add("WebPartName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("A webpart 'SharePointWebPart' is available in site http://demo2010a/");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_SharePointWebPart();

								}
		}
				public partial class RecipeRunner_SimpleFilterConsumerWebPart : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_SimpleFilterConsumerWebPart(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "SimpleFilterConsumerWebPart", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_SimpleFilterConsumerWebPart()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"SimpleFilterConsumerWebPart";
						arguments.Add("WebPartName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("A webpart 'SimpleFilterConsumerWebPart' is available in site http://demo2010a/");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_SimpleFilterConsumerWebPart();

								}
		}
				public partial class RecipeRunner_VisualWebPart : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_VisualWebPart(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "VisualWebPart", _parentTest)
			{
				NotSandboxSupported = true;
			}
		
							public void RunTestCase_VisualWebPart()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"VisualWebPart";
						arguments.Add("WebPartName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
											AddExpectedDeployResult("A webpart 'VisualWebPart' is available in site http://demo2010a/");
										}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_VisualWebPart();

								}
		}
				public partial class RecipeRunner_SimpleWorkflow : BaseSiteFeatureRecipeRunner		{		
			public RecipeRunner_SimpleWorkflow(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "SimpleWorkflow", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
							public void RunTestCase_DefaultWorkflow()
				{
				    					Dictionary<string, object> arguments = new Dictionary<string, object>();
					object argObject = null;
					argObject = @"WorkflowDefault";
						arguments.Add("WorkflowName", argObject);					
					
											RunRecipeWithSpecifiedParameters(arguments);

					//add expected results to parent test
									}
						
			public void RunAllTestCases()
			{
								//RunRecipeWithoutRequiredValues();

									RunTestCase_DefaultWorkflow();

								}
		}
				public partial class RecipeRunner_ClassLibraryProject : BaseRecipeRunner		{		
			public RecipeRunner_ClassLibraryProject(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "ClassLibraryProject", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_DeploymentProject : BaseRecipeRunner		{		
			public RecipeRunner_DeploymentProject(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "DeploymentProject", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_MSBuildProcessorRecipe : BaseRecipeRunner		{		
			public RecipeRunner_MSBuildProcessorRecipe(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "MSBuildProcessorRecipe", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_MSBuildTask : BaseRecipeRunner		{		
			public RecipeRunner_MSBuildTask(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "MSBuildTask", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
				public partial class RecipeRunner_PowerShellProject : BaseRecipeRunner		{		
			public RecipeRunner_PowerShellProject(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, BaseTest _parentTest) : 
				base(guidancePackage, "PowerShellProject", _parentTest)
			{
				NotSandboxSupported = false;
			}
		
					
			public void RunAllTestCases()
			{
									RunRecipeWithDefaultValues();
									//RunRecipeWithoutRequiredValues();

							}
		}
			
		[TestClass()]
		public class Recipe_AdministrationPage_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe AdministrationPage with test case DefaultAdministrationPage")]
			[HostType("VS IDE")]
			public void Recipe_AdministrationPage_RunTestCase_DefaultAdministrationPage()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_AdministrationPage(coreRecipeGuidancePackage, this).RunTestCase_DefaultAdministrationPage();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe AdministrationPage with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_AdministrationPage_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_AdministrationPage(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_ApplicationPage_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ApplicationPage with test case DefaultApplicationPage")]
			[HostType("VS IDE")]
			public void Recipe_ApplicationPage_RunTestCase_DefaultApplicationPage()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ApplicationPage(coreRecipeGuidancePackage, this).RunTestCase_DefaultApplicationPage();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ApplicationPage with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_ApplicationPage_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ApplicationPage(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_UnsecuredApplicationPage_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe UnsecuredApplicationPage with test case DefaultApplicationPage")]
			[HostType("VS IDE")]
			public void Recipe_UnsecuredApplicationPage_RunTestCase_DefaultApplicationPage()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_UnsecuredApplicationPage(coreRecipeGuidancePackage, this).RunTestCase_DefaultApplicationPage();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe UnsecuredApplicationPage with test case AnonymousApplicationPage")]
			[HostType("VS IDE")]
			public void Recipe_UnsecuredApplicationPage_RunTestCase_AnonymousApplicationPage()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_UnsecuredApplicationPage(coreRecipeGuidancePackage, this).RunTestCase_AnonymousApplicationPage();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe UnsecuredApplicationPage with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_UnsecuredApplicationPage_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_UnsecuredApplicationPage(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_AdministrationWebService_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe AdministrationWebService with test case DefaultAdministrationWebService")]
			[HostType("VS IDE")]
			public void Recipe_AdministrationWebService_RunTestCase_DefaultAdministrationWebService()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_AdministrationWebService(coreRecipeGuidancePackage, this).RunTestCase_DefaultAdministrationWebService();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe AdministrationWebService with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_AdministrationWebService_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_AdministrationWebService(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_DelegateControlASCX_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe DelegateControlASCX with test case DefaultDelegateControlASCX")]
			[HostType("VS IDE")]
			public void Recipe_DelegateControlASCX_RunTestCase_DefaultDelegateControlASCX()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_DelegateControlASCX(coreRecipeGuidancePackage, this).RunTestCase_DefaultDelegateControlASCX();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe DelegateControlASCX with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_DelegateControlASCX_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_DelegateControlASCX(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_DelegateControlCS_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe DelegateControlCS with test case DefaultDelegateControlCS")]
			[HostType("VS IDE")]
			public void Recipe_DelegateControlCS_RunTestCase_DefaultDelegateControlCS()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_DelegateControlCS(coreRecipeGuidancePackage, this).RunTestCase_DefaultDelegateControlCS();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe DelegateControlCS with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_DelegateControlCS_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_DelegateControlCS(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_HttpHandler_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe HttpHandler with test case DefaultHttpHandler")]
			[HostType("VS IDE")]
			public void Recipe_HttpHandler_RunTestCase_DefaultHttpHandler()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_HttpHandler(coreRecipeGuidancePackage, this).RunTestCase_DefaultHttpHandler();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe HttpHandler with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_HttpHandler_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_HttpHandler(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_HttpModule_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe HttpModule with test case DefaultHttpModule")]
			[HostType("VS IDE")]
			public void Recipe_HttpModule_RunTestCase_DefaultHttpModule()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_HttpModule(coreRecipeGuidancePackage, this).RunTestCase_DefaultHttpModule();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe HttpModule with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_HttpModule_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_HttpModule(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_WCFWebService_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe WCFWebService with test case DefaultWebService")]
			[HostType("VS IDE")]
			public void Recipe_WCFWebService_RunTestCase_DefaultWebService()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_WCFWebService(coreRecipeGuidancePackage, this).RunTestCase_DefaultWebService();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe WCFWebService with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_WCFWebService_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_WCFWebService(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_WebService_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe WebService with test case DefaultWebService")]
			[HostType("VS IDE")]
			public void Recipe_WebService_RunTestCase_DefaultWebService()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_WebService(coreRecipeGuidancePackage, this).RunTestCase_DefaultWebService();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe WebService with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_WebService_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_WebService(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_BCSDesignWithAssembly_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe BCSDesignWithAssembly with default values")]
			[HostType("VS IDE")]
			public void Recipe_BCSDesignWithAssembly_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_BCSDesignWithAssembly(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe BCSDesignWithAssembly without required values")]
			[HostType("VS IDE")]
			public void Recipe_BCSDesignWithAssembly_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_BCSDesignWithAssembly(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_BCSListInstance_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe BCSListInstance with default values")]
			[HostType("VS IDE")]
			public void Recipe_BCSListInstance_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_BCSListInstance(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe BCSListInstance without required values")]
			[HostType("VS IDE")]
			public void Recipe_BCSListInstance_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_BCSListInstance(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_BCSModelFromDatabaseWithClasses_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe BCSModelFromDatabaseWithClasses with default values")]
			[HostType("VS IDE")]
			public void Recipe_BCSModelFromDatabaseWithClasses_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_BCSModelFromDatabaseWithClasses(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe BCSModelFromDatabaseWithClasses without required values")]
			[HostType("VS IDE")]
			public void Recipe_BCSModelFromDatabaseWithClasses_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_BCSModelFromDatabaseWithClasses(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_BCSModelFromDatabaseWithLINQ_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe BCSModelFromDatabaseWithLINQ with default values")]
			[HostType("VS IDE")]
			public void Recipe_BCSModelFromDatabaseWithLINQ_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_BCSModelFromDatabaseWithLINQ(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe BCSModelFromDatabaseWithLINQ without required values")]
			[HostType("VS IDE")]
			public void Recipe_BCSModelFromDatabaseWithLINQ_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_BCSModelFromDatabaseWithLINQ(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_BCSModelFromDatabaseWithSQLQueries_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe BCSModelFromDatabaseWithSQLQueries with default values")]
			[HostType("VS IDE")]
			public void Recipe_BCSModelFromDatabaseWithSQLQueries_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_BCSModelFromDatabaseWithSQLQueries(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe BCSModelFromDatabaseWithSQLQueries without required values")]
			[HostType("VS IDE")]
			public void Recipe_BCSModelFromDatabaseWithSQLQueries_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_BCSModelFromDatabaseWithSQLQueries(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_BCSQuickDeploy_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe BCSQuickDeploy with default values")]
			[HostType("VS IDE")]
			public void Recipe_BCSQuickDeploy_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_BCSQuickDeploy(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe BCSQuickDeploy without required values")]
			[HostType("VS IDE")]
			public void Recipe_BCSQuickDeploy_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_BCSQuickDeploy(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_BCSQuickDeployIncAssembly_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe BCSQuickDeployIncAssembly with default values")]
			[HostType("VS IDE")]
			public void Recipe_BCSQuickDeployIncAssembly_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_BCSQuickDeployIncAssembly(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe BCSQuickDeployIncAssembly without required values")]
			[HostType("VS IDE")]
			public void Recipe_BCSQuickDeployIncAssembly_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_BCSQuickDeployIncAssembly(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_EmptyBCSModel_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe EmptyBCSModel with default values")]
			[HostType("VS IDE")]
			public void Recipe_EmptyBCSModel_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_EmptyBCSModel(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe EmptyBCSModel without required values")]
			[HostType("VS IDE")]
			public void Recipe_EmptyBCSModel_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_EmptyBCSModel(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_CustomActionContentType_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionContentType with test case CustomActionContentTypeWithUrlAction")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionContentType_RunTestCase_CustomActionContentTypeWithUrlAction()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionContentType(coreRecipeGuidancePackage, this).RunTestCase_CustomActionContentTypeWithUrlAction();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionContentType with test case CustomActionContentTypeWithControlClass")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionContentType_RunTestCase_CustomActionContentTypeWithControlClass()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionContentType(coreRecipeGuidancePackage, this).RunTestCase_CustomActionContentTypeWithControlClass();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionContentType with test case CustomActionContentTypeWithControlTemplate")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionContentType_RunTestCase_CustomActionContentTypeWithControlTemplate()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionContentType(coreRecipeGuidancePackage, this).RunTestCase_CustomActionContentTypeWithControlTemplate();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionContentType with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionContentType_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionContentType(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_CustomActionFileType_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionFileType with test case CustomActionFileTypeWithUrlAction")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionFileType_RunTestCase_CustomActionFileTypeWithUrlAction()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionFileType(coreRecipeGuidancePackage, this).RunTestCase_CustomActionFileTypeWithUrlAction();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionFileType with test case CustomActionFileTypeWithControlClass")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionFileType_RunTestCase_CustomActionFileTypeWithControlClass()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionFileType(coreRecipeGuidancePackage, this).RunTestCase_CustomActionFileTypeWithControlClass();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionFileType with test case CustomActionFileTypeWithControlTemplate")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionFileType_RunTestCase_CustomActionFileTypeWithControlTemplate()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionFileType(coreRecipeGuidancePackage, this).RunTestCase_CustomActionFileTypeWithControlTemplate();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionFileType with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionFileType_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionFileType(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_CustomActionGroup_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionGroup with test case CustomActionGroup")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionGroup_RunTestCase_CustomActionGroup()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionGroup(coreRecipeGuidancePackage, this).RunTestCase_CustomActionGroup();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionGroup with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionGroup_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionGroup(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_CustomActionList_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionList with test case CustomActionListWithUrlAction")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionList_RunTestCase_CustomActionListWithUrlAction()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionList(coreRecipeGuidancePackage, this).RunTestCase_CustomActionListWithUrlAction();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionList with test case CustomActionListWithControlClass")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionList_RunTestCase_CustomActionListWithControlClass()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionList(coreRecipeGuidancePackage, this).RunTestCase_CustomActionListWithControlClass();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionList with test case CustomActionListWithControlTemplate")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionList_RunTestCase_CustomActionListWithControlTemplate()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionList(coreRecipeGuidancePackage, this).RunTestCase_CustomActionListWithControlTemplate();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionList with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionList_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionList(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_CustomActionListToolbar_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionListToolbar with test case CustomActionListToolbarWithUrlAction")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionListToolbar_RunTestCase_CustomActionListToolbarWithUrlAction()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionListToolbar(coreRecipeGuidancePackage, this).RunTestCase_CustomActionListToolbarWithUrlAction();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionListToolbar with test case CustomActionListToolbarWithControlClass")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionListToolbar_RunTestCase_CustomActionListToolbarWithControlClass()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionListToolbar(coreRecipeGuidancePackage, this).RunTestCase_CustomActionListToolbarWithControlClass();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionListToolbar with test case CustomActionListToolbarWithControlTemplate")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionListToolbar_RunTestCase_CustomActionListToolbarWithControlTemplate()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionListToolbar(coreRecipeGuidancePackage, this).RunTestCase_CustomActionListToolbarWithControlTemplate();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionListToolbar with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionListToolbar_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionListToolbar(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_CustomActionProgId_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionProgId with test case CustomActionProgIdWithUrlAction")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionProgId_RunTestCase_CustomActionProgIdWithUrlAction()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionProgId(coreRecipeGuidancePackage, this).RunTestCase_CustomActionProgIdWithUrlAction();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionProgId with test case CustomActionProgIdWithControlClass")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionProgId_RunTestCase_CustomActionProgIdWithControlClass()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionProgId(coreRecipeGuidancePackage, this).RunTestCase_CustomActionProgIdWithControlClass();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionProgId with test case CustomActionProgIdWithControlTemplate")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionProgId_RunTestCase_CustomActionProgIdWithControlTemplate()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionProgId(coreRecipeGuidancePackage, this).RunTestCase_CustomActionProgIdWithControlTemplate();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionProgId with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionProgId_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionProgId(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_CustomActionSite_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionSite with test case CustomActionSiteWithUrlAction")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionSite_RunTestCase_CustomActionSiteWithUrlAction()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionSite(coreRecipeGuidancePackage, this).RunTestCase_CustomActionSiteWithUrlAction();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionSite with test case CustomActionSiteWithControlClass")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionSite_RunTestCase_CustomActionSiteWithControlClass()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionSite(coreRecipeGuidancePackage, this).RunTestCase_CustomActionSiteWithControlClass();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionSite with test case CustomActionSiteWithControlTemplate")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionSite_RunTestCase_CustomActionSiteWithControlTemplate()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionSite(coreRecipeGuidancePackage, this).RunTestCase_CustomActionSiteWithControlTemplate();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionSite with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionSite_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionSite(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_CustomActionSiteActionsMenu_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionSiteActionsMenu with test case CustomSiteActionWithUrlAction")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionSiteActionsMenu_RunTestCase_CustomSiteActionWithUrlAction()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionSiteActionsMenu(coreRecipeGuidancePackage, this).RunTestCase_CustomSiteActionWithUrlAction();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionSiteActionsMenu with test case CustomSiteActionWithControlClass")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionSiteActionsMenu_RunTestCase_CustomSiteActionWithControlClass()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionSiteActionsMenu(coreRecipeGuidancePackage, this).RunTestCase_CustomSiteActionWithControlClass();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionSiteActionsMenu with test case CustomSiteActionWithControlTemplate")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionSiteActionsMenu_RunTestCase_CustomSiteActionWithControlTemplate()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionSiteActionsMenu(coreRecipeGuidancePackage, this).RunTestCase_CustomSiteActionWithControlTemplate();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe CustomActionSiteActionsMenu with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_CustomActionSiteActionsMenu_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_CustomActionSiteActionsMenu(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_HideCustomAction_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe HideCustomAction with test case HideCustomActionWithUrlAction")]
			[HostType("VS IDE")]
			public void Recipe_HideCustomAction_RunTestCase_HideCustomActionWithUrlAction()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_HideCustomAction(coreRecipeGuidancePackage, this).RunTestCase_HideCustomActionWithUrlAction();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe HideCustomAction with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_HideCustomAction_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_HideCustomAction(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_EmailEventReceiver_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe EmailEventReceiver with test case Default")]
			[HostType("VS IDE")]
			public void Recipe_EmailEventReceiver_RunTestCase_Default()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_EmailEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_Default();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe EmailEventReceiver with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_EmailEventReceiver_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_EmailEventReceiver(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_ItemEventReceiver_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ItemEventReceiver with test case Default")]
			[HostType("VS IDE")]
			public void Recipe_ItemEventReceiver_RunTestCase_Default()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ItemEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_Default();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ItemEventReceiver with test case ItemEventWithScopeSite")]
			[HostType("VS IDE")]
			public void Recipe_ItemEventReceiver_RunTestCase_ItemEventWithScopeSite()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ItemEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_ItemEventWithScopeSite();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ItemEventReceiver with test case ItemEventWithRootWebOnly")]
			[HostType("VS IDE")]
			public void Recipe_ItemEventReceiver_RunTestCase_ItemEventWithRootWebOnly()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ItemEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_ItemEventWithRootWebOnly();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ItemEventReceiver with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_ItemEventReceiver_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ItemEventReceiver(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_ListEventReceiver_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ListEventReceiver with test case Default")]
			[HostType("VS IDE")]
			public void Recipe_ListEventReceiver_RunTestCase_Default()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ListEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_Default();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ListEventReceiver with test case ListEventWithScopeSite")]
			[HostType("VS IDE")]
			public void Recipe_ListEventReceiver_RunTestCase_ListEventWithScopeSite()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ListEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_ListEventWithScopeSite();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ListEventReceiver with test case ListEventWithRootWebOnly")]
			[HostType("VS IDE")]
			public void Recipe_ListEventReceiver_RunTestCase_ListEventWithRootWebOnly()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ListEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_ListEventWithRootWebOnly();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ListEventReceiver with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_ListEventReceiver_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ListEventReceiver(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_WebEventReceiver_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe WebEventReceiver with test case Default")]
			[HostType("VS IDE")]
			public void Recipe_WebEventReceiver_RunTestCase_Default()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_WebEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_Default();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe WebEventReceiver with test case WebEventWithScopeSite")]
			[HostType("VS IDE")]
			public void Recipe_WebEventReceiver_RunTestCase_WebEventWithScopeSite()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_WebEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_WebEventWithScopeSite();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe WebEventReceiver with test case WebEventWithRootWebOnly")]
			[HostType("VS IDE")]
			public void Recipe_WebEventReceiver_RunTestCase_WebEventWithRootWebOnly()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_WebEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_WebEventWithRootWebOnly();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe WebEventReceiver with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_WebEventReceiver_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_WebEventReceiver(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_EmptyFeature_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe EmptyFeature with test case SiteFeature")]
			[HostType("VS IDE")]
			public void Recipe_EmptyFeature_RunTestCase_SiteFeature()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_EmptyFeature(coreRecipeGuidancePackage, this).RunTestCase_SiteFeature();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe EmptyFeature with test case FeatureWithReceiver")]
			[HostType("VS IDE")]
			public void Recipe_EmptyFeature_RunTestCase_FeatureWithReceiver()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_EmptyFeature(coreRecipeGuidancePackage, this).RunTestCase_FeatureWithReceiver();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe EmptyFeature with test case WebFeature")]
			[HostType("VS IDE")]
			public void Recipe_EmptyFeature_RunTestCase_WebFeature()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_EmptyFeature(coreRecipeGuidancePackage, this).RunTestCase_WebFeature();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe EmptyFeature with test case FeatureWithDependencies")]
			[HostType("VS IDE")]
			public void Recipe_EmptyFeature_RunTestCase_FeatureWithDependencies()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_EmptyFeature(coreRecipeGuidancePackage, this).RunTestCase_FeatureWithDependencies();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe EmptyFeature with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_EmptyFeature_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_EmptyFeature(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_EmptyFeatureActivationDependency14_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe EmptyFeatureActivationDependency14 with default values")]
			[HostType("VS IDE")]
			public void Recipe_EmptyFeatureActivationDependency14_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_EmptyFeatureActivationDependency14(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe EmptyFeatureActivationDependency14 without required values")]
			[HostType("VS IDE")]
			public void Recipe_EmptyFeatureActivationDependency14_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_EmptyFeatureActivationDependency14(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_EmptyFeatureProperty_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe EmptyFeatureProperty with default values")]
			[HostType("VS IDE")]
			public void Recipe_EmptyFeatureProperty_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_EmptyFeatureProperty(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe EmptyFeatureProperty without required values")]
			[HostType("VS IDE")]
			public void Recipe_EmptyFeatureProperty_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_EmptyFeatureProperty(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_EmptyFeatureUpgradeActionContentType_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe EmptyFeatureUpgradeActionContentType with default values")]
			[HostType("VS IDE")]
			public void Recipe_EmptyFeatureUpgradeActionContentType_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_EmptyFeatureUpgradeActionContentType(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe EmptyFeatureUpgradeActionContentType without required values")]
			[HostType("VS IDE")]
			public void Recipe_EmptyFeatureUpgradeActionContentType_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_EmptyFeatureUpgradeActionContentType(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_FieldType_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FieldType with test case FieldTypeDefault")]
			[HostType("VS IDE")]
			public void Recipe_FieldType_RunTestCase_FieldTypeDefault()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FieldType(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeDefault();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FieldType with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_FieldType_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FieldType(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_FieldTypeChoice_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FieldTypeChoice with test case FieldTypeChoice")]
			[HostType("VS IDE")]
			public void Recipe_FieldTypeChoice_RunTestCase_FieldTypeChoice()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FieldTypeChoice(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeChoice();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FieldTypeChoice with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_FieldTypeChoice_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FieldTypeChoice(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_FieldTypeComplexValue_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FieldTypeComplexValue with test case FieldTypeComplexValue")]
			[HostType("VS IDE")]
			public void Recipe_FieldTypeComplexValue_RunTestCase_FieldTypeComplexValue()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FieldTypeComplexValue(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeComplexValue();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FieldTypeComplexValue with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_FieldTypeComplexValue_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FieldTypeComplexValue(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_FieldTypeFlash_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FieldTypeFlash with test case FieldTypeFlash")]
			[HostType("VS IDE")]
			public void Recipe_FieldTypeFlash_RunTestCase_FieldTypeFlash()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FieldTypeFlash(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeFlash();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FieldTypeFlash with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_FieldTypeFlash_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FieldTypeFlash(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_FieldTypeISBN10_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FieldTypeISBN10 with test case FieldTypeISBN")]
			[HostType("VS IDE")]
			public void Recipe_FieldTypeISBN10_RunTestCase_FieldTypeISBN()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FieldTypeISBN10(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeISBN();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FieldTypeISBN10 with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_FieldTypeISBN10_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FieldTypeISBN10(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_FieldTypeMultiColumn_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FieldTypeMultiColumn with test case FieldTypeMultiColumn")]
			[HostType("VS IDE")]
			public void Recipe_FieldTypeMultiColumn_RunTestCase_FieldTypeMultiColumn()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FieldTypeMultiColumn(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeMultiColumn();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FieldTypeMultiColumn with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_FieldTypeMultiColumn_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FieldTypeMultiColumn(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_FieldTypeRatings_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FieldTypeRatings with test case FieldTypeRatings")]
			[HostType("VS IDE")]
			public void Recipe_FieldTypeRatings_RunTestCase_FieldTypeRatings()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FieldTypeRatings(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeRatings();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FieldTypeRatings with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_FieldTypeRatings_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FieldTypeRatings(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_FieldTypeSSN_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FieldTypeSSN with test case FieldTypeSSN")]
			[HostType("VS IDE")]
			public void Recipe_FieldTypeSSN_RunTestCase_FieldTypeSSN()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FieldTypeSSN(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeSSN();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FieldTypeSSN with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_FieldTypeSSN_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FieldTypeSSN(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_ApplicationResourceFile_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ApplicationResourceFile with test case Default")]
			[HostType("VS IDE")]
			public void Recipe_ApplicationResourceFile_RunTestCase_Default()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ApplicationResourceFile(coreRecipeGuidancePackage, this).RunTestCase_Default();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ApplicationResourceFile with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_ApplicationResourceFile_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ApplicationResourceFile(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_ClassResourceFile_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ClassResourceFile with test case Default")]
			[HostType("VS IDE")]
			public void Recipe_ClassResourceFile_RunTestCase_Default()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ClassResourceFile(coreRecipeGuidancePackage, this).RunTestCase_Default();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ClassResourceFile with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_ClassResourceFile_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ClassResourceFile(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_GlobalResxFile_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe GlobalResxFile with test case Default")]
			[HostType("VS IDE")]
			public void Recipe_GlobalResxFile_RunTestCase_Default()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_GlobalResxFile(coreRecipeGuidancePackage, this).RunTestCase_Default();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe GlobalResxFile with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_GlobalResxFile_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_GlobalResxFile(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_ImageFile_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ImageFile with test case Default")]
			[HostType("VS IDE")]
			public void Recipe_ImageFile_RunTestCase_Default()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ImageFile(coreRecipeGuidancePackage, this).RunTestCase_Default();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ImageFile with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_ImageFile_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ImageFile(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_LayoutsFile_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe LayoutsFile with test case Default")]
			[HostType("VS IDE")]
			public void Recipe_LayoutsFile_RunTestCase_Default()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_LayoutsFile(coreRecipeGuidancePackage, this).RunTestCase_Default();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe LayoutsFile with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_LayoutsFile_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_LayoutsFile(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_ContentType_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ContentType with test case DefaultContentType")]
			[HostType("VS IDE")]
			public void Recipe_ContentType_RunTestCase_DefaultContentType()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ContentType(coreRecipeGuidancePackage, this).RunTestCase_DefaultContentType();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ContentType with test case ContentTypeWithReceiver")]
			[HostType("VS IDE")]
			public void Recipe_ContentType_RunTestCase_ContentTypeWithReceiver()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ContentType(coreRecipeGuidancePackage, this).RunTestCase_ContentTypeWithReceiver();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ContentType with test case ContentTypeWithCustomForm")]
			[HostType("VS IDE")]
			public void Recipe_ContentType_RunTestCase_ContentTypeWithCustomForm()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ContentType(coreRecipeGuidancePackage, this).RunTestCase_ContentTypeWithCustomForm();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ContentType with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_ContentType_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ContentType(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_ContentTypeBinding_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ContentTypeBinding with test case ContentTypeBindingAnnouncements")]
			[HostType("VS IDE")]
			public void Recipe_ContentTypeBinding_RunTestCase_ContentTypeBindingAnnouncements()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ContentTypeBinding(coreRecipeGuidancePackage, this).RunTestCase_ContentTypeBindingAnnouncements();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ContentTypeBinding with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_ContentTypeBinding_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ContentTypeBinding(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_ContentTypeCustomForm_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ContentTypeCustomForm with default values")]
			[HostType("VS IDE")]
			public void Recipe_ContentTypeCustomForm_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ContentTypeCustomForm(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ContentTypeCustomForm without required values")]
			[HostType("VS IDE")]
			public void Recipe_ContentTypeCustomForm_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ContentTypeCustomForm(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_ContentTypeDocumentTemplate_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ContentTypeDocumentTemplate with default values")]
			[HostType("VS IDE")]
			public void Recipe_ContentTypeDocumentTemplate_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ContentTypeDocumentTemplate(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ContentTypeDocumentTemplate without required values")]
			[HostType("VS IDE")]
			public void Recipe_ContentTypeDocumentTemplate_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ContentTypeDocumentTemplate(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_ContentTypeEventReceiver_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ContentTypeEventReceiver with default values")]
			[HostType("VS IDE")]
			public void Recipe_ContentTypeEventReceiver_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ContentTypeEventReceiver(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ContentTypeEventReceiver without required values")]
			[HostType("VS IDE")]
			public void Recipe_ContentTypeEventReceiver_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ContentTypeEventReceiver(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_ContentTypeFieldRef_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ContentTypeFieldRef with default values")]
			[HostType("VS IDE")]
			public void Recipe_ContentTypeFieldRef_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ContentTypeFieldRef(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ContentTypeFieldRef without required values")]
			[HostType("VS IDE")]
			public void Recipe_ContentTypeFieldRef_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ContentTypeFieldRef(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_LinqToSharePoint_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe LinqToSharePoint with default values")]
			[HostType("VS IDE")]
			public void Recipe_LinqToSharePoint_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_LinqToSharePoint(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe LinqToSharePoint without required values")]
			[HostType("VS IDE")]
			public void Recipe_LinqToSharePoint_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_LinqToSharePoint(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_ListDefinition_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ListDefinition with default values")]
			[HostType("VS IDE")]
			public void Recipe_ListDefinition_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ListDefinition(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ListDefinition without required values")]
			[HostType("VS IDE")]
			public void Recipe_ListDefinition_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ListDefinition(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_ListInstance_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ListInstance with test case DefaultListInstance")]
			[HostType("VS IDE")]
			public void Recipe_ListInstance_RunTestCase_DefaultListInstance()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ListInstance(coreRecipeGuidancePackage, this).RunTestCase_DefaultListInstance();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ListInstance with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_ListInstance_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ListInstance(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_Module_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe Module with test case DefaultModule")]
			[HostType("VS IDE")]
			public void Recipe_Module_RunTestCase_DefaultModule()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_Module(coreRecipeGuidancePackage, this).RunTestCase_DefaultModule();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe Module with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_Module_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_Module(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_SiteColumn_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SiteColumn with default values")]
			[HostType("VS IDE")]
			public void Recipe_SiteColumn_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SiteColumn(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SiteColumn without required values")]
			[HostType("VS IDE")]
			public void Recipe_SiteColumn_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SiteColumn(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_PowerShellCmdLet_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe PowerShellCmdLet with default values")]
			[HostType("VS IDE")]
			public void Recipe_PowerShellCmdLet_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_PowerShellCmdLet(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe PowerShellCmdLet without required values")]
			[HostType("VS IDE")]
			public void Recipe_PowerShellCmdLet_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_PowerShellCmdLet(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_PowerShellPSCmdLet_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe PowerShellPSCmdLet with default values")]
			[HostType("VS IDE")]
			public void Recipe_PowerShellPSCmdLet_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_PowerShellPSCmdLet(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe PowerShellPSCmdLet without required values")]
			[HostType("VS IDE")]
			public void Recipe_PowerShellPSCmdLet_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_PowerShellPSCmdLet(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_PowerShellScript_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe PowerShellScript with default values")]
			[HostType("VS IDE")]
			public void Recipe_PowerShellScript_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_PowerShellScript(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe PowerShellScript without required values")]
			[HostType("VS IDE")]
			public void Recipe_PowerShellScript_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_PowerShellScript(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_SPCmdLet_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SPCmdLet with test case DefaultCommand")]
			[HostType("VS IDE")]
			public void Recipe_SPCmdLet_RunTestCase_DefaultCommand()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SPCmdLet(coreRecipeGuidancePackage, this).RunTestCase_DefaultCommand();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SPCmdLet with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_SPCmdLet_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SPCmdLet(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_SPCmdLetWithPipeBind_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SPCmdLetWithPipeBind with test case DefaultCommand")]
			[HostType("VS IDE")]
			public void Recipe_SPCmdLetWithPipeBind_RunTestCase_DefaultCommand()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SPCmdLetWithPipeBind(coreRecipeGuidancePackage, this).RunTestCase_DefaultCommand();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SPCmdLetWithPipeBind with test case GetSPListByName")]
			[HostType("VS IDE")]
			public void Recipe_SPCmdLetWithPipeBind_RunTestCase_GetSPListByName()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SPCmdLetWithPipeBind(coreRecipeGuidancePackage, this).RunTestCase_GetSPListByName();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SPCmdLetWithPipeBind with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_SPCmdLetWithPipeBind_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SPCmdLetWithPipeBind(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_MasterPage_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe MasterPage with test case DefaultMasterPage")]
			[HostType("VS IDE")]
			public void Recipe_MasterPage_RunTestCase_DefaultMasterPage()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_MasterPage(coreRecipeGuidancePackage, this).RunTestCase_DefaultMasterPage();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe MasterPage with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_MasterPage_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_MasterPage(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_PageLayout_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe PageLayout with test case DefaultPageLayout")]
			[HostType("VS IDE")]
			public void Recipe_PageLayout_RunTestCase_DefaultPageLayout()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_PageLayout(coreRecipeGuidancePackage, this).RunTestCase_DefaultPageLayout();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe PageLayout with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_PageLayout_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_PageLayout(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_PubSiteDef_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe PubSiteDef with test case PublishingSiteDefinitionDefault")]
			[HostType("VS IDE")]
			public void Recipe_PubSiteDef_RunTestCase_PublishingSiteDefinitionDefault()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_PubSiteDef(coreRecipeGuidancePackage, this).RunTestCase_PublishingSiteDefinitionDefault();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe PubSiteDef with test case PublishingSiteDefinitionWithProvisioningXML")]
			[HostType("VS IDE")]
			public void Recipe_PubSiteDef_RunTestCase_PublishingSiteDefinitionWithProvisioningXML()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_PubSiteDef(coreRecipeGuidancePackage, this).RunTestCase_PublishingSiteDefinitionWithProvisioningXML();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe PubSiteDef with test case PublishingSiteDefinitionWithProvisioningHandler")]
			[HostType("VS IDE")]
			public void Recipe_PubSiteDef_RunTestCase_PublishingSiteDefinitionWithProvisioningHandler()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_PubSiteDef(coreRecipeGuidancePackage, this).RunTestCase_PublishingSiteDefinitionWithProvisioningHandler();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe PubSiteDef with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_PubSiteDef_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_PubSiteDef(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_HideRibbonControl_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe HideRibbonControl with test case DefaultRibbonControl")]
			[HostType("VS IDE")]
			public void Recipe_HideRibbonControl_RunTestCase_DefaultRibbonControl()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_HideRibbonControl(coreRecipeGuidancePackage, this).RunTestCase_DefaultRibbonControl();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe HideRibbonControl with test case Hide")]
			[HostType("VS IDE")]
			public void Recipe_HideRibbonControl_RunTestCase_Hide()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_HideRibbonControl(coreRecipeGuidancePackage, this).RunTestCase_Hide();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe HideRibbonControl with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_HideRibbonControl_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_HideRibbonControl(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_RibbonControl_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonControl with test case DefaultRibbonControl")]
			[HostType("VS IDE")]
			public void Recipe_RibbonControl_RunTestCase_DefaultRibbonControl()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonControl(coreRecipeGuidancePackage, this).RunTestCase_DefaultRibbonControl();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonControl with test case RibbonControlWithPermissions")]
			[HostType("VS IDE")]
			public void Recipe_RibbonControl_RunTestCase_RibbonControlWithPermissions()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonControl(coreRecipeGuidancePackage, this).RunTestCase_RibbonControlWithPermissions();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonControl with test case RibbonControlWithButton")]
			[HostType("VS IDE")]
			public void Recipe_RibbonControl_RunTestCase_RibbonControlWithButton()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonControl(coreRecipeGuidancePackage, this).RunTestCase_RibbonControlWithButton();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonControl with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_RibbonControl_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonControl(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_RibbonGroup_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonGroup with test case DefaultRibbonGroup")]
			[HostType("VS IDE")]
			public void Recipe_RibbonGroup_RunTestCase_DefaultRibbonGroup()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonGroup(coreRecipeGuidancePackage, this).RunTestCase_DefaultRibbonGroup();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonGroup with test case RibbonGroupWithPermissions")]
			[HostType("VS IDE")]
			public void Recipe_RibbonGroup_RunTestCase_RibbonGroupWithPermissions()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonGroup(coreRecipeGuidancePackage, this).RunTestCase_RibbonGroupWithPermissions();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonGroup with test case RibbonGroupForList")]
			[HostType("VS IDE")]
			public void Recipe_RibbonGroup_RunTestCase_RibbonGroupForList()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonGroup(coreRecipeGuidancePackage, this).RunTestCase_RibbonGroupForList();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonGroup with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_RibbonGroup_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonGroup(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_RibbonSampleContentTypeButton_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonSampleContentTypeButton with test case DefaultRibbonGroup")]
			[HostType("VS IDE")]
			public void Recipe_RibbonSampleContentTypeButton_RunTestCase_DefaultRibbonGroup()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonSampleContentTypeButton(coreRecipeGuidancePackage, this).RunTestCase_DefaultRibbonGroup();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonSampleContentTypeButton with test case RibbonGroupForList")]
			[HostType("VS IDE")]
			public void Recipe_RibbonSampleContentTypeButton_RunTestCase_RibbonGroupForList()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonSampleContentTypeButton(coreRecipeGuidancePackage, this).RunTestCase_RibbonGroupForList();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonSampleContentTypeButton with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_RibbonSampleContentTypeButton_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonSampleContentTypeButton(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_RibbonSampleItemFormButton_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonSampleItemFormButton with default values")]
			[HostType("VS IDE")]
			public void Recipe_RibbonSampleItemFormButton_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonSampleItemFormButton(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonSampleItemFormButton without required values")]
			[HostType("VS IDE")]
			public void Recipe_RibbonSampleItemFormButton_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonSampleItemFormButton(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_RibbonSampleListViewButton_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonSampleListViewButton with default values")]
			[HostType("VS IDE")]
			public void Recipe_RibbonSampleListViewButton_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonSampleListViewButton(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonSampleListViewButton without required values")]
			[HostType("VS IDE")]
			public void Recipe_RibbonSampleListViewButton_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonSampleListViewButton(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_RibbonSampleTab_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonSampleTab with test case DefaultRibbonTab")]
			[HostType("VS IDE")]
			public void Recipe_RibbonSampleTab_RunTestCase_DefaultRibbonTab()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonSampleTab(coreRecipeGuidancePackage, this).RunTestCase_DefaultRibbonTab();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonSampleTab with test case SampleRibbonTab")]
			[HostType("VS IDE")]
			public void Recipe_RibbonSampleTab_RunTestCase_SampleRibbonTab()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonSampleTab(coreRecipeGuidancePackage, this).RunTestCase_SampleRibbonTab();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonSampleTab with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_RibbonSampleTab_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonSampleTab(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_RibbonTab_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonTab with test case DefaultRibbonTab")]
			[HostType("VS IDE")]
			public void Recipe_RibbonTab_RunTestCase_DefaultRibbonTab()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonTab(coreRecipeGuidancePackage, this).RunTestCase_DefaultRibbonTab();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonTab with test case RibbonTabWithPermissions")]
			[HostType("VS IDE")]
			public void Recipe_RibbonTab_RunTestCase_RibbonTabWithPermissions()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonTab(coreRecipeGuidancePackage, this).RunTestCase_RibbonTabWithPermissions();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonTab with test case RibbonTabForList")]
			[HostType("VS IDE")]
			public void Recipe_RibbonTab_RunTestCase_RibbonTabForList()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonTab(coreRecipeGuidancePackage, this).RunTestCase_RibbonTabForList();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RibbonTab with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_RibbonTab_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RibbonTab(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_AspNetHostingPermission_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe AspNetHostingPermission with default values")]
			[HostType("VS IDE")]
			public void Recipe_AspNetHostingPermission_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_AspNetHostingPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe AspNetHostingPermission without required values")]
			[HostType("VS IDE")]
			public void Recipe_AspNetHostingPermission_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_AspNetHostingPermission(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_DnsPermission_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe DnsPermission with default values")]
			[HostType("VS IDE")]
			public void Recipe_DnsPermission_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_DnsPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe DnsPermission without required values")]
			[HostType("VS IDE")]
			public void Recipe_DnsPermission_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_DnsPermission(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_EnvironmentPermission_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe EnvironmentPermission with default values")]
			[HostType("VS IDE")]
			public void Recipe_EnvironmentPermission_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_EnvironmentPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe EnvironmentPermission without required values")]
			[HostType("VS IDE")]
			public void Recipe_EnvironmentPermission_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_EnvironmentPermission(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_FileIOPermission_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FileIOPermission with default values")]
			[HostType("VS IDE")]
			public void Recipe_FileIOPermission_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FileIOPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FileIOPermission without required values")]
			[HostType("VS IDE")]
			public void Recipe_FileIOPermission_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FileIOPermission(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_MediumCAS_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe MediumCAS with default values")]
			[HostType("VS IDE")]
			public void Recipe_MediumCAS_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_MediumCAS(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe MediumCAS without required values")]
			[HostType("VS IDE")]
			public void Recipe_MediumCAS_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_MediumCAS(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_MinimalCAS_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe MinimalCAS with default values")]
			[HostType("VS IDE")]
			public void Recipe_MinimalCAS_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_MinimalCAS(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe MinimalCAS without required values")]
			[HostType("VS IDE")]
			public void Recipe_MinimalCAS_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_MinimalCAS(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_PrintingPermission_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe PrintingPermission with default values")]
			[HostType("VS IDE")]
			public void Recipe_PrintingPermission_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_PrintingPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe PrintingPermission without required values")]
			[HostType("VS IDE")]
			public void Recipe_PrintingPermission_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_PrintingPermission(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_ReflectionPermission_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ReflectionPermission with default values")]
			[HostType("VS IDE")]
			public void Recipe_ReflectionPermission_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ReflectionPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ReflectionPermission without required values")]
			[HostType("VS IDE")]
			public void Recipe_ReflectionPermission_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ReflectionPermission(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_RegistryPermission_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RegistryPermission with default values")]
			[HostType("VS IDE")]
			public void Recipe_RegistryPermission_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RegistryPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe RegistryPermission without required values")]
			[HostType("VS IDE")]
			public void Recipe_RegistryPermission_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_RegistryPermission(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_SecurityPermission_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SecurityPermission with default values")]
			[HostType("VS IDE")]
			public void Recipe_SecurityPermission_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SecurityPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SecurityPermission without required values")]
			[HostType("VS IDE")]
			public void Recipe_SecurityPermission_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SecurityPermission(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_SharePointPermission_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SharePointPermission with default values")]
			[HostType("VS IDE")]
			public void Recipe_SharePointPermission_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SharePointPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SharePointPermission without required values")]
			[HostType("VS IDE")]
			public void Recipe_SharePointPermission_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SharePointPermission(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_SmtpPermission_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SmtpPermission with default values")]
			[HostType("VS IDE")]
			public void Recipe_SmtpPermission_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SmtpPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SmtpPermission without required values")]
			[HostType("VS IDE")]
			public void Recipe_SmtpPermission_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SmtpPermission(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_SqlClientPermission_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SqlClientPermission with default values")]
			[HostType("VS IDE")]
			public void Recipe_SqlClientPermission_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SqlClientPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SqlClientPermission without required values")]
			[HostType("VS IDE")]
			public void Recipe_SqlClientPermission_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SqlClientPermission(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_WebPartPermission_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe WebPartPermission with default values")]
			[HostType("VS IDE")]
			public void Recipe_WebPartPermission_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_WebPartPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe WebPartPermission without required values")]
			[HostType("VS IDE")]
			public void Recipe_WebPartPermission_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_WebPartPermission(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_SilverlightApplication_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SilverlightApplication with test case EmptySilverlightApplication")]
			[HostType("VS IDE")]
			public void Recipe_SilverlightApplication_RunTestCase_EmptySilverlightApplication()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SilverlightApplication(coreRecipeGuidancePackage, this).RunTestCase_EmptySilverlightApplication();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SilverlightApplication with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_SilverlightApplication_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SilverlightApplication(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_SilverlightApplicationSampleDataBinding_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SilverlightApplicationSampleDataBinding with test case DataBindingSilverlightApplication")]
			[HostType("VS IDE")]
			public void Recipe_SilverlightApplicationSampleDataBinding_RunTestCase_DataBindingSilverlightApplication()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SilverlightApplicationSampleDataBinding(coreRecipeGuidancePackage, this).RunTestCase_DataBindingSilverlightApplication();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SilverlightApplicationSampleDataBinding with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_SilverlightApplicationSampleDataBinding_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SilverlightApplicationSampleDataBinding(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_SilverlightApplicationSampleListViewer_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SilverlightApplicationSampleListViewer with test case ListViewerSilverlightApplication")]
			[HostType("VS IDE")]
			public void Recipe_SilverlightApplicationSampleListViewer_RunTestCase_ListViewerSilverlightApplication()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SilverlightApplicationSampleListViewer(coreRecipeGuidancePackage, this).RunTestCase_ListViewerSilverlightApplication();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SilverlightApplicationSampleListViewer with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_SilverlightApplicationSampleListViewer_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SilverlightApplicationSampleListViewer(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_SilverlightApplicationSampleTaskAdder_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SilverlightApplicationSampleTaskAdder with test case TaskAdderSilverlightApplication")]
			[HostType("VS IDE")]
			public void Recipe_SilverlightApplicationSampleTaskAdder_RunTestCase_TaskAdderSilverlightApplication()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SilverlightApplicationSampleTaskAdder(coreRecipeGuidancePackage, this).RunTestCase_TaskAdderSilverlightApplication();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SilverlightApplicationSampleTaskAdder with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_SilverlightApplicationSampleTaskAdder_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SilverlightApplicationSampleTaskAdder(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_BlankSiteDefinition_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe BlankSiteDefinition with test case PublishingSiteDefinitionDefault")]
			[HostType("VS IDE")]
			public void Recipe_BlankSiteDefinition_RunTestCase_PublishingSiteDefinitionDefault()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_BlankSiteDefinition(coreRecipeGuidancePackage, this).RunTestCase_PublishingSiteDefinitionDefault();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe BlankSiteDefinition with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_BlankSiteDefinition_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_BlankSiteDefinition(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_FeatureStapling_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FeatureStapling with test case FeatureAssocation1")]
			[HostType("VS IDE")]
			public void Recipe_FeatureStapling_RunTestCase_FeatureAssocation1()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FeatureStapling(coreRecipeGuidancePackage, this).RunTestCase_FeatureAssocation1();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FeatureStapling with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_FeatureStapling_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FeatureStapling(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_TeamSiteDefinition_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe TeamSiteDefinition with test case PublishingSiteDefinitionDefault")]
			[HostType("VS IDE")]
			public void Recipe_TeamSiteDefinition_RunTestCase_PublishingSiteDefinitionDefault()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_TeamSiteDefinition(coreRecipeGuidancePackage, this).RunTestCase_PublishingSiteDefinitionDefault();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe TeamSiteDefinition with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_TeamSiteDefinition_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_TeamSiteDefinition(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_AjaxWebPart_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe AjaxWebPart with test case AjaxWebPart")]
			[HostType("VS IDE")]
			public void Recipe_AjaxWebPart_RunTestCase_AjaxWebPart()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_AjaxWebPart(coreRecipeGuidancePackage, this).RunTestCase_AjaxWebPart();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe AjaxWebPart with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_AjaxWebPart_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_AjaxWebPart(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_ASPWebPart_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ASPWebPart with test case ASPWebPart")]
			[HostType("VS IDE")]
			public void Recipe_ASPWebPart_RunTestCase_ASPWebPart()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ASPWebPart(coreRecipeGuidancePackage, this).RunTestCase_ASPWebPart();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ASPWebPart with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_ASPWebPart_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ASPWebPart(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_FilterConsumerWebPart_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FilterConsumerWebPart with default values")]
			[HostType("VS IDE")]
			public void Recipe_FilterConsumerWebPart_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FilterConsumerWebPart(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FilterConsumerWebPart without required values")]
			[HostType("VS IDE")]
			public void Recipe_FilterConsumerWebPart_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FilterConsumerWebPart(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_FilterProviderWebPart_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FilterProviderWebPart with default values")]
			[HostType("VS IDE")]
			public void Recipe_FilterProviderWebPart_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FilterProviderWebPart(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe FilterProviderWebPart without required values")]
			[HostType("VS IDE")]
			public void Recipe_FilterProviderWebPart_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_FilterProviderWebPart(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_SharePointWebPart_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SharePointWebPart with test case SharePointWebPart")]
			[HostType("VS IDE")]
			public void Recipe_SharePointWebPart_RunTestCase_SharePointWebPart()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SharePointWebPart(coreRecipeGuidancePackage, this).RunTestCase_SharePointWebPart();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SharePointWebPart with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_SharePointWebPart_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SharePointWebPart(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_SimpleFilterConsumerWebPart_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SimpleFilterConsumerWebPart with test case SimpleFilterConsumerWebPart")]
			[HostType("VS IDE")]
			public void Recipe_SimpleFilterConsumerWebPart_RunTestCase_SimpleFilterConsumerWebPart()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SimpleFilterConsumerWebPart(coreRecipeGuidancePackage, this).RunTestCase_SimpleFilterConsumerWebPart();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SimpleFilterConsumerWebPart with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_SimpleFilterConsumerWebPart_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SimpleFilterConsumerWebPart(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_VisualWebPart_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe VisualWebPart with test case VisualWebPart")]
			[HostType("VS IDE")]
			public void Recipe_VisualWebPart_RunTestCase_VisualWebPart()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_VisualWebPart(coreRecipeGuidancePackage, this).RunTestCase_VisualWebPart();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe VisualWebPart with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_VisualWebPart_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_VisualWebPart(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_SimpleWorkflow_Tests : BaseTest
		{
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SimpleWorkflow with test case DefaultWorkflow")]
			[HostType("VS IDE")]
			public void Recipe_SimpleWorkflow_RunTestCase_DefaultWorkflow()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SimpleWorkflow(coreRecipeGuidancePackage, this).RunTestCase_DefaultWorkflow();
				});
			}
						[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe SimpleWorkflow with all test cases")]
			[HostType("VS IDE")]
			public void Recipe_SimpleWorkflow_RunAllTestCases()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_SimpleWorkflow(coreRecipeGuidancePackage, this).RunAllTestCases();
				});
			}
					}
			
		[TestClass()]
		public class Recipe_ClassLibraryProject_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ClassLibraryProject with default values")]
			[HostType("VS IDE")]
			public void Recipe_ClassLibraryProject_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ClassLibraryProject(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe ClassLibraryProject without required values")]
			[HostType("VS IDE")]
			public void Recipe_ClassLibraryProject_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_ClassLibraryProject(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_DeploymentProject_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe DeploymentProject with default values")]
			[HostType("VS IDE")]
			public void Recipe_DeploymentProject_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_DeploymentProject(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe DeploymentProject without required values")]
			[HostType("VS IDE")]
			public void Recipe_DeploymentProject_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_DeploymentProject(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_MSBuildProcessorRecipe_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe MSBuildProcessorRecipe with default values")]
			[HostType("VS IDE")]
			public void Recipe_MSBuildProcessorRecipe_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_MSBuildProcessorRecipe(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe MSBuildProcessorRecipe without required values")]
			[HostType("VS IDE")]
			public void Recipe_MSBuildProcessorRecipe_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_MSBuildProcessorRecipe(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_MSBuildTask_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe MSBuildTask with default values")]
			[HostType("VS IDE")]
			public void Recipe_MSBuildTask_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_MSBuildTask(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe MSBuildTask without required values")]
			[HostType("VS IDE")]
			public void Recipe_MSBuildTask_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_MSBuildTask(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
		[TestClass()]
		public class Recipe_PowerShellProject_Tests : BaseTest
		{
					
			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe PowerShellProject with default values")]
			[HostType("VS IDE")]
			public void Recipe_PowerShellProject_RunWithDefaultValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_PowerShellProject(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
				});
			}

			[TestMethod()]
			[TestCategory("Recipe Test")]
			[Description("Tests recipe PowerShellProject without required values")]
			[HostType("VS IDE")]
			public void Recipe_PowerShellProject_RunWithoutRequiredValues()
			{
				UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
					new RecipeRunner_PowerShellProject(coreRecipeGuidancePackage, this).RunRecipeWithoutRequiredValues();
				});
			}
		
					}
			
			[TestClass()]
			public class Category_Features_Tests : BaseTest
			{
				[TestMethod()]
				[Description("Runs solutions test cases")]
				[HostType("VS IDE")]
				public void Category_Features_RunSolutionTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe EmptyFeature SiteFeature");
								try
								{
									
									new RecipeRunner_EmptyFeature(coreRecipeGuidancePackage, this).RunTestCase_SiteFeature();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe EmptyFeature FeatureWithReceiver");
								try
								{
									
									new RecipeRunner_EmptyFeature(coreRecipeGuidancePackage, this).RunTestCase_FeatureWithReceiver();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe EmptyFeature WebFeature");
								try
								{
									
									new RecipeRunner_EmptyFeature(coreRecipeGuidancePackage, this).RunTestCase_WebFeature();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe EmptyFeature FeatureWithDependencies");
								try
								{
									
									new RecipeRunner_EmptyFeature(coreRecipeGuidancePackage, this).RunTestCase_FeatureWithDependencies();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
								
					});
				}

				[TestMethod()]
				[Description("Runs syntax test cases")]
				[HostType("VS IDE")]
				public void Category_Features_RunSyntaxTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe EmptyFeatureActivationDependency14 RunRecipeWithDefaultValues");
								new RecipeRunner_EmptyFeatureActivationDependency14(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe EmptyFeatureProperty RunRecipeWithDefaultValues");
								new RecipeRunner_EmptyFeatureProperty(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe EmptyFeatureUpgradeActionContentType RunRecipeWithDefaultValues");
								new RecipeRunner_EmptyFeatureUpgradeActionContentType(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
								
					});
				}			
			}			
	
				
			[TestClass()]
			public class Category_ListAndDoc_Tests : BaseTest
			{
				[TestMethod()]
				[Description("Runs solutions test cases")]
				[HostType("VS IDE")]
				public void Category_ListAndDoc_RunSolutionTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe ListInstance DefaultListInstance");
								try
								{
									
									new RecipeRunner_ListInstance(coreRecipeGuidancePackage, this).RunTestCase_DefaultListInstance();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe Module DefaultModule");
								try
								{
									
									new RecipeRunner_Module(coreRecipeGuidancePackage, this).RunTestCase_DefaultModule();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe ContentType ContentTypeWithReceiver");
								try
								{
									
									new RecipeRunner_ContentType(coreRecipeGuidancePackage, this).RunTestCase_ContentTypeWithReceiver();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe ContentType ContentTypeWithCustomForm");
								try
								{
									
									new RecipeRunner_ContentType(coreRecipeGuidancePackage, this).RunTestCase_ContentTypeWithCustomForm();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe ContentTypeBinding ContentTypeBindingAnnouncements");
								try
								{
									
									new RecipeRunner_ContentTypeBinding(coreRecipeGuidancePackage, this).RunTestCase_ContentTypeBindingAnnouncements();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
								
					});
				}

				[TestMethod()]
				[Description("Runs syntax test cases")]
				[HostType("VS IDE")]
				public void Category_ListAndDoc_RunSyntaxTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe ListDefinition RunRecipeWithDefaultValues");
								new RecipeRunner_ListDefinition(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe SiteColumn RunRecipeWithDefaultValues");
								new RecipeRunner_SiteColumn(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe ContentType DefaultContentType");
								try
								{
									new RecipeRunner_ContentType(coreRecipeGuidancePackage, this).RunTestCase_DefaultContentType();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe ContentTypeFieldRef RunRecipeWithDefaultValues");
								new RecipeRunner_ContentTypeFieldRef(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe ContentTypeDocumentTemplate RunRecipeWithDefaultValues");
								new RecipeRunner_ContentTypeDocumentTemplate(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe ContentTypeEventReceiver RunRecipeWithDefaultValues");
								new RecipeRunner_ContentTypeEventReceiver(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe ContentTypeCustomForm RunRecipeWithDefaultValues");
								new RecipeRunner_ContentTypeCustomForm(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe LinqToSharePoint RunRecipeWithDefaultValues");
								new RecipeRunner_LinqToSharePoint(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
								
					});
				}			
			}			
	
				
			[TestClass()]
			public class Category_Sites_Tests : BaseTest
			{
				[TestMethod()]
				[Description("Runs solutions test cases")]
				[HostType("VS IDE")]
				public void Category_Sites_RunSolutionTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe BlankSiteDefinition PublishingSiteDefinitionDefault");
								try
								{
									
									new RecipeRunner_BlankSiteDefinition(coreRecipeGuidancePackage, this).RunTestCase_PublishingSiteDefinitionDefault();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe TeamSiteDefinition PublishingSiteDefinitionDefault");
								try
								{
									
									new RecipeRunner_TeamSiteDefinition(coreRecipeGuidancePackage, this).RunTestCase_PublishingSiteDefinitionDefault();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe FeatureStapling FeatureAssocation1");
								try
								{
									
									new RecipeRunner_FeatureStapling(coreRecipeGuidancePackage, this).RunTestCase_FeatureAssocation1();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
								
					});
				}

				[TestMethod()]
				[Description("Runs syntax test cases")]
				[HostType("VS IDE")]
				public void Category_Sites_RunSyntaxTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

					
					});
				}			
			}			
	
				
			[TestClass()]
			public class Category_CustomActions_Tests : BaseTest
			{
				[TestMethod()]
				[Description("Runs solutions test cases")]
				[HostType("VS IDE")]
				public void Category_CustomActions_RunSolutionTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe CustomActionContentType CustomActionContentTypeWithUrlAction");
								try
								{
									
									new RecipeRunner_CustomActionContentType(coreRecipeGuidancePackage, this).RunTestCase_CustomActionContentTypeWithUrlAction();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe CustomActionContentType CustomActionContentTypeWithControlClass");
								try
								{
									
									new RecipeRunner_CustomActionContentType(coreRecipeGuidancePackage, this).RunTestCase_CustomActionContentTypeWithControlClass();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe CustomActionFileType CustomActionFileTypeWithUrlAction");
								try
								{
									
									new RecipeRunner_CustomActionFileType(coreRecipeGuidancePackage, this).RunTestCase_CustomActionFileTypeWithUrlAction();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe CustomActionFileType CustomActionFileTypeWithControlClass");
								try
								{
									
									new RecipeRunner_CustomActionFileType(coreRecipeGuidancePackage, this).RunTestCase_CustomActionFileTypeWithControlClass();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe CustomActionProgId CustomActionProgIdWithUrlAction");
								try
								{
									
									new RecipeRunner_CustomActionProgId(coreRecipeGuidancePackage, this).RunTestCase_CustomActionProgIdWithUrlAction();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe CustomActionProgId CustomActionProgIdWithControlClass");
								try
								{
									
									new RecipeRunner_CustomActionProgId(coreRecipeGuidancePackage, this).RunTestCase_CustomActionProgIdWithControlClass();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe CustomActionList CustomActionListWithUrlAction");
								try
								{
									
									new RecipeRunner_CustomActionList(coreRecipeGuidancePackage, this).RunTestCase_CustomActionListWithUrlAction();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe CustomActionList CustomActionListWithControlClass");
								try
								{
									
									new RecipeRunner_CustomActionList(coreRecipeGuidancePackage, this).RunTestCase_CustomActionListWithControlClass();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe CustomActionListToolbar CustomActionListToolbarWithUrlAction");
								try
								{
									
									new RecipeRunner_CustomActionListToolbar(coreRecipeGuidancePackage, this).RunTestCase_CustomActionListToolbarWithUrlAction();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe CustomActionListToolbar CustomActionListToolbarWithControlClass");
								try
								{
									
									new RecipeRunner_CustomActionListToolbar(coreRecipeGuidancePackage, this).RunTestCase_CustomActionListToolbarWithControlClass();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe CustomActionSite CustomActionSiteWithUrlAction");
								try
								{
									
									new RecipeRunner_CustomActionSite(coreRecipeGuidancePackage, this).RunTestCase_CustomActionSiteWithUrlAction();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe CustomActionSite CustomActionSiteWithControlClass");
								try
								{
									
									new RecipeRunner_CustomActionSite(coreRecipeGuidancePackage, this).RunTestCase_CustomActionSiteWithControlClass();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe CustomActionSiteActionsMenu CustomSiteActionWithUrlAction");
								try
								{
									
									new RecipeRunner_CustomActionSiteActionsMenu(coreRecipeGuidancePackage, this).RunTestCase_CustomSiteActionWithUrlAction();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe CustomActionSiteActionsMenu CustomSiteActionWithControlClass");
								try
								{
									
									new RecipeRunner_CustomActionSiteActionsMenu(coreRecipeGuidancePackage, this).RunTestCase_CustomSiteActionWithControlClass();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe CustomActionGroup CustomActionGroup");
								try
								{
									
									new RecipeRunner_CustomActionGroup(coreRecipeGuidancePackage, this).RunTestCase_CustomActionGroup();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe HideCustomAction HideCustomActionWithUrlAction");
								try
								{
									
									new RecipeRunner_HideCustomAction(coreRecipeGuidancePackage, this).RunTestCase_HideCustomActionWithUrlAction();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe RibbonGroup RibbonGroupForList");
								try
								{
									
									new RecipeRunner_RibbonGroup(coreRecipeGuidancePackage, this).RunTestCase_RibbonGroupForList();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe RibbonControl RibbonControlWithButton");
								try
								{
									
									new RecipeRunner_RibbonControl(coreRecipeGuidancePackage, this).RunTestCase_RibbonControlWithButton();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe RibbonSampleContentTypeButton RibbonGroupForList");
								try
								{
									
									new RecipeRunner_RibbonSampleContentTypeButton(coreRecipeGuidancePackage, this).RunTestCase_RibbonGroupForList();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe RibbonSampleTab SampleRibbonTab");
								try
								{
									
									new RecipeRunner_RibbonSampleTab(coreRecipeGuidancePackage, this).RunTestCase_SampleRibbonTab();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe HideRibbonControl Hide");
								try
								{
									
									new RecipeRunner_HideRibbonControl(coreRecipeGuidancePackage, this).RunTestCase_Hide();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
								
					});
				}

				[TestMethod()]
				[Description("Runs syntax test cases")]
				[HostType("VS IDE")]
				public void Category_CustomActions_RunSyntaxTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe CustomActionContentType CustomActionContentTypeWithControlTemplate");
								try
								{
									new RecipeRunner_CustomActionContentType(coreRecipeGuidancePackage, this).RunTestCase_CustomActionContentTypeWithControlTemplate();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe CustomActionFileType CustomActionFileTypeWithControlTemplate");
								try
								{
									new RecipeRunner_CustomActionFileType(coreRecipeGuidancePackage, this).RunTestCase_CustomActionFileTypeWithControlTemplate();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe CustomActionProgId CustomActionProgIdWithControlTemplate");
								try
								{
									new RecipeRunner_CustomActionProgId(coreRecipeGuidancePackage, this).RunTestCase_CustomActionProgIdWithControlTemplate();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe CustomActionList CustomActionListWithControlTemplate");
								try
								{
									new RecipeRunner_CustomActionList(coreRecipeGuidancePackage, this).RunTestCase_CustomActionListWithControlTemplate();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe CustomActionListToolbar CustomActionListToolbarWithControlTemplate");
								try
								{
									new RecipeRunner_CustomActionListToolbar(coreRecipeGuidancePackage, this).RunTestCase_CustomActionListToolbarWithControlTemplate();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe CustomActionSite CustomActionSiteWithControlTemplate");
								try
								{
									new RecipeRunner_CustomActionSite(coreRecipeGuidancePackage, this).RunTestCase_CustomActionSiteWithControlTemplate();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe CustomActionSiteActionsMenu CustomSiteActionWithControlTemplate");
								try
								{
									new RecipeRunner_CustomActionSiteActionsMenu(coreRecipeGuidancePackage, this).RunTestCase_CustomSiteActionWithControlTemplate();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe RibbonTab DefaultRibbonTab");
								try
								{
									new RecipeRunner_RibbonTab(coreRecipeGuidancePackage, this).RunTestCase_DefaultRibbonTab();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe RibbonTab RibbonTabWithPermissions");
								try
								{
									new RecipeRunner_RibbonTab(coreRecipeGuidancePackage, this).RunTestCase_RibbonTabWithPermissions();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe RibbonTab RibbonTabForList");
								try
								{
									new RecipeRunner_RibbonTab(coreRecipeGuidancePackage, this).RunTestCase_RibbonTabForList();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe RibbonGroup DefaultRibbonGroup");
								try
								{
									new RecipeRunner_RibbonGroup(coreRecipeGuidancePackage, this).RunTestCase_DefaultRibbonGroup();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe RibbonGroup RibbonGroupWithPermissions");
								try
								{
									new RecipeRunner_RibbonGroup(coreRecipeGuidancePackage, this).RunTestCase_RibbonGroupWithPermissions();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe RibbonControl DefaultRibbonControl");
								try
								{
									new RecipeRunner_RibbonControl(coreRecipeGuidancePackage, this).RunTestCase_DefaultRibbonControl();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe RibbonControl RibbonControlWithPermissions");
								try
								{
									new RecipeRunner_RibbonControl(coreRecipeGuidancePackage, this).RunTestCase_RibbonControlWithPermissions();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe RibbonSampleListViewButton RunRecipeWithDefaultValues");
								new RecipeRunner_RibbonSampleListViewButton(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe RibbonSampleItemFormButton RunRecipeWithDefaultValues");
								new RecipeRunner_RibbonSampleItemFormButton(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe RibbonSampleContentTypeButton DefaultRibbonGroup");
								try
								{
									new RecipeRunner_RibbonSampleContentTypeButton(coreRecipeGuidancePackage, this).RunTestCase_DefaultRibbonGroup();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe RibbonSampleTab DefaultRibbonTab");
								try
								{
									new RecipeRunner_RibbonSampleTab(coreRecipeGuidancePackage, this).RunTestCase_DefaultRibbonTab();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe HideRibbonControl DefaultRibbonControl");
								try
								{
									new RecipeRunner_HideRibbonControl(coreRecipeGuidancePackage, this).RunTestCase_DefaultRibbonControl();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
								
					});
				}			
			}			
	
				
			[TestClass()]
			public class Category_Events_Tests : BaseTest
			{
				[TestMethod()]
				[Description("Runs solutions test cases")]
				[HostType("VS IDE")]
				public void Category_Events_RunSolutionTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe ItemEventReceiver ItemEventWithScopeSite");
								try
								{
									
									new RecipeRunner_ItemEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_ItemEventWithScopeSite();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe ListEventReceiver ListEventWithScopeSite");
								try
								{
									
									new RecipeRunner_ListEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_ListEventWithScopeSite();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe WebEventReceiver WebEventWithScopeSite");
								try
								{
									
									new RecipeRunner_WebEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_WebEventWithScopeSite();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe EmailEventReceiver Default");
								try
								{
									
									new RecipeRunner_EmailEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_Default();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
								
					});
				}

				[TestMethod()]
				[Description("Runs syntax test cases")]
				[HostType("VS IDE")]
				public void Category_Events_RunSyntaxTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe ItemEventReceiver Default");
								try
								{
									new RecipeRunner_ItemEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_Default();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe ItemEventReceiver ItemEventWithRootWebOnly");
								try
								{
									new RecipeRunner_ItemEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_ItemEventWithRootWebOnly();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe ListEventReceiver Default");
								try
								{
									new RecipeRunner_ListEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_Default();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe ListEventReceiver ListEventWithRootWebOnly");
								try
								{
									new RecipeRunner_ListEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_ListEventWithRootWebOnly();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe WebEventReceiver Default");
								try
								{
									new RecipeRunner_WebEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_Default();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe WebEventReceiver WebEventWithRootWebOnly");
								try
								{
									new RecipeRunner_WebEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_WebEventWithRootWebOnly();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
								
					});
				}			
			}			
	
				
			[TestClass()]
			public class Category_WebParts_Tests : BaseTest
			{
				[TestMethod()]
				[Description("Runs solutions test cases")]
				[HostType("VS IDE")]
				public void Category_WebParts_RunSolutionTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe SharePointWebPart SharePointWebPart");
								try
								{
									
									new RecipeRunner_SharePointWebPart(coreRecipeGuidancePackage, this).RunTestCase_SharePointWebPart();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe ASPWebPart ASPWebPart");
								try
								{
									
									new RecipeRunner_ASPWebPart(coreRecipeGuidancePackage, this).RunTestCase_ASPWebPart();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe VisualWebPart VisualWebPart");
								try
								{
									
									new RecipeRunner_VisualWebPart(coreRecipeGuidancePackage, this).RunTestCase_VisualWebPart();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe AjaxWebPart AjaxWebPart");
								try
								{
									
									new RecipeRunner_AjaxWebPart(coreRecipeGuidancePackage, this).RunTestCase_AjaxWebPart();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe SimpleFilterConsumerWebPart SimpleFilterConsumerWebPart");
								try
								{
									
									new RecipeRunner_SimpleFilterConsumerWebPart(coreRecipeGuidancePackage, this).RunTestCase_SimpleFilterConsumerWebPart();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
								
					});
				}

				[TestMethod()]
				[Description("Runs syntax test cases")]
				[HostType("VS IDE")]
				public void Category_WebParts_RunSyntaxTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe FilterConsumerWebPart RunRecipeWithDefaultValues");
								new RecipeRunner_FilterConsumerWebPart(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe FilterProviderWebPart RunRecipeWithDefaultValues");
								new RecipeRunner_FilterProviderWebPart(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
								
					});
				}			
			}			
	
				
			[TestClass()]
			public class Category_FieldTypes_Tests : BaseTest
			{
				[TestMethod()]
				[Description("Runs solutions test cases")]
				[HostType("VS IDE")]
				public void Category_FieldTypes_RunSolutionTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe FieldType FieldTypeDefault");
								try
								{
									
									new RecipeRunner_FieldType(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeDefault();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe FieldTypeComplexValue FieldTypeComplexValue");
								try
								{
									
									new RecipeRunner_FieldTypeComplexValue(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeComplexValue();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe FieldTypeMultiColumn FieldTypeMultiColumn");
								try
								{
									
									new RecipeRunner_FieldTypeMultiColumn(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeMultiColumn();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe FieldTypeChoice FieldTypeChoice");
								try
								{
									
									new RecipeRunner_FieldTypeChoice(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeChoice();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe FieldTypeISBN10 FieldTypeISBN");
								try
								{
									
									new RecipeRunner_FieldTypeISBN10(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeISBN();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe FieldTypeSSN FieldTypeSSN");
								try
								{
									
									new RecipeRunner_FieldTypeSSN(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeSSN();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe FieldTypeFlash FieldTypeFlash");
								try
								{
									
									new RecipeRunner_FieldTypeFlash(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeFlash();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe FieldTypeRatings FieldTypeRatings");
								try
								{
									
									new RecipeRunner_FieldTypeRatings(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeRatings();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
								
					});
				}

				[TestMethod()]
				[Description("Runs syntax test cases")]
				[HostType("VS IDE")]
				public void Category_FieldTypes_RunSyntaxTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

					
					});
				}			
			}			
	
				
			[TestClass()]
			public class Category_Publishing_Tests : BaseTest
			{
				[TestMethod()]
				[Description("Runs solutions test cases")]
				[HostType("VS IDE")]
				public void Category_Publishing_RunSolutionTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe PubSiteDef PublishingSiteDefinitionDefault");
								try
								{
									
									new RecipeRunner_PubSiteDef(coreRecipeGuidancePackage, this).RunTestCase_PublishingSiteDefinitionDefault();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe MasterPage DefaultMasterPage");
								try
								{
									
									new RecipeRunner_MasterPage(coreRecipeGuidancePackage, this).RunTestCase_DefaultMasterPage();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe PageLayout DefaultPageLayout");
								try
								{
									
									new RecipeRunner_PageLayout(coreRecipeGuidancePackage, this).RunTestCase_DefaultPageLayout();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
								
					});
				}

				[TestMethod()]
				[Description("Runs syntax test cases")]
				[HostType("VS IDE")]
				public void Category_Publishing_RunSyntaxTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe PubSiteDef PublishingSiteDefinitionWithProvisioningXML");
								try
								{
									new RecipeRunner_PubSiteDef(coreRecipeGuidancePackage, this).RunTestCase_PublishingSiteDefinitionWithProvisioningXML();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe PubSiteDef PublishingSiteDefinitionWithProvisioningHandler");
								try
								{
									new RecipeRunner_PubSiteDef(coreRecipeGuidancePackage, this).RunTestCase_PublishingSiteDefinitionWithProvisioningHandler();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
								
					});
				}			
			}			
	
				
			[TestClass()]
			public class Category_ApplicationPages_Tests : BaseTest
			{
				[TestMethod()]
				[Description("Runs solutions test cases")]
				[HostType("VS IDE")]
				public void Category_ApplicationPages_RunSolutionTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe AdministrationPage DefaultAdministrationPage");
								try
								{
									
									new RecipeRunner_AdministrationPage(coreRecipeGuidancePackage, this).RunTestCase_DefaultAdministrationPage();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe ApplicationPage DefaultApplicationPage");
								try
								{
									
									new RecipeRunner_ApplicationPage(coreRecipeGuidancePackage, this).RunTestCase_DefaultApplicationPage();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe UnsecuredApplicationPage AnonymousApplicationPage");
								try
								{
									
									new RecipeRunner_UnsecuredApplicationPage(coreRecipeGuidancePackage, this).RunTestCase_AnonymousApplicationPage();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
								
					});
				}

				[TestMethod()]
				[Description("Runs syntax test cases")]
				[HostType("VS IDE")]
				public void Category_ApplicationPages_RunSyntaxTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe UnsecuredApplicationPage DefaultApplicationPage");
								try
								{
									new RecipeRunner_UnsecuredApplicationPage(coreRecipeGuidancePackage, this).RunTestCase_DefaultApplicationPage();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
								
					});
				}			
			}			
	
				
			[TestClass()]
			public class Category_ASPNET_Tests : BaseTest
			{
				[TestMethod()]
				[Description("Runs solutions test cases")]
				[HostType("VS IDE")]
				public void Category_ASPNET_RunSolutionTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe WebService DefaultWebService");
								try
								{
									
									new RecipeRunner_WebService(coreRecipeGuidancePackage, this).RunTestCase_DefaultWebService();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe WCFWebService DefaultWebService");
								try
								{
									
									new RecipeRunner_WCFWebService(coreRecipeGuidancePackage, this).RunTestCase_DefaultWebService();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe AdministrationWebService DefaultAdministrationWebService");
								try
								{
									
									new RecipeRunner_AdministrationWebService(coreRecipeGuidancePackage, this).RunTestCase_DefaultAdministrationWebService();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe DelegateControlASCX DefaultDelegateControlASCX");
								try
								{
									
									new RecipeRunner_DelegateControlASCX(coreRecipeGuidancePackage, this).RunTestCase_DefaultDelegateControlASCX();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe DelegateControlCS DefaultDelegateControlCS");
								try
								{
									
									new RecipeRunner_DelegateControlCS(coreRecipeGuidancePackage, this).RunTestCase_DefaultDelegateControlCS();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe HttpHandler DefaultHttpHandler");
								try
								{
									
									new RecipeRunner_HttpHandler(coreRecipeGuidancePackage, this).RunTestCase_DefaultHttpHandler();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe HttpModule DefaultHttpModule");
								try
								{
									
									new RecipeRunner_HttpModule(coreRecipeGuidancePackage, this).RunTestCase_DefaultHttpModule();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
								
					});
				}

				[TestMethod()]
				[Description("Runs syntax test cases")]
				[HostType("VS IDE")]
				public void Category_ASPNET_RunSyntaxTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

					
					});
				}			
			}			
	
				
			[TestClass()]
			public class Category_Workflows_Tests : BaseTest
			{
				[TestMethod()]
				[Description("Runs solutions test cases")]
				[HostType("VS IDE")]
				public void Category_Workflows_RunSolutionTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe SimpleWorkflow DefaultWorkflow");
								try
								{
									
									new RecipeRunner_SimpleWorkflow(coreRecipeGuidancePackage, this).RunTestCase_DefaultWorkflow();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
								
					});
				}

				[TestMethod()]
				[Description("Runs syntax test cases")]
				[HostType("VS IDE")]
				public void Category_Workflows_RunSyntaxTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

					
					});
				}			
			}			
	
				
			[TestClass()]
			public class Category_BCS_Tests : BaseTest
			{
				[TestMethod()]
				[Description("Runs solutions test cases")]
				[HostType("VS IDE")]
				public void Category_BCS_RunSolutionTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

					
					});
				}

				[TestMethod()]
				[Description("Runs syntax test cases")]
				[HostType("VS IDE")]
				public void Category_BCS_RunSyntaxTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe EmptyBCSModel RunRecipeWithDefaultValues");
								new RecipeRunner_EmptyBCSModel(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe BCSModelFromDatabaseWithSQLQueries RunRecipeWithDefaultValues");
								new RecipeRunner_BCSModelFromDatabaseWithSQLQueries(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe BCSModelFromDatabaseWithClasses RunRecipeWithDefaultValues");
								new RecipeRunner_BCSModelFromDatabaseWithClasses(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe BCSModelFromDatabaseWithLINQ RunRecipeWithDefaultValues");
								new RecipeRunner_BCSModelFromDatabaseWithLINQ(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe BCSDesignWithAssembly RunRecipeWithDefaultValues");
								new RecipeRunner_BCSDesignWithAssembly(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe BCSListInstance RunRecipeWithDefaultValues");
								new RecipeRunner_BCSListInstance(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe BCSQuickDeploy RunRecipeWithDefaultValues");
								new RecipeRunner_BCSQuickDeploy(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe BCSQuickDeployIncAssembly RunRecipeWithDefaultValues");
								new RecipeRunner_BCSQuickDeployIncAssembly(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
								
					});
				}			
			}			
	
				
			[TestClass()]
			public class Category_Security_Tests : BaseTest
			{
				[TestMethod()]
				[Description("Runs solutions test cases")]
				[HostType("VS IDE")]
				public void Category_Security_RunSolutionTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

					
					});
				}

				[TestMethod()]
				[Description("Runs syntax test cases")]
				[HostType("VS IDE")]
				public void Category_Security_RunSyntaxTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe MinimalCAS RunRecipeWithDefaultValues");
								new RecipeRunner_MinimalCAS(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe MediumCAS RunRecipeWithDefaultValues");
								new RecipeRunner_MediumCAS(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe AspNetHostingPermission RunRecipeWithDefaultValues");
								new RecipeRunner_AspNetHostingPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe DnsPermission RunRecipeWithDefaultValues");
								new RecipeRunner_DnsPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe EnvironmentPermission RunRecipeWithDefaultValues");
								new RecipeRunner_EnvironmentPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe ReflectionPermission RunRecipeWithDefaultValues");
								new RecipeRunner_ReflectionPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe FileIOPermission RunRecipeWithDefaultValues");
								new RecipeRunner_FileIOPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe PrintingPermission RunRecipeWithDefaultValues");
								new RecipeRunner_PrintingPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe SqlClientPermission RunRecipeWithDefaultValues");
								new RecipeRunner_SqlClientPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe WebPartPermission RunRecipeWithDefaultValues");
								new RecipeRunner_WebPartPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe RegistryPermission RunRecipeWithDefaultValues");
								new RecipeRunner_RegistryPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe SecurityPermission RunRecipeWithDefaultValues");
								new RecipeRunner_SecurityPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe SharePointPermission RunRecipeWithDefaultValues");
								new RecipeRunner_SharePointPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe SmtpPermission RunRecipeWithDefaultValues");
								new RecipeRunner_SmtpPermission(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
								
					});
				}			
			}			
	
				
			[TestClass()]
			public class Category_Files_Tests : BaseTest
			{
				[TestMethod()]
				[Description("Runs solutions test cases")]
				[HostType("VS IDE")]
				public void Category_Files_RunSolutionTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe GlobalResxFile Default");
								try
								{
									
									new RecipeRunner_GlobalResxFile(coreRecipeGuidancePackage, this).RunTestCase_Default();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe ClassResourceFile Default");
								try
								{
									
									new RecipeRunner_ClassResourceFile(coreRecipeGuidancePackage, this).RunTestCase_Default();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe ApplicationResourceFile Default");
								try
								{
									
									new RecipeRunner_ApplicationResourceFile(coreRecipeGuidancePackage, this).RunTestCase_Default();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe ImageFile Default");
								try
								{
									
									new RecipeRunner_ImageFile(coreRecipeGuidancePackage, this).RunTestCase_Default();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe LayoutsFile Default");
								try
								{
									
									new RecipeRunner_LayoutsFile(coreRecipeGuidancePackage, this).RunTestCase_Default();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
								
					});
				}

				[TestMethod()]
				[Description("Runs syntax test cases")]
				[HostType("VS IDE")]
				public void Category_Files_RunSyntaxTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

					
					});
				}			
			}			
	
				
			[TestClass()]
			public class Category_Silverlight_Tests : BaseTest
			{
				[TestMethod()]
				[Description("Runs solutions test cases")]
				[HostType("VS IDE")]
				public void Category_Silverlight_RunSolutionTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe SilverlightApplication EmptySilverlightApplication");
								try
								{
									
									new RecipeRunner_SilverlightApplication(coreRecipeGuidancePackage, this).RunTestCase_EmptySilverlightApplication();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe SilverlightApplicationSampleDataBinding DataBindingSilverlightApplication");
								try
								{
									
									new RecipeRunner_SilverlightApplicationSampleDataBinding(coreRecipeGuidancePackage, this).RunTestCase_DataBindingSilverlightApplication();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe SilverlightApplicationSampleTaskAdder TaskAdderSilverlightApplication");
								try
								{
									
									new RecipeRunner_SilverlightApplicationSampleTaskAdder(coreRecipeGuidancePackage, this).RunTestCase_TaskAdderSilverlightApplication();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe SilverlightApplicationSampleListViewer ListViewerSilverlightApplication");
								try
								{
									
									new RecipeRunner_SilverlightApplicationSampleListViewer(coreRecipeGuidancePackage, this).RunTestCase_ListViewerSilverlightApplication();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
								
					});
				}

				[TestMethod()]
				[Description("Runs syntax test cases")]
				[HostType("VS IDE")]
				public void Category_Silverlight_RunSyntaxTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

					
					});
				}			
			}			
	
				
			[TestClass()]
			public class Category_PowerShell_Tests : BaseTest
			{
				[TestMethod()]
				[Description("Runs solutions test cases")]
				[HostType("VS IDE")]
				public void Category_PowerShell_RunSolutionTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe SPCmdLet DefaultCommand");
								try
								{
									
									new RecipeRunner_SPCmdLet(coreRecipeGuidancePackage, this).RunTestCase_DefaultCommand();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe SPCmdLetWithPipeBind GetSPListByName");
								try
								{
									
									new RecipeRunner_SPCmdLetWithPipeBind(coreRecipeGuidancePackage, this).RunTestCase_GetSPListByName();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
								
					});
				}

				[TestMethod()]
				[Description("Runs syntax test cases")]
				[HostType("VS IDE")]
				public void Category_PowerShell_RunSyntaxTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe SPCmdLetWithPipeBind DefaultCommand");
								try
								{
									new RecipeRunner_SPCmdLetWithPipeBind(coreRecipeGuidancePackage, this).RunTestCase_DefaultCommand();
								}
								catch(Exception ex)
								{
									throw new Exception(ex.ToString());
								}
																this.TestContext.WriteLine("Running Recipe PowerShellCmdLet RunRecipeWithDefaultValues");
								new RecipeRunner_PowerShellCmdLet(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe PowerShellPSCmdLet RunRecipeWithDefaultValues");
								new RecipeRunner_PowerShellPSCmdLet(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
																this.TestContext.WriteLine("Running Recipe PowerShellScript RunRecipeWithDefaultValues");
								new RecipeRunner_PowerShellScript(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
								
					});
				}			
			}			
	
				
			[TestClass()]
			public class Category_Helpers_Tests : BaseTest
			{
				[TestMethod()]
				[Description("Runs solutions test cases")]
				[HostType("VS IDE")]
				public void Category_Helpers_RunSolutionTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

					
					});
				}

				[TestMethod()]
				[Description("Runs syntax test cases")]
				[HostType("VS IDE")]
				public void Category_Helpers_RunSyntaxTestCases()
				{
					UIThreadInvoker.Invoke((ThreadInvoker)delegate()
					{

													this.TestContext.WriteLine("Running Recipe MSBuildProcessorRecipe RunRecipeWithDefaultValues");
								new RecipeRunner_MSBuildProcessorRecipe(coreRecipeGuidancePackage, this).RunRecipeWithDefaultValues();
					
								
					});
				}			
			}			
	
				[TestClass()]
	public class AllSolutionsTest : BaseTest
	{
		[TestMethod()]
		[Description("Runs all solutions test cases")]
		[HostType("VS IDE")]
		public void AllSolutionTestCases()
		{
			UIThreadInvoker.Invoke((ThreadInvoker)delegate()
				{
			new RecipeRunner_EmptyFeature(coreRecipeGuidancePackage, this).RunTestCase_SiteFeature();
				new RecipeRunner_EmptyFeature(coreRecipeGuidancePackage, this).RunTestCase_FeatureWithReceiver();
				new RecipeRunner_EmptyFeature(coreRecipeGuidancePackage, this).RunTestCase_WebFeature();
				new RecipeRunner_EmptyFeature(coreRecipeGuidancePackage, this).RunTestCase_FeatureWithDependencies();
				new RecipeRunner_ListInstance(coreRecipeGuidancePackage, this).RunTestCase_DefaultListInstance();
				new RecipeRunner_Module(coreRecipeGuidancePackage, this).RunTestCase_DefaultModule();
				new RecipeRunner_ContentType(coreRecipeGuidancePackage, this).RunTestCase_ContentTypeWithReceiver();
				new RecipeRunner_ContentType(coreRecipeGuidancePackage, this).RunTestCase_ContentTypeWithCustomForm();
				new RecipeRunner_ContentTypeBinding(coreRecipeGuidancePackage, this).RunTestCase_ContentTypeBindingAnnouncements();
				new RecipeRunner_BlankSiteDefinition(coreRecipeGuidancePackage, this).RunTestCase_PublishingSiteDefinitionDefault();
				new RecipeRunner_TeamSiteDefinition(coreRecipeGuidancePackage, this).RunTestCase_PublishingSiteDefinitionDefault();
				new RecipeRunner_FeatureStapling(coreRecipeGuidancePackage, this).RunTestCase_FeatureAssocation1();
				new RecipeRunner_CustomActionContentType(coreRecipeGuidancePackage, this).RunTestCase_CustomActionContentTypeWithUrlAction();
				new RecipeRunner_CustomActionContentType(coreRecipeGuidancePackage, this).RunTestCase_CustomActionContentTypeWithControlClass();
				new RecipeRunner_CustomActionFileType(coreRecipeGuidancePackage, this).RunTestCase_CustomActionFileTypeWithUrlAction();
				new RecipeRunner_CustomActionFileType(coreRecipeGuidancePackage, this).RunTestCase_CustomActionFileTypeWithControlClass();
				new RecipeRunner_CustomActionProgId(coreRecipeGuidancePackage, this).RunTestCase_CustomActionProgIdWithUrlAction();
				new RecipeRunner_CustomActionProgId(coreRecipeGuidancePackage, this).RunTestCase_CustomActionProgIdWithControlClass();
				new RecipeRunner_CustomActionList(coreRecipeGuidancePackage, this).RunTestCase_CustomActionListWithUrlAction();
				new RecipeRunner_CustomActionList(coreRecipeGuidancePackage, this).RunTestCase_CustomActionListWithControlClass();
				new RecipeRunner_CustomActionListToolbar(coreRecipeGuidancePackage, this).RunTestCase_CustomActionListToolbarWithUrlAction();
				new RecipeRunner_CustomActionListToolbar(coreRecipeGuidancePackage, this).RunTestCase_CustomActionListToolbarWithControlClass();
				new RecipeRunner_CustomActionSite(coreRecipeGuidancePackage, this).RunTestCase_CustomActionSiteWithUrlAction();
				new RecipeRunner_CustomActionSite(coreRecipeGuidancePackage, this).RunTestCase_CustomActionSiteWithControlClass();
				new RecipeRunner_CustomActionSiteActionsMenu(coreRecipeGuidancePackage, this).RunTestCase_CustomSiteActionWithUrlAction();
				new RecipeRunner_CustomActionSiteActionsMenu(coreRecipeGuidancePackage, this).RunTestCase_CustomSiteActionWithControlClass();
				new RecipeRunner_CustomActionGroup(coreRecipeGuidancePackage, this).RunTestCase_CustomActionGroup();
				new RecipeRunner_HideCustomAction(coreRecipeGuidancePackage, this).RunTestCase_HideCustomActionWithUrlAction();
				new RecipeRunner_RibbonGroup(coreRecipeGuidancePackage, this).RunTestCase_RibbonGroupForList();
				new RecipeRunner_RibbonControl(coreRecipeGuidancePackage, this).RunTestCase_RibbonControlWithButton();
				new RecipeRunner_RibbonSampleContentTypeButton(coreRecipeGuidancePackage, this).RunTestCase_RibbonGroupForList();
				new RecipeRunner_RibbonSampleTab(coreRecipeGuidancePackage, this).RunTestCase_SampleRibbonTab();
				new RecipeRunner_HideRibbonControl(coreRecipeGuidancePackage, this).RunTestCase_Hide();
				new RecipeRunner_ItemEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_ItemEventWithScopeSite();
				new RecipeRunner_ListEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_ListEventWithScopeSite();
				new RecipeRunner_WebEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_WebEventWithScopeSite();
				new RecipeRunner_EmailEventReceiver(coreRecipeGuidancePackage, this).RunTestCase_Default();
				new RecipeRunner_SharePointWebPart(coreRecipeGuidancePackage, this).RunTestCase_SharePointWebPart();
				new RecipeRunner_ASPWebPart(coreRecipeGuidancePackage, this).RunTestCase_ASPWebPart();
				new RecipeRunner_VisualWebPart(coreRecipeGuidancePackage, this).RunTestCase_VisualWebPart();
				new RecipeRunner_AjaxWebPart(coreRecipeGuidancePackage, this).RunTestCase_AjaxWebPart();
				new RecipeRunner_SimpleFilterConsumerWebPart(coreRecipeGuidancePackage, this).RunTestCase_SimpleFilterConsumerWebPart();
				new RecipeRunner_FieldType(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeDefault();
				new RecipeRunner_FieldTypeComplexValue(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeComplexValue();
				new RecipeRunner_FieldTypeMultiColumn(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeMultiColumn();
				new RecipeRunner_FieldTypeChoice(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeChoice();
				new RecipeRunner_FieldTypeISBN10(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeISBN();
				new RecipeRunner_FieldTypeSSN(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeSSN();
				new RecipeRunner_FieldTypeFlash(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeFlash();
				new RecipeRunner_FieldTypeRatings(coreRecipeGuidancePackage, this).RunTestCase_FieldTypeRatings();
				new RecipeRunner_PubSiteDef(coreRecipeGuidancePackage, this).RunTestCase_PublishingSiteDefinitionDefault();
				new RecipeRunner_MasterPage(coreRecipeGuidancePackage, this).RunTestCase_DefaultMasterPage();
				new RecipeRunner_PageLayout(coreRecipeGuidancePackage, this).RunTestCase_DefaultPageLayout();
				new RecipeRunner_AdministrationPage(coreRecipeGuidancePackage, this).RunTestCase_DefaultAdministrationPage();
				new RecipeRunner_ApplicationPage(coreRecipeGuidancePackage, this).RunTestCase_DefaultApplicationPage();
				new RecipeRunner_UnsecuredApplicationPage(coreRecipeGuidancePackage, this).RunTestCase_AnonymousApplicationPage();
				new RecipeRunner_WebService(coreRecipeGuidancePackage, this).RunTestCase_DefaultWebService();
				new RecipeRunner_WCFWebService(coreRecipeGuidancePackage, this).RunTestCase_DefaultWebService();
				new RecipeRunner_AdministrationWebService(coreRecipeGuidancePackage, this).RunTestCase_DefaultAdministrationWebService();
				new RecipeRunner_DelegateControlASCX(coreRecipeGuidancePackage, this).RunTestCase_DefaultDelegateControlASCX();
				new RecipeRunner_DelegateControlCS(coreRecipeGuidancePackage, this).RunTestCase_DefaultDelegateControlCS();
				new RecipeRunner_HttpHandler(coreRecipeGuidancePackage, this).RunTestCase_DefaultHttpHandler();
				new RecipeRunner_HttpModule(coreRecipeGuidancePackage, this).RunTestCase_DefaultHttpModule();
				new RecipeRunner_SimpleWorkflow(coreRecipeGuidancePackage, this).RunTestCase_DefaultWorkflow();
				new RecipeRunner_GlobalResxFile(coreRecipeGuidancePackage, this).RunTestCase_Default();
				new RecipeRunner_ClassResourceFile(coreRecipeGuidancePackage, this).RunTestCase_Default();
				new RecipeRunner_ApplicationResourceFile(coreRecipeGuidancePackage, this).RunTestCase_Default();
				new RecipeRunner_ImageFile(coreRecipeGuidancePackage, this).RunTestCase_Default();
				new RecipeRunner_LayoutsFile(coreRecipeGuidancePackage, this).RunTestCase_Default();
				new RecipeRunner_SilverlightApplication(coreRecipeGuidancePackage, this).RunTestCase_EmptySilverlightApplication();
				new RecipeRunner_SilverlightApplicationSampleDataBinding(coreRecipeGuidancePackage, this).RunTestCase_DataBindingSilverlightApplication();
				new RecipeRunner_SilverlightApplicationSampleTaskAdder(coreRecipeGuidancePackage, this).RunTestCase_TaskAdderSilverlightApplication();
				new RecipeRunner_SilverlightApplicationSampleListViewer(coreRecipeGuidancePackage, this).RunTestCase_ListViewerSilverlightApplication();
				new RecipeRunner_SPCmdLet(coreRecipeGuidancePackage, this).RunTestCase_DefaultCommand();
				new RecipeRunner_SPCmdLetWithPipeBind(coreRecipeGuidancePackage, this).RunTestCase_GetSPListByName();
				});
		}
	}
}
