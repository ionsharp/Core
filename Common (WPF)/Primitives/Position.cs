using Imagin.Common.Input;
using System;
using System.Windows;
using System.Xml.Serialization;

namespace Imagin.Common.Primitives
{
    /// <summary>
    /// Represents <see cref="System.Windows.Point"/> in two-dimensional space with binding support; variant of <see cref="System.Windows.Point"/>.
    /// </summary>
    [Serializable]
    public class Position : BindableObject, ICloneable, IVariant<Point>
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<EventArgs<Point>> Changed;

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
                OnChanged(Point);
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
                OnChanged(Point);
                OnPropertyChanged("Y");
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

        #endregion

        #region Position

        /// <summary>
        /// 
        /// </summary>
        public Position() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Point"></param>
        public Position(Point Point) : base()
        {
            Set(Point);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public Position(double X, double Y) : base()
        {
            Set(X, Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public static implicit operator Position(Point Value)
        {
            return new Position(Value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Position Clone()
        {
            return new Position(Point);
        }
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Point Get()
        {
            return Point;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        public void OnChanged(Point Value)
        {
            Changed?.Invoke(this, new EventArgs<Point>(Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Point"></param>
        public void Set(Point Point)
        {
            Set(Point.X, Point.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Set(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Point.ToString();
        }

        #endregion
    }
}
