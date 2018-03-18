namespace Imagin.Common
{
    /// <summary>
    /// Specifies an element capable of hosting dialogs.
    /// </summary>
    public interface IDialogHost
    {
        /// <summary>
        /// Gets a new instance of <see cref="IDialog"/>.
        /// </summary>
        /// <returns></returns>
        IDialog GetDialog();
    }
}
