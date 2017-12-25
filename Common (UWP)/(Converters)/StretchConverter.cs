using Imagin.Common.Linq;
using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Imagin.Common.Data
{
    /// <summary>
    /// Converts <see cref="Imagin.Common.Media.Stretch"/> to <see cref="Windows.UI.Xaml.Media.Stretch"/>.
    /// </summary>
    public class StretchConverter : IValueConverter
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
            if (value is Imagin.Common.Media.Stretch)
                return value.ToString().ParseEnum<Stretch>();

            return Stretch.None;
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
            if (value is Stretch)
                return value.ToString().ParseEnum<Imagin.Common.Media.Stretch>();

            return Imagin.Common.Media.Stretch.None;
        }
    }
}
