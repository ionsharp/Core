using Imagin.Common.Linq;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Imagin.Common.Data
{
    public class NullImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value?.ToString().IsEmpty() == false ? value : DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
