using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(Orientation), typeof(Orientation))]
    public class OrientationInverseConverter : IValueConverter
    {
        Orientation Convert(object value)
        {
            if (value is Orientation)
            {
                var _value = (Orientation)value;
                return _value.Invert();
            }

            return Orientation.Vertical;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => Convert(value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Convert(value);
    }
}

