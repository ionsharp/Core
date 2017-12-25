using Imagin.Common.Data.Converters;
using Imagin.Common.Debug;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Data
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(Result), typeof(Visibility))]
    public class ResultToVisibilityConverter : BooleanToVisibilityConverter
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
            var Result = false;
            if (value is Result)
                Result = (Result)value is Success;

            return base.Convert(Result, targetType, parameter, culture);
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
            throw new NotSupportedException();
        }
    }
}
