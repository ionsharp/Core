using Imagin.Common.Linq;
using System;

namespace Imagin.Common.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class ValueNotEqualToParameterConverter : ValueEqualsParameterConverter
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
            return !base.Convert(value, targetType, parameter, language).To<bool>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public override object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return !base.ConvertBack(value, targetType, parameter, language).To<bool>();
        }
    }
}
