using Imagin.Core.Linq;
using System;

namespace Imagin.Core.Numerics;

[Serializable]
public class MVector2<T> : Base, IChange, ICloneable, IEquatable<MVector2<T>>
{
    [field: NonSerialized]
    public event ChangedEventHandler Changed;

    ///

    public T X { get => Get<T>(); set => Set(value); }

    public T Y { get => Get<T>(); set => Set(value); }

    ///

    public MVector2() : base() { }

    public MVector2(T x, T y) : this()
    {
        X = x;
        Y = y;
    }

    ///

    public override void OnPropertyChanged(PropertyEventArgs e)
    {
        base.OnPropertyChanged(e);
        Changed?.Invoke(this);
    }

    public override string ToString() => $"{nameof(X)} = {X}, {nameof(Y)} = {Y}";

    ///

    public MVector2<T> Clone() => new(X, Y);
    object ICloneable.Clone() => Clone();

    ///

    public static bool operator ==(MVector2<T> left, MVector2<T> right) => left.EqualsOverload(right);

    public static bool operator !=(MVector2<T> left, MVector2<T> right) => !(left == right);

    public bool Equals(MVector2<T> i) => this.Equals<MVector2<T>>(i) && X.Equals(i.X) && Y.Equals(i.Y);

    public override bool Equals(object i) => Equals(i as MVector2<T>);

    public override int GetHashCode() => new { X, Y }.GetHashCode();
}