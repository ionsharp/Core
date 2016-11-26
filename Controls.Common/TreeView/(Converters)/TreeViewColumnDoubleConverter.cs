using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Controls.Common.Converters
{
    [ValueConversion(typeof(double), typeof(double))]
    public class TreeViewColumnDoubleConverter : IValueConverter
    {
        public double Offset
        {
            get; private set;
        }

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value - (double)parameter * this.Offset;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Parameter = (double)parameter;
            return (double)value + ((Parameter <= 0 ? 1 : Parameter) / this.Offset);
        }

        public TreeViewColumnDoubleConverter(double Offset) : base()
        {
            this.Offset = Offset;
        }
    }
}
