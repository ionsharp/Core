using Imagin.Common.Drawing;
using Imagin.Common.Extensions;
using System;
using System.Windows.Media;

namespace Imagin.Controls.Extended.Primitives
{
    /// <summary>
    /// Structure to define CIE L*a*b*.
    /// </summary>
    [Serializable]
    public struct Lab
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
            public static double A = 128d;
            /// <summary>
            /// 
            /// </summary>
            public static double B = 128d;
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
            public static double A = -127d;
            /// <summary>
            /// 
            /// </summary>
            public static double B = -127d;
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

        double a;
        /// <summary>
        /// Gets or sets the opposing a component (-127 to 128).
        /// </summary>
        public double A
        {
            get
            {
                return a;
            }
            set
            {
                a = value.Coerce(MaxValue.A, MinValue.A);
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
                return b;
            }
            set
            {
                b = value.Coerce(MaxValue.B, MinValue.B);
            }
        }

        #endregion

        #region Lab

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Lab a, Lab b)
        {
            return a.L == b.L && a.A == b.A && a.B == b.B;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Lab a, Lab b)
        {
            return a.L != b.L || a.A != b.A || a.B != b.B;
        }

        /// <summary>
        /// Creates an instance of the <see cref="Lab"/> structure.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        public Lab(double L, double A, double B)
        {
            l = L.Coerce(MaxValue.L, MinValue.L);
            a = A.Coerce(MaxValue.A, MinValue.A);
            b = B.Coerce(MaxValue.B, MinValue.B);
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
            return Object == null || GetType() != Object.GetType() ? false : this == (Lab)Object;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return l.GetHashCode() ^ a.GetHashCode() ^ b.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "L => {0}, A => {1}, B => {2}".F(l, a, b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Color"></param>
        /// <returns></returns>
        public static Lab FromColor(Color Color)
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
        public static Lab FromRgba(byte R, byte G, byte B, byte A = 255, Illuminant Illuminant = Illuminant.Default)
        {
            var RLinear = R.ToDouble() / 255d;
            var GLinear = G.ToDouble() / 255d;
            var BLinear = B.ToDouble() / 255d;

            RLinear = (RLinear > 0.04045) ? Math.Pow((RLinear + 0.055) / (1 + 0.055), 2.2) : (RLinear / 12.92);
            GLinear = (GLinear > 0.04045) ? Math.Pow((GLinear + 0.055) / (1 + 0.055), 2.2) : (GLinear / 12.92);
            BLinear = (BLinear > 0.04045) ? Math.Pow((BLinear + 0.055) / (1 + 0.055), 2.2) : (BLinear / 12.92);

            double x = RLinear * 0.4124 + GLinear * 0.3576 + BLinear * 0.1805;
            double y = RLinear * 0.2126 + GLinear * 0.7152 + BLinear * 0.0722;
            double z = RLinear * 0.0193 + GLinear * 0.1192 + BLinear * 0.9505;

            var mx = Xyz.Max[Xyz.Component.X, Illuminant];
            var my = Xyz.Max[Xyz.Component.Y, Illuminant];
            var mz = Xyz.Max[Xyz.Component.Z, Illuminant];

            double l = 116d * Xyz.Fxyz(y / my) - 16d;
            double a = 500d * (Xyz.Fxyz(x / mx) - Xyz.Fxyz(y / my));
            double b = 200d * (Xyz.Fxyz(y / my) - Xyz.Fxyz(z / mz));

            return new Lab(l, a, b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="l"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Color ToColor(double l, double a, double b)
        {
            return ToRgba(l, a, b).ToColor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="l"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="Illuminant"></param>
        /// <returns></returns>
        public static Rgba ToRgba(double l, double a, double b, Illuminant Illuminant = Illuminant.Default)
        {
            double theta = 6.0 / 29.0;

            double fy = (l + 16.0) / 116.0;
            double fx = fy + (a / 500.0);
            double fz = fy - (b / 200.0);

            var mx = Xyz.Max[Xyz.Component.X, Illuminant];
            var my = Xyz.Max[Xyz.Component.Y, Illuminant];
            var mz = Xyz.Max[Xyz.Component.Z, Illuminant];

            var x = (fx > theta ? mx * (fx * fx * fx) : (fx - 16.0 / 116.0) * 3.0 * (theta * theta) * mx).Coerce(mx);
            var y = (fy > theta ? my * (fy * fy * fy) : (fy - 16.0 / 116.0) * 3.0 * (theta * theta) * my).Coerce(my);
            var z = (fz > theta ? mz * (fz * fz * fz) : (fz - 16.0 / 116.0) * 3.0 * (theta * theta) * mz).Coerce(mz);

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
