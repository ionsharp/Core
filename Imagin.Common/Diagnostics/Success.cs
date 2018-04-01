namespace Imagin.Common.Debug
{
    /// <summary>
    /// Represents a successful result.
    /// </summary>
    public class Success : Success<object>
    {
        /// <summary>
        /// 
        /// </summary>
        public Success() : base(string.Empty, default(object))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        public Success(string Message) : base(Message, default(object))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Data"></param>
        public Success(object Data) : base(string.Empty, Data)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Data"></param>
        public Success(string Message, object Data) : base(Data)
        {
        }
    }
}
