using System;

namespace Imagin.Common.Tracing
{
    /// <summary>
    /// Specifies a type of <see cref="LogEntry"/>.
    /// </summary>
    [Flags]
    [Serializable]
    public enum LogEntryType
    {
        /// <summary>
        /// Specifies an error message.
        /// </summary>
        Error = 0,
        /// <summary>
        /// Specifies a general message.
        /// </summary>
        Message = 1,
        /// <summary>
        /// Specifies a success message.
        /// </summary>
        Success = 2,
        /// <summary>
        /// Specifies a warning message.
        /// </summary>
        Warning = 4,
    }
}
