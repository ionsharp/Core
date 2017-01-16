using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Gadgets.Search
{
    [ValueConversion(typeof(string), typeof(string))]
    public class SearchProviderToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            switch (value.ToString())
            {
                case @"https://www.google.com/#q=":
                    return "/Images/Google.png";
                case @"http://www.bing.com/search?q=":
                    return "/Images/Bing.png";
                case @"https://search.yahoo.com/search;?p=":
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
