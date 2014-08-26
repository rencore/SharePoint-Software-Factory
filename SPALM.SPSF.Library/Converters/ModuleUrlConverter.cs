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
	public class ModuleUrlConverter : TypeConverter
  {
    static string[] validValues = new string[] {
            "_catalogs/masterpage", "_catalogs/masterpage/Preview Images", "_catalogs/wp", "Style Library" };

    public override bool IsValid(ITypeDescriptorContext context, object value)
    {
      return true;
    }

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
			return false;
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
