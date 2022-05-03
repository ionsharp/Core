using Imagin.Common;
using Imagin.Common.Colors;
using Imagin.Common.Controls;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Color), DisplayName("Hue")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class HueEffect : TargetImageEffect
    {
        public HueEffect() : base() { }

        public HueEffect(TargetImageEffect input) : base(input) { }

        public HueEffect(double amount, Targets target = Targets.Color) : base(amount, target) { }

        public override Color Apply(Color color, double opacity = 1)
        {
            var rgb = new RGB(color);
            var hsl = HSL.From(rgb);

            return new HSL(0, hsl[1], hsl[2]).Convert(); //(hsl[0] + Hue) % 359;
        }

        public override ImageEffect Copy() => new HueEffect(this);
    }
}