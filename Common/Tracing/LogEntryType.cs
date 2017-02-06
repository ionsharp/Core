using System;
using System.ComponentModel;

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
        /// 
        /// </summary>
        [Browsable(false)]
        None = 0,
        /// <summary>
        /// Specifies an error message.
        /// </summary>
        Error = 1,
        /// <summary>
        /// Specifies a general message.
        /// </summary>
        Message = 2,
        /// <summary>
        /// Specifies a success message.
        /// </summary>
        Success = 4,
        /// <summary>
        /// Specifies a warning message.
        /// </summary>
        Warning = 8,
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        All = Error | Message | Success | Warning
    }
}
