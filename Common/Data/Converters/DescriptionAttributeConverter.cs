using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(Enum), typeof(string))]
    public class DescriptionAttributeConverter : IValueConverter
    {
        public object Convert(object Value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Value != null)
            {
                var Type = Value.GetType();
                var Info = Type.GetMember(Value.ToString());
                if (Info != null && Info.Length >= 1)
                {
                    var Attributes = Info[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                    return ((DescriptionAttribute)Attributes[0]).Description;
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
