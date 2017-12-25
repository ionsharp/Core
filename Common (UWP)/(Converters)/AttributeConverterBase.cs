using System;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml.Data;

namespace Imagin.Common.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    public abstract class AttributeConverterBase<TAttribute> : IValueConverter where TAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Attribute"></param>
        /// <returns></returns>
        protected abstract object GetValue(TAttribute Attribute);

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
            if (value != null)
            {
                var Type = value.GetType();

                var i = Type.GetMember(value.ToString());
                if (i != null && i.Length >= 1)
                {
                    var a = i[0].GetCustomAttributes(typeof(TAttribute), false);
                    return GetValue((TAttribute)a.First());
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
        /// <param name="language"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
