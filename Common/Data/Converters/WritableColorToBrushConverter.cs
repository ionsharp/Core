using Imagin.Common.Extensions;
using Imagin.Common.Serialization;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common.Data.Converters
{
    [ValueConversion(typeof(WritableColor), typeof(SolidColorBrush))]
    public class WritableColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.As<WritableColor>().Hex.ToSolidColorBrush();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new WritableColor(value.As<SolidColorBrush>().Color.ToHexWithAlpha());
        }
    }
}
