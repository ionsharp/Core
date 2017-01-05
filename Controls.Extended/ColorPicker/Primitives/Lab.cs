using Imagin.Common.Extensions;
using System;
using System.Windows.Media;

namespace Imagin.Controls.Extended.Primitives
{
    [Serializable]
    /// <summary>
    /// Structure to define CIE L*a*b*.
    /// </summary>
    public struct Lab
    {
        #region Properties

        public struct MaxValue
        {
            public static double L = 100;

            public static double A = 128;

            public static double B = 128;
        }

        public struct MinValue
        {
            public static double L = 0;

            public static double A = -127;

            public static double B = -127;
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
                this.l = value.Coerce(Lab.MaxValue.L, Lab.MinValue.L);
            }
        }

        double a;
        /// <summary>
        /// Gets or sets the opposing a component (-127 to 128).
        /// </summary>
        public double A
        {
            get
            {
                return this.a;
            }
            set
            {
                this.a = value.Coerce(Lab.MaxValue.A, Lab.MinValue.A);
            }
        }

        double b;
        /// <summary>
        /// Gets or sets the opposing b component (-127 to 128).
        /// </summary>
        public double B
        {
            get
            {
                return this.b;
            }
            set
            {
                this.b = value.Coerce(Lab.MaxValue.B, Lab.MinValue.B);
            }
        }

        #endregion

        #region Lab

        public static bool operator ==(Lab a, Lab b)
        {
            return a.L == b.L && a.A == b.A && a.B == b.B;
        }

        public static bool operator !=(Lab a, Lab b)
        {
            return a.L != b.L || a.A != b.A || a.B != b.B;
        }

        /// <summary>
        /// Creates an instance of a Lab structure.
        /// </summary>
        public Lab(double L, double A, double B)
        {
            this.l = L.Coerce(Lab.MaxValue.L, Lab.MinValue.L);
            this.a = A.Coerce(Lab.MaxValue.A, Lab.MinValue.A);
            this.b = B.Coerce(Lab.MaxValue.B, Lab.MinValue.B);
        }

        #endregion

        #region Methods

        public override bool Equals(Object Object)
        {
            if (Object == null || GetType() != Object.GetType()) return false;
            return (this == (Lab)Object);
        }

        public override int GetHashCode()
        {
            return L.GetHashCode() ^ a.GetHashCode() ^ b.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("L => {0}, A => {1}, B => {2}", this.L.ToString(), this.A.ToString(), this.B.ToString());
        }

        public static Lab FromColor(Color Color)
        {
            return Lab.FromRgba(Color.R, Color.G, Color.B);
        }

        public static Lab FromRgba(byte R, byte G, byte B, byte A = 255)
        {
            double RLinear = R.ToDouble() / 255.0;
            double GLinear = G.ToDouble() / 255.0;
            double BLinear = B.ToDouble() / 255.0;

            RLinear = (RLinear > 0.04045) ? Math.Pow((RLinear + 0.055) / (1 + 0.055), 2.2) : (RLinear / 12.92);
            GLinear = (GLinear > 0.04045) ? Math.Pow((GLinear + 0.055) / (1 + 0.055), 2.2) : (GLinear / 12.92);
            BLinear = (BLinear > 0.04045) ? Math.Pow((BLinear + 0.055) / (1 + 0.055), 2.2) : (BLinear / 12.92);

            double x = RLinear * 0.4124 + GLinear * 0.3576 + BLinear * 0.1805;
            double y = RLinear * 0.2126 + GLinear * 0.7152 + BLinear * 0.0722;
            double z = RLinear * 0.0193 + GLinear * 0.1192 + BLinear * 0.9505;

            double l = 116.0 * Xyz.Fxyz(y / Xyz.MaxValue.Y) - 16.0;
            double a = 500.0 * (Xyz.Fxyz(x / Xyz.MaxValue.X) - Xyz.Fxyz(y / Xyz.MaxValue.Y));
            double b = 200.0 * (Xyz.Fxyz(y / Xyz.MaxValue.Y) - Xyz.Fxyz(z / Xyz.MaxValue.Z));

            return new Lab(l, a, b);
        }

        public static Color ToColor(double l, double a, double b)
        {
            return Lab.ToRgba(l, a, b).ToColor();
        }

        public static Rgba ToRgba(double l, double a, double b)
        {
            double theta = 6.0 / 29.0;

            double fy = (l + 16.0) / 116.0;
            double fx = fy + (a / 500.0);
            double fz = fy - (b / 200.0);

            var x = (fx > theta ? Xyz.MaxValue.X * (fx * fx * fx) : (fx - 16.0 / 116.0) * 3.0 * (theta * theta) * Xyz.MaxValue.X).Coerce(Xyz.MaxValue.X);
            var y = (fy > theta ? Xyz.MaxValue.Y * (fy * fy * fy) : (fy - 16.0 / 116.0) * 3.0 * (theta * theta) * Xyz.MaxValue.Y).Coerce(Xyz.MaxValue.Y);
            var z = (fz > theta ? Xyz.MaxValue.Z * (fz * fz * fz) : (fz - 16.0 / 116.0) * 3.0 * (theta * theta) * Xyz.MaxValue.Z).Coerce(Xyz.MaxValue.Z);

            double[] Clinear = new double[3];
            Clinear[0] = x * 3.2410 - y * 1.5374 - z * 0.4986;  //R
            Clinear[1] = -x * 0.9692 + y * 1.8760 - z * 0.0416; //G
            Clinear[2] = x * 0.0556 - y * 0.2040 + z * 1.0570;  //B

            for (int i = 0; i < 3; i++)
            {
                Clinear[i] = (Clinear[i] <= 0.0031308) ? 12.92 * Clinear[i] : (1.0 + 0.055) * Math.Pow(Clinear[i], (1.0 / 2.4)) - 0.055;
                Clinear[i] = Math.Min(Clinear[i], 1);
                Clinear[i] = Math.Max(Clinear[i], 0);
                Clinear[i] *= 255.0;
                Clinear[i] = Clinear[i].Coerce(255.0);
            }

            return new Rgba(Clinear[0].ToInt32(), Clinear[1].ToInt32(), Clinear[2].ToInt32());
        }

        #endregion
    }
}
