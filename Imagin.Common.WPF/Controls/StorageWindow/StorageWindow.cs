using Imagin.Common.Collections;
using Imagin.Common.Configuration;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public abstract class StorageWindow : Window
    {
        #region struct DefaultLabels

        struct DefaultLabels
        {
            public const string OpenButton = "Select";

            public const string OpenTitle = "Open";

            public const string SaveButton = "Save";

            public const string SaveTitle = "Save";
        }

        #endregion

        #region Fields

        public event PropertyChangedEventHandler PropertyChanged;

        //...

        readonly Handle handleClosing = false;

        readonly Handle handleFileNames = false;

        //...

        readonly string FavoritesPath;

        readonly string OptionsPath;

        //...

        public readonly List<string> Paths = new();

        #endregion

        #region Properties

        public static readonly DependencyProperty FileExtensionProperty = DependencyProperty.Register(nameof(FileExtension), typeof(int), typeof(StorageWindow), new FrameworkPropertyMetadata(0, OnFileExtensionChanged));
        public int FileExtension
        {
            get => (int)GetValue(FileExtensionProperty);
            set => SetValue(FileExtensionProperty, value);
        }
        static void OnFileExtensionChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as StorageWindow).OnFileExtensionChanged(new Value<int>(e));

        static readonly DependencyPropertyKey FileExtensionsKey = DependencyProperty.RegisterReadOnly(nameof(FileExtensions), typeof(IList<string>), typeof(StorageWindow), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty FileExtensionsProperty = FileExtensionsKey.DependencyProperty;
        public IList<string> FileExtensions
        {
            get => (IList<string>)GetValue(FileExtensionsProperty);
            private set => SetValue(FileExtensionsKey, value);
        }

        static readonly DependencyPropertyKey FileExtensionGroupsKey = DependencyProperty.RegisterReadOnly(nameof(FileExtensionGroups), typeof(FileExtensionGroups), typeof(StorageWindow), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty FileExtensionGroupsProperty = FileExtensionGroupsKey.DependencyProperty;
        public FileExtensionGroups FileExtensionGroups
        {
            get => (FileExtensionGroups)GetValue(FileExtensionGroupsProperty);
            private set => SetValue(FileExtensionGroupsKey, value);
        }

        public static readonly DependencyProperty FileNamesProperty = DependencyProperty.Register(nameof(FileNames), typeof(string), typeof(StorageWindow), new FrameworkPropertyMetadata(string.Empty, OnFileNamesChanged));
        public string FileNames
        {
            get => (string)GetValue(FileNamesProperty);
            set => SetValue(FileNamesProperty, value);
        }
        static void OnFileNamesChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as StorageWindow).OnFileNamesChanged(new Value<string>(e));

        static readonly DependencyPropertyKey FilterModeKey = DependencyProperty.RegisterReadOnly(nameof(FilterMode), typeof(StorageWindowFilterModes), typeof(StorageWindow), new FrameworkPropertyMetadata(StorageWindowFilterModes.Single));
        public static readonly DependencyProperty FilterModeProperty = FilterModeKey.DependencyProperty;
        public StorageWindowFilterModes FilterMode
        {
            get => (StorageWindowFilterModes)GetValue(FilterModeProperty);
            private set => SetValue(FilterModeKey, value);
        }

        static readonly DependencyPropertyKey ModeKey = DependencyProperty.RegisterReadOnly(nameof(Mode), typeof(StorageWindowModes), typeof(StorageWindow), new FrameworkPropertyMetadata(StorageWindowModes.Open));
        public static readonly DependencyProperty ModeProperty = ModeKey.DependencyProperty;
        public StorageWindowModes Mode
        {
            get => (StorageWindowModes)GetValue(ModeProperty);
            private set => SetValue(ModeKey, value);
        }

        static readonly DependencyPropertyKey OpenLabelKey = DependencyProperty.RegisterReadOnly(nameof(OpenLabel), typeof(string), typeof(StorageWindow), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty OpenLabelProperty = OpenLabelKey.DependencyProperty;
        public string OpenLabel
        {
            get => (string)GetValue(OpenLabelProperty);
            private set => SetValue(OpenLabelKey, value);
        }

        public static readonly DependencyProperty OptionsProperty = DependencyProperty.Register(nameof(Options), typeof(ExplorerWindowOptions), typeof(StorageWindow), new FrameworkPropertyMetadata(null));
        public ExplorerWindowOptions Options
        {
            get => (ExplorerWindowOptions)GetValue(OptionsProperty);
            set => SetValue(OptionsProperty, value);
        }

        public static readonly DependencyProperty OverwriteProperty = DependencyProperty.Register(nameof(Overwrite), typeof(bool), typeof(StorageWindow), new FrameworkPropertyMetadata(true));
        public bool Overwrite
        {
            get => (bool)GetValue(OverwriteProperty);
            set => SetValue(OverwriteProperty, value);
        }
        
        public static readonly DependencyProperty OverwriteMessageProperty = DependencyProperty.Register(nameof(OverwriteMessage), typeof(string), typeof(StorageWindow), new FrameworkPropertyMetadata(null));
        public string OverwriteMessage
        {
            get => (string)GetValue(OverwriteMessageProperty);
            set => SetValue(OverwriteMessageProperty, value);
        }

        public static readonly DependencyProperty OverwriteTitleProperty = DependencyProperty.Register(nameof(OverwriteTitle), typeof(string), typeof(StorageWindow), new FrameworkPropertyMetadata(null));
        public string OverwriteTitle
        {
            get => (string)GetValue(OverwriteTitleProperty);
            set => SetValue(OverwriteTitleProperty, value);
        }

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(string), typeof(StorageWindow), new FrameworkPropertyMetadata(null));
        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }

        static readonly DependencyPropertyKey SelectedFileExtensionsKey = DependencyProperty.RegisterReadOnly(nameof(SelectedFileExtensions), typeof(string), typeof(StorageWindow), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty SelectedFileExtensionsProperty = SelectedFileExtensionsKey.DependencyProperty;
        public string SelectedFileExtensions
        {
            get => (string)GetValue(SelectedFileExtensionsProperty);
            private set => SetValue(SelectedFileExtensionsKey, value);
        }

        static readonly DependencyPropertyKey SelectionKey = DependencyProperty.RegisterReadOnly(nameof(Selection), typeof(ICollectionChanged), typeof(StorageWindow), new FrameworkPropertyMetadata(null, OnSelectionChanged));
        public static readonly DependencyProperty SelectionProperty = SelectionKey.DependencyProperty;
        public ICollectionChanged Selection
        {
            get => (ICollectionChanged)GetValue(SelectionProperty);
            protected set => SetValue(SelectionKey, value);
        }
        static void OnSelectionChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as StorageWindow).OnSelectionChanged(new Value<ICollectionChanged>(e));

        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(nameof(SelectionMode), typeof(SelectionMode), typeof(StorageWindow), new FrameworkPropertyMetadata(SelectionMode.Single, null, OnSelectionModeCoerced));
        public SelectionMode SelectionMode
        {
            get => (SelectionMode)GetValue(SelectionModeProperty);
            set => SetValue(SelectionModeProperty, value);
        }
        static object OnSelectionModeCoerced(DependencyObject i, object value)
        {
            return (i as StorageWindow).Mode switch
            {
                StorageWindowModes.Open or StorageWindowModes.OpenFile => value,
                StorageWindowModes.OpenFolder or StorageWindowModes.SaveFile => SelectionMode.Single,
                _ => throw new NotSupportedException(),
            };
        }

        static readonly DependencyPropertyKey TitleLabelKey = DependencyProperty.RegisterReadOnly(nameof(TitleLabel), typeof(string), typeof(StorageWindow), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty TitleLabelProperty = TitleLabelKey.DependencyProperty;
        public string TitleLabel
        {
            get => (string)GetValue(TitleLabelProperty);
            private set => SetValue(TitleLabelKey, value);
        }

        public static readonly DependencyProperty ViewFilesProperty = DependencyProperty.Register(nameof(ViewFiles), typeof(bool), typeof(StorageWindow), new FrameworkPropertyMetadata(false));
        public bool ViewFiles
        {
            get => (bool)GetValue(ViewFilesProperty);
            set => SetValue(ViewFilesProperty, value);
        }

        public static readonly DependencyProperty ViewOptionsProperty = DependencyProperty.Register(nameof(ViewOptions), typeof(bool), typeof(StorageWindow), new FrameworkPropertyMetadata(false));
        public bool ViewOptions
        {
            get => (bool)GetValue(ViewOptionsProperty);
            set => SetValue(ViewOptionsProperty, value);
        }

        #endregion

        #region StorageWindow

        protected StorageWindow(string title, StorageWindowModes mode, SelectionMode selectionMode, StorageWindowFilterModes filterMode, IEnumerable<string> fileExtensions, string defaultPath) : base()
        {
            OptionsPath 
                = $@"{ApplicationProperties.GetFolderPath(DataFolders.Documents)}\{nameof(StorageWindow)}\Options.data";
            ExplorerWindowOptions.Load(OptionsPath, out ExplorerWindowOptions options);
            SetCurrentValue(OptionsProperty, options);

            //...

            FavoritesPath
                = $@"{ApplicationProperties.GetFolderPath(DataFolders.Documents)}\{nameof(Explorer)}";
            Options.ExplorerOptions.Favorites 
                = new Favorites(FavoritesPath, new Limit(250, Limit.Actions.RemoveFirst));
            Options.ExplorerOptions.Favorites.Load();

            //...

            FileExtensionGroups
                = new FileExtensionGroups();
            FileExtensions
                = mode != StorageWindowModes.OpenFolder ? fileExtensions?.ToList() : null;
            FilterMode
                = filterMode;
            Mode
                = mode;

            switch (mode)
            {
                case StorageWindowModes.Open:
                case StorageWindowModes.OpenFile:
                    SetCurrentValue(ViewFilesProperty, true);
                    OpenLabel = DefaultLabels.OpenButton;
                    break;

                case StorageWindowModes.SaveFile:
                    SetCurrentValue(ViewFilesProperty, true);
                    OpenLabel = DefaultLabels.SaveButton;
                    break;

                case StorageWindowModes.OpenFolder:
                    SetCurrentValue(ViewFilesProperty, false);
                    OpenLabel = DefaultLabels.OpenButton;
                    break;
            }
            SetCurrentValue(SelectionModeProperty, SelectionMode);

            SetCurrentValue(PathProperty,
                File.Long.Exists(defaultPath)
                ? System.IO.Path.GetDirectoryName(defaultPath)
                : defaultPath);
            SetCurrentValue(SelectionModeProperty,
                selectionMode);

            //...

            TitleLabel
                = GetTitle(title, mode);

            //...

            UpdateFileExtensionGroups();

            this.RegisterHandler(null, i =>
            {
                Selection.If(j => j is not null, j => j.CollectionChanged -= OnSelectionChanged);

                Options.ExplorerOptions.Favorites.Save();
                Options.Save();

                handleClosing.SafeInvoke(() => Paths.Clear());
            });
        }

        #endregion

        #region Methods

        void OnFileOpened(object sender, EventArgs<string> e) => Close();

        void OnFilesOpened(object sender, EventArgs<IEnumerable<string>> e) => Close();

        void OnSelectionChanged(object sender, NotifyCollectionChangedEventArgs e) => Update();

        //...

        string GetActualFilePath(string filePath)
        {
            var lastExtension = StoragePath.GetLastExtension(filePath);

            if (FileExtensions == null || FileExtensions.Count == 0 || FileExtensionGroups[FileExtension].FileExtensions.Contains(FileExtensionGroup.Wild))
                return filePath;

            var fileExtensions = FileExtensionGroups[FileExtension].FileExtensions;

            //If only one extension is specified, allow it to be absent
            if (fileExtensions.Count == 1)
            {
                //The first extension found is that extension
                if (lastExtension == fileExtensions[0])
                    return filePath;

                //The desired extension must be added
                return $"{filePath}.{fileExtensions[0]}";
            }

            //If more than one extension is specified, the first extension found must be present
            return fileExtensions.Contains(lastExtension) ? filePath : null;
        }

        string GetFilePath(string fileName) => $@"{Path.TrimEnd('\\')}\{fileName}";

        string GetTitle(string title, StorageWindowModes mode)
        {
            return mode switch
            {
                StorageWindowModes.Open or StorageWindowModes.OpenFile or StorageWindowModes.OpenFolder => title.NullOrEmpty() ? DefaultLabels.OpenTitle : title,
                StorageWindowModes.SaveFile => title.NullOrEmpty() ? DefaultLabels.SaveTitle : title,
                _ => default,
            };
        }

        //...

        void Open()
        {
            if (Mode == StorageWindowModes.SaveFile)
            {
                if (Overwrite)
                {
                    var filePath = Paths.First();
                    if (File.Long.Exists(filePath))
                    {
                        var result = Dialog.Show(OverwriteTitle, OverwriteMessage.F(filePath), DialogImage.Warning, Buttons.YesNo);
                        if (result == 1)
                            return;
                    }
                }
            }
            handleClosing.Invoke(() => Close());
        }

        //...

        void UpdateMultiple()
        {
            var result = string.Empty;
            foreach (Item i in Selection)
            {
                switch (Mode)
                {
                    case StorageWindowModes.Open:
                        goto default;

                    case StorageWindowModes.OpenFile:
                        if (i is File)
                            goto default;

                        break;

                    case StorageWindowModes.SaveFile:
                        break;

                    case StorageWindowModes.OpenFolder:
                        if (i is Storage.Container)
                            goto default;

                        if (i is Shortcut)
                        {
                            if (Shortcut.TargetsFolder(i.Path))
                                goto default;
                        }

                        break;

                    default:
                        result += $"\"{Computer.FriendlyName(i.Path)}\" ";
                        Paths.Add(i.Path);
                        break;
                }
            }
            SetCurrentValue(FileNamesProperty, result);
        }

        void UpdateOne()
        {
            switch (Mode)
            {
                case StorageWindowModes.Open:
                    goto default;

                case StorageWindowModes.OpenFile:
                    if (Selection[0] is File)
                        goto default;

                    break;

                case StorageWindowModes.SaveFile:
                    if (Selection[0] is File)
                    {
                        SetCurrentValue(FileNamesProperty, $"{Computer.FriendlyName(Selection.First<Item>().Path)}");
                        if (GetActualFilePath(Selection.First<Item>().Path) is string i)
                            Paths.Add(i);

                        break;
                    }

                    if (GetActualFilePath(GetFilePath(FileNames)) is string j)
                        Paths.Add(j);
                    break;

                case StorageWindowModes.OpenFolder:
                    if (Selection[0] is Storage.Container)
                        goto default;

                    if (Selection[0] is Shortcut)
                    {
                        if (Shortcut.TargetsFolder(Selection.First<Item>().Path))
                            goto default;
                    }

                    break;

                default:
                    SetCurrentValue(FileNamesProperty, $"{Computer.FriendlyName(Selection.First<Item>().Path)}");
                    Paths.Add(Selection.First<Item>().Path);
                    break;
            }
        }

        void UpdateZero()
        {
            switch (Mode)
            {
                case StorageWindowModes.Open:
                case StorageWindowModes.OpenFolder:
                    SetCurrentValue(FileNamesProperty, $"<{Computer.FriendlyName(Path)}>");
                    Paths.Add(Path);
                    break;

                case StorageWindowModes.SaveFile:
                    if (GetActualFilePath(GetFilePath(FileNames)) is string i)
                        Paths.Add(i);

                    break;

                default:
                    SetCurrentValue(FileNamesProperty, string.Empty);
                    break;
            }
        }

        //...

        void Update()
        {
            handleFileNames.Invoke(() =>
            {
                Paths.Clear();
                if (Selection == null || Selection.Count == 0)
                    UpdateZero();

                else if (SelectionMode == SelectionMode.Single || Selection.Count == 1)
                    UpdateOne();

                else if (SelectionMode == SelectionMode.Multiple && Selection.Count > 1)
                    UpdateMultiple();
            });
        }

        void UpdateFileExtensionGroups()
        {
            FileExtensionGroups.Clear();
            if (Mode != StorageWindowModes.OpenFolder)
            {
                if (FileExtensions != null)
                {
                    switch (FilterMode)
                    {
                        case StorageWindowFilterModes.Alphabetical:
                            var firstResult = new Dictionary<char, List<string>>();
                            foreach (var i in FileExtensions)
                            {
                                if (char.IsLetterOrDigit(i[0]))
                                {
                                    var key = char.IsLetter(i[0]) ? i[0] : '0';
                                    if (!firstResult.ContainsKey(key))
                                        firstResult.Add(key, new List<string>());

                                    if (!firstResult[key].Contains(i))
                                        firstResult[key].Add(i);
                                }
                            }

                            List<string[]> secondResult = new();
                            foreach (var i in firstResult)
                                secondResult.Add(i.Value.ToArray());

                            var thirdResult = secondResult.OrderBy(i => i.First());
                            foreach (var i in thirdResult)
                                FileExtensionGroups.Add(i);
                            
                            break;

                        case StorageWindowFilterModes.Single:
                            var lastResult = FileExtensions.OrderBy(i => i);
                            foreach (var i in lastResult)
                                FileExtensionGroups.Add(i);

                            break;
                    }
                }

                FileExtensionGroups.Add(FileExtensionGroup.Wild);
                OnFileExtensionChanged(new Value<int>(0));
            }
        }

        //...

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Selection.If(i => i.CollectionChanged -= OnSelectionChanged);
        }

        protected virtual void OnFileExtensionChanged(Value<int> input)
            => SelectedFileExtensions = FileExtensionGroups[input.New].FileExtensions.Contains(FileExtensionGroup.Wild)
            ? null
            : FileExtensionGroups[input.New].FileExtensions.ToString<string>(";");

        protected virtual void OnFileNamesChanged(Value<string> input)
        {
            handleFileNames.SafeInvoke(() =>
            {
                if (Mode == StorageWindowModes.SaveFile)
                {
                    Paths.Clear();
                    if (GetActualFilePath(GetFilePath(input.New)) is string actualPath)
                        Paths.Add(actualPath);
                }
            });
        }

        protected virtual void OnPathChanged(Value<string> input) => Update();

        protected virtual void OnSelectionChanged(Value<ICollectionChanged> input)
        {
            if (input.Old is ICollectionChanged oldValue)
                oldValue.CollectionChanged -= OnSelectionChanged;

            if (input.New is ICollectionChanged newValue)
            {
                newValue.CollectionChanged -= OnSelectionChanged;
                newValue.CollectionChanged += OnSelectionChanged;
            }
        }

        //...

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        //...

        ICommand openCommand;
        public ICommand OpenCommand => openCommand ??= new RelayCommand(Open, () => Paths.Count > 0);

        //...

        static StorageWindow New<T>(string title, StorageWindowModes mode, SelectionMode selectionMode, StorageWindowFilterModes filterMode, IEnumerable<string> fileExtensions, string defaultPath) where T : StorageWindow
            => typeof(T).Create<StorageWindow>(title, mode, selectionMode, filterMode, fileExtensions, defaultPath);

        //...

        public static bool Show
        #region <parameters>
        (
            out string[] paths,
            string title
                = "",
            StorageWindowModes mode
                = StorageWindowModes.OpenFile,
            IEnumerable<string> fileExtensions
                = null,
            string defaultPath
                = "",
            StorageWindowFilterModes filterMode
                = StorageWindowFilterModes.Single,
            StorageWindowTypes storageWindowType
                = StorageWindowTypes.Explorer
        )
        #endregion
        #region <body>
        {
            if (mode == StorageWindowModes.SaveFile)
                throw new NotSupportedException();

            paths = new string[0];
            StorageWindow result = null;

            if (storageWindowType == StorageWindowTypes.Explorer)
                result = New<ExplorerWindow>(title, mode, SelectionMode.Multiple, filterMode, fileExtensions, defaultPath);

            if (storageWindowType == StorageWindowTypes.Navigator)
                result = New<NavigatorWindow>(title, mode, SelectionMode.Multiple, filterMode, fileExtensions, defaultPath);

            result.ShowDialog();
            if (result.Paths.Count > 0)
            {
                paths = result.Paths.ToArray();
                return true;
            }
            return false;
        }
        #endregion

        public static bool Show
        #region <parameters>
        (
            out string path,
            string title
                = "",
            StorageWindowModes mode
                = StorageWindowModes.OpenFile,
            IEnumerable<string> fileExtensions
                = null,
            string defaultPath
                = "",
            StorageWindowFilterModes filterMode
                = StorageWindowFilterModes.Single,
            StorageWindowTypes storageWindowType
                = StorageWindowTypes.Explorer
        )
        #endregion
        #region <body>
        {
            path = string.Empty;
            StorageWindow result = null;

            if (storageWindowType == StorageWindowTypes.Explorer)
                result = New<ExplorerWindow>(title, mode, SelectionMode.Single, filterMode, fileExtensions, defaultPath);

            if (storageWindowType == StorageWindowTypes.Navigator)
                result = New<NavigatorWindow>(title, mode, SelectionMode.Single, filterMode, fileExtensions, defaultPath);

            result.ShowDialog();
            if (result.Paths.Count > 0)
            {
                path = result.Paths.First();
                return true;
            }
            return false;
        }
        #endregion

        #endregion
    }
}