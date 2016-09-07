using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using Imagin.Common.Extensions;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(string), typeof(Visibility))]
    public class EmptyStringToVisibilityConverter : IValueConverter
    {
        enum Parameter
        {
            Normal, Inverted
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Parameter Parameter = parameter == null ? Parameter.Normal : (Parameter)Enum.Parse(typeof(Parameter), parameter.ToString());
            bool Result = string.IsNullOrEmpty(value.As<string>());
            return Parameter == Parameter.Normal ? (!Result).ToVisibility() : Result.ToVisibility();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
