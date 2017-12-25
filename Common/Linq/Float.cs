using System;

namespace Imagin.Common.Linq
{
    public static class FloatExtensions
    {
        public static int ToInt(this float ToConvert)
        {
            return Convert.ToInt32(ToConvert);
        }
    }
}
