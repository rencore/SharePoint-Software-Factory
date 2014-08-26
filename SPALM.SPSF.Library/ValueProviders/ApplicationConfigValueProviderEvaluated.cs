using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Security.Cryptography;
using System.Collections;
using System.Diagnostics;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.Common;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Services;

namespace SPALM.SPSF.Library.ValueProviders
{
    [ServiceDependency(typeof(DTE))]
	public class ApplicationConfigValueProviderEvaluated : ValueProvider, IAttributesConfigurable
    {
			protected string GetBasePath()
			{
				return base.GetService<IConfigurationService>(true).BasePath;
			}

        private string _ConfigValue = "";
        public string ConfigValue
        {
            get { return _ConfigValue; }
            set { _ConfigValue = value; }
        }

        private string _DefaultValue = "";
        public string DefaultValue
        {
          get { return _DefaultValue; }
          set { _DefaultValue = value; }
        }

        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
          if (currentValue != null)
          {
            // Do not assign a new value, and return false to flag that 
            // we don't want the current value to be changed.
            newValue = null;
            return false;
          }

					DTE service = (DTE)this.GetService(typeof(DTE));
					
					string basePath = GetBasePath();
					string output = Helpers.GetApplicationConfigValueEvaluated(service, ConfigValue, DefaultValue, basePath);

					newValue = output;
          return true;
        }

        #region IAttributesConfigurable Members

        void IAttributesConfigurable.Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if(attributes["ConfigValue"] != null)
            {
                ConfigValue = attributes["ConfigValue"];
            }
            if (attributes["DefaultValue"] != null)
            {
              DefaultValue = attributes["DefaultValue"];
            }
        }

        #endregion
    }
}

