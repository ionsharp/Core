using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// Represents a color in <see langword="HSL"/> (Hue/Saturation/Lightness).
    /// </summary>
    [Serializable]
    public struct HSL : IColor, IEquatable<HSL>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Maximum = Vector.New(360, 1, 1);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Minimum = Vector.New(0, 0, 0);

        /// <summary>
        /// 
        /// </summary>
        public Vector Vector => new[] { _H, _S, _L };

        readonly double _H;
        /// <summary>
        /// Gets the <see langword="H"/> component [0, 360].
        /// </summary>
        public double H => _H;

        readonly double _S;
        /// <summary>
        /// Gets the <see langword="S"/> component [0, 1].
        /// </summary>
        public double S => _S;

        readonly double _L;
        /// <summary>
        /// Gets the <see langword="L"/> component [0, 1].
        /// </summary>
        public double L => _L;

        /// <summary>
        /// Initializes an instance of the <see cref="HSL"/> structure.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="l"></param>
        public HSL(double h, double s, double l)
        {
            /*
            if (!h.InRange(Minimum[0], Maximum[0]))
                throw new ArgumentOutOfRangeException(nameof(h));

            if (!s.InRange(Minimum[1], Maximum[1]))
                throw new ArgumentOutOfRangeException(nameof(s));

            if (!l.InRange(Minimum[2], Maximum[2]))
                throw new ArgumentOutOfRangeException(nameof(l));
            */

            _H = h;
            _S = s;
            _L = l;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="HSL"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public HSL(Vector input) : this(input[0], input[1], input[2]) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(HSL left, HSL right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(HSL left, HSL right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator HSL(Vector input) => new HSL(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(HSL input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(HSL o) => this.Equals<HSL>(o) && _H == o._H && _S == o._S && _L == o._L;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((HSL)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _H, _S, _L }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(H), _H, nameof(S), _S, nameof(L), _L);
    }
}
