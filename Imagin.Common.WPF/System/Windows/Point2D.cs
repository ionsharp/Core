using Imagin.Common;
using Imagin.Common.Linq;

namespace System.Windows
{
    [Serializable]
    public class Point2D : Base, ICloneable, IEquatable<Point2D>
    {
        double x = 0;
        public double X
        {
            get => x;
            set => this.Change(ref x, value);
        }

        double y = 0;
        public double Y
        {
            get => y;
            set => this.Change(ref y, value);
        }

        public Point2D() : base() { }

        public Point2D(double x, double y) : this(new Point(x, y)) { }

        public Point2D(Point input) : base()
        {
            X = input.X;
            Y = input.Y;
        }

        public static bool operator ==(Point2D left, Point2D right) => left.EqualsOverload(right);

        public static bool operator !=(Point2D left, Point2D right) => !(left == right);

        public override bool Equals(object i) => Equals(i as Point2D);

        public bool Equals(Point2D right) => this.Equals<Point2D>(right) && x == right.x && y == right.y;

        public override int GetHashCode() => ((Point)this).GetHashCode();

        public static implicit operator Point2D(Point right) => new(right);

        public static implicit operator Point(Point2D right) => new(right.X, right.Y);

        public Point2D Clone() => new(x, y);
        object ICloneable.Clone() => Clone();

        public override string ToString() => $"X => {x}, Y => {y}";
    }
}