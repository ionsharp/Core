using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// Represents a color in <see langword="CIE L*a*b* (1976)"/>.
    /// </summary>
    [Serializable]
    public struct Lab : IColor, IEquatable<Lab>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly XYZ DefaultIlluminant = Illuminants.D50;

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
        public Vector Vector => new[] { _L, _a, _b };

        readonly double _L;
        /// <summary>
        /// Gets the <see langword="L"/> component [0, 100].
        /// </summary>
        public double L => _L;

        readonly double _a;
        /// <summary>
        /// Gets the <see langword="a"/> component [-100, 100].
        /// </summary>
        public double a => _a;

        readonly double _b;
        /// <summary>
        /// Gets the <see langword="b"/> component [-100, 100].
        /// </summary>
        public double b => _b;

        readonly XYZ _illuminant;
        /// <summary>
        /// Gets the illuminant.
        /// </summary>
        public XYZ Illuminant => _illuminant;

        /// <summary>
        /// Initializes an instance of the <see cref="Lab"/> structure.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="illuminant"></param>
        public Lab(double l, double a, double b, XYZ illuminant = default(XYZ))
        {
            /*
            if (!l.InRange(Minimum[0], Maximum[0]))
                throw new ArgumentOutOfRangeException(nameof(l));

            if (!a.InRange(Minimum[1], Maximum[1]))
                throw new ArgumentOutOfRangeException(nameof(a));

            if (!b.InRange(Minimum[2], Maximum[2]))
                throw new ArgumentOutOfRangeException(nameof(b));
            */

            _L = l;
            _a = a;
            _b = b;
            _illuminant = illuminant == default(XYZ) ? DefaultIlluminant : illuminant;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Lab"/> structure.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="illuminant"></param>
        public Lab(Vector input, XYZ illuminant = default(XYZ)) : this(input[0], input[1], input[2], illuminant) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Lab left, Lab right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Lab left, Lab right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Lab(Vector input) => new Lab(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(Lab input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(Lab o) => this.Equals<Lab>(o) && _L == o._L && _a == o._a && _b == o._b;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((Lab)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { L, a, b }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(L), _L, nameof(a), _a, nameof(b), _b);
    }
}
