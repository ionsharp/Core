using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    /// <summary>An effect that pinches a circular region.</summary>
    [Category(ImageEffectCategory.Distort), DisplayName("Pinch")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class PinchEffect : ImageEffect
    {
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(PinchEffect), new FrameworkPropertyMetadata(new Point(0.5D, 0.5D), PixelShaderConstantCallback(0)));
        /// <summary>The center point of the pinched region.</summary>
        [Hidden(false)]
        public Point Center
        {
            get => (Point)GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(PinchEffect), new FrameworkPropertyMetadata(((double)(0.25D)), PixelShaderConstantCallback(1)));
        /// <summary>The radius of the pinched region.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [Format(RangeFormat.Both)]
        public double Radius
        {
            get => (double)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        public static readonly DependencyProperty StrengthProperty = DependencyProperty.Register("Strength", typeof(double), typeof(PinchEffect), new FrameworkPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(2)));
        /// <summary>The strength of the pinch effect.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [Format(RangeFormat.Both)]
        public double Strength
        {
            get => (double)GetValue(StrengthProperty);
            set => SetValue(StrengthProperty, value);
        }

        public static readonly DependencyProperty AspectRatioProperty = DependencyProperty.Register("AspectRatio", typeof(double), typeof(PinchEffect), new FrameworkPropertyMetadata(((double)(1.5D)), PixelShaderConstantCallback(3)));
        /// <summary>The aspect ratio (width / height) of the input.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [Format(RangeFormat.Both)]
        public double AspectRatio
        {
            get => (double)GetValue(AspectRatioProperty);
            set => SetValue(AspectRatioProperty, value);
        }

        public PinchEffect() : base()
        {
            UpdateShaderValue(CenterProperty);
            UpdateShaderValue(RadiusProperty);
            UpdateShaderValue(StrengthProperty);
            UpdateShaderValue(AspectRatioProperty);
        }

        public override Color Apply(Color color, double opacity = 1) => color;

        public override ImageEffect Copy()
        {
            return new PinchEffect()
            {
                Center = Center,
                Radius = Radius,
                Strength = Strength,
                AspectRatio = AspectRatio
            };
        }
    }
}
