using Imagin.Common;
using Imagin.Common.Controls;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Stroke), DisplayName("Satin")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class SatinEffect : ImageEffect
    {
        double angle;
        public double Angle
        {
            get; set;
        }

        bool antiAliased = false;
        public bool AntiAliased
        {
            get; set;
        }

        object contour;
        public object Contour
        {
            get; set;
        }

        double distance = 11;
        public double Distance
        {
            get; set;
        }

        bool invert = true;
        public bool Invert
        {
            get; set;
        }

        double size = 14;
        public double Size
        {
            get; set;
        }

        public SatinEffect() : base() { }

        public override Color Apply(Color color, double opacity = 1) => color;

        public override ImageEffect Copy() => new SatinEffect();
    }
}