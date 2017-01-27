using Imagin.Common.Extensions;
using System;
using System.Windows.Media;

namespace Imagin.Controls.Extended.Primitives
{
    /// <summary>
    /// Structure to define LCH.
    /// </summary>
    [Serializable]
    public struct Lch
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
            public static double L = 100d;
            /// <summary>
            /// 
            /// </summary>
            public static double C = 100d;
            /// <summary>
            /// 
            /// </summary>
            public static double H = 359d;
        }

        /// <summary>
        /// 
        /// </summary>
        public struct MinValue
        {
            /// <summary>
            /// 
            /// </summary>
            public static double L = 0d;
            /// <summary>
            /// 
            /// </summary>
            public static double C = 0d;
            /// <summary>
            /// 
            /// </summary>
            public static double H = 0d;
        }

        double l;
        /// <summary>
        /// Gets or sets the luminance component (0 to 100).
        /// </summary>
        public double L
        {
            get
            {
                return l;
            }
            set
            {
                l = value.Coerce(MaxValue.L, MinValue.L);
            }
        }

        double c;
        /// <summary>
        /// Gets or sets the chroma component (0 to 100).
        /// </summary>
        public double C
        {
            get
            {
                return c;
            }
            set
            {
                c = value.Coerce(MaxValue.C, MinValue.C);
            }
        }

        double h;
        /// <summary>
        /// Gets or sets the hue component (0 to 359).
        /// </summary>
        public double H
        {
            get
            {
                return h;
            }
            set
            {
                h = value.Coerce(MaxValue.H, MinValue.H);
            }
        }

        #endregion

        #region Lch

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Lch a, Lch b)
        {
            return a.L == b.L && a.C == b.C && a.H == b.H;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Lch a, Lch b)
        {
            return a.L != b.L || a.C != b.C || a.H != b.H;
        }

        /// <summary>
        /// Creates an instance of the <see cref="Lch"/> structure.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="C"></param>
        /// <param name="H"></param>
        public Lch(double L, double C, double H)
        {
            l = L.Coerce(MaxValue.L, MinValue.L);
            c = C.Coerce(MaxValue.C, MinValue.C);
            h = H.Coerce(MaxValue.H, MinValue.H);
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
            return Object == null || GetType() != Object.GetType() ? false : this == (Lch)Object;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return l.GetHashCode() ^ c.GetHashCode() ^ h.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "L => {0}, C => {1}, H => {2}".F(l, c, h);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Color"></param>
        /// <returns></returns>
        public static Lch FromColor(Color Color)
        {
            return FromRgba(Color.R, Color.G, Color.B, Color.A);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="L"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Lch FromLab(double L, double A, double B)
        {
            double l, c, h;
            h = Math.Atan2(B, A);

            if (h > 0.0)
                h = (h / Math.PI) * 180.0;
            else h = 360.0 - (Math.Abs(h) / Math.PI) * 180.0;

            l = L;
            c = Math.Sqrt(Math.Pow(A, 2.0) + Math.Pow(B, 2.0));

            return new Lch(l, c, h);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="R"></param>
        /// <param name="G"></param>
        /// <param name="B"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public static Lch FromRgba(byte R, byte G, byte B, byte A = 255)
        {
            var lab = Lab.FromRgba(R, G, B, A);
            return FromLab(lab.L, lab.A, lab.B);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="l"></param>
        /// <param name="c"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public static Color ToColor(double l, double c, double h)
        {
            return ToRgba(l, c, h).ToColor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="L"></param>
        /// <param name="C"></param>
        /// <param name="H"></param>
        /// <returns></returns>
        public static Lab ToLab(double L, double C, double H)
        {
            double l = L, a, b;
            a = Math.Round(Math.Cos(H.ToRadians()) * C);
            b = Math.Round(Math.Sin(H.ToRadians()) * C);
            return new Lab(l, a, b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="l"></param>
        /// <param name="c"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public static Rgba ToRgba(double l, double c, double h)
        {
            var lab = ToLab(l, c, h);
            return Lab.ToRgba(lab.L, lab.A, lab.B);
        }

        #endregion
    }
}
