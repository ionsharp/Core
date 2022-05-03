using Imagin.Common.Linq;
using System;

namespace Imagin.Common.Numbers
{
    [Serializable]
    public class Vector<Value> : IEquatable<Vector<Value>> 
    {
        readonly Value[] values;

        public int Length 
            => values.Length;

        public Value this[int index] 
            => values[index];

        public static implicit operator Vector<Value>(Value[] input)
            => new Vector<Value>(input);

        public static implicit operator Value[] (Vector<Value> input)
            => input.values;

        public static bool operator ==(Vector<Value> left, Vector<Value> right)
            => left.EqualsOverload(right);

        public static bool operator !=(Vector<Value> left, Vector<Value> right)
            => !(left == right);

        public Vector(params Value[] values)
        {
            this.values = values;
        }

        public TOutput[] Transform<TOutput>(Func<int, Value, TOutput> action)
        {
            var result = new TOutput[values.Length];

            for (int i = 0, length = values.Length; i < length; i++)
                result[i] = action(i, values[i]);

            return result;
        }

        public Vector<Value> Each(Func<int, Value, Value> action)
        {
            var result = new Value[Length];

            for (int i = 0, count = Length; i < count; i++)
                result[i] = action(i, this[i]);

            return new Vector<Value>(result);
        }

        public bool Equals(Vector<Value> o) 
            => this.Equals<Vector<Value>>(o) && values == o.values;

        public override bool Equals(object o)
            => Equals((Vector<Value>)o);

        public override int GetHashCode() 
            => values.GetHashCode();
    }
}