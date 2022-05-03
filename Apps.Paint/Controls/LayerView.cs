using System.Windows;
using System.Windows.Controls;

namespace Imagin.Apps.Paint
{
    public class LayerView : Control
    {
        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register(nameof(Layer), typeof(object), typeof(LayerView), new FrameworkPropertyMetadata(null));
        public object Layer
        {
            get => GetValue(LayerProperty);
            set => SetValue(LayerProperty, value);
        }

        public LayerView() : base() { }
    }
}