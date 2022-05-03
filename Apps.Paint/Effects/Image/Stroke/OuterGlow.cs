using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Stroke), DisplayName("Outer glow")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class OuterGlowEffect : ImageEffect
    {
        public OuterGlowEffect() : base() { }

        public override Color Apply(Color color, double opacity = 1) => color;

        public override ImageEffect Copy() => new OuterGlowEffect();
    }
}