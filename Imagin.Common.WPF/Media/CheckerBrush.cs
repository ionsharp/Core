using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Media
{
    public class CheckerBrush : DependencyObject
    {
        public static DrawingBrush Default => Create(DefaultBackground, DefaultForeground, DefaultSize);

        //...

        public static Brush DefaultBackground => Brushes.White;

        public static Brush DefaultForeground => Brushes.LightGray;

        public static double DefaultSize => 10;
        
        //...

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(CheckerBrush), new FrameworkPropertyMetadata(DefaultBackground, OnPropertyChanged));
        public Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        static readonly DependencyPropertyKey BrushKey = DependencyProperty.RegisterReadOnly(nameof(Brush), typeof(DrawingBrush), typeof(CheckerBrush), new FrameworkPropertyMetadata(null, null, OnBrushCoerced));
        public static readonly DependencyProperty BrushProperty = BrushKey.DependencyProperty;
        public DrawingBrush Brush
        {
            get => (DrawingBrush)GetValue(BrushProperty);
            set => SetValue(BrushKey, value);
        }

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(nameof(Foreground), typeof(Brush), typeof(CheckerBrush), new FrameworkPropertyMetadata(DefaultForeground, OnPropertyChanged));
        public Brush Foreground
        {
            get => (Brush)GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(nameof(Size), typeof(double), typeof(CheckerBrush), new FrameworkPropertyMetadata(DefaultSize, OnPropertyChanged));
        public double Size
        {
            get => (double)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public CheckerBrush() : base() { }

        public static implicit operator Brush(CheckerBrush i)
        {
            var result = i.Brush;
            i.InvalidateProperty(BrushProperty);
            return result;
        }

        static object OnBrushCoerced(DependencyObject sender, object input)
        {
            if (sender is CheckerBrush brush)
                return Create(brush.Background, brush.Foreground, brush.Size);

            return input;
        }

        static void OnPropertyChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as CheckerBrush).InvalidateProperty(BrushProperty);
    
        static DrawingBrush Create(Brush background, Brush foreground, double size)
        {
            DrawingBrush result = null;
            result = new DrawingBrush
            {
                TileMode = TileMode.Tile,
                Viewport = new Rect(0.0, 0.0, size, size),
                ViewportUnits = BrushMappingMode.Absolute
            };

            var drawingGroup = new DrawingGroup();
            drawingGroup.Children.Add(new GeometryDrawing()
            {
                Brush = foreground,
                Geometry = System.Windows.Media.Geometry.Parse("M5,5 L0,5 0,10 5,10 5,5 10,5 10,0 5,0 Z")
            });
            drawingGroup.Children.Add(new GeometryDrawing()
            {
                Brush = background,
                Geometry = System.Windows.Media.Geometry.Parse("M0,0 L0,5 0,10 0,5, 10,5 10,10 5,10 5,0 Z")
            });

            result.Drawing = drawingGroup;
            return result;
        }
    }
}