using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// Represents a color in <see langword="CIE L*C*h°"/> (the cylindrical form of <see langword="CIE L*u*v* (1976)"/>).
    /// </summary>
    [Serializable]
    public struct LChuv : IColor, IEquatable<LChuv>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly XYZ DefaultIlluminant = Illuminants.D65;

        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Maximum = Vector.New(100, 100, 360);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Minimum = Vector.New(0, 0, 0);

        /// <summary>
        /// 
        /// </summary>
        public Vector Vector => new[] { _L, _C, _h };

        readonly double _L;
        /// <summary>
        /// Gets the <see langword="L"/> component [0, 100].
        /// </summary>
        public double L => _L;

        readonly double _C;
        /// <summary>
        /// Gets the <see langword="C"/> component [0, 100].
        /// </summary>
        public double C => _C;

        readonly double _h;
        /// <summary>
        /// Gets the <see langword="h"/> component [0, 360].
        /// </summary>
        public double h => _h;

        readonly XYZ _illuminant;
        /// <summary>
        /// Gets the illuminant.
        /// </summary>
        public XYZ Illuminant => _illuminant;

        /// <summary>
        /// Initializes an instance of the <see cref="Lab"/> structure.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="C"></param>
        /// <param name="h"></param>
        /// <param name="illuminant"></param>
        public LChuv(double L, double C, double h, XYZ illuminant = default(XYZ))
        {
            /*
            if (!L.InRange(Minimum[0], Maximum[0]))
                throw new ArgumentOutOfRangeException(nameof(L));

            if (!C.InRange(Minimum[1], Maximum[1]))
                throw new ArgumentOutOfRangeException(nameof(C));

            if (!h.InRange(Minimum[2], Maximum[2]))
                throw new ArgumentOutOfRangeException(nameof(h));
            */

            _L = L;
            _C = C;
            _h = h;
            _illuminant = illuminant == default(XYZ) ? DefaultIlluminant : illuminant;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Lab"/> structure.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="illuminant"></param>
        public LChuv(Vector input, XYZ illuminant = default(XYZ)) : this(input[0], input[1], input[2], illuminant) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(LChuv left, LChuv right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(LChuv left, LChuv right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator LChuv(Vector input) => new LChuv(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(LChuv input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(LChuv o) => this.Equals<LChuv>(o) && _L == o._L && _C == o._C && _h == o._h;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((LChuv)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _L, _C, _h }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(L), _L, nameof(C), _C, nameof(h), _h);

        /// <summary>
        /// Gets saturation of the color (chroma normalized by lightness).
        /// </summary>
        public static double GetSaturation(double L, double C)
        {
            var result = 100 * (C / L);

            if (double.IsNaN(result))
                return 0;

            return result;
        }

        /// <summary>
        /// Gets chroma from saturation and lightness.
        /// </summary>
        public static double GetChroma(double saturation, double L) => L * (saturation / 100);

        /// <summary>
        /// Computes saturation of the color (chroma normalized by lightness).
        /// </summary>
        /// <remarks>
        /// Ranges from 0 to 100.
        /// </remarks>
        public double Saturation => GetSaturation(_L, _C);

        /// <summary>
        /// Constructs the color using saturation instead of chromas.
        /// </summary>
        public static LChuv FromSaturation(double lightness, double hue, double saturation)
        {
            var chroma = GetChroma(saturation, lightness);
            return new LChuv(lightness, chroma, hue);
        }
    }
}
