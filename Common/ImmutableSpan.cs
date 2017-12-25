namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class ImmutableSpan<T1, T2> : Span<T1, T2>
    {
        readonly T1 first = default(T1);
        /// <summary>
        /// Gets or sets the first component.
        /// </summary>
        public new T1 First
        {
            get
            {
                return first;
            }
        }

        readonly T2 second = default(T2);
        /// <summary>
        /// Gets or sets the second component.
        /// </summary>
        public new T2 Second
        {
            get
            {
                return second;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmutableSpan{T1, T2}"/> class.
        /// </summary>
        /// <param name="First"></param>
        /// <param name="Second"></param>
        public ImmutableSpan(T1 First, T2 Second) : base()
        {
            first = First;
            second = Second;
        }
    }
}
