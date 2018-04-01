using Imagin.Common;
using Imagin.Common.Linq;
using System;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// 
    /// </summary>
    public struct YDbDr : IColor, IEquatable<YDbDr>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Maximum = Vector.New(1, 1.333, 1.333);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Vector Minimum = Vector.New(0, -1.333, -1.333);

        /// <summary>
        /// 
        /// </summary>
        public Vector Vector => Vector.New(_Y, _Db, _Dr);

        readonly double _Y;
        /// <summary>
        /// Gets the <see langword="Y"/> component [0, 1].
        /// </summary>
        public double Y => _Y;

        readonly double _Db;
        /// <summary>
        /// Gets the <see langword="Db"/> component [-1.333, 1.333].
        /// </summary>
        public double Db => _Db;

        readonly double _Dr;
        /// <summary>
        /// Gets the <see langword="Dr"/> component [-1.333, 1.333].
        /// </summary>
        public double Dr => _Dr;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Y"></param>
        /// <param name="Db"></param>
        /// <param name="Dr"></param>
        public YDbDr(double Y, double Db, double Dr)
        {
            _Y = Y;
            _Db = Db;
            _Dr = Dr;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="HSB"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public YDbDr(Vector input) : this(input[0], input[1], input[2]) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(YDbDr left, YDbDr right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(YDbDr left, YDbDr right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator YDbDr(Vector input) => new YDbDr(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(YDbDr input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(YDbDr o) => this.Equals<YDbDr>(o) && _Y == o._Y && _Db == o._Db && _Dr == o._Dr;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((YDbDr)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _Y, _Db, _Dr }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(Y), _Y, nameof(Db), _Db, nameof(Dr), _Dr);
    }
}
