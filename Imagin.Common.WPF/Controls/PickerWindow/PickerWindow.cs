using System.Windows;

namespace Imagin.Common.Controls
{
    public class PickerWindow : Window
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(object), typeof(PickerWindow), new FrameworkPropertyMetadata(null));
        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public PickerWindow() : base() { }
    }
}