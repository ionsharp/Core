using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    public class CheckerBoard : ContentControl
    {
        public DrawingBrush DrawingBrush
        {
            get
            {
                DrawingBrush DrawingBrush = new DrawingBrush();
                DrawingBrush.TileMode = TileMode.Tile;
                DrawingBrush.Viewport = new Rect(0.0, 0.0, this.CheckerSize, this.CheckerSize);
                DrawingBrush.ViewportUnits = BrushMappingMode.Absolute;

                DrawingGroup DrawingGroup = new DrawingGroup();
                DrawingGroup.Children.Add(new GeometryDrawing()
                {
                    Brush = this.CheckerForeground,
                    Geometry = Geometry.Parse("M5,5 L0,5 0,10 5,10 5,5 10,5 10,0 5,0 Z")
                });
                DrawingGroup.Children.Add(new GeometryDrawing()
                {
                    Brush = this.CheckerBackground,
                    Geometry = Geometry.Parse("M0,0 L0,5 0,10 0,5, 10,5 10,10 5,10 5,0 Z")
                });

                DrawingBrush.Drawing = DrawingGroup;
                return DrawingBrush;
            }
        }

        public static DependencyProperty CheckerForegroundProperty = DependencyProperty.Register("CheckerForeground", typeof(Brush), typeof(CheckerBoard), new PropertyMetadata(Brushes.Black, OnCheckerPropertyChanged));
        public Brush CheckerForeground
        {
            get
            {
                return (Brush)GetValue(CheckerForegroundProperty);
            }
            set
            {
                SetValue(CheckerForegroundProperty, value);
            }
        }

        public static DependencyProperty CheckerBackgroundProperty = DependencyProperty.Register("CheckerBackground", typeof(Brush), typeof(CheckerBoard), new PropertyMetadata(Brushes.White, OnCheckerPropertyChanged));
        public Brush CheckerBackground
        {
            get
            {
                return (Brush)GetValue(CheckerBackgroundProperty);
            }
            set
            {
                SetValue(CheckerBackgroundProperty, value);
            }
        }

        public static DependencyProperty CheckerSizeProperty = DependencyProperty.Register("CheckerSize", typeof(double), typeof(CheckerBoard), new PropertyMetadata(10.0, OnCheckerPropertyChanged));
        public double CheckerSize
        {
            get
            {
                return (double)GetValue(CheckerSizeProperty);
            }
            set
            {
                SetValue(CheckerSizeProperty, value);
            }
        }

        static void OnCheckerPropertyChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            Object.As<CheckerBoard>().Background = Object.As<CheckerBoard>().DrawingBrush;
        }

        public CheckerBoard() : base()
        {
            this.DefaultStyleKey = typeof(CheckerBoard);
            this.Background = this.DrawingBrush;
        }
    }
}
