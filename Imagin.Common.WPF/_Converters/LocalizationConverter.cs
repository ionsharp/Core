using Imagin.Common.Globalization;
using System;
using System.Globalization;
using System.Windows.Data;
using Imagin.Common.Linq;

namespace Imagin.Common.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(string), typeof(string))]
    public class LocalizationConverter : IValueConverter
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
            var result = string.Empty;

            if (value is object)
            {
                var key = value.ToString();
                result = Localizer.GetValue<string>(result);

                if (result.IsNullOrEmpty())
                    result = key;
            }

            return result;
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
            throw new NotSupportedException();
        }
    }
}
