using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// Represents a color in <see langword="RG Chromacity"/> (Red/Green).
    /// </summary>
    /// <remarks>
    /// https://en.wikipedia.org/wiki/Rg_chromaticity
    /// </remarks>
    public struct RG : IColor, IEquatable<RG>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Maximum = Vector.New(1, 1);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Minimum = Vector.New(0, 0);

        /// <summary>
        /// 
        /// </summary>
        public Vector Vector => Vector.New(_R, _G, _g);

        readonly double _R;
        /// <summary>
        /// Gets the <see langword="Red"/> component [0, 1].
        /// </summary>
        public double R => _R;

        readonly double _G;
        /// <summary>
        /// Gets the <see langword="Green"/> component [0, 1].
        /// </summary>
        public double G => _G;

        readonly double _g;
        /// <summary>
        /// Gets the <see langword="Green"/> (normalized) component [0, 1].
        /// </summary>
        public double g => _g;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="R"></param>
        /// <param name="G"></param>
        /// <param name="g"></param>
        public RG(double R, double G, double g)
        {
            _R = R;
            _G = G;
            _g = g;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="HSB"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public RG(Vector input) : this(input[0], input[1], input[2]) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(RG left, RG right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RG left, RG right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator RG(Vector input) => new RG(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(RG input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(RG o) => this.Equals<RG>(o) && _R == o._R && _G == o._G && _g == o._g;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((RG)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _R, _G, _g }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(R), _R, nameof(G), _G, nameof(g), _g);
    }
}