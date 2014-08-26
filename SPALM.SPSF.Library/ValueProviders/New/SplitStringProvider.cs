using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using System.ComponentModel.Design;
using Microsoft.Practices.Common;

namespace SPALM.SPSF.Library.ValueProviders
{
    //removes all special characters from a string so that the returned string could be used e.g. as a resource key
    [ServiceDependency(typeof(DTE))]
    public class SplitStringProvider : ValueProvider, IAttributesConfigurable
    {
        private string RecipeArgument = "";
        private string SplitChar = "-";
        private string Index = "0";

        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            newValue = null;
            return this.Evaluate(out newValue);
        }

        public override bool OnBeforeActions(object currentValue, out object newValue)
        {
            newValue = null;
            return this.Evaluate(out newValue);
        }

        private bool Evaluate(out object newValue)
        {   
            try
            {
                string returnvalue = "";

                IDictionaryService dictionaryService = (IDictionaryService)ServiceHelper.GetService(this, typeof(IDictionaryService));
                string argumentvalue = dictionaryService.GetValue(RecipeArgument).ToString();
                if (argumentvalue != null)
                {
                    returnvalue = argumentvalue;

                    try
                    {
                        if (!string.IsNullOrEmpty(SplitChar))
                        {
                            char[] splitCharacters = SplitChar.ToCharArray();
                            string[] splittedString = argumentvalue.Split(splitCharacters);

                            if (!string.IsNullOrEmpty(Index))
                            {
                                int index = Int32.Parse(Index);
                                returnvalue = splittedString[index];
                            }
                        }
                    }
                    catch { }
                }

                newValue = returnvalue;
                return true;
            }
            catch (Exception)
            {
            }

            newValue = null;
            return false;
        }

        #region IAttributesConfigurable Members

        public void Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes["RecipeArgument"] != null)
            {
                RecipeArgument = attributes["RecipeArgument"];
            }
            if (attributes["SplitChar"] != null)
            {
                SplitChar = attributes["SplitChar"];
            }
            if (attributes["Index"] != null)
            {
                Index = attributes["Index"];
            }
        }

        #endregion
    }
}
