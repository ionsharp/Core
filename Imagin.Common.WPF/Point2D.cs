using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;

namespace Imagin.Common
{
    /// <summary>
    /// A bindable variation of <see cref="Point"/>.
    /// </summary>
    [Serializable]
    public class Point2D : ObjectBase, ICloneable, IEquatable<Point2D>, IVariation<Point>
    {
        /// <summary>
        /// 
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<EventArgs<Point>> Changed;

        /// <summary>
        /// 
        /// </summary>
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
                OnChanged(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
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
                OnChanged(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Point2D() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point2D(double x, double y) : this(new Point(x, y)) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public Point2D(Point input) : base() => Set(input);

#pragma warning disable 1591
        public static bool operator ==(Point2D left, Point2D right) => left.Equals_(right);

        public static bool operator !=(Point2D left, Point2D right) => !(left == right);

        public static implicit operator Point2D(Point right) => new Point2D(right);

        public static implicit operator Point(Point2D right) => new Point(right.X, right.Y);
#pragma warning restore 1591

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Point2D Clone() => new Point2D(_x, _y);
        object ICloneable.Clone() => Clone();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(Point2D o) => this.Equals<Point2D>(o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Point Get() => this;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void OnChanged(Point value) => Changed?.Invoke(this, new EventArgs<Point>(value));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void Set(Point value)
        {
            X = value.X;
            Y = value.Y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((Point2D)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => ((Point)this).GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "X => {0}, Y => {1}".F(_x, _y);
    }
}
