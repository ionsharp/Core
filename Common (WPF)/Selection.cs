using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;

namespace Imagin.Common
{
    /// <summary>
    /// A bindable variation of <see cref="System.Windows.Rect"/>.
    /// </summary>
    [Serializable]
    public class Selection : ObjectBase, ICloneable, IEquatable<Selection>, IVariation<Rect>
    {
        /// <summary>
        /// 
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<EventArgs<Rect>> Changed;

        /// <summary>
        /// 
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<EventArgs<Point>> LocationChanged;

        /// <summary>
        /// 
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<EventArgs<Size>> SizeChanged;

        double _height = 0;
        /// <summary>
        /// 
        /// </summary>
        public double Height
        {
            get => _height;
            set
            {
                SetValue(ref _height, value);
                OnSizeChanged(Size);
                OnChanged(this);
            }
        }

        double _width = 0;
        /// <summary>
        /// 
        /// </summary>
        public double Width
        {
            get => _width;
            set
            {
                SetValue(ref _width, value);
                OnSizeChanged(Size);
                OnChanged(this);
            }
        }

        double _x = 0;
        /// <summary>
        /// 
        /// </summary>
        public double X
        {
            get => _x;
            set
            {
                SetValue(ref _x, value);
                OnPositionChanged(Location);
                OnChanged(this);
            }
        }

        double _y = 0;
        /// <summary>
        /// 
        /// </summary>
        public double Y
        {
            get => _y;
            set
            {
                SetValue(ref _y, value);
                OnPositionChanged(Location);
                OnChanged(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Point Location => Rect.Location;

        /// <summary>
        /// 
        /// </summary>
        public Point TopLeft => Rect.TopLeft;

        /// <summary>
        /// 
        /// </summary>
        public Point TopRight => Rect.TopRight;

        /// <summary>
        /// 
        /// </summary>
        public Point BottomLeft => Rect.BottomLeft;

        /// <summary>
        /// 
        /// </summary>
        public Point BottomRight => Rect.BottomRight;

        /// <summary>
        /// 
        /// </summary>
        public Rect Rect => new Rect(_x, _y, _width, _height);

        /// <summary>
        /// 
        /// </summary>
        public Size Size => Rect.Size;

        /// <summary>
        /// 
        /// </summary>
        public Selection() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="size"></param>
        public Selection(Point location, Size size) : this(new Rect(location, size)) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public Selection(Rect input) => Set(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Selection(double x, double y, double width, double height) : this(new Rect(x, y, width, height)) { }

#pragma warning disable 1591
        public static bool operator ==(Selection left, Selection right) => left.Equals_(right);

        public static bool operator !=(Selection left, Selection right) => !(left == right);

        public static implicit operator Selection(Rect input) => new Selection(input);

        public static implicit operator Rect(Selection input) => new Rect(input.Location, input.Size);
#pragma warning restore 1591

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Point"></param>
        protected virtual void OnPositionChanged(Point Point) => LocationChanged?.Invoke(this, new EventArgs<Point>(Point));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Size"></param>
        protected virtual void OnSizeChanged(Size Size) => SizeChanged?.Invoke(this, new EventArgs<Size>(Size));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Selection Clone() => new Selection(Rect);
        object ICloneable.Clone() => Clone();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(Selection o) => this.Equals<Selection>(o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Rect Get() => this;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void OnChanged(Rect value) => Changed?.Invoke(this, new EventArgs<Rect>(value));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Set(Rect value)
        {
            X = value.X;
            Y = value.Y;
            Width = value.Width;
            Height = value.Height;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((Selection)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => ((Rect)this).GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => string.Format("X = {0}, Y = {1}, Width = {2}, Height = {3}", X, Y, Width, Height);
    }
}
