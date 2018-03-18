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
        /// Adds given value (a.X + b, a.Y + b).
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point Add(this Point a, double b)
        {
            return a.Add(new Point(b, b));
        }

        /// <summary>
        /// Adds given point (a.X + b.X, a.Y + b.Y).
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point Add(this Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="maximum"></param>
        /// <param name="minimum"></param>
        /// <returns></returns>
        public static Point Coerce(this Point source, Point maximum, Point minimum = default(Point))
        {
            minimum = minimum == default(Point) ? new Point(0, 0) : minimum;
                
            var x = source.X;
            var y = source.Y;

            x = x > maximum.X ? maximum.X : (x < minimum.X ? minimum.X : x);
            y = y > maximum.Y ? maximum.Y : (y < minimum.Y ? minimum.Y : y);

            return new Point(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point Divide(this Point a, double b)
        {
            return a.Divide(new Point(b, b));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point Divide(this Point a, Point b)
        {
            return new Point(a.X / b.X, a.Y / b.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point Multiply(this Point a, double b)
        {
            return a.Multiply(new Point(b, b));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point Multiply(this Point a, Point b)
        {
            return new Point(a.X * b.X, a.Y * b.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point Subtract(this Point a, double b)
        {
            return a.Subtract(new Point(b, b));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point Subtract(this Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }
    }
}
