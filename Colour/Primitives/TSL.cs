using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// Represents a color in <see langword="TSL"/> (Tint/Saturation/Lightness).
    /// </summary>
    public struct TSL : IColor, IEquatable<TSL>
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
        public Vector Vector => Vector.New(_T, _S, _L);

        readonly double _T;
        /// <summary>
        /// Gets the <see langword="T"/> component [0, 1].
        /// </summary>
        public double T => _T;

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
        /// 
        /// </summary>
        /// <param name="T"></param>
        /// <param name="S"></param>
        /// <param name="L"></param>
        public TSL(double T, double S, double L)
        {
            _T = T;
            _S = S;
            _L = L;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public TSL(Vector input) : this(input[0], input[1], input[2]) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(TSL left, TSL right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(TSL left, TSL right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator TSL(Vector input) => new TSL(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(TSL input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(TSL o) => this.Equals<TSL>(o) && _T == o._T && _S == o._S && _L == o._L;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((TSL)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _T, _S, _L }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(T), _T, nameof(S), _S, nameof(L), _L);
    }
}
