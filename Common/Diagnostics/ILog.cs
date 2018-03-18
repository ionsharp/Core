using System.Threading.Tasks;

namespace Imagin.Common.Debug
{
    /// <summary>
    /// Specifies a log.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// 
        /// </summary>
        void Clear();

        /// <summary>
        /// Load current instance with data from file it is associated with.
        /// </summary>
        /// <returns>The result of the task.</returns>
        Task<Result> LoadAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Type"></param>
        void Write(object Message, LogEntryType Type = LogEntryType.Message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Source"></param>
        /// <param name="Type"></param>
        void Write(object Message, object Source, LogEntryType Type = LogEntryType.Message);
    }
}
