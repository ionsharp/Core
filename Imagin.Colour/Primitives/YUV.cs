using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// Represents a color in <see langword="YUV"/>.
    /// </summary>
    [Serializable]
    public struct YUV : IColor, IEquatable<YUV>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Maximum = Vector.New(1, .436, .615);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Minimum = Vector.New(0, -.436, -.615);

        /// <summary>
        /// 
        /// </summary>
        public Vector Vector => new[] { _Y, _U, _V };

        readonly double _Y;
        /// <summary>
        /// Gets the <see langword="Y"/> component [0, 1].
        /// </summary>
        public double Y => _Y;

        readonly double _U;
        /// <summary>
        /// Gets the <see langword="U"/> component [-0.436, 0.436].
        /// </summary>
        public double U => _U;

        readonly double _V;
        /// <summary>
        /// Gets the <see langword="V"/> component [-0.615, 0.615].
        /// </summary>
        public double V => _V;

        /// <summary>
        /// Initializes an instance of the <see cref="YUV"/> structure.
        /// </summary>
        /// <param name="y"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        public YUV(double y, double u, double v)
        {
            /*
            if (!y.InRange(Minimum[0], Maximum[0]))
                throw new ArgumentOutOfRangeException(nameof(y));

            if (!u.InRange(Minimum[1], Maximum[1]))
                throw new ArgumentOutOfRangeException(nameof(u));

            if (!v.InRange(Minimum[2], Maximum[2]))
                throw new ArgumentOutOfRangeException(nameof(v));
            */

            _Y = y;
            _U = u;
            _V = v;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="YUV"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public YUV(Vector input) : this(input[0], input[1], input[2]) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(YUV left, YUV right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(YUV left, YUV right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator YUV(Vector input) => new YUV(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(YUV input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(YUV o) => this.Equals<YUV>(o) && _Y == o._Y && _U == o._U && _V == o._V;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((YUV)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _Y, _U, _V }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(Y), _Y, nameof(U), _U, nameof(V), _V);
    }
}
