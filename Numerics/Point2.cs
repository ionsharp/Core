using Imagin.Core.Linq;
using System;

namespace Imagin.Core.Numerics;

[Serializable]
public class Point2 : Base, ICloneable, IEquatable<Point2>
{
    public double X { get => Get(.0); set => Set(value); }

    public double Y { get => Get(.0); set => Set(value); }

    public Point2() : base() { }

    public Point2(double x, double y) : this() 
    {
        X = x; Y = y;
    }

    public Point2 Clone() => new(X, Y);
    object ICloneable.Clone() => Clone();

    public override string ToString() => $"X => {X}, Y => {Y}";

    #region ==

    public static bool operator ==(Point2 left, Point2 right) => left.EqualsOverload(right);

    public static bool operator !=(Point2 left, Point2 right) => !(left == right);

    public override bool Equals(object i) => Equals(i as Point2);

    public bool Equals(Point2 right) => this.Equals<Point2>(right) && X == right.X && Y == right.Y;

    public override int GetHashCode() => XArray.New(X, Y).GetHashCode();

    #endregion
}