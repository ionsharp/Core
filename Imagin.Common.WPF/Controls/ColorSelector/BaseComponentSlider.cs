using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class BaseComponentSlider : ColorSelector
    {
        public static readonly DependencyProperty ArrowForegroundProperty = DependencyProperty.Register(nameof(ArrowForeground), typeof(Brush), typeof(BaseComponentSlider), new FrameworkPropertyMetadata(Brushes.Black));
        public Brush ArrowForeground
        {
            get => (Brush)GetValue(ArrowForegroundProperty);
            set => SetValue(ArrowForegroundProperty, value);
        }

        static readonly DependencyPropertyKey ArrowPositionKey = DependencyProperty.RegisterReadOnly(nameof(ArrowPosition), typeof(Point2D), typeof(BaseComponentSlider), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty ArrowPositionProperty = ArrowPositionKey.DependencyProperty;
        public Point2D ArrowPosition
        {
            get => (Point2D)GetValue(ArrowPositionProperty);
            set => SetValue(ArrowPositionKey, value);
        }

        public static readonly DependencyProperty ArrowTemplateProperty = DependencyProperty.Register(nameof(ArrowTemplate), typeof(DataTemplate), typeof(BaseComponentSlider), new FrameworkPropertyMetadata(null));
        public DataTemplate ArrowTemplate
        {
            get => (DataTemplate)GetValue(ArrowTemplateProperty);
            set => SetValue(ArrowTemplateProperty, value);
        }

        public BaseComponentSlider() : base()
        {
            ArrowPosition = new Point2D(0, -8);
        }
    }
}