namespace Imagin.Common.Debug
{
    /// <summary>
    /// Represents a result with data.
    /// </summary>
    public class Result<T> : AbstractObject
    {
        readonly T data;
        /// <summary>
        /// Abitrary data to store a reference to based on the result.
        /// </summary>
        public T Data
        {
            get
            {
                return data;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        public Result() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="Data"></param>
        public Result(T Data) : base()
        {
            data = Data;
        }

        /// <summary>
        /// If result is success, true; else, false.
        /// </summary>
        /// <param name="a"></param>
        public static implicit operator bool(Result<T> a)
        {
            return a is Success;
        }
    }
}
