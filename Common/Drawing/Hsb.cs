using Imagin.Common.Extensions;
using System;
using System.Windows.Media;

namespace Imagin.Common.Drawing
{
    /// <summary>
    /// Structure to define a color for HSB (hue/saturation/brightness).
    /// </summary>
    [Serializable]
    public struct Hsb : IColor
    {
        #region Properties

        /// <summary>
        /// Specifies a HSB component.
        /// </summary>
        public enum Component
        {
            /// <summary>
            /// Specifies the <see cref="Hsb.H"/> component.
            /// </summary>
            H,
            /// <summary>
            /// Specifies the <see cref="Hsb.S"/> component.
            /// </summary>
            S,
            /// <summary>
            /// Specifies the <see cref="Hsb.B"/> component.
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
            public const double H = 359d;
            /// <summary>
            /// 
            /// </summary>
            public const double S = 100d;
            /// <summary>
            /// 
            /// </summary>
            public const double B = 100d;
        }

        /// <summary>
        /// 
        /// </summary>
        public static class MinValue
        {
            /// <summary>
            /// 
            /// </summary>
            public const double H = 0;
            /// <summary>
            /// 
            /// </summary>
            public const double S = 0;
            /// <summary>
            /// 
            /// </summary>
            public const double B = 0;
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

        double h;
        /// <summary>
        /// Gets or sets the hue component (0 to 1).
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

        double s;
        /// <summary>
        /// Gets or sets the saturation component (0 to 1).
        /// </summary>
        public double S
        {
            get
            {
                return s;
            }
            private set
            {
                s = value.Coerce(MaxValue.S, MinValue.S);
            }
        }

        double b;
        /// <summary>
        /// Gets or sets the brightness component (0 to 1).
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

        #endregion

        #region Hsb

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Hsb a, Hsb b)
        {
            return a.H == b.H && a.S == b.S && a.B == b.B;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Hsb a, Hsb b)
        {
            return a.H != b.H || a.S != b.S || a.B != b.B;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Color"></param>
        public Hsb(Color Color) : this(new Rgba(Color))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Rgba"></param>
        public Hsb(Rgba Rgba) : this(0, 0, 0)
        {
            var h = System.Drawing.Color.FromArgb(Rgba.A, Rgba.R, Rgba.G, Rgba.B).GetHue() / 359d;

            var s = 0d;

            var Max = Math.Max(Math.Max(Rgba.R, Rgba.G), Rgba.B).ToDouble() / 255.0;
            if (Max == 0)
                s = 0d;
            else
            {
                var Min = Math.Min(Math.Min(Rgba.R, Rgba.G), Rgba.B).ToDouble() / 255.0;
                s = (Max - Min) / Max;
            }

            var b = Math.Max(Math.Max(Rgba.R, Rgba.G), Rgba.B).ToDouble() / 255.0;

            H = h.Multiply(MaxValue.H);
            S = s.Multiply(MaxValue.S);
            B = b.Multiply(MaxValue.B);
        }

        /// <summary>
        /// Creates an instance of the <see cref="Hsb"/> structure.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="b"></param>
        public Hsb(double h, double s, double b) : this()
        {
            H = h;
            S = s;
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
            return Object == null || GetType() != Object.GetType() ? false : this == (Hsb)Object;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return h.GetHashCode() ^ s.GetHashCode() ^ b.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "H => {0}, S => {1}, B => {2}".F(h, s, b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Component"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public Hsb New(Component Component, double Value)
        {
            switch (Component)
            {
                case Component.H:
                    h = Value;
                    break;
                case Component.S:
                    s = Value;
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
            double h = this.h / MaxValue.H, s = this.s / MaxValue.S, b = this.b / MaxValue.B;

            double R = 0, G = 0, B = 0;

            if (s == 0)
            {
                R = G = B = b;
            }
            else
            {
                h *= MaxValue.H;

                //The color wheel consists of 6 sectors: Figure out which sector we're in...
                var SectorPosition = h / 60d;
                var SectorNumber = Math.Floor(SectorPosition).ToInt32();

                //Get the fractional part of the sector
                var FractionalSector = SectorPosition - SectorNumber;

                //Calculate values for the three axes of the color. 
                var p = b * (1d - s);
                var q = b * (1d - (s * FractionalSector));
                var t = b * (1d - (s * (1d - FractionalSector)));

                //Assign the fractional colors to r, g, and b based on the sector the angle is in.
                switch (SectorNumber)
                {
                    case 0:
                        R = b;
                        G = t;
                        B = p;
                        break;
                    case 1:
                        R = q;
                        G = b;
                        B = p;
                        break;
                    case 2:
                        R = p;
                        G = b;
                        B = t;
                        break;
                    case 3:
                        R = p;
                        G = q;
                        B = b;
                        break;
                    case 4:
                        R = t;
                        G = p;
                        B = b;
                        break;
                    case 5:
                        R = b;
                        G = p;
                        B = q;
                        break;
                }
            }

            R = R.Multiply(Rgba.MaxValue).Coerce(Rgba.MaxValue).Round();
            G = G.Multiply(Rgba.MaxValue).Coerce(Rgba.MaxValue).Round();
            B = B.Multiply(Rgba.MaxValue).Coerce(Rgba.MaxValue).Round();

            return new Rgba(R, G, B);
        }

        #endregion
    }
}
