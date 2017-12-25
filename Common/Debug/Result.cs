namespace Imagin.Common.Debug
{
    /// <summary>
    /// Represents a result.
    /// </summary>
    public abstract class Result : BindableObject
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly object data;
        /// <summary>
        /// Abitrary data associated with the result.
        /// </summary>
        public virtual object Data
        {
            get
            {
                return data;
            }
        }

        /// <summary>
        /// The message associated with the result.
        /// </summary>
        public abstract string Message
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public Result() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Data"></param>
        public Result(object Data) : base()
        {
            data = Data;
        }

        /// <summary>
        /// True, if <see cref="Result"/> is <see cref="Success"/>; false, otherwise.
        /// </summary>
        /// <param name="a"></param>
        public static implicit operator bool(Result a)
        {
            return a is Success;
        }
    }
}
