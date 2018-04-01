using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// 
    /// </summary>
    public struct YCoCg : IColor, IEquatable<YCoCg>
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
        public Vector Vector => Vector.New(_Y, _Co, _Cg);

        readonly double _Y;
        /// <summary>
        /// Gets the <see langword="Y"/> component [0, 1].
        /// </summary>
        public double Y => _Y;

        readonly double _Co;
        /// <summary>
        /// Gets the <see langword="Co"/> component [-0.5, 0.5].
        /// </summary>
        public double Co => _Co;

        readonly double _Cg;
        /// <summary>
        /// Gets the <see langword="Cg"/> component [-0.5, 0.5].
        /// </summary>
        public double Cg => _Cg;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="y"></param>
        /// <param name="co"></param>
        /// <param name="cg"></param>
        public YCoCg(double y, double co, double cg)
        {
            _Y = y;
            _Co = co;
            _Cg = cg;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="HSB"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public YCoCg(Vector input) : this(input[0], input[1], input[2]) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(YCoCg left, YCoCg right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(YCoCg left, YCoCg right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator YCoCg(Vector input) => new YCoCg(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(YCoCg input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(YCoCg o) => this.Equals<YCoCg>(o) && _Y == o._Y && _Co == o._Co && _Cg == o._Cg;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((YCoCg)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _Y, _Co, _Cg }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(Y), _Y, nameof(Co), _Co, nameof(Cg), _Cg);
    }
}
