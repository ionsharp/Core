using Imagin.Common.Collections;
using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(Enum), typeof(EnumCollection))]
    public class EnumToCollectionConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public EnumCollection Convert(Type type)
        {
            var result = new EnumCollection();
            if (type.IsEnum)
            {
                foreach (var i in Enum.GetValues(type))
                {
                    var field = i as Enum;

                    var attribute = field.GetAttribute<BrowsableAttribute>();
                    if (attribute == null || attribute.Browsable)
                        result.Add(i as Enum);
                }
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
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var type = default(Type);
                if (value is Enum)
                {
                    type = value.GetType();
                }
                else if (value.Is<Type>())
                {
                    type = value.As<Type>();
                }
                else return null;
                return Convert(type);
            }
            return default(object);
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
            throw new NotSupportedException();
        }
    }
}
