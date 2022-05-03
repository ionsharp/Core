using Imagin.Common.Controls;
using Imagin.Common.Converters;
using Imagin.Common.Storage;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Linq
{
    [Extends(typeof(IExplorer))]
    public static class XExplorer
    {
        #region (IResourceKey) ToolTipTemplateKey

        public static readonly ResourceKey ToolTipTemplateKey = new();

        #endregion

        #region (IValueConverter) LegacyToolTipConverter

        public static IValueConverter LegacyToolTipConverter = new SimpleConverter<string, string>(i =>
        {
            var result = new StringBuilder();

            var type = Computer.GetType(i);
            switch (type)
            {
                case ItemType.Drive:
                    foreach (var j in Computer.Drives)
                    {
                        if (j.Name == i)
                        {
                            result.AppendLine($"Available space: {j.AvailableFreeSpace.FileSize(Data.FileSizeFormat.BinaryUsingSI)}");
                            result.Append($"Total space: {j.TotalSize.FileSize(Data.FileSizeFormat.BinaryUsingSI)}");
                            break;
                        }
                    }
                    break;

                case ItemType.File:
                    var fileInfo = new FileInfo(i);
                    result.AppendLine($"Type: {Storage.File.Long.Description(i)}");
                    result.Append($"Size: {fileInfo.Length.FileSize(Data.FileSizeFormat.BinaryUsingSI)}");
                    break;

                case ItemType.Folder:
                    var directoryInfo = new DirectoryInfo(i);
                    result.Append($"Date created: {directoryInfo.CreationTime}");
                    break;

                case ItemType.Shortcut:
                    if (!Try.Invoke(() => result.Append($"Location: {Shortcut.TargetPath(i)}")))
                        goto case ItemType.File;

                    break;
            }
            return result.ToString();
        });

        #endregion

        #region DefaultPath

        public static readonly DependencyProperty DefaultPathProperty = DependencyProperty.RegisterAttached("DefaultPath", typeof(string), typeof(XExplorer), new FrameworkPropertyMetadata(Explorer.DefaultPath));
        public static string GetDefaultPath(IExplorer i) => (string)i.As<DependencyObject>().GetValue(DefaultPathProperty);
        public static void SetDefaultPath(IExplorer i, string input) => i.As<DependencyObject>().SetValue(DefaultPathProperty, input);

        #endregion

        #region CopyWarningTitle

        public static readonly DependencyProperty CopyWarningTitleProperty = DependencyProperty.RegisterAttached("CopyWarningTitle", typeof(string), typeof(XExplorer), new FrameworkPropertyMetadata(string.Empty));
        public static string GetCopyWarningTitle(IExplorer i) => (string)i.As<DependencyObject>().GetValue(CopyWarningTitleProperty);
        public static void SetCopyWarningTitle(IExplorer i, string input) => i.As<DependencyObject>().SetValue(CopyWarningTitleProperty, input);

        #endregion

        #region CopyWarningMessage

        public static readonly DependencyProperty CopyWarningMessageProperty = DependencyProperty.RegisterAttached("CopyWarningMessage", typeof(string), typeof(XExplorer), new FrameworkPropertyMetadata(string.Empty));
        public static string GetCopyWarningMessage(IExplorer i) => (string)i.As<DependencyObject>().GetValue(CopyWarningMessageProperty);
        public static void SetCopyWarningMessage(IExplorer i, string input) => i.As<DependencyObject>().SetValue(CopyWarningMessageProperty, input);

        #endregion

        #region InvalidPathAlert

        public static readonly DependencyProperty InvalidPathAlertProperty = DependencyProperty.RegisterAttached("InvalidPathAlert", typeof(bool), typeof(XExplorer), new FrameworkPropertyMetadata(false));
        public static bool GetInvalidPathAlert(IExplorer i) => (bool)i.As<DependencyObject>().GetValue(InvalidPathAlertProperty);
        public static void SetInvalidPathAlert(IExplorer i, bool input) => i.As<DependencyObject>().SetValue(InvalidPathAlertProperty, input);

        #endregion

        #region InvalidPathAlertMessage

        public static readonly DependencyProperty InvalidPathAlertMessageProperty = DependencyProperty.RegisterAttached("InvalidPathAlertMessage", typeof(string), typeof(XExplorer), new FrameworkPropertyMetadata(string.Empty));
        public static string GetInvalidPathAlertMessage(IExplorer i) => (string)i.As<DependencyObject>().GetValue(InvalidPathAlertMessageProperty);
        public static void SetInvalidPathAlertMessage(IExplorer i, string input) => i.As<DependencyObject>().SetValue(InvalidPathAlertMessageProperty, input);

        #endregion

        #region InvalidPathAlertTitle

        public static readonly DependencyProperty InvalidPathAlertTitleProperty = DependencyProperty.RegisterAttached("InvalidPathAlertTitle", typeof(string), typeof(XExplorer), new FrameworkPropertyMetadata(string.Empty));
        public static string GetInvalidPathAlertTitle(IExplorer i) => (string)i.As<DependencyObject>().GetValue(InvalidPathAlertTitleProperty);
        public static void SetInvalidPathAlertTitle(IExplorer i, string input) => i.As<DependencyObject>().SetValue(InvalidPathAlertTitleProperty, input);

        #endregion

        #region MoveWarningTitle

        public static readonly DependencyProperty MoveWarningTitleProperty = DependencyProperty.RegisterAttached("MoveWarningTitle", typeof(string), typeof(XExplorer), new FrameworkPropertyMetadata(string.Empty));
        public static string GetMoveWarningTitle(IExplorer i) => (string)i.As<DependencyObject>().GetValue(MoveWarningTitleProperty);
        public static void SetMoveWarningTitle(IExplorer i, string input) => i.As<DependencyObject>().SetValue(MoveWarningTitleProperty, input);

        #endregion

        #region MoveWarningMessage

        public static readonly DependencyProperty MoveWarningMessageProperty = DependencyProperty.RegisterAttached("MoveWarningMessage", typeof(string), typeof(XExplorer), new FrameworkPropertyMetadata(string.Empty));
        public static string GetMoveWarningMessage(IExplorer i) => (string)i.As<DependencyObject>().GetValue(MoveWarningMessageProperty);
        public static void SetMoveWarningMessage(IExplorer i, string input) => i.As<DependencyObject>().SetValue(MoveWarningMessageProperty, input);

        #endregion

        #region Path

        public static readonly DependencyProperty PathProperty = DependencyProperty.RegisterAttached("Path", typeof(string), typeof(XExplorer), new FrameworkPropertyMetadata(string.Empty, OnPathChanged, OnPathCoerced));
        public static string GetPath(IExplorer i) => (string)i.As<DependencyObject>().GetValue(PathProperty);
        public static void SetPath(IExplorer i, string input) => i.As<DependencyObject>().SetValue(PathProperty, input);
        static void OnPathChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is IExplorer i)
            {
                if (i is FrameworkElement j)
                    j.RaiseEvent(new PathChangedEventArgs(XExplorer.PathChangedEvent, (string)e.NewValue));
            }
        }
        static object OnPathCoerced(DependencyObject i, object value)
        {
            if (i is IExplorer j)
            {
                var defaultPath = j.Path.NullOrEmpty() ? (GetDefaultPath(j) ?? Explorer.DefaultPath) : j.Path;

                string input = value?.ToString();
                if (!Folder.Long.Exists(input))
                {
                    if (input != StoragePath.Root)
                    {
                        return defaultPath;
                        if (GetInvalidPathAlert(j))
                            Dialog.Show(GetInvalidPathAlertTitle(j), GetInvalidPathAlertMessage(j)?.F(input), DialogImage.Error, Buttons.Ok);
                    }
                }
                return input;
            }
            throw new NotSupportedException();
        }

        #endregion

        #region PathChanged

        public static readonly RoutedEvent PathChangedEvent = EventManager.RegisterRoutedEvent("PathChanged", RoutingStrategy.Direct, typeof(PathChangedEventHandler), typeof(XExplorer));
        public static void AddPathChangedHandler(DependencyObject i, PathChangedEventHandler handler)
        {
            if (i is UIElement j)
                j.AddHandler(PathChangedEvent, handler);
        }
        public static void AddPathChanged(this IExplorer i, PathChangedEventHandler handler) => AddPathChangedHandler(i as DependencyObject, handler);
        public static void RemovePathChangedHandler(DependencyObject i, PathChangedEventHandler handler)
        {
            if (i is UIElement j)
                j.RemoveHandler(PathChangedEvent, handler);
        }
        public static void RemovePathChanged(this IExplorer i, PathChangedEventHandler handler) => RemovePathChangedHandler(i as DependencyObject, handler);

        #endregion

        #region WarnBeforeDrop

        public static readonly DependencyProperty WarnBeforeDropProperty = DependencyProperty.RegisterAttached("WarnBeforeDrop", typeof(bool), typeof(XExplorer), new FrameworkPropertyMetadata(true));
        public static bool GetWarnBeforeDrop(IExplorer i) => (bool)i.As<DependencyObject>().GetValue(WarnBeforeDropProperty);
        public static void SetWarnBeforeDrop(IExplorer i, bool input) => i.As<DependencyObject>().SetValue(WarnBeforeDropProperty, input);

        #endregion
    }
}