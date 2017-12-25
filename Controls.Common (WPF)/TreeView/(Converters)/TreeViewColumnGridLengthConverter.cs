using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Controls.Common
{
    [ValueConversion(typeof(GridLength), typeof(GridLength))]
    public class TreeViewColumnGridLengthConverter : TreeViewColumnDoubleConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new GridLength(base.Convert(((GridLength)value).Value, targetType, parameter, culture).As<double>(), GridUnitType.Pixel);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new GridLength(base.ConvertBack(((GridLength)value).Value, targetType, parameter, culture).As<double>(), GridUnitType.Pixel);
        }

        public TreeViewColumnGridLengthConverter(double Offset) : base(Offset)
        {
        }
    }
}
