using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    /// <summary>An effect that applies a radial blur to the input.</summary>
    [Category(ImageEffectCategory.Blur), DisplayName("Zoom blur")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class ZoomBlurEffect : ImageEffect
    {
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(ZoomBlurEffect), new FrameworkPropertyMetadata(new Point(0.9D, 0.6D), PixelShaderConstantCallback(0)));
        /// <summary>The center of the blur.</summary>
        [Hidden(false)]
        public Point Center
        {
            get => (Point)GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }
        
        public static readonly DependencyProperty BlurAmountProperty = DependencyProperty.Register("BlurAmount", typeof(double), typeof(ZoomBlurEffect), new FrameworkPropertyMetadata(((double)(0.1D)), PixelShaderConstantCallback(1)));
        /// <summary>The amount of blur.</summary>
        [DisplayName("Amount")]
        [Hidden(false)]
        [Range(0.0, 0.2, 0.01)]
        [Format(RangeFormat.Both)]
        public double BlurAmount
        {
            get => (double)GetValue(BlurAmountProperty);
            set => SetValue(BlurAmountProperty, value);
        }

        public ZoomBlurEffect() : base()
        {
            UpdateShaderValue(CenterProperty);
            UpdateShaderValue(BlurAmountProperty);
        }

        public override Color Apply(Color color, double opacity = 1) => color;

        public override ImageEffect Copy()
        {
            return new ZoomBlurEffect()
            {
                Center = Center,
                BlurAmount = BlurAmount
            };
        }
    }
}