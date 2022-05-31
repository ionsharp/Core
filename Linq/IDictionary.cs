using System.Collections;

namespace Imagin.Core.Linq
{
    public static class XDictionary
    {
        public static object GetValueOrDefault(this IDictionary input, object key) => input.Contains(key) ? input[key] : null;
    }
}