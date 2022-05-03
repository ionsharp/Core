using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    /// <summary>An effect that swirls the input in a spiral.</summary>
    [Category(ImageEffectCategory.Distort), DisplayName("Swirl")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class SwirlEffect : ImageEffect
    {
        public static readonly DependencyProperty CenterProperty = DependencyProperty.Register("Center", typeof(Point), typeof(SwirlEffect), new FrameworkPropertyMetadata(new Point(0.5D, 0.5D), PixelShaderConstantCallback(0)));
        /// <summary>The center point of the spiral. (1,1) is lower right corner</summary>
        [Hidden(false)]
        public Point Center
        {
            get => (Point)GetValue(CenterProperty);
            set => SetValue(CenterProperty, value);
        }

        public static readonly DependencyProperty SpiralStrengthProperty = DependencyProperty.Register("SpiralStrength", typeof(double), typeof(SwirlEffect), new FrameworkPropertyMetadata(((double)(10D)), PixelShaderConstantCallback(1)));
        /// <summary>The amount of twist to the spiral.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [Format(RangeFormat.Both)]
        public double SpiralStrength
        {
            get => (double)GetValue(SpiralStrengthProperty);
            set => SetValue(SpiralStrengthProperty, value);
        }

        public static readonly DependencyProperty AspectRatioProperty = DependencyProperty.Register("AspectRatio", typeof(double), typeof(SwirlEffect), new FrameworkPropertyMetadata(((double)(1.5D)), PixelShaderConstantCallback(2)));
        /// <summary>The aspect ratio (width / height) of the input.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [Format(RangeFormat.Both)]
        public double AspectRatio
        {
            get => (double)GetValue(AspectRatioProperty);
            set => SetValue(AspectRatioProperty, value);
        }

        public SwirlEffect() : base()
        {
            UpdateShaderValue(CenterProperty);
            UpdateShaderValue(SpiralStrengthProperty);
            UpdateShaderValue(AspectRatioProperty);
        }

        public override Color Apply(Color color, double opacity = 1) => color;

        public override ImageEffect Copy()
        {
            return new SwirlEffect()
            {
                Center = Center,
                SpiralStrength = SpiralStrength,
                AspectRatio = AspectRatio
            };
        }
    }
}
