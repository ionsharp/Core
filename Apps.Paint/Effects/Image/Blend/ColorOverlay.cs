using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Blend), DisplayName("Color overlay")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class ColorOverlayEffect : BlendImageEffect
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(ColorOverlayEffect), new FrameworkPropertyMetadata(Color.FromArgb(255, 0, 0, 0), PixelShaderConstantCallback(2)));
        [Visible]
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public ColorOverlayEffect() : base()
        {
            UpdateShaderValue(ColorProperty);
        }

        public override Color Apply(Color color, double opacity = 1) => color.Blend(Color, ActualBlendMode);

        public override ImageEffect Copy() => new ColorOverlayEffect() { Color = Color };
    }
}