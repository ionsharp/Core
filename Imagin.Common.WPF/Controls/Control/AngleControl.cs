using Imagin.Common.Converters;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Imagin.Common.Controls
{
    public class AngleControl : Control
    {
        public static readonly ReferenceKey<Ellipse> EllipseKey = new();

        public static readonly ReferenceKey<Line> LineKey = new();

        #region Properties

        public static readonly DependencyProperty DegreesProperty = DependencyProperty.Register(nameof(Degrees), typeof(double), typeof(AngleControl), new FrameworkPropertyMetadata(0d, OnDegreesChanged));
        public double Degrees
        {
            get => (double)GetValue(DegreesProperty);
            set => SetValue(DegreesProperty, value);
        }
        static void OnDegreesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.As<AngleControl>().OnDegreesChanged(new Value<double>(e));

        public static readonly DependencyProperty OriginFillProperty = DependencyProperty.Register(nameof(OriginFill), typeof(Brush), typeof(AngleControl), new FrameworkPropertyMetadata(Brushes.Black));
        public Brush OriginFill
        {
            get => (Brush)GetValue(OriginFillProperty);
            set => SetValue(OriginFillProperty, value);
        }

        public static readonly DependencyProperty OriginStrokeProperty = DependencyProperty.Register(nameof(OriginStroke), typeof(Brush), typeof(AngleControl), new FrameworkPropertyMetadata(Brushes.Black));
        public Brush OriginStroke
        {
            get => (Brush)GetValue(OriginStrokeProperty);
            set => SetValue(OriginStrokeProperty, value);
        }

        public static readonly DependencyProperty OriginStrokeThicknessProperty = DependencyProperty.Register(nameof(OriginStrokeThickness), typeof(double), typeof(AngleControl), new FrameworkPropertyMetadata(8d));
        public double OriginStrokeThickness
        {
            get => (double)GetValue(OriginStrokeThicknessProperty);
            set => SetValue(OriginStrokeThicknessProperty, value);
        }

        public static readonly DependencyProperty OriginVisibilityProperty = DependencyProperty.Register(nameof(OriginVisibility), typeof(Visibility), typeof(AngleControl), new FrameworkPropertyMetadata(Visibility.Visible));
        public Visibility OriginVisibility
        {
            get => (Visibility)GetValue(OriginVisibilityProperty);
            set => SetValue(OriginVisibilityProperty, value);
        }
        
        public static readonly DependencyProperty NeedleStrokeProperty = DependencyProperty.Register(nameof(NeedleStroke), typeof(Brush), typeof(AngleControl), new FrameworkPropertyMetadata(Brushes.Black));
        public Brush NeedleStroke
        {
            get => (Brush)GetValue(NeedleStrokeProperty);
            set => SetValue(NeedleStrokeProperty, value);
        }

        public static readonly DependencyProperty NeedleStrokeThicknessProperty = DependencyProperty.Register(nameof(NeedleStrokeThickness), typeof(double), typeof(AngleControl), new FrameworkPropertyMetadata(2d));
        public double NeedleStrokeThickness
        {
            get => (double)GetValue(NeedleStrokeThicknessProperty);
            set => SetValue(NeedleStrokeThicknessProperty, value);
        }
        
        public static readonly DependencyProperty RadiansProperty = DependencyProperty.Register(nameof(Radians), typeof(double), typeof(AngleControl), new FrameworkPropertyMetadata(0d, OnRadiansChanged));
        public double Radians
        {
            get => (double)GetValue(RadiansProperty);
            set => SetValue(RadiansProperty, value);
        }
        static void OnRadiansChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => d.As<AngleControl>().OnRadiansChanged(new Value<double>(e));

        #endregion

        #region AngleControl

        readonly BindingExpressionBase j = null;

        public AngleControl() : base()
        {
            this.Bind(HeightProperty, nameof(Width), this, BindingMode.TwoWay);
            j = this.Bind(RadiansProperty, nameof(Degrees), this, BindingMode.TwoWay, new SimpleConverter<double, double>
            (
                i => i.GetRadian(),
                i => i.GetDegree()
            ));

            this.RegisterHandler(i =>
            {
                this.GetChild(EllipseKey).If(j =>
                {
                    j.MouseDown
                        += OnMouseDown;
                    j.MouseMove
                        += OnMouseMove;
                    j.MouseUp
                        += OnMouseUp;
                });
                CenterLine();
            }, i => 
            {
                this.GetChild(EllipseKey).If(j =>
                {
                    j.MouseDown
                        -= OnMouseDown;
                    j.MouseMove
                        -= OnMouseMove;
                    j.MouseUp
                        -= OnMouseUp;
                });
            });
        }

        #endregion

        #region Methods

        void CenterLine() => this.GetChild(LineKey).If(i =>
        {
            i.X1 = i.X2 = ActualWidth / 2d;
            i.Y2 = ActualHeight / 2d;
        });

        void UpdateLine() => this.GetChild(LineKey).If(i => i.RenderTransform = new RotateTransform(Degrees + 90d));

        //...

        double RadiansFromPoint(Point point)
        {
            var center = new Point(ActualWidth / 2, ActualHeight / 2);
            point.X = point.X.Coerce(ActualWidth);
            point.Y = point.Y.Coerce(ActualHeight);
            return Math.Atan2(point.Y - center.Y, point.X - center.X);
        }

        //...

        void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (sender is Ellipse i)
                {
                    i.CaptureMouse();
                    SetCurrentValue(RadiansProperty, RadiansFromPoint(e.GetPosition(i)));
                }
            }
        }

        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (sender is Ellipse i)
                    SetCurrentValue(RadiansProperty, RadiansFromPoint(e.GetPosition(i)));
            }
        }

        void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                if (sender is Ellipse i)
                {
                    if (i.IsMouseCaptured)
                        i.ReleaseMouseCapture();
                }
            }
        }

        //...

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            CenterLine();
            j?.UpdateTarget();
        }

        //...

        protected virtual void OnDegreesChanged(Value<double> input) => UpdateLine();

        protected virtual void OnRadiansChanged(Value<double> input) => UpdateLine();

        #endregion
    }
}