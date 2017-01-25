namespace Imagin.Common.Debug
{
    /// <summary>
    /// Represents a result.
    /// </summary>
    public abstract class Result : Result<object>
    {
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
        public Result(object Data) : base(Data)
        {
        }
    }
}
