using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(string), typeof(string))]
    public class FileNameConverter : IValueConverter
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
            if (value is string)
            {
                var Value = value.ToString();
                var Parameter = parameter?.ToString()?.ToLower();

                var With = true;
                switch (Parameter)
                {
                    case "":
                    case "with":
                    case null:
                        With = true;
                        break;
                    case "without":
                        With = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return Value.GetFileName(!With);
            }
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
