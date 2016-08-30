using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    public class Line : UserControl
    {
        public static DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(Line), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush Color
        {
            get
            {
                return (Brush)GetValue(ColorProperty);
            }
            set
            {
                SetValue(ColorProperty, value);
            }
        }

        public static DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(double), typeof(Line), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double Stroke
        {
            get
            {
                return (double)GetValue(StrokeProperty);
            }
            set
            {
                SetValue(StrokeProperty, value);
            }
        }

        public static DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Line), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Orientation Orientation
        {
            get
            {
                return (Orientation)GetValue(OrientationProperty);
            }
            set
            {
                SetValue(OrientationProperty, value);
            }
        }

        public Line() : base()
        {
            this.DefaultStyleKey = typeof(Line);
        }

        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();
        }
    }
}
