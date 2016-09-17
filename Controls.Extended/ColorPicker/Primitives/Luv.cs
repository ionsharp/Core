using Imagin.Common.Extensions;
using System;
using System.Windows.Media;

namespace Imagin.Controls.Extended.Primitives
{
    [Serializable]
    /// <summary>
    /// Structure to define CIE L*uv.
    /// </summary>
    public struct Luv
    {
        #region Properties

        double l;
        /// <summary>
        /// Gets or sets the lightness component (0 to 100).
        /// </summary>
        public double L
        {
            get
            {
                return l;
            }
            set
            {
                l = value;
            }
        }

        double u;
        /// <summary>
        /// Gets or sets the saturation component (0 to 1).
        /// </summary>
        public double U
        {
            get
            {
                return u;
            }
            set
            {
                u = value;
            }
        }

        double v;
        /// <summary>
        /// Gets or sets the brightness component (0 to 1).
        /// </summary>
        public double V
        {
            get
            {
                return v;
            }
            set
            {
                v = value;
            }
        }

        #endregion

        #region Luv

        public static bool operator ==(Luv a, Luv b)
        {
            return a.L == b.L && a.U == b.U && a.V == b.V;
        }

        public static bool operator !=(Luv a, Luv b)
        {
            return a.L != b.L || a.U != b.U || a.V != b.V;
        }

        /// <summary>
        /// Creates an instance of a Luv structure.
        /// </summary>
        public Luv(double L, double U, double V)
        {
            this.l = L;
            this.u = U;
            this.v = V;
        }

        #endregion

        #region Methods

        public override bool Equals(Object Object)
        {
            if (Object == null || GetType() != Object.GetType()) return false;
            return (this == (Luv)Object);
        }

        public override int GetHashCode()
        {
            return L.GetHashCode() ^ U.GetHashCode() ^ V.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("L => {0}, U => {1}, V => {2}", this.L.ToString(), this.U.ToString(), this.V.ToString());
        }

        public static Luv FromColor(Color Color)
        {
            return Luv.FromRgba(Color.R, Color.G, Color.B, Color.A);
        }

        public static Luv FromRgba(byte R, byte G, byte B, byte A = 255)
        {
            Xyz Xyz = Xyz.FromRgba(R, G, B, A);
            return Luv.FromXyz(Xyz.X, Xyz.Y, Xyz.Z);
        }

        public static Luv FromXyz(double X, double Y, double Z)
        {
            var w = X + (15.0 * Y) + (3.0 * Z);
            var u = (4.0 * X) / w;
            var v = (9.0 * Y) / w;

            var y = Y / 100.0;
            y = y > 0.008856 ? Math.Pow(y, 1.0 / 3.0) : (7.787 * y) + (16.0 / 116.0);

            var rl = Xyz.MaxValue.X.Shift(2) + (15.0 * Xyz.MaxValue.Y.Shift(2)) + (3.0 * Xyz.MaxValue.Z.Shift(2));
            var ru = (4.0 * Xyz.MaxValue.X.Shift(2)) / rl;
            var rv = (9.0 * Xyz.MaxValue.Y.Shift(2)) / rl;

            var l = (116.0 * y) - 16.0;
            u = 13.0 * l * (u - ru);
            v = 13.0 * l * (v - rv);

            return new Luv(l, u, v);
        }

        public static Color ToColor(double L, double U, double V)
        {
            return Luv.ToRgba(L, U, V).ToColor();
        }

        public static Xyz ToXyz(double L, double U, double V)
        {
            var y = (L + 16.0) / 116.0;
            y = Math.Pow(y, 3.0) > 0.008856 ? Math.Pow(y, 3) : (y - 16.0 / 116) / 7.787;

            var rl = Xyz.MaxValue.X.Shift(2) + (15.0 * Xyz.MaxValue.Y.Shift(2)) + (3.0 + Xyz.MaxValue.Z.Shift(2));
            var ru = (4.0 * Xyz.MaxValue.X.Shift(2)) / rl;
            var rv = (9.0 * Xyz.MaxValue.Y.Shift(2)) / rl;

            var u = U / (13.0 * L) + ru;
            var v = V / (13.0 * L) + rv;

            y *= 100.0;
            var x = -(9.0 * y * u) / ((u - 4.0) * v - u * v);
            var z = (9 * y - (15.0 * v * y) - (v * x)) / (3.0 * v);

            return new Xyz(x, y, z);
        }

        public static Rgba ToRgba(double L, double U, double V)
        {
            Xyz Xyz = Luv.ToXyz(L, U, V);
            return Xyz.ToRgba(Xyz.X, Xyz.Y, Xyz.Z);
        }

        #endregion
    }
}
