using System.Collections;

namespace Imagin.Common.Linq
{
    public static class XICollection
    {
        public static bool Contains(this ICollection input, object i)
        {
            foreach (var j in input)
            {
                if (j.Equals(i))
                    return true;
            }
            return false;
        }
    }
}