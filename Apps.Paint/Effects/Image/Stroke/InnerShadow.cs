using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Stroke), DisplayName("Inner shadow")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class InnerShadowEffect : ImageEffect
    {
        public InnerShadowEffect() : base() { }

        public override Color Apply(Color color, double opacity = 1) => color;

        public override ImageEffect Copy() => new InnerShadowEffect();
    }
}