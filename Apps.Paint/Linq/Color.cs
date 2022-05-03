using Imagin.Common.Colors;
using Imagin.Common.Linq;
using System;
using System.Windows.Media;

namespace Imagin.Apps.Paint
{
    public static class XColor
    {
        static double BlendColorBurnf(double a, double b)
            => ((b == 0.0) ? b : Math.Max((1.0 - ((1.0 - a) / b)), 0.0));

        static double BlendColorDodgef(double a, double b)
            => ((b == 1.0) ? b : Math.Min(a / (1.0 - b), 1.0));

        static double BlendHardMixf(double a, double b) 
            => BlendVividLightf(a, b) < 0.5 ? 0 : 1;

        static double BlendReflectf(double a, double b) 
            => (b == 1.0) ? b : Math.Min(a * a / (1.0 - b), 1.0);

        static double BlendSoftLightf(double a, double b) 
            => ((b < 0.5) ? (2.0 * a * b + a * a * (1.0 - 2.0 * b)) : (Math.Sqrt(a) * (2.0 * b - 1.0) + 2.0 * a * (1.0 - b)));

        static double BlendVividLightf(double a, double b) 
            => b < 0.5 ? BlendColorBurnf(a, 2.0 * b) : BlendColorDodgef(a, 2.0 * (b - 0.5));

        public static Color Blend(this Color a, Color b, BlendModes blendMode = BlendModes.Normal, double amount = 1)
        {
            b = b.A((b.A.Double() / 255d * amount * 255d).Byte());

            double a1 = a.A.Divide(255.0), r1 = a.R.Divide(255.0), g1 = a.G.Divide(255.0), b1 = a.B.Divide(255.0),
                a2 = b.A.Divide(255.0), r2 = b.R.Divide(255.0), g2 = b.G.Divide(255.0), b2 = b.B.Divide(255.0);

            double a3 = 0, r3 = 0, g3 = 0, b3 = 0;

            HSB hsb1 = null, hsb2 = null;
            RGB rgb = null;

            a3 = a1;
            switch (blendMode)
            {
                #region Average
                case BlendModes.Average:
                    a3 = (a1 + a2) / 2;
                    r3 = (r1 + r2) / 2;
                    g3 = (g1 + g2) / 2;
                    b3 = (b1 + b2) / 2;
                    break;
                #endregion
                #region Color
                case BlendModes.Color:
                    hsb1 = HSB.From(new RGB(r1, g1, b1));
                    hsb2 = HSB.From(new RGB(r2, g2, b2));
                    rgb = new HSB(hsb2[0], hsb2[1], hsb1[2]).Convert();

                    r3 = rgb[0];
                    g3 = rgb[1];
                    b3 = rgb[2];
                    break;
                #endregion
                #region ColorBurn
                case BlendModes.ColorBurn:
                    //R = 1 - (1 - a) / b
                    r3 = (1 - (1 - r1) / r2).Coerce(1);
                    g3 = (1 - (1 - g1) / g2).Coerce(1);
                    b3 = (1 - (1 - b1) / b2).Coerce(1);
                    break;
                #endregion
                #region ColorDodge
                case BlendModes.ColorDodge:
                    //R = a / (1 - b)
                    r3 = BlendColorDodgef(r1, r2);
                    g3 = BlendColorDodgef(g1, g2);
                    b3 = BlendColorDodgef(b1, b2);
                    break;
                #endregion
                #region Darken
                case BlendModes.Darken:
                    r3 = r1 < r2 ? r1 : r2;
                    g3 = g1 < g2 ? g1 : g2;
                    b3 = b1 < b2 ? b1 : b2;
                    break;
                #endregion
                #region Difference
                case BlendModes.Difference:
                    r3 = (r1 - r2).Absolute();
                    g3 = (g1 - g2).Absolute();
                    b3 = (b1 - b2).Absolute();
                    break;
                #endregion
                #region Exclusion
                case BlendModes.Exclusion:
                    r3 = r1 + r2 - (2 * r1 * r2);
                    g3 = g1 + g2 - (2 * g1 * g2);
                    b3 = b1 + b2 - (2 * b1 * b2);
                    break;
                #endregion
                #region Glow
                case BlendModes.Glow:
                    r3 = BlendReflectf(r2, r1);
                    g3 = BlendReflectf(g2, g1);
                    b3 = BlendReflectf(b2, b1);
                    break;
                #endregion
                #region HardLight
                case BlendModes.HardLight:
                    r3 = r1 < 0.5 ? r1 * r2 : (r1 <= 1 || r2 <= 1 ? r1 + r2 - (r1 * r2) : Math.Max(r1, r2));
                    g3 = g1 < 0.5 ? g1 * g2 : (g1 <= 1 || g2 <= 1 ? g1 + g2 - (g1 * g2) : Math.Max(g1, g2));
                    b3 = b1 < 0.5 ? b1 * b2 : (b1 <= 1 || b2 <= 1 ? b1 + b2 - (b1 * b2) : Math.Max(b1, b2));
                    break;
                #endregion
                #region HardMix
                case BlendModes.HardMix:
                    r3 = BlendHardMixf(r1, r2);
                    g3 = BlendHardMixf(g1, g2);
                    b3 = BlendHardMixf(b1, b2);
                    break;
                #endregion
                #region Hue
                case BlendModes.Hue:
                    hsb1 = HSB.From(new RGB(r1, g1, b1));
                    hsb2 = HSB.From(new RGB(r2, g2, b2));
                    rgb = new HSB(hsb2[0], hsb1[1], hsb1[2]).Convert();

                    r3 = rgb[0];
                    g3 = rgb[1];
                    b3 = rgb[2];
                    break;
                #endregion
                #region Lighten
                case BlendModes.Lighten:
                    r3 = r1 > r2 ? r1 : r2;
                    g3 = g1 > g2 ? g1 : g2;
                    b3 = b1 > b2 ? b1 : b2;
                    break;
                #endregion
                #region LinearBurn
                case BlendModes.LinearBurn:
                    //R = a + b - 1
                    r3 = (r1 + r2 - 1).Coerce(1);
                    g3 = (g1 + g2 - 1).Coerce(1);
                    b3 = (b1 + b2 - 1).Coerce(1);
                    break;
                #endregion
                #region LinearDodge
                case BlendModes.LinearDodge:
                    r3 = r1 + r2;
                    g3 = g1 + g2;
                    b3 = b1 + b2;
                    break;
                #endregion
                #region LinearLight
                case BlendModes.LinearLight:
                    //if (Blend > ½) R = Base + 2×(Blend-½); if (Blend <= ½) R = Base + 2×Blend - 1
                    r3 = r2 > 0.5 ? r1 + 2 * (r2 - 0.5) : r1 + 2 * r2 - 1;
                    g3 = g2 > 0.5 ? g1 + 2 * (g2 - 0.5) : g1 + 2 * g2 - 1;
                    b3 = b2 > 0.5 ? b1 + 2 * (b2 - 0.5) : b1 + 2 * b2 - 1;
                    break;
                #endregion
                #region Luminosity
                case BlendModes.Luminosity:
                    hsb1 = HSB.From(new RGB(r1, g1, b1));
                    hsb2 = HSB.From(new RGB(r2, g2, b2));
                    rgb = new HSB(hsb1[0], hsb1[1], hsb2[2]).Convert();

                    r3 = rgb[0];
                    g3 = rgb[1];
                    b3 = rgb[2];
                    break;
                #endregion
                #region Multiply
                case BlendModes.Multiply:
                    r3 = r1 * r2;
                    g3 = g1 * g2;
                    b3 = b1 * b2;
                    break;
                #endregion
                #region Negation
                case BlendModes.Negation:
                    r3 = 1 - Math.Abs(1 - r1 - r2);
                    g3 = 1 - Math.Abs(1 - g1 - g2);
                    b3 = 1 - Math.Abs(1 - b1 - b2);
                    break;
                #endregion
                #region Normal
                case BlendModes.Normal:
                    a3 = 1.0 - (1.0 - a2) * (1.0 - a1);
                    r3 = r2 * a2 / a3 + r1 * a1 * (1.0 - a2) / a3;
                    g3 = g2 * a2 / a3 + g1 * a1 * (1.0 - a2) / a3;
                    b3 = b2 * a2 / a3 + b1 * a1 * (1.0 - a2) / a3;

                    a3 = double.IsNaN(a3) ? 0 : a3;
                    r3 = double.IsNaN(r3) ? 0 : r3;
                    g3 = double.IsNaN(g3) ? 0 : g3;
                    b3 = double.IsNaN(b3) ? 0 : b3;
                    break;
                #endregion
                #region Overlay
                case BlendModes.Overlay:
                    //if (Base > ½) R = 1 - (1-2×(Base-½)) × (1-Blend); if (Base <= ½) R = (2×Base) × Blend
                    r3 = r1 > 0.5 ? 1 - (1 - 2 * (r1 - 0.5)) * (1 - r2) : (2 * r1) * r2;
                    g3 = g1 > 0.5 ? 1 - (1 - 2 * (g1 - 0.5)) * (1 - g2) : (2 * g1) * g2;
                    b3 = b1 > 0.5 ? 1 - (1 - 2 * (b1 - 0.5)) * (1 - b2) : (2 * b1) * b2;
                    break;
                #endregion
                #region Phoenix
                case BlendModes.Phoenix:
                    r3 = r1 - r2;
                    g3 = g1 - g2;
                    b3 = b1 - b2;
                    break;
                #endregion
                #region PinLight
                case BlendModes.PinLight:
                    //if (Blend > ½) R = max(Base,2×(Blend-½)); if (Blend <= ½) R = min(Base,2×Blend))
                    r3 = r2 > 0.5 ? Math.Max(r1, 2 * (r2 - 0.5)) : Math.Min(r1, 2 * r2);
                    g3 = g2 > 0.5 ? Math.Max(g1, 2 * (g2 - 0.5)) : Math.Min(g1, 2 * g2);
                    b3 = b2 > 0.5 ? Math.Max(b1, 2 * (b2 - 0.5)) : Math.Min(b1, 2 * b2);
                    break;
                #endregion
                #region Reflect
                case BlendModes.Reflect:
                    r3 = r1 / (r2 == 0 ? 0.01 : r2);
                    g3 = g1 / (g2 == 0 ? 0.01 : g2);
                    b3 = b1 / (b2 == 0 ? 0.01 : b2);
                    break;
                #endregion
                #region Saturation
                case BlendModes.Saturation:
                    hsb1 = HSB.From(new RGB(r1, g1, b1));
                    hsb2 = HSB.From(new RGB(r2, g2, b2));
                    rgb = new HSB(hsb1[0], hsb2[1], hsb1[2]).Convert();

                    r3 = rgb[0];
                    g3 = rgb[1];
                    b3 = rgb[2];
                    break;
                #endregion
                #region Screen
                case BlendModes.Screen:
                    r3 = r1 <= 1 || r2 <= 1 ? r1 + r2 - (r1 * r2) : Math.Max(r1, r2);
                    g3 = g1 <= 1 || g2 <= 1 ? g1 + g2 - (g1 * g2) : Math.Max(g1, g2);
                    b3 = b1 <= 1 || b2 <= 1 ? b1 + b2 - (b1 * b2) : Math.Max(b1, b2);
                    break;
                #endregion
                #region SoftLight
                case BlendModes.SoftLight:
                    r3 = BlendSoftLightf(r1, r2);
                    g3 = BlendSoftLightf(g1, g2);
                    b3 = BlendSoftLightf(b1, b2);
                    break;
                #endregion
                #region VividLight
                case BlendModes.VividLight:
                    r3 = BlendVividLightf(r1, r2);
                    g3 = BlendVividLightf(g1, g2);
                    b3 = BlendVividLightf(b1, b2);
                    break;
                #endregion
            }
            return Color.FromArgb(a3.Multiply(255), r3.Multiply(255), g3.Multiply(255), b3.Multiply(255));
        }
    }
}