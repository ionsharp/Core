using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(double), typeof(double))]
    public class TreeViewColumnDoubleConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        public double Offset
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value - (double)parameter * this.Offset;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Parameter = (double)parameter;
            return (double)value + ((Parameter <= 0 ? 1 : Parameter) / this.Offset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Offset"></param>
        public TreeViewColumnDoubleConverter(double Offset) : base()
        {
            this.Offset = Offset;
        }
    }
}
