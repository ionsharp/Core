using Imagin.Core.Linq;
using System;

namespace Imagin.Core.Numerics;

[Serializable]
public class Point2<T> : Base, ICloneable, IEquatable<Point2<T>>
{
    public T X { get => Get<T>(); set => Set(value); }

    public T Y { get => Get<T>(); set => Set(value); }

    public Point2() : base() { }

    public Point2(T x, T y) : this() 
    {
        X = x; Y = y;
    }

    public Point2<T> Clone() => new(X, Y);
    object ICloneable.Clone() => Clone();

    public override string ToString() => $"X => {X}, Y => {Y}";

    #region ==

    public static bool operator ==(Point2<T> left, Point2<T> right) => left.EqualsOverload(right);

    public static bool operator !=(Point2<T> left, Point2<T> right) => !(left == right);

    public override bool Equals(object i) => i is Point2<T> j && Equals(j);

    public bool Equals(Point2<T> right) => this.Equals<Point2<T>>(right) && X?.Equals(right.X) == true && Y?.Equals(right.Y) == true;

    public override int GetHashCode() => XArray.New(X, Y).GetHashCode();

    #endregion
}