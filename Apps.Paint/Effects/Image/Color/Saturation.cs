using Imagin.Common;
using Imagin.Common.Colors;
using Imagin.Common.Controls;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Color), DisplayName("Saturation")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class SaturationEffect : TargetImageEffect
    {
        public SaturationEffect() : base() { }

        public SaturationEffect(TargetImageEffect input) : base(input) { }

        public SaturationEffect(double amount, Targets target = Targets.Color) : base(amount, target) { }

        public override Color Apply(Color color, double opacity = 1)
        {
            var rgb = new RGB(color);
            var hsl = HSL.From(rgb);

            return new HSL(hsl[0], 0, hsl[2]).Convert(); //(hsl[1] + Saturation).Coerce(100);
        }

        public override ImageEffect Copy() => new SaturationEffect(this);
    }
}