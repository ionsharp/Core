using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// Represents a color in <see langword="RGB"/>.
    /// </summary>
    [Serializable]
    public struct RGB : IColor, IEquatable<RGB>
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
        /// Initializes an instance of the <see cref="RGB"/> structure.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="workingSpace"></param>
        public RGB(double r, double g, double b, WorkingSpace workingSpace = default(WorkingSpace))
        {
            /*
            if (!r.InRange(Minimum[0], Maximum[0]))
                throw new ArgumentOutOfRangeException(nameof(r));

            if (!g.InRange(Minimum[1], Maximum[1]))
                throw new ArgumentOutOfRangeException(nameof(g));

            if (!b.InRange(Minimum[2], Maximum[2]))
                throw new ArgumentOutOfRangeException(nameof(b));
            */

            _R = r;
            _G = g;
            _B = b;
            _workingSpace
                = workingSpace == default(WorkingSpace)
                ? WorkingSpaces.Default
                : workingSpace;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="RGB"/> structure.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="workingSpace"></param>
        public RGB(Vector input, WorkingSpace workingSpace = default(WorkingSpace)) : this(input[0], input[1], input[2], workingSpace) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(RGB left, RGB right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(RGB left, RGB right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator RGB(Vector input) => new RGB(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(RGB input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(RGB o) => this.Equals<RGB>(o) && _R == o._R && _G == o._G && _B == o._B && _workingSpace == o._workingSpace;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((RGB)o);

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