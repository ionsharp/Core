using System;
using System.Windows;

namespace Imagin.Common.Linq
{
    public static class XPoint
    {
        #region System.Drawing.Point

        public static System.Drawing.Point Between(this System.Drawing.Point a, System.Drawing.Point b)
            => new(((a.X + b.X).Double() / 2.0).Int32(), ((a.Y + b.Y).Double() / 2.0).Int32());

        public static float Distance(this System.Drawing.Point a, System.Drawing.Point b)
            => (float)Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));

        public static Point Double(this System.Drawing.Point a) => new(a.X, a.Y);

        #endregion

        #region System.Windows.Point

        /// <summary>
        /// Adds given value (a.X + b, a.Y + b).
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point Add(this Point a, double b)
            => a.Add(new Point(b, b));

        /// <summary>
        /// Adds given point (a.X + b.X, a.Y + b.Y).
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Point Add(this Point a, Point b)
            => new(a.X + b.X, a.Y + b.Y);

        public static Point Between(this Point a, Point b)
            => new((a.X + b.X) / 2.0, (a.Y + b.Y) / 2.0);

        public static Point Coerce(this Point source, Point maximum, Point minimum = default)
        {
            minimum = minimum == default ? new Point(0, 0) : minimum;
                
            var x = source.X;
            var y = source.Y;

            x = x > maximum.X ? maximum.X : x < minimum.X ? minimum.X : x;
            y = y > maximum.Y ? maximum.Y : y < minimum.Y ? minimum.Y : y;

            return new Point(x, y);
        }

        public static float Distance(this Point a, Point b)
            => (float)Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));

        public static Point Divide(this Point a, double b)
        {
            return a.Divide(new Point(b, b));
        }

        public static Point Divide(this Point a, Point b)
        {
            return new Point(a.X / b.X, a.Y / b.Y);
        }

        public static System.Drawing.Point Int32(this Point a) => new(a.X.Int32(), a.Y.Int32());

        public static Point Multiply(this Point a, double b)
        {
            return a.Multiply(new Point(b, b));
        }

        public static Point Multiply(this Point a, Point b)
        {
            return new Point(a.X * b.X, a.Y * b.Y);
        }

        public static Point Subtract(this Point a, double b)
        {
            return a.Subtract(new Point(b, b));
        }

        public static Point Subtract(this Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        #endregion
    }
}