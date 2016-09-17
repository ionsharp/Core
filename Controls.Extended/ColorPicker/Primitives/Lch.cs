using Imagin.Common.Extensions;
using System;
using System.Windows.Media;

namespace Imagin.Controls.Extended.Primitives
{
    [Serializable]
    /// <summary>
    /// Structure to define LCH.
    /// </summary>
    public struct Lch
    {
        #region Properties

        public struct MaxValue
        {
            public static double L = 100.0;

            public static double C = 100.0;

            public static double H = 359.0;
        }

        public struct MinValue
        {
            public static double L = 0.0;

            public static double C = 0.0;

            public static double H = 0.0;
        }

        double l;
        /// <summary>
        /// Gets or sets the luminance component (0 to 100).
        /// </summary>
        public double L
        {
            get
            {
                return this.l;
            }
            set
            {
                this.l = value.Coerce(Lch.MaxValue.L, Lch.MinValue.L);
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
                return this.c;
            }
            set
            {
                this.c = value.Coerce(Lch.MaxValue.C, Lch.MinValue.C);
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
                return this.h;
            }
            set
            {
                this.h = value.Coerce(Lch.MaxValue.H, Lch.MinValue.H);
            }
        }

        #endregion

        #region Lch

        public static bool operator ==(Lch a, Lch b)
        {
            return a.L == b.L && a.C == b.C && a.H == b.H;
        }

        public static bool operator !=(Lch a, Lch b)
        {
            return a.L != b.L || a.C != b.C || a.H != b.H;
        }

        /// <summary>
        /// Creates an instance of a Lch structure.
        /// </summary>
        public Lch(double L, double C, double H)
        {
            this.l = L.Coerce(Lch.MaxValue.L, Lch.MinValue.L);
            this.c = C.Coerce(Lch.MaxValue.C, Lch.MinValue.C);
            this.h = H.Coerce(Lch.MaxValue.H, Lch.MinValue.H);
        }

        #endregion

        #region Methods

        public override bool Equals(Object Object)
        {
            if (Object == null || GetType() != Object.GetType()) return false;

            return (this == (Lch)Object);
        }

        public override int GetHashCode()
        {
            return L.GetHashCode() ^ c.GetHashCode() ^ h.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("L => {0}, C => {1}, H => {2}", this.L.ToString(), this.C.ToString(), this.H.ToString());
        }

        public static Lch FromColor(Color Color)
        {
            return Lch.FromRgba(Color.R, Color.G, Color.B, Color.A);
        }

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

        public static Lch FromRgba(byte R, byte G, byte B, byte A = 255)
        {
            Lab lab = Lab.FromRgba(R, G, B, A);
            return Lch.FromLab(lab.L, lab.A, lab.B);
        }

        public static Color ToColor(double l, double c, double h)
        {
            return Lch.ToRgba(l, c, h).ToColor();
        }

        public static Lab ToLab(double L, double C, double H)
        {
            double l = L, a, b;
            a = Math.Round(Math.Cos(H.ToRadians()) * C);
            b = Math.Round(Math.Sin(H.ToRadians()) * C);
            return new Lab(l, a, b);
        }

        public static Rgba ToRgba(double l, double c, double h)
        {
            Lab lab = Lch.ToLab(l, c, h);
            return Lab.ToRgba(lab.L, lab.A, lab.B);
        }

        #endregion
    }
}
