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
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : IValueConverter
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
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Result = default(Visibility);

            var Value = false;
            if (value is bool)
            {
                Value = (bool)value;
            }
            else if (value is bool?)
                Value = value.To<bool?>().Value;

            Result = Value.ToVisibility();

            var Parameter = parameter?.ToString();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Result = false;

            var Value = default(Visibility);
            if (value is Visibility)
                Value = (Visibility)value;

            Result = Value.ToBoolean();

            var Parameter = parameter?.ToString();
            if (Parameter != null)
            {
                if (Parameter == "Normal")
                {
                    //Do nothing!
                }
                else if (Parameter == "Inverted")
                {
                    Result = !Result;
                }
                else AssertParameter();
            }

            return Result;
        }
    }
}
