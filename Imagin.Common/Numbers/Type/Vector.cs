using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System;
using System.Text;

namespace Imagin.Common
{
    /// <summary>
    /// Represents a quantity with both a magnitude and a direction.
    /// </summary>
    [Serializable]
    public struct Vector : IEquatable<Vector>
    {
        public static Vector Zero2 => new Vector(0.0, 0);

        public static Vector Zero3 => new Vector(0.0, 0, 0);

        public static Vector Zero4 => new Vector(0.0, 0, 0, 0);

        public const VectorType DefaultType = VectorType.Column;

        readonly double[] values;

        readonly VectorType type;
        /// <summary>
        /// Gets the type of <see cref="Vector"/> (if <see cref="VectorType.Column"/>, [m, 1]; if <see cref="VectorType.Row"/>, [1, m]).
        /// </summary>
        public VectorType Type => type;

        public int Length => values.Length;

        public double this[int index] => values[index];

        //...

        public static implicit operator Vector(double[] input) => new Vector(input);

        public static implicit operator double[] (Vector input) => input.values;

        public static explicit operator Vector(Matrix input) => new Vector(input);

        public static bool operator ==(Vector left, Vector right) => left.EqualsOverload(right);

        public static bool operator !=(Vector left, Vector right) => !(left == right);

        public static Vector operator +(Vector left, double right) => left.Add(right);

        public static Vector operator -(Vector left, double right) => left.Subtract(right);

        public static Vector operator *(Vector left, double right) => left.Multiply(right);

        public static Vector operator /(Vector left, double right) => left.Divide(right);

        public static Vector operator +(Vector left, Vector right) => left.Add(right);

        public static Vector operator -(Vector left, Vector right) => left.Subtract(right);

        public static Vector operator *(Vector left, Vector right) => left.Multiply(right);

        public static Vector operator /(Vector left, Vector right) => left.Divide(right);

        Vector(double[] values, VectorType type) 
        {
            this.values = values;
            this.type = type;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Vector"/> structure.
        /// </summary>
        /// <param name="values"></param>
        public Vector(params double[] values) : this(values, DefaultType) { }

        public Vector(Vector2<double> input) : this(Array<double>.New(input.X, input.Y), DefaultType) { }

        public Vector(Vector3<double> input) : this(Array<double>.New(input.X, input.Y, input.Z), DefaultType) { }

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
                type = VectorType.Column;
                values = new double[input.Rows];
            }
            else if (input.Rows == 1)
            {
                //Row vector
                type = VectorType.Row;
                values = new double[input.Columns];
            }
            else throw new ArgumentOutOfRangeException(nameof(input), "The matrix must have either a) one column and variable rows or b) one row and variable columns in order to become a vector.");

            var instance = this;

            var i = 0;
            input.Each(value =>
            {
                instance.values[i] = value;
                i++;
            });
        }

        //...

        /// <summary>
        /// Gets an absolute <see cref="Vector"/>.
        /// </summary>
        /// <returns></returns>
        public Vector Absolute() => Each((index, value) => value.Absolute());

        //...

        public Vector Add(double right) => Each((index, value) => value + right);

        public Vector Add(Vector right) => Each((index, value) => value + right[index]);

        public Vector Divide(double right) => Each((index, value) => value / right);

        public Vector Divide(Vector right) => Each((index, value) => value / right[index]);

        public Vector Multiply(double right) => Each((index, value) => value * right);

        public Vector Multiply(Vector right) => Each((index, value) => value * right[index]);

        public Vector Subtract(double right) => Each((index, value) => value - right);

        public Vector Subtract(Vector right) => Each((index, value) => value - right[index]);

        //...

        public TOutput[] Transform<TOutput>(Func<double, TOutput> action)
        {
            var result = new TOutput[values.Length];

            for (int i = 0, length = values.Length; i < length; i++)
                result[i] = action(values[i]);

            return result;
        }

        public TOutput[] Transform<TOutput>(Func<int, double, TOutput> action)
        {
            var result = new TOutput[values.Length];

            for (int i = 0, length = values.Length; i < length; i++)
                result[i] = action(i, values[i]);

            return result;
        }

        //...

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

        //...

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

            return new Vector(type, result);
        }

        //...

        public bool Equals(Vector o) => this.Equals<Vector>(o) && values == o.values;

        //...

        /// <summary>
        /// Gets the largest value.
        /// </summary>
        /// <returns></returns>
        public double Largest() => values.Largest();

        /// <summary>
        /// Gets the smallest value.
        /// </summary>
        /// <returns></returns>
        public double Smallest() => values.Smallest();

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
            values.ForEach(i => result += i);
            return result;
        }

        //...

        public override bool Equals(object o) => Equals((Vector)o);

        public override int GetHashCode() => values.GetHashCode();

        public override string ToString()
        {
            var result = new StringBuilder();
            var separator = ", ";

            switch (type)
            {
                case VectorType.Column:
                    goto case VectorType.Row;
                    /*
                    for (int i = 0, length = values.Length; i < length; i++)
                    {
                        var j = values[i].ToString();
                        if (i < length - 1)
                        {
                            result.AppendLine(j);
                        }
                        else result.Append(j);
                    }
                    break;
                    */

                case VectorType.Row:
                    for (int i = 0, length = values.Length; i < length; i++)
                    {
                        result.Append(values[i]);
                        if (i < length - 1)
                            result.Append(separator);
                    }
                    break;
            }
            return result.ToString();
        }

        //...

        /// <summary>
        /// Initializes an instance of the <see cref="Vector"/> structure.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Vector New(params double[] values) => new Vector(values);
    }
}