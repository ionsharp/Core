using Imagin.Common.Linq;
using System;
using System.Windows.Media;

namespace Imagin.Common.Drawing
{
    /// <summary>
    /// Structure to define LCH color.
    /// </summary>
    [Serializable]
    public struct Lch : IColor, IReflectiveColor
    {
        #region Properties

        /// <summary>
        /// Specifies a <see cref="Lch"/> component.
        /// </summary>
        public enum Component
        {
            /// <summary>
            /// Specifies the <see cref="Lch.L"/> component.
            /// </summary>
            L,
            /// <summary>
            /// Specifies the <see cref="Lch.C"/> component.
            /// </summary>
            C,
            /// <summary>
            /// Specifies the <see cref="Lch.H"/> component.
            /// </summary>
            H
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
            public const double C = 100d;
            /// <summary>
            /// 
            /// </summary>
            public const double H = 359d;
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
            public const double C = 0d;
            /// <summary>
            /// 
            /// </summary>
            public const double H = 0d;
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

        double c;
        /// <summary>
        /// Gets or sets the chroma component (0 to 100).
        /// </summary>
        public double C
        {
            get
            {
                return c;
            }
            private set
            {
                c = value.Coerce(MaxValue.C, MinValue.C);
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
                return h;
            }
            private set
            {
                h = value.Coerce(MaxValue.H, MinValue.H);
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

        #region Lch

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Lch a, Lch b)
        {
            return a.L == b.L && a.C == b.C && a.H == b.H;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Lch a, Lch b)
        {
            return a.L != b.L || a.C != b.C || a.H != b.H;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Color"></param>
        /// <param name="Observer"></param>
        /// <param name="Illuminant"></param>
        public Lch(Color Color, ObserverAngle Observer = ObserverAngle.Two, Illuminant Illuminant = Illuminant.Default) : this(new Rgba(Color), Observer, Illuminant)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Rgba"></param>
        /// <param name="Observer"></param>
        /// <param name="Illuminant"></param>
        public Lch(Rgba Rgba, ObserverAngle Observer = ObserverAngle.Two, Illuminant Illuminant = Illuminant.Default) : this(new Lab(Rgba), Observer, Illuminant)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Lab"></param>
        /// <param name="Observer"></param>
        /// <param name="Illuminant"></param>
        public Lch(Lab Lab, ObserverAngle Observer = ObserverAngle.Two, Illuminant Illuminant = Illuminant.Default) : this(0, 0, 0, Observer, Illuminant)
        {
            double l, c, h = Math.Atan2(Lab.B, Lab.A);

            if (h > 0d)
            {
                h = (h / Math.PI) * 180d;
            }
            else h = 360d - (h.Abs() / Math.PI) * 180d;

            l = Lab.L;
            c = Math.Sqrt(Math.Pow(Lab.A, 2d) + Math.Pow(Lab.B, 2d));

            L = l;
            C = c;
            H = h;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Lch"/> structure.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="c"></param>
        /// <param name="h"></param>
        /// <param name="observer"></param>
        /// <param name="illuminant"></param>
        public Lch(double l, double c, double h, ObserverAngle observer = ObserverAngle.Two, Illuminant illuminant = Illuminant.Default) : this()
        {
            Observer = observer;
            Illuminant = illuminant;

            L = l;
            C = c;
            H = h;
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
            return Object == null || GetType() != Object.GetType() ? false : this == (Lch)Object;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return l.GetHashCode() ^ c.GetHashCode() ^ h.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "L => {0}, C => {1}, H => {2}".F(l, c, h);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Lch New(Component Component, double Value)
        {
            switch (Component)
            {
                case Component.L:
                    l = Value;
                    break;
                case Component.C:
                    c = Value;
                    break;
                case Component.H:
                    h = Value;
                    break;
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Lab ToLab()
        {
            double l = L, a, b;
            a = Math.Round(Math.Cos(H.ToRadians()) * C);
            b = Math.Round(Math.Sin(H.ToRadians()) * C);
            return new Lab(l, a, b, Observer, Illuminant);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Rgba ToRgba()
        {
            return ToLab().ToRgba();
        }

        #endregion
    }
}
