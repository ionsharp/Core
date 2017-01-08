using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public class HexagonButton : RadioButton
    {
        #region Properties

        /// <summary>
        /// Corresponds to path geometry in XAML.
        /// </summary>
        public const double Radius = 12d;

        public static readonly double Offset = Radius * 2 * Math.Cos(30d * Math.PI / 180d) + 1.5;

        internal bool Visited;

        HexagonButton[] neighbors;
        internal HexagonButton[] Neighbors
        {
            get
            {
                return neighbors;
            }
        }

        Color nominalColor;
        public Color NominalColor
        {
            get
            {
                return nominalColor;
            }
            set
            {
                nominalColor = value;
                Background = GetBackgroundGradient(0.5f);
            }
        }

        public static DependencyProperty UsesGradientsProperty = DependencyProperty.Register("UsesGradients", typeof(bool), typeof(HexagonButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnUsesGradientsChanged));
        public bool UsesGradients
        {
            get
            {
                return (bool)GetValue(UsesGradientsProperty);
            }
            set
            {
                SetValue(UsesGradientsProperty, value);
            }
        }
        static void OnUsesGradientsChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            HexagonButton HexagonButton = (HexagonButton)Object;
            HexagonButton.Background = HexagonButton.GetBackgroundGradient(0.5f);
        }

        public static DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(double), typeof(HexagonButton), new FrameworkPropertyMetadata(0.15, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double StrokeThickness
        {
            get
            {
                return (double)GetValue(StrokeThicknessProperty);
            }
            set
            {
                SetValue(StrokeThicknessProperty, value);
            }
        }

        #endregion

        #region HexagonButton

        internal HexagonButton() : base()
        {
            this.OnInitialized();
        }

        internal HexagonButton(bool UsesGradients) : base()
        {
            this.OnInitialized();
            this.UsesGradients = UsesGradients;
        }

        #endregion

        #region Methods

        Brush GetBackgroundGradient(float inflection)
        {
            Color a = NominalColor, z = Color.FromArgb(200, 0, 0, 0);
            Color m = Color.FromScRgb(a.ScA / 2, a.ScR / 2, a.ScG / 2, a.ScB / 2);

            LinearGradientBrush lgb = new LinearGradientBrush();
            lgb.ColorInterpolationMode = ColorInterpolationMode.ScRgbLinearInterpolation;
            lgb.StartPoint = new Point(0.5, 0);
            lgb.EndPoint = new Point(0, 0.75);
            lgb.GradientStops.Add(new GradientStop(a, 0d));
            lgb.GradientStops.Add(new GradientStop(m, 0.8 * (1d - inflection)));
            lgb.GradientStops.Add(new GradientStop(z, 1d));

            if (this.UsesGradients)
                return lgb;

            return new SolidColorBrush(NominalColor);
        }

        protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            Background = GetBackgroundGradient(0.2f);
        }

        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            Background = GetBackgroundGradient(0.5f);
        }

        protected override void OnIsPressedChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsPressedChanged(e);
            if (IsPressed)
                Background = GetBackgroundGradient(0.8f);
            else Background = GetBackgroundGradient(0.5f);
        }

        protected virtual void OnInitialized()
        {
            this.DefaultStyleKey = typeof(HexagonButton);
            neighbors = new HexagonButton[6];
            nominalColor = Color.FromScRgb(1f, 1f, 1f, 1f);
        }

        #endregion
    }
}
