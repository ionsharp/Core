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
        public static Int32 Largest(this Int32[] input)
        {
            var result = Int32.MinValue;
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
        public static Int32 Smallest(this Int32[] input)
        {
            var result = Int32.MaxValue;
            foreach (var i in input)
            {
                if (i < result)
                    result = i;
            }
            return result;
        }
    }
}
