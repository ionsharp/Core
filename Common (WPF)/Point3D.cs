using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;

namespace Imagin.Common
{
    /// <summary>
    /// A bindable variation of <see cref="System.Windows.Media.Media3D.Point3D"/>.
    /// </summary>
    [Serializable]
    public class Point3D : ObjectBase, ICloneable, IEquatable<Point3D>, IVariation<System.Windows.Media.Media3D.Point3D>
    {
        /// <summary>
        /// 
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<EventArgs<System.Windows.Media.Media3D.Point3D>> Changed;

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
        double _z = 0;
        /// <summary>
        /// 
        /// </summary>
        public double Z
        {
            get => _z;
            set
            {
                SetValue(ref _z, value);
                OnChanged(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Point3D() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public Point3D(Point input) : this(new System.Windows.Media.Media3D.Point3D(input.X, input.Y, 0)) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public Point3D(System.Windows.Media.Media3D.Point3D input) : base() => Set(input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Point3D(double x, double y, double z) : this(new System.Windows.Media.Media3D.Point3D(x, y, z)) { }

#pragma warning disable 1591
        public static bool operator ==(Point3D left, Point3D right) => left.Equals_(right);

        public static bool operator !=(Point3D left, Point3D right) => !(left == right);

        public static implicit operator Point3D(Point right) => new Point3D(right);

        public static implicit operator Point(Point3D right) => new Point(right.X, right.Y);

        public static implicit operator Point3D(System.Windows.Media.Media3D.Point3D right) => new Point3D(right);

        public static implicit operator System.Windows.Media.Media3D.Point3D(Point3D right) => new System.Windows.Media.Media3D.Point3D(right.X, right.Y, right.Z);
#pragma warning disable 1591

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public bool Equals(Point3D o) => this.Equals<Point3D>(o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Point3D Clone() => new Point3D(_x, _y, _z);
        object ICloneable.Clone() => Clone();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public System.Windows.Media.Media3D.Point3D Get() => this;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public void OnChanged(System.Windows.Media.Media3D.Point3D Value) => Changed?.Invoke(this, new EventArgs<System.Windows.Media.Media3D.Point3D>(Value));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public void Set(System.Windows.Media.Media3D.Point3D input)
        {
            X = input.X;
            Y = input.Y;
            Z = input.Z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o) => Equals((Point3D)o);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => ((System.Windows.Media.Media3D.Point3D)this).GetHashCode();
    }
}
