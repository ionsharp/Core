using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// Represents a color in <see langword="CIE XYZ (1931)"/>.
    /// </summary>
    [Serializable]
    public struct XYZ : IColor, IEquatable<XYZ>
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
        public Vector Vector => new[] { _X, _Y, _Z };

        readonly double _X;
        /// <summary>
        /// Gets the <see langword="X"/> component [0, 1].
        /// </summary>
        public double X
        {
            get => _X;
        }

        readonly double _Y;
        /// <summary>
        /// Gets the <see langword="Y"/> component [0, 1].
        /// </summary>
        public double Y
        {
            get => _Y;
        }

        readonly double _Z;
        /// <summary>
        /// Gets the <see langword="Z"/> component [0, 1].
        /// </summary>
        public double Z
        {
            get => _Z;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="XYZ"/> structure.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public XYZ(double x, double y, double z) : this(Vector.New(x, y, z)) { }

        /// <summary>
        /// Initializes an instance of the <see cref="XYZ"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public XYZ(Vector input)
        {
            _X = input[0];
            _Y = input[1];
            _Z = input[2];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(XYZ left, XYZ right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(XYZ left, XYZ right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator XYZ(Vector input) => new XYZ(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(XYZ input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(XYZ o)
        {
            if (ReferenceEquals(o, null))
                return false;

            if (ReferenceEquals(this, o))
                return true;

            if (GetType() != o.GetType())
                return false;

            return _X == o._X && _Y == o._Y && _Z == o._Z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((XYZ)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _X, _Y, _Z }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(X), _X, nameof(Y), _Y, nameof(Z), _Z);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="whitePoint"></param>
        /// <returns></returns>
        public static double ComputeKa(XYZ whitePoint)
        {
            if (whitePoint == null)
                throw new ArgumentNullException(nameof(whitePoint));

            if (whitePoint == (XYZ)Illuminants.C)
                return 175;

            return 100 * (175 / 198.04) * (whitePoint.X + whitePoint.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="whitePoint"></param>
        /// <returns></returns>
        public static double ComputeKb(XYZ whitePoint)
        {
            if (whitePoint == null)
                throw new ArgumentNullException(nameof(whitePoint));

            if (whitePoint == (XYZ)Illuminants.C)
                return 70;

            return 100 * (70 / 218.11) * (whitePoint.Y + whitePoint.Z);
        }
    }
}
