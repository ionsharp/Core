using Imagin.Common.Media;
using System;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Imagin.Common.Data
{
    public class AccentColorToSolidColorBrushConverter : AccentColorToColorConverter
    {
        public override object Convert(object value, Type targetType, object parameter, string language)
        {
            var Result = (Color)base.Convert(value, targetType, parameter, language);
            return new SolidColorBrush(Result);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var Value = (SolidColorBrush)value;
            return (AccentColor)base.ConvertBack(Value.Color, targetType, parameter, language);
        }
    }
}
