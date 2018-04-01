using Imagin.Common;
using Imagin.Common.Linq;

namespace Imagin.Colour.Primitives
{
    /// <summary>
    /// 
    /// </summary>
    public struct YcCbcCrc : IColor
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
        public Vector Vector => Vector.New(_Yc, _Cbc, _Crc);

        readonly double _Yc;
        /// <summary>
        /// Gets the <see langword="Yc"/> component [0, 1].
        /// </summary>
        public double Yc => _Yc;

        readonly double _Cbc;
        /// <summary>
        /// Gets the <see langword="Cbc"/> component [-0.5, 0.5].
        /// </summary>
        public double Cbc => _Cbc;

        readonly double _Crc;
        /// <summary>
        /// Gets the <see langword="Crc"/> component [-0.5, 0.5].
        /// </summary>
        public double Crc => _Crc;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Yc"></param>
        /// <param name="Cbc"></param>
        /// <param name="Crc"></param>
        public YcCbcCrc(double Yc, double Cbc, double Crc)
        {
            _Yc = Yc;
            _Cbc = Cbc;
            _Crc = Crc;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="HSB"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public YcCbcCrc(Vector input) : this(input[0], input[1], input[2]) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(YcCbcCrc left, YcCbcCrc right) => left.Equals_(right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(YcCbcCrc left, YcCbcCrc right) => !(left == right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator YcCbcCrc(Vector input) => new YcCbcCrc(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(YcCbcCrc input) => input.Vector;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(YcCbcCrc o) => this.Equals<YcCbcCrc>(o) && _Yc == o._Yc && _Cbc == o._Cbc && _Crc == o._Crc;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((YcCbcCrc)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => new { _Yc, _Cbc, _Crc }.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{0}: {1}, {2}: {3}, {4}: {5}".F(nameof(Yc), _Yc, nameof(Cbc), _Cbc, nameof(Crc), _Crc);
    }
}
