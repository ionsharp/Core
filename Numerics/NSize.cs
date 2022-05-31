using Imagin.Core.Linq;
using System;
using System.Runtime.CompilerServices;

namespace Imagin.Core.Numerics;

[Serializable]
public class NSize<T> : Base, IChange, ICloneable, IEquatable<NSize<T>>
{
    public event ChangedEventHandler Changed;

    //...

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

    public NSize() : this(default) { }

    public NSize(T input) : this(input, input) { }

    public NSize(T height, T width) : base()
    {
        Height = height;
        Width = width;
    }

    //...

    public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        base.OnPropertyChanged(propertyName);
        Changed?.Invoke(this);
    }

    public override string ToString() => $"{nameof(Height)} = {height}, {nameof(Width)} = {width}";

    //...

    public NSize<T> Clone() => new NSize<T>(width, height);
    object ICloneable.Clone() => Clone();

    //...

    public static bool operator ==(NSize<T> left, NSize<T> right) => left.EqualsOverload(right);

    public static bool operator !=(NSize<T> left, NSize<T> right) => !(left == right);

    public bool Equals(NSize<T> i) => this.Equals<NSize<T>>(i) && height.Equals(i.height) && width.Equals(i.width);

    public override bool Equals(object i) => Equals(i as NSize<T>);

    public override int GetHashCode() => new { height, width }.GetHashCode();
}

[Serializable]
public class Int32Size : NSize<int>
{
    public Int32Size() : base() { }

    public Int32Size(int input) : base(input) { }

    public Int32Size(int height, int width) : base(height, width) { }

    //...

    public static Int32Size operator +(Int32Size left, double right) => new Int32Size((left.Height + right).Int32(), (left.Width + right).Int32());

    public static Int32Size operator -(Int32Size left, double right) => new Int32Size((left.Height - right).Int32(), (left.Width - right).Int32());

    public static Int32Size operator *(Int32Size left, double right) => new Int32Size((left.Height * right).Int32(), (left.Width * right).Int32());

    public static Int32Size operator /(Int32Size left, double right) => new Int32Size((left.Height / right).Int32(), (left.Width / right).Int32());

    //...

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

[Serializable]
public class DoubleSize : NSize<double>
{
    public DoubleSize() : base() { }

    public DoubleSize(double input) : base(input) { }

    public DoubleSize(double height, double width) : base(height, width) { }

    //...

    public static DoubleSize operator +(DoubleSize left, double right) => new DoubleSize(left.Height + right, left.Width + right);

    public static DoubleSize operator -(DoubleSize left, double right) => new DoubleSize(left.Height - right, left.Width - right);

    public static DoubleSize operator *(DoubleSize left, double right) => new DoubleSize(left.Height * right, left.Width * right);

    public static DoubleSize operator /(DoubleSize left, double right) => new DoubleSize(left.Height / right, left.Width / right);

    //...

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