using System;
using System.Globalization;
using System.Windows.Data;
using Imagin.Common.Attributes;

namespace Imagin.Common.Data.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(Enum), typeof(string))]
    public class AbbreviationAttributeConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object Value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Value != null)
            {
                var Type = Value.GetType();
                var Info = Type.GetMember(Value.ToString());
                if (Info != null && Info.Length >= 1)
                {
                    var Attributes = Info[0].GetCustomAttributes(typeof(AbbreviationAttribute), false);
                    return ((AbbreviationAttribute)Attributes[0]).Value;
                }
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
            throw new NotImplementedException();
        }
    }
}
