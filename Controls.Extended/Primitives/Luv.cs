using Imagin.Common.Drawing;
using Imagin.Common.Extensions;
using System;
using System.Windows.Media;

namespace Imagin.Controls.Extended.Primitives
{
    /// <summary>
    /// Structure to define CIE L*uv.
    /// </summary>
    [Serializable]
    public struct Luv
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
            public static double L = 1d;
            /// <summary>
            /// 
            /// </summary>
            public static double U = 1.28;
            /// <summary>
            /// 
            /// </summary>
            public static double V = 1.28;
        }

        /// <summary>
        /// 
        /// </summary>
        public struct MinValue
        {
            /// <summary>
            /// 
            /// </summary>
            public static double L = 0;
            /// <summary>
            /// 
            /// </summary>
            public static double U = -1.27;
            /// <summary>
            /// 
            /// </summary>
            public static double V = -1.27;
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
            set
            {
                l = value.Coerce(MaxValue.L, MinValue.L);
            }
        }

        double u;
        /// <summary>
        /// Gets or sets the u component (-1 to 1).
        /// </summary>
        public double U
        {
            get
            {
                return u;
            }
            set
            {
                u = value.Coerce(MaxValue.U, MinValue.U);
            }
        }

        double v;
        /// <summary>
        /// Gets or sets the v component (-1 to 1).
        /// </summary>
        public double V
        {
            get
            {
                return v;
            }
            set
            {
                v = value.Coerce(MaxValue.V, MinValue.V);
            }
        }

        #endregion

        #region Luv
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Luv a, Luv b)
        {
            return a.L == b.L && a.U == b.U && a.V == b.V;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Luv a, Luv b)
        {
            return a.L != b.L || a.U != b.U || a.V != b.V;
        }

        /// <summary>
        /// Creates an instance of the <see cref="Luv"/> structure.
        /// </summary>
        public Luv(double L, double U, double V)
        {
            l = L.Coerce(MaxValue.L, MinValue.L);
            u = U.Coerce(MaxValue.U, MinValue.U);
            v = V.Coerce(MaxValue.V, MinValue.V);
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
            return Object == null || GetType() != Object.GetType() ? false : this == (Luv)Object;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return l.GetHashCode() ^ u.GetHashCode() ^ v.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "L => {0}, U => {1}, V => {2}".F(l, u, v);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Color"></param>
        /// <returns></returns>
        public static Luv FromColor(Color Color)
        {
            return FromRgba(Color.R, Color.G, Color.B, Color.A);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="R"></param>
        /// <param name="G"></param>
        /// <param name="B"></param>
        /// <param name="A"></param>
        /// <returns></returns>
        public static Luv FromRgba(byte R, byte G, byte B, byte A = 255)
        {
            Xyz Xyz = Xyz.FromRgba(R, G, B, A);
            return FromXyz(Xyz.X, Xyz.Y, Xyz.Z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        /// <param name="Illuminant"></param>
        /// <returns></returns>
        public static Luv FromXyz(double X, double Y, double Z, Illuminant Illuminant = Illuminant.Default)
        {
            var w = X + (15d * Y) + (3d * Z);
            var u = (4d * X) / w;
            var v = (9d * Y) / w;

            var y = Y > 0.008856 ? Math.Pow(Y, 1d / 3d) : (7.787 * Y) + (16d / 116d);

            var mx = Xyz.Max[Xyz.Component.X, Illuminant];
            var my = Xyz.Max[Xyz.Component.Y, Illuminant];
            var mz = Xyz.Max[Xyz.Component.Z, Illuminant];

            var rl = mx.Shift(2) + (15.0 * my.Shift(2)) + (3.0 * mz.Shift(2));
            var ru = (4.0 * mx.Shift(2)) / rl;
            var rv = (9.0 * my.Shift(2)) / rl;

            var l = (116d * y) - 16d;
            u = 13d * l * (u - ru);
            v = 13d * l * (v - rv);

            return new Luv(l.Divide(MaxValue.L), u.Divide(MaxValue.U), v.Divide(MaxValue.V));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="L"></param>
        /// <param name="U"></param>
        /// <param name="V"></param>
        /// <returns></returns>
        public static Color ToColor(double L, double U, double V)
        {
            return ToRgba(L, U, V).ToColor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="L"></param>
        /// <param name="U"></param>
        /// <param name="V"></param>
        /// <param name="Illuminant"></param>
        /// <returns></returns>
        public static Xyz ToXyz(double L, double U, double V, Illuminant Illuminant = Illuminant.Default)
        {
            var y = (L + 16.0) / 116.0;
            y = Math.Pow(y, 3.0) > 0.008856 ? Math.Pow(y, 3) : (y - 16.0 / 116) / 7.787;

            var mx = Xyz.Max[Xyz.Component.X, Illuminant];
            var my = Xyz.Max[Xyz.Component.Y, Illuminant];
            var mz = Xyz.Max[Xyz.Component.Z, Illuminant];

            var rl = mx.Shift(2) + (15.0 * my.Shift(2)) + (3.0 + mz.Shift(2));
            var ru = (4.0 * mx.Shift(2)) / rl;
            var rv = (9.0 * my.Shift(2)) / rl;

            var u = U / (13.0 * L) + ru;
            var v = V / (13.0 * L) + rv;

            y *= 100.0;
            var x = -(9.0 * y * u) / ((u - 4.0) * v - u * v);
            var z = (9 * y - (15.0 * v * y) - (v * x)) / (3.0 * v);

            return new Xyz(x, y, z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="L"></param>
        /// <param name="U"></param>
        /// <param name="V"></param>
        /// <returns></returns>
        public static Rgba ToRgba(double L, double U, double V)
        {
            Xyz Xyz = ToXyz(L, U, V);
            return Xyz.ToRgba(Xyz.X, Xyz.Y, Xyz.Z);
        }

        #endregion
    }
}
