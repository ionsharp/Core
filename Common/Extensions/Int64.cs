using System;

namespace Imagin.Common.Extensions
{
    public static class Int64Extensions
    {
        /// <summary>
        /// Imagin.Core
        /// </summary>
        public static string ToFileSize(this long Bytes)
        {
            return Convert.ToDouble(Bytes).ToFileSize();
        }
    }
}
