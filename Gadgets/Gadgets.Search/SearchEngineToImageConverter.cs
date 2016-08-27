using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Gadgets.Search
{
    [ValueConversion(typeof(string), typeof(string))]
    public class SearchEngineToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is SearchEngineType))
                return string.Empty;
            switch ((SearchEngineType)value)
            {
                case SearchEngineType.Google:
                    return "/Images/Google.png";
                case SearchEngineType.Bing:
                    return "/Images/Bing.png";
                case SearchEngineType.Yahoo:
                    return "/Images/Yahoo.png";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
