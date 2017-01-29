using System;

namespace Imagin.Common.Tracing
{
    /// <summary>
    /// Specifies a log entry.
    /// </summary>
    public interface ILogEntry
    {
        /// <summary>
        /// Specifies when the entry was created.
        /// </summary>
        DateTime Date
        {
            get;
        }

        /// <summary>
        /// Specifies a message to convey.
        /// </summary>
        string Message
        {
            get; set;
        }

        /// <summary>
        /// Specifies the source of the entry.
        /// </summary>
        object Source
        {
            get; set;
        }

        /// <summary>
        /// Specifies what kind of entry it is.
        /// </summary>
        LogEntryType Type
        {
            get; set;
        }
    }
}
