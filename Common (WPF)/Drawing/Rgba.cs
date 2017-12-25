using Imagin.Common.Linq;
using Imagin.Common.Linq;
using System;
using System.Windows.Media;

namespace Imagin.Common.Drawing
{
    /// <summary>
    /// Structure to define RGBA.
    /// </summary>
    [Serializable]
    public struct Rgba : IColor
    {
        #region Properties

        /// <summary>
        /// Specifies a <see cref="Rgba"/> component.
        /// </summary>
        public enum Component
        {
            /// <summary>
            /// Specifies the <see cref="Rgba.R"/> component.
            /// </summary>
            R,
            /// <summary>
            /// Specifies the <see cref="Rgba.G"/> component.
            /// </summary>
            G,
            /// <summary>
            /// Specifies the <see cref="Rgba.B"/> component.
            /// </summary>
            B,
            /// <summary>
            /// Specifies the <see cref="Rgba.A"/> component.
            /// </summary>
            A
        }

        /// <summary>
        /// 
        /// </summary>
        public const byte MaxValue = byte.MaxValue;

        /// <summary>
        /// 
        /// </summary>
        public const byte MinValue = byte.MinValue;

        /// <summary>
        /// 
        /// </summary>
        public Color Color
        {
            get
            {
                return Color.FromArgb(a, r, g, b);
            }
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
            private set
            {
                r = value;
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
            private set
            {
                g = value;
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
            private set
            {
                b = value;
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
            private set
            {
                a = value;
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
        /// Initializes an instance of the <see cref="Rgba"/> structure.
        /// </summary>
        /// <param name="Color"></param>
        public Rgba(Color Color) : this(Color.R, Color.G, Color.B, Color.A)
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Rgba"/> structure.
        /// </summary>
        /// <param name="R"></param>
        /// <param name="G"></param>
        /// <param name="B"></param>
        /// <param name="A"></param>
        public Rgba(int R, int G, int B, int A = 255) : this(R.Coerce(MaxValue).ToByte(), G.Coerce(MaxValue).ToByte(), B.Coerce(MaxValue).ToByte(), A.Coerce(MaxValue).ToByte())
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Rgba"/> structure.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        public Rgba(byte r, byte g, byte b, byte a = 255) : this()
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Rgba"/> structure.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        /// <param name="fromLinear"></param>
        public Rgba(double r, double g, double b, double a = 255d, bool fromLinear = false) : this()
        {
            double ma = MaxValue, mi = MinValue;

            if (fromLinear)
            {
                r *= ma;
                g *= ma;
                b *= ma;
                a *= ma;
            }

            R = r.Coerce(ma, mi).ToByte();
            G = g.Coerce(ma, mi).ToByte();
            B = b.Coerce(ma, mi).ToByte();
            A = a.Coerce(ma, mi).ToByte();
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
        /// <param name="Value"></param>
        /// <returns></returns>
        public static double Linear(byte Value)
        {
            return Linear(Value.ToInt32());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static double Linear(int Value)
        {
            return Value.ToDouble() / MaxValue.ToDouble();
        }

        #endregion
    }
}
