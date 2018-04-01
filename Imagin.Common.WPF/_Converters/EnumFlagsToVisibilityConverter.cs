using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(object[]), typeof(Visibility))]
    public class EnumFlagsToVisibilityConverter : IMultiValueConverter
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
            object a = null, b = null;

            var Result = true;

            var i = 0;
            foreach (var j in values)
            {
                if (i.IsEven())
                {
                    a = j;
                }
                else
                {
                    b = j;
                    if (a != null && b != null)
                    {
                        Result = Result && a.As<Enum>().HasFlag(b as Enum);
                        a = null;
                        b = null;
                    }
                }
                i++;
            }

            return Result.ToVisibility();
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
