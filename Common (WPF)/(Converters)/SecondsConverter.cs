using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows.Data;
using Imagin.Common.Linq;

namespace Imagin.Common.Data.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(int), typeof(string))]
    public class SecondsConverter : IValueConverter
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
            if (value.Is<int>())
                return TimeSpan.FromSeconds(value.As<int>()).ToShortTime();
            else if (value.Is<string>())
                return TimeSpan.FromSeconds(value.ToString().ToInt32()).ToShortTime();
            else if (value is DateTime)
                return (value.As<DateTime>() - DateTime.UtcNow).ToShortTime();
            else if (value is DateTime? && value != null)
                return (value.As<DateTime?>().Value - DateTime.UtcNow).ToShortTime();

            return string.Empty;
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
