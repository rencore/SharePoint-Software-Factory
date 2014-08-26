using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.Common;

namespace SPALM.SPSF.Library.ValueProviders
{
    public class DefaultNameValueItemProvider : ValueProvider, IAttributesConfigurable
    {
        #region IAttributesConfigurable Members

        void IAttributesConfigurable.Configure(System.Collections.Specialized.StringDictionary attributes)
        {
            if (attributes["Name"] != null)
            {
                Name = attributes["Name"];
            }
            if (attributes["Value"] != null)
            {
                Value = attributes["Value"];
            }
						if (attributes["Group"] != null)
						{
							Group = attributes["Group"];
						}
            if (attributes["ItemType"] != null)
            {
                ItemType = attributes["ItemType"];
            }
        }

        #endregion

        private string _Name = "";
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _Value = "";
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

				private string _Group = "";
				public string Group
				{
					get { return _Group; }
					set { _Group = value; }
				}

        private string _ItemType = "";
        public string ItemType
        {
            get { return _ItemType; }
            set { _ItemType = value; }
        }

        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
            if (currentValue != null)
            {                
                newValue = null;
                return false;
            }


						NameValueItem newitem = new NameValueItem(ItemType, Name, Value);
						newitem.Group = Group;
						newValue = newitem;
            return true;
            
        }
    }
}
