using Imagin.Common.Geometry;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(object[]), typeof(double))]
    public class MathMultiValueConverter : IMultiValueConverter
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
            if (values?.Length == 3)
            {
                var valuea = (double)values[0];
                var operation = (NumeralOperation)values[1];
                var valueb = (double)values[2];

                switch (operation)
                {
                    case NumeralOperation.Add:
                        return valuea + valueb;
                    case NumeralOperation.Divide:
                        return valuea / valueb;
                    case NumeralOperation.Multiply:
                        return valuea * valueb;
                    case NumeralOperation.Subtract:
                        return valuea - valueb;
                }
            }

            return default(double);
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
            throw new NotSupportedException();
        }
    }
}
