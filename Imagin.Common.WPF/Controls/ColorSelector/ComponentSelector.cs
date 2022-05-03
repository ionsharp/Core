using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    #region ComponentSelector

    public class ComponentSelector : ColorSelector
    {
        public static readonly DependencyProperty EllipseDiameterProperty = DependencyProperty.Register(nameof(EllipseDiameter), typeof(double), typeof(ComponentSelector), new FrameworkPropertyMetadata(12.0));
        public double EllipseDiameter
        {
            get => (double)GetValue(EllipseDiameterProperty);
            set => SetValue(EllipseDiameterProperty, value);
        }

        public static readonly DependencyProperty EllipsePositionProperty = DependencyProperty.Register(nameof(EllipsePosition), typeof(Point2D), typeof(ComponentSelector), new FrameworkPropertyMetadata(null));
        public Point2D EllipsePosition
        {
            get => (Point2D)GetValue(EllipsePositionProperty);
            private set => SetValue(EllipsePositionProperty, value);
        }

        public static readonly DependencyProperty XProperty = DependencyProperty.Register(nameof(X), typeof(One), typeof(ComponentSelector), new FrameworkPropertyMetadata(One.Zero));
        public One X
        {
            get => (One)GetValue(XProperty);
            set => SetValue(XProperty, value);
        }

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(nameof(Y), typeof(One), typeof(ComponentSelector), new FrameworkPropertyMetadata(One.Zero));
        public One Y
        {
            get => (One)GetValue(YProperty);
            set => SetValue(YProperty, value);
        }

        public static readonly DependencyProperty ZProperty = DependencyProperty.Register(nameof(Z), typeof(One), typeof(ComponentSelector), new FrameworkPropertyMetadata(One.Zero, OnZChanged));
        public One Z
        {
            get => (One)GetValue(ZProperty);
            set => SetValue(ZProperty, value);
        }
        static void OnZChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as ComponentSelector).OnZChanged(new Value<One>(e));

        //...

        public ComponentSelector() : base()
        {
            SetCurrentValue(EllipsePositionProperty, new Point2D(0, 0));
        }

        //...

        /// <remarks>
        /// Rounding avoids "shaking" when changing quickly between two really close values. This is particularly evident with <see cref="Media.Models.HSB"/> and similarly complex models.
        /// </remarks>
        protected override void Mark()
        {
            EllipsePosition.X = ((X * ActualWidth) - (EllipseDiameter / 2.0)).Round();
            EllipsePosition.Y = ((ActualHeight - (Y * ActualHeight)) - (EllipseDiameter / 2.0)).Round();
        }

        protected override void OnMouseChanged(Vector2<One> input)
        {
            base.OnMouseChanged(input);
            SetCurrentValue(XProperty, input.X);
            SetCurrentValue(YProperty, input.Y);
        }

        protected virtual void OnZChanged(Value<One> input)
        {
            Mark();
        }
    }

    #endregion

    #region ComponentSelectorEllipse

    public class ComponentSelectorEllipse : FrameworkElement
    {
        public static readonly DependencyProperty DiameterProperty = DependencyProperty.Register(nameof(Diameter), typeof(double), typeof(ComponentSelectorEllipse), new FrameworkPropertyMetadata(12.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double Diameter
        {
            get => (double)GetValue(DiameterProperty);
            set => SetValue(DiameterProperty, value);
        }

        public static readonly DependencyProperty XProperty = DependencyProperty.Register(nameof(X), typeof(double), typeof(ComponentSelectorEllipse), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double X
        {
            get => (double)GetValue(XProperty);
            set => SetValue(XProperty, value);
        }

        public static readonly DependencyProperty YProperty = DependencyProperty.Register(nameof(Y), typeof(double), typeof(ComponentSelectorEllipse), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));
        public double Y
        {
            get => (double)GetValue(YProperty);
            set => SetValue(YProperty, value);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var radius = Diameter / 2; var x = X + radius; var y = Y + radius;
            drawingContext.DrawEllipse(null, new Pen(Brushes.Black, 1), new(x, y), radius, radius);
            drawingContext.DrawEllipse(null, new Pen(Brushes.White, 1), new(x, y), radius - 1, radius - 1);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            InvalidateVisual();
        }
    }

    #endregion
}