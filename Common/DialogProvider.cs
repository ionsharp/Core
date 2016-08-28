using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace Imagin.Common
{
    public class DialogProvider
    {
        #region Internal Enums

        public enum Type
        {
            Open,
            Save,
            Folder,
            Error
        }
        public enum Mode
        {
            None,
            Single,
            Multiple
        }
        public enum Filter
        {
            Readable,
            Writable
        }

        #endregion

        public static string GetFilter(IEnumerable<string> Formats = null)
        {
            string Result = string.Empty;
            if (Formats != null) foreach (string e in Formats) Result += "." + e + "|*." + e + "|";
            Result += "All Files (*)|*.*|";
            return Result.Substring(0, Result.Length - 1); //For whatever reason, <return Result> throws error.
        }

        public static bool Show(out string[] Paths, string Title = "", Type Type = Type.Open, Mode Mode = Mode.Single, IEnumerable<string> Formats = null, string DefaultPath = "")
        {
            switch (Type)
            {
                case Type.Open:
                    OpenFileDialog OpenFileDialog = new OpenFileDialog();
                    OpenFileDialog.AddExtension = true;
                    OpenFileDialog.CheckPathExists = true;
                    OpenFileDialog.Filter = DialogProvider.GetFilter(Formats);
                    OpenFileDialog.Multiselect = Mode == Mode.Single ? false : true;
                    OpenFileDialog.Title = Title.Length == 0 ? "Open..." : Title;
                    OpenFileDialog.FileName = DefaultPath == null || DefaultPath == string.Empty ? string.Empty : System.IO.Path.GetFileName(DefaultPath);
                    Nullable<bool> OpenFileDialogResult = OpenFileDialog.ShowDialog();
                    if (OpenFileDialogResult == true)
                    {
                        Paths = OpenFileDialog.FileNames;
                        return true;
                    }
                    else
                    {
                        Paths = null;
                        return false;
                    }
                case Type.Save:
                    SaveFileDialog SaveFileDialog = new SaveFileDialog();
                    SaveFileDialog.AddExtension = true;
                    SaveFileDialog.CheckPathExists = true;
                    SaveFileDialog.Filter = DialogProvider.GetFilter(Formats);
                    SaveFileDialog.Title = Title.Length == 0 ? "Save As..." : Title;
                    SaveFileDialog.FileName = DefaultPath == null || DefaultPath == string.Empty ? string.Empty : System.IO.Path.GetFileName(DefaultPath);

                    Nullable<bool> SaveFileDialogResult = SaveFileDialog.ShowDialog();
                    if (SaveFileDialogResult == true)
                    {

                        Paths = SaveFileDialog.FileNames;
                        return true;
                    }
                    else
                    {
                        Paths = null;
                        return false;
                    }
                case Type.Folder:
                    System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                    FolderBrowserDialog.ShowNewFolderButton = true;
                    FolderBrowserDialog.Description = Title.Length == 0 ? "Browse..." : Title;
                    FolderBrowserDialog.SelectedPath = DefaultPath;

                    System.Windows.Forms.DialogResult FolderBrowserDialogResult = FolderBrowserDialog.ShowDialog();
                    if (FolderBrowserDialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        Paths = new string[1];
                        Paths[0] = FolderBrowserDialog.SelectedPath;
                        return true;
                    }
                    else
                    {
                        Paths = null;
                        return false;
                    }
                default:
                    Paths = null;
                    return false;
            }
        }
    }
}
