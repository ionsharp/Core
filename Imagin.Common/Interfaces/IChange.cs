namespace Imagin.Common
{
    public delegate void ChangedEventHandler(object sender);

    /// <summary>
    /// Specifies an <see cref="object"/> that changes.
    /// </summary>
    public interface IChange
    {
        /// <summary>
        /// Occurs when the <see cref="object"/> changes.
        /// </summary>
        event ChangedEventHandler Changed;
    }
}
