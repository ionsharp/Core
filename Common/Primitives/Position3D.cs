using System;
using System.Windows;
using System.Windows.Media.Media3D;
using Imagin.Common.Input;
using System;
using System.Windows;
using System.Xml.Serialization;

namespace Imagin.Common.Primitives
{
    /// <summary>
    /// Represents <see cref="System.Windows.Point"/> in three-dimensional space with binding support; variant of <see cref="System.Windows.Media.Media3D.Point3D"/>.
    /// </summary>
    [Serializable]
    public class Position3D : AbstractObject, IVariant<Point3D>
    {
        [field: NonSerialized]
        public event EventHandler<EventArgs<Point3D>> Changed;

        protected double x = 0d;
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                OnChanged(Point3D);
                OnPropertyChanged("X");
            }
        }

        protected double y = 0d;
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                OnChanged(Point3D);
                OnPropertyChanged("Y");
            }
        }

        protected double z = 0d;
        public double Z
        {
            get
            {
                return z;
            }
            set
            {
                z = value;
                OnChanged(Point3D);
                OnPropertyChanged("Z");
            }
        }

        [XmlIgnore]
        public Point Point
        {
            get
            {
                return new Point(x, y);
            }
        }

        [XmlIgnore]
        public Point3D Point3D
        {
            get
            {
                return new Point3D(x, y, z);
            }
        }

        public Position3D() : base()
        {
        }

        public Position3D(Point Point) : base()
        {
            Set(Point);
        }

        public Position3D(Point3D Point) : base()
        {
            Set(Point);
        }

        public Position3D(double X, double Y, double Z) : base()
        {
            Set(X, Y, Z);
        }

        public static implicit operator Position3D(Point Value)
        {
            return new Position3D(Value);
        }

        public static implicit operator Position3D(Point3D Value)
        {
            return new Position3D(Value);
        }

        public Point3D Get()
        {
            return Point3D;
        }

        public void OnChanged(Point3D Value)
        {
            Changed?.Invoke(this, new EventArgs<Point3D>(Value));
        }

        public void Set(Point Point)
        {
            Set(Point.X, Point.Y, 0d);
        }

        public void Set(Point3D Point)
        {
            Set(Point.X, Point.Y, Point.Z);
        }

        public void Set(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
