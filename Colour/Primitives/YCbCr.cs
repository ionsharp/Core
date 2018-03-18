using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// 
    /// </summary>
    public struct YCbCr : IColor, IEquatable<YCbCr>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Maximum = Vector.New(235, 240, 240);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Minimum = Vector.New(16, 16, 16);

        /// <summary>
        /// 
        /// </summary>
        public Vector Vector => Vector.New(_Y, _Cb, _Cr);

        readonly double _Y;
        /// <summary>
        /// Gets the <see langword="Y"/> component [16, 235].
        /// </summary>
        public double Y => _Y;

        readonly double _Cb;
        /// <summary>
        /// Gets the <see langword="Cb"/> component [16, 240].
        /// </summary>
        public double Cb => _Cb;

        readonly double _Cr;
        /// <summary>
        /// Gets the <see langword="Cr"/> component [16, 240].
        /// </summary>
        public double Cr => _Cr;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Y"></param>
        /// <param name="Cb"></param>
        /// <param name="Cr"></param>
        public YCbCr(double Y, double Cb, double Cr)
        {
            _Y = Y;
            _Cb = Cb;
            _Cr = Cr;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="HSB"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public YCbCr(Vector input) : this(input[0], input[1], input[2]) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(YCbCr left, YCbCr right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(YCbCr left, YCbCr right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator YCbCr(Vector input) => new YCbCr(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(YCbCr input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(YCbCr o) => this.Equals<YCbCr>(o) && _Y == o._Y && _Cb == o._Cb && _Cr == o._Cr;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((YCbCr)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _Y, _Cb, _Cr }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(Y), _Y, nameof(Cb), _Cb, nameof(Cr), _Cr);
    }
}
