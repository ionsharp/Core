namespace Imagin.Common
{
    /// <summary>
    /// Provides facilities for managing generic objects.
    /// </summary>
    public static class _
    {
        /// <summary>
        /// Gets the default value of the given type.
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <returns></returns>
        public static TElement Default<TElement>() => default(TElement);

        /// <summary>
        /// Initializes a new array with the given elements; if no elements are specified, an empty array is returned.
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static TElement[] New<TElement>(params TElement[] elements) => elements ?? new TElement[0];
    }
}
