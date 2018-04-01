using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(Color), typeof(Brush))]
    public class ColorToSolidColorBrushConverter : IValueConverter
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
            Color Color = (Color)value;
            if (Color != default(Color))
                return new SolidColorBrush(Color);
            return default(Brush);
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
            Brush Brush = (Brush)value;
            if (Brush is SolidColorBrush)
                return (Brush as SolidColorBrush).Color;
            return default(Color);
        }
    }
}

