using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    public class ConverterContainer : DependencyObject
    {
        public static readonly DependencyProperty DataKeyProperty = DependencyProperty.Register(nameof(DataKey), typeof(object), typeof(ConverterContainer), new FrameworkPropertyMetadata(null));
        public object DataKey
        {
            get => GetValue(DataKeyProperty);
            set => SetValue(DataKeyProperty, value);
        }

        public static readonly DependencyProperty ConverterProperty = DependencyProperty.Register(nameof(Converter), typeof(IValueConverter), typeof(ConverterContainer), new FrameworkPropertyMetadata(null));
        public IValueConverter Converter
        {
            get => (IValueConverter)GetValue(ConverterProperty);
            set => SetValue(ConverterProperty, value);
        }

        public ConverterContainer() : base() { }
    }
}