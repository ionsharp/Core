using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Stroke), DisplayName("Drop shadow")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class DropShadowEffect : ImageEffect
    {
        public DropShadowEffect() : base() { }

        public override Color Apply(Color color, double opacity = 1) => color;

        public override ImageEffect Copy() => new DropShadowEffect();
    }
}