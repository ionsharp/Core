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
        void Write(string Message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Message"></param>
        void Write(string Source, string Message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="Source"></param>
        /// <param name="Message"></param>
        void Write(LogEntryStatus Status, string Source, string Message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="WarningLevel"></param>
        /// <param name="Status"></param>
        /// <param name="Source"></param>
        /// <param name="Message"></param>
        void Write(WarningLevel WarningLevel, LogEntryStatus Status, string Source, string Message);
    }
}
