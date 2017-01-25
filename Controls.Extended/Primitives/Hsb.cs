using Imagin.Common.Extensions;
using System;
using System.Windows.Media;

namespace Imagin.Controls.Extended.Primitives
{
    /// <summary>
    /// Structure to define a color for HSB (hue/saturation/brightness).
    /// </summary>
    [Serializable]
    public struct Hsb
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public struct MaxValue
        {
            public static double H = 1d;
            public static double S = 1d;
            public static double B = 1d;
        }

        /// <summary>
        /// 
        /// </summary>
        public struct MinValue
        {
            public static double H = 0;
            public static double S = 0;
            public static double B = 0;
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
            set
            {
                h = value.Coerce(Hsb.MaxValue.H, Hsb.MinValue.H);
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
            set
            {
                s = value.Coerce(Hsb.MaxValue.S, Hsb.MinValue.S);
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
            set
            {
                b = value.Coerce(Hsb.MaxValue.B, Hsb.MinValue.B);
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
        /// Creates an instance of a Hsb structure.
        /// </summary>
        public Hsb(double h, double s, double b)
        {
            this.h = h.Coerce(Hsb.MaxValue.H, Hsb.MinValue.H);
            this.s = s.Coerce(Hsb.MaxValue.S, Hsb.MinValue.S);
            this.b = b.Coerce(Hsb.MaxValue.B, Hsb.MinValue.B);
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
            if (Object == null || GetType() != Object.GetType()) return false;
            return (this == (Hsb)Object);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return H.GetHashCode() ^ S.GetHashCode() ^ B.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("H => {0}, S => {1}, B => {2}", this.H.ToString(), this.S.ToString(), this.B.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Color"></param>
        /// <returns></returns>
        public static Hsb FromColor(Color Color)
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
        public static Hsb FromRgba(byte R, byte G, byte B, byte A = 255)
        {
            var h = System.Drawing.Color.FromArgb(A, R, G, B).GetHue() / 359.0;

            var s = 0d;

            var Max = Math.Max(Math.Max(R, G), B).ToDouble() / 255.0;
            if (Max == 0)
                s = 0d;
            else
            {
                var Min = Math.Min(Math.Min(R, G), B).ToDouble() / 255.0;
                s = (Max - Min) / Max;
            }

            var b = Math.Max(Math.Max(R, G), B).ToDouble() / 255.0;

            return new Hsb(h, s, b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="H"></param>
        /// <param name="S"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Color ToColor(double H, double S, double B)
        {
            return ToRgba(H, S, B).ToColor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="H"></param>
        /// <param name="S"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Rgba ToRgba(double H, double S, double B)
        {
            var r = 0d;
            var g = 0d;
            var b = 0d;

            if (S == 0)
            {
                r = g = b = B;
            }
            else
            {
                H *= 359d;

                //The color wheel consists of 6 sectors: Figure out which sector we're in...
                var SectorPosition = H / 60d;
                var SectorNumber = Math.Floor(SectorPosition).ToInt32();

                //Get the fractional part of the sector
                var FractionalSector = SectorPosition - SectorNumber;

                // calculate values for the three axes of the color. 
                double p = B * (1d - S);
                double q = B * (1d - (S * FractionalSector));
                double t = B * (1d - (S * (1d - FractionalSector)));

                // assign the fractional colors to r, g, and b based on the sector the angle is in.
                switch (SectorNumber)
                {
                    case 0:
                        r = B;
                        g = t;
                        b = p;
                        break;
                    case 1:
                        r = q;
                        g = B;
                        b = p;
                        break;
                    case 2:
                        r = p;
                        g = B;
                        b = t;
                        break;
                    case 3:
                        r = p;
                        g = q;
                        b = B;
                        break;
                    case 4:
                        r = t;
                        g = p;
                        b = B;
                        break;
                    case 5:
                        r = B;
                        g = p;
                        b = q;
                        break;
                }
            }

            r = r.Multiply(Rgba.MaxValue.R).Coerce(Rgba.MaxValue.R);
            g = g.Multiply(Rgba.MaxValue.G).Coerce(Rgba.MaxValue.G);
            b = b.Multiply(Rgba.MaxValue.B).Coerce(Rgba.MaxValue.B);

            return new Rgba(r.ToByte(), g.ToByte(), b.ToByte());
        }

        #endregion
    }
}
