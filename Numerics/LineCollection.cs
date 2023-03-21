using Imagin.Core.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imagin.Core.Numerics;

[Serializable]
public class LineCollection<T> : List<Line<T>>
{
    public static LineCollection<T> Empty = new();
}

[Serializable]
public class Int32LineCollection : LineCollection<int>
{
    public Int32LineCollection() : base() { }

    ///

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 19;
            foreach (var i in this)
                hash = hash * 31 + i.GetHashCode();

            return hash;
        }
    }

    public static bool operator ==(Int32LineCollection a, Int32LineCollection b) => a.EqualsOverload(b);

    public static bool operator !=(Int32LineCollection a, Int32LineCollection b) => !(a == b);

    public bool Equals(Int32LineCollection i)
    {
        if (this.Equals<Int32LineCollection>(i))
        {
            if (Count == i.Count)
            {
                for (var j = 0; j < Count; j++)
                {
                    if (this[j] != i[j])
                        return false;
                }
                return true;
            }
        }
        return false;
    }

    public override bool Equals(object i) => Equals(i as Int32LineCollection);

    public override string ToString()
    {
        var result = new StringBuilder();
        foreach (var i in this)
            result.AppendLine($"Line: {i}");

        return result.ToString();
    }
}