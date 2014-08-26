using System;
using System.ComponentModel;

namespace SPALM.SPSF.Library.Converters
{
	/// <summary>
	/// Checks a given string if it is a valid version
	/// </summary>
	public class VersionConverter : StringConverter
	{
		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			if (value is string)
			{
				try
				{
					Version a = new Version(value as string); //throws Exception if not valid
					return true;
				}
				catch (Exception)
				{
				}
			}
			return false;
		}
	}
}
