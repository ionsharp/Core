using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Controls.Common.Converters
{
    [ValueConversion(typeof(WindowType), typeof(Visibility))]
    public class WindowTypeVisibilityConverter : IValueConverter
    {
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
