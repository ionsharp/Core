using Imagin.Core.Linq;
using System;
using System.Text;
using static Imagin.Core.Numerics.M;

namespace Imagin.Core.Numerics;

#region (struct) Vector(*)

/// <summary>
/// Represents a quantity with both a magnitude and a direction.
/// </summary>
[Serializable]
public struct Vector : IEquatable<Vector>, IVector
{
    public static Vector Zero2 => new Vector(0.0, 0);

    public static Vector Zero3 => new Vector(0.0, 0, 0);

    public static Vector Zero4 => new Vector(0.0, 0, 0, 0);

    public const VectorType DefaultType = VectorType.Column;

    readonly double[] values;
    double[] IVector.Values => values;

    readonly VectorType type;
    /// <summary>
    /// Gets the type of <see cref="Vector"/> (if <see cref="VectorType.Column"/>, [m, 1]; if <see cref="VectorType.Row"/>, [1, m]).
    /// </summary>
    public VectorType Type => type;

    public int Length
    {
        get
        {
            if (values == null)
                throw new NotImplementedException("What happened?");

            return values?.Length ?? 0;
        }
    }

    public double this[int index]
    {
        get
        {
            if (values == null)
                throw new NotImplementedException("What happened?");

            return values[index];
        }
    }

    //...

    Vector(double[] input, VectorType type) 
    {
        this.values = input ?? throw new NullReferenceException("A vector cannot be empty."); this.type = type;
    }

    /// <summary>
    /// Initializes an instance of the <see cref="Vector"/> structure.
    /// </summary>
    /// <param name="input"></param>
    public Vector(params double[] input) : this(input, DefaultType) { }

    public Vector(Vector2<double> input) : this(XArray.New(input.X, input.Y), DefaultType) { }

    public Vector(VectorType type, params double[] input) : this(input, type) { }

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

    public static implicit operator Vector(double[] input) => new Vector(input);

    public static implicit operator double[](Vector input) => input.values;

    public static explicit operator Vector(Matrix input) => new(input);

    //...

    public static Vector operator +(Vector a, double b) => a.Transform((i, j) => j + b);

    public static Vector operator -(Vector a, double b) => a.Transform((i, j) => j - b);

    public static Vector operator *(Vector a, double b) => a.Transform((i, j) => j * b);

    public static Vector operator /(Vector a, double b) => a.Transform((i, j) => j / b);

    public static Vector operator +(Vector a, Vector b) => a.Transform((i, j) => j + b[i]);

    public static Vector operator -(Vector a, Vector b) => a.Transform((i, j) => j - b[i]);

    public static Vector operator *(Vector a, Vector b) => a.Transform((i, j) => j * b[i]);

    public static Vector operator /(Vector a, Vector b) => a.Transform((i, j) => j / b[i]);

    //...

    /// <summary>Gets an absolute <see cref="Vector"/>.</summary>
    public Vector Absolute() => Transform((i, j) => j.Abs());

    /// <summary>Coerces the range of the <see cref="Vector"/> based on the specified range.</summary>
    public Vector Coerce(double minimum, double maximum) => Transform((i, j) => Clamp(j, maximum, minimum));

    /// <summary>Coerces the range of the <see cref="Vector"/> based on the specified range.</summary>
    public Vector Coerce(Vector minimum, Vector maximum)
    {
        if (minimum.Length != Length)
            throw new ArgumentOutOfRangeException(nameof(minimum));

        if (maximum.Length != Length)
            throw new ArgumentOutOfRangeException(nameof(maximum));

        return Transform((index, value) => Clamp(value, maximum[index], minimum[index]));
    }

    /// <summary>Gets a rounded copy.</summary>
    public Vector Round() => Transform((i, j) => j.Round());

    /// <summary>Gets a new <see cref="Vector"/> based on the given transformation.</summary>
    public T[] Transform<T>(Func<double, T> action)
    {
        var result = new T[values.Length];

        for (int i = 0, length = values.Length; i < length; i++)
            result[i] = action(values[i]);

        return result;
    }

    /// <summary>Gets a new <see cref="Vector"/> based on the given transformation.</summary>
    public T[] Transform<T>(Func<int, double, T> action)
    {
        var result = new T[values.Length];

        for (int i = 0, length = values.Length; i < length; i++)
            result[i] = action(i, values[i]);

        return result;
    }

    /// <summary>Gets a new <see cref="Vector"/> based on the given transformation.</summary>
    public Vector Transform(Func<int, double, double> action)
    {
        var result = new double[Length];

        for (int i = 0, count = Length; i < count; i++)
            result[i] = action(i, this[i]);

        return new Vector(type, result);
    }

    //...

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

    #region ==

    public static bool operator ==(Vector a, Vector b) => a.EqualsOverload(b);

    public static bool operator !=(Vector a, Vector b) => !(a == b);

    public bool Equals(Vector i) => this.Equals<Vector>(i) && values == i.values;

    public override bool Equals(object i) => i is Vector j && Equals(j);

    public override int GetHashCode() => values.GetHashCode();

    #endregion
}

#endregion

#region (struct) Vector(2)

[Serializable]
public struct Vector2 : IEquatable<Vector2>, IVector
{
    public const uint Length = 2;
    int IVector.Length => (int)Length;

    public static Vector2 One => new(1);

    public static Vector2 Zero => new(0);

    public double X { get; private set; }

    public double Y { get; private set; }

    double[] IVector.Values => Values;
    internal double[] Values => new double[] { X, Y };

    public double this[int index] => Values[index];

    //...

    public Vector2(double xy) : this(xy, xy) { }

    public Vector2(double x, double y)
    {
        X = x;
        Y = y;
    }

    //...

    public static implicit operator double[](Vector2 input)
        => XArray.New<double>(input.X, input.Y);

    public static implicit operator Vector(Vector2 input) => new(input);

    //...

    public static Vector2 operator +(Vector2 a, double b) => a.Transform((i, j) => j + b);

    public static Vector2 operator -(Vector2 a, double b) => a.Transform((i, j) => j - b);

    public static Vector2 operator *(Vector2 a, double b) => a.Transform((i, j) => j * b);

    public static Vector2 operator /(Vector2 a, double b) => a.Transform((i, j) => j / b);

    public static Vector2 operator +(Vector2 a, Vector2 b) => a.Transform((i, j) => j + b[i]);

    public static Vector2 operator -(Vector2 a, Vector2 b) => a.Transform((i, j) => j - b[i]);

    public static Vector2 operator *(Vector2 a, Vector2 b) => a.Transform((i, j) => j * b[i]);

    public static Vector2 operator /(Vector2 a, Vector2 b) => a.Transform((i, j) => j / b[i]);

    //...

    public override string ToString()
        => $"x = {X}, y = {Y}";

    //...

    /// <summary>Gets a new <see cref="Vector2"/> based on the given transformation.</summary>
    public Vector2 Transform(Func<int, double, double> action) => new(action(0, X), action(1, Y));
    Vector IVector.Transform(Func<int, double, double> action) => Transform(action);

    #region ==

    public static bool operator ==(Vector2 a, Vector2 b)
        => a.EqualsOverload(b);

    public static bool operator !=(Vector2 a, Vector2 b)
        => !(a == b);

    public bool Equals(Vector2 i)
        => this.Equals<Vector2>(i) && X.Equals(i.X) && Y.Equals(i.Y);

    public override bool Equals(object i)
        => i is Vector2 j && Equals(j);

    public override int GetHashCode()
        => XArray.New<double>(X, Y).GetHashCode();

    #endregion
}

#endregion

#region (struct) Vector(3)

[Serializable]
public struct Vector3 : IEquatable<Vector3>, IVector
{
    public const uint Length = 3;
    int IVector.Length => (int)Length;

    public static Vector3 One => new(1);

    public static Vector3 Zero => new(0);

    public double X { get; private set; }

    public double Y { get; private set; }

    public double Z { get; private set; }

    [Hidden]
    public Vector2 XY => new(X, Y);

    /// <summary>(0) <see cref="R"/> = <see cref="X"/></summary>
    [Hidden]
    public double R => X;

    /// <summary>(1) <see cref="G"/> = <see cref="Y"/></summary>
    [Hidden]
    public double G => Y;

    /// <summary>(2) <see cref="B"/> = <see cref="Z"/></summary>
    [Hidden]
    public double B => Z;

    double[] IVector.Values => Values;
    internal double[] Values => new double[] { X, Y, Z };

    public double this[int index] => Values[index];

    //...

    public Vector3(double xyz) : this(xyz, xyz, xyz) { }

    public Vector3(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    //...

    public static implicit operator double[](Vector3 input)
        => XArray.New<double>(input.X, input.Y, input.Z);

    public static implicit operator Vector(Vector3 input) => new(input);

    //...

    public static Vector3 operator +(Vector3 a, double b) => a.Transform((i, j) => j + b);

    public static Vector3 operator -(Vector3 a, double b) => a.Transform((i, j) => j - b);

    public static Vector3 operator *(Vector3 a, double b) => a.Transform((i, j) => j * b);

    public static Vector3 operator /(Vector3 a, double b) => a.Transform((i, j) => j / b);

    public static Vector3 operator +(Vector3 a, Vector3 b) => a.Transform((i, j) => j + b[i]);

    public static Vector3 operator -(Vector3 a, Vector3 b) => a.Transform((i, j) => j - b[i]);

    public static Vector3 operator *(Vector3 a, Vector3 b) => a.Transform((i, j) => j * b[i]);

    public static Vector3 operator /(Vector3 a, Vector3 b) => a.Transform((i, j) => j / b[i]);

    //...

    public override string ToString()
        => $"x = {X}, y = {Y}, z = {Z}";

    //...

    /// <summary>Gets a new <see cref="Vector3"/> based on the given transformation.</summary>
    public Vector3 Transform(Func<int, double, double> action) => new(action(0, X), action(1, Y), action(2, Z));
    Vector IVector.Transform(Func<int, double, double> action) => Transform(action);

    #region ==

    public static bool operator ==(Vector3 a, Vector3 b)
        => a.EqualsOverload(b);

    public static bool operator !=(Vector3 a, Vector3 b)
        => !(a == b);

    public bool Equals(Vector3 i)
        => this.Equals<Vector3>(i) && X.Equals(i.X) && Y.Equals(i.Y) && Z.Equals(i.Z);

    public override bool Equals(object i)
        => i is Vector3 j && Equals(j);

    public override int GetHashCode()
        => XArray.New<double>(X, Y, Z).GetHashCode();

    #endregion
}

#endregion

#region (struct) Vector(4)

[Serializable]
public struct Vector4 : IEquatable<Vector4>, IVector
{
    public const uint Length = 4;
    int IVector.Length => (int)Length;

    public static Vector4 One => new(1);

    public static Vector4 Zero => new(0);

    public double X { get; private set; }

    public double Y { get; private set; }

    public double Z { get; private set; }

    public double W { get; private set; }

    [Hidden]
    public Vector3 XYZ => new(X, Y, Z);

    /// <summary>(0) <see cref="R"/> = <see cref="X"/></summary>
    [Hidden]
    public double R => X;

    /// <summary>(1) <see cref="G"/> = <see cref="Y"/></summary>
    [Hidden]
    public double G => Y;

    /// <summary>(2) <see cref="B"/> = <see cref="Z"/></summary>
    [Hidden]
    public double B => Z;

    /// <summary>(3) <see cref="A"/> = <see cref="W"/></summary>
    [Hidden]
    public double A => W;

    double[] IVector.Values => Values;
    internal double[] Values => new double[] { X, Y, Z, W };

    public double this[int index] => Values[index];

    //...

    public Vector4(double xyzw) : this(xyzw, xyzw, xyzw, xyzw) { }

    public Vector4(double x, double y, double z, double w)
    {
        X = x; Y = y; Z = z; W = w;
    }

    //...

    public static implicit operator double[](Vector4 input)
        => XArray.New(input.X, input.Y, input.Z, input.W);

    public static implicit operator Vector(Vector4 input) => new(input);

    //...

    public static Vector4 operator +(Vector4 a, double b) => a.Transform((i, j) => j + b);

    public static Vector4 operator -(Vector4 a, double b) => a.Transform((i, j) => j - b);

    public static Vector4 operator *(Vector4 a, double b) => a.Transform((i, j) => j * b);

    public static Vector4 operator /(Vector4 a, double b) => a.Transform((i, j) => j / b);

    public static Vector4 operator +(Vector4 a, Vector4 b) => a.Transform((i, j) => j + b[i]);

    public static Vector4 operator -(Vector4 a, Vector4 b) => a.Transform((i, j) => j - b[i]);

    public static Vector4 operator *(Vector4 a, Vector4 b) => a.Transform((i, j) => j * b[i]);

    public static Vector4 operator /(Vector4 a, Vector4 b) => a.Transform((i, j) => j / b[i]);

    //...

    public override string ToString()
        => $"x = {X}, y = {Y}, z = {Z}, w = {W}";

    //...

    /// <summary>Gets a new <see cref="Vector4"/> based on the given transformation.</summary>
    public Vector4 Transform(Func<int, double, double> action) => new(action(0, X), action(1, Y), action(2, Z), action(3, W));
    Vector IVector.Transform(Func<int, double, double> action) => Transform(action);

    #region ==

    public static bool operator ==(Vector4 a, Vector4 b)
        => a.EqualsOverload(b);

    public static bool operator !=(Vector4 a, Vector4 b)
        => !(a == b);

    public bool Equals(Vector4 i)
        => this.Equals<Vector4>(i) && X.Equals(i.X) && Y.Equals(i.Y) && Z.Equals(i.Z) && W.Equals(i.W);

    public override bool Equals(object i)
        => i is Vector4 j && Equals(j);

    public override int GetHashCode()
        => XArray.New(X, Y, Z, W).GetHashCode();

    #endregion
}

#endregion