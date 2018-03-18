using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// Represents a color in <see langword="HSI"/> (Hue/Saturation/Intensity).
    /// </summary>
    public struct HSI : IColor, IEquatable<HSI>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Maximum = Vector.New(360, 100, 255);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Minimum = Vector.New(0, 0, 0);

        /// <summary>
        /// 
        /// </summary>
        public Vector Vector => Vector.New(_H, _S, _I);

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

        readonly double _I;
        /// <summary>
        /// Gets the <see langword="I"/> component [0, 255].
        /// </summary>
        public double I => _I;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="H"></param>
        /// <param name="S"></param>
        /// <param name="I"></param>
        public HSI(double H, double S, double I)
        {
            _H = H;
            _S = S;
            _I = I;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public HSI(Vector input) : this(input[0], input[1], input[2]) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(HSI left, HSI right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(HSI left, HSI right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator HSI(Vector input) => new HSI(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(HSI input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(HSI o) => this.Equals<HSI>(o) && _H == o._H && _S == o._S && _I == o._I;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((HSI)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _H, _S, _I }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(H), _H, nameof(S), _S, nameof(I), _I);
    }
}
