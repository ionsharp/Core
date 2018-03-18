using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// Represents a color in <see langword="HCG"/> (Hue/Chroma/Gray).
    /// </summary>
    public struct HCG : IColor, IEquatable<HCG>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Maximum = Vector.New(360, 100, 100);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Minimum = Vector.New(0, 0, 0);

        /// <summary>
        /// 
        /// </summary>
        public Vector Vector => Vector.New(_H, _C, _G);

        readonly double _H;
        /// <summary>
        /// Gets the <see langword="H"/> component [0, 360].
        /// </summary>
        public double H => _H;

        readonly double _C;
        /// <summary>
        /// Gets the <see langword="C"/> component [0, 100].
        /// </summary>
        public double C => _C;

        readonly double _G;
        /// <summary>
        /// Gets the <see langword="G"/> component [0, 100].
        /// </summary>
        public double G => _G;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="H"></param>
        /// <param name="C"></param>
        /// <param name="G"></param>
        public HCG(double H, double C, double G)
        {
            _H = H;
            _C = C;
            _G = G;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public HCG(Vector input) : this(input[0], input[1], input[2]) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(HCG left, HCG right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(HCG left, HCG right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator HCG(Vector input) => new HCG(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(HCG input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(HCG o) => this.Equals<HCG>(o) && _H == o._H && _C == o._C && _G == o._G;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((HCG)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _H, _C, _G }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(H), _H, nameof(C), _C, nameof(G), _G);
    }
}
