namespace Imagin.Common
{
    /// <summary>
    /// Provides facilities for managing arrays.
    /// </summary>
    public static class Arr
    {
        /// <summary>
        /// Initializes a new array with the given elements; if no elements are specified, an empty array is returned.
        /// </summary>
        /// <typeparam name="TKind"></typeparam>
        /// <param name="Elements"></param>
        /// <returns></returns>
        public static TKind[] New<TKind>(params TKind[] Elements)
        {
            return Elements ?? new TKind[0];
        }
    }
}
