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
  public class SPExportIncludeSecurityEnum : TypeConverter
  {
    static string[] validValues = new string[] {
            "All", "None", "WSSOnly" };

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

  public class SPExportIncludeVersionsEnum : TypeConverter
  {
    static string[] validValues = new string[] {
            "All", "CurrentVersion", "LastMajor", "LastMajorAndMinor"  };

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

  public class SPImportUpdateVersionsEnum : TypeConverter
  {
    static string[] validValues = new string[] {
            "AddAsNewVersions", "OverwriteFileAndVersions", "IgnoreFileInDestination"  };

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

  public class SPExportMethod : TypeConverter
  {
    static string[] validValues = new string[] {
            "ExportAll", "ExportChanges"  };

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
