using Imagin.Common.Extensions;
using System;
using System.Windows.Media;

namespace Imagin.Controls.Extended.Primitives
{
    /// <summary>
    /// Structure to define HSL.
    /// </summary>
    [Serializable]
    public struct Hsl
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public struct MaxValue
        {
            /// <summary>
            /// 
            /// </summary>
            public static double H = 1.0;
            /// <summary>
            /// 
            /// </summary>
            public static double S = 1.0;
            /// <summary>
            /// 
            /// </summary>
            public static double B = 1.0;
        }

        /// <summary>
        /// 
        /// </summary>
        public struct MinValue
        {
            /// <summary>
            /// 
            /// </summary>
            public static double H = 0;
            /// <summary>
            /// 
            /// </summary>
            public static double S = 0;
            /// <summary>
            /// 
            /// </summary>
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
                h = value.Coerce(1d);
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
                s = value.Coerce(1d);
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
                l = value.Coerce(1d);
            }
        }

        #endregion

        #region Hsl

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Hsl a, Hsl b)
        {
            return a.H == b.H && a.S == b.S && a.L == b.L;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Hsl a, Hsl b)
        {
            return a.H != b.H || a.S != b.S || a.L != b.L;
        }

        /// <summary>
        /// Creates an instance of the <see cref="Hsl"/> structure.
        /// </summary>
        /// <param name="H"></param>
        /// <param name="S"></param>
        /// <param name="L"></param>
        public Hsl(double H, double S, double L)
        {
            h = H.Coerce(1d);
            s = S.Coerce(1d);
            l = L.Coerce(1d);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Object"></param>
        /// <returns></returns>
        public override bool Equals(Object Object)
        {
            return Object == null || GetType() != Object.GetType() ? false : this == (Hsl)Object;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return h.GetHashCode() ^ s.GetHashCode() ^ l.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "H => {0}, S => {1}, L => {2}".F(h, s, l);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <param name="t"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Color"></param>
        /// <returns></returns>
        public static Hsl FromColor(Color Color)
        {
            return FromRgba(Color.R, Color.G, Color.B);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="R"></param>
        /// <param name="G"></param>
        /// <param name="B"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public static Hsl FromRgba(byte R, byte G, byte B, byte A = 255)
        {
            double r = R.ToDouble() / 255.0, g = G.ToDouble() / 255.0, b = B.ToDouble() / 255.0;

            double max = Math.Max(Math.Max(r, g), b), min = Math.Min(Math.Min(r, g), b);
            double h = 0, s = 0, l = (max + min) / 2d;
            var d = max - min;
            if (d == 0)
            {
                h = s = 0;
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public static Color ToColor(double h, double s, double l)
        {
            return ToRgba(h, s, l).ToColor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public static Rgba ToRgba(double h, double s, double l)
        {
            double r = 0, g = 0, b = 0;
            if (s == 0)
            {
                r = g = b = l;
            }
            else
            {
                var q = l <= 0.5 ? l * (1d + s) : l + s - (l * s);
                var p = 2d * l - q;

                r = Frgb(p, q, h + 1d / 3d);
                g = Frgb(p, q, h);
                b = Frgb(p, q, h - 1d / 3d);
            }

            r = (r * Rgba.MaxValue).Round().Coerce(Rgba.MaxValue);
            g = (g * Rgba.MaxValue).Round().Coerce(Rgba.MaxValue);
            b = (b * Rgba.MaxValue).Round().Coerce(Rgba.MaxValue);

            return new Rgba(r.ToInt32(), g.ToInt32(), b.ToInt32(), 255);
        }

        #endregion
    }
}
