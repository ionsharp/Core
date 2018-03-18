using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class ComponentGliderBase : UserControl
    {
        Point? LastPoint
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        protected Grid PART_Marker
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        protected Grid PART_Surface
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool ColorChangeHandled = false;

        /// <summary>
        /// 
        /// </summary>
        protected bool mouseDown = false;

        /// <summary>
        /// 
        /// </summary>
        public static double MarkerLength = 7.0;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CheckerBackgroundProperty = DependencyProperty.Register(nameof(CheckerBackground), typeof(Color), typeof(ComponentGliderBase), new FrameworkPropertyMetadata(Colors.White));
        /// <summary>
        /// 
        /// </summary>
        public Color CheckerBackground
        {
            get => (Color)GetValue(CheckerBackgroundProperty);
            set => SetValue(CheckerBackgroundProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CheckerForegroundProperty = DependencyProperty.Register(nameof(CheckerForeground), typeof(Color), typeof(ComponentGliderBase), new FrameworkPropertyMetadata(Colors.LightGray));
        /// <summary>
        /// 
        /// </summary>
        public Color CheckerForeground
        {
            get => (Color)GetValue(CheckerForegroundProperty);
            set => SetValue(CheckerForegroundProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ComponentGliderBase), new FrameworkPropertyMetadata(Colors.Transparent, OnColorChanged));
        /// <summary>
        /// 
        /// </summary>
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
        static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<ComponentGliderBase>().OnColorChanged((Color)e.OldValue, (Color)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(ComponentGliderBase), new FrameworkPropertyMetadata(Orientation.Horizontal, OnOrientationChanged));
        /// <summary>
        /// 
        /// </summary>
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<ComponentGliderBase>().OnOrientationChanged((Orientation)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public ComponentGliderBase() => DefaultStyleKey = typeof(ComponentGliderBase);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected void UpdateMarker(double value)
        {
            if (PART_Marker != null)
            {
                switch (Orientation)
                {
                    case Orientation.Horizontal:
                        Canvas.SetLeft(PART_Marker, value);
                        Canvas.SetTop(PART_Marker, 0);
                        break;
                    case Orientation.Vertical:
                        Canvas.SetLeft(PART_Marker, 0);
                        Canvas.SetTop(PART_Marker, value);
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        protected void UpdateMarker(Point position)
        {
            var value = default(double);

            switch (Orientation)
            {
                case Orientation.Horizontal:
                    value = position.X.Coerce(ActualWidth) - (MarkerLength / 2.0);
                    break;
                case Orientation.Vertical:
                    value = position.Y.Coerce(ActualHeight) - (MarkerLength / 2.0);
                    break;
            }

            UpdateMarker(value);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Marker = Template.FindName("PART_Marker", this) as Grid;
            UpdateMarker(-(MarkerLength / 2.0));

            PART_Surface = Template.FindName("PART_Surface", this) as Grid;
            PART_Surface.MouseDown += OnMouseDown;
            PART_Surface.MouseMove += OnMouseMove;
            PART_Surface.MouseUp += OnMouseUp;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void Draw() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        protected virtual void Update(Point position) => UpdateMarker(position);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        protected virtual void OnColorChanged(Color OldValue, Color NewValue)
        {
            if (!ColorChangeHandled)
                Draw();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Update(e.GetPosition(this));
                ((Grid)sender).CaptureMouse();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                Update(e.GetPosition(this));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                ((Grid)sender).ReleaseMouseCapture();
                LastPoint = e.GetPosition(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnOrientationChanged(Orientation value)
        {
            if (LastPoint == null)
            {
                UpdateMarker(0);
            }
            else UpdateMarker(LastPoint.Value);
            Draw();
        }
    }
}