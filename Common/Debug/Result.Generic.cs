namespace Imagin.Common.Debug
{
    /// <summary>
    /// Represents a result with data.
    /// </summary>
    public class Result<T> : AbstractObject
    {
        public T Data
        {
            get; set;
        }

        public Result() : base()
        {
        }

        public Result(T data) : base()
        {
            Data = data;
        }

        /// <summary>
        /// If result is success, true; else, false.
        /// </summary>
        /// <param name="a"></param>
        public static implicit operator bool(Result<T> a)
        {
            return a is Success<T>;
        }
    }
}
