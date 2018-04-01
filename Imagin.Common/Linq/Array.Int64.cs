using System;

namespace Imagin.Common.Linq
{
    public static partial class ArrayExtensions
    {
        /// <summary>
        /// Gets the largest value.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Int64 Largest(this Int64[] input)
        {
            var result = Int64.MinValue;
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
        public static Int64 Smallest(this Int64[] input)
        {
            var result = Int64.MaxValue;
            foreach (var i in input)
            {
                if (i < result)
                    result = i;
            }
            return result;
        }
    }
}
