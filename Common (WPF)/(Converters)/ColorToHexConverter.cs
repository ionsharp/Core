using Imagin.Common.Linq;
using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common.Data.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(Color), typeof(string))]
    public class ColorToHexConverter : IValueConverter
    {
        void AssertParameter()
        {
            throw new ArgumentOutOfRangeException("Parameter must either be null or assigned 'WithAlpha'.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Color = value.As<Color>();
            if (Color != null)
            {
                var Result = Color.ToHex();
                if (parameter != null)
                {
                    if (parameter.ToString() == "WithAlpha")
                    {
                        return Color.ToHexWithAlpha();
                    }
                    else AssertParameter();
                }
                return Result;
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value.ToString() != string.Empty)
            {
                var WithAlpha = false;
                if (parameter != null)
                {
                    if (parameter.ToString() == "WithAlpha")
                    {
                        WithAlpha = true;
                    }
                    else AssertParameter();
                }
                var Max = WithAlpha ? 8 : 6;

                var Result = value.ToString();
                var Length = Result.Length;

                if (Length > Max)
                {
                    Result = Result.Substring(0, Max);
                }
                else if (Length == 3)
                {
                    Result = "{0}{1}{2}{3}".F(WithAlpha ? "FF" : "", new string(Result[0], 2), new string(Result[1], 2), new string(Result[2], 2));
                }
                else
                {
                    Result = "{0}{1}".F(new string('0', Max - Length), Result);
                    Result = WithAlpha ? "FF{0}".F(Result.Substring(2)) : Result;
                }

                return Result.ToSolidColorBrush().Color;
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
