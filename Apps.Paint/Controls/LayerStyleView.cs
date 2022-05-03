using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Apps.Paint
{
    public class LayerStyleView : Control
    {
        public static readonly DependencyProperty LayerStyleProperty = DependencyProperty.Register(nameof(LayerStyle), typeof(LayerStyle), typeof(LayerStyleView), new FrameworkPropertyMetadata(null));
        public LayerStyle LayerStyle
        {
            get => (LayerStyle)GetValue(LayerStyleProperty);
            set => SetValue(LayerStyleProperty, value);
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof(Image), typeof(ImageSource), typeof(LayerStyleView), new FrameworkPropertyMetadata(null));
        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public LayerStyleView() : base() { }
    }
}