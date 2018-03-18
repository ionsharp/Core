using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// 
    /// </summary>
    public struct YIQ : IColor, IEquatable<YIQ>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Maximum = Vector.New(1, 0.5957, 0.5226);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Minimum = Vector.New(0, -0.5957, -0.5226);

        /// <summary>
        /// 
        /// </summary>
        public Vector Vector => Vector.New(_Y, _I, _Q);

        readonly double _Y;
        /// <summary>
        /// Gets the <see langword="Y"/> component [0, 1].
        /// </summary>
        public double Y => _Y;

        readonly double _I;
        /// <summary>
        /// Gets the <see langword="I"/> component [-0.5957, 0.5957].
        /// </summary>
        public double I => _I;

        readonly double _Q;
        /// <summary>
        /// Gets the <see langword="Q"/> component [-0.5226, 0.5226].
        /// </summary>
        public double Q => _Q;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Y"></param>
        /// <param name="I"></param>
        /// <param name="Q"></param>
        public YIQ(double Y, double I, double Q)
        {
            _Y = Y;
            _I = I;
            _Q = Q;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="HSB"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public YIQ(Vector input) : this(input[0], input[1], input[2]) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(YIQ left, YIQ right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(YIQ left, YIQ right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator YIQ(Vector input) => new YIQ(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(YIQ input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(YIQ o) => this.Equals<YIQ>(o) && _Y == o._Y && _I == o._I && _Q == o._Q;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((YIQ)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _Y, _I, _Q }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(Y), _Y, nameof(I), _I, nameof(Q), _Q);
    }
}
