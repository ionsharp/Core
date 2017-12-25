using System;
using Windows.UI.Xaml.Data;

namespace Imagin.Common.Data
{
    public class UpperCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value?.ToString().ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
