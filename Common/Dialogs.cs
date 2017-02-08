using System.Windows;

namespace Imagin.Common
{
    /// <summary>
    /// Provides facilities for showing a dialogs anywhere.
    /// </summary>
    public static class Dialogs
    {
        /// <summary>
        /// Shows a dialog with an error message.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public static void ShowError(string title, string message)
        {
            //If application implements the right interface and the instance returned is not null, call the appropriate method
            (Application.Current as IDialogHost)?.GetDialog()?.ShowError(title, message);
        }
    }
}
