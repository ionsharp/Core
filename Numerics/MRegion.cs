using Imagin.Core.Linq;
using System;

namespace Imagin.Core.Numerics;

[Serializable]
public class MRegion<T> : Base, IChange, ICloneable, IEquatable<MRegion<T>>
{
    [field: NonSerialized]
    public event ChangedEventHandler Changed;

    ///

    public T X { get => Get<T>(); set => Set(value); }

    public T Y { get => Get<T>(); set => Set(value); }

    public T Height { get => Get<T>(); set => Set(value); }

    public T Width { get => Get<T>(); set => Set(value); }

    ///

    public MRegion() : base() { }

    public MRegion(T x, T y, T width, T height) : this()
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    ///

    public override void OnPropertyChanged(PropertyEventArgs e)
    {
        base.OnPropertyChanged(e);
        Changed?.Invoke(this);
    }

    public override string ToString() => $"{nameof(X)} = {X}, {nameof(Y)} = {Y}, {nameof(Height)} = {Height}, {nameof(Width)} = {Width}";

    ///

    public MRegion<T> Clone() => new(X, Y, Width, Height);
    object ICloneable.Clone() => Clone();

    ///

    public static bool operator ==(MRegion<T> left, MRegion<T> right) => left.EqualsOverload(right);

    public static bool operator !=(MRegion<T> left, MRegion<T> right) => !(left == right);

    public bool Equals(MRegion<T> i) => this.Equals<MRegion<T>>(i) && X.Equals(i.X) && Y.Equals(i.Y) && Height.Equals(i.Height) && Width.Equals(i.Width);

    public override bool Equals(object i) => Equals(i as MRegion<T>);

    public override int GetHashCode() => new { X, Y, Height, Width }.GetHashCode();
}

[Serializable]
public class Int32Region : MRegion<int>
{
    public Vector2<int> TopLeft => new(X, Y);

    public Vector2<int> TopRight => new(X + Width, Y);

    public Vector2<int> BottomLeft => new(X, Y + Height);

    public Vector2<int> BottomRight => new(X + Width, Y + Height);

    public Vector2<int> Center => new(X + (Width / 2.0).Int32(), Y - (Height / 2.0).Int32());

    public Int32Region() : this(default, default, default, default) { }

    public Int32Region(int x, int y, int width, int height) : base(x, y, width, height) { }

    public static implicit operator Int32Region(int i) => new(i, i, i, i);
}

[Serializable]
public class DoubleRegion : MRegion<double>
{
    public Vector2<double> TopLeft => new(X, Y);

    public Vector2<double> TopRight => new(X + Width, Y);

    public Vector2<double> BottomLeft => new(X, Y + Height);

    public Vector2<double> BottomRight => new(X + Width, Y + Height);

    public Vector2<double> Center => new(X + (Width / 2.0), Y - (Height / 2.0));

    public DoubleRegion() : this(default, default, default, default) { }

    public DoubleRegion(double x, double y, double width, double height) : base(x, y, width, height) { }

    public static implicit operator DoubleRegion(double i) => new(i, i, i, i);
}