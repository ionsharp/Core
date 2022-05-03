using Imagin.Common.Converters;
using Imagin.Common.Media;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    #region (abstract) XOrElement

    public abstract class XOrElement : FrameworkElement
    {
        #region Legacy

        /*
        public const int R2_NOT = 6;  // Inverted drawing mode

        [DllImport("gdi32.dll", EntryPoint = "SetROP2", CallingConvention = CallingConvention.StdCall)]
        public extern static int SetROP2(IntPtr hdc, int fnDrawMode);

        [DllImport("user32.dll", EntryPoint = "GetDC", CallingConvention = CallingConvention.StdCall)]
        public extern static IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC", CallingConvention = CallingConvention.StdCall)]
        public extern static IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", EntryPoint = "MoveToEx", CallingConvention = CallingConvention.StdCall)]
        public extern static bool MoveToEx(IntPtr hdc, int x, int y, IntPtr lpPoint);

        [DllImport("gdi32.dll", EntryPoint = "LineTo", CallingConvention = CallingConvention.StdCall)]
        public extern static bool LineTo(IntPtr hdc, int x, int y);
        */

        #endregion

        public static readonly DependencyProperty DashStyle1Property = DependencyProperty.Register(nameof(DashStyle1), typeof(DashStyle), typeof(XOrElement), new FrameworkPropertyMetadata(default(DashStyle), FrameworkPropertyMetadataOptions.None, OnChanged));
        [TypeConverter(typeof(DashStyleTypeConverter))]
        public DashStyle DashStyle1
        {
            get => (DashStyle)GetValue(DashStyle1Property);
            set => SetValue(DashStyle1Property, value);
        }

        public static readonly DependencyProperty DashStyle2Property = DependencyProperty.Register(nameof(DashStyle2), typeof(DashStyle), typeof(XOrElement), new FrameworkPropertyMetadata(default(DashStyle), FrameworkPropertyMetadataOptions.None, OnChanged));
        [TypeConverter(typeof(DashStyleTypeConverter))]
        public DashStyle DashStyle2
        {
            get => (DashStyle)GetValue(DashStyle2Property);
            set => SetValue(DashStyle2Property, value);
        }

        public static readonly DependencyProperty StrokeThickness1Property = DependencyProperty.Register(nameof(StrokeThickness1), typeof(double), typeof(XOrElement), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.None, OnChanged));
        public double StrokeThickness1
        {
            get => (double)GetValue(StrokeThickness1Property);
            set => SetValue(StrokeThickness1Property, value);
        }

        public static readonly DependencyProperty StrokeThickness2Property = DependencyProperty.Register(nameof(StrokeThickness2), typeof(double), typeof(XOrElement), new FrameworkPropertyMetadata(0.5, FrameworkPropertyMetadataOptions.None, OnChanged));
        public double StrokeThickness2
        {
            get => (double)GetValue(StrokeThickness2Property);
            set => SetValue(StrokeThickness2Property, value);
        }

        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(nameof(Zoom), typeof(double), typeof(XOrElement), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.None, OnChanged));
        public double Zoom
        {
            get => (double)GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }

        public XOrElement() : base()
        {
            SetCurrentValue(DashStyle1Property, DashStyles.Solid);
            SetCurrentValue(DashStyle2Property, DashStyles.Solid);
        }

        void OnChanged() => InvalidateVisual();

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            InvalidateVisual();
        }

        protected static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var shape = d as XOrElement;
            //var handle = GetDC(IntPtr.Zero);
            //SetROP2(handle, R2_NOT);
            shape.OnChanged();
            //ReleaseDC(IntPtr.Zero, handle);
        }
    }

    #endregion

    #region (abstract) XOrRegion

    public abstract class XOrRegion : XOrElement
    {
        public static readonly DependencyProperty XProperty = DependencyProperty.Register(nameof(X), typeof(int), typeof(XOrRegion), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, OnChanged));
        public int X
        {
            get => (int)GetValue(XProperty);
            set => SetValue(XProperty, value);
        }

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(nameof(Y), typeof(int), typeof(XOrRegion), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, OnChanged));
        public int Y
        {
            get => (int)GetValue(YProperty);
            set => SetValue(YProperty, value);
        }

        public static readonly DependencyProperty HProperty = DependencyProperty.Register(nameof(H), typeof(int), typeof(XOrRegion), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, OnChanged));
        public int H
        {
            get => (int)GetValue(HProperty);
            set => SetValue(HProperty, value);
        }

        public static readonly DependencyProperty WProperty = DependencyProperty.Register(nameof(W), typeof(int), typeof(XOrRegion), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, OnChanged));
        public int W
        {
            get => (int)GetValue(WProperty);
            set => SetValue(WProperty, value);
        }
    }

    #endregion

    #region XOrEllipse

    public class XOrEllipse : XOrRegion
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawEllipse(Brushes.Transparent, new Pen(Brushes.Black, StrokeThickness1 / Zoom) { DashStyle = DashStyle1 }, new Point(X, Y), W, H);
            drawingContext.DrawEllipse(Brushes.Transparent, new Pen(Brushes.White, StrokeThickness2 / Zoom) { DashStyle = DashStyle2 }, new Point(X, Y), W, H);
        }
    }

    #endregion

    #region XOrLine

    public class XOrLine : XOrElement
    {
        public static readonly DependencyProperty X1Property = DependencyProperty.Register(nameof(X1), typeof(int), typeof(XOrLine), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, OnChanged));
        public int X1
        {
            get => (int)GetValue(X1Property);
            set => SetValue(X1Property, value);
        }

        public static readonly DependencyProperty Y1Property = DependencyProperty.Register(nameof(Y1), typeof(int), typeof(XOrLine), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, OnChanged));
        public int Y1
        {
            get => (int)GetValue(Y1Property);
            set => SetValue(Y1Property, value);
        }

        public static readonly DependencyProperty X2Property = DependencyProperty.Register(nameof(X2), typeof(int), typeof(XOrLine), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, OnChanged));
        public int X2
        {
            get => (int)GetValue(X2Property);
            set => SetValue(X2Property, value);
        }

        public static readonly DependencyProperty Y2Property = DependencyProperty.Register(nameof(Y2), typeof(int), typeof(XOrLine), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.None, OnChanged));
        public int Y2
        {
            get => (int)GetValue(Y2Property);
            set => SetValue(Y2Property, value);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawLine(new Pen(Brushes.Black, StrokeThickness1 / Zoom) { DashStyle = DashStyle1 }, new Point(X1, Y1), new Point(X2, Y2));
            drawingContext.DrawLine(new Pen(Brushes.White, StrokeThickness2 / Zoom) { DashStyle = DashStyle2 }, new Point(X1, Y1), new Point(X2, Y2));
        }
    }

    #endregion

    #region XOrPolygon

    public class XOrPolygon : XOrElement
    {
        public static readonly DependencyProperty ClosedProperty = DependencyProperty.Register(nameof(Closed), typeof(bool), typeof(XOrPolygon), new FrameworkPropertyMetadata(false, OnChanged));
        public bool Closed
        {
            get => (bool)GetValue(ClosedProperty);
            set => SetValue(ClosedProperty, value);
        }

        public static readonly DependencyProperty PointsProperty = DependencyProperty.Register(nameof(Points), typeof(PointCollection), typeof(XOrPolygon), new FrameworkPropertyMetadata(null, OnChanged));
        public PointCollection Points
        {
            get => (PointCollection)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (Points?.Count > 1)
            {
                var geometry = new Shape(Points).Geometry(Closed);
                drawingContext.DrawGeometry(Brushes.Transparent, new Pen(Brushes.Black, StrokeThickness1 / Zoom) { DashStyle = DashStyle1 }, geometry);
                drawingContext.DrawGeometry(Brushes.Transparent, new Pen(Brushes.White, StrokeThickness2 / Zoom) { DashStyle = DashStyle2 }, geometry);
            }
        }
    }

    #endregion

    #region XOrRectangle

    public class XOrRectangle : XOrRegion
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Black, StrokeThickness1 / Zoom) { DashStyle = DashStyle1 }, new Rect(X, Y, W, H));
            drawingContext.DrawRectangle(Brushes.Transparent, new Pen(Brushes.White, StrokeThickness2 / Zoom) { DashStyle = DashStyle2 }, new Rect(X, Y, W, H));
        }
    }

    #endregion
}