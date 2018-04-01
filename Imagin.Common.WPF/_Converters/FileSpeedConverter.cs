using Imagin.Common.Data;
using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(double), typeof(string))]
    public class FileSpeedConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var Value = 0L;
            if (value is string)
            {
                Value = value.ToString().ToInt64();
            }
            else if (value is double)
            {
                Value = value.As<double>().ToInt64();
            }

            return Value == 0 ? string.Empty : "{0}/s".F(Value.ToFileSize(FileSizeFormat.BinaryUsingSI));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
