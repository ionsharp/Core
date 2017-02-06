using Imagin.Common.Debug;
using System.Threading.Tasks;

namespace Imagin.Common.Tracing
{
    /// <summary>
    /// Specifies a log.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Load current instance with data from file it is associated with.
        /// </summary>
        /// <returns>The result of the task.</returns>
        Task<Result> Load();

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
