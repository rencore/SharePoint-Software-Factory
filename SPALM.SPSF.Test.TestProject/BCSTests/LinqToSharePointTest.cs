using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPALM.SPSF.Library;

namespace SharePointSoftwareFactory.Tests
{
    public class LinqToSharePointTest : BaseCustomizationRecipeRunner
    {
        public LinqToSharePointTest(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, string recipeName, BaseTest parentTest) :
            base(guidancePackage, recipeName, parentTest)
        {
        }

        public override Dictionary<string, object> GetInitialArguments()
        {
            Dictionary<string, object> arguments = new Dictionary<string, object>();

            List<NameValueItem> lists = new List<NameValueItem>();

            NameValueItem list1 = new NameValueItem();
            list1.Name = "Calendar";
            list1.Value = "Calendar";
            lists.Add(list1);

            NameValueItem ct1 = new NameValueItem();
            ct1.Name = "Event";
            ct1.Value = "Event";
            list1.Childs.Add(ct1);

            NameValueItem field1 = new NameValueItem();
            field1.Name = "Title";
            field1.Value = "Title";
            ct1.Childs.Add(field1);
            NameValueItem field2 = new NameValueItem();
            field2.Name = "Location";
            field2.Value = "Location";
            ct1.Childs.Add(field2);
            NameValueItem field3 = new NameValueItem();
            field3.Name = "Description";
            field3.Value = "Description";
            ct1.Childs.Add(field3);

            NameValueItem list2 = new NameValueItem();
            list2.Name = "Tasks";
            list2.Value = "Tasks";
            lists.Add(list2);

            NameValueItem ct1a = new NameValueItem();
            ct1a.Name = "Event";
            ct1a.Value = "Event";
            list2.Childs.Add(ct1a);

            NameValueItem field1a = new NameValueItem();
            field1a.Name = "Title";
            field1a.Value = "Title";
            ct1a.Childs.Add(field1a);
            NameValueItem field2a = new NameValueItem();
            field2a.Name = "Status";
            field2a.Value = "Status";
            ct1a.Childs.Add(field2a);
            NameValueItem field3a = new NameValueItem();
            field3a.Name = "Body";
            field3a.Value = "Body";
            ct1a.Childs.Add(field3a);

            arguments.Add("LINQIncludedLists", lists);

            return arguments;
        }
    }
}