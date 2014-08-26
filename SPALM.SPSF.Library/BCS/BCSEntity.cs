using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPALM.SPSF.Library
{
    public class BCSEntity
    {
        public List<BCSField> Fields;

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Schema { get; set; }  //e.g. dbo
        public string TitleField { get; set; }
        public string OfficeItemType { get; set; }

        public bool CreateReadOperation { get; set; }
        public bool CreateUpdateOperation { get; set; }
        public bool CreateCreateOperation { get; set; }
        public bool CreateDeleteOperation { get; set; }

        public BCSEntity()
        {
            Fields = new List<BCSField>();
       
            CreateReadOperation = true;
            CreateUpdateOperation = true;
            CreateCreateOperation = true;
            CreateDeleteOperation = true;
        }

        public string ReadItemOperationName
        {
            get
            {
                return "Read" + Name + "Item";
            }
        }

        public string ReadListOperationName
        {
            get
            {
                return "Read" + Name + "List";
            }
        }

        public string UpdateOperationName
        {
            get
            {
                return "Update" + Name;
            }
        }

        public string DeleteOperationName
        {
            get
            {
                return "Delete" + Name;
            }
        }

        public string CreateOperationName
        {
            get
            {
                return "Create" + Name;
            }
        }
    }
}
