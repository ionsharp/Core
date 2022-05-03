using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Imagin.Apps.Paint.Effects
{
    /// <summary>An effect that sharpens the input.</summary>
    [Category(ImageEffectCategory.Sharpen), DisplayName("Sharpen")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class SharpenEffect : ImageEffect
    {
        public static readonly DependencyProperty AmountProperty = DependencyProperty.Register("Amount", typeof(double), typeof(SharpenEffect), new FrameworkPropertyMetadata(((double)(1D)), PixelShaderConstantCallback(0)));
        /// <summary>The amount of sharpening.</summary>
        [Hidden(false)]
        [Range(0.0, 2.0, 0.01)]
        [Format(RangeFormat.Both)]
        public double Amount
        {
            get => (double)GetValue(AmountProperty);
            set => SetValue(AmountProperty, value);
        }

        public static readonly DependencyProperty InputSizeProperty = DependencyProperty.Register("InputSize", typeof(Size), typeof(SharpenEffect), new FrameworkPropertyMetadata(new Size(800D, 600D), PixelShaderConstantCallback(1)));
        /// <summary>The size of the input (in pixels).</summary>
        [Hidden(false)]
        public Size InputSize
        {
            get => (Size)GetValue(InputSizeProperty);
            set => SetValue(InputSizeProperty, value);
        }

        public SharpenEffect() : base()
        {
            UpdateShaderValue(AmountProperty);
            UpdateShaderValue(InputSizeProperty);
        }

        public override Color Apply(Color color, double opacity = 1) => color;

        public override ImageEffect Copy()
        {
            return new SharpenEffect()
            {
                Amount = Amount,
                InputSize = InputSize
            };
        }
    }
}
