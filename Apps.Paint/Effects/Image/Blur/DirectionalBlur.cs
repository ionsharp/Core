using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows;

namespace Imagin.Apps.Paint.Effects
{
    /// <summary>
    /// An effect that blurs in a single direction.
    /// </summary>
    [Category(ImageEffectCategory.Blur), DisplayName("Directional blur")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class DirectionalBlurEffect : ImageEffect
    {
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register("Angle", typeof(double), typeof(DirectionalBlurEffect), new FrameworkPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        /// <summary>The direction of the blur (in degrees).</summary>
        [Hidden(false)]
        [Range(0.0, 359.0, 1.0)]
        [Format(RangeFormat.Both)]
        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        public static readonly DependencyProperty BlurAmountProperty = DependencyProperty.Register("BlurAmount", typeof(double), typeof(DirectionalBlurEffect), new FrameworkPropertyMetadata(((double)(0.003D)), PixelShaderConstantCallback(1)));
        /// <summary>The scale of the blur (as a fraction of the input size).</summary>
        [Hidden(false)]
        [Range(0.0, 0.01, 0.001)]
        [Format(RangeFormat.Both)]
        public double BlurAmount
        {
            get => (double)GetValue(BlurAmountProperty);
            set => SetValue(BlurAmountProperty, value);
        }

        public DirectionalBlurEffect() : base()
        {
            UpdateShaderValue(AngleProperty);
            UpdateShaderValue(BlurAmountProperty);
        }

        public override ImageEffect Copy()
        {
            return new DirectionalBlurEffect()
            {
                Angle = Angle,
                BlurAmount = BlurAmount
            };
        }
    }
}