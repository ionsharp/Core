using Imagin.Common.Linq;
using System;

namespace Imagin.Common
{
    /// <summary>
    /// Provides facilities for managing arrays.
    /// </summary>
    public static class Batch
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Elements"></param>
        /// <returns></returns>
        public static TElement[] Add<TElement>(ref TElement[] Source, params TElement[] Elements)
        {
            var OldLength = Source.Length;
            System.Array.Resize(ref Source, OldLength + Elements.Length);

            for (int j = 0, Count = Elements.Length; j < Count; j++)
                Source[OldLength + j] = Elements[j];

            return Source;
        }

        /// <summary>
        /// Initializes a new array with the given elements; if no elements are specified, an empty array is returned.
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="Elements"></param>
        /// <returns></returns>
        public static TElement[] New<TElement>(params TElement[] Elements)
        {
            return Elements ?? new TElement[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Elements"></param>
        public static void Remove<TElement>(ref TElement[] Source, params TElement[] Elements)
        {
            foreach (var i in Elements)
                RemoveAt(ref Source, Source.IndexOf(i));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        public static void RemoveAt<TElement>(ref TElement[] Source, int Index)
        {
            var Result = new TElement[Source.Length - 1];

            if (Index > 0)
                System.Array.Copy(Source, 0, Result, 0, Index);

            if (Index < Source.Length - 1)
                System.Array.Copy(Source, Index + 1, Result, Index, Source.Length - Index - 1);

            Source = Result;
        }
    }
}
