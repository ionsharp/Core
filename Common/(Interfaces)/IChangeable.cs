using System;

namespace Imagin.Common
{
    /// <summary>
    /// Specifies an object that can observe changes to itself.
    /// </summary>
    public interface IChangeable
    {
        /// <summary>
        /// Occurs when the object is changed in any way.
        /// </summary>
        event EventHandler<EventArgs> Changed;
    }
}
