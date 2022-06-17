using Imagin.Core.Linq;
using System;

namespace Imagin.Core.Numerics;

[Serializable]
public class Point2<T> : Base, ICloneable, IEquatable<Point2<T>>
{
    T x = default;
    public T X
    {
        get => x;
        set => this.Change(ref x, value);
    }

    T y = default;
    public T Y
    {
        get => y;
        set => this.Change(ref y, value);
    }

    public Point2() : base() { }

    public Point2(T x, T y) : this() 
    {
        X = x; Y = y;
    }

    public Point2<T> Clone() => new(x, y);
    object ICloneable.Clone() => Clone();

    public override string ToString() => $"X => {x}, Y => {y}";

    #region ==

    public static bool operator ==(Point2<T> left, Point2<T> right) => left.EqualsOverload(right);

    public static bool operator !=(Point2<T> left, Point2<T> right) => !(left == right);

    public override bool Equals(object i) => i is Point2<T> j && Equals(j);

    public bool Equals(Point2<T> right) => this.Equals<Point2<T>>(right) && x?.Equals(right.x) == true && y?.Equals(right.y) == true;

    public override int GetHashCode() => XArray.New(X, Y).GetHashCode();

    #endregion
}