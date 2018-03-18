using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// 
    /// </summary>
    public struct YES : IColor, IEquatable<YES>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Maximum = Vector.New(1, 1, 1);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Minimum = Vector.New(0, 0, 0);

        /// <summary>
        /// 
        /// </summary>
        public Vector Vector => Vector.New(_Y, _E, _S);

        readonly double _Y;
        /// <summary>
        /// Gets the <see langword="Y"/> component [0, 1].
        /// </summary>
        public double Y => _Y;

        readonly double _E;
        /// <summary>
        /// Gets the <see langword="E"/> component [0, 1].
        /// </summary>
        public double E => _E;

        readonly double _S;
        /// <summary>
        /// Gets the <see langword="S"/> component [0, 1].
        /// </summary>
        public double S => _S;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Y"></param>
        /// <param name="E"></param>
        /// <param name="S"></param>
        public YES(double Y, double E, double S)
        {
            _Y = Y;
            _E = E;
            _S = S;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="HSB"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public YES(Vector input) : this(input[0], input[1], input[2]) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(YES left, YES right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(YES left, YES right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator YES(Vector input) => new YES(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(YES input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(YES o) => this.Equals<YES>(o) && _Y == o._Y && _E == o._E && _S == o._S;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((YES)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _Y, _E, _S }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(Y), _Y, nameof(E), _E, nameof(S), _S);
    }
}
