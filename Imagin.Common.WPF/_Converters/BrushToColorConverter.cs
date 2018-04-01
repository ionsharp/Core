using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(Brush), typeof(Color))]
    public class BrushToColorConverter : IValueConverter
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
            var b = (Brush)value;
            if (b is SolidColorBrush) return (b as SolidColorBrush).Color;
            return default(Color);
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
            var c = (Color)value;
            if (c != default(Color)) return new SolidColorBrush(c);
            return default(Brush);
        }
    }
}

