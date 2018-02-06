using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Data.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(string), typeof(Visibility))]
    public class StringToVisibilityConverter : StringToBooleanConverter
    {
        void AssertParameter()
        {
            throw new ArgumentException("A value of 'Normal' or 'Inverted' is expected.");
        }

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
            var Value = (bool)base.Convert(value, targetType, parameter, culture);
            var Parameter = (string)parameter;

            var Result = Value.ToVisibility();

            if (Parameter != null)
            {
                if (Parameter == "Normal")
                {
                    //Do nothing!
                }
                else if (Parameter == "Inverted")
                {
                    Result = Result.Invert();
                }
                else AssertParameter();
            }

            return Result;
        }
    }
}
