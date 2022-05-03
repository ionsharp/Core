using Imagin.Common.Linq;
using System;

namespace Imagin.Common.Numbers
{
    [Serializable]
    public class Vector2<T> : IEquatable<Vector2<T>> 
    {
        public const uint Length = 2;

        public readonly T X;

        public readonly T Y;

        public static implicit operator T[] (Vector2<T> input)
            => Array<T>.New(input.X, input.Y);

        public static bool operator ==(Vector2<T> left, Vector2<T> right) 
            => left.EqualsOverload(right);

        public static bool operator !=(Vector2<T> left, Vector2<T> right) 
            => !(left == right);

        public Vector2() : this(default) { }

        public Vector2(T xy) : this(xy, xy) { }

        public Vector2(T x, T y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Vector2<T> o) 
            => this.Equals<Vector2<T>>(o) && X.Equals(o.X) && Y.Equals(o.Y);

        public override bool Equals(object o) 
            => Equals((Vector2<T>)o);

        public override int GetHashCode()
            => Array<T>.New(X, Y).GetHashCode();

        public override string ToString()
            => $"x = {X}, y = {Y}";
    }
}