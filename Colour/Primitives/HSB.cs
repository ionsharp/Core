using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// Represents a color in <see langword="HSB"/> (Hue/Saturation/Brightness).
    /// </summary>
    [Serializable]
    public struct HSB : IColor, IEquatable<HSB>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Maximum = Vector.New(360, 100, 100);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Minimum = Vector.New(0, 0, 0);

        /// <summary>
        /// 
        /// </summary>
        public Vector Vector => new[] { _H, _S, _B };

        readonly double _H;
        /// <summary>
        /// Gets the <see langword="H"/> component [0, 360].
        /// </summary>
        public double H => _H;

        readonly double _S;
        /// <summary>
        /// Gets the <see langword="S"/> component [0, 100].
        /// </summary>
        public double S => _S;

        readonly double _B;
        /// <summary>
        /// Gets the <see langword="B"/> component [0, 100].
        /// </summary>
        public double B => _B;

        /// <summary>
        /// Creates an instance of the <see cref="HSB"/> structure.
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="b"></param>
        public HSB(double h, double s, double b)
        {
            if (!h.Within(Minimum[0], Maximum[0]))
                throw new ArgumentOutOfRangeException(nameof(h));

            if (!s.Within(Minimum[1], Maximum[1]))
                throw new ArgumentOutOfRangeException(nameof(s));

            if (!b.Within(Minimum[2], Maximum[2]))
                throw new ArgumentOutOfRangeException(nameof(b));

            _H = h;
            _S = s;
            _B = b;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="HSB"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public HSB(Vector input) : this(input[0], input[1], input[2]) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(HSB left, HSB right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(HSB left, HSB right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator HSB(Vector input) => new HSB(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(HSB input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(HSB o) => this.Equals<HSB>(o) && _H == o._H && _S == o._S && _B == o._B;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((HSB)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _H, _S, _B }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(H), _H, nameof(S), _S, nameof(B), _B);
    }
}
