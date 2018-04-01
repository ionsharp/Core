using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(WindowType), typeof(Visibility))]
    public class WindowTypeVisibilityConverter : IValueConverter
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
            WindowType Type = (WindowType)value;
            switch (Type)
            {
                case WindowType.Window:
                    return Visibility.Visible;
                case WindowType.Tool:
                    return Visibility.Collapsed;
            }
            return null;
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
            return null;
        }
    }
}
