using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// Represents a color in <see langword="CIE xyY"/> (derived from <see langword="CIE XYZ (1931)"/>).
    /// </summary>
    [Serializable]
    public struct xyY : IColor, IEquatable<xyY>
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
        public Vector Vector => new[] { x, y, _Y };

        readonly ChromacityPoint _chromacity;
        /// <summary>
        /// Gets the chromacity.
        /// </summary>
        public ChromacityPoint Chromacity => _chromacity;

        /// <summary>
        /// Gets the <see langword="x"/> component [0, 1].
        /// </summary>
        public double x => _chromacity.X;

        /// <summary>
        /// Gets the <see langword="y"/> component [0, 1].
        /// </summary>
        public double y => _chromacity.Y;

        readonly double _Y;
        /// <summary>
        /// Gets the <see langword="Y"/> component [0, 1].
        /// </summary>
        public double Y => _Y;

        /// <summary>
        /// Initializes an instance of the <see cref="xyY"/> structure.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="Y"></param>
        public xyY(double x, double y, double Y) : this(new[] { x, y, Y }) { }

        /// <summary>
        /// Initializes an instance of the <see cref="xyY"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public xyY(Vector input) : this(new ChromacityPoint(input[0], input[1]), input[2]) { }

        /// <summary>
        /// Initializes an instance of the <see cref="xyY"/> structure.
        /// </summary>
        /// <param name="chromacity"></param>
        /// <param name="Y"></param>
        public xyY(ChromacityPoint chromacity, double Y)
        {
            _chromacity = chromacity;
            _Y = Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(xyY left, xyY right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(xyY left, xyY right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator xyY(Vector input) => new xyY(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(xyY input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(xyY o) => this.Equals<xyY>(o) && x == o.x && y == o.y && _Y == o._Y;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((xyY)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { x, y, _Y }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(x), x, nameof(y), y, nameof(Y), _Y);
    }
}
