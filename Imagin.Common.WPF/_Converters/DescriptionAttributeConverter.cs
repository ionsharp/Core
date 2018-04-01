using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(Enum), typeof(string))]
    public class DescriptionAttributeConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object Value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = string.Empty;

            var text = Value?.ToString();
            if (Value is Enum)
                result = Value.As<Enum>().GetAttribute<DescriptionAttribute>()?.Description ?? Value.As<Enum>().GetAttribute<System.ComponentModel.DescriptionAttribute>()?.Description;

            if (result.IsNullOrEmpty())
                return Value;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
