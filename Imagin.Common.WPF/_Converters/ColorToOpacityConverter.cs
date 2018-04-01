using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(Color), typeof(double))]
    public class ColorToOpacityConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => value is Color ? ((Color)value).A.ToDouble() / 255.0 : 1.0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
