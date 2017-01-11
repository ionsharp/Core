using Imagin.Common.Extensions;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(Color), typeof(string))]
    public class ColorToHexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color Color = value.As<Color>();
            if (Color != null) return Color.ToHexWithAlpha();
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value.ToString() != string.Empty)
            {
                try
                {
                    return value.ToString().ToSolidColorBrush().Color;
                }
                catch
                {
                }
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
