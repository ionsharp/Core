using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Stroke), DisplayName("Inner glow")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class InnerGlowEffect : ImageEffect
    {
        public InnerGlowEffect() : base() { }

        public override Color Apply(Color color, double opacity = 1) => color;

        public override ImageEffect Copy() => new InnerGlowEffect();
    }
}