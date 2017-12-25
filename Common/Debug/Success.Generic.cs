using Imagin.Common.Linq;

namespace Imagin.Common.Debug
{
    /// <summary>
    /// Represents a successful result.
    /// </summary>
    public class Success<TData> : Result<TData>
    {
        readonly string message;
        /// <summary>
        /// 
        /// </summary>
        public override string Message
        {
            get
            {
                return message;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Success() : this(string.Empty, default(TData))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        public Success(string Message) : this(Message, default(TData))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Data"></param>
        public Success(TData Data) : this(string.Empty, Data)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Data"></param>
        public Success(string Message, TData Data) : base(Data)
        {
            message = Message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (message.IsNullOrEmpty())
                return GetType().ToString();

            return message;
        }
    }
}
