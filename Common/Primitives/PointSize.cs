using System.Windows;

namespace Imagin.Common.Primitives
{
    public struct PointSize
    {
        public Point Point
        {
            get; set;
        }

        public Size Size
        {
            get; set;
        }

        public PointSize(Point Point, Size Size)
        {
            this.Point = Point;
            this.Size = Size;
        }
    }
}
