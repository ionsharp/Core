using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    [TemplatePart(Name = nameof(PART_Grid), Type = typeof(Grid))]
    public class PatternControl : Control
    {
        Grid PART_Grid = null;

        public sealed class Dot : Base
        {
            bool isConnected;
            public bool IsConnected
            {
                get => isConnected;
                set => this.Change(ref isConnected, value);
            }

            Point position;
            public Point Position
            {
                get => position;
                set => this.Change(ref position, value);
            }

            public Dot() : base() { }

            public Dot(Point position) : base()
            {
                Position = position;
            }
        }

        public sealed class Line : MLine<double>
        {
            bool isOpen;
            public bool IsOpen
            {
                get => isOpen;
                set => this.Change(ref isOpen, value);
            }

            public Line() : base() { }

            public Line(bool isOpen, Line<int> point) : this(isOpen, point.X1, point.Y1, point.X2, point.Y2) { }

            public Line(bool isOpen, double x12, double y12) : this(isOpen, x12, y12, x12, y12) { }

            public Line(bool isOpen, double x1, double y1, double x2, double y2) : this()
            {
                IsOpen = isOpen;
                X1 = x1;
                Y1 = y1;
                X2 = x2;
                Y2 = y2;
            }
        }

        #region Properties

        Line currentLine;

        Int32Pattern currentPattern;

        bool isDrawing = false;

        readonly Handle handle = false;

        public event EventHandler<EventArgs> Drawn;

        public static readonly DependencyProperty CanvasLengthProperty = DependencyProperty.Register(nameof(CanvasLength), typeof(double), typeof(PatternControl), new FrameworkPropertyMetadata(255d, OnCanvasLengthChanged));
        public double CanvasLength
        {
            get => (double)GetValue(CanvasLengthProperty);
            set => SetValue(CanvasLengthProperty, value);
        }
        static void OnCanvasLengthChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<PatternControl>().Refresh();

        public static readonly DependencyProperty ClosedLineStrokeProperty = DependencyProperty.Register(nameof(ClosedLineStroke), typeof(Brush), typeof(PatternControl), new FrameworkPropertyMetadata(Brushes.Green));
        public Brush ClosedLineStroke
        {
            get => (Brush)GetValue(ClosedLineStrokeProperty);
            set => SetValue(ClosedLineStrokeProperty, value);
        }

        public static readonly DependencyProperty DotBackgroundProperty = DependencyProperty.Register(nameof(DotBackground), typeof(Brush), typeof(PatternControl), new FrameworkPropertyMetadata(default(Brush)));
        public Brush DotBackground
        {
            get => (Brush)GetValue(DotBackgroundProperty);
            set => SetValue(DotBackgroundProperty, value);
        }

        public static readonly DependencyProperty DotBorderBrushProperty = DependencyProperty.Register(nameof(DotBorderBrush), typeof(Brush), typeof(PatternControl), new FrameworkPropertyMetadata(default(Brush)));
        public Brush DotBorderBrush
        {
            get => (Brush)GetValue(DotBorderBrushProperty);
            set => SetValue(DotBorderBrushProperty, value);
        }

        public static readonly DependencyProperty DotBorderThicknessProperty = DependencyProperty.Register(nameof(DotBorderThickness), typeof(Thickness), typeof(PatternControl), new FrameworkPropertyMetadata(default(Thickness)));
        public Thickness DotBorderThickness
        {
            get => (Thickness)GetValue(DotBorderThicknessProperty);
            set => SetValue(DotBorderThicknessProperty, value);
        }

        public static readonly DependencyProperty DotLengthProperty = DependencyProperty.Register(nameof(DotLength), typeof(double), typeof(PatternControl), new FrameworkPropertyMetadata(48d, OnDotLengthChanged));
        public double DotLength
        {
            get => (double)GetValue(DotLengthProperty);
            set => SetValue(DotLengthProperty, value);
        }
        static void OnDotLengthChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<PatternControl>().Refresh();

        public static readonly DependencyProperty DotsProperty = DependencyProperty.Register(nameof(Dots), typeof(ObservableCollection<Dot>), typeof(PatternControl), new FrameworkPropertyMetadata(null));
        public ObservableCollection<Dot> Dots
        {
            get => (ObservableCollection<Dot>)GetValue(DotsProperty);
            private set => SetValue(DotsProperty, value);
        }

        public static readonly DependencyProperty InnerDotBackgroundProperty = DependencyProperty.Register(nameof(InnerDotBackground), typeof(Brush), typeof(PatternControl), new FrameworkPropertyMetadata(default(Brush)));
        public Brush InnerDotBackground
        {
            get => (Brush)GetValue(InnerDotBackgroundProperty);
            set => SetValue(InnerDotBackgroundProperty, value);
        }

        public static readonly DependencyProperty InnerDotConnectedBackgroundProperty = DependencyProperty.Register(nameof(InnerDotConnectedBackground), typeof(Brush), typeof(PatternControl), new FrameworkPropertyMetadata(default(Brush)));
        public Brush InnerDotConnectedBackground
        {
            get => (Brush)GetValue(InnerDotConnectedBackgroundProperty);
            set => SetValue(InnerDotConnectedBackgroundProperty, value);
        }

        public static readonly DependencyProperty InnerDotLengthProperty = DependencyProperty.Register(nameof(InnerDotLength), typeof(double), typeof(PatternControl), new FrameworkPropertyMetadata(28d, OnInnerDotLengthChanged));
        public double InnerDotLength
        {
            get => (double)GetValue(InnerDotLengthProperty);
            set => SetValue(InnerDotLengthProperty, value);
        }
        static void OnInnerDotLengthChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<PatternControl>().Refresh();

        public static readonly DependencyProperty IsDrawingEnabledProperty = DependencyProperty.Register(nameof(IsDrawingEnabled), typeof(bool), typeof(PatternControl), new FrameworkPropertyMetadata(true));
        public bool IsDrawingEnabled
        {
            get => (bool)GetValue(IsDrawingEnabledProperty);
            set => SetValue(IsDrawingEnabledProperty, value);
        }

        public static readonly DependencyProperty LineStrokeProperty = DependencyProperty.Register(nameof(LineStroke), typeof(double), typeof(PatternControl), new FrameworkPropertyMetadata(10d));
        public double LineStroke
        {
            get => (double)GetValue(LineStrokeProperty);
            set => SetValue(LineStrokeProperty, value);
        }

        public static readonly DependencyProperty OpenLineStrokeProperty = DependencyProperty.Register(nameof(OpenLineStroke), typeof(Brush), typeof(PatternControl), new FrameworkPropertyMetadata(Brushes.LightGray));
        public Brush OpenLineStroke
        {
            get => (Brush)GetValue(OpenLineStrokeProperty);
            set => SetValue(OpenLineStrokeProperty, value);
        }

        public static readonly DependencyProperty LinesProperty = DependencyProperty.Register(nameof(Lines), typeof(ObservableCollection<Line>), typeof(PatternControl), new FrameworkPropertyMetadata(null));
        public ObservableCollection<Line> Lines
        {
            get => (ObservableCollection<Line>)GetValue(LinesProperty);
            private set => SetValue(LinesProperty, value);
        }

        public static readonly DependencyProperty PatternProperty = DependencyProperty.Register(nameof(Pattern), typeof(Int32Pattern), typeof(PatternControl), new FrameworkPropertyMetadata(null, OnPatternChanged));
        public Int32Pattern Pattern
        {
            get => (Int32Pattern)GetValue(PatternProperty);
            set => SetValue(PatternProperty, value);
        }
        static void OnPatternChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<PatternControl>().OnPatternChanged((Int32Pattern)e.NewValue);

        #endregion

        #region PatternControl

        public PatternControl() : base()
        {
            //SetCurrentValue(PatternProperty, new());

            Dots 
                = new ObservableCollection<Dot>();
            Lines 
                = new ObservableCollection<Line>();

            Refresh();
        }

        #endregion

        #region Methods

        bool AlreadyConnected(Point point)
        {
            var result = false;
            foreach (var i in Lines)
            {
                if (i != currentLine)
                {
                    if
                    (
                        (i.X1 == point.X && i.Y1 == point.Y)
                        ||
                        (i.X2 == point.X && i.Y2 == point.Y)
                    )
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        void ConnectDot(Line<int> point)
        {
            foreach (var i in Dots)
            {
                if
                (
                    !i.IsConnected
                    &&
                    (
                        (i.Position.X == point.X1 && i.Position.Y == point.Y1)
                        ||
                        (i.Position.X == point.X2 && i.Position.Y == point.Y2)
                    )
                )
                {
                    i.IsConnected = true;
                }
            }
        }

        Point GetPoint(int length, Point point)
        {
            var x = 0;
            var y = 0;

            var lengthHalf = length / 2;

            var action = new Func<double, int>(value =>
            {
                if (value >= 0 && value < length)
                {
                    return lengthHalf;
                }
                else if (value >= length && value < length * 2)
                {
                    return length + lengthHalf;
                }
                else if (value >= length * 2 && value < length * 3)
                    return (length * 2) + lengthHalf;

                return 0;
            });

            x = action(point.X);
            y = action(point.Y);

            return new Point(x, y);
        }

        Point? GetPoint(int length, int hotLength, Point point)
        {
            int? x = 0;
            int? y = 0;

            var lengthHalf = length / 2;

            var hotLengthStart = lengthHalf - (hotLength / 2);
            var hotLengthEnd = hotLengthStart + hotLength;

            var action = new Func<double, int?>(value =>
            {
                if (value >= hotLengthStart && value < hotLengthEnd)
                {
                    return lengthHalf;
                }
                else if (value >= length + hotLengthStart && value < length + hotLengthEnd)
                {
                    return length + lengthHalf;
                }
                else if (value >= length * 2 + hotLengthStart && value < length * 2 + hotLengthEnd)
                    return (length * 2) + lengthHalf;

                return null;
            });

            x = action(point.X);
            y = action(point.Y);

            if (x == null || y == null)
                return null;

            return new Point(x.Value, y.Value);
        }

        void Refresh()
        {
            Dots.Clear();
            var length = (CanvasLength / 3).Int32();

            var x = length / 2;
            var y = length / 2;

            for (var i = 0; i < 9; i++)
            {
                Dots.Add(new Dot(new Point(x, y)));
                if (x == length * 2 + (length / 2))
                {
                    x = length / 2;
                    y += length;
                }
                else x += length;
            }
        }

        //...

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (IsDrawingEnabled)
            {
                var result = e.GetPosition(PART_Grid);
                OnDrawingStarted(result);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed && isDrawing)
            {
                var result = e.GetPosition(PART_Grid);
                OnDrawing(result);
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (isDrawing)
                OnDrawingEnded();
        }

        //...

        protected virtual void OnDrawing(Point MousePosition)
        {
            currentLine.X2 = MousePosition.X;
            currentLine.Y2 = MousePosition.Y;

            var targetPoint = GetPoint((CanvasLength / 3).Int32(), InnerDotLength.Int32(), MousePosition);
            if (targetPoint != null)
            {
                if (!AlreadyConnected(targetPoint.Value))
                {
                    var linearPoint = new Line<int>(currentLine.X1.Int32(), currentLine.Y1.Int32(), targetPoint.Value.X.Int32(), targetPoint.Value.Y.Int32());

                    currentLine.X2 = linearPoint.X2;
                    currentLine.Y2 = linearPoint.Y2;
                    currentLine.IsOpen = false;

                    ConnectDot(linearPoint);

                    currentPattern?.Add(linearPoint);

                    currentLine = new Line(true, linearPoint.X2, linearPoint.Y2, linearPoint.X2, linearPoint.Y2);
                    Lines.Add(currentLine);
                }
            }
        }

        protected virtual void OnDrawingEnded()
        {
            if (currentLine != null)
            {
                Lines.Remove(currentLine);
                currentLine = null;
            }
            if (currentPattern != null)
            {
                handle.Invoke(() =>
                {
                    SetCurrentValue(PatternProperty, currentPattern);
                    currentPattern = null;
                });
            }

            isDrawing = false;
            Drawn?.Invoke(this, new EventArgs());
        }

        protected virtual void OnDrawingStarted(Point MousePosition)
        {
            isDrawing = true;

            currentPattern = new Int32Pattern();
            Reset();

            var point = GetPoint((CanvasLength / 3).Int32(), MousePosition);

            currentLine = new Line(true, point.X, point.Y);
            Lines.Add(currentLine);
        }

        //...

        protected virtual void OnPatternChanged(Int32Pattern input)
        {
            handle.SafeInvoke(() =>
            {
                Reset();
                foreach (var i in input ?? Int32Pattern.Empty)
                {
                    Lines.Add(new Line(false, i));
                    ConnectDot(i);
                }
            });
        }

        //...

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_Grid = Template.FindName(nameof(PART_Grid), this) as Grid;
        }

        public void Reset()
        {
            Lines.Clear();
            Dots.ForEach(i => i.IsConnected = false);
        }

        #endregion
    }
}