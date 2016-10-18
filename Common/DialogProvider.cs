using Imagin.Common.Extensions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace Imagin.Common
{
    public enum DialogProviderType
    {
        FolderBrowser,
        OpenFile,
        SaveFile
    }

    public enum DialogProviderMode
    {
        Single,
        Multiple
    }

    public static class DialogProvider
    {
        struct DefaultTitle
        {
            public static string Folder = "Browse...";

            public static string Open = "Open...";

            public static string Save = "Save...";
        }

        static string GetFilter(IEnumerable<string> Formats = null)
        {
            if (Formats.IsNull()) Formats = new[] { "*" };
            string Result = string.Empty;
            foreach (string e in Formats)
            {
                if (e == "*") Result += "All Files (*)|*.*|";
                else Result += "." + e + "|*." + e + "|";
            }
            return Result.Substring(0, Result.Length - 1);
        }

        static System.Windows.Forms.FolderBrowserDialog GetFolderBrowserDialog(string Title, string DefaultPath)
        {
            return new System.Windows.Forms.FolderBrowserDialog()
            {
                Description = Title.IsNullOrEmpty() ? DefaultTitle.Folder : Title,
                SelectedPath = DefaultPath.IsNull() ? string.Empty : DefaultPath
            };
        }

        static OpenFileDialog GetOpenFileDialog(string Title, bool Multiselect, string Format, string DefaultPath)
        {
            return new OpenFileDialog()
            {
                AddExtension = true,
                CheckPathExists = true,
                Filter = Format,
                Multiselect = Multiselect,
                Title = Title.IsNullOrEmpty() ? DefaultTitle.Open : Title,
                FileName = DefaultPath.IsNull() ? string.Empty : DefaultPath
            };
        }

        static SaveFileDialog GetSaveFileDialog(string Title, string Format, string DefaultPath)
        {
            return new SaveFileDialog()
            {
                AddExtension = true,
                CheckPathExists = true,
                Filter = Format,
                Title = Title.IsNullOrEmpty() ? DefaultTitle.Save : Title,
                FileName = DefaultPath.IsNull() ? string.Empty : DefaultPath
            };
        }
        
        public static bool Show(out string[] Paths, string Title = "", DialogProviderType Type = DialogProviderType.OpenFile, DialogProviderMode Mode = DialogProviderMode.Single, IEnumerable<string> Formats = null, string DefaultPath = "")
        {
            switch (Type)
            {
                case DialogProviderType.FolderBrowser:
                    var FolderBrowserDialog = GetFolderBrowserDialog(Title, DefaultPath);
                    if (FolderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Paths = new[] { FolderBrowserDialog.SelectedPath };
                        return true;
                    }
                    break;
                case DialogProviderType.OpenFile:
                    var OpenFileDialog = GetOpenFileDialog(Title, Mode == DialogProviderMode.Multiple, GetFilter(Formats), DefaultPath);
                    if (OpenFileDialog.ShowDialog().Value)
                    {
                        Paths = OpenFileDialog.FileNames;
                        return true;
                    }
                    break;
                case DialogProviderType.SaveFile:
                    var SaveFileDialog = GetSaveFileDialog(Title, GetFilter(Formats), DefaultPath);
                    if (SaveFileDialog.ShowDialog().Value)
                    {
                        Paths = SaveFileDialog.FileNames;
                        return true;
                    }
                    break;
            }
            Paths = null;
            return false;
        }

        public static bool Show(out string Path, string Title = "", DialogProviderType Type = DialogProviderType.OpenFile, IEnumerable<string> Formats = null, string DefaultPath = "")
        {
            switch (Type)
            {
                case DialogProviderType.FolderBrowser:
                    var FolderBrowserDialog = GetFolderBrowserDialog(Title, DefaultPath);
                    if (FolderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Path = FolderBrowserDialog.SelectedPath;
                        return true;
                    }
                    break;
                case DialogProviderType.OpenFile:
                    var OpenFileDialog = GetOpenFileDialog(Title, false, GetFilter(Formats), DefaultPath);
                    if (OpenFileDialog.ShowDialog().Value)
                    {
                        Path = OpenFileDialog.FileNames[0];
                        return true;
                    }
                    break;
                case DialogProviderType.SaveFile:
                    var SaveFileDialog = GetSaveFileDialog(Title, GetFilter(Formats), DefaultPath);
                    if (SaveFileDialog.ShowDialog() == true)
                    {
                        Path = SaveFileDialog.FileNames[0];
                        return true;
                    }
                    break;
            }
            Path = string.Empty;
            return false;
        }
    }
}
