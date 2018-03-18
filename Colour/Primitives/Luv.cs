using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// Represents a color in <see langword="CIE L*u*v* (1976)"/>.
    /// </summary>
    [Serializable]
    public struct Luv : IColor, IEquatable<Luv>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly XYZ DefaultIlluminant = Illuminants.D65;

        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Maximum = Vector.New(100, 100, 100);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Minimum = Vector.New(0, -100, -100);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Range = Minimum.Absolute() + Maximum;

        /// <summary>
        /// 
        /// </summary>
        public Vector Vector => new[] { _L, _u, _v };

        readonly double _L;
        /// <summary>
        /// Gets the <see langword="L"/> component [0, 100]
        /// </summary>
        public double L => _L;

        readonly double _u;
        /// <summary>
        /// Gets the <see langword="u"/> component [-100, 100].
        /// </summary>
        public double u => _u;

        readonly double _v;
        /// <summary>
        /// Gets the <see langword="v"/> component [-100, 100].
        /// </summary>
        public double v => _v;

        readonly XYZ _illuminant;
        /// <summary>
        /// Gets the illuminant.
        /// </summary>
        public XYZ Illuminant => _illuminant;

        /// <summary>
        /// Initializes an instance of the <see cref="Luv"/> structure.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="illuminant"></param>
        public Luv(double l, double u, double v, XYZ illuminant = default(XYZ))
        {
            /*
            if (!l.InRange(Minimum[0], Maximum[0]))
                throw new ArgumentOutOfRangeException(nameof(l));

            if (!u.InRange(Minimum[1], Maximum[1]))
                throw new ArgumentOutOfRangeException(nameof(u));

            if (!v.InRange(Minimum[2], Maximum[2]))
                throw new ArgumentOutOfRangeException(nameof(v));
            */

            _L = l;
            _u = u;
            _v = v;
            _illuminant = illuminant == default(XYZ) ? DefaultIlluminant : illuminant;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Luv"/> structure.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="illuminant"></param>
        public Luv(Vector input, XYZ illuminant = default(XYZ)) : this(input[0], input[1], input[2], illuminant) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Luv left, Luv right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Luv left, Luv right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Luv(Vector input) => new Luv(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(Luv input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(Luv o) => this.Equals<Luv>(o) && L == o.L && u == o.u && v == o.v;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((Luv)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { L, u, v }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(L), _L, nameof(u), _u, nameof(v), _v);
    }
}
