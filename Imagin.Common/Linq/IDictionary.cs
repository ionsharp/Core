using System.Collections;

namespace Imagin.Common.Linq
{
    public static class XIDictionary
    {
        public static object GetValueOrDefault(this IDictionary input, object key) => input.Contains(key) ? input[key] : null;
    }
}