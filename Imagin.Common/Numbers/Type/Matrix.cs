using Imagin.Common.Linq;
using System;
using System.Text;

namespace Imagin.Common
{
    /// <summary>
    /// Represents a rectangular array of numbers.
    /// </summary>
    [Serializable]
    public struct Matrix : IEquatable<Matrix>
    {
        readonly double[][] _values;

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        public int Columns => _values[0].Length;

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        public int Rows => _values.Length;

        /// <summary>
        /// Initializes an instance of the <see cref="Matrix"/> structure.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        Matrix(int rows, int columns)
        {
            var result = new double[rows][];

            for (var i = 0; i < rows; i++)
                result[i] = new double[columns];

            _values = result;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Matrix"/> structure.
        /// </summary>
        /// <param name="input"></param>
        public Matrix(Vector input)
        {
            var length = input.Length;

            var result = default(double[][]);
            switch (input.Type)
            {
                //The matrix will have dimensions [m, 1], where m = rows and 1 = columns.
                case VectorType.Column:
                    result = new double[length][];

                    for (int i = 0; i < length; i++)
                        result[i] = new double[1] { input[i] };
                    break;
                //The matrix will have dimensions [1, m], where 1 = rows and m = columns.
                case VectorType.Row:
                    result = new double[1][];
                    result[0] = new double[length];

                    for (int i = 0; i < length; i++)
                        result[0][i] = input[i];
                    break;
            }
            _values = result;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="Matrix"/> structure.
        /// </summary>
        /// <param name="values">A two-dimensional collection of values.</param>
        public Matrix(double[][] values)
        {
            if (values?.Length > 0)
            {
                //Each row must have identical number of columns
                var length = double.MinValue;
                foreach (var i in values)
                {
                    if (length == double.MinValue)
                    {
                        length = i.Length;
                    }
                    else if (i.Length != length)
                        throw new ArgumentOutOfRangeException(nameof(values));
                }
                _values = values;
            }
            else throw new ArgumentOutOfRangeException(nameof(values), "A matrix must specify at least one value.");
        }

        public double[] this[int row] => GetRow(row);

        public double this[int row, int column] => GetValue(row, column);

        public double[] GetRow(int row) => _values[row];

        public double GetValue(int row, int column) => _values[row][column];

        public static bool operator ==(Matrix left, Matrix right)
        {
            if (ReferenceEquals(left, null))
            {
                if (ReferenceEquals(right, null))
                    return true;

                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(Matrix left, Matrix right) => !(left == right);

        public static Matrix operator +(Matrix left, Matrix right) => left.Add(right);

        public static Matrix operator -(Matrix left, Matrix right) => left.Subtract(right);

        public static Matrix operator *(Matrix left, Matrix right) => left.Multiply(right);

        public static Matrix operator *(Matrix left, double right) => left.Multiply(right);

        public static Vector operator *(Matrix left, Vector right) => left.Multiply(right);

        public static implicit operator Matrix(double[][] input) => new Matrix(input);

        public static implicit operator double[][] (Matrix input) => input._values;

        public static implicit operator Matrix(double[,] input) => new Matrix(input.Project());

        public static implicit operator double[,] (Matrix input) => input._values.Project();

        public static explicit operator Matrix(Vector input) => new Matrix(input);

        public bool Equals(Matrix o)
        {
            if (ReferenceEquals(o, null))
                return false;

            if (ReferenceEquals(this, o))
                return true;

            if (GetType() != o.GetType())
                return false;

            return _values == o._values;
        }

        public override bool Equals(object o) => Equals((Matrix)o);

        public override int GetHashCode() => _values.GetHashCode();

        public override string ToString()
        {
            var result = new StringBuilder();

            var padding = Longest();
            var spacing = " ";

            for (int row = 0, rows = Rows; row < rows; row++)
            {
                for (int column = 0, columns = Columns; column < columns; column++)
                {
                    var  value  = _values[row][column];
                    var _value  = value.ToString();

                    if (value < 0)
                    {
                        result.Append("-{0}".F(_value.Remove(0).PadLeft(padding - 1, '0')));
                    }
                    else result.Append(_value.PadLeft(padding, '0'));
                    result.Append(spacing);
                }
                if (row < rows - 1)
                    result.AppendLine(string.Empty);
            }
            return result.ToString();
        }

        /// <summary>
        /// Gets an absolute <see cref="Matrix"/>.
        /// </summary>
        /// <returns></returns>
        public Matrix Absolute() => Each((row, column, value) => value.Absolute());

        /// <summary>
        /// Adds the given <see cref="Matrix"/> (throws <see cref="ArgumentOutOfRangeException"/> if the dimensions aren't identical).
        /// </summary>
        /// <param name="input"></param>
        public Matrix Add(Matrix input)
        {
            if (Columns != input.Columns || Rows != input.Rows)
                throw new ArgumentOutOfRangeException(nameof(input));

            return Each((row, column, value) => value + input._values[row][column]);
        }

        /// <summary>
        /// Enumerates each value in the <see cref="Matrix"/>.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public void Each(Action<double> action)
        {
            for (int row = 0, rows = Rows; row < rows; row++)
            {
                for (int column = 0, columns = Columns; column < columns; column++)
                    action(_values[row][column]);
            }
        }

        /// <summary>
        /// Enumerates each value in the <see cref="Matrix"/> and gets a new <see cref="Matrix"/> based on the returned values.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public Matrix Each(Func<int, int, double, double> action)
        {
            int rows = Rows, columns = Columns;
            var result = new Matrix(rows, columns);

            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                    result._values[row][column] = action(row, column, _values[row][column]);
            }

            return result;
        }

        /// <summary>
        /// Gets the largest occuring number in the <see cref="Matrix"/>.
        /// </summary>
        /// <returns></returns>
        public double Largest()
        {
            var result = double.MinValue;
            Each(value => result = value > result ? value : result);
            return result;
        }

        /// <summary>
        /// Gets the length of the number with the longest <see cref="string"/> representation.
        /// </summary>
        /// <returns></returns>
        public int Longest()
        {
            var result = int.MinValue;
            Each(value =>
            {
                var length = value.ToString().Length;
                result = length > result ? length : result;
            });
            return result;
        }

        /// <summary>
        /// Gets the smallest occurring number in the <see cref="Matrix"/>.
        /// </summary>
        /// <returns></returns>
        public double Smallest()
        {
            var result = double.MaxValue;
            Each(value => result = value < result ? value : result);
            return result;
        }

        /// <summary>
        /// Multiplies by the given <see cref="double"/>.
        /// </summary>
        /// <param name="scalar"></param>
        public Matrix Multiply(double scalar) => Each((row, column, value) => value * scalar);

        /// <summary>
        /// Multiplies by the given <see cref="Matrix"/> (throws <see cref="ArgumentOutOfRangeException"/> if <see cref="Columns"/> != <paramref langword="scalar"/>.Rows or <see cref="Rows"/> != <see langword="scalar"/>.Columns).
        /// </summary>
        /// <param name="scalar"></param>
        public Matrix Multiply(Matrix scalar)
        {
            if (Columns != scalar.Rows)
                throw new ArgumentOutOfRangeException(nameof(scalar), "The number of columns for the first matrix must equal the number of rows for the second matrix.");

            var instance = this;
            return Each((row, column, value) =>
            {
                var _value = 0.0;

                for (int x = 0, columns = instance.Columns; x < columns; x++)
                    _value += instance._values[row][x] * scalar._values[x][row];

                return _value;
            });
        }

        /// <summary>
        /// Multiplies by the given <see cref="Vector"/>.
        /// </summary>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public Vector Multiply(Vector scalar)
        {
            if (Columns != scalar.Length)
                throw new ArgumentOutOfRangeException(nameof(scalar), "The number of columns for the matrix must equal the length of the vector.");

            var rows = Rows;
            var result = new double[rows];

            for (int row = 0; row < rows; ++row)
            {
                for (int z = 0, length = scalar.Length; z < length; ++z)
                    result[row] += _values[row][z] * scalar[z];
            }

            return result;
        }

        /// <summary>
        /// Gets a rounded <see cref="Matrix"/>.
        /// </summary>
        public Matrix Round() => Each((row, column, value) => value.Round());

        /// <summary>
        /// Subtracts by the given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Matrix Subtract(Matrix input)
        {
            if (Columns != input.Columns || Rows != input.Rows)
                throw new ArgumentOutOfRangeException(nameof(input));

            return Each((row, column, value) => value - input._values[row][column]);
        }

        /// <summary>
        /// Gets the inverse of the <see cref="Matrix"/> (gets <see langword="null"/> if the inverse doesn't exist).
        /// </summary>
        /// <returns></returns>
        public Matrix? Invert()
        {
            var values = _values.Project();

            const double tiny = 0.00001;

            // Build the augmented matrix.
            int num_rows = values.GetUpperBound(0) + 1;
            double[,] augmented = new double[num_rows, 2 * num_rows];
            for (int row = 0; row < num_rows; row++)
            {
                for (int col = 0; col < num_rows; col++)
                    augmented[row, col] = values[row, col];
                augmented[row, row + num_rows] = 1;
            }

            // num_cols is the number of the augmented matrix.
            int num_cols = 2 * num_rows;

            // Solve.
            for (int row = 0; row < num_rows; row++)
            {
                // Zero out all entries in column r after this row.
                // See if this row has a non-zero entry in column r.
                if (Math.Abs(augmented[row, row]) < tiny)
                {
                    // Too close to zero. Try to swap with a later row.
                    for (int r2 = row + 1; r2 < num_rows; r2++)
                    {
                        if (Math.Abs(augmented[r2, row]) > tiny)
                        {
                            // This row will work. Swap them.
                            for (int c = 0; c < num_cols; c++)
                            {
                                double tmp = augmented[row, c];
                                augmented[row, c] = augmented[r2, c];
                                augmented[r2, c] = tmp;
                            }
                            break;
                        }
                    }
                }

                // If this row has a non-zero entry in column r, use it.
                if (Math.Abs(augmented[row, row]) > tiny)
                {
                    // Divide the row by augmented[row, row] to make this entry 1.
                    for (int col = 0; col < num_cols; col++)
                        if (col != row)
                            augmented[row, col] /= augmented[row, row];
                    augmented[row, row] = 1;

                    // Subtract this row from the other rows.
                    for (int row2 = 0; row2 < num_rows; row2++)
                    {
                        if (row2 != row)
                        {
                            double factor = augmented[row2, row] / augmented[row, row];
                            for (int col = 0; col < num_cols; col++)
                                augmented[row2, col] -= factor * augmented[row, col];
                        }
                    }
                }
            }

            // See if we have a solution.
            if (augmented[num_rows - 1, num_rows - 1] == 0)
                return null;

            // Extract the inverse array.
            double[,] inverse = new double[num_rows, num_rows];
            for (int row = 0; row < num_rows; row++)
            {
                for (int col = 0; col < num_rows; col++)
                    inverse[row, col] = augmented[row, col + num_rows];
            }

            return new Matrix(inverse.Project());
        }

        /// <summary>
        /// Gets the inverse of the <see cref="Matrix"/> (throws <see cref="InvalidOperationException"/> if the <see cref="Matrix"/> is not [3, 3]).
        /// </summary>
        /// <returns></returns>
        public Matrix Invert3By3()
        {
            if (Columns != Rows)
                throw new InvalidOperationException("Inversion is supported only on [3, 3] matrices.");

            var A =  (_values[1][1] * _values[2][2] - _values[1][2] * _values[2][1]);
            var B = -(_values[1][0] * _values[2][2] - _values[1][2] * _values[2][0]);
            var C =  (_values[1][0] * _values[2][1] - _values[1][1] * _values[2][0]);
            var D = -(_values[0][1] * _values[2][2] - _values[0][2] * _values[2][1]);
            var E =  (_values[0][0] * _values[2][2] - _values[0][2] * _values[2][0]);
            var F = -(_values[0][0] * _values[2][1] - _values[0][1] * _values[2][0]);
            var G =  (_values[0][1] * _values[1][2] - _values[0][2] * _values[1][1]);
            var H = -(_values[0][0] * _values[1][2] - _values[0][2] * _values[1][0]);
            var I =  (_values[0][0] * _values[1][1] - _values[0][1] * _values[1][0]);

            var d = _values[0][0] * A + _values[0][1] * B + _values[0][2] * C;

            return new[]
            {
                new[] { A / d, D / d, G / d },
                new[] { B / d, E / d, H / d },
                new[] { C / d, F / d, I / d },
            };
        }

        /// <summary>
        /// Gets a diagonal <see cref="Matrix"/> based on the given collection of values.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Matrix Diagonal(params double[] values)
        {
            var size = values.Length;
            var result = new double[size][];
            for (var i = 0; i < size; i++)
            {
                result[i] = new double[size];
                result[i][i] = values[i];
            }
            return result;
        }

        /// <summary>
        /// Gets an identity <see cref="Matrix"/> based on the given size.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Matrix Identity(int size)
        {
            var result = new double[size][];
            for (var i = 0; i < size; i++)
            {
                result[i] = new double[size];
                result[i][i] = 1;
            }
            return result;
        }

        /// <summary>
        /// Gets a new <see cref="Matrix"/> based on the given rows and columns.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static Matrix New(int rows, int columns) => new Matrix(rows, columns);
    }
}
