using Imagin.Common.Extensions;
using System;
using System.Windows.Media;

namespace Imagin.Controls.Extended.Primitives
{
    [Serializable]
    /// <summary>
    /// Structure to define HSL.
    /// </summary>
    public struct Hsl
    {
        #region Properties

        public struct MaxValue
        {
            public static double H = 1.0;

            public static double S = 1.0;

            public static double B = 1.0;
        }

        public struct MinValue
        {
            public static double H = 0;

            public static double S = 0;

            public static double B = 0;
        }

        double h;
        /// <summary>
        /// Gets or sets the hue component (0 to 1).
        /// </summary>
        public double H
        {
            get
            {
                return h;
            }
            set
            {
                h = Coerce(value, 1.0);
            }
        }

        double s;
        /// <summary>
        /// Gets or sets the saturation component (0 to 1).
        /// </summary>
        public double S
        {
            get
            {
                return s;
            }
            set
            {
                s = Coerce(value, 1.0);
            }
        }

        double l;
        /// <summary>
        /// Gets or sets the lightness component (0 to 1).
        /// </summary>
        public double L
        {
            get
            {
                return l;
            }
            set
            {
                l = Coerce(value, 1.0);
            }
        }

        #endregion

        #region Hsl

        public static bool operator ==(Hsl a, Hsl b)
        {
            return a.H == b.H && a.S == b.S && a.L == b.L;
        }

        public static bool operator !=(Hsl a, Hsl b)
        {
            return a.H != b.H || a.S != b.S || a.L != b.L;
        }

        /// <summary>
        /// Creates an instance of a Hsl structure.
        /// </summary>
        public Hsl(double H, double S, double L)
        {
            this.h = Coerce(H, 1.0);
            this.s = Coerce(S, 1.0);
            this.l = Coerce(L, 1.0);
        }

        #endregion

        #region Methods

        public static double Coerce(double ToCoerce, double Max)
        {
            return ToCoerce > Max ? Max : (ToCoerce < 0.0 ? 0.0 : ToCoerce);
        }

        public override bool Equals(Object Object)
        {
            if (Object == null || GetType() != Object.GetType()) return false;
            return (this == (Hsl)Object);
        }

        public override int GetHashCode()
        {
            return this.H.GetHashCode() ^ this.S.GetHashCode() ^ L.GetHashCode();
        }

        static double Frgb(double p, double q, double t)
        {
            if (t < 0.0)
                t += 1.0;
            if (t > 1.0)
                t -= 1.0;
            if (t < 1.0 / 6.0)
                return p + (q - p) * 6.0 * t;
            if (t < 1.0 / 2.0)
                return q;
            if (t < 2.0 / 3.0)
                return p + (q - p) * (2.0 / 3.0 - t) * 6.0;
            return p;
        }

        public static Hsl FromColor(Color Color)
        {
            return Hsl.FromRgba(Color.R, Color.G, Color.B);
        }

        public static Hsl FromRgba(byte R, byte G, byte B, byte A = 255)
        {
            double r = R.ToDouble() / 255.0, g = G.ToDouble() / 255.0, b = B.ToDouble() / 255.0;

            double max = Math.Max(Math.Max(r, g), b), min = Math.Min(Math.Min(r, g), b);
            double h = 0, s = 0, l = (max + min) / 2d;
            var d = max - min;
            if (d == 0)
                h = s = 0;
            else
            {
                s = l > 0.5 ? d / (2d - max - min) : d / (max + min);

                double del_r, del_b, del_g;

                del_r = (((max - r) / 6) + (d / 2)) / d;
                del_g = (((max - g) / 6) + (d / 2)) / d;
                del_b = (((max - b) / 6) + (d / 2)) / d;

                if (r == max)
                    h = del_b - del_g;
                else if (g == max)
                    h = (1d / 3d) + del_r - del_b;
                else if (b == max)
                    h = (2d / 3d) + del_g - del_r;
                h = h < 0 ? h += 1 : h;
                h = h > 1 ? h -= 1 : h;
            }
            return new Hsl(h, s, l);
        }

        public static Color ToColor(double h, double s, double l)
        {
            return Hsl.ToRgba(h, s, l).ToColor();
        }

        public static Rgba ToRgba(double hue, double saturation, double lightness)
        {
            double r = 0.0, g = 0.0, b = 0.0;
            if (saturation == 0.0)
                r = g = b = lightness;
            else
            {
                double q = lightness <= 0.5 ? lightness * (1.0 + saturation) : lightness + saturation - (lightness * saturation);
                double p = 2.0 * lightness - q;

                r = Hsl.Frgb(p, q, hue + 1.0 / 3.0);
                g = Hsl.Frgb(p, q, hue);
                b = Hsl.Frgb(p, q, hue - 1.0 / 3.0);
            }

            r = (r * 255.0).Round().Coerce(255.0);
            g = (g * 255.0).Round().Coerce(255.0);
            b = (b * 255.0).Round().Coerce(255.0);

            return new Rgba(r.ToInt(), g.ToInt(), b.ToInt(), 255);
        }

        #endregion
    }
}
