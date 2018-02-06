using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class LoadingDialog
    {
        static ILoadingDialogHost Host
        {
            get
            {
                return Application.Current as ILoadingDialogHost;
            }
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
        public static async Task<ILoadingDialog> ShowAsync(string title, string text, Uri image, DialogAction action, params DialogButton[] buttons)
        {
            if (Host != null)
            {
                var dialog = Host.GetDialog();
                await dialog?.ShowAsync(title, text, image, action, buttons);
                return dialog;
            }

            return default(ILoadingDialog);
        }
    }
}
