using Imagin.Core.Linq;
using System;
using System.Collections.ObjectModel;
using System.Text;

namespace Imagin.Core.Numerics;

#region (class) Matrix<Value>

[Serializable]
public class Matrix<Value> : IEquatable<Matrix<Value>> where Value : struct
{
    [Serializable]
    public enum Orientation { Horizontal, Vertical }

    #region Fields

    protected readonly Value[][] values;

    #endregion

    #region Properties

    public uint Columns 
        => (uint)values[0].Length;

    public uint Rows 
        => (uint)values.Length;

    #endregion

    #region Constructors

    public Matrix() : base() { }

    public Matrix(uint rows, uint columns)
    {
        var result = new Value[rows][];

        for (var i = 0; i < rows; i++)
            result[i] = new Value[columns];

        values = result;
    }

    public Matrix(Value[][] values)
    {
        if (values?.Length > 0)
        {
            //Each row must have identical number of columns
            var length = (int?)null;
            foreach (var i in values)
            {
                if (length == null)
                {
                    length = i.Length;
                }
                else if (i.Length != length)
                    throw new ArgumentOutOfRangeException(nameof(values));
            }
            this.values = values;
        }
        else throw new ArgumentOutOfRangeException(nameof(values), "A matrix must specify at least one value.");
    }

    public Matrix(Value[,] input) : this(input.Project()) { }

    public Matrix(Matrix<Value> input) : this(input.values.Duplicate()) { }

    #endregion

    #region Operators

    public static bool operator ==(Matrix<Value> left, Matrix<Value> right) 
        => left.EqualsOverload(right);

    public static bool operator !=(Matrix<Value> left, Matrix<Value> right) 
        => !(left == right);

    public static implicit operator Matrix<Value>(Value[][] input)
        => new Matrix<Value>(input);

    public static implicit operator Value[][] (Matrix<Value> input)
        => input.values;

    public static implicit operator Matrix<Value>(Value[,] input) 
        => new Matrix<Value>(input);

    public static implicit operator Value[,] (Matrix<Value> input) 
        => input.values.Project();

    #endregion

    #region Indexors

    public Value[] this[uint row] 
        => GetRow(row);

    public Value this[int row, int column]
    {
        get => GetValue((uint)row, (uint)column);
        set => SetValue((uint)row, (uint)column, value);
    }

    public Value this[uint row, uint column]
    {
        get => GetValue(row, column);
        set => SetValue(row, column, value);
    }

    #endregion

    #region Methods

    #region Overrides

    public override bool Equals(object input) 
        => Equals(input as Matrix<Value>);

    public override int GetHashCode() 
        => values.GetHashCode();

    public override string ToString()
        => ToString(value => value.ToString());

    #endregion

    #region Public

    /// <summary></summary>
    /// <param name="target"></param>
    /// <param name="predicate">Whether or not to check a given cell.</param>
    /// <returns></returns>
    public Vector2<uint> Contains(Matrix<Value> target, Predicate<Value> predicate = null)
    {
        for (uint row = 0; row < Rows; row++)
        {
            for (uint column = 0; column < this.Columns; column++)
            {
                if (target.Columns > this.Columns - column || target.Rows > this.Rows - row)
                    continue;

                for (uint _row = 0; _row < target.Rows; _row++)
                {
                    for (uint _column = 0; _column < target.Columns; _column++)
                    {
                        if (predicate == null || predicate(target[_row, _column]))
                        {
                            if (!this[row + _row, column + _column].Equals(target[_row, _column]))
                                goto Continue;
                        }
                    }
                }
                return new Vector2<uint>(column, row);
                Continue: continue;
            }
        }
        return default;
    }

    public void Each(Func<Value, Value> action)
        => Each((row, column, value) => action(value));

    public void Each(Func<int, int, Value, Value> action)
    {
        uint rows = Rows, columns = Columns;
        for (var row = 0; row < rows; row++)
        {
            for (var column = 0; column < columns; column++)
                values[row][column] = action(row, column, values[row][column]);
        }
    }

    public bool Equals(Matrix<Value> o)
        => this.Equals<Matrix<Value>>(o) && values == o.values;

    public void Flip(Orientation orientation)
    {
        var clone = new Matrix<Value>(this);
        for (var row = (uint)0; row < Rows; row++)
        {
            for (var column = (uint)0; column < Columns; column++)
            {
                switch (orientation)
                {
                    case Orientation.Horizontal:
                        values[row][(Columns - 1) - column] = clone[row, column];
                        break;
                    case Orientation.Vertical:
                        values[(Rows - 1) - row][column] = clone[row, column];
                        break;
                }
            }
        }
    }

    public Matrix<Value> Get(uint row, uint column)
    {
        var result = new Matrix<Value>(Rows - row, Columns - column);
        for (var r = row; r < Rows; r++)
        {
            for (var c = column; c < Columns; c++)
                result[r - row, c - column] = values[r][c];
        }
        return result;
    }

    public Value[] GetRow(uint row)
        => values[row];

    public Value GetValue(uint row, uint column)
        => values[row][column];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <param name="predicate">Whether or not to insert at a given cell.</param>
    public void Insert(Matrix<Value> input, uint row, uint column, Predicate<Value> predicate)
    {
        uint rows = Rows, columns = Columns;
        if (input.Rows > rows - row || input.Columns > columns - column)
            throw new ArgumentOutOfRangeException(nameof(input));

        for (var r = row; r < row + input.Rows; r++)
        {
            for (var c = column; c < column + input.Columns; c++)
            {
                if (predicate == null || predicate(input.values[r - row][c - column]))
                    values[r][c] = input.values[r - row][c - column];
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rotation">The number of rotations (if negative, the <see cref="Matrix{TValue}"/> is rotated counterclockwise; 
    /// otherwise, it's rotated clockwise). A single (positive) rotation is equivalent to 90° or -270°; a single (negative) rotation is 
    /// equivalent to -90° or 270°. Matrices may be rotated by 90°, 180°, or 270° only (or multiples thereof).</param>
    /// <returns></returns>
    public Matrix<Value> Rotate(int rotation)
    {
        var result = default(Matrix<Value>);

        //This normalizes the requested rotation (for instance, if 10 is specified, the rotation is actually just +-2 or +-180°, but all 
        //correspond to the same rotation).
        var d = rotation.Double() / 4d;
        d -= (int)d;

        var degree = (d - 1d) * 4d;

        //This gets the type of rotation to make; there are a total of four unique rotations possible (0°, 90°, 180°, and 270°).
        //Each correspond to 0, 1, 2, and 3, respectively (or 0, -1, -2, and -3, if in the other direction). Since
        //1 is equivalent to -3 and so forth, we combine both cases into one. 
        switch (degree)
        {
            case -3:
            case +1:
                degree = 3;
                break;
            case -2:
            case +2:
                degree = 2;
                break;
            case -1:
            case +3:
                degree = 1;
                break;
            case -4:
            case  0:
            case +4:
                degree = 0;
                break;
        }
        switch (degree)
        {
            //The rotation is 0, +-180°
            case 0:
            case 2:
                result = new Value[Rows, Columns];
                break;
            //The rotation is +-90°
            case 1:
            case 3:
                result = new Value[Columns, Rows];
                break;
        }

        for (uint i = 0; i < Columns; ++i)
        {
            for (uint j = 0; j < Rows; ++j)
            {
                switch (degree)
                {
                    //If rotation is 0°
                    case 0:
                        result.values[j][i] = values[j][i];
                        break;
                    //If rotation is -90°
                    case 1:
                        //Transpose, then reverse each column OR reverse each row, then transpose
                        result.values[i][j] = values[j][Columns - i - 1];
                        break;
                    //If rotation is +-180°
                    case 2:
                        //Reverse each column, then reverse each row
                        result.values[(Rows - 1) - j][(Columns - 1) - i] = values[j][i];
                        break;
                    //If rotation is +90°
                    case 3:
                        //Transpose, then reverse each row
                        result.values[i][j] = values[Rows - j - 1][i];
                        break;
                }
            }
        }
        return result;
    }

    public void SetValue(uint row, uint column, Value value)
        => values[row][column] = value;

    public string ToString(Func<Value, string> format)
    {
        const string spacing = " ";

        var result = new StringBuilder();
        for (uint row = 0, rows = Rows; row < rows; row++)
        {
            for (uint column = 0, columns = Columns; column < columns; column++)
            {
                var value = values[row][column];
                result.Append(format(value));
                result.Append(spacing);
            }
            if (row < rows - 1)
                result.AppendLine(string.Empty);
        }
        return result.ToString();
    }

    public Matrix<TOutput> Transform<TOutput>(Func<Value, TOutput> action) where TOutput : struct
    {
        var result = new Matrix<TOutput>(Rows, Columns);
        for (uint row = 0, rows = Rows; row < rows; row++)
        {
            for (uint column = 0, columns = Columns; column < columns; column++)
                result[row, column] = action(values[row][column]);
        }
        return result;
    }

    public Matrix<Value> Transpose()
    {
        var result = new Value[Columns, Rows];
        for (uint row = 0; row < Rows; row++)
        {
            for (uint column = 0; column < Columns; column++)
                result[column, row] = values[row][column];
        }
        return result;
    }

    #endregion

    #endregion
}

#endregion

#region (class) DoubleMatrix : Matrix<double>

[Serializable]
public class DoubleMatrix : Matrix<double>, IMatrix
{
    /// <summary>
    /// Convert all values into range of 0 and 1 based on smallest and largest value.
    /// </summary>
    [Hidden]
    public ObservableCollection<double> Normalized
    {
        get
        {
            var result = new ObservableCollection<double>();

            var n = this.Normalize();
            n.Each((x, y, i) =>
            {
                result.Add(i);
                return i;
            });

            return result;
        }
    }

    double[][] IMatrix.Values => values;

    public DoubleMatrix() : base() { }

    public DoubleMatrix(uint rows, uint columns) : base(rows, columns) { }

    public DoubleMatrix(double[][] values) : base(values) { }

    public DoubleMatrix(double[,] input) : base(input) { }

    public DoubleMatrix(Matrix<double> input) : base(input) { }
}

#endregion