using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Imagin.Apps.Paint
{
    public partial class TransformView : UserControl
    {
        readonly object positionLock = new();

        readonly Thumb[] thumbs;

        readonly Thumb tl = null, t = null, tr = null, l = null, r = null, bl = null, b = null, br = null;

        Point centerPoint;

        Point startPoint;

        Vector startVector;

        PointCollection startPoints;

        static readonly DependencyPropertyKey BoundsKey = DependencyProperty.RegisterReadOnly(nameof(Bounds), typeof(PointCollection), typeof(TransformView), new FrameworkPropertyMetadata(null, OnBoundsChanged));
        public static readonly DependencyProperty BoundsProperty = BoundsKey.DependencyProperty;
        public PointCollection Bounds
        {
            get => (PointCollection)GetValue(BoundsProperty);
            private set => SetValue(BoundsKey, value);
        }
        protected static void OnBoundsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) 
            => sender.As<TransformView>().OnBoundsChanged((PointCollection)e.NewValue);
        
        public static readonly DependencyProperty LayerProperty = DependencyProperty.Register(nameof(Layer), typeof(VisualLayer), typeof(TransformView), new FrameworkPropertyMetadata(default(VisualLayer), OnLayerChanged));
        public VisualLayer Layer
        {
            get => (VisualLayer)GetValue(LayerProperty);
            set => SetValue(LayerProperty, value);
        }
        protected static void OnLayerChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
            => sender.As<TransformView>().OnLayerChanged((VisualLayer)e.NewValue);

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(TransformModes), typeof(TransformView), new FrameworkPropertyMetadata(TransformModes.Scale, OnModeChanged));
        public TransformModes Mode
        {
            get => (TransformModes)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }
        protected static void OnModeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
            => sender.As<TransformView>().OnModeChanged((TransformModes)e.NewValue);

        public static readonly DependencyProperty ShapeBoundsProperty = DependencyProperty.Register(nameof(ShapeBounds), typeof(PointCollection), typeof(TransformView), new FrameworkPropertyMetadata(default(PointCollection)));
        public PointCollection ShapeBounds
        {
            get => (PointCollection)GetValue(ShapeBoundsProperty);
            set => SetValue(ShapeBoundsProperty, value);
        }

        public static readonly DependencyProperty ThumbStyleProperty = DependencyProperty.Register(nameof(ThumbStyle), typeof(Style), typeof(TransformView), new FrameworkPropertyMetadata(default(Style)));
        public Style ThumbStyle
        {
            get => (Style)GetValue(ThumbStyleProperty);
            set => SetValue(ThumbStyleProperty, value);
        }

        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(nameof(Zoom), typeof(double), typeof(TransformView), new FrameworkPropertyMetadata(1d));
        public double Zoom
        {
            get => (double)GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }

        //...

        public TransformView()
        {
            InitializeComponent();

            tl = new Thumb(); t = new Thumb(); tr = new Thumb(); l = new Thumb(); r = new Thumb(); bl = new Thumb(); b = new Thumb(); br = new Thumb();

            thumbs = new Thumb[8] { tl, t, tr, l, r, bl, b, br };
            thumbs.ForEach(i =>
            {
                Thumbs.Children.Add(i);
                i.DragStarted += OnDragStarted;
                i.Bind(Thumb.StyleProperty, nameof(ThumbStyle), this);
            });

            SubscribeThumbs();
        }

        //...

        void OnBoundsChanged(PointCollection bounds)
        {
            lock (positionLock)
            {
                if (bounds.Count == 4)
                {
                    XCanvas.Set(tl, Bounds[0]);
                    XCanvas.Set(t, Bounds[0].Between(Bounds[1]));
                    XCanvas.Set(tr, Bounds[1]);
                    XCanvas.Set(l, Bounds[0].Between(Bounds[3]));
                    XCanvas.Set(r, Bounds[1].Between(Bounds[2]));
                    XCanvas.Set(bl, Bounds[3]);
                    XCanvas.Set(b, Bounds[3].Between(Bounds[2]));
                    XCanvas.Set(br, Bounds[2]);
                }
            }
        }

        void OnLayerChanged(VisualLayer layer)
            => Bounds = layer?.Bounds;

        void OnModeChanged(TransformModes mode)
            => Bounds = Layer?.Bounds;

        //...

        void SubscribeThumbs()
        {
            tl.DragDelta += OnTopLeftDragDelta;
            t.DragDelta += OnTopDragDelta;
            tr.DragDelta += OnTopRightDragDelta;
            l.DragDelta += OnLeftDragDelta;
            r.DragDelta += OnRightDragDelta;
            bl.DragDelta += OnBottomLeftDragDelta;
            b.DragDelta += OnBottomDragDelta;
            br.DragDelta += OnBottomRightDragDelta;
        }

        void UnsubscribeThumbs()
        {
            tl.DragDelta -= OnTopLeftDragDelta;
            t.DragDelta -= OnTopDragDelta;
            tr.DragDelta -= OnTopRightDragDelta;
            l.DragDelta -= OnLeftDragDelta;
            r.DragDelta -= OnRightDragDelta;
            bl.DragDelta -= OnBottomLeftDragDelta;
            b.DragDelta -= OnBottomDragDelta;
            br.DragDelta -= OnBottomRightDragDelta;
        }

        //...

        PointCollection Clone(PointCollection points)
        {
            var result = new PointCollection();
            points.ForEach(i => result.Add(i));
            return result;
        }

        double GetRadians()
        {
            var currentPoint = System.Windows.Input.Mouse.GetPosition(Thumbs);
            System.Windows.Vector deltaVector = Point.Subtract(currentPoint, centerPoint);
            double angle = System.Windows.Vector.AngleBetween(startVector, deltaVector);
            return angle.GetRadian();
        }

        void Rotate(PointCollection points)
        {
            var radians = GetRadians();

            double x(Point p) => Math.Cos(radians) * (p.X - centerPoint.X) - Math.Sin(radians) * (p.Y - centerPoint.Y) + centerPoint.X;
            double y(Point p) => Math.Sin(radians) * (p.X - centerPoint.X) + Math.Cos(radians) * (p.Y - centerPoint.Y) + centerPoint.X;

            points[0] = new Point(x(startPoints[0]), y(startPoints[0]));
            points[1] = new Point(x(startPoints[1]), y(startPoints[1]));
            points[2] = new Point(x(startPoints[2]), y(startPoints[2]));
            points[3] = new Point(x(startPoints[3]), y(startPoints[3]));
        }

        //...

        void OnDragStarted(object sender, DragStartedEventArgs e)
        {
            centerPoint = new Point(Bounds[0].X + ((Bounds[1].X - Bounds[0].X) / 2), Bounds[0].Y + ((Bounds[2].Y - Bounds[1].Y) / 2));
            startVector = Point.Subtract(startPoint, centerPoint);
            startPoints = Clone(Bounds);
            startPoint = System.Windows.Input.Mouse.GetPosition(Thumbs);
        }

        //...

        private void OnBottomLeftDragDelta(object sender, DragDeltaEventArgs e)
        {
            var result = Clone(Bounds);
            switch (Mode)
            {
                case TransformModes.Distort:
                    result[3] = new Point(startPoints[3].X + e.HorizontalChange, startPoints[3].Y + e.VerticalChange);
                    break;

                case TransformModes.Perspective:
                    break;

                case TransformModes.Rotate:
                    Rotate(result);
                    break;

                case TransformModes.Scale:
                    result[0] = new Point(startPoints[0].X + e.HorizontalChange, startPoints[0].Y);
                    result[2] = new Point(startPoints[2].X, startPoints[2].Y + e.VerticalChange);
                    result[3] = new Point(startPoints[3].X + e.HorizontalChange, startPoints[3].Y + e.VerticalChange);
                    break;

                case TransformModes.Skew:
                    //y = mx + b
                    //b = y - mx
                    double m = 0, b = 0;
                    if (e.HorizontalChange > e.VerticalChange)
                    {
                        m = (startPoints[2].Y - startPoints[3].Y) / (startPoints[2].X - startPoints[3].X);
                        b = startPoints[3].Y - (m * startPoints[3].X);

                        var x = startPoints[3].X + e.HorizontalChange;
                        result[3] = new Point(x, m * x + b);
                    }
                    else
                    {
                        m = (startPoints[0].Y - startPoints[3].Y) / (startPoints[0].X - startPoints[3].X);
                        b = startPoints[3].Y - (m * startPoints[3].X);

                        var y = startPoints[3].Y + e.VerticalChange;
                        result[3] = new Point((y - b) / m, y);
                    }
                    break;
            }
            Bounds = result;
        }

        private void OnBottomRightDragDelta(object sender, DragDeltaEventArgs e)
        {
            var result = Clone(Bounds);
            switch (Mode)
            {
                case TransformModes.Distort:
                    result[2] = new Point(startPoints[2].X + e.HorizontalChange, startPoints[2].Y + e.VerticalChange);
                    break;

                case TransformModes.Perspective:
                    break;

                case TransformModes.Rotate:
                    Rotate(result);
                    break;

                case TransformModes.Scale:
                    result[1] = new Point(startPoints[1].X + e.HorizontalChange, startPoints[1].Y);
                    result[2] = new Point(startPoints[2].X + e.HorizontalChange, startPoints[2].Y + e.VerticalChange);
                    result[3] = new Point(startPoints[3].X, startPoints[3].Y + e.VerticalChange);
                    break;

                case TransformModes.Skew:
                    //y = mx + b
                    //b = y - mx
                    double m = 0, b = 0;
                    if (e.HorizontalChange > e.VerticalChange)
                    {
                        m = (startPoints[2].Y - startPoints[3].Y) / (startPoints[2].X - startPoints[3].X);
                        b = startPoints[2].Y - (m * startPoints[2].X);

                        var x = startPoints[2].X + e.HorizontalChange;
                        result[2] = new Point(x, m * x + b);
                    }
                    else
                    {
                        m = (startPoints[1].Y - startPoints[2].Y) / (startPoints[1].X - startPoints[2].X);
                        b = startPoints[2].Y - (m * startPoints[2].X);

                        var y = startPoints[2].Y + e.VerticalChange;
                        result[2] = new Point((y - b) / m, y);
                    }
                    break;
            }
            Bounds = result;
        }

        //...

        private void OnBottomDragDelta(object sender, DragDeltaEventArgs e)
        {
            var result = Clone(Bounds);
            switch (Mode)
            {
                case TransformModes.Distort:
                    result[3] = new Point(startPoints[3].X + e.HorizontalChange, startPoints[3].Y + e.VerticalChange);
                    result[2] = new Point(startPoints[2].X + e.HorizontalChange, startPoints[2].Y + e.VerticalChange);
                    break;

                case TransformModes.Perspective:
                case TransformModes.Skew:
                    //y = mx + b
                    //b = y - mx
                    var m = (startPoints[2].Y - startPoints[3].Y) / (startPoints[2].X - startPoints[3].X);
                    var b = startPoints[2].Y - (m * startPoints[2].X);

                    var x1 = startPoints[2].X + e.HorizontalChange;
                    var x2 = startPoints[3].X + e.HorizontalChange;

                    result[2] = new Point(x1, m * x1 + b);
                    result[3] = new Point(x2, m * x2 + b);
                    break;

                case TransformModes.Rotate:
                    Rotate(result);
                    break;

                case TransformModes.Scale:
                    result[3] = new Point(startPoints[3].X, startPoints[3].Y + e.VerticalChange);
                    result[2] = new Point(startPoints[2].X, startPoints[2].Y + e.VerticalChange);
                    break;
            }
            Bounds = result;
        }

        private void OnLeftDragDelta(object sender, DragDeltaEventArgs e)
        {
            var result = Clone(Bounds);
            switch (Mode)
            {
                case TransformModes.Distort:
                    result[0] = new Point(startPoints[0].X + e.HorizontalChange, startPoints[0].Y + e.VerticalChange);
                    result[3] = new Point(startPoints[3].X + e.HorizontalChange, startPoints[3].Y + e.VerticalChange);
                    break;

                case TransformModes.Perspective:
                case TransformModes.Skew:
                    //x = (y - b) / m
                    //b = y - mx
                    var m = (startPoints[0].Y - startPoints[3].Y) / (startPoints[0].X - startPoints[3].X);
                    var b = startPoints[0].Y - (m * startPoints[0].X);

                    var y1 = startPoints[0].Y + e.VerticalChange;
                    var y2 = startPoints[3].Y + e.VerticalChange;

                    result[0] = new Point((y1 - b) / m, y1);
                    result[3] = new Point((y2 - b) / m, y2);
                    break;

                case TransformModes.Rotate:
                    Rotate(result);
                    break;

                case TransformModes.Scale:
                    result[0] = new Point(startPoints[0].X + e.HorizontalChange, startPoints[0].Y);
                    result[3] = new Point(startPoints[3].X + e.HorizontalChange, startPoints[3].Y);
                    break;
            }
            Bounds = result;
        }

        private void OnRightDragDelta(object sender, DragDeltaEventArgs e)
        {
            var result = Clone(Bounds);
            switch (Mode)
            {
                case TransformModes.Distort:
                    result[1] = new Point(startPoints[1].X + e.HorizontalChange, startPoints[1].Y + e.VerticalChange);
                    result[2] = new Point(startPoints[2].X + e.HorizontalChange, startPoints[2].Y + e.VerticalChange);
                    break;

                case TransformModes.Perspective:
                case TransformModes.Skew:
                    //x = (y - b) / m
                    //b = y - mx
                    var m = (startPoints[1].Y - startPoints[2].Y) / (startPoints[1].X - startPoints[2].X);
                    var b = startPoints[1].Y - (m * startPoints[1].X);

                    var y1 = startPoints[1].Y + e.VerticalChange;
                    var y2 = startPoints[2].Y + e.VerticalChange;

                    result[1] = new Point((y1 - b) / m, y1);
                    result[2] = new Point((y2 - b) / m, y2);
                    break;

                case TransformModes.Rotate:
                    Rotate(result);
                    break;

                case TransformModes.Scale:
                    result[1] = new Point(startPoints[1].X + e.HorizontalChange, startPoints[1].Y);
                    result[2] = new Point(startPoints[2].X + e.HorizontalChange, startPoints[2].Y);
                    break;
            }
            Bounds = result;
        }

        private void OnTopDragDelta(object sender, DragDeltaEventArgs e)
        {
            var result = Clone(Bounds);
            switch (Mode)
            {
                case TransformModes.Distort:
                    result[0] = new Point(startPoints[0].X + e.HorizontalChange, startPoints[0].Y + e.VerticalChange);
                    result[1] = new Point(startPoints[1].X + e.HorizontalChange, startPoints[1].Y + e.VerticalChange);
                    break;

                case TransformModes.Perspective:
                case TransformModes.Skew:
                    //y = mx + b
                    //b = y - mx
                    var m = (startPoints[0].Y - startPoints[1].Y) / (startPoints[0].X - startPoints[1].X);
                    var b = startPoints[0].Y - (m * startPoints[0].X);

                    var x1 = startPoints[0].X + e.HorizontalChange;
                    var x2 = startPoints[1].X + e.HorizontalChange;

                    result[0] = new Point(x1, m * x1 + b);
                    result[1] = new Point(x2, m * x2 + b);
                    break;

                case TransformModes.Rotate:
                    Rotate(result);
                    break;

                case TransformModes.Scale:
                    result[0] = new Point(startPoints[0].X, startPoints[0].Y + e.VerticalChange);
                    result[1] = new Point(startPoints[1].X, startPoints[1].Y + e.VerticalChange);
                    break;
            }
            Bounds = result;
        }

        //...

        private void OnTopLeftDragDelta(object sender, DragDeltaEventArgs e)
        {
            var result = Clone(Bounds);
            switch (Mode)
            {
                case TransformModes.Distort:
                    result[0] = new Point(startPoints[0].X + e.HorizontalChange, startPoints[0].Y + e.VerticalChange);
                    break;

                case TransformModes.Perspective:
                    break;

                case TransformModes.Rotate:
                    Rotate(result);
                    break;

                case TransformModes.Scale:
                    result[3] = new Point(startPoints[3].X + e.HorizontalChange, startPoints[3].Y);
                    result[0] = new Point(startPoints[0].X + e.HorizontalChange, startPoints[0].Y + e.VerticalChange);
                    result[1] = new Point(startPoints[1].X, startPoints[1].Y + e.VerticalChange);
                    break;

                case TransformModes.Skew:
                    //y = mx + b
                    //b = y - mx
                    double m = 0, b = 0;
                    if (e.HorizontalChange > e.VerticalChange)
                    {
                        m = (startPoints[0].Y - startPoints[1].Y) / (startPoints[0].X - startPoints[1].X);
                        b = startPoints[0].Y - (m * startPoints[0].X);

                        var x = startPoints[0].X + e.HorizontalChange;
                        result[0] = new Point(x, m * x + b);
                    }
                    else
                    {
                        m = (startPoints[0].Y - startPoints[3].Y) / (startPoints[0].X - startPoints[3].X);
                        b = startPoints[0].Y - (m * startPoints[0].X);
                        
                        var y = startPoints[0].Y + e.VerticalChange;
                        result[0] = new Point((y - b) / m, y);
                    }
                    break;
            }
            Bounds = result;
        }

        private void OnTopRightDragDelta(object sender, DragDeltaEventArgs e)
        {
            var result = Clone(Bounds);
            switch (Mode)
            {
                case TransformModes.Distort:
                    result[1] = new Point(startPoints[1].X + e.HorizontalChange, startPoints[1].Y + e.VerticalChange);
                    break;

                case TransformModes.Perspective:
                    break;

                case TransformModes.Rotate:
                    Rotate(result);
                    break;

                case TransformModes.Scale:
                    result[0] = new Point(startPoints[0].X, startPoints[0].Y + e.VerticalChange);
                    result[1] = new Point(startPoints[1].X + e.HorizontalChange, startPoints[1].Y + e.VerticalChange);
                    result[2] = new Point(startPoints[2].X + e.HorizontalChange, startPoints[2].Y);
                    break;

                case TransformModes.Skew:
                    //y = mx + b
                    //b = y - mx
                    double m = 0, b = 0;
                    if (e.HorizontalChange > e.VerticalChange)
                    {
                        m = (startPoints[0].Y - startPoints[1].Y) / (startPoints[0].X - startPoints[1].X);
                        b = startPoints[1].Y - (m * startPoints[1].X);

                        var x = startPoints[1].X + e.HorizontalChange;
                        result[1] = new Point(x, m * x + b);
                    }
                    else
                    {
                        m = (startPoints[1].Y - startPoints[2].Y) / (startPoints[1].X - startPoints[2].X);
                        b = startPoints[1].Y - (m * startPoints[1].X);

                        var y = startPoints[1].Y + e.VerticalChange;
                        result[1] = new Point((y - b) / m, y);
                    }
                    break;
            }
            Bounds = result;
        }
    }
}