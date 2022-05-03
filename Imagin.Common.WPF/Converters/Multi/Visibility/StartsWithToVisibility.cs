using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(Visibility))]
    public class StartsWithToVisibilityMultiConverter : MultiConverter<Visibility>
    {
        public static StartsWithToVisibilityMultiConverter Default { get; private set; } = new StartsWithToVisibilityMultiConverter();
        StartsWithToVisibilityMultiConverter() { }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length == 2)
            {

                var input = values[0].ToString();
                var query = values[1].ToString();

                if (query.NullOrEmpty())
                {
                    return Visibility.Visible;
                }
                
                if (input.StartsWith(query))
                {
                    return Visibility.Visible;
                }

                return Visibility.Collapsed;
            }
            return null;
        }
    }
}