using Imagin.Core.Linq;
using System;

namespace Imagin.Core.Numerics;

[Serializable]
public class MVector3<T> : Base, IChange, ICloneable, IEquatable<MVector3<T>>
{
    [field: NonSerialized]
    public event ChangedEventHandler Changed;

    ///

    public T X { get => Get<T>(); set => Set(value); }

    public T Y { get => Get<T>(); set => Set(value); }

    public T Z { get => Get<T>(); set => Set(value); }

    ///

    public MVector3() : base() { }

    public MVector3(T x, T y, T z) : this()
    {
        X = x;
        Y = y;
        Z = z;
    }

    ///

    public override void OnPropertyChanged(PropertyEventArgs e)
    {
        base.OnPropertyChanged(e);
        Changed?.Invoke(this);
    }

    public override string ToString() => $"{nameof(X)} = {X}, {nameof(Y)} = {Y}, {nameof(Z)} = {Z}";

    ///

    public MVector3<T> Clone() => new(X, Y, Z);
    object ICloneable.Clone() => Clone();

    ///

    public static bool operator ==(MVector3<T> left, MVector3<T> right) => left.EqualsOverload(right);

    public static bool operator !=(MVector3<T> left, MVector3<T> right) => !(left == right);

    public bool Equals(MVector3<T> i) => this.Equals<MVector3<T>>(i) && X.Equals(i.X) && Y.Equals(i.Y) && Z.Equals(i.Z);

    public override bool Equals(object i) => Equals(i as MVector3<T>);

    public override int GetHashCode() => new { X, Y, Z }.GetHashCode();
}