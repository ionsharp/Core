using Imagin.Core.Linq;
using System;

namespace Imagin.Core.Numerics;

#region (struct) Vector(*)<T>

/// <summary>
/// Represents a quantity with both a magnitude and a direction.
/// </summary>
[Serializable]
public struct Vector<T> : IEquatable<Vector<T>> 
{
    readonly T[] values;

    public int Length 
        => values.Length;

    public T this[int index] 
        => values[index];

    public static implicit operator Vector<T>(T[] input)
        => new Vector<T>(input);

    public static implicit operator T[] (Vector<T> input)
        => input.values;

    public static bool operator ==(Vector<T> left, Vector<T> right)
        => left.EqualsOverload(right);

    public static bool operator !=(Vector<T> left, Vector<T> right)
        => !(left == right);

    public Vector(params T[] values)
    {
        this.values = values;
    }

    public TOutput[] Transform<TOutput>(Func<int, T, TOutput> action)
    {
        var result = new TOutput[values.Length];

        for (int i = 0, length = values.Length; i < length; i++)
            result[i] = action(i, values[i]);

        return result;
    }

    public Vector<T> Each(Func<int, T, T> action)
    {
        var result = new T[Length];

        for (int i = 0, count = Length; i < count; i++)
            result[i] = action(i, this[i]);

        return new Vector<T>(result);
    }

    public bool Equals(Vector<T> o) 
        => this.Equals<Vector<T>>(o) && values == o.values;

    public override bool Equals(object o)
        => Equals((Vector<T>)o);

    public override int GetHashCode() 
        => values.GetHashCode();
}

#endregion

#region (struct) Vector(2)<T>

[Serializable]
public struct Vector2<T> : IEquatable<Vector2<T>>
{
    public const uint Length = 2;

    public readonly T X;

    public readonly T Y;

    public static implicit operator T[](Vector2<T> input)
        => Array<T>.New(input.X, input.Y);

    public static bool operator ==(Vector2<T> left, Vector2<T> right)
        => left.EqualsOverload(right);

    public static bool operator !=(Vector2<T> left, Vector2<T> right)
        => !(left == right);

    public Vector2(T xy) : this(xy, xy) { }

    public Vector2(T x, T y)
    {
        X = x;
        Y = y;
    }

    public bool Equals(Vector2<T> o)
        => this.Equals<Vector2<T>>(o) && X.Equals(o.X) && Y.Equals(o.Y);

    public override bool Equals(object o)
        => Equals((Vector2<T>)o);

    public override int GetHashCode()
        => Array<T>.New(X, Y).GetHashCode();

    public override string ToString()
        => $"x = {X}, y = {Y}";
}

#endregion

#region (struct) Vector(3)<T>

[Serializable]
public struct Vector3<T> : IEquatable<Vector3<T>>
{
    public const uint Length = 3;

    public readonly T X;

    public readonly T Y;

    public readonly T Z;

    /// <summary>(0) <see cref="R"/> = <see cref="X"/></summary>
    public T R => X;

    /// <summary>(1) <see cref="G"/> = <see cref="Y"/></summary>
    public T G => Y;

    /// <summary>(2) <see cref="B"/> = <see cref="Z"/></summary>
    public T B => Z;

    public static implicit operator T[](Vector3<T> input)
        => Array<T>.New(input.X, input.Y, input.Z);

    public static bool operator ==(Vector3<T> left, Vector3<T> right)
        => left.EqualsOverload(right);

    public static bool operator !=(Vector3<T> left, Vector3<T> right)
        => !(left == right);

    public Vector3(T xyz) : this(xyz, xyz, xyz) { }

    public Vector3(T x, T y, T z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public bool Equals(Vector3<T> o)
        => this.Equals<Vector3<T>>(o) && X.Equals(o.X) && Y.Equals(o.Y) && Z.Equals(o.Z);

    public override bool Equals(object o)
        => Equals((Vector3<T>)o);

    public override int GetHashCode()
        => Array<T>.New(X, Y, Z).GetHashCode();

    public override string ToString()
        => $"x = {X}, y = {Y}, z = {Z}";
}

#endregion

#region (struct) Vector(4)<T>

[Serializable]
public struct Vector4<T> : IEquatable<Vector4<T>>
{
    public const uint Length = 4;

    public readonly T W;

    public readonly T X;

    public readonly T Y;

    public readonly T Z;

    /// <summary>(0) <see cref="A"/> = <see cref="W"/></summary>
    public T A => W;

    /// <summary>(1) <see cref="R"/> = <see cref="X"/></summary>
    public T R => X;

    /// <summary>(2) <see cref="G"/> = <see cref="Y"/></summary>
    public T G => Y;

    /// <summary>(3) <see cref="B"/> = <see cref="Z"/></summary>
    public T B => Z;

    public static implicit operator T[](Vector4<T> input)
        => Array<T>.New(input.X, input.Y, input.Z);

    public static bool operator ==(Vector4<T> left, Vector4<T> right)
        => left.EqualsOverload(right);

    public static bool operator !=(Vector4<T> left, Vector4<T> right)
        => !(left == right);

    public Vector4(T wxyz) : this(wxyz, wxyz, wxyz, wxyz) { }

    public Vector4(T w, T x, T y, T z)
    {
        W = w;
        X = x;
        Y = y;
        Z = z;
    }

    public bool Equals(Vector4<T> o)
        => this.Equals<Vector4<T>>(o) && W.Equals(o.W) && X.Equals(o.X) && Y.Equals(o.Y) && Z.Equals(o.Z);

    public override bool Equals(object o)
        => Equals((Vector4<T>)o);

    public override int GetHashCode()
        => Array<T>.New(W, X, Y, Z).GetHashCode();

    public override string ToString()
        => $"w = {W}, x = {X}, y = {Y}, z = {Z}";
}

#endregion