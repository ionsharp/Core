using System;
using System.Collections.ObjectModel;
using System.Reflection;
using Windows.UI.Xaml.Data;
using Imagin.Common.Linq;

namespace Imagin.Common.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class EnumToCollectionConverter : IValueConverter
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
            if (value == null) return null;

            Type EnumType;
            if (value is Enum)
            {
                EnumType = value.GetType();
            }
            else if (value is Type)
            {
                EnumType = value as Type;
            }
            else return null;

            var Result = new ObservableCollection<object>();
            if (EnumType.GetTypeInfo().IsEnum)
            {
                foreach (var i in Enum.GetValues(EnumType))
                {
                    var j = i as Enum;
                    var Attribute = j.GetAttribute<BrowsableAttribute>();
                    if (Attribute == null || Attribute.Browsable)
                        Result.Add(j);
                }
            }

            return Result;
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
            throw new NotImplementedException();
        }
    }
}
