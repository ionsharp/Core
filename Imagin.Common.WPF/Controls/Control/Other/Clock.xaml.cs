using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Imagin.Common.Controls
{
    public partial class Clock : Control
    {
        public static readonly ReferenceKey<Canvas> CanvasKey = new();

        //...

        const double height = 100;

        const double width = 100;

        //...

        static System.Windows.Threading.DispatcherTimer DefaultTimer => new()
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        System.Windows.Threading.DispatcherTimer timer = null;

        //...

        Canvas Canvas => this.GetChild<Canvas>(CanvasKey);

        //...

        readonly List<Line> intermediateTicks = new();

        readonly List<Line> majorTicks = new();

        readonly List<Ellipse> minorTicks = new();

        //...

        public static readonly DependencyProperty CenterTemplateProperty = DependencyProperty.Register(nameof(CenterTemplate), typeof(DataTemplate), typeof(Clock), new FrameworkPropertyMetadata(null));
        public DataTemplate CenterTemplate
        {
            get => (DataTemplate)GetValue(CenterTemplateProperty);
            set => SetValue(CenterTemplateProperty, value);
        }

        public static readonly DependencyProperty DateTimeProperty = DependencyProperty.Register(nameof(DateTime), typeof(DateTime), typeof(Clock), new FrameworkPropertyMetadata(default(DateTime)));
        public DateTime DateTime
        {
            get => (DateTime)GetValue(DateTimeProperty);
            private set => SetValue(DateTimeProperty, value);
        }

        public static readonly DependencyProperty AProperty = DependencyProperty.Register(nameof(A), typeof(DateTime), typeof(Clock), new FrameworkPropertyMetadata(DateTime.Now.Date.AddHours(3), OnAChanged));
        public DateTime A
        {
            get => (DateTime)GetValue(AProperty);
            set => SetValue(AProperty, value);
        }
        static void OnAChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Clock control)
                control.APosition = control.GetPosition(new Value<DateTime>(e).New);
        }

        static readonly DependencyPropertyKey APositionKey = DependencyProperty.RegisterReadOnly(nameof(APosition), typeof(Point), typeof(Clock), new FrameworkPropertyMetadata(new Point(0, 0)));
        public static readonly DependencyProperty APositionProperty = APositionKey.DependencyProperty;
        public Point APosition
        {
            get => (Point)GetValue(APositionProperty);
            private set => SetValue(APositionKey, value);
        }

        public static readonly DependencyProperty AStrokeProperty = DependencyProperty.Register(nameof(AStroke), typeof(Brush), typeof(Clock), new FrameworkPropertyMetadata(Brushes.Black));
        public Brush AStroke
        {
            get => (Brush)GetValue(AStrokeProperty);
            set => SetValue(AStrokeProperty, value);
        }

        public static readonly DependencyProperty AStrokeThicknessProperty = DependencyProperty.Register(nameof(AStrokeThickness), typeof(double), typeof(Clock), new FrameworkPropertyMetadata(6.0));
        public double AStrokeThickness
        {
            get => (double)GetValue(AStrokeThicknessProperty);
            set => SetValue(AStrokeThicknessProperty, value);
        }

        public static readonly DependencyProperty AVisibilityProperty = DependencyProperty.Register(nameof(AVisibility), typeof(Visibility), typeof(Clock), new FrameworkPropertyMetadata(Visibility.Collapsed));
        public Visibility AVisibility
        {
            get => (Visibility)GetValue(AVisibilityProperty);
            set => SetValue(AVisibilityProperty, value);
        }

        //...

        public static readonly DependencyProperty BProperty = DependencyProperty.Register(nameof(B), typeof(DateTime), typeof(Clock), new FrameworkPropertyMetadata(DateTime.Now.Date.AddHours(9), OnBChanged));
        public DateTime B
        {
            get => (DateTime)GetValue(BProperty);
            set => SetValue(BProperty, value);
        }
        static void OnBChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Clock control)
                control.BPosition = control.GetPosition(new Value<DateTime>(e).New);
        }

        static readonly DependencyPropertyKey BPositionKey = DependencyProperty.RegisterReadOnly(nameof(BPosition), typeof(Point), typeof(Clock), new FrameworkPropertyMetadata(new Point(0, 0)));
        public static readonly DependencyProperty BPositionProperty = BPositionKey.DependencyProperty;
        public Point BPosition
        {
            get => (Point)GetValue(BPositionProperty);
            private set => SetValue(BPositionKey, value);
        }

        public static readonly DependencyProperty BStrokeProperty = DependencyProperty.Register(nameof(BStroke), typeof(Brush), typeof(Clock), new FrameworkPropertyMetadata(Brushes.Black));
        public Brush BStroke
        {
            get => (Brush)GetValue(BStrokeProperty);
            set => SetValue(BStrokeProperty, value);
        }

        public static readonly DependencyProperty BStrokeThicknessProperty = DependencyProperty.Register(nameof(BStrokeThickness), typeof(double), typeof(Clock), new FrameworkPropertyMetadata(6.0));
        public double BStrokeThickness
        {
            get => (double)GetValue(BStrokeThicknessProperty);
            set => SetValue(BStrokeThicknessProperty, value);
        }

        public static readonly DependencyProperty BVisibilityProperty = DependencyProperty.Register(nameof(BVisibility), typeof(Visibility), typeof(Clock), new FrameworkPropertyMetadata(Visibility.Collapsed));
        public Visibility BVisibility
        {
            get => (Visibility)GetValue(BVisibilityProperty);
            set => SetValue(BVisibilityProperty, value);
        }

        //...

        public static readonly DependencyProperty HourStrokeProperty = DependencyProperty.Register(nameof(HourStroke), typeof(Brush), typeof(Clock), new FrameworkPropertyMetadata(Brushes.Black));
        public Brush HourStroke
        {
            get => (Brush)GetValue(HourStrokeProperty);
            set => SetValue(HourStrokeProperty, value);
        }

        public static readonly DependencyProperty HourStrokeThicknessProperty = DependencyProperty.Register(nameof(HourStrokeThickness), typeof(double), typeof(Clock), new FrameworkPropertyMetadata(3.0));
        public double HourStrokeThickness
        {
            get => (double)GetValue(HourStrokeThicknessProperty);
            set => SetValue(HourStrokeThicknessProperty, value);
        }

        static readonly DependencyPropertyKey HourTransformKey = DependencyProperty.RegisterReadOnly(nameof(HourTransform), typeof(RotateTransform), typeof(Clock), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty HourTransformProperty = HourTransformKey.DependencyProperty;
        public RotateTransform HourTransform
        {
            get => (RotateTransform)GetValue(HourTransformProperty);
            private set => SetValue(HourTransformKey, value);
        }

        //...

        public static readonly DependencyProperty MinuteStrokeProperty = DependencyProperty.Register(nameof(MinuteStroke), typeof(Brush), typeof(Clock), new FrameworkPropertyMetadata(Brushes.Black));
        public Brush MinuteStroke
        {
            get => (Brush)GetValue(MinuteStrokeProperty);
            set => SetValue(MinuteStrokeProperty, value);
        }

        public static readonly DependencyProperty MinuteStrokeThicknessProperty = DependencyProperty.Register(nameof(MinuteStrokeThickness), typeof(double), typeof(Clock), new FrameworkPropertyMetadata(2.0));
        public double MinuteStrokeThickness
        {
            get => (double)GetValue(MinuteStrokeThicknessProperty);
            set => SetValue(MinuteStrokeThicknessProperty, value);
        }

        static readonly DependencyPropertyKey MinuteTransformKey = DependencyProperty.RegisterReadOnly(nameof(MinuteTransform), typeof(RotateTransform), typeof(Clock), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty MinuteTransformProperty = MinuteTransformKey.DependencyProperty;
        public RotateTransform MinuteTransform
        {
            get => (RotateTransform)GetValue(MinuteTransformProperty);
            private set => SetValue(MinuteTransformKey, value);
        }

        //...

        public static readonly DependencyProperty SecondStrokeProperty = DependencyProperty.Register(nameof(SecondStroke), typeof(Brush), typeof(Clock), new FrameworkPropertyMetadata(Brushes.Black));
        public Brush SecondStroke
        {
            get => (Brush)GetValue(SecondStrokeProperty);
            set => SetValue(SecondStrokeProperty, value);
        }

        public static readonly DependencyProperty SecondStrokeThicknessProperty = DependencyProperty.Register(nameof(SecondStrokeThickness), typeof(double), typeof(Clock), new FrameworkPropertyMetadata(1.0));
        public double SecondStrokeThickness
        {
            get => (double)GetValue(SecondStrokeThicknessProperty);
            set => SetValue(SecondStrokeThicknessProperty, value);
        }

        static readonly DependencyPropertyKey SecondTransformKey = DependencyProperty.RegisterReadOnly(nameof(SecondTransform), typeof(RotateTransform), typeof(Clock), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty SecondTransformProperty = SecondTransformKey.DependencyProperty;
        public RotateTransform SecondTransform
        {
            get => (RotateTransform)GetValue(SecondTransformProperty);
            private set => SetValue(SecondTransformKey, value);
        }

        //...

        public static readonly DependencyProperty MajorTickStrokeProperty = DependencyProperty.Register(nameof(MajorTickStroke), typeof(Brush), typeof(Clock), new FrameworkPropertyMetadata(Brushes.Black, OnTicksChanged));
        public Brush MajorTickStroke
        {
            get => (Brush)GetValue(MajorTickStrokeProperty);
            set => SetValue(MajorTickStrokeProperty, value);
        }

        public static readonly DependencyProperty MajorTickStrokeThicknessProperty = DependencyProperty.Register(nameof(MajorTickStrokeThickness), typeof(double), typeof(Clock), new FrameworkPropertyMetadata(2.0, OnTicksChanged));
        public double MajorTickStrokeThickness
        {
            get => (double)GetValue(MajorTickStrokeThicknessProperty);
            set => SetValue(MajorTickStrokeThicknessProperty, value);
        }

        public static readonly DependencyProperty MajorTickLengthProperty = DependencyProperty.Register(nameof(MajorTickLength), typeof(double), typeof(Clock), new FrameworkPropertyMetadata(8.0, OnTicksChanged));
        public double MajorTickLength
        {
            get => (double)GetValue(MajorTickLengthProperty);
            set => SetValue(MajorTickLengthProperty, value);
        }

        //...

        public static readonly DependencyProperty MinorTickStrokeProperty = DependencyProperty.Register(nameof(MinorTickStroke), typeof(Brush), typeof(Clock), new FrameworkPropertyMetadata(Brushes.Black, OnTicksChanged));
        public Brush MinorTickStroke
        {
            get => (Brush)GetValue(MinorTickStrokeProperty);
            set => SetValue(MinorTickStrokeProperty, value);
        }

        public static readonly DependencyProperty MinorTickStrokeThicknessProperty = DependencyProperty.Register(nameof(MinorTickStrokeThickness), typeof(double), typeof(Clock), new FrameworkPropertyMetadata(2.0, OnTicksChanged));
        public double MinorTickStrokeThickness
        {
            get => (double)GetValue(MinorTickStrokeThicknessProperty);
            set => SetValue(MinorTickStrokeThicknessProperty, value);
        }

        //...

        public static readonly DependencyProperty IntermediateTickStrokeProperty = DependencyProperty.Register(nameof(IntermediateTickStroke), typeof(Brush), typeof(Clock), new FrameworkPropertyMetadata(Brushes.Black, OnTicksChanged));
        public Brush IntermediateTickStroke
        {
            get => (Brush)GetValue(IntermediateTickStrokeProperty);
            set => SetValue(IntermediateTickStrokeProperty, value);
        }

        public static readonly DependencyProperty IntermediateTickStrokeThicknessProperty = DependencyProperty.Register(nameof(IntermediateTickStrokeThickness), typeof(double), typeof(Clock), new FrameworkPropertyMetadata(2.0, OnTicksChanged));
        public double IntermediateTickStrokeThickness
        {
            get => (double)GetValue(IntermediateTickStrokeThicknessProperty);
            set => SetValue(IntermediateTickStrokeThicknessProperty, value);
        }

        public static readonly DependencyProperty IntermediateTickLengthProperty = DependencyProperty.Register(nameof(IntermediateTickLength), typeof(double), typeof(Clock), new FrameworkPropertyMetadata(4.0, OnTicksChanged));
        public double IntermediateTickLength
        {
            get => (double)GetValue(IntermediateTickLengthProperty);
            set => SetValue(IntermediateTickLengthProperty, value);
        }

        //...

        public static readonly DependencyProperty TimeZoneProperty = DependencyProperty.Register(nameof(TimeZone), typeof(Time.TimeZone), typeof(Clock), new FrameworkPropertyMetadata(Time.TimeZone.EasternStandardTime));
        public Time.TimeZone TimeZone
        {
            get => (Time.TimeZone)GetValue(TimeZoneProperty);
            set => SetValue(TimeZoneProperty, value);
        }

        //...

        static void OnTicksChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as Clock).DrawTicks();

        //...

        public Clock() : base()
        {
            HourTransform 
                = new();
            MinuteTransform 
                = new();
            SecondTransform 
                = new();

            this.RegisterHandler(i =>
            {
                timer = DefaultTimer;
                timer.Tick += OnTick;
                timer.Start();
            }, i =>
            {
                timer.Tick -= OnTick;
                timer.Stop();
                timer = null;
            });

            InitializeComponent();
        }

        //...

        double AngleFrom(DateTime input) => ((input.Hour > 11 ? input.Hour - 12 : input.Hour) * 30.0) + (input.Minute * 0.5) - 90;

        //...

        void OnTick(object sender, EventArgs e)
        {
            TimeZone.TryNow(out DateTime now);

            HourTransform.CenterX
                = width / 2.0;
            HourTransform.CenterY
                = height / 2.0;
            HourTransform.Angle
                = (now.Hour * 30.0) + (now.Minute * 0.5);

            MinuteTransform.CenterX
                = width / 2.0;
            MinuteTransform.CenterY
                = height / 2.0;
            MinuteTransform.Angle
                = now.Minute * 6.0;

            SecondTransform.CenterX
                = width / 2.0;
            SecondTransform.CenterY
                = height / 2.0;
            SecondTransform.Angle
                = now.Second * 6.0;

            SetCurrentValue(DateTimeProperty, now);
        }

        //...

        void ClearTicks()
        {
            if (Canvas == null)
                return;

            for (var i = majorTicks.Count - 1; i >= 0; i--)
            {
                Canvas.Children.Remove(majorTicks[i]);
                majorTicks.RemoveAt(i);
            }
            for (var i = minorTicks.Count - 1; i >= 0; i--)
            {
                Canvas.Children.Remove(minorTicks[i]);
                minorTicks.RemoveAt(i);
            }
            for (var i = intermediateTicks.Count - 1; i >= 0; i--)
            {
                Canvas.Children.Remove(intermediateTicks[i]);
                intermediateTicks.RemoveAt(i);
            }
        }

        void DrawTicks()
        {
            if (Canvas == null)
                return;

            ClearTicks();

            var x = width / 2.0;
            var y = 0.0;

            for (var i = 0; i < 4; i++)
            {
                var line = new Line
                {
                    Fill = MajorTickStroke,
                    Stroke = MajorTickStroke,
                    StrokeThickness = MajorTickStrokeThickness,

                    X1 = x,
                    Y1 = y
                };

                switch (i)
                {
                    case 0:
                        line.X2 = x;
                        line.Y2 = y + MajorTickLength;
                        x += width / 2.0;
                        y += height / 2.0;
                        break;
                    case 1:
                        line.X2 = x - MajorTickLength;
                        line.Y2 = y;
                        x -= width / 2.0;
                        y += height / 2.0;
                        break;
                    case 2:
                        line.X2 = x;
                        line.Y2 = y - MajorTickLength;
                        x -= width / 2.0;
                        y -= height / 2.0;
                        break;
                    case 3:
                        line.X2 = x + MajorTickLength;
                        line.Y2 = y;
                        x += width / 2.0;
                        y -= height / 2.0;
                        break;
                }

                Canvas.Children.Add(line);
                majorTicks.Add(line);

                x = x > width ? 0 : x;
                y = y > height ? 0 : y;
            }

            for (var i = 0; i < 360; i += 6)
            {
                var l = new Ellipse
                {
                    Fill = MinorTickStroke,
                    Height = MinorTickStrokeThickness,
                    Width = MinorTickStrokeThickness
                };

                x = Math.Cos(i.Double().GetRadian()) * 50.0;
                y = Math.Sin(i.Double().GetRadian()) * 50.0;

                Canvas.SetLeft(l, x.Round() + (width / 2.0) - (MinorTickStrokeThickness / 2.0));
                Canvas.SetTop(l, y.Round() + (height / 2.0) - (MinorTickStrokeThickness / 2.0));

                Canvas.Children.Add(l);
                minorTicks.Add(l);
            }

            var k = 0;
            for (var i = 0; i < 360; i += (6 * 5))
            {
                if (k == 0)
                {
                    k++;
                    continue;
                }
                else if (k == 3)
                {
                    k = 1;
                    continue;
                }
                else k++;

                var l = new Line
                {
                    Fill = IntermediateTickStroke,
                    Stroke = IntermediateTickStroke,
                    StrokeThickness = IntermediateTickStrokeThickness
                };

                var angle = i.Double().GetRadian();

                x = Math.Cos(angle) * 50.0;
                y = Math.Sin(angle) * 50.0;

                l.X1 = x.Round() + (width / 2.0);
                l.Y1 = y.Round() + (height / 2.0);

                l.X2 = l.X1.Round() - (IntermediateTickLength * Math.Cos(angle));
                l.Y2 = l.Y1.Round() - (IntermediateTickLength * Math.Sin(angle));

                Canvas.Children.Add(l);
                intermediateTicks.Add(l);
            }
        }

        //...

        double X(double i) => (Math.Cos(i.GetRadian()) * 50.0) + (width / 2.0);

        double Y(double i) => (Math.Sin(i.GetRadian()) * 50.0) + (height / 2.0);

        //...

        Point GetPosition(DateTime input)
        {
            var x = Math.Cos(Angle.GetRadian((input.Hour * 30.0) + (input.Minute * 0.5) - 90)) * 50.0;
            var y = Math.Sin(Angle.GetRadian((input.Hour * 30.0) + (input.Minute * 0.5) - 90)) * 50.0;
            return new Point(x + (width / 2.0) - 3, y + (height / 2.0) - 3);
        }

        //...

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            DrawTicks();
        }
    }
}