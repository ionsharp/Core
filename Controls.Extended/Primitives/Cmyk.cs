using Imagin.Common.Extensions;
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace Imagin.Controls.Extended.Primitives
{
    [Serializable]
    /// <summary>
    /// Structure to define CMYK.
    /// </summary>
    public struct Cmyk 
    {
        #region Properties

        public struct MaxValue
        {
            public static double C = 1.0;

            public static double M = 1.0;

            public static double Y = 1.0;

            public static double K = 1.0;
        }

        public struct MinValue
        {
            public static double C = 0.0;

            public static double M = 0.0;

            public static double Y = 0.0;

            public static double K = 0.0;
        }

        double c;
        /// <summary>
        /// Gets or sets the cyan component (0 to 1).
        /// </summary>
        public double C
        {
            get
            {
                return c;
            }
            set
            {
                c = value.Coerce(Cmyk.MaxValue.C, Cmyk.MinValue.C);
            }
        }

        double m;
        /// <summary>
        /// Gets or sets the magenta component (0 to 1).
        /// </summary>
        public double M
        {
            get
            {
                return m;
            }
            set
            {
                m = value.Coerce(Cmyk.MaxValue.M, Cmyk.MinValue.M);
            }
        }

        double y;
        /// <summary>
        /// Gets or sets the yellow component (0 to 1).
        /// </summary>
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value.Coerce(Cmyk.MaxValue.Y, Cmyk.MinValue.Y);
            }
        }

        double k;
        /// <summary>
        /// Gets or sets the black component (0 to 1).
        /// </summary>
        public double K
        {
            get
            {
                return k;
            }
            set
            {
                k = value.Coerce(Cmyk.MaxValue.K, Cmyk.MinValue.K);
            }
        }

        #endregion

        #region Cmyk

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Cmyk a, Cmyk b)
        {
            return a.C == b.C && a.M == b.M && a.Y == b.Y && a.K == b.K;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Cmyk a, Cmyk b)
        {
            return a.C != b.C || a.M != b.M || a.Y != b.Y || a.K != b.K;
        }

        /// <summary>
        /// Creates an instance of a Cmyk structure.
        /// </summary>
        public Cmyk(double C, double M, double Y, double K)
        {
            c = C.Coerce(MaxValue.C, MinValue.C);
            m = M.Coerce(MaxValue.M, MinValue.M);
            y = Y.Coerce(MaxValue.Y, MinValue.Y);
            k = K.Coerce(MaxValue.K, MinValue.K);
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
            if (Object == null || GetType() != Object.GetType()) return false;
            return (this == (Cmyk)Object);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return C.GetHashCode() ^ M.GetHashCode() ^ Y.GetHashCode() ^ K.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("C => {0}, M => {1}, Y => {2}, K => {3}", this.C, this.M, this.Y, this.K);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Color"></param>
        /// <returns></returns>
        public static Cmyk FromColor(Color Color)
        {
            return FromRgba(Color.R, Color.G, Color.B, Color.A);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Cmyk FromRgba(byte r, byte g, byte b, byte a = 255)
        {
            var k = 0d;
            if (r == 0 && g == 0 && b == 0)
            {
                k = 1d;
                return new Cmyk(0, 0, 0, k);
            }

            var c = 1d - (r / 255d);
            var m = 1d - (g / 255d);
            var y = 1d - (b / 255d);

            k = Math.Min(c, Math.Min(m, y));

            c = (c - k) / (1 - k);
            m = (m - k) / (1 - k);
            y = (y - k) / (1 - k);

            return new Cmyk(c, m, y, k);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="m"></param>
        /// <param name="y"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Color ToColor(double c, double m, double y, double k)
        {
            var r = 255d * (1d - c) * (1d - k);
            var g = 255d * (1d - m) * (1d - k);
            var b = 255d * (1d - y) * (1d - k);

            r = r.Round().Coerce(Rgba.MaxValue.R, Rgba.MinValue.R);
            g = g.Round().Coerce(Rgba.MaxValue.G, Rgba.MinValue.G);
            b = b.Round().Coerce(Rgba.MaxValue.B, Rgba.MinValue.B);

            return Color.FromRgb(r.ToByte(), g.ToByte(), b.ToByte());
        }

        #endregion
    }
}
