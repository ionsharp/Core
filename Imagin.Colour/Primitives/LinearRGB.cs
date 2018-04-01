using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// Represents a color in <see langword="RGB"/> (each channel is linear and uncompanded).
    /// </summary>
    [Serializable]
    public struct LinearRGB : IColor, IEquatable<LinearRGB>
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
        public Vector Vector => new[] { _R, _G, _B };

        readonly double _R;
        /// <summary>
        /// Gets the <see langword="R"/> component [0, 1].
        /// </summary>
        public double R => _R;

        readonly double _G;
        /// <summary>
        /// Gets the <see langword="G"/> component [0, 1].
        /// </summary>
        public double G => _G;

        readonly double _B;
        /// <summary>
        /// Gets the <see langword="B"/> component [0, 1].
        /// </summary>
        public double B => _B;

        readonly WorkingSpace _workingSpace;
        /// <summary>
        /// 
        /// </summary>
        public WorkingSpace WorkingSpace => _workingSpace;

        /// <summary>
        /// Initializes an instance of the <see cref="LinearRGB"/> structure.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="workingSpace"></param>
        public LinearRGB(double r, double g, double b, WorkingSpace workingSpace = default(WorkingSpace))
        {
            if (!r.Within(Minimum[0], Maximum[0]))
                throw new ArgumentOutOfRangeException(nameof(r));

            if (!g.Within(Minimum[1], Maximum[1]))
                throw new ArgumentOutOfRangeException(nameof(g));

            if (!b.Within(Minimum[2], Maximum[2]))
                throw new ArgumentOutOfRangeException(nameof(b));

            _R = r;
            _G = g;
            _B = b;
            _workingSpace
                = workingSpace == default(WorkingSpace)
                ? WorkingSpaces.Default
                : workingSpace;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="LinearRGB"/> structure.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="workingSpace"></param>
        public LinearRGB(Vector input, WorkingSpace workingSpace = default(WorkingSpace)) : this(input[0], input[1], input[2], workingSpace) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(LinearRGB left, LinearRGB right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(LinearRGB left, LinearRGB right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator LinearRGB(Vector input) => new LinearRGB(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(LinearRGB input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(LinearRGB o) => this.Equals<LinearRGB>(o) && _R == o._R && _G == o._G && _B == o._B;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((LinearRGB)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _R, _G, _B }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(R), _R, nameof(G), _G, nameof(B), _B);
    }
}
