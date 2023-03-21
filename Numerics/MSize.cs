using Imagin.Core.Linq;
using System;

namespace Imagin.Core.Numerics;

[Serializable]
public class MSize<T> : Base, IChange, ICloneable, IEquatable<MSize<T>>
{
    public event ChangedEventHandler Changed;

    ///

    public T Height { get => Get<T>(); set => Set(value); }

    public T Width { get => Get<T>(); set => Set(value); }

    ///

    public MSize() : this(default) { }

    public MSize(T input) : this(input, input) { }

    public MSize(T height, T width) : base()
    {
        Height = height;
        Width = width;
    }

    ///

    public override void OnPropertyChanged(PropertyEventArgs e)
    {
        base.OnPropertyChanged(e);
        Changed?.Invoke(this);
    }

    public override string ToString() => $"{nameof(Height)} = {Height}, {nameof(Width)} = {Width}";

    ///

    public MSize<T> Clone() => new(Width, Height);
    object ICloneable.Clone() => Clone();

    ///

    public static bool operator ==(MSize<T> left, MSize<T> right) => left.EqualsOverload(right);

    public static bool operator !=(MSize<T> left, MSize<T> right) => !(left == right);

    public bool Equals(MSize<T> i) => this.Equals<MSize<T>>(i) && Height.Equals(i.Height) && Width.Equals(i.Width);

    public override bool Equals(object i) => Equals(i as MSize<T>);

    public override int GetHashCode() => new { Height, Width }.GetHashCode();
}

[Serializable]
public class Int32Size : MSize<int>
{
    public Int32Size() : base() { }

    public Int32Size(int input) : base(input) { }

    public Int32Size(int height, int width) : base(height, width) { }

    ///

    public static Int32Size operator +(Int32Size left, double right) => new((left.Height + right).Int32(), (left.Width + right).Int32());

    public static Int32Size operator -(Int32Size left, double right) => new((left.Height - right).Int32(), (left.Width - right).Int32());

    public static Int32Size operator *(Int32Size left, double right) => new((left.Height * right).Int32(), (left.Width * right).Int32());

    public static Int32Size operator /(Int32Size left, double right) => new((left.Height / right).Int32(), (left.Width / right).Int32());

    ///

    public Int32Size Resize(SizeProperty field, int value)
    {
        int oldHeight = Height, oldWidth = Width, newHeight = 0, newWidth = 0;
        switch (field)
        {
            case SizeProperty.Height:
                newHeight = value;
                newWidth = (newHeight.Double() / (oldHeight.Double() / oldWidth.Double())).Int32();
                break;

            case SizeProperty.Width:
                newWidth = value;
                newHeight = (newWidth.Double() * (oldHeight.Double() / oldWidth.Double())).Int32();
                break;
        }
        return new Int32Size(newHeight, newWidth);
    }
}

[Categorize(false), Serializable]
public class DoubleSize : MSize<double>
{
    public DoubleSize() : base() { }

    public DoubleSize(double input) : base(input) { }

    public DoubleSize(double height, double width) : base(height, width) { }

    ///

    public static DoubleSize operator +(DoubleSize left, double right) => new(left.Height + right, left.Width + right);

    public static DoubleSize operator -(DoubleSize left, double right) => new(left.Height - right, left.Width - right);

    public static DoubleSize operator *(DoubleSize left, double right) => new(left.Height * right, left.Width * right);

    public static DoubleSize operator /(DoubleSize left, double right) => new(left.Height / right, left.Width / right);

    ///

    public DoubleSize Resize(SizeProperty field, double value)
    {
        double oldHeight = Height, oldWidth = Width, newHeight = 0, newWidth = 0;
        switch (field)
        {
            case SizeProperty.Height:
                newHeight = value;
                newWidth = newHeight / (oldHeight / oldWidth);
                break;

            case SizeProperty.Width:
                newWidth = value;
                newHeight = newWidth * (oldHeight / oldWidth);
                break;
        }
        return new DoubleSize(newHeight, newWidth);
    }
}