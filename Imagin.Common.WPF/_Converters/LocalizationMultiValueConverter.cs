using Imagin.Common.Globalization;
using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(object[]), typeof(string))]
    public class LocalizationMultiValueConverter : IMultiValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var Result = parameter.ToString();

            for (int i = 1, Count = values.Length; i < Count; i++)
                values[i] = Localizer.GetValue<string>(values[i].ToString());

            try
            {
                return Result?.F(values);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
