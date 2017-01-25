namespace Imagin.Common.Debug
{
    /// <summary>
    /// Represents a successful result.
    /// </summary>
    public class Success : Result
    {
        /// <summary>
        /// 
        /// </summary>
        public Success() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Data"></param>
        public Success(object Data) : base(Data)
        {
        }
    }
}
