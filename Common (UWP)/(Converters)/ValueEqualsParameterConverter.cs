using System;
using Windows.UI.Xaml.Data;

namespace Imagin.Common.Data
{
    public class ValueEqualsParameterConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public virtual object Convert(object value, Type targetType, object parameter, string language)
        {
            return value == parameter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public virtual object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return parameter;
        }
    }
}
