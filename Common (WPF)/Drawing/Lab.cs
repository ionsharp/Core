using Imagin.Common.Linq;
using System;
using System.Windows.Media;

namespace Imagin.Common.Drawing
{
    /// <summary>
    /// Structure to define CIE LAB color.
    /// </summary>
    [Serializable]
    public struct Lab : IColor, IReflectiveColor
    {
        #region Properties

        /// <summary>
        /// Specifies a <see cref="Lab"/> component.
        /// </summary>
        public enum Component
        {
            /// <summary>
            /// Specifies the <see cref="Lab.L"/> component.
            /// </summary>
            L,
            /// <summary>
            /// Specifies the <see cref="Lab.A"/> component.
            /// </summary>
            A,
            /// <summary>
            /// Specifies the <see cref="Lab.B"/> component.
            /// </summary>
            B
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
            public const double A = 128d;
            /// <summary>
            /// 
            /// </summary>
            public const double B = 128d;
        }


        /// <summary>
        /// 
        /// </summary>
        public static class MinValue
        {
            /// <summary>
            /// 
            /// </summary>
            public const double L = 0d;
            /// <summary>
            /// 
            /// </summary>
            public const double A = -127d;
            /// <summary>
            /// 
            /// </summary>
            public const double B = -127d;
        }

        /// <summary>
        /// 
        /// </summary>
        public static class Range
        {
            /// <summary>
            /// 
            /// </summary>
            public static double L
            {
                get
                {
                    return MaxValue.L.Add(MinValue.L.Abs());
                }
            }
            /// <summary>
            /// 
            /// </summary>
            public static double A
            {
                get
                {
                    return MaxValue.A.Add(MinValue.A.Abs());
                }
            }
            /// <summary>
            /// 
            /// </summary>
            public static double B
            {
                get
                {
                    return MaxValue.B.Add(MinValue.B.Abs());
                }
            }
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
        /// Gets or sets the luminance component (0 to 100).
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
            private set
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
            private set
            {
                b = value.Coerce(MaxValue.B, MinValue.B);
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
        /// 
        /// </summary>
        /// <param name="Color"></param>
        /// <param name="Observer"></param>
        /// <param name="Illuminant"></param>
        public Lab(Color Color, ObserverAngle Observer = ObserverAngle.Two, Illuminant Illuminant = Illuminant.Default) : this(new Rgba(Color), Observer, Illuminant)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Rgba"></param>
        /// <param name="Observer"></param>
        /// <param name="Illuminant"></param>
        public Lab(Rgba Rgba, ObserverAngle Observer = ObserverAngle.Two, Illuminant Illuminant = Illuminant.Default) : this(0, 0, 0, Observer, Illuminant)
        {
            var RLinear = Rgba.R.ToDouble() / 255d;
            var GLinear = Rgba.G.ToDouble() / 255d;
            var BLinear = Rgba.B.ToDouble() / 255d;

            RLinear = (RLinear > 0.04045) ? Math.Pow((RLinear + 0.055) / (1 + 0.055), 2.2) : (RLinear / 12.92);
            GLinear = (GLinear > 0.04045) ? Math.Pow((GLinear + 0.055) / (1 + 0.055), 2.2) : (GLinear / 12.92);
            BLinear = (BLinear > 0.04045) ? Math.Pow((BLinear + 0.055) / (1 + 0.055), 2.2) : (BLinear / 12.92);

            double x = RLinear * 0.4124 + GLinear * 0.3576 + BLinear * 0.1805;
            double y = RLinear * 0.2126 + GLinear * 0.7152 + BLinear * 0.0722;
            double z = RLinear * 0.0193 + GLinear * 0.1192 + BLinear * 0.9505;

            var mx = Xyz.Max[Xyz.Component.X, Observer, Illuminant];
            var my = Xyz.Max[Xyz.Component.Y, Observer, Illuminant];
            var mz = Xyz.Max[Xyz.Component.Z, Observer, Illuminant];

            L = 116d * Xyz.Fxyz(y / my) - 16d;
            A = 500d * (Xyz.Fxyz(x / mx) - Xyz.Fxyz(y / my));
            B = 200d * (Xyz.Fxyz(y / my) - Xyz.Fxyz(z / mz));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Lab"/> structure.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="observer"></param>
        /// <param name="illuminant"></param>
        public Lab(double l, double a, double b, ObserverAngle observer = ObserverAngle.Two, Illuminant illuminant = Illuminant.Default) : this()
        {
            Observer = Observer;
            Illuminant = Illuminant;

            L = l;
            A = a;
            B = b;
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
        /// <param name="Component"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public Lab New(Component Component, double Value)
        {
            switch (Component)
            {
                case Component.L:
                    l = Value;
                    break;
                case Component.A:
                    a = Value;
                    break;
                case Component.B:
                    b = Value;
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
            double theta = 6.0 / 29.0;

            double fy = (l + 16.0) / 116.0;
            double fx = fy + (a / 500.0);
            double fz = fy - (b / 200.0);

            var mx = Xyz.Max[Xyz.Component.X, Observer, Illuminant];
            var my = Xyz.Max[Xyz.Component.Y, Observer, Illuminant];
            var mz = Xyz.Max[Xyz.Component.Z, Observer, Illuminant];

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
