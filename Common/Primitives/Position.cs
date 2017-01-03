using System.Windows;

namespace Imagin.Common.Primitives
{
    public class Position : AbstractObject
    {
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
    }
}
