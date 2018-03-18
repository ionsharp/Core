using System;
using System.Threading.Tasks;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILoadingDialog
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="image"></param>
        /// <param name="action"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        Task<ILoadingDialog> ShowAsync(string title, string text, Uri image, DialogAction action, params DialogButton[] buttons);
    }
}
