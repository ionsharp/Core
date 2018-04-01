using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// Represents a color in <see langword="LMS"/> (each component corresponds to one of the three types of cones in the human eye).
    /// </summary>
    [Serializable]
    public struct LMS : IColor, IEquatable<LMS>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Maximum = Vector.New(1, 1, 1);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Minimum = Vector.New(-1, -1, -1);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Range = Minimum.Absolute() + Maximum;

        /// <summary>
        /// 
        /// </summary>
        public Vector Vector => new[] { _L, _M, _S };

        readonly double _L;
        /// <summary>
        /// Gets the <see langword="L"/> component [-1, 1]
        /// </summary>
        public double L => _L;

        readonly double _M;
        /// <summary>
        /// Gets the <see langword="M"/> component [-1, 1].
        /// </summary>
        public double M => _M;

        readonly double _S;
        /// <summary>
        /// Gets the <see langword="S"/> component [-1, 1].
        /// </summary>
        public double S => _S;

        /// <summary>
        /// Initializes an instance of the <see cref="LMS"/> structure.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="m"></param>
        /// <param name="s"></param>
        public LMS(double l, double m, double s)
        {
            _L = l;
            _M = m;
            _S = s;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="LMS"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public LMS(Vector input) : this(input[0], input[1], input[2]) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(LMS left, LMS right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(LMS left, LMS right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator LMS(Vector input) => new LMS(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(LMS input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(LMS o) => this.Equals<LMS>(o) && _L == o._L && _M == o._M && _S == o._S;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((LMS)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _L, _M, _S }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(L), _L, nameof(M), _M, nameof(S), _S);
    }
}
