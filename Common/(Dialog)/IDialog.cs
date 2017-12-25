using System;
using System.Threading.Tasks;

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
        string Input
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        int Result
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        DialogType Type
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="buttons"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        IDialog Show(string title, string text, Uri image, params DialogButton[] buttons);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="content"></param>
        /// <param name="image"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        IDialog Show(string title, string text, object content, Uri image, params DialogButton[] buttons);

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
        Task<IDialog> ShowAsync(string title, string text, Uri image, DialogAction action, params DialogButton[] buttons);

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
        Task<IDialog> ShowAsync(string title, string text, object content, Uri image, DialogAction action, params DialogButton[] buttons);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        IDialog ShowError(string title, string text, params DialogButton[] buttons);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        IDialog ShowInfo(string title, string text, params DialogButton[] buttons);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="defaultInput"></param>
        /// <param name="buttons"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        IDialog ShowInput(string title, string text, string defaultInput, Uri image, params DialogButton[] buttons);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="defaultPassword"></param>
        /// <param name="image"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        IDialog ShowPassword(string title, string text, string defaultPassword, Uri image, params DialogButton[] buttons);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        IDialog ShowSuccess(string title, string text, params DialogButton[] buttons);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        IDialog ShowWarning(string title, string text, params DialogButton[] buttons);
    }
}
