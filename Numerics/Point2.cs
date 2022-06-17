using Imagin.Core.Linq;
using System;

namespace Imagin.Core.Numerics;

[Serializable]
public class Point2 : Base, ICloneable, IEquatable<Point2>
{
    double x = 0;
    public double X
    {
        get => x;
        set => this.Change(ref x, value);
    }

    double y = 0;
    public double Y
    {
        get => y;
        set => this.Change(ref y, value);
    }

    public Point2() : base() { }

    public Point2(double x, double y) : this() 
    {
        X = x; Y = y;
    }

    public Point2 Clone() => new(x, y);
    object ICloneable.Clone() => Clone();

    public override string ToString() => $"X => {x}, Y => {y}";

    #region ==

    public static bool operator ==(Point2 left, Point2 right) => left.EqualsOverload(right);

    public static bool operator !=(Point2 left, Point2 right) => !(left == right);

    public override bool Equals(object i) => Equals(i as Point2);

    public bool Equals(Point2 right) => this.Equals<Point2>(right) && x == right.x && y == right.y;

    public override int GetHashCode() => XArray.New(X, Y).GetHashCode();

    #endregion
}