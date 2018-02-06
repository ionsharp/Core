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

        bool ToBoolean(object value, object parameter)
        {
            var result = false;

            var _value = default(Visibility);
            if (value is Visibility)
                _value = (Visibility)value;

            result = _value.ToBoolean();

            var p = parameter?.ToString();
            if (p != null)
            {
                if (p == "Normal")
                {
                    //Do nothing!
                }
                else if (p == "Inverted")
                {
                    result = !result;
                }
                else AssertParameter();
            }

            return result;
        }

        Visibility ToVisibility(object value, object parameter)
        {
            var result = default(Visibility);

            var _value = false;
            if (value is bool)
            {
                _value = (bool)value;
            }
            else if (value is bool?)
                _value = value.To<bool?>().Value;

            result = _value.ToVisibility();

            var p = parameter?.ToString();
            if (p != null)
            {
                if (p == "Normal")
                {
                    //Do nothing!
                }
                else if (p == "Inverted")
                {
                    result = result.Invert();
                }
                else AssertParameter();
            }

            return result;
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
            if (value is bool || value is bool?)
            {
                return ToVisibility(value, parameter);
            }
            else if (value is Visibility)
                return ToBoolean(value, parameter);

            throw new NotSupportedException();
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
            if (value is Visibility)
            {
                return ToBoolean(value, parameter);
            }
            else if (value is bool || value is bool?)
                return ToVisibility(value, parameter);

            throw new NotSupportedException();
        }
    }
}
