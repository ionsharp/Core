using Imagin.Core.Linq;
using System;

namespace Imagin.Core.Numerics;

[Serializable]
public class MLine<T> : Base, IChange, ICloneable, IEquatable<MLine<T>>
{
    public event ChangedEventHandler Changed;

    ///

    public virtual Vector2<T> Distance { get; }

    ///

    public T X1 { get => Get<T>(); set => Set(value); }

    public T Y1 { get => Get<T>(); set => Set(value); }

    public T X2 { get => Get<T>(); set => Set(value); }

    public T Y2 { get => Get<T>(); set => Set(value); }

    ///

    public MLine() : base() { }

    public MLine(T x1, T y1, T x2, T y2) : this()
    {
        X1 = x1;
        Y1 = y1;
        X2 = x2;
        Y2 = y2;
    }

    ///

    public override void OnPropertyChanged(PropertyEventArgs e)
    {
        base.OnPropertyChanged(e);
        Changed?.Invoke(this);
    }

    public override string ToString() => $"{nameof(X1)} = {X1}, {nameof(Y1)} = {Y1}, {nameof(X2)} = {X2}, {nameof(Y2)} = {Y2}";

    ///

    public MLine<T> Clone() => new(X1, Y1, X2, Y2);
    object ICloneable.Clone() => Clone();

    ///

    public static bool operator ==(MLine<T> left, MLine<T> right) => left.EqualsOverload(right);

    public static bool operator !=(MLine<T> left, MLine<T> right) => !(left == right);

    public bool Equals(MLine<T> i) => this.Equals<MLine<T>>(i) && X1.Equals(i.X1) && X2.Equals(i.X2) && Y1.Equals(i.Y1) && Y2.Equals(i.Y2);

    public override bool Equals(object i) => Equals(i as MLine<T>);

    public override int GetHashCode() => new { X1, Y1, X2, Y2 }.GetHashCode();
}

[Serializable]
public class DoubleLine : MLine<double>
{
    public override Vector2<double> Distance => new(X1 > X2 ? X1 - X2 : X1 < X2 ? X2 - X1 : 0, Y1 > Y2 ? Y1 - Y2 : Y1 < Y2 ? Y2 - Y1 : 0);

    public DoubleLine(double x1, double y1, double x2, double y2) : base(x1, y1, x2, y2) { }
}

[Serializable]
public class Int32Line : MLine<int>
{
    public override Vector2<int> Distance => new(X1 > X2 ? X1 - X2 : X1 < X2 ? X2 - X1 : 0, Y1 > Y2 ? Y1 - Y2 : Y1 < Y2 ? Y2 - Y1 : 0);

    public Int32Line(int x1, int y1, int x2, int y2) : base(x1, y1, x2, y2) { }
}