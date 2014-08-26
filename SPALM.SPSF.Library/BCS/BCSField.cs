using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPALM.SPSF.Library
{
    /// <summary>
    /// Field of a BCS Entity
    /// </summary>
    public class BCSField
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public bool IsKey { get; set; }
        public bool IsIdentity { get; set; }

        public string OleDbType { get; set; }
        public Type DataType { get; set; }
        public string DbType { get; set; }
        public string DataTypeSize { get; set; }
        public bool IsNullAllowed { get; set; }
        public bool ShowInPicker { get; set; }
        public bool IsRequired { get; set; }
        public bool IsReadOnly { get; set; }

        public string ReferencedEntity { get; set; }
        public string ReferencedField { get; set; }

        public string OfficeProperty { get; set; }

        public string AssociationName { get; set; }
        public string AssociationDisplayName { get; set; }
        

    }
}
