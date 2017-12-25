using Imagin.Common.Linq;
using System;
using Windows.UI.Xaml.Data;

namespace Imagin.Common.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public abstract class EnumToIntConverter<TEnum> : IValueConverter where TEnum : struct, IFormattable, IComparable, IConvertible
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
            if (value is TEnum)
            {
                var i = value.To<TEnum>().To<int>();
                if (Enum.IsDefined(typeof(TEnum), i))
                    return i;
            }
            return 0;
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
            if (value is int)
            {
                var i = value.To<int>();
                if (Enum.IsDefined(typeof(TEnum), i))
                    return i.To<TEnum>();
            }
            return default(TEnum);
        }
    }
}
