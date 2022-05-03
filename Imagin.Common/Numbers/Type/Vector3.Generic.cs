using Imagin.Common.Linq;
using System;

namespace Imagin.Common.Numbers
{
    [Serializable]
    public class Vector3<T> : IEquatable<Vector3<T>>
    {
        public const uint Length = 2;

        public readonly T X;

        public readonly T Y;

        public readonly T Z;

        public static implicit operator T[] (Vector3<T> input) 
            => Array<T>.New(input.X, input.Y, input.Z);

        public static bool operator ==(Vector3<T> left, Vector3<T> right) 
            => left.EqualsOverload(right);

        public static bool operator !=(Vector3<T> left, Vector3<T> right) 
            => !(left == right);

        public Vector3(T xyz) : this(xyz, xyz, xyz) { }

        public Vector3(T x, T y, T z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public bool Equals(Vector3<T> o) 
            => this.Equals<Vector3<T>>(o) && X.Equals(o.X) && Y.Equals(o.Y) && Z.Equals(o.Z);

        public override bool Equals(object o) 
            => Equals((Vector3<T>)o);

        public override int GetHashCode()
            => Array<T>.New(X, Y, Z).GetHashCode();

        public override string ToString()
            => $"x = {X}, y = {Y}, z = {Z}";
    }
}