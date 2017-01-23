using System;
using System.Globalization;
using System.Windows.Data;
using Imagin.Common.Extensions;

namespace Imagin.Gadgets
{
    [ValueConversion(typeof(string), typeof(string))]
    public class SearchProviderToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var Result = "pack://application:,,,/Imagin.Gadgets;component/Images/{0}.png";
            switch (value.ToString())
            {
                case @"https://www.google.com/#q=":
                    return Result.F("Google");
                case @"http://www.bing.com/search?q=":
                    return Result.F("Bing");
                case @"https://search.yahoo.com/search;?p=":
                    return Result.F("Yahoo");
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
