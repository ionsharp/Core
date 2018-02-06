using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;

namespace Imagin.Common.Data.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(Orientation), typeof(Visibility))]
    public class OrientationToVisibilityConverter : IValueConverter
    {
        enum Parameter
        {
            Inverted, 
            Normal
        }

        Visibility GetVisibility(Orientation Orientation)
        {
            return Orientation == Orientation.Horizontal ? Visibility.Visible : Visibility.Collapsed;
        }

        Visibility GetOpposite(Visibility Visibility)
        {
            return Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

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
            Orientation Orientation = (Orientation)value;
            if (parameter == null) return this.GetVisibility(Orientation);
            Parameter Parameter = (Parameter)Enum.Parse(typeof(Parameter), (string)parameter);
            return Parameter == Parameter.Inverted ? this.GetOpposite(this.GetVisibility(Orientation)) : this.GetVisibility(Orientation);
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
