using Imagin.Common.Debug;
using System.Threading.Tasks;

namespace Imagin.Common.Tracing
{
    /// <summary>
    /// 
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
        /// <param name="Kind"></param>
        void Write(string Message, LogEntryKind Kind);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Level"></param>
        /// <param name="Kind"></param>
        void Write(string Message, WarningLevel Level = WarningLevel.Moderate, LogEntryKind Kind = LogEntryKind.Info);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Message"></param>
        /// <param name="Kind"></param>
        void Write(string Source, string Message, LogEntryKind Kind);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Message"></param>
        /// <param name="Level"></param>
        /// <param name="Kind"></param>
        void Write(string Source, string Message, WarningLevel Level = WarningLevel.Moderate, LogEntryKind Kind = LogEntryKind.Info);
    }
}
