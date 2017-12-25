using System;

namespace Imagin.Common.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class StringToVisibilityConverter : BooleanToVisibilityConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public override object Convert(object value, Type targetType, object parameter, string language)
        {
            return base.Convert(value?.ToString().Length > 0, targetType, parameter, language);
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public override object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
