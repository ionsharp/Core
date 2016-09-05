using Imagin.Common.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class FileNameConverter : IValueConverter
    {
        enum Parameters
        {
            With, 
            Without
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !value.Is<string>())
                return string.Empty;
            string Value = value.ToString();
            if (Value == "/" || Value == @"\" || Value.EndsWith(@":\"))
                return Value;
            Parameters Type = parameter == null ? Parameters.With : ((string)parameter).ParseEnum<Parameters>();
            return Type == Parameters.With ? Value.GetFileName() : Value.GetFileNameWithoutExtension();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
