using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class CheckerBoard : ContentControl
    {
        /// <summary>
        /// 
        /// </summary>
        public DrawingBrush DrawingBrush
        {
            get
            {
                var DrawingBrush = new DrawingBrush();
                DrawingBrush.TileMode = TileMode.Tile;
                DrawingBrush.Viewport = new Rect(0.0, 0.0, this.CheckerSize, this.CheckerSize);
                DrawingBrush.ViewportUnits = BrushMappingMode.Absolute;

                var DrawingGroup = new DrawingGroup();
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CheckerForegroundProperty = DependencyProperty.Register("CheckerForeground", typeof(Brush), typeof(CheckerBoard), new PropertyMetadata(Brushes.Black, OnCheckerPropertyChanged));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CheckerBackgroundProperty = DependencyProperty.Register("CheckerBackground", typeof(Brush), typeof(CheckerBoard), new PropertyMetadata(Brushes.White, OnCheckerPropertyChanged));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CheckerSizeProperty = DependencyProperty.Register("CheckerSize", typeof(double), typeof(CheckerBoard), new PropertyMetadata(10.0, OnCheckerPropertyChanged));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public CheckerBoard() : base()
        {
            DefaultStyleKey = typeof(CheckerBoard);
            SetCurrentValue(BackgroundProperty, DrawingBrush);
        }
    }
}
