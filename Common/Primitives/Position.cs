using Imagin.Common.Input;
using System;
using System.Windows;

namespace Imagin.Common.Primitives
{
    /// <summary>
    /// Represents a point with binding support.
    /// </summary>
    [Serializable]
    public class Position : AbstractObject, IVariant<Point>
    {
        #region Properties

        [field: NonSerialized]
        public event EventHandler<EventArgs<Point>> Changed;

        double x = 0d;
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

        double y = 0d;
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

        public Point Point
        {
            get
            {
                return new Point(x, y);
            }
        }

        #endregion

        #region Position

        public Position() : base()
        {
        }

        public Position(Point Point) : base()
        {
            Set(Point);
        }

        public Position(double X, double Y) : base()
        {
            Set(X, Y);
        }

        public static implicit operator Position(Point Value)
        {
            return new Position(Value);
        }

        #endregion

        #region Methods

        public Point Get()
        {
            return Point;
        }

        public void OnChanged(Point Value)
        {
            if (Changed != null)
                Changed(this, new EventArgs<Point>(Value));
        }

        public void Set(Point Point)
        {
            Set(Point.X, Point.Y);
        }

        public void Set(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return Point.ToString();
        }

        #endregion
    }
}
