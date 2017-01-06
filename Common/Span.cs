namespace Imagin.Common
{
    /// <summary>
    /// Represents a writable 2-tuple, or pair.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class Span<T1, T2>
    {
        /// <summary>
        /// Get or set the first component.
        /// </summary>
        public T1 First { get; set; } = default(T1);

        /// <summary>
        /// Get or set the second component.
        /// </summary>
        public T2 Second { get; set; } = default(T2);

        /// <summary>
        /// Initializes a new instance of the Span class.
        /// </summary>
        /// <param name="First"></param>
        /// <param name="Second"></param>
        public Span(T1 First, T2 Second) : base()
        {
            this.First = First;
            this.Second = Second;
        }
    }

    /// <summary>
    /// Represents a writable 3-tuple.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    public class Span<T1, T2, T3> : Span<T1, T2>
    {
        /// <summary>
        /// Get or set the third component.
        /// </summary>
        public T3 Third { get; set; } = default(T3);

        /// <summary>
        /// Initializes a new instance of the Span class.
        /// </summary>
        /// <param name="First"></param>
        /// <param name="Second"></param>
        /// <param name="Third"></param>
        public Span(T1 First, T2 Second, T3 Third) : base(First, Second)
        {
            this.Third = Third;
        }
    }

    /// <summary>
    /// Represents a writable 4-tuple.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    public class Span<T1, T2, T3, T4> : Span<T1, T2, T3>
    {
        /// <summary>
        /// Get or set the fourth component.
        /// </summary>
        public T4 Fourth { get; set; } = default(T4);

        /// <summary>
        /// Initializes a new instance of the Span class.
        /// </summary>
        /// <param name="First"></param>
        /// <param name="Second"></param>
        /// <param name="Third"></param>
        /// <param name="Fourth"></param>
        public Span(T1 First, T2 Second, T3 Third, T4 Fourth) : base(First, Second, Third)
        {
            this.Fourth = Fourth;
        }
    }

    /// <summary>
    /// Represents a writable 5-tuple.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    public class Span<T1, T2, T3, T4, T5> : Span<T1, T2, T3, T4>
    {
        /// <summary>
        /// Get or set the fifth component.
        /// </summary>
        public T5 Fifth { get; set; } = default(T5);

        /// <summary>
        /// Initializes a new instance of the Span class.
        /// </summary>
        /// <param name="First"></param>
        /// <param name="Second"></param>
        /// <param name="Third"></param>
        /// <param name="Fourth"></param>
        /// <param name="Fifth"></param>
        public Span(T1 First, T2 Second, T3 Third, T4 Fourth, T5 Fifth) : base(First, Second, Third, Fourth)
        {
            this.Fifth = Fifth;
        }
    }

    /// <summary>
    /// Represents a writable 6-tuple.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    /// <typeparam name="T6"></typeparam>
    public class Span<T1, T2, T3, T4, T5, T6> : Span<T1, T2, T3, T4, T5>
    {
        /// <summary>
        /// Get or set the sixth component.
        /// </summary>
        public T6 Sixth { get; set; } = default(T6);

        /// <summary>
        /// Initializes a new instance of the Span class.
        /// </summary>
        /// <param name="First"></param>
        /// <param name="Second"></param>
        /// <param name="Third"></param>
        /// <param name="Fourth"></param>
        /// <param name="Fifth"></param>
        /// <param name="Sixth"></param>
        public Span(T1 First, T2 Second, T3 Third, T4 Fourth, T5 Fifth, T6 Sixth) : base(First, Second, Third, Fourth, Fifth)
        {
            this.Sixth = Sixth;
        }
    }
}
