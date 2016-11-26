using System;

namespace Imagin.Common.Tracing
{
    [Flags]
    public enum WarningLevel
    {
        /// <summary>
        /// Display no warnings.
        /// </summary>
        None = 0,
        /// <summary>
        /// Display insignificant warnings.
        /// </summary>
        Low = 1,
        /// <summary>
        ///Display regular warnings.
        /// </summary>
        Moderate = 2,
        /// <summary>
        /// Display significant warnings.
        /// </summary>
        High = 4,
        /// <summary>
        /// Display critical warnings.
        /// </summary>
        Severe = 8,
        /// <summary>
        /// Display all warnings.
        /// </summary>
        All = Low | Moderate | High | Severe
    }
}
