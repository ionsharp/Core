namespace Imagin.Common.Debug
{
    /// <summary>
    /// Represents a result with data.
    /// </summary>
    public abstract class Result<TData> : Result
    {
        /// <summary>
        /// 
        /// </summary>
        public new TData Data
        {
            get => (TData)base.Data;
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
        public Result(TData Data) : base(Data)
        {
        }
    }
}
