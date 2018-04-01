using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// 
    /// </summary>
    public struct xvYCC : IColor, IEquatable<xvYCC>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Maximum = Vector.New(255, 255, 255);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Minimum = Vector.New(0, 0, 0);

        /// <summary>
        /// 
        /// </summary>
        public Vector Vector => Vector.New(_Y, _Cb, _Cr);

        readonly double _Y;
        /// <summary>
        /// Gets the <see langword="Y"/> component [0, 255].
        /// </summary>
        public double Y => _Y;

        readonly double _Cb;
        /// <summary>
        /// Gets the <see langword="Cb"/> component [0, 255].
        /// </summary>
        public double Cb => _Cb;

        readonly double _Cr;
        /// <summary>
        /// Gets the <see langword="Cr"/> component [0, 255].
        /// </summary>
        public double Cr => _Cr;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Y"></param>
        /// <param name="Cb"></param>
        /// <param name="Cr"></param>
        public xvYCC(double Y, double Cb, double Cr)
        {
            _Y = Y;
            _Cb = Cb;
            _Cr = Cr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public xvYCC(Vector input) : this(input[0], input[1], input[2]) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(xvYCC left, xvYCC right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(xvYCC left, xvYCC right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator xvYCC(Vector input) => new xvYCC(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(xvYCC input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(xvYCC o) => this.Equals<xvYCC>(o) && _Y == o._Y && _Cb == o._Cb && _Cr == o._Cr;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((xvYCC)o);

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
