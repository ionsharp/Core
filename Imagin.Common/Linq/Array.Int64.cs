using System;

namespace Imagin.Common.Linq
{
    public static partial class XArray
    {
        /// <summary>
        /// Gets the largest value.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static long Largest(this long[] input)
        {
            var result = long.MinValue;
            foreach (var i in input)
            {
                if (i > result)
                    result = i;
            }
            return result;
        }

        /// <summary>
        /// Gets the smallest value.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static long Smallest(this long[] input)
        {
            var result = long.MaxValue;
            foreach (var i in input)
            {
                if (i < result)
                    result = i;
            }
            return result;
        }
    }
}