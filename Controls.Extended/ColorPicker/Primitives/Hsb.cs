using Imagin.Common.Extensions;
using System;
using System.Windows.Media;

namespace Imagin.Controls.Extended.Primitives
{
    [Serializable]
    /// <summary>
    /// Structure to define HSB.
    /// </summary>
    public struct Hsb
    {
        #region Properties

        public struct MaxValue
        {
            public static double H = 1.0;

            public static double S = 1.0;

            public static double B = 1.0;
        }

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

        public static bool operator ==(Hsb a, Hsb b)
        {
            return a.H == b.H && a.S == b.S && a.B == b.B;
        }

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

        public override bool Equals(Object Object)
        {
            if (Object == null || GetType() != Object.GetType()) return false;
            return (this == (Hsb)Object);
        }

        public override int GetHashCode()
        {
            return H.GetHashCode() ^ S.GetHashCode() ^ B.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("H => {0}, S => {1}, B => {2}", this.H.ToString(), this.S.ToString(), this.B.ToString());
        }

        public static Hsb FromColor(Color Color)
        {
            return Hsb.FromRgba(Color.R, Color.G, Color.B, Color.A);
        }

        public static Hsb FromRgba(byte R, byte G, byte B, byte A = 255)
        {
            var h = System.Drawing.Color.FromArgb(A, R, G, B).GetHue() / 359.0;

            var s = 0.0;
            var Max = Math.Max(Math.Max(R, G), B).ToDouble() / 255.0;
            if (Max == 0)
                s = 0.0;
            else
            {
                var Min = Math.Min(Math.Min(R, G), B).ToDouble() / 255.0;
                s = (Max - Min) / Max;
            }

            var b = Math.Max(Math.Max(R, G), B).ToDouble() / 255.0;

            return new Hsb(h, s, b);
        }

        public static Color ToColor(double hue, double saturation, double brightness)
        {
            return Hsb.ToRgba(hue, saturation, brightness).ToColor();
        }

        public static Rgba ToRgba(double hue, double saturation, double brightness)
        {
            double r = 0;
            double g = 0;
            double b = 0;

            if (saturation == 0)
            {
                r = g = b = brightness;
            }
            else
            {
                hue *= 359.0;

                // the color wheel consists of 6 sectors. Figure out which sector you're in.
                double sectorPos = hue / 60.0;
                int sectorNumber = Math.Floor(sectorPos).ToInt();
                // get the fractional part of the sector
                double fractionalSector = sectorPos - sectorNumber;

                // calculate values for the three axes of the color. 
                double p = brightness * (1.0 - saturation);
                double q = brightness * (1.0 - (saturation * fractionalSector));
                double t = brightness * (1.0 - (saturation * (1 - fractionalSector)));

                // assign the fractional colors to r, g, and b based on the sector the angle is in.
                switch (sectorNumber)
                {
                    case 0:
                        r = brightness;
                        g = t;
                        b = p;
                        break;
                    case 1:
                        r = q;
                        g = brightness;
                        b = p;
                        break;
                    case 2:
                        r = p;
                        g = brightness;
                        b = t;
                        break;
                    case 3:
                        r = p;
                        g = q;
                        b = brightness;
                        break;
                    case 4:
                        r = t;
                        g = p;
                        b = brightness;
                        break;
                    case 5:
                        r = brightness;
                        g = p;
                        b = q;
                        break;
                }
            }

            r = (r * 255.0).Coerce(255.0);
            g = (g * 255.0).Coerce(255.0);
            b = (b * 255.0).Coerce(255.0);

            return new Rgba(r.ToByte(), g.ToByte(), b.ToByte());
        }

        #endregion
    }
}
