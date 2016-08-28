using System;

namespace Imagin.Common.Extensions
{
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static string ToShortTime(this TimeSpan t)
        {
            if (t.TotalSeconds == 0) return string.Empty;
            string Result = string.Empty;
            if (t.Hours > 0)
            {
                Result += string.Format("{0}h ", t.Hours.ToString());
            }
            if (t.Minutes > 0)
            {
                Result += string.Format("{0}m ", t.Minutes.ToString());
            }
            if (t.Seconds > 0)
            {
                Result += string.Format("{0}s", t.Seconds.ToString());
            }
            return Result;
        }
    }
}
