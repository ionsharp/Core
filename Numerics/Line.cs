using Imagin.Core.Linq;
using System;

namespace Imagin.Core.Numerics;

[Serializable]
public struct Line<T> : IEquatable<Line<T>>
{
    public static Line<T> Default = new(default);

    ///

    public readonly T X1;

    public readonly T X2;

    public readonly T Y1;

    public readonly T Y2;

    ///

    public Line(T i) : this(i, i, i, i) { }

    public Line(T x1, T y1, T x2, T y2)
    {
        X1 = x1;
        Y1 = y1;
        X2 = x2;
        Y2 = y2;
    }

    ///

    public override string ToString() => $"{nameof(X1)} = {X1}, {nameof(Y1)} = {Y1}, {nameof(X2)} = {X2}, {nameof(Y2)} = {Y2}";

    ///

    public static bool operator ==(Line<T> left, Line<T> right) => left.EqualsOverload(right);

    public static bool operator !=(Line<T> left, Line<T> right)
    {
        return !(left == right);
    }

    public bool Equals(Line<T> i) => this.Equals<Line<T>>(i) && X1.Equals(i.X1) && X2.Equals(i.X2) && Y1.Equals(i.Y1) && Y2.Equals(i.Y2);

    public override bool Equals(object i) => i is Line<T> j ? Equals(j) : false;

    public override int GetHashCode() => new { X1, Y1, X2, Y2 }.GetHashCode();
}