using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class DashedBorder : Border
    {
        Brush actualBrush;

        protected static void OnBorderBrushChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DashedBorder border)
            {
                if (e.NewValue is GradientBrush || e.NewValue is SolidColorBrush)
                {
                    border.actualBrush = e.NewValue as Brush;
                    border.Update();
                }
            }
        }

        protected static void OnBorderThicknessChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DashedBorder border)
                border.Update();
        }

        public static readonly DependencyProperty BorderDashSizeProperty = DependencyProperty.Register(nameof(BorderDashSize), typeof(Size), typeof(DashedBorder), new FrameworkPropertyMetadata(default(Size), OnBorderDashSizeChanged));
        public Size BorderDashSize
        {
            get => (Size)GetValue(BorderDashSizeProperty);
            set => SetValue(BorderDashSizeProperty, value);
        }
        static void OnBorderDashSizeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DashedBorder border)
                border.Update();
        }

        static DashedBorder()
        {
            BorderBrushProperty.OverrideMetadata(typeof(DashedBorder), 
                new FrameworkPropertyMetadata(default(Brush), OnBorderBrushChanged));
            BorderThicknessProperty.OverrideMetadata(typeof(DashedBorder), 
                new FrameworkPropertyMetadata(default(Thickness), OnBorderThicknessChanged));
        }
        
        void Update()
        {
            if (actualBrush == null)
                return;

            var geometry = new GeometryGroup();
            geometry.Children.Add(new RectangleGeometry(new Rect(0, 0, 50, 50)));
            geometry.Children.Add(new RectangleGeometry(new Rect(50, 50, 50, 50)));

            var drawing = new GeometryDrawing
            {
                Brush = actualBrush,
                Geometry = geometry
            };

            var result = new DrawingBrush
            {
                Drawing = drawing,
                TileMode = TileMode.Tile,
                Viewport = new(0, 0, BorderDashSize.Width, BorderDashSize.Height),
                ViewportUnits = BrushMappingMode.Absolute
            };

            SetCurrentValue(BorderBrushProperty, result);
        }
    }
}