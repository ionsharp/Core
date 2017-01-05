using Imagin.Common.Extensions;
using System;
using System.Windows.Media;

namespace Imagin.Controls.Extended.Primitives
{
    [Serializable]
    /// <summary>
    /// Structure to define CIE XYZ.
    /// </summary>
    /// <remarks>
    /// There are 11 main illuminants:
    /// 1. A*
    /// 2. C*
    /// 3. D50*
    /// 4. D55*
    /// 5. D65*
    /// 6. D75
    /// 7. E*^
    /// 8. F2
    /// 9. F7
    /// 10. F11
    /// 11. ICC*
    /// 
    /// * Defined in this struct
    /// ^ Theoretical
    /// 
    /// And 2 types of observers 
    /// 1.  2° = 1931
    /// 2. 10° = 1964
    /// 
    /// This structure supports Observer = 2°, Illuminant = D65 by default.
    /// </remarks>
    public struct Xyz
    {
        #region Properties

        public static readonly double[] A = new double[] { 1.0985, 1.0, 0.3558 };

        public static readonly double[] C = new double[] { 0.9807, 1.0, 1.1822 };

        public static readonly double[] E = new double[] { 1.0, 1.0, 1.0 };

        public static readonly double[] D50 = new double[] { 0.9642, 1.0, 0.8251 };

        public static readonly double[] D55 = new double[] { 0.9568, 1.0, 0.9214 };

        public static readonly double[] D65 = new double[] { 0.95047, 1.0, 1.08883 };

        public static readonly double[] ICC = new double[] { 0.962, 1.0, 0.8249 };

        public struct MaxValue
        {
            public static double X = E[0];

            public static double Y = E[1];

            public static double Z = E[2];
        }

        public struct MinValue
        {
            public static double X = 0.0;

            public static double Y = 0.0;

            public static double Z = 0.0;
        }

        double x;
        /// <summary>
        /// Gets or sets the x component (0 to Illuminant.Max.X).
        /// </summary>
        public double X
        {
            get
            {
                return this.x;
            }
            set
            {
                this.x = value.Coerce(Xyz.MaxValue.X, Xyz.MinValue.X);
            }
        }

        double y;
        /// <summary>
        /// Gets or sets the y component (0 to Illuminant.Max.Y).
        /// </summary>
        public double Y
        {
            get
            {
                return this.y;
            }
            set
            {
                this.y = value.Coerce(Xyz.MaxValue.Y, Xyz.MinValue.Y);
            }
        }

        double z;
        /// <summary>
        /// Gets or sets the component (0 to Illuminant.Max.Z).
        /// </summary>
        public double Z
        {
            get
            {
                return this.z;
            }
            set
            {
                this.z = value.Coerce(Xyz.MaxValue.Z, Xyz.MinValue.Z);
            }
        }

        #endregion

        #region Xyz

        public static bool operator ==(Xyz a, Xyz b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Xyz a, Xyz b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        /// <summary>
        /// Creates an instance of a Xyz structure.
        /// </summary>
        public Xyz(double X, double Y, double Z)
        {
            this.x = X.Coerce(Xyz.MaxValue.X, Xyz.MinValue.X);
            this.y = Y.Coerce(Xyz.MaxValue.Y, Xyz.MinValue.Y);
            this.z = Z.Coerce(Xyz.MaxValue.Z, Xyz.MinValue.Z);
        }

        #endregion

        #region Methods

        public override bool Equals(Object Object)
        {
            if (Object == null || GetType() != Object.GetType()) return false;

            return (this == (Xyz)Object);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("X => {0}, Y => {1}, Z => {2}", this.X.ToString(), this.Y.ToString(), this.Z.ToString());
        }

        public static double Fxyz(double t)
        {
            return ((t > 0.008856) ? Math.Pow(t, (1.0 / 3.0)) : (7.787 * t + 16.0 / 116.0));
        }

        public static Xyz FromColor(Color Color)
        {
            return Xyz.FromRgba(Color.R, Color.G, Color.B, Color.A);
        }

        public static Xyz FromRgba(byte R, byte G, byte B, byte A = 255)
        {
            double[] Rgb = new double[3];
            Rgb[0] = R.ToDouble() / 255.0;
            Rgb[1] = G.ToDouble() / 255.0;
            Rgb[2] = B.ToDouble() / 255.0;

            for (int i = 0; i < 3; i++)
                Rgb[i] = Rgb[i] > 0.04045 ? Math.Pow((Rgb[i] + 0.055) / 1.055, 2.4) : Rgb[i] / 12.92;

            var x = Rgb[0] * 0.4124 + Rgb[1] * 0.3576 + Rgb[2] * 0.1805;
            var y = Rgb[0] * 0.2126 + Rgb[1] * 0.7152 + Rgb[2] * 0.0722;
            var z = Rgb[0] * 0.0193 + Rgb[1] * 0.1192 + Rgb[2] * 0.9505;

            return new Xyz(x, y, z);
        }

        public static Color ToColor(double x, double y, double z)
        {
            return Xyz.ToRgba(x, y, z).ToColor();
        }

        public static Rgba ToRgba(double x, double y, double z)
        {
            double[] Clinear = new double[3];
            Clinear[0] = x * 3.2406 + y * -1.5372 + z * -0.4986; //Red
            Clinear[1] = x * -0.9689 + y * 1.8758 + z * 0.0415;  //Green
            Clinear[2] = x * 0.0557 + y * -0.2040 + z * 1.0570;  //Blue

            for (int i = 0; i < 3; i++)
            {
                Clinear[i] = (Clinear[i] <= 0.0031308) ? 12.92 * Clinear[i] : (1.055 * Math.Pow(Clinear[i], 1.0 / 2.4)) - 0.055;
                Clinear[i] = Math.Round(Clinear[i] * 255.0);
                Clinear[i] = Clinear[i].Coerce(255.0);
            }
            return new Rgba(Clinear[0].ToInt32(), Clinear[1].ToInt32(), Clinear[2].ToInt32());
        }

        #endregion
    }
}
