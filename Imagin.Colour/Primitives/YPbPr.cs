using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// 
    /// </summary>
    public struct YPbPr : IColor, IEquatable<YPbPr>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Maximum = Vector.New(1, 0.5, 0.5);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Minimum = Vector.New(0, -0.5, -0.5);

        /// <summary>
        /// 
        /// </summary>
        public Vector Vector => Vector.New(_Y, _Pb, _Pr);

        readonly double _Y;
        /// <summary>
        /// Gets the <see langword="Y"/> component [0, 1].
        /// </summary>
        public double Y => _Y;

        readonly double _Pb;
        /// <summary>
        /// Gets the <see langword="Pb"/> component [-0.5, 0.5].
        /// </summary>
        public double Pb => _Pb;

        readonly double _Pr;
        /// <summary>
        /// Gets the <see langword="Pr"/> component [-0.5, 0.5].
        /// </summary>
        public double Pr => _Pr;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Y"></param>
        /// <param name="Pb"></param>
        /// <param name="Pr"></param>
        public YPbPr(double Y, double Pb, double Pr)
        {
            _Y = Y;
            _Pb = Pb;
            _Pr = Pr;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="HSB"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public YPbPr(Vector input) : this(input[0], input[1], input[2]) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(YPbPr left, YPbPr right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(YPbPr left, YPbPr right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator YPbPr(Vector input) => new YPbPr(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(YPbPr input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(YPbPr o) => this.Equals<YPbPr>(o) && _Y == o._Y && _Pb == o._Pb && _Pr == o._Pr;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((YPbPr)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _Y, _Pb, _Pr }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(Y), _Y, nameof(Pb), _Pb, nameof(Pr), _Pr);
    }
}