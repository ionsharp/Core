using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// Represents a color in <see langword="HWB"/> (Hue/Whiteness/Blackness).
    /// </summary>
    public struct HWB : IColor, IEquatable<HWB>
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
        public Vector Vector => Vector.New(_H, _W, _B);

        readonly double _H;
        /// <summary>
        /// Gets the <see langword="H"/> component [0, 360].
        /// </summary>
        public double H => _H;

        readonly double _W;
        /// <summary>
        /// Gets the <see langword="W"/> component [0, 100].
        /// </summary>
        public double W => _W;

        readonly double _B;
        /// <summary>
        /// Gets the <see langword="B"/> component [0, 100].
        /// </summary>
        public double B => _B;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="H"></param>
        /// <param name="W"></param>
        /// <param name="B"></param>
        public HWB(double H, double W, double B)
        {
            _H = H;
            _W = W;
            _B = B;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public HWB(Vector input) : this(input[0], input[1], input[2]) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(HWB left, HWB right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(HWB left, HWB right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator HWB(Vector input) => new HWB(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(HWB input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(HWB o) => this.Equals<HWB>(o) && _H == o._H && _W == o._W && _B == o._B;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((HWB)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _H, _W, _B }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(H), _H, nameof(W), _W, nameof(B), _B);
    }
}
