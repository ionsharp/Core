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
        public static Int16 Largest(this Int16[] input)
        {
            var result = Int16.MinValue;
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
        public static Int16 Smallest(this Int16[] input)
        {
            var result = Int16.MaxValue;
            foreach (var i in input)
            {
                if (i < result)
                    result = i;
            }
            return result;
        }
    }
}
