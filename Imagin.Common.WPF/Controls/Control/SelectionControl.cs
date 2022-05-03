using Imagin.Common.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public abstract class SelectionControl : Control
    {
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(SelectionControl), new FrameworkPropertyMetadata(Brushes.Transparent));
        public Brush Fill
        {
            get => (Brush)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }

        public static readonly DependencyProperty StrokePrimaryProperty = DependencyProperty.Register(nameof(StrokePrimary), typeof(Brush), typeof(SelectionControl), new FrameworkPropertyMetadata(Brushes.Black));
        public Brush StrokePrimary
        {
            get => (Brush)GetValue(StrokePrimaryProperty);
            set => SetValue(StrokePrimaryProperty, value);
        }

        public static readonly DependencyProperty StrokeSecondaryProperty = DependencyProperty.Register(nameof(StrokeSecondary), typeof(Brush), typeof(SelectionControl), new FrameworkPropertyMetadata(Brushes.White));
        public Brush StrokeSecondary
        {
            get => (Brush)GetValue(StrokeSecondaryProperty);
            set => SetValue(StrokeSecondaryProperty, value);
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(SelectionControl), new FrameworkPropertyMetadata(1.0));
        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }
    }
    

    public class SelectionRectangle : SelectionControl { }

    public class SelectionPolygon : SelectionControl
    {
        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(Shape), typeof(SelectionControl), new FrameworkPropertyMetadata(null));
        public Shape Path
        {
            get => (Shape)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
    }
}