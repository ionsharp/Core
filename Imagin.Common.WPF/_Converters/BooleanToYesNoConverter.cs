using Imagin.Common.Globalization;
using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToYesNoConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return Localizer.GetValue<string>("Imagin.Common.Wpf:Main:{0}".F((bool)value ? "Yes" : "No"));

            return string.Empty;
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

