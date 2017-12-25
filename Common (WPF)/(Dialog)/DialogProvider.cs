using Imagin.Common.Linq;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using Imagin.Common.Linq;

namespace Imagin.Common
{
    public static class DialogProvider
    {  
        struct DefaultTitle
        {
            public static string Folder = "Browse...";

            public static string Open = "Open...";

            public static string Save = "Save...";
        }

        static CommonFileDialog GetDialog(string Title, DialogProviderMode DialogProviderMode, DialogProviderSelectionMode DialogProviderSelectionMode, IEnumerable<string> Extensions, string DefaultPath)
        {
            var Result = default(CommonFileDialog);

            var f = DialogProviderMode == DialogProviderMode.OpenFolder;

            switch (DialogProviderMode)
            {
                case DialogProviderMode.Open:
                case DialogProviderMode.OpenFile:
                case DialogProviderMode.OpenFolder:
                    Result = new CommonOpenFileDialog()
                    {
                        AddToMostRecentlyUsedList = true,
                        AllowNonFileSystemItems = f,
                        DefaultDirectory = Environment.SpecialFolder.Desktop.GetPath(),
                        DefaultFileName = DefaultPath.IsNull() ? string.Empty : DefaultPath,
                        EnsureValidNames = true,
                        EnsureFileExists = !f,
                        EnsurePathExists = true,
                        InitialDirectory = DefaultPath,
                        IsFolderPicker = f,
                        Multiselect = DialogProviderSelectionMode == DialogProviderSelectionMode.Multiple || f,
                        NavigateToShortcut = true,
                        ShowHiddenItems = true,
                        ShowPlacesList = true,
                        Title = Title.IsNullOrEmpty() ? DefaultTitle.Open : Title,
                    };
                    break;
                case DialogProviderMode.SaveFile:
                    Result = new CommonSaveFileDialog()
                    {
                        AddToMostRecentlyUsedList = true,
                        AlwaysAppendDefaultExtension = true,
                        DefaultDirectory = Environment.SpecialFolder.Desktop.GetPath(),
                        EnsureValidNames = true,
                        InitialDirectory = DefaultPath,
                        IsExpandedMode = true,
                        NavigateToShortcut = true,
                        OverwritePrompt = true,
                        ShowHiddenItems = true,
                        ShowPlacesList = true,
                        Title = Title.IsNullOrEmpty() ? DefaultTitle.Save : Title,
                    };
                    break;
            }
            if (Extensions != null && !f)
            {
                foreach (var i in Extensions)
                    Result.Filters.Add(new CommonFileDialogFilter(i.ToUpper() + " Files", "*." + i));
            }

            if (!f)
                Result.Filters.Add(new CommonFileDialogFilter("(*) All Files", "*"));

            return Result;
        }

        public static bool Show(out string[] Paths, string Title = "", DialogProviderMode DialogProviderMode = DialogProviderMode.OpenFile, DialogProviderSelectionMode DialogProviderSelectionMode = DialogProviderSelectionMode.Single, IEnumerable<string> Extensions = null, string DefaultPath = "")
        {
            Paths = new string[0];

            var Dialog = GetDialog(Title, DialogProviderMode, DialogProviderSelectionMode, Extensions, DefaultPath);

            var Result = Dialog.ShowDialog();

            if (Result == CommonFileDialogResult.Ok)
            {
                switch (DialogProviderMode)
                {
                    case DialogProviderMode.Open:
                    case DialogProviderMode.OpenFile:
                    case DialogProviderMode.OpenFolder:
                        Paths = (Dialog as CommonOpenFileDialog).FileNames.ToArray<string>();
                        break;
                    case DialogProviderMode.SaveFile:
                        Paths = new string[] 
                        {
                            (Dialog as CommonSaveFileDialog).FileName
                        };
                        break;
                }
                return true;
            }
            return false;
        }

        public static bool Show(out string Path, string Title = "", DialogProviderMode DialogProviderMode = DialogProviderMode.OpenFile, IEnumerable<string> Extensions = null, string DefaultPath = "")
        {
            Path = string.Empty;

            var Dialog = GetDialog(Title, DialogProviderMode, DialogProviderSelectionMode.Single, Extensions, DefaultPath);

            var Result = Dialog.ShowDialog();
            if (Result == CommonFileDialogResult.Ok)
            {
                Path = (Dialog as CommonFileDialog).FileName;
                return true;
            }
            return false;
        }
    }
}
