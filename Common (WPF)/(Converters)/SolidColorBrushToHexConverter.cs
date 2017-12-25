using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Imagin.Common.Linq;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(Brush), typeof(string))]
    public class SolidColorBrushToHexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush s = value.As<SolidColorBrush>();
            if (s != null) return s.Color.ToHexWithAlpha();
            return string.Empty;
        }

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
