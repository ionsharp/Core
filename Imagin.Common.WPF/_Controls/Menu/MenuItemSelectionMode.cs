using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public enum MenuItemSelectionMode
    {
        /// <summary>
        /// One item may be checked.
        /// </summary>
        Single,
        /// <summary>
        /// Either one or no items may be checked.
        /// </summary>
        SingleOrNone,
        /// <summary>
        /// Any number of items may be checked; equivalent to
        /// not specifying a group name.
        /// </summary>
        Multiple
    }
}
