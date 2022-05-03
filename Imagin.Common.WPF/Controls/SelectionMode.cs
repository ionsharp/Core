using System;

namespace Imagin.Common.Controls
{
    [Serializable]
    public enum SelectionMode
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
        /// Any number of items may be checked.
        /// </summary>
        Multiple
    }
}