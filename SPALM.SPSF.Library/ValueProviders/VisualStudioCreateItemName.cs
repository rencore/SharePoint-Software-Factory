using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using System.ComponentModel.Design;
using Microsoft.Practices.Common;
using EnvDTE;
using System.IO;
using EnvDTE80;

namespace SPALM.SPSF.Library.ValueProviders
{
    public class VisualStudioCreateItemName : ValueProvider, IAttributesConfigurable
    {
        private string _defaultValue = "";
        private string _recipeArgument = "";
        private bool _makeSafe = true;

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

            IDictionaryService dictionaryService = (IDictionaryService)this.GetService(typeof(IDictionaryService));
            try
            {
                string itemname = dictionaryService.GetValue("ItemName").ToString();
                if (itemname != "")
                {
                    if (_makeSafe)
                    {
                        itemname = Helpers.GetSaveName(itemname);
                    }

                    newValue = itemname;
                    return true;
                }
            }
            catch (Exception)
            {
            }

            if (_recipeArgument != "")
            {
                try
                {
                    string arg = dictionaryService.GetValue(_recipeArgument).ToString();
                    if (arg != "")
                    {
                        newValue = arg;
                        return true;
                    }
                }
                catch (Exception)
                {
                }
            }

            //let's try recipe name 
            try
            {
                string arg = dictionaryService.GetValue("RecipeName").ToString();
                if (arg != "")
                {
                    newValue = arg;
                    return true;
                }
            }
            catch (Exception)
            {
            }
            

            if (_defaultValue != "")
            {
                newValue = _defaultValue;
                return true;
            }

            newValue = null;
            return false;
        }

        #region IAttributesConfigurable Members

        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes["DefaultValue"] != null)
            {
                _defaultValue = attributes["DefaultValue"];
            }
            if (attributes["RecipeArgument"] != null)
            {
                _recipeArgument = attributes["RecipeArgument"];
            }
            if (attributes["MakeSafe"] != null)
            {
                try
                {
                    _makeSafe = Boolean.Parse(attributes["MakeSafe"]);
                }
                catch (Exception)
                {
                }
            }
        }

        #endregion
    }
}
