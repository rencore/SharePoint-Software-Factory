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
    public class ListTemplateCategoryConverter : TypeConverter
    {
        static string[] validValues = new string[] {
            "Custom Lists", "Libraries", "Communications", "Tracking" };

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

    public class ListTemplateBaseTypeConverter : StringConverter
    {
        static NameValueItem[] validValues = new NameValueItem[0];

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<NameValueItem> validValues = new List<NameValueItem>();

            NameValueItem item0 = new NameValueItem();
            item0.Name = "Custom List";
            item0.Value = "0";
            item0.ItemType = "BaseType";
            item0.Description = "Custom List";
            validValues.Add(item0);

            NameValueItem item1 = new NameValueItem();
            item1.Name = "Document Library";
            item1.Value = "1";
            item1.ItemType = "BaseType";
            item1.Description = "Document Library";
            validValues.Add(item1);

            NameValueItem item4 = new NameValueItem();
            item4.Name = "Surveys";
            item4.Value = "4";
            item4.ItemType = "BaseType";
            item4.Description = "Surveys";
            validValues.Add(item4);

            NameValueItem item5 = new NameValueItem();
            item5.Name = "Issues List";
            item5.Value = "5";
            item5.ItemType = "BaseType";
            item5.Description = "Issues List";
            validValues.Add(item5);

            return new StandardValuesCollection(validValues.ToArray());
        }

        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            if (value != null)
            {
                return true;
            }
            return false;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(NameValueItem))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(System.String))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (context.PropertyDescriptor.PropertyType == typeof(System.String))
            {
                return value.ToString();
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(System.String))
            {
                return value.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }

    public class ListTemplateSecurityBitsConverterRead : TypeConverter
    {
        static string[] validValues = new string[] {
            "1 - Users can read all items", "2 - Users can read only their own items" };

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

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return base.ConvertFrom(context, culture, value);
        }
    }

    public class ListTemplateSecurityBitsConverterEdit : TypeConverter
    {
        static string[] validValues = new string[] {
            "1 - Users can edit all items", "2 - Users can edit only their own items", "4 Users cannot edit items" };

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

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return base.ConvertFrom(context, culture, value);
        }
    }
}
