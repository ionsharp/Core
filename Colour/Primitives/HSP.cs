using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// Represents a color in <see langword="HSP"/> (Hue/Saturation/Perceived-Brightness).
    ///
    /// </summary>
    public struct HSP : IColor, IEquatable<HSP>
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
        public Vector Vector => Vector.New(_H, _S, _P);

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

        readonly double _P;
        /// <summary>
        /// Gets the <see langword="P"/> component [0, 255].
        /// </summary>
        public double P => _P;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="H"></param>
        /// <param name="S"></param>
        /// <param name="P"></param>
        public HSP(double H, double S, double P)
        {
            _H = H;
            _S = S;
            _P = P;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="HSP"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public HSP(Vector input) : this(input[0], input[1], input[2]) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(HSP left, HSP right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(HSP left, HSP right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator HSP(Vector input) => new HSP(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(HSP input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(HSP o) => this.Equals<HSP>(o) && _H == o._H && _S == o._S && _P == o._P;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((HSP)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _H, _S, _P }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(H), _H, nameof(S), _S, nameof(P), _P);
    }
}
