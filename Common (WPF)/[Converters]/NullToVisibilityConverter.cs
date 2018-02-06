using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class NullToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        public enum Parameter
        {
            /// <summary>
            /// 
            /// </summary>
            Normal,
            /// <summary>
            /// 
            /// </summary>
            Inverted
        }
        Visibility Evaluate(Visibility Visibility, Parameter Parameter)
        {
            return Parameter == Parameter.Normal ? Visibility : (Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);
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
            Parameter Parameter = parameter == null ? Parameter.Normal : (Parameter)Enum.Parse(typeof(Parameter), (string)parameter);
            return this.Evaluate(value == null ? Visibility.Collapsed : Visibility.Visible, Parameter);
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
