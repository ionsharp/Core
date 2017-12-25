using Imagin.Common.Linq;
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace Imagin.Common.Drawing
{
    /// <summary>
    /// Structure to define CMYK.
    /// </summary>
    [Serializable]
    public struct Cmyk : IColor
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public const double MaxValue = 100d;

        /// <summary>
        /// 
        /// </summary>
        public const double MinValue = 0;

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
            private set
            {
                c = value.Coerce(MaxValue, MinValue);
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
            private set
            {
                m = value.Coerce(MaxValue, MinValue);
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
            private set
            {
                y = value.Coerce(MaxValue, MinValue);
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
            private set
            {
                k = value.Coerce(MaxValue, MinValue);
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
        /// 
        /// </summary>
        /// <param name="Color"></param>
        public Cmyk(Color Color) : this(new Rgba(Color))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Rgba"></param>
        public Cmyk(Rgba Rgba) : this()
        {
            double c = 0, y = 0, m = 0, k = 0;

            k = 0d;
            if (Rgba.R == 0 && Rgba.G == 0 && Rgba.B == 0)
            {
                c = m = y = 0;
                k = 1d;
            }

            c = 1d - (Rgba.R / 255d);
            m = 1d - (Rgba.G / 255d);
            y = 1d - (Rgba.B / 255d);

            k = Math.Min(c, Math.Min(m, y));

            c = (c - k) / (1 - k);
            m = (m - k) / (1 - k);
            y = (y - k) / (1 - k);

            C = c.Multiply(MaxValue);
            Y = y.Multiply(MaxValue);
            M = m.Multiply(MaxValue);
            K = k.Multiply(MaxValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="m"></param>
        /// <param name="y"></param>
        /// <param name="k"></param>
        public Cmyk(double c, double m, double y, double k) : this()
        {
            C = c;
            M = m;
            Y = y;
            K = k;
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
        /// <returns></returns>
        public Rgba ToRgba()
        {
            double c = this.c / MaxValue, y = this.y / MaxValue, m = this.m / MaxValue, k = this.k / MaxValue;

            var r = 255d * (1d - c) * (1d - k);
            var g = 255d * (1d - m) * (1d - k);
            var b = 255d * (1d - y) * (1d - k);

            return new Rgba(r.Round(), g.Round(), b.Round());
        }

        #endregion
    }
}
