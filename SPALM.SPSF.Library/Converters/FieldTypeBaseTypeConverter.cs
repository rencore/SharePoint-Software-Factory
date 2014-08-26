using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Drawing.Design;
using Microsoft.Practices.ComponentModel;
using EnvDTE;

namespace SPALM.SPSF.Library.Converters
{
  public class FieldTypeBaseTypeConverter : TypeConverter
  {
    static string[] validValues = new string[] {
            "AllDayEvent",
            "Attachments",
            "Boolean",
            "Calculated",
            "Choice",
            "Computed",
            "ContentTypeId",
            "Counter",
            "CrossProjectLink",
            "Currency",
            "DateTime",
            "Error",
            "File",
            "GridChoice",
            "Guid",
            "Integer",
            "Lookup",
            "MaxItems",
            "ModStat",
            "MultiChoice",
            "Note",
            "Number",
            "PageSeparator",
            "Recurrence",
            "Text",
            "ThreadIndex",
            "Threading",
            "URL",
            "User",
            "WorkflowEventType",
            "WorkflowStatus" };

    public override bool IsValid(ITypeDescriptorContext context, object value)
    {
      return true;
    }

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
      return true; //no other values allowed
    }

    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
      return true;
    }

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
      return new StandardValuesCollection(validValues);
    }
  }

}
