using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(Brush), typeof(string))]
    public class SolidColorBrushToHexConverter : IValueConverter
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
            SolidColorBrush s = value.As<SolidColorBrush>();
            if (s != null) return s.Color.ToHexWithAlpha();
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
            if (value == null || value.ToString() == string.Empty)
                return DependencyProperty.UnsetValue;
            try
            {
                return value.ToString().ToSolidColorBrush();
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
