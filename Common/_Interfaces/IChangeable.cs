using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ChangedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Specifies an <see cref="object"/> that can observe changes to itself.
    /// </summary>
    public interface IChangeable
    {
        /// <summary>
        /// Occurs when the <see cref="object"/> changes in some way.
        /// </summary>
        event ChangedEventHandler Changed;
    }
}
