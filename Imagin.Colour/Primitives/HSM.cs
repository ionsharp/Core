using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// Represents a color in <see langword="HSM"/> (Hue/Saturation/Mixture).
    /// </summary>
    /// <remarks>
    /// http://seer.ufrgs.br/rita/article/viewFile/rita_v16_n2_p141/7428
    /// </remarks>
    [Serializable]
    public struct HSM : IColor, IEquatable<HSM>
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
        public Vector Vector => new[] { _H, _S, _M };

        readonly double _H;
        /// <summary>
        /// Gets the <see langword="H"/> component [0, 1].
        /// </summary>
        public double H => _H;

        readonly double _S;
        /// <summary>
        /// Gets the <see langword="S"/> component [0, 1].
        /// </summary>
        public double S => _S;

        readonly double _M;
        /// <summary>
        /// Gets the <see langword="M"/> component [0, 1].
        /// </summary>
        public double M => _M;

        /// <summary>
        /// Initializes an instance of the <see cref="HSM"/> structure.
        /// </summary>
        /// <param name="H"></param>
        /// <param name="S"></param>
        /// <param name="M"></param>
        public HSM(double H, double S, double M)
        {
            /*
            if (!h.InRange(Minimum[0], Maximum[0]))
                throw new ArgumentOutOfRangeException(nameof(h));

            if (!s.InRange(Minimum[1], Maximum[1]))
                throw new ArgumentOutOfRangeException(nameof(s));

            if (!l.InRange(Minimum[2], Maximum[2]))
                throw new ArgumentOutOfRangeException(nameof(l));
            */

            _H = H;
            _S = S;
            _M = M;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="HSM"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public HSM(Vector input) : this(input[0], input[1], input[2]) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(HSM left, HSM right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(HSM left, HSM right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator HSM(Vector input) => new HSM(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(HSM input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(HSM o) => this.Equals<HSM>(o) && _H == o._H && _S == o._S && _M == o._M;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((HSM)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _H, _S, _M }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(H), _H, nameof(S), _S, nameof(M), _M);
    }
}