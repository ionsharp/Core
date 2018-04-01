using System;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    /// <summary>
    /// 
    /// </summary>
    public class Converter<TSource, TResult> : IValueConverter
    {
        readonly Func<TResult, TSource> _back;

        readonly Func<TSource, TResult> _to;

        Converter(Func<TSource, TResult> to, Func<TResult, TSource> back)
        {
            _to = to;
            _back = back;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TSource)
                return _to((TSource)value);

            return default(TResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TResult)
                return _back((TResult)value);

            return default(TSource);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="back"></param>
        /// <returns></returns>
        public static Converter<TSource, TResult> New(Func<TSource, TResult> to, Func<TResult, TSource> back)
        {
            return new Converter<TSource, TResult>(to, back);
        }
    }
}