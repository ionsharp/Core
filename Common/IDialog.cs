namespace Imagin.Common
{
    /// <summary>
    /// Specifies a dialog.
    /// </summary>
    public interface IDialog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        void Show(string title, string text);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        void ShowError(string title, string text);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        void ShowWarning(string title, string text);
    }
}
