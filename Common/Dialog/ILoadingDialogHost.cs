namespace Imagin.Common
{
    /// <summary>
    /// Specifies an element capable of hosting loading dialogs.
    /// </summary>
    public interface ILoadingDialogHost
    {
        /// <summary>
        /// Gets a new instance of <see cref="ILoadingDialog"/>.
        /// </summary>
        /// <returns></returns>
        ILoadingDialog GetDialog();
    }
}
