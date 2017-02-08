using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// Specifies a key that is capable of generating a token when pressed.
    /// </summary>
    [Flags]
    [Serializable]
    public enum TokenTriggerKey
    {
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
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
        [Browsable(false)]
        All = Return | Tab
    }
}
