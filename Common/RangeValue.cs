namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Range
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Minimum"></param>
        /// <param name="Maximum"></param>
        /// <returns></returns>
        public static Range<T> New<T>(T Minimum, T Maximum)
        {
            return new Range<T>(Minimum, Maximum);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Range<T>
    {
        T minimum;
        /// <summary>
        /// 
        /// </summary>
        public T Minimum
        {
            get
            {
                return minimum;
            }
            set
            {
                minimum = value;
            }
        }

        T maximum;
        /// <summary>
        /// 
        /// </summary>
        public T Maximum
        {
            get
            {
                return maximum;
            }
            set
            {
                maximum = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Minimum"></param>
        /// <param name="Maximum"></param>
        public Range(T Minimum, T Maximum)
        {
            minimum = Minimum;
            maximum = Maximum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Minimum"></param>
        /// <param name="Maximum"></param>
        /// <returns></returns>
        public static Range<T> New(T Minimum, T Maximum)
        {
            return new Range<T>(Minimum, Maximum);
        }
    }
}
