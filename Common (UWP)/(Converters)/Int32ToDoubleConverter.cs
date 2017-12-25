using Imagin.Common.Linq;
using System;
using Windows.UI.Xaml.Data;

namespace Imagin.Common.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class Int32ToDoubleConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is int ? value.To<int>().ToDouble() : default(double);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is double ? value.To<double>().ToInt32() : default(int);
        }
    }
}
