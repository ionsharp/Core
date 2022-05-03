using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows;

namespace Imagin.Apps.Paint.Effects
{
    /// <summary>An effect that embosses the input.</summary>
    [Category(ImageEffectCategory.Stroke), DisplayName("Emboss")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class EmbossEffect : ImageEffect
    {
        public static readonly DependencyProperty AmountProperty = DependencyProperty.Register("Amount", typeof(double), typeof(EmbossEffect), new FrameworkPropertyMetadata(((double)(0.5D)), PixelShaderConstantCallback(0)));
        /// <summary>The amplitude of the embossing.</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [Format(RangeFormat.Both)]
        public double Amount
        {
            get => (double)GetValue(AmountProperty);
            set => SetValue(AmountProperty, value);
        }

        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(double), typeof(EmbossEffect), new FrameworkPropertyMetadata(((double)(0.003D)), PixelShaderConstantCallback(1)));
        /// <summary>The separation between samples (as a fraction of input size).</summary>
        [Hidden(false)]
        [Range(0.0, 1.0, 0.01)]
        [Format(RangeFormat.Both)]
        public double Width
        {
            get => (double)GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }

        public EmbossEffect() : base()
        {
            UpdateShaderValue(AmountProperty);
            UpdateShaderValue(WidthProperty);
        }

        public override ImageEffect Copy()
        {
            return new EmbossEffect()
            {
                Amount = Amount,
                Width = Width
            };
        }
    }
}
