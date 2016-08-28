using System;

namespace Imagin.Common.Extensions
{
    public static class DoubleExtensions
    {
        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static string ToFileSize(this double Bytes)
        {
            const int byteConversion = 1024;
            if (Bytes >= Math.Pow(byteConversion, 3)) //GB Range
                return string.Concat(Math.Round(Bytes / Math.Pow(byteConversion, 3), 2), " GB");
            else if (Bytes >= Math.Pow(byteConversion, 2)) //MB Range
                return string.Concat(Math.Round(Bytes / Math.Pow(byteConversion, 2), 2), " MB");
            else if (Bytes >= byteConversion) //KB Range
                return string.Concat(Math.Round(Bytes / byteConversion, 2), " KB");
            else
                return string.Concat(Bytes, " B");
        }
    }
}
