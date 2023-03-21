using System.IO;

namespace Imagin.Core.Linq;

public static class XStream
{
    public static byte[] Array(this Stream input)
    {
        using var result = new MemoryStream();
        input.CopyTo(result);
        return result.ToArray();
    }
}