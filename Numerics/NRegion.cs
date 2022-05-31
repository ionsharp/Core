using Imagin.Core.Linq;
using System;
using System.Runtime.CompilerServices;

namespace Imagin.Core.Numerics;

[Serializable]
public class NRegion<T> : Base, IChange, ICloneable, IEquatable<NRegion<T>>
{
    [field: NonSerialized]
    public event ChangedEventHandler Changed;

    //...

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

    T height = default;
    public T Height
    {
        get => height;
        set => this.Change(ref height, value);
    }

    T width = default;
    public T Width
    {
        get => width;
        set => this.Change(ref width, value);
    }

    //...

    public NRegion() : base() { }

    public NRegion(T x, T y, T width, T height) : this()
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    //...

    public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        base.OnPropertyChanged(propertyName);
        Changed?.Invoke(this);
    }

    public override string ToString() => $"{nameof(X)} = {x}, {nameof(Y)} = {y}, {nameof(Height)} = {height}, {nameof(Width)} = {width}";

    //...

    public NRegion<T> Clone() => new NRegion<T>(x, y, width, height);
    object ICloneable.Clone() => Clone();

    //...

    public static bool operator ==(NRegion<T> left, NRegion<T> right) => left.EqualsOverload(right);

    public static bool operator !=(NRegion<T> left, NRegion<T> right) => !(left == right);

    public bool Equals(NRegion<T> i) => this.Equals<NRegion<T>>(i) && x.Equals(i.x) && y.Equals(i.y) && height.Equals(i.height) && width.Equals(i.width);

    public override bool Equals(object i) => Equals(i as NRegion<T>);

    public override int GetHashCode() => new { x, y, height, width }.GetHashCode();
}

[Serializable]
public class Int32Region : NRegion<int>
{
    public Vector2<int> TopLeft => new Vector2<int>(X, Y);

    public Vector2<int> TopRight => new Vector2<int>(X + Width, Y);

    public Vector2<int> BottomLeft => new Vector2<int>(X, Y + Height);

    public Vector2<int> BottomRight => new Vector2<int>(X + Width, Y + Height);

    public Vector2<int> Center => new Vector2<int>(X + (Width / 2.0).Int32(), Y - (Height / 2.0).Int32());

    public Int32Region() : this(default, default, default, default) { }

    public Int32Region(int x, int y, int width, int height) : base(x, y, width, height) { }

    public static implicit operator Int32Region(int i) => new Int32Region(i, i, i, i);
}

[Serializable]
public class DoubleRegion : NRegion<double>
{
    public Vector2<double> TopLeft => new Vector2<double>(X, Y);

    public Vector2<double> TopRight => new Vector2<double>(X + Width, Y);

    public Vector2<double> BottomLeft => new Vector2<double>(X, Y + Height);

    public Vector2<double> BottomRight => new Vector2<double>(X + Width, Y + Height);

    public Vector2<double> Center => new Vector2<double>(X + (Width / 2.0), Y - (Height / 2.0));

    public DoubleRegion() : this(default, default, default, default) { }

    public DoubleRegion(double x, double y, double width, double height) : base(x, y, width, height) { }

    public static implicit operator DoubleRegion(double i) => new DoubleRegion(i, i, i, i);
}