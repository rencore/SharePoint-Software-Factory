using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPALM.SPSF.Library;

namespace SharePointSoftwareFactory.Tests
{
    public class BCSBaseTest : BaseFarmFeatureRecipeRunner
    {
        public BCSBaseTest(Microsoft.Practices.RecipeFramework.GuidancePackage guidancePackage, string recipeName, BaseTest parentTest) :
            base(guidancePackage, recipeName, parentTest)
		{
		}

        public override Dictionary<string, object> GetInitialArguments()
        {
            Dictionary<string, object> arguments = new Dictionary<string, object>();

            arguments.Add("BCSDatabase", "AdventureWorks");

            //add here BCSModel
            BCSModel bcsModel = new BCSModel();

            AddEntity(bcsModel, "Entity1", "Entity 1");
            AddEntity(bcsModel, "Entity2", "Entity 2");
            AddEntity(bcsModel, "Entity3", "Entity 3");

            arguments.Add("BCSModel", bcsModel);

            return arguments;
        }

        private void AddEntity(BCSModel bcsModel, string name, string display)
        {
            BCSEntity entity = new BCSEntity();

            entity.Name = name;
            entity.DisplayName = display;
            entity.CreateCreateOperation = true;
            entity.CreateDeleteOperation = true;
            entity.CreateReadOperation = true;
            entity.CreateUpdateOperation = true;

            bcsModel.Entities.Add(entity);

            AddField(entity, "Field1", "Field 1", typeof(System.Int32), true, true, false);
            AddField(entity, "Field2", "Field 2", typeof(System.String), false, false, false);
            AddField(entity, "Field3", "Field 3", typeof(System.String), false, false, true);
        }

        private void AddField(BCSEntity entity, string name, string display, Type datatype, bool isKey, bool isIdentity, bool isRequired)
        {
            BCSField field = new BCSField();

            field.Name = name;
            field.DisplayName = display;
            field.DataType = datatype;
            field.IsIdentity = isIdentity;
            field.IsKey = isKey;
            field.IsRequired = isRequired;

            entity.Fields.Add(field);
        }        
    }
}
