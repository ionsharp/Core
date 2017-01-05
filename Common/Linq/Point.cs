using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Imagin.Common.Extensions
{
    public static class PointExtensions
    {
        public static Point Add(this Point Point, double Value)
        {
            return Point.Add(new Point(Value, Value));
        }

        public static Point Add(this Point Point, Point Values)
        {
            return new Point(Point.X + Values.X, Point.Y + Values.Y);
        }

        public static Point Divide(this Point Point, double Value)
        {
            return Point.Divide(new Point(Value, Value));
        }

        public static Point Divide(this Point Point, Point Values)
        {
            return new Point(Point.X / Values.X, Point.Y / Values.Y);
        }

        public static Point Multiply(this Point Point, double Value)
        {
            return Point.Multiply(new Point(Value, Value));
        }

        public static Point Multiply(this Point Point, Point Values)
        {
            return new Point(Point.X * Values.X, Point.Y * Values.Y);
        }

        public static Point Subtract(this Point Point, double Value)
        {
            return Point.Subtract(new Point(Value, Value));
        }

        public static Point Subtract(this Point Point, Point Values)
        {
            return new Point(Point.X - Values.X, Point.Y - Values.Y);
        }
    }
}
