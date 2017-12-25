using Imagin.Common.Collections;
using Imagin.Common.Linq;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Data
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(Enum), typeof(ObservableCollection<Enum>))]
    public class EnumToCollectionConverter : IValueConverter
    {
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
            var result = new EnumCollection();
            if (value != null)
            {
                var enumType = default(Type);
                if (value is Enum)
                {
                    enumType = value.GetType();
                }
                else if (value.Is<Type>())
                {
                    enumType = value.As<Type>();
                }
                else return null;

                if (enumType.IsEnum)
                {
                    foreach (var i in Enum.GetValues(enumType))
                    {
                        var field = i as Enum;
                        result.Add(field);
                        continue;

                        var attribute = field.GetAttribute<BrowsableAttribute>();
                        if (attribute == null || attribute.Browsable)
                            result.Add(i as Enum);
                    }
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
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
