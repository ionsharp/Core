using Imagin.Common.Linq;
using System;
using System.Runtime.CompilerServices;

namespace Imagin.Common.Numbers
{
    [Serializable]
    public class MSize<T> : Base, IChange, ICloneable, IEquatable<MSize<T>>
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

        public MSize() : this(default) { }

        public MSize(T input) : this(input, input) { }

        public MSize(T height, T width) : base()
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

        public MSize<T> Clone() => new MSize<T>(width, height);
        object ICloneable.Clone() => Clone();

        //...

        public static bool operator ==(MSize<T> left, MSize<T> right) => left.EqualsOverload(right);

        public static bool operator !=(MSize<T> left, MSize<T> right) => !(left == right);

        public bool Equals(MSize<T> i) => this.Equals<MSize<T>>(i) && height.Equals(i.height) && width.Equals(i.width);

        public override bool Equals(object i) => Equals(i as MSize<T>);

        public override int GetHashCode() => new { height, width }.GetHashCode();
    }

    [Serializable]
    public class Int32Size : MSize<int>
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

        public Int32Size Resize(SizeField field, int value)
        {
            int oldHeight = Height, oldWidth = Width, newHeight = 0, newWidth = 0;
            switch (field)
            {
                case SizeField.Height:
                    newHeight = value;
                    newWidth = (newHeight.Double() / (oldHeight.Double() / oldWidth.Double())).Int32();
                    break;

                case SizeField.Width:
                    newWidth = value;
                    newHeight = (newWidth.Double() * (oldHeight.Double() / oldWidth.Double())).Int32();
                    break;
            }
            return new Int32Size(newHeight, newWidth);
        }
    }

    [Serializable]
    public class DoubleSize : MSize<double>
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

        public DoubleSize Resize(SizeField field, double value)
        {
            double oldHeight = Height, oldWidth = Width, newHeight = 0, newWidth = 0;
            switch (field)
            {
                case SizeField.Height:
                    newHeight = value;
                    newWidth = newHeight / (oldHeight / oldWidth);
                    break;

                case SizeField.Width:
                    newWidth = value;
                    newHeight = newWidth * (oldHeight / oldWidth);
                    break;
            }
            return new DoubleSize(newHeight, newWidth);
        }
    }
}