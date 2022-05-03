using Imagin.Common;
using Imagin.Common.Colors;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using System.Windows.Media;

namespace Imagin.Apps.Paint.Effects
{
    [Category(ImageEffectCategory.Color), DisplayName("Brightness")]
    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class BrightnessEffect : TargetImageEffect
    {
        public BrightnessEffect() : base() { }

        public BrightnessEffect(TargetImageEffect input) : base(input) { }

        public BrightnessEffect(double amount, Targets target = Targets.Color) : base(amount, target) { }

        public override Color Apply(Color color, double opacity = 1)
        {
            var c = color.Int32();
            var hue
                = c.GetHue().Double();
            var saturation
                = c.GetSaturation().Double();
            var brightness
                = c.GetBrightness().Double();

            var r = c.R;
            var g = c.G;
            var b = c.B;

            //Figure out which component is most dominant and increase or decrease appropriately
            var oldBrightness = brightness;

            //Yellow
            if (r > b && g > b)
            {
                brightness += (oldBrightness * Yellow.Double().Shift(-2));
            }
            //Magenta
            else if (r > g && b > g)
            {
                brightness += (oldBrightness * Magenta.Double().Shift(-2));
            }
            //Cyan
            else if (g > r && b > r)
            {
                brightness += (oldBrightness * Cyan.Double().Shift(-2));
            }

            //Red
            if (c.R > c.G && c.R > c.B)
            {
                brightness += (oldBrightness * Red.Double().Shift(-2));
            }
            //Green
            else if (c.G > c.R && c.G > c.B)
            {
                brightness += (oldBrightness * Green.Double().Shift(-2));
            }
            //Blue
            else if (c.B > c.R && c.B > c.G)
            {
                brightness += (oldBrightness * Blue.Double().Shift(-2));
            }

            var hsb = new HSB(hue, saturation, brightness.Coerce(1));
            var rgb = hsb.Convert();
            return Color.FromArgb(color.A, rgb[0].Shift(2).Multiply(255), rgb[1].Shift(2).Multiply(255), rgb[2].Shift(2).Multiply(255));
        }

        /*
        public override Color Apply(Color color, double opacity = 1)
        {
            var c = color.Int32();

            //The following two components are used later for calculating the new color
            var hue
                = c.GetHue().Double();
            var saturation
                = c.GetSaturation().Double();

            //This determines the tonal range
            var brightness
                = c.GetBrightness().Double();

            //It's a shadow!
            if (brightness <= ShadowRange.Double().Shift(-2))
            {
                //If it is a shadow, increase the brightness of the color by a percentage
                brightness += brightness * ShadowAmount.Double().Shift(-2);
            }
            //It's a highlight!
            else if (brightness >= 1 - HighlightRange.Double().Shift(-2))
            {
                //If it is a highlight, decrease the brightness of the color by a percentage
                brightness -= brightness * HighlightAmount.Double().Shift(-2);
            }
            //It's an unaffected midtone!
            else return color;

            var hsb = new Color<HSB>(hue, saturation, brightness);
            var rgb = HSB.From(hsb);

            return Color.FromArgb(color.A, rgb[0].Shift(2).Multiply(255), rgb[1].Shift(2).Multiply(255), rgb[2].Shift(2).Multiply(255));
        }
        */

        public override ImageEffect Copy() => new BrightnessEffect(this);
    }
}