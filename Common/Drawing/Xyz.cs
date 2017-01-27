using Imagin.Common.Extensions;
using System;
using System.Windows.Media;

namespace Imagin.Common.Drawing
{
    /// <summary>
    /// Structure to define CIE XYZ color.
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
    [Serializable]
    public struct Xyz : IColor, IReflectiveColor
    {
        #region Properties

        /// <summary>
        /// Specifies a <see cref="Xyz"/> component.
        /// </summary>
        public enum Component
        {
            /// <summary>
            /// Specifies the <see cref="Xyz.X"/> component.
            /// </summary>
            X,
            /// <summary>
            /// Specifies the <see cref="Xyz.Y"/> component.
            /// </summary>
            Y,
            /// <summary>
            /// Specifies the <see cref="Xyz.Z"/> component.
            /// </summary>
            Z
        }

        static MaxValue maxValue = new MaxValue();
        /// <summary>
        /// 
        /// </summary>
        public static MaxValue Max
        {
            get
            {
                return maxValue;
            }
        }

        /// <summary>
        /// TO-DO: Disallow instance creation (which does nothing).
        /// </summary>
        public struct MaxValue
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Component"></param>
            /// <param name="Illuminant"></param>
            /// <returns></returns>
            public double this[Component Component, ObserverAngle Observer, Illuminant Illuminant]
            {
                get
                {
                    switch (Illuminant)
                    {
                        case Illuminant.A:
                            switch (Component)
                            {
                                case Component.X:
                                    return 1.0985;
                                case Component.Y:
                                    return 1d;
                                case Component.Z:
                                    return 0.3558;
                            }
                            break;
                        case Illuminant.C:
                            switch (Component)
                            {
                                case Component.X:
                                    return 0.9807;
                                case Component.Y:
                                    return 1d;
                                case Component.Z:
                                    return 1.1822;
                            }
                            break;
                        case Illuminant.E:
                            switch (Component)
                            {
                                case Component.X:
                                    return 1d;
                                case Component.Y:
                                    return 1d;
                                case Component.Z:
                                    return 1d;
                            }
                            break;
                        case Illuminant.D50:
                            switch (Component)
                            {
                                case Component.X:
                                    return 0.9642;
                                case Component.Y:
                                    return 1d;
                                case Component.Z:
                                    return 0.8251;
                            }
                            break;
                        case Illuminant.D55:
                            switch (Component)
                            {
                                case Component.X:
                                    return 0.9568;
                                case Component.Y:
                                    return 1d;
                                case Component.Z:
                                    return 0.9214;
                            }
                            break;
                        case Illuminant.ICC:
                            switch (Component)
                            {
                                case Component.X:
                                    return 0.962;
                                case Component.Y:
                                    return 1d;
                                case Component.Z:
                                    return 0.8249;
                            }
                            break;
                        case Illuminant.D65:
                        default:
                            switch (Component)
                            {
                                case Component.X:
                                    return 0.95047;
                                case Component.Y:
                                    return 1d;
                                case Component.Z:
                                    return 1.08883;
                            }
                            break;
                    }
                    return 0d;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static class MinValue
        {
            /// <summary>
            /// 
            /// </summary>
            public const double X = 0;
            /// <summary>
            /// 
            /// </summary>
            public const double Y = 0;
            /// <summary>
            /// 
            /// </summary>
            public const double Z = 0;
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

        double x;
        /// <summary>
        /// Gets or sets the x component (0 to Illuminant.Max.X).
        /// </summary>
        public double X
        {
            get
            {
                return x;
            }
            private set
            {
                x = value.Coerce(Max[Component.X, Observer, Illuminant], MinValue.X);
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
                return y;
            }
            private set
            {
                y = value.Coerce(Max[Component.Y, Observer, Illuminant], MinValue.Y);
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
                return z;
            }
            private set
            {
                z = value.Coerce(Max[Component.Z, Observer, Illuminant], MinValue.Z);
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

        #region Xyz

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Xyz a, Xyz b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Xyz a, Xyz b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Color"></param>
        /// <param name="Observer"></param>
        /// <param name="Illuminant"></param>
        public Xyz(Color Color, ObserverAngle Observer = ObserverAngle.Two, Illuminant Illuminant = Illuminant.Default) : this(new Rgba(Color), Observer, Illuminant)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Rgba"></param>
        /// <param name="Observer"></param>
        /// <param name="Illuminant"></param>
        public Xyz(Rgba Rgba, ObserverAngle Observer = ObserverAngle.Two, Illuminant Illuminant = Illuminant.Default) : this(0, 0, 0, Observer, Illuminant)
        {
            var Rgb = new double[3];

            Rgb[0] = Rgba.Linear(Rgba.R);
            Rgb[1] = Rgba.Linear(Rgba.G);
            Rgb[2] = Rgba.Linear(Rgba.B);

            for (int i = 0; i < 3; i++)
                Rgb[i] = Rgb[i] > 0.04045 ? Math.Pow((Rgb[i] + 0.055) / 1.055, 2.4) : Rgb[i] / 12.92;

            X = Rgb[0] * 0.4124 + Rgb[1] * 0.3576 + Rgb[2] * 0.1805;
            Y = Rgb[0] * 0.2126 + Rgb[1] * 0.7152 + Rgb[2] * 0.0722;
            Z = Rgb[0] * 0.0193 + Rgb[1] * 0.1192 + Rgb[2] * 0.9505;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Xyz"/> structure.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="observer"></param>
        /// <param name="illuminant"></param>
        public Xyz(double x, double y, double z, ObserverAngle observer = ObserverAngle.Two, Illuminant illuminant = Illuminant.Default) : this()
        {
            Observer = observer;
            Illuminant = illuminant;

            X = x;
            Y = y;
            Z = z;
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
            return Object == null || GetType() != Object.GetType() ? false : this == (Xyz)Object;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "X => {0}, Y => {1}, Z => {2}".F(x, y, z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        internal static double Fxyz(double t)
        {
            return t > 0.008856 ? Math.Pow(t, (1.0 / 3.0)) : 7.787 * t + 16.0 / 116.0;
        }

        /// <summary>
        /// Clones current <see cref="Xyz"/> instance and sets given component value.
        /// </summary>
        /// <param name="Component"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public Xyz New(Component Component, double Value)
        {
            switch (Component)
            {
                case Component.X:
                    x = Value;
                    break;
                case Component.Y:
                    y = Value;
                    break;
                case Component.Z:
                    z = Value;
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
            var Clinear = new double[3];
            Clinear[0] = x * 3.2406 + y * -1.5372 + z * -0.4986; //Red
            Clinear[1] = x * -0.9689 + y * 1.8758 + z * 0.0415;  //Green
            Clinear[2] = x * 0.0557 + y * -0.2040 + z * 1.0570;  //Blue

            for (int i = 0; i < 3; i++)
            {
                Clinear[i] = (Clinear[i] <= 0.0031308) ? 12.92 * Clinear[i] : (1.055 * Math.Pow(Clinear[i], 1.0 / 2.4)) - 0.055;
                Clinear[i] = Math.Round(Clinear[i] * 255d);
                Clinear[i] = Clinear[i].Coerce(255d);
            }
            return new Rgba(Clinear[0].ToInt32(), Clinear[1].ToInt32(), Clinear[2].ToInt32());
        }

        #endregion
    }
}
