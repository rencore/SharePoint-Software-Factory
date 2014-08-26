using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPALM.SPSF.Library
{
    public class BCSAssociation
    {
        public string FieldInCurrentEntity { get; set; }
        public string ReferencedEntityName { get; set; }
        public string RefrencedEntityField { get; set; }

        public BCSAssociation(string fieldInCurrentEntity, string referencedEntityName, string refrencedEntityField)
        {
            // TODO: Complete member initialization
            this.FieldInCurrentEntity = fieldInCurrentEntity;
            this.ReferencedEntityName = referencedEntityName;
            this.RefrencedEntityField = refrencedEntityField;
        }
    }
}
