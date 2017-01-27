using Imagin.Common.Extensions;
using System;
using System.Windows.Media;

namespace Imagin.Common.Drawing
{
    /// <summary>
    /// Structure to define HSL.
    /// </summary>
    [Serializable]
    public struct Hsl : IColor
    {
        #region Properties

        /// <summary>
        /// Specifies a <see cref="Hsl"/> component.
        /// </summary>
        public enum Component
        {
            /// <summary>
            /// Specifies the <see cref="Hsl.H"/> component.
            /// </summary>
            H,
            /// <summary>
            /// Specifies the <see cref="Hsl.S"/> component.
            /// </summary>
            S,
            /// <summary>
            /// Specifies the <see cref="Hsl.L"/> component.
            /// </summary>
            L
        }

        /// <summary>
        /// 
        /// </summary>
        public static class MaxValue
        {
            /// <summary>
            /// 
            /// </summary>
            public const double H = 359d;
            /// <summary>
            /// 
            /// </summary>
            public const double S = 100d;
            /// <summary>
            /// 
            /// </summary>
            public const double L = 100d;
        }

        /// <summary>
        /// 
        /// </summary>
        public static class MinValue
        {
            /// <summary>
            /// 
            /// </summary>
            public const double H = 0;
            /// <summary>
            /// 
            /// </summary>
            public const double S = 0;
            /// <summary>
            /// 
            /// </summary>
            public const double L = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public Color Color
        {
            get
            {
                return ToRgba().Color;
            }
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
            private set
            {
                h = value.Coerce(MaxValue.H, MinValue.H);
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
            private set
            {
                s = value.Coerce(MaxValue.S, MinValue.S);
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
            private set
            {
                l = value.Coerce(MaxValue.L, MinValue.L);
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
        /// 
        /// </summary>
        /// <param name="Color"></param>
        public Hsl(Color Color) : this(new Rgba(Color))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Rgba"></param>
        public Hsl(Rgba Rgba) : this(0, 0, 0)
        {
            double r = Rgba.Linear(Rgba.R), g = Rgba.Linear(Rgba.G), b = Rgba.Linear(Rgba.B);

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

            H = h.Multiply(MaxValue.H);
            S = s.Multiply(MaxValue.S);
            L = l.Multiply(MaxValue.L);
        }

        /// <summary>
        /// Creates an instance of the <see cref="Hsl"/> structure.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="l"></param>
        public Hsl(double h, double s, double l) : this()
        {
            H = h;
            S = s;
            L = l;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        static double Frgb(double p, double q, double t)
        {
            if (t < 0)
                t += 1d;

            if (t > 1d)
                t -= 1d;

            if (t < 1d / 6d)
                return p + (q - p) * 6d * t;

            if (t < 1d / 2d)
                return q;

            if (t < 2d / 3d)
                return p + (q - p) * (2d / 3d - t) * 6d;

            return p;
        }

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
        /// <param name="Component"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public Hsl New(Component Component, double Value)
        {
            switch (Component)
            {
                case Component.H:
                    h = Value;
                    break;
                case Component.S:
                    s = Value;
                    break;
                case Component.L:
                    l = Value;
                    break;
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public Rgba ToRgba()
        {
            double h = this.h / MaxValue.H, s = this.s / MaxValue.S, l = this.l / MaxValue.L;

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

            r = (r * Rgba.MaxValue).Coerce(Rgba.MaxValue).Round();
            g = (g * Rgba.MaxValue).Coerce(Rgba.MaxValue).Round();
            b = (b * Rgba.MaxValue).Coerce(Rgba.MaxValue).Round();

            return new Rgba(r.ToByte(), g.ToByte(), b.ToByte());
        }

        #endregion
    }
}
