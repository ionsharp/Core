using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(Orientation), typeof(bool))]
    public class OrientationToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Orientation Orientation = (Orientation)value;
            Orientation Compare = (Orientation)Enum.Parse(typeof(Orientation), (string)parameter);
            return Orientation == Compare ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool Bool = (bool)value;
            Orientation Compare = (Orientation)Enum.Parse(typeof(Orientation), (string)parameter);
            return Bool ? Compare : DependencyProperty.UnsetValue;
        }
    }
}
