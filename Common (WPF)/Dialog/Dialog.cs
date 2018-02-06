using Imagin.Common.Linq;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
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
            get => Application.Current as IDialogHost;
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
        /// <param name="buttons"></param>
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
        /// <param name="buttons"></param>
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
        /// <param name="buttons"></param>
        /// <returns></returns>
        public static IDialog ShowWarning(string title, string text, params DialogButton[] buttons)
        {
            return Show<IDialog>(d => d?.ShowWarning(title, text, buttons));
        }

        #endregion

        #endregion

        #region Windows

        struct DefaultTitle
        {
            public const string Folder = "Browse";

            public const string Open = "Open";

            public const string Save = "Save";
        }

        static CommonFileDialog GetDialog(string title, WindowsDialogMode dialogMode, WindowsDialogSelectionMode dialogSelectionMode, IEnumerable<string> extensions, string defaultPath)
        {
            var Result = default(CommonFileDialog);

            var f = dialogMode == WindowsDialogMode.OpenFolder;

            switch (dialogMode)
            {
                case WindowsDialogMode.Open:
                case WindowsDialogMode.OpenFile:
                case WindowsDialogMode.OpenFolder:
                    Result = new CommonOpenFileDialog()
                    {
                        AddToMostRecentlyUsedList = true,
                        AllowNonFileSystemItems = f,
                        DefaultDirectory = Environment.SpecialFolder.Desktop.GetPath(),
                        DefaultFileName = defaultPath.IsNull() ? string.Empty : defaultPath,
                        EnsureValidNames = true,
                        EnsureFileExists = !f,
                        EnsurePathExists = true,
                        InitialDirectory = defaultPath,
                        IsFolderPicker = f,
                        Multiselect = dialogSelectionMode == WindowsDialogSelectionMode.Multiple || f,
                        NavigateToShortcut = true,
                        ShowHiddenItems = true,
                        ShowPlacesList = true,
                        Title = title.IsNullOrEmpty() ? DefaultTitle.Open : title,
                    };
                    break;
                case WindowsDialogMode.SaveFile:
                    Result = new CommonSaveFileDialog()
                    {
                        AddToMostRecentlyUsedList = true,
                        AlwaysAppendDefaultExtension = true,
                        DefaultDirectory = Environment.SpecialFolder.Desktop.GetPath(),
                        EnsureValidNames = true,
                        InitialDirectory = defaultPath,
                        IsExpandedMode = true,
                        NavigateToShortcut = true,
                        OverwritePrompt = true,
                        ShowHiddenItems = true,
                        ShowPlacesList = true,
                        Title = title.IsNullOrEmpty() ? DefaultTitle.Save : title,
                    };
                    break;
            }
            if (extensions != null && !f)
            {
                foreach (var i in extensions)
                    Result.Filters.Add(new CommonFileDialogFilter(i.ToUpper() + " Files", "*." + i));
            }

            if (!f)
                Result.Filters.Add(new CommonFileDialogFilter("(*) All Files", "*"));

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="title"></param>
        /// <param name="dialogMode"></param>
        /// <param name="dialogSelectionMode"></param>
        /// <param name="extensions"></param>
        /// <param name="defaultPath"></param>
        /// <returns></returns>
        public static bool Show(out string[] paths, string title = "", WindowsDialogMode dialogMode = WindowsDialogMode.OpenFile, WindowsDialogSelectionMode dialogSelectionMode = WindowsDialogSelectionMode.Single, IEnumerable<string> extensions = null, string defaultPath = "")
        {
            paths = new string[0];

            var Dialog = GetDialog(title, dialogMode, dialogSelectionMode, extensions, defaultPath);

            var Result = Dialog.ShowDialog();

            if (Result == CommonFileDialogResult.Ok)
            {
                switch (dialogMode)
                {
                    case WindowsDialogMode.Open:
                    case WindowsDialogMode.OpenFile:
                    case WindowsDialogMode.OpenFolder:
                        paths = (Dialog as CommonOpenFileDialog).FileNames.ToArray<string>();
                        break;
                    case WindowsDialogMode.SaveFile:
                        paths = new string[]
                        {
                            (Dialog as CommonSaveFileDialog).FileName
                        };
                        break;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="title"></param>
        /// <param name="dialogMode"></param>
        /// <param name="extensions"></param>
        /// <param name="defaultPath"></param>
        /// <returns></returns>
        public static bool Show(out string path, string title = "", WindowsDialogMode dialogMode = WindowsDialogMode.OpenFile, IEnumerable<string> extensions = null, string defaultPath = "")
        {
            path = string.Empty;

            var Dialog = GetDialog(title, dialogMode, WindowsDialogSelectionMode.Single, extensions, defaultPath);

            var result = Dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                path = (Dialog as CommonFileDialog).FileName;
                return true;
            }
            return false;
        }

        #endregion
    }
}
