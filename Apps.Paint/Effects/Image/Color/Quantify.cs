using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Color), DisplayName("Quantify")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class QuantifyEffect : ImageEffect
    {
        public static readonly DependencyProperty SizeXProperty = DependencyProperty.Register(nameof(SizeX), typeof(double), typeof(QuantifyEffect), new FrameworkPropertyMetadata(10d, PixelShaderConstantCallback(0)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 1000.0, 1.0)]
        [Visible]
        public double SizeX
        {
            get => (double)GetValue(SizeXProperty);
            set => SetValue(SizeXProperty, value);
        }

        public static readonly DependencyProperty SizeYProperty = DependencyProperty.Register(nameof(SizeY), typeof(double), typeof(QuantifyEffect), new FrameworkPropertyMetadata(10d, PixelShaderConstantCallback(1)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 1000.0, 1.0)]
        [Visible]
        public double SizeY
        {
            get => (double)GetValue(SizeYProperty);
            set => SetValue(SizeYProperty, value);
        }

        public static readonly DependencyProperty SizeZProperty = DependencyProperty.Register(nameof(SizeZ), typeof(double), typeof(QuantifyEffect), new FrameworkPropertyMetadata(10d, PixelShaderConstantCallback(2)));
        [Format(RangeFormat.Both)]
        [Range(0.0, 1000.0, 1.0)]
        [Visible]
        public double SizeZ
        {
            get => (double)GetValue(SizeZProperty);
            set => SetValue(SizeZProperty, value);
        }

        public QuantifyEffect() : base()
        {
            UpdateShaderValue(SizeXProperty);
            UpdateShaderValue(SizeYProperty);
            UpdateShaderValue(SizeZProperty);
        }

        public override Color Apply(Color color, double opacity = 1) => default;

        public override ImageEffect Copy() => new QuantifyEffect();
    }
}