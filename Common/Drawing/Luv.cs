using Imagin.Common.Extensions;
using System;
using System.Windows.Media;

namespace Imagin.Common.Drawing
{
    /// <summary>
    /// Structure to define CIE LUV color.
    /// </summary>
    [Serializable]
    public struct Luv : IColor, IReflectiveColor
    {
        #region Properties

        /// <summary>
        /// Specifies a <see cref="Luv"/> component.
        /// </summary>
        public enum Component
        {
            /// <summary>
            /// Specifies the <see cref="Luv.L"/> component.
            /// </summary>
            L,
            /// <summary>
            /// Specifies the <see cref="Luv.U"/> component.
            /// </summary>
            U,
            /// <summary>
            /// Specifies the <see cref="Luv.V"/> component.
            /// </summary>
            V
        }

        /// <summary>
        /// 
        /// </summary>
        public static class MaxValue
        {
            /// <summary>
            /// 
            /// </summary>
            public const double L = 100d;
            /// <summary>
            /// 
            /// </summary>
            public const double U = 224d;
            /// <summary>
            /// 
            /// </summary>
            public const double V = 122d;
        }

        /// <summary>
        /// 
        /// </summary>
        public static class MinValue
        {
            /// <summary>
            /// 
            /// </summary>
            public const double L = 0;
            /// <summary>
            /// 
            /// </summary>
            public const double U = -134d;
            /// <summary>
            /// 
            /// </summary>
            public const double V = -140d;
        }

        /// <summary>
        /// 
        /// </summary>
        public static class Range
        {
            /// <summary>
            /// 
            /// </summary>
            public static double L = MaxValue.L.Add(MinValue.L.Abs());
            /// <summary>
            /// 
            /// </summary>
            public static double U = MaxValue.U.Add(MinValue.U.Abs());
            /// <summary>
            /// 
            /// </summary>
            public static double V = MaxValue.V.Add(MinValue.V.Abs());
        }

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
            private set
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
            private set
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
            private set
            {
                v = value.Coerce(MaxValue.V, MinValue.V);
            }
        }

        Illuminant illuminant;
        /// <summary>
        /// 
        /// </summary>
        public Illuminant Illuminant
        {
            get
            {
                return illuminant;
            }
            private set
            {
                illuminant = value;
            }
        }

        ObserverAngle observer;
        /// <summary>
        /// 
        /// </summary>
        public ObserverAngle Observer
        {
            get
            {
                return observer;
            }
            private set
            {
                observer = value;
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
        /// 
        /// </summary>
        /// <param name="Color"></param>
        /// <param name="Observer"></param>
        /// <param name="Illuminant"></param>
        public Luv(Color Color, ObserverAngle Observer = ObserverAngle.Two, Illuminant Illuminant = Illuminant.Default) : this(new Rgba(Color), Observer, Illuminant)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Rgba"></param>
        /// <param name="Observer"></param>
        /// <param name="Illuminant"></param>
        public Luv(Rgba Rgba, ObserverAngle Observer = ObserverAngle.Two, Illuminant Illuminant = Illuminant.Default) : this(new Xyz(Rgba), Observer, Illuminant)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Xyz"></param>
        /// <param name="Observer"></param>
        /// <param name="Illuminant"></param>
        public Luv(Xyz Xyz, ObserverAngle Observer = ObserverAngle.Two, Illuminant Illuminant = Illuminant.Default) : this(0, 0, 0, Observer, Illuminant)
        {
            var w = Xyz.X + (15d * Xyz.Y) + (3d * Xyz.Z);
            var u = (4d * Xyz.X) / w;
            var v = (9d * Xyz.Y) / w;

            var y = Xyz.Y / 100d;
            y = Xyz.Y > 0.008856 ? Math.Pow(Xyz.Y, 1d / 3d) : (7.787 * Xyz.Y) + (16d / 116d);

            var mx = Xyz.Max[Xyz.Component.X, Observer, Illuminant].Shift(2);
            var my = Xyz.Max[Xyz.Component.Y, Observer, Illuminant].Shift(2);
            var mz = Xyz.Max[Xyz.Component.Z, Observer, Illuminant].Shift(2);

            var rl = mx + (15d * my) + (3d * mz);
            var ru = (4d * mx) / rl;
            var rv = (9d * my) / rl;

            var l = (116d * y) - 16d;
            u = 13d * l * (u - ru);
            v = 13d * l * (v - rv);

            L = l;
            U = u;
            V = v;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Luv"/> structure.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="observer"></param>
        /// <param name="illuminant"></param>
        public Luv(double l, double u, double v, ObserverAngle observer = ObserverAngle.Two, Illuminant illuminant = Illuminant.Default) : this()
        {
            Observer = observer;
            Illuminant = illuminant;

            L = l;
            U = u;
            V = v;
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
        /// <param name="Component"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public Luv New(Component Component, double Value)
        {
            switch (Component)
            {
                case Component.L:
                    l = Value;
                    break;
                case Component.U:
                    u = Value;
                    break;
                case Component.V:
                    v = Value;
                    break;
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Rgba ToRgba()
        {
            return ToXyz().ToRgba();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Xyz ToXyz()
        {
            var y = (L + 16d) / 116d;
            y = Math.Pow(y, 3d) > 0.008856 ? Math.Pow(y, 3d) : (y - 16d / 116d) / 7.787;

            var mx = Xyz.Max[Xyz.Component.X, Observer, Illuminant].Shift(2);
            var my = Xyz.Max[Xyz.Component.Y, Observer, Illuminant].Shift(2);
            var mz = Xyz.Max[Xyz.Component.Z, Observer, Illuminant].Shift(2);

            var rl = mx + (15d * my) + (3d + mz);
            var ru = (4d * mx) / rl;
            var rv = (9d * my) / rl;

            var u = U / (13d * L) + ru;
            var v = V / (13d * L) + rv;

            y *= 100d;
            var x = -(9d * y * u) / ((u - 4d) * v - u * v);
            var z = (9 * y - (15d * v * y) - (v * x)) / (3d * v);

            return new Xyz(x.Shift(-2), y.Shift(-2), z.Shift(-2), Observer, Illuminant);
        }

        #endregion
    }
}
