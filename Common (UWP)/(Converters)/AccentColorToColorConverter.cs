using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace Imagin.Common.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class AccentColorToColorConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public virtual object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is AccentColor)
                return value.To<AccentColor>().ToColor();

            return default(Color);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public virtual object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Color)
                return value.To<Color>().ToAccentColor();

            throw new InvalidCastException("Value must be of type Windows.UI.Color.");
        }
    }
}
