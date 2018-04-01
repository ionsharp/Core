using System;

namespace Imagin.Common.Debug
{
    /// <summary>
    /// Specifies a type of <see cref="LogEntry"/>.
    /// </summary>
    [Flags]
    public enum LogEntryType
    {
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Imagin.Common:Main:None")]
        [Browsable(false)]
        None = 0,
        /// <summary>
        /// Specifies an error message.
        /// </summary>
        [DisplayName("Imagin.Common:Main:Error")]
        Error = 1,
        /// <summary>
        /// Specifies a general message.
        /// </summary>
        [DisplayName("Imagin.Common:Main:Message")]
        Message = 2,
        /// <summary>
        /// Specifies a success message.
        /// </summary>
        [DisplayName("Imagin.Common:Main:Success")]
        Success = 4,
        /// <summary>
        /// Specifies a warning message.
        /// </summary>
        [DisplayName("Imagin.Common:Main:Warning")]
        Warning = 8,
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [DisplayName("Imagin.Common:Main:All")]
        All = Error | Message | Success | Warning
    }
}
