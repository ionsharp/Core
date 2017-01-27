using Imagin.Common.Extensions;
using System;
using System.Windows.Media;

namespace Imagin.Controls.Extended.Primitives
{
    /// <summary>
    /// Structure to define RGBA.
    /// </summary>
    [Serializable]
    public struct Rgba
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public static byte MaxValue = 255;

        /// <summary>
        /// 
        /// </summary>
        public static byte MinValue = 0;

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
                r = value.Coerce(MaxValue, MinValue);
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
                g = value.Coerce(MaxValue, MinValue);
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
                b = value.Coerce(MaxValue, MinValue);
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
                a = value.Coerce(MaxValue, MinValue);
            }
        }

        #endregion

        #region Rgba

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Rgba a, Rgba b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Rgba a, Rgba b)
        {
            return a.R != b.R || a.G != b.G || a.B != b.B || a.A != b.A;
        }

        /// <summary>
        /// Creates an instance of the <see cref="Rgba"/> structure.
        /// </summary>
        public Rgba(int R, int G, int B, int A = 255)
        {
            r = R.Coerce(MaxValue, MinValue).ToByte();
            g = G.Coerce(MaxValue, MinValue).ToByte();
            b = B.Coerce(MaxValue, MinValue).ToByte();
            a = A.Coerce(MaxValue, MinValue).ToByte();
        }

        /// <summary>
        /// Creates an instance of the <see cref="Rgba"/> structure.
        /// </summary>
        public Rgba(byte R, byte G, byte B, byte A = 255)
        {
            r = R.Coerce(MaxValue, MinValue);
            g = G.Coerce(MaxValue, MinValue);
            b = B.Coerce(MaxValue, MinValue);
            a = A.Coerce(MaxValue, MinValue);
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
            return Object == null || GetType() != Object.GetType() ? false : this == (Rgba)Object;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return r.GetHashCode() ^ g.GetHashCode() ^ b.GetHashCode() ^ a.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "R => {0}, G => {1}, B => {2}, A => {3}".F(r, g, b, a);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="First"></param>
        /// <param name="Second"></param>
        /// <returns></returns>
        public double Distance(Color First, Color Second)
        {
            return Math.Sqrt(Math.Pow(First.R - Second.R, 2) + Math.Pow(First.G - Second.G, 2) + Math.Pow(First.B - Second.B, 2));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Color ToColor()
        {
            return Color.FromArgb(a, r, g, b);
        }

        #endregion
    }
}
