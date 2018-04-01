using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// Represents a color in <see langword="Hunter Lab"/> (Lightness/a/b).
    /// </summary>
    [Serializable]
    public struct HunterLab : IColor, IEquatable<HunterLab>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly XYZ DefaultIlluminant = Illuminants.C;

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
        /// Gets the <see langword="L"/> component [0 to 100].
        /// </summary>
        public double L => _L;

        readonly double _a;
        /// <summary>
        /// Gets the <see langword="a"/> component [-100 to 100].
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
        /// Initializes an instance of the <see cref="HunterLab"/> structure.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="illuminant"></param>
        public HunterLab(double l, double a, double b, XYZ illuminant = default(XYZ))
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
        /// Initializes an instance of the <see cref="HunterLab"/> structure.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="illuminant"></param>
        public HunterLab(Vector input, XYZ illuminant = default(XYZ)) : this(input[0], input[1], input[2], illuminant) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(HunterLab left, HunterLab right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(HunterLab left, HunterLab right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator HunterLab(Vector input) => new HunterLab(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(HunterLab input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(HunterLab o) => this.Equals<HunterLab>(o) && _L == o._L && _a == o._a && _b == o._b;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((HunterLab)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _L, _a, _b }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(L), _L, nameof(a), _a, nameof(b), _b);
    }
}
