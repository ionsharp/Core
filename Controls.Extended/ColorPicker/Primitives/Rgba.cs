using Imagin.Common.Extensions;
using System;
using System.Windows.Media;

namespace Imagin.Controls.Extended.Primitives
{
    [Serializable]
    /// <summary>
    /// Structure to define RGBA.
    /// </summary>
    public struct Rgba
    {
        #region Properties

        public struct MaxValue
        {
            public static byte R = 255;

            public static byte G = 255;

            public static byte B = 255;

            public static byte A = 255;
        }

        public struct MinValue
        {
            public static byte R = 0;

            public static byte G = 0;

            public static byte B = 0;

            public static byte A = 0;
        }

        byte r;
        /// <summary>
        /// Gets or sets the red component (0 to 255).
        /// </summary>
        public byte R
        {
            get
            {
                return r;
            }
            set
            {
                r = value.Coerce(Rgba.MaxValue.R, Rgba.MinValue.R);
            }
        }

        byte g;
        /// <summary>
        /// Gets or sets the green component (0 to 255).
        /// </summary>
        public byte G
        {
            get
            {
                return g;
            }
            set
            {
                g = value.Coerce(Rgba.MaxValue.G, Rgba.MinValue.G);
            }
        }

        byte b;
        /// <summary>
        /// Gets or sets the blue component (0 to 255).
        /// </summary>
        public byte B
        {
            get
            {
                return b;
            }
            set
            {
                b = value.Coerce(Rgba.MaxValue.B, Rgba.MinValue.B);
            }
        }

        byte a;
        /// <summary>
        /// Gets or sets the alpha component (0 to 255).
        /// </summary>
        public byte A
        {
            get
            {
                return a;
            }
            set
            {
                a = value.Coerce(Rgba.MaxValue.A, Rgba.MinValue.A);
            }
        }

        #endregion

        #region Rgba

        public static bool operator ==(Rgba a, Rgba b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;
        }

        public static bool operator !=(Rgba a, Rgba b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A;
        }

        /// <summary>
        /// Creates an instance of a Rgba structure.
        /// </summary>
        public Rgba(int R, int G, int B, int A = 255)
        {
            this.r = R.Coerce(Rgba.MaxValue.R, Rgba.MinValue.R).ToByte();
            this.g = G.Coerce(Rgba.MaxValue.G, Rgba.MinValue.G).ToByte();
            this.b = B.Coerce(Rgba.MaxValue.B, Rgba.MinValue.B).ToByte();
            this.a = A.Coerce(Rgba.MaxValue.A, Rgba.MinValue.A).ToByte();
        }

        /// <summary>
        /// Creates an instance of a Rgba structure.
        /// </summary>
        public Rgba(byte R, byte G, byte B, byte A = 255)
        {
            this.r = R.Coerce(Rgba.MaxValue.R, Rgba.MinValue.R);
            this.g = G.Coerce(Rgba.MaxValue.G, Rgba.MinValue.G);
            this.b = B.Coerce(Rgba.MaxValue.B, Rgba.MinValue.B);
            this.a = A.Coerce(Rgba.MaxValue.A, Rgba.MinValue.A);
        }

        #endregion

        #region Methods

        public override bool Equals(Object Object)
        {
            if (Object == null || GetType() != Object.GetType()) return false;

            return (this == (Rgba)Object);
        }

        public override int GetHashCode()
        {
            return R.GetHashCode() ^ G.GetHashCode() ^ B.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("R => {0}, G => {1}, B => {2}", this.R.ToString(), this.G.ToString(), this.B.ToString());
        }

        public double Distance(Color First, Color Second)
        {
            return Math.Sqrt(Math.Pow(First.R - Second.R, 2) + Math.Pow(First.G - Second.G, 2) + Math.Pow(First.B - Second.B, 2));
        }

        public Color ToColor()
        {
            return Color.FromArgb(this.A, this.R, this.G, this.B);
        }

        #endregion
    }
}
