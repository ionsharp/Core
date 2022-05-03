using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imagin.Common.Numbers
{
    [Serializable]
    public class MPattern<T> : List<Line<T>>
    {
        public static MPattern<T> Empty = new MPattern<T>();
    }

    [Serializable]
    public class Int32Pattern : MPattern<int>
    {
        public Int32Pattern() : base() { }

        //...

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

        public static bool operator ==(Int32Pattern a, Int32Pattern b) => a.EqualsOverload(b);

        public static bool operator !=(Int32Pattern a, Int32Pattern b) => !(a == b);

        public bool Equals(Int32Pattern i)
        {
            if (this.Equals<Int32Pattern>(i))
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

        public override bool Equals(object i) => Equals(i as Int32Pattern);

        public override string ToString()
        {
            var result = new StringBuilder();
            foreach (var i in this)
                result.AppendLine($"Line: {i}");

            return result.ToString();
        }
    }
}