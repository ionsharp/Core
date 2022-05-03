using Imagin.Common;
using Imagin.Common.Linq;

namespace System.Windows
{
    [Serializable]
    public class Point3D : Base, ICloneable, IEquatable<Point3D>
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

        double z = 0;
        public double Z
        {
            get => z;
            set => this.Change(ref z, value);
        }

        public Point3D() : base() { }

        public Point3D(Point input) : this(new Media.Media3D.Point3D(input.X, input.Y, 0)) { }

        public Point3D(Media.Media3D.Point3D input) : base()
        {
            X = input.X;
            Y = input.Y;
            Z = input.Z;
        }

        public Point3D(double x, double y, double z) : this(new Media.Media3D.Point3D(x, y, z)) { }

        public static bool operator ==(Point3D left, Point3D right) => left.EqualsOverload(right);

        public static bool operator !=(Point3D left, Point3D right) => !(left == right);

        public override bool Equals(object i) => Equals(i as Point3D);

        public bool Equals(Point3D right) => this.Equals<Point3D>(right) && x == right.x && y == right.y && z == right.z;

        public override int GetHashCode() => ((Media.Media3D.Point3D)this).GetHashCode();

        public static implicit operator Point3D(Point right) => new(right);

        public static implicit operator Point(Point3D right) => new(right.X, right.Y);

        public static implicit operator Point3D(Media.Media3D.Point3D right) => new(right);

        public static implicit operator Media.Media3D.Point3D(Point3D right) => new(right.X, right.Y, right.Z);

        public Point3D Clone() => new(x, y, z);
        object ICloneable.Clone() => Clone();
    }
}