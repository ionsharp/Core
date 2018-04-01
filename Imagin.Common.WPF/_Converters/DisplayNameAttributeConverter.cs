using Imagin.Common.Linq;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(object), typeof(string))]
    public class DisplayNameAttributeConverter : IValueConverter
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
            if (Value is Enum)
                result = Value.As<Enum>().GetAttribute<DisplayNameAttribute>()?.DisplayName ?? Value.As<Enum>().GetAttribute<System.ComponentModel.DisplayNameAttribute>()?.DisplayName;

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
