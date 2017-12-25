using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;

namespace Imagin.Common.Data
{
    public class NullToVisibilityConverter : IValueConverter
    {
        enum Parameter
        {
            Normal,
            Inverted
        }

        Visibility Evaluate(Visibility Visibility, Parameter Parameter)
        {
            return Parameter == Parameter.Normal ? Visibility : (Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Parameter Parameter = parameter == null ? Parameter.Normal : (Parameter)Enum.Parse(typeof(Parameter), (string)parameter);
            return Evaluate(value == null ? Visibility.Collapsed : Visibility.Visible, Parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
