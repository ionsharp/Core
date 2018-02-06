using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(GridLength), typeof(GridLength))]
    public class TreeViewColumnGridLengthConverter : TreeViewColumnDoubleConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new GridLength(base.Convert(((GridLength)value).Value, targetType, parameter, culture).As<double>(), GridUnitType.Pixel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new GridLength(base.ConvertBack(((GridLength)value).Value, targetType, parameter, culture).As<double>(), GridUnitType.Pixel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Offset"></param>
        public TreeViewColumnGridLengthConverter(double Offset) : base(Offset)
        {
        }
    }
}
