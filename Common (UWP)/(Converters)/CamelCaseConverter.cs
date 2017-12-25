using Imagin.Common.Linq;
using System;
using Windows.UI.Xaml.Data;

namespace Imagin.Common.Data
{
    public class CamelCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                var Value = value.ToString();

                if (!value.IsNot<string>() || Value.IsEmpty())
                    return Value;

                return Value.SplitCamelCase();
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
