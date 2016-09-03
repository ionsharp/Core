using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Imagin.Common.Extensions;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(Brush), typeof(string))]
    public class SolidColorBrushToHexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush s = value.As<SolidColorBrush>();
            if (s != null)
                return s.Color.R.ToString("X2") + s.Color.G.ToString("X2") + s.Color.B.ToString("X2");
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.ToString() == string.Empty)
                return DependencyProperty.UnsetValue;
            try
            {
                return (SolidColorBrush)(new BrushConverter().ConvertFrom("#" + value.ToString()));
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
