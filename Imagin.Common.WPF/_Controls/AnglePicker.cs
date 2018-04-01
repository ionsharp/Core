using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [TemplatePart(Name = "PART_Ellipse", Type = typeof(Ellipse))]
    [TemplatePart(Name = "PART_Line", Type = typeof(Line))]
    [TemplatePart(Name = "PART_Origin", Type = typeof(Ellipse))]
    public class AnglePicker : System.Windows.Controls.UserControl
    {
        #region Properties

        Ellipse PART_Ellipse { get; set; } = null;

        System.Windows.Shapes.Line PART_Line { get; set; } = null;

        Ellipse PART_Origin { get; set; } = null;

        bool AngleChangeHandled = false;

        bool RadiansChangeHandled = false;

        bool ValueChangeHandled = false;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty AngleProperty = DependencyProperty.Register("Angle", typeof(double), typeof(AnglePicker), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnAngleChanged));
        /// <summary>
        /// 
        /// </summary>
        public double Angle
        {
            get
            {
                return (double)GetValue(AngleProperty);
            }
            set
            {
                SetValue(AngleProperty, value);
            }
        }
        static void OnAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<AnglePicker>().OnAngleChanged((double)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty OriginFillProperty = DependencyProperty.Register("OriginFill", typeof(Brush), typeof(AnglePicker), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Brush OriginFill
        {
            get
            {
                return (Brush)GetValue(OriginFillProperty);
            }
            set
            {
                SetValue(OriginFillProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty OriginStrokeProperty = DependencyProperty.Register("OriginStroke", typeof(Brush), typeof(AnglePicker), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Brush OriginStroke
        {
            get
            {
                return (Brush)GetValue(OriginStrokeProperty);
            }
            set
            {
                SetValue(OriginStrokeProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty OriginStrokeThicknessProperty = DependencyProperty.Register("OriginStrokeThickness", typeof(double), typeof(AnglePicker), new FrameworkPropertyMetadata(8d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double OriginStrokeThickness
        {
            get
            {
                return (double)GetValue(OriginStrokeThicknessProperty);
            }
            set
            {
                SetValue(OriginStrokeThicknessProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty OriginVisibilityProperty = DependencyProperty.Register("OriginVisibility", typeof(Visibility), typeof(AnglePicker), new FrameworkPropertyMetadata(Visibility.Visible, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Visibility OriginVisibility
        {
            get
            {
                return (Visibility)GetValue(OriginVisibilityProperty);
            }
            set
            {
                SetValue(OriginVisibilityProperty, value);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty NeedleStrokeProperty = DependencyProperty.Register("NeedleStroke", typeof(Brush), typeof(AnglePicker), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Brush NeedleStroke
        {
            get
            {
                return (Brush)GetValue(NeedleStrokeProperty);
            }
            set
            {
                SetValue(NeedleStrokeProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty NeedleStrokeThicknessProperty = DependencyProperty.Register("NeedleStrokeThickness", typeof(double), typeof(AnglePicker), new FrameworkPropertyMetadata(2d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double NeedleStrokeThickness
        {
            get
            {
                return (double)GetValue(NeedleStrokeThicknessProperty);
            }
            set
            {
                SetValue(NeedleStrokeThicknessProperty, value);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty RadiansProperty = DependencyProperty.Register("Radians", typeof(double), typeof(AnglePicker), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnRadiansChanged));
        /// <summary>
        /// 
        /// </summary>
        public double Radians
        {
            get
            {
                return (double)GetValue(RadiansProperty);
            }
            set
            {
                SetValue(RadiansProperty, value);
            }
        }
        static void OnRadiansChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<AnglePicker>().OnRadiansChanged((double)e.NewValue);
        }

        #endregion

        #region AnglePicker

        /// <summary>
        /// 
        /// </summary>
        public AnglePicker()
        {
            DefaultStyleKey = typeof(AnglePicker);

            Background = Brushes.LightGray;

            Loaded += AnglePicker_Loaded;
        }

        #endregion

        #region Methods

        void AnglePicker_Loaded(object sender, RoutedEventArgs e)
        {
            if (PART_Line != null)
            {
                PART_Line.X1 = PART_Line.X2 = Width / 2;
                PART_Line.Y2 = Height / 2;
            }
        }

        void Adjust(Ellipse Ellipse, Point CurrentPoint)
        {
            var CenterPoint = new Point(Ellipse.ActualWidth / 2, Ellipse.ActualHeight / 2);

            CurrentPoint.X = CurrentPoint.X.Coerce(Ellipse.ActualWidth);
            CurrentPoint.Y = CurrentPoint.Y.Coerce(Ellipse.ActualHeight);

            ValueChangeHandled = true;
            Angle = Geometry.Angle.GetDegree(Radians);
            Radians = Math.Atan2(CurrentPoint.Y - CenterPoint.Y, CurrentPoint.X - CenterPoint.X);
            ValueChangeHandled = false;

            PART_Line.RenderTransform = new RotateTransform(Angle + 90d);
        }

        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var Ellipse = sender as Ellipse;
                Ellipse.CaptureMouse();

                Adjust(Ellipse, e.GetPosition(Ellipse));
            }
        }

        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var Ellipse = sender as Ellipse;
                Adjust(sender as Ellipse, e.GetPosition(Ellipse));
            }
        }

        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                var Ellipse = sender as Ellipse;

                if (Ellipse.IsMouseCaptured)
                    Ellipse.ReleaseMouseCapture();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnAngleChanged(double Value)
        {
            if (!ValueChangeHandled && !AngleChangeHandled)
            {
                RadiansChangeHandled = true;
                Radians = Geometry.Angle.GetRadian(Value);
                RadiansChangeHandled = false;

                PART_Line.RenderTransform = new RotateTransform(Value + 90d);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnRadiansChanged(double Value)
        {
            if (!ValueChangeHandled && !RadiansChangeHandled)
            {
                AngleChangeHandled = true;
                Angle = Geometry.Angle.GetDegree(Value);
                AngleChangeHandled = false;

                PART_Line.RenderTransform = new RotateTransform(Angle + 90d);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Ellipse = Template.FindName("PART_Ellipse", this) as Ellipse;
            PART_Line = Template.FindName("PART_Line", this) as System.Windows.Shapes.Line;
            PART_Origin = Template.FindName("PART_Origin", this) as Ellipse;

            PART_Ellipse.MouseDown += OnMouseDown;
            PART_Ellipse.MouseMove += OnMouseMove;
            PART_Ellipse.MouseUp += OnMouseUp;
        }

        #endregion
    }
}
