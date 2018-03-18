using Imagin.Common.Linq;
using System;
using System.Text;

namespace Imagin.Common
{
    /// <summary>
    /// Represents a quantity with both a magnitude and a direction.
    /// </summary>
    public struct Vector : IEquatable<Vector>
    {
        /// <summary>
        /// 
        /// </summary>
        public const VectorType DefaultType = VectorType.Column;

        readonly double[] _values;

        readonly VectorType _type;
        /// <summary>
        /// Gets the type of <see cref="Vector"/> (if <see cref="VectorType.Column"/>, [m, 1]; if <see cref="VectorType.Row"/>, [1, m]).
        /// </summary>
        public VectorType Type => _type;

        /// <summary>
        /// 
        /// </summary>
        public int Length => _values.Length;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double this[int index] => _values[index];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator Vector(double[] input) => new Vector(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public static implicit operator double[] (Vector input) => input._values;

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        /// Funny things might happen if this is defined implicitly.
        /// </remarks>
        /// <param name="input"></param>
        public static explicit operator Vector(Matrix input) => new Vector(input);

#pragma warning disable 1591
        public static bool operator ==(Vector left, Vector right) => left.Equals_(right);

        public static bool operator !=(Vector left, Vector right) => !(left == right);

        public static Vector operator +(Vector left, double right) => left.Add(right);

        public static Vector operator -(Vector left, double right) => left.Subtract(right);

        public static Vector operator *(Vector left, double right) => left.Multiply(right);

        public static Vector operator /(Vector left, double right) => left.Divide(right);

        public static Vector operator +(Vector left, Vector right) => left.Add(right);

        public static Vector operator -(Vector left, Vector right) => left.Subtract(right);

        public static Vector operator *(Vector left, Vector right) => left.Multiply(right);

        public static Vector operator /(Vector left, Vector right) => left.Divide(right);
#pragma warning restore 1591

        Vector(double[] values, VectorType type) 
        {
            _values = values;
            _type = type;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Vector"/> structure.
        /// </summary>
        /// <param name="values"></param>
        public Vector(params double[] values) : this(values, DefaultType) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="values"></param>
        public Vector(VectorType type, params double[] values) : this(values, type) { }

        /// <summary>
        /// Initializes an instance of the <see cref="Vector"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public Vector(Matrix input)
        {
            if (input.Columns == 1)
            {
                //Column vector
                _type = VectorType.Column;
                _values = new double[input.Rows];
            }
            else if (input.Rows == 1)
            {
                //Row vector
                _type = VectorType.Row;
                _values = new double[input.Columns];
            }
            else throw new ArgumentOutOfRangeException(nameof(input), "The matrix must have either a) one column and variable rows or b) one row and variable columns in order to become a vector.");

            var instance = this;

            var i = 0;
            input.Each(value =>
            {
                instance._values[i] = value;
                i++;
            });
        }

        /// <summary>
        /// Gets an absolute <see cref="Vector"/>.
        /// </summary>
        /// <returns></returns>
        public Vector Absolute() => Each((index, value) => value.Absolute());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Vector Add(double right) => Each((index, value) => value + right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Vector Add(Vector right) => Each((index, value) => value + right[index]);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Vector Divide(double right) => Each((index, value) => value / right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Vector Divide(Vector right) => Each((index, value) => value / right[index]);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Vector Multiply(double right) => Each((index, value) => value * right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Vector Multiply(Vector right) => Each((index, value) => value * right[index]);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Vector Subtract(double right) => Each((index, value) => value - right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Vector Subtract(Vector right) => Each((index, value) => value - right[index]);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public TOutput[] Transform<TOutput>(Func<int, double, TOutput> action)
        {
            var result = new TOutput[_values.Length];

            for (int i = 0, length = _values.Length; i < length; i++)
                result[i] = action(i, _values[i]);

            return result;
        }

        /// <summary>
        /// Coerces the range of the <see cref="Vector"/> based on the specified range.
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public Vector Coerce(double minimum, double maximum) => Each((index, value) => value.Coerce(maximum, minimum));

        /// <summary>
        /// Coerces the range of the <see cref="Vector"/> based on the specified range.
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public Vector Coerce(Vector minimum, Vector maximum)
        {
            if (minimum.Length != Length)
                throw new ArgumentOutOfRangeException(nameof(minimum));

            if (maximum.Length != Length)
                throw new ArgumentOutOfRangeException(nameof(maximum));

            return Each((index, value) => value.Coerce(maximum[index], minimum[index]));
        }

        /// <summary>
        /// Gets a new <see cref="Vector"/> based on the given transformation.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Vector Each(Func<int, double, double> action)
        {
            var result = new double[Length];

            for (int i = 0, count = Length; i < count; i++)
                result[i] = action(i, this[i]);

            return new Vector(_type, result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(Vector o) => this.Equals<Vector>(o) && _values == o._values;

        /// <summary>
        /// Gets the largest value.
        /// </summary>
        /// <returns></returns>
        public double Largest() => _values.Largest();

        /// <summary>
        /// Gets the smallest value.
        /// </summary>
        /// <returns></returns>
        public double Smallest() => _values.Smallest();

        /// <summary>
        /// Gets a rounded copy.
        /// </summary>
        /// <returns></returns>
        public Vector Round() => Each((index, value) => value.Round());

        /// <summary>
        /// Gets the sum of all values.
        /// </summary>
        /// <returns></returns>
        public double Sum()
        {
            var result = .0;
            _values.ForEach(i => result += i);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((Vector)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => _values.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var result = new StringBuilder();

            var separator = ", ";

            switch (_type)
            {
                case VectorType.Column:
                    for (int i = 0, length = _values.Length; i < length; i++)
                    {
                        var _result = _values[i].ToString();

                        if (i < length - 1)
                        {
                            result.AppendLine(_result);
                        }
                        else result.Append(_result);
                    }
                    break;
                case VectorType.Row:
                    for (int i = 0, length = _values.Length; i < length; i++)
                    {
                        result.Append(_values[i]);
                        if (i < length - 1)
                            result.Append(separator);
                    }
                    break;
            }
            return result.ToString();
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Vector"/> structure.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Vector New(params double[] values) => new Vector(values);
    }
}
