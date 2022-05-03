using System;

namespace Imagin.Common.Controls
{
    /// <summary>
    /// Specifies a key that is capable of generating a token when pressed.
    /// </summary>
    [Flags]
    [Serializable]
    public enum TokenTriggerKey
    {
        None = 0,
        /// <summary>
        /// Corresponds to <see cref="Key.Enter"/>.
        /// </summary>
        Return = 1,
        /// <summary>
        /// Corresponds to <see cref="Key.Tab"/>.
        /// </summary>
        Tab = 2,
        /// <summary>
        /// Corresponds to all enumerated keys.
        /// </summary>
        All = Return | Tab
    }
}