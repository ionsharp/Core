using Imagin.Core.Linq;
using System;

namespace Imagin.Core.Numerics;

[Serializable]
public class MVector4<T> : Base, IChange, ICloneable, IEquatable<MVector4<T>>
{
    [field: NonSerialized]
    public event ChangedEventHandler Changed;

    ///

    public T X { get => Get<T>(); set => Set(value); }

    public T Y { get => Get<T>(); set => Set(value); }

    public T Z { get => Get<T>(); set => Set(value); }

    public T W { get => Get<T>(); set => Set(value); }

    ///

    public MVector4() : base() { }

    public MVector4(T x, T y, T z, T w) : this()
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    ///

    public override void OnPropertyChanged(PropertyEventArgs e)
    {
        base.OnPropertyChanged(e);
        Changed?.Invoke(this);
    }

    public override string ToString() => $"{nameof(X)} = {X}, {nameof(Y)} = {Y}, {nameof(Z)} = {Z}, {nameof(W)} = {W}";

    ///

    public MVector4<T> Clone() => new(X, Y, Z, W);
    object ICloneable.Clone() => Clone();

    ///

    public static bool operator ==(MVector4<T> left, MVector4<T> right) => left.EqualsOverload(right);

    public static bool operator !=(MVector4<T> left, MVector4<T> right) => !(left == right);

    public bool Equals(MVector4<T> i) => this.Equals<MVector4<T>>(i) && X.Equals(i.X) && Y.Equals(i.Y) && Z.Equals(i.Z) && W.Equals(i.W);

    public override bool Equals(object i) => Equals(i as MVector4<T>);

    public override int GetHashCode() => new { X, Y, Z, W }.GetHashCode();
}