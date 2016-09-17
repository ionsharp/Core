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

        public static bool operator ==(Cmyk a, Cmyk b)
        {
            return a.C == b.C && a.M == b.M && a.Y == b.Y && a.K == b.K;
        }

        public static bool operator !=(Cmyk a, Cmyk b)
        {
            return a.C != b.C || a.M != b.M || a.Y != b.Y || a.K != b.K;
        }

        /// <summary>
        /// Creates an instance of a Cmyk structure.
        /// </summary>
        public Cmyk(double C, double M, double Y, double K)
        {
            this.c = C.Coerce(Cmyk.MaxValue.C, Cmyk.MinValue.C);
            this.m = M.Coerce(Cmyk.MaxValue.M, Cmyk.MinValue.M);
            this.y = Y.Coerce(Cmyk.MaxValue.Y, Cmyk.MinValue.Y);
            this.k = K.Coerce(Cmyk.MaxValue.K, Cmyk.MinValue.K);
        }

        #endregion

        #region Methods

        public override bool Equals(Object Object)
        {
            if (Object == null || GetType() != Object.GetType()) return false;
            return (this == (Cmyk)Object);
        }

        public override int GetHashCode()
        {
            return C.GetHashCode() ^ M.GetHashCode() ^ Y.GetHashCode() ^ K.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("C => {0}, M => {1}, Y => {2}, K => {3}", this.C, this.M, this.Y, this.K);
        }

        public static Cmyk FromColor(Color Color)
        {
            return Cmyk.FromRgba(Color.R, Color.G, Color.B, Color.A);
        }

        public static Cmyk FromRgba(byte r, byte g, byte b, byte a = 255)
        {
            var R = r.ToDouble() / 255.0;
            var G = g.ToDouble() / 255.0;
            var B = b.ToDouble() / 255.0;

            var k = 1.0 - Math.Max(Math.Max(R, G), B);
            var c = (1.0 - R - k) / (1.0 - k);
            var m = (1.0 - G - k) / (1.0 - k);
            var y = (1.0 - B - k) / (1.0 - k);

            return new Cmyk(c, m, y, k);
        }

        public static Color ToColor(double c, double m, double y, double k)
        {
            var r = 255.0 * (1.0 - c) * (1.0 - k);
            var g = 255.0 * (1.0 - m) * (1.0 - k);
            var b = 255.0 * (1.0 - y) * (1.0 - k);

            r = r.Round().Coerce(Rgba.MaxValue.R, Rgba.MinValue.R);
            g = g.Round().Coerce(Rgba.MaxValue.G, Rgba.MinValue.G);
            b = b.Round().Coerce(Rgba.MaxValue.B, Rgba.MinValue.B);

            return Color.FromRgb(r.ToByte(), g.ToByte(), b.ToByte());
        }

        #endregion
    }
}
