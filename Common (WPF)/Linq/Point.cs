using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class PointExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Point"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Point Add(this Point Point, double Value)
        {
            return Point.Add(new Point(Value, Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Point"></param>
        /// <param name="Values"></param>
        /// <returns></returns>
        public static Point Add(this Point Point, Point Values)
        {
            return new Point(Point.X + Values.X, Point.Y + Values.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Point"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Point Divide(this Point Point, double Value)
        {
            return Point.Divide(new Point(Value, Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Point"></param>
        /// <param name="Values"></param>
        /// <returns></returns>
        public static Point Divide(this Point Point, Point Values)
        {
            return new Point(Point.X / Values.X, Point.Y / Values.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Point"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Point Multiply(this Point Point, double Value)
        {
            return Point.Multiply(new Point(Value, Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Point"></param>
        /// <param name="Values"></param>
        /// <returns></returns>
        public static Point Multiply(this Point Point, Point Values)
        {
            return new Point(Point.X * Values.X, Point.Y * Values.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Point"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Point Subtract(this Point Point, double Value)
        {
            return Point.Subtract(new Point(Value, Value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Point"></param>
        /// <param name="Values"></param>
        /// <returns></returns>
        public static Point Subtract(this Point Point, Point Values)
        {
            return new Point(Point.X - Values.X, Point.Y - Values.Y);
        }
    }
}
