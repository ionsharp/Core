using Imagin.Common.Numbers;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Imagin.Common.Converters
{
    public class OneTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string actualValue)
            {
                if (actualValue.Length > 0)
                {
                    if (actualValue[actualValue.Length - 1] == '%')
                    {
                        actualValue = actualValue.Substring(0, actualValue.Length - 1);
                        return (One)(double.Parse(actualValue) / 100d);
                    }
                    return (One)double.Parse(actualValue);
                }
            }
            throw new InvalidOperationException();
        }
    }
}