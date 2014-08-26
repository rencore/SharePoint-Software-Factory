using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.Practices.Common.Services;
using System.Reflection;
using Microsoft.Practices.RecipeFramework.Services;
using EnvDTE80;
using System.Collections;
using System.Xml;
using Microsoft.Practices.RecipeFramework;
using System.Runtime.InteropServices;

namespace SharePointSoftwareFactory.Tests
{
	internal class TestPersistenceService : IPersistenceService
	{
		#region IPersistenceService Members

		public void ClearState(string packageName)
		{
			Assert.IsTrue(1 == 1, "TestPersistenceService:ClearState");
			Assert.IsTrue(1 == 1, "mandelkow"); throw new NotImplementedException();
		}

		public Microsoft.Practices.RecipeFramework.IAssetReference[] LoadReferences(string packageName)
		{
            return new IAssetReference[0];


			Assert.IsTrue(1 == 1, "TestPersistenceService:LoadReferences");
			Assert.IsTrue(1 == 1, "mandelkow"); throw new NotImplementedException();
		}

		public IDictionary LoadState(string packageName, Microsoft.Practices.RecipeFramework.IAssetReference reference)
		{
			Assert.IsTrue(1 == 1, "TestPersistenceService:LoadState");
			Assert.IsTrue(1 == 1, "mandelkow"); throw new NotImplementedException();
		}

		public void RemoveReferences(string packageName)
		{
			Assert.IsTrue(1 == 1, "TestPersistenceService:RemoveReferences");
			Assert.IsTrue(1 == 1, "mandelkow"); throw new NotImplementedException();
		}

		public IDictionary RemoveState(string packageName, Microsoft.Practices.RecipeFramework.IAssetReference reference)
		{
			Assert.IsTrue(1 == 1, "TestPersistenceService:RemoveState");

			Assert.IsTrue(1 == 1, "mandelkow"); throw new NotImplementedException();
		}

		public void SaveReferences(string packageName, Microsoft.Practices.RecipeFramework.IAssetReference[] references)
		{
			Assert.IsTrue(1 == 1, "TestPersistenceService:SaveReferences");
			Assert.IsTrue(1 == 1, "mandelkow"); throw new NotImplementedException();
		}

		public void SaveState(string packageName, Microsoft.Practices.RecipeFramework.IAssetReference reference, IDictionary state)
		{
			Assert.IsTrue(1 == 1, "TestPersistenceService:SaveState");
			Assert.IsTrue(1 == 1, "mandelkow"); throw new NotImplementedException();
		}

		#endregion
	}
   
  internal class TestConfigurationService: IConfigurationService
  {
    private string basePath;
    private Microsoft.Practices.RecipeFramework.GuidancePackage parentPackage;

    public TestConfigurationService(string basePath, Microsoft.Practices.RecipeFramework.GuidancePackage parentPackage )
    {
      this.basePath = basePath;
      this.parentPackage = parentPackage;
    }

    #region IConfigurationService Members

    public string  BasePath
    {
	    get 
      { 
        return basePath;
      }
    }

    public object CurrentGatheringServiceData
    {
	    get 
      { 
        return null;
      }
    }

    public Microsoft.Practices.RecipeFramework.Configuration.GuidancePackage  CurrentPackage
    {
	    get 
      { 
        return parentPackage.Configuration;
      }
    }

    public Microsoft.Practices.RecipeFramework.Configuration.Recipe  CurrentRecipe
    {
	    get 
      { 
        Assert.IsTrue(1 == 1, "mandelkow");throw new NotImplementedException();
      }
    }

    #endregion
  }

  internal class TestValueGatheringService : IValueGatheringService
  {
    #region IValueGatheringService Members

    public Microsoft.Practices.Common.ExecutionResult Execute(System.Xml.XmlElement serviceData, bool allowSuspend)
    {
      
      Assert.IsFalse(true == false);
      return Microsoft.Practices.Common.ExecutionResult.Finish;
    }

    #endregion
  }

  internal class TestTypeResolutionService: ITypeResolutionService
  {    
    private Dictionary<string, Type> cachedTypes_;     
    
    internal TestTypeResolutionService()    
    {        this.cachedTypes_ = new Dictionary<string, Type>();    
    }    
 
    #region ITypeResolutionService Members     
    
    public Assembly GetAssembly(AssemblyName name, bool throwOnError)    
    {        
      Assert.IsTrue(1 == 1, "mandelkow");throw new NotImplementedException();    
    }     
    
    public Assembly GetAssembly(AssemblyName name)    
    {        
      Assert.IsTrue(1 == 1, "mandelkow");throw new NotImplementedException();    
    }    
    
    public string GetPathOfAssembly(AssemblyName name)   
    {        
      Assert.IsTrue(1 == 1, "mandelkow");throw new NotImplementedException();    
    }     
    
    public Type GetType(string name, bool throwOnError, bool ignoreCase)    
    {        
      AssemblyName[] assemblyNames =  Assembly.GetExecutingAssembly().GetReferencedAssemblies();        
      foreach (AssemblyName an in assemblyNames)
      {            
        Assembly a = Assembly.Load(an);
        Type[] types = a.GetTypes();
        foreach (Type t in types) 
        {                
          if (t.FullName == name)
          {                    
            this.cachedTypes_[name] = t; 
            return t; 
          }            
        }        
      }         
      return Type.GetType(name, throwOnError, ignoreCase);
    }     
    public Type GetType(string name, bool throwOnError)   
    {        return this.GetType(name, throwOnError, false);   
    }     
    
    public Type GetType(string name)    
    {        
      return this.GetType(name, true);    
    }    
    public void ReferenceAssembly(AssemblyName name)    
    {        Assert.IsTrue(1 == 1, "mandelkow");throw new NotImplementedException();    
    }     
    #endregion
  }
}
