using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Colour.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Hexagon : RadioButton
    {
        #region Properties

        /// <summary>
        /// Corresponds to path geometry in XAML.
        /// </summary>
        public const double Radius = 12.0;

        /// <summary>
        /// 
        /// </summary>
        public static readonly double Offset = Radius * 2 * Math.Cos(30d * Math.PI / 180d) + 1.5;

        internal bool Visited;

        Hexagon[] _neighbors = new Hexagon[6];
        internal Hexagon[] Neighbors => _neighbors;

        Color _nominalColor = Color.FromScRgb(1f, 1f, 1f, 1f);
        /// <summary>
        /// 
        /// </summary>
        public Color NominalColor
        {
            get => _nominalColor;
            set
            {
                _nominalColor = value;
                Background = GetBackground(0.5f);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DispositionProperty = DependencyProperty.Register(nameof(Disposition), typeof(ColorDisposition), typeof(Hexagon), new FrameworkPropertyMetadata(ColorDisposition.Solid, OnDispositionChanged));
        /// <summary>
        /// 
        /// </summary>
        public ColorDisposition Disposition
        {
            get => (ColorDisposition)GetValue(DispositionProperty);
            set => SetValue(DispositionProperty, value);
        }
        static void OnDispositionChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            var _element = (Hexagon)element;
            _element.Background = _element.GetBackground(0.5f);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(Hexagon), new FrameworkPropertyMetadata(0.15));
        /// <summary>
        /// 
        /// </summary>
        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }

        #endregion

        #region Constructors

        internal Hexagon() : base() => DefaultStyleKey = typeof(Hexagon);

        internal Hexagon(ColorDisposition disposition) : this()
            => Disposition = disposition;

        #endregion

        #region Methods

        Brush GetBackground(float inflection)
        {
            Color a = NominalColor, z = Color.FromArgb(200, 0, 0, 0);
            Color m = Color.FromScRgb(a.ScA / 2, a.ScR / 2, a.ScG / 2, a.ScB / 2);

            var result = new LinearGradientBrush();

            result.ColorInterpolationMode = ColorInterpolationMode.ScRgbLinearInterpolation;
            result.StartPoint = new Point(0.5, 0);
            result.EndPoint = new Point(0, 0.75);
            result.GradientStops.Add(new GradientStop(a, 0d));
            result.GradientStops.Add(new GradientStop(m, 0.8 * (1d - inflection)));
            result.GradientStops.Add(new GradientStop(z, 1d));

            if (Disposition == ColorDisposition.Gradual)
                return result;

            return new SolidColorBrush(NominalColor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            Background = GetBackground(0.2f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            Background = GetBackground(0.5f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnIsPressedChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsPressedChanged(e);
            if (IsPressed)
                Background = GetBackground(0.8f);
            else Background = GetBackground(0.5f);
        }

        #endregion
    }
}
