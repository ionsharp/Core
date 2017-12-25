using System;
using System.Threading.Tasks;
using System.Windows;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class Dialog
    {
        #region Properties

        static IDialogHost Host
        {
            get
            {
                return Application.Current as IDialogHost;
            }
        }

        #endregion

        #region Methods

        #region Delegates

        delegate Task<TDialog> ShowAsyncAction<TDialog>(TDialog Dialog);

        #endregion

        #region Private

        static TDialog Show<TDialog>(Action<TDialog> action)
        {
            if (Host != null)
            {
                var Dialog = default(TDialog);

                if (typeof(TDialog) == typeof(IDialog))
                    Dialog = (TDialog)Host.GetDialog();

                action(Dialog);

                return Dialog;
            }

            return default(TDialog);
        }

        static async Task<TDialog> ShowAsync<TDialog>(ShowAsyncAction<TDialog> action)
        {
            if (Host != null)
            {
                var Dialog = default(TDialog);

                if (typeof(TDialog) == typeof(IDialog))
                    Dialog = (TDialog)Host.GetDialog();

                await action(Dialog);

                return Dialog;
            }

            return default(TDialog);
        }

        #endregion

        #region Public

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="buttons"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public static IDialog Show(string title, string text, Uri image, params DialogButton[] buttons)
        {
            return Show<IDialog>(d => d?.Show(title, text, image, buttons));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="content"></param>
        /// <param name="image"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public static IDialog Show(string title, string text, object content, Uri image, params DialogButton[] buttons)
        {
            return Show<IDialog>(d => d?.Show(title, text, content, image, buttons));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="image"></param>
        /// <param name="action"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public static async Task<IDialog> ShowAsync(string title, string text, Uri image, DialogAction action, params DialogButton[] buttons)
        {
            return await ShowAsync<IDialog>(async d => await d?.ShowAsync(title, text, image, action, buttons));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="content"></param>
        /// <param name="image"></param>
        /// <param name="action"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public static async Task<IDialog> ShowAsync(string title, string text, object content, Uri image, DialogAction action, params DialogButton[] buttons)
        {
            return await ShowAsync<IDialog>(async d => await d?.ShowAsync(title, text, content, image, action, buttons));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="defaultInput"></param>
        /// <param name="buttons"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public static IDialog ShowError(string title, string text, params DialogButton[] buttons)
        {
            return Show<IDialog>(d => d?.ShowError(title, text, buttons));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="defaultInput"></param>
        /// <param name="buttons"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public static IDialog ShowInfo(string title, string text, params DialogButton[] buttons)
        {
            return Show<IDialog>(d => d?.ShowInfo(title, text, buttons));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="defaultInput"></param>
        /// <param name="buttons"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public static IDialog ShowInput(string title, string text, string defaultInput, Uri image, params DialogButton[] buttons)
        {
            return Show<IDialog>(d => d?.ShowInput(title, text, defaultInput, image, buttons));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="defaultPassword"></param>
        /// <param name="image"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public static IDialog ShowPassword(string title, string text, string defaultPassword, Uri image, params DialogButton[] buttons)
        {
            return Show<IDialog>(d => d?.ShowInput(title, text, defaultPassword, image, buttons));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public static IDialog ShowSuccess(string title, string text, params DialogButton[] buttons)
        {
            return Show<IDialog>(d => d?.ShowSuccess(title, text, buttons));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="defaultInput"></param>
        /// <param name="buttons"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public static IDialog ShowWarning(string title, string text, params DialogButton[] buttons)
        {
            return Show<IDialog>(d => d?.ShowWarning(title, text, buttons));
        }

        #endregion

        #endregion
    }
}
