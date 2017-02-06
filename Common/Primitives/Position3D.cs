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
    public class Position3D : AbstractObject, ICloneable, IVariant<Point3D>
    {
        /// <summary>
        /// 
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<EventArgs<Point3D>> Changed;

        /// <summary>
        /// 
        /// </summary>
        protected double x = 0d;
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        protected double y = 0d;
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        protected double z = 0d;
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public Point Point
        {
            get
            {
                return new Point(x, y);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public Point3D Point3D
        {
            get
            {
                return new Point3D(x, y, z);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Position3D() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Point"></param>
        public Position3D(Point Point) : base()
        {
            Set(Point);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Point"></param>
        public Position3D(Point3D Point) : base()
        {
            Set(Point);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Z"></param>
        public Position3D(double X, double Y, double Z) : base()
        {
            Set(X, Y, Z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public static implicit operator Position3D(Point Value)
        {
            return new Position3D(Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public static implicit operator Position3D(Point3D Value)
        {
            return new Position3D(Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Position3D Clone()
        {
            return new Position3D(Point3D);
        }
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Point3D Get()
        {
            return Point3D;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public void OnChanged(Point3D Value)
        {
            Changed?.Invoke(this, new EventArgs<Point3D>(Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Point"></param>
        public void Set(Point Point)
        {
            Set(Point.X, Point.Y, 0d);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Point"></param>
        public void Set(Point3D Point)
        {
            Set(Point.X, Point.Y, Point.Z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public void Set(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
