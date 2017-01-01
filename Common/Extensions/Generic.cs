using System;

namespace Imagin.Common.Extensions
{
    public static class GenericExtensions
    {
        public static T[] Merge<T>(this T[] a, T[] b)
        {
            var al = a.Length;
            var bl = b.Length;
            Array.Resize<T>(ref a, al + bl);
            Array.Copy(b, 0, a, al, bl);

            return a;
        }
    }
}
