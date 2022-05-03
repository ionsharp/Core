using Imagin.Common.Converters;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Imagin.Common.Controls
{
    public class ImageElement : FrameworkElement
    {
        readonly ImageBrush mask;

        readonly Rectangle rectangle;

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.RegisterAttached(nameof(Foreground), typeof(Brush), typeof(ImageElement), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush, FrameworkPropertyMetadataOptions.Inherits));
        public Brush Foreground
        {
            get => (Brush)GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(ImageElement), new FrameworkPropertyMetadata(null));
        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty SourceSizeProperty = DependencyProperty.Register(nameof(SourceSize), typeof(DoubleSize), typeof(ImageElement), new FrameworkPropertyMetadata(null));
        [TypeConverter(typeof(DoubleSizeTypeConverter))]
        public DoubleSize SourceSize
        {
            get => (DoubleSize)GetValue(SourceSizeProperty);
            set => SetValue(SourceSizeProperty, value);
        }

        protected override int VisualChildrenCount => 1;

        public ImageElement() : base() 
        {
            rectangle = new();
            AddVisualChild(rectangle);
            AddLogicalChild(rectangle);

            rectangle.Bind(Rectangle.FillProperty, 
                nameof(Foreground), this);
            rectangle.Bind(Rectangle.HeightProperty, 
                $"{nameof(SourceSize)}.{nameof(DoubleSize.Height)}", this);
            rectangle.Bind(Rectangle.SnapsToDevicePixelsProperty,
                nameof(SnapsToDevicePixels), this);
            rectangle.Bind(Rectangle.WidthProperty,
                $"{nameof(SourceSize)}.{nameof(DoubleSize.Width)}", this);
            rectangle.Bind(RenderOptions.BitmapScalingModeProperty,
                new PropertyPath("(0)", RenderOptions.BitmapScalingModeProperty), this);

            mask = new();
            mask.Bind(ImageBrush.ImageSourceProperty, nameof(Source), this);

            rectangle.OpacityMask = mask;
        }

        protected override Visual GetVisualChild(int index) => rectangle;

        protected override Size ArrangeOverride(Size finalSize)
        {
            rectangle.Arrange(new Rect(new Point(0, 0), finalSize));
            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            rectangle.Measure(availableSize);
            return rectangle.DesiredSize;
        }
    }
}