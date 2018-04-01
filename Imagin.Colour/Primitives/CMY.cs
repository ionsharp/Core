using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// Represents a color in <see langword="CMY"/> (Cyan/Magenta/Yellow).
    /// </summary>
    public struct CMY : IColor, IEquatable<CMY>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Maximum = Vector.New(1, 1, 1);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Minimum = Vector.New(0, 0, 0);

        /// <summary>
        /// 
        /// </summary>
        public Vector Vector => Vector.New(_C, _M, _Y);

        readonly double _C;
        /// <summary>
        /// Gets the <see langword="Cyan"/> component [0, 1].
        /// </summary>
        public double C => _C;

        readonly double _M;
        /// <summary>
        /// Gets the <see langword="Magenta"/> component [0, 1].
        /// </summary>
        public double M => _M;

        readonly double _Y;
        /// <summary>
        /// Gets the <see langword="Yellow"/> component [0, 1].
        /// </summary>
        public double Y => _Y;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="C"></param>
        /// <param name="M"></param>
        /// <param name="Y"></param>
        public CMY(double C, double M, double Y)
        {
            _C = C;
            _M = M;
            _Y = Y;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="HSB"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public CMY(Vector input) : this(input[0], input[1], input[2]) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(CMY left, CMY right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(CMY left, CMY right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator CMY(Vector input) => new CMY(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(CMY input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(CMY o) => this.Equals<CMY>(o) && _C == o._C && _M == o._M && _Y == o._Y;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((CMY)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _C, _M, _Y }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(C), _C, nameof(M), _M, nameof(Y), _Y);
    }
}