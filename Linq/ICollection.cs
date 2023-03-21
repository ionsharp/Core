using System.Collections;

namespace Imagin.Core.Linq;

public static class XCollection
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