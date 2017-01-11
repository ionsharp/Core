using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Imagin.Common.Extensions;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        enum Mode
        {
            Normal,
            Inverted
        }

        Visibility GetInverse(Visibility Visibility)
        {
            return Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value.IsAny(typeof(bool), typeof(int)))
            {
                var Value = (bool)value;
                var Type = Mode.Normal;

                if (parameter is Visibility)
                    return Value ? Visibility.Visible : parameter;
                else if (parameter != null)
                    Type = (Mode)Enum.Parse(typeof(Mode), parameter.ToString());

                var Result = Value ? Visibility.Visible : Visibility.Collapsed;
                return Type == Mode.Inverted ? GetInverse(Result) : Result;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is Visibility)
            {
                var Value = (Visibility)value;
                var Type = Mode.Normal;

                var Result = Value == Visibility.Visible ? true : false;
                if (parameter is Visibility)
                    return Result;
                else if (parameter != null)
                    Type = (Mode)Enum.Parse(typeof(Mode), parameter.ToString());

                return Type == Mode.Normal ? Result : !Result;
            }
            return false;
        }
    }
}
