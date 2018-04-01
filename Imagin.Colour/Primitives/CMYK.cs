using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// Represents a color in <see langword="CMYK"/> (Cyan/Magenta/Yellow/Black).
    /// </summary>
    [Serializable]
    public struct CMYK : IColor, IEquatable<CMYK>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Maximum = Vector.New(1, 1, 1, 1);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Minimum = Vector.New(0, 0, 0, 0);

        /// <summary>
        /// 
        /// </summary>
        public Vector Vector => new[] { _C, _M, _Y, _K };

        readonly double _C;
        /// <summary>
        /// Gets the <see langword="Cyan"/> component [0, 1].
        /// </summary>
        public double C
        {
            get => _C;
        }

        readonly double _M;
        /// <summary>
        /// Gets the <see langword="Magenta"/> component [0, 1].
        /// </summary>
        public double M
        {
            get => _M;
        }

        readonly double _Y;
        /// <summary>
        /// Gets the <see langword="Yellow"/> component [0, 1].
        /// </summary>
        public double Y
        {
            get => _Y;
        }

        readonly double _K;
        /// <summary>
        /// Gets the <see langword="Black"/> component [0, 1].
        /// </summary>
        public double K
        {
            get => _K;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="m"></param>
        /// <param name="y"></param>
        /// <param name="k"></param>
        public CMYK(double c, double m, double y, double k)
        {
            if (!c.Within(Minimum[0], Maximum[0]))
                throw new ArgumentOutOfRangeException(nameof(c));

            if (!m.Within(Minimum[1], Maximum[1]))
                throw new ArgumentOutOfRangeException(nameof(m));

            if (!y.Within(Minimum[2], Maximum[2]))
                throw new ArgumentOutOfRangeException(nameof(y));

            if (!k.Within(Minimum[3], Maximum[3]))
                throw new ArgumentOutOfRangeException(nameof(k));

            _C = c;
            _M = m;
            _Y = y;
            _K = k;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public CMYK(Vector input) : this(input[0], input[1], input[2], input[3]) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(CMYK left, CMYK right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(CMYK left, CMYK right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(CMYK o) => this.Equals<CMYK>(o) && _C == o._C && _M == o._M && _Y == o._Y && _K == o._K;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((CMYK)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { C, M, Y, K }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}, {6}: {7}".F(nameof(C), _C, nameof(M), _M, nameof(Y), _Y, nameof(K), _K);
    }
}