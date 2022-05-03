using Imagin.Common.Analytics;
using Imagin.Common.Collections;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Data;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using Imagin.Common.Threading;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public class Browser : Control, IExplorer
    {
        public static readonly ReferenceKey<ListView> ListViewKey = new();

        #region Events

        public event EventHandler<EventArgs<string>> FileOpened;

        public event DefaultEventHandler<IEnumerable<string>> FilesOpened;
        
        public event EventHandler<EventArgs<string>> FolderOpened;

        public event CollectionChangedEventHandler SelectionChanged;

        #endregion

        #region Properties

        readonly CancelTask lengthTask;

        readonly CancelTask<string[]> selectionTask;

        //...

        #region ColumnVisibility

        public static readonly DependencyProperty ColumnVisibilityProperty = DependencyProperty.Register(nameof(ColumnVisibility), typeof(BooleanList), typeof(Browser), new FrameworkPropertyMetadata(null));
        public BooleanList ColumnVisibility
        {
            get => (BooleanList)GetValue(ColumnVisibilityProperty);
            private set => SetValue(ColumnVisibilityProperty, value);
        }

        #endregion

        #region DragTemplate

        public static readonly DependencyProperty DragTemplateProperty = DependencyProperty.Register(nameof(DragTemplate), typeof(DataTemplate), typeof(Browser), new FrameworkPropertyMetadata(null));
        public DataTemplate DragTemplate
        {
            get => (DataTemplate)GetValue(DragTemplateProperty);
            set => SetValue(DragTemplateProperty, value);
        }

        #endregion

        #region DropHandler

        public BrowserDropHandler DropHandler { get; private set; } = null;

        #endregion

        #region FileAttributes

        public static readonly DependencyProperty FileAttributesProperty = DependencyProperty.Register(nameof(FileAttributes), typeof(Attributes), typeof(Browser), new FrameworkPropertyMetadata(Attributes.All));
        public Attributes FileAttributes
        {
            get => (Attributes)GetValue(FileAttributesProperty);
            set => SetValue(FileAttributesProperty, value);
        }

        #endregion

        #region FileExtensions

        public static readonly DependencyProperty FileExtensionsProperty = DependencyProperty.Register(nameof(FileExtensions), typeof(string), typeof(Browser), new FrameworkPropertyMetadata(string.Empty));
        public string FileExtensions
        {
            get => (string)GetValue(FileExtensionsProperty);
            set => SetValue(FileExtensionsProperty, value);
        }

        #endregion

        #region FileSizeFormat

        public static readonly DependencyProperty FileSizeFormatProperty = DependencyProperty.Register(nameof(FileSizeFormat), typeof(FileSizeFormat), typeof(Browser), new FrameworkPropertyMetadata(FileSizeFormat.BinaryUsingSI));
        public FileSizeFormat FileSizeFormat
        {
            get => (FileSizeFormat)GetValue(FileSizeFormatProperty);
            set => SetValue(FileSizeFormatProperty, value);
        }

        #endregion

        #region FolderAttributes

        public static readonly DependencyProperty FolderAttributesProperty = DependencyProperty.Register(nameof(FolderAttributes), typeof(Attributes), typeof(Browser), new FrameworkPropertyMetadata(Attributes.All));
        public Attributes FolderAttributes
        {
            get => (Attributes)GetValue(FolderAttributesProperty);
            set => SetValue(FolderAttributesProperty, value);
        }

        #endregion

        #region FooterVisibility

        public static readonly DependencyProperty FooterVisibilityProperty = DependencyProperty.Register(nameof(FooterVisibility), typeof(Visibility), typeof(Browser), new FrameworkPropertyMetadata(Visibility.Collapsed));
        public Visibility FooterVisibility
        {
            get => (Visibility)GetValue(FooterVisibilityProperty);
            set => SetValue(FooterVisibilityProperty, value);
        }

        #endregion

        #region GroupDirection

        public static readonly DependencyProperty GroupDirectionProperty = DependencyProperty.Register(nameof(GroupDirection), typeof(ListSortDirection), typeof(Browser), new FrameworkPropertyMetadata(ListSortDirection.Ascending));
        public ListSortDirection GroupDirection
        {
            get => (ListSortDirection)GetValue(GroupDirectionProperty);
            set => SetValue(GroupDirectionProperty, value);
        }

        #endregion

        #region GroupName

        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register(nameof(GroupName), typeof(ItemProperty), typeof(Browser), new FrameworkPropertyMetadata(ItemProperty.None));
        public ItemProperty GroupName
        {
            get => (ItemProperty)GetValue(GroupNameProperty);
            set => SetValue(GroupNameProperty, value);
        }

        #endregion

        #region GroupStyle

        public static readonly DependencyProperty GroupStyleProperty = DependencyProperty.Register(nameof(GroupStyle), typeof(GroupStyle), typeof(Browser), new FrameworkPropertyMetadata(null));
        public GroupStyle GroupStyle
        {
            get => (GroupStyle)GetValue(GroupStyleProperty);
            set => SetValue(GroupStyleProperty, value);
        }

        #endregion

        #region History

        public static readonly DependencyProperty HistoryProperty = DependencyProperty.Register(nameof(History), typeof(History), typeof(Browser), new FrameworkPropertyMetadata(null));
        public History History
        {
            get => (History)GetValue(HistoryProperty);
            set => SetValue(HistoryProperty, value);
        }

        #endregion

        #region IsReadOnly

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(Browser), new FrameworkPropertyMetadata(false));
        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        #endregion

        #region (readonly) Items

        static readonly DependencyPropertyKey ItemsKey = DependencyProperty.RegisterReadOnly(nameof(Items), typeof(Storage.ItemCollection), typeof(Browser), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty ItemsProperty = ItemsKey.DependencyProperty;
        public Storage.ItemCollection Items
        {
            get => (Storage.ItemCollection)GetValue(ItemsProperty);
            private set => SetValue(ItemsKey, value);
        }

        #endregion

        #region ItemSizeIncrement

        public static readonly DependencyProperty ItemSizeIncrementProperty = DependencyProperty.Register(nameof(ItemSizeIncrement), typeof(double), typeof(Browser), new FrameworkPropertyMetadata(4.0));
        public double ItemSizeIncrement
        {
            get => (double)GetValue(ItemSizeIncrementProperty);
            set => SetValue(ItemSizeIncrementProperty, value);
        }

        #endregion

        #region ItemSizeMaximum

        public static readonly DependencyProperty ItemSizeMaximumProperty = DependencyProperty.Register(nameof(ItemSizeMaximum), typeof(double), typeof(Browser), new FrameworkPropertyMetadata(512.0));
        public double ItemSizeMaximum
        {
            get => (double)GetValue(ItemSizeMaximumProperty);
            set => SetValue(ItemSizeMaximumProperty, value);
        }

        #endregion

        #region ItemSizeMinimum

        public static readonly DependencyProperty ItemSizeMinimumProperty = DependencyProperty.Register(nameof(ItemSizeMinimum), typeof(double), typeof(Browser), new FrameworkPropertyMetadata(16.0));
        public double ItemSizeMinimum
        {
            get => (double)GetValue(ItemSizeMinimumProperty);
            set => SetValue(ItemSizeMinimumProperty, value);
        }

        #endregion

        #region Layout

        public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register(nameof(Layout), typeof(BrowserLayout), typeof(Browser), new FrameworkPropertyMetadata(BrowserLayout.Static));
        public BrowserLayout Layout
        {
            get => (BrowserLayout)GetValue(LayoutProperty);
            private set => SetValue(LayoutProperty, value);
        }

        #endregion

        #region (readonly) Length

        static readonly DependencyPropertyKey LengthKey = DependencyProperty.RegisterReadOnly(nameof(Length), typeof(long), typeof(Browser), new FrameworkPropertyMetadata((long)0));
        public static readonly DependencyProperty LengthProperty = LengthKey.DependencyProperty;
        public long Length
        {
            get => (long)GetValue(LengthProperty);
            private set => SetValue(LengthKey, value);
        }

        #endregion

        #region OpenFilesOnClick

        public static readonly DependencyProperty OpenFilesOnClickProperty = DependencyProperty.Register(nameof(OpenFilesOnClick), typeof(bool), typeof(Browser), new FrameworkPropertyMetadata(true));
        /// <summary>
        /// Gets or sets whether or not a <see cref="File"/> can be opened when clicking once or twice.
        /// </summary>
        public bool OpenFilesOnClick
        {
            get => (bool)GetValue(OpenFilesOnClickProperty);
            set => SetValue(OpenFilesOnClickProperty, value);
        }

        #endregion

        #region OpenOnDoubleClick

        public static readonly DependencyProperty OpenOnDoubleClickProperty = DependencyProperty.Register(nameof(OpenOnDoubleClick), typeof(bool), typeof(Browser), new FrameworkPropertyMetadata(true));
        /// <summary>
        /// Gets or sets whether or not an <see cref="Item"/> can be opened when clicked once (if <see cref="false"/>) or twice (if <see cref="true"/>).
        /// </summary>
        public bool OpenOnDoubleClick
        {
            get => (bool)GetValue(OpenOnDoubleClickProperty);
            set => SetValue(OpenOnDoubleClickProperty, value);
        }

        #endregion

        #region XExplorer.Path

        public string Path
        {
            get => XExplorer.GetPath(this);
            set => XExplorer.SetPath(this, value);
        }

        #endregion

        #region Selection

        static readonly DependencyPropertyKey SelectionKey = DependencyProperty.RegisterReadOnly(nameof(Selection), typeof(ICollectionChanged), typeof(Browser), new FrameworkPropertyMetadata(null, OnSelectionChanged));
        public static readonly DependencyProperty SelectionProperty = SelectionKey.DependencyProperty;
        public ICollectionChanged Selection
        {
            get => (ICollectionChanged)GetValue(SelectionProperty);
            private set => SetValue(SelectionKey, value);
        }
        static void OnSelectionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.As<Browser>().OnSelectionChanged(e.NewValue as ICollectionChanged);

        #endregion

        #region (readonly) SelectionCount

        static readonly DependencyPropertyKey SelectionCountKey = DependencyProperty.RegisterReadOnly(nameof(SelectionCount), typeof(int), typeof(Browser), new FrameworkPropertyMetadata(0));
        public static readonly DependencyProperty SelectionCountProperty = SelectionCountKey.DependencyProperty;
        public int SelectionCount
        {
            get => (int)GetValue(SelectionCountProperty);
            private set => SetValue(SelectionCountKey, value);
        }

        #endregion

        #region (readonly) SelectionLength

        static readonly DependencyPropertyKey SelectionLengthKey = DependencyProperty.RegisterReadOnly(nameof(SelectionLength), typeof(long), typeof(Browser), new FrameworkPropertyMetadata((long)0));
        public static readonly DependencyProperty SelectionLengthProperty = SelectionLengthKey.DependencyProperty;
        public long SelectionLength
        {
            get => (long)GetValue(SelectionLengthProperty);
            private set => SetValue(SelectionLengthKey, value);
        }

        #endregion

        #region SelectionMode

        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(nameof(SelectionMode), typeof(System.Windows.Controls.SelectionMode), typeof(Browser), new FrameworkPropertyMetadata(System.Windows.Controls.SelectionMode.Extended));
        public System.Windows.Controls.SelectionMode SelectionMode
        {
            get => (System.Windows.Controls.SelectionMode)GetValue(SelectionModeProperty);
            set => SetValue(SelectionModeProperty, value);
        }

        #endregion

        #region ShortcutArrowTemplate

        public static readonly DependencyProperty ShortcutArrowTemplateProperty = DependencyProperty.Register(nameof(ShortcutArrowTemplate), typeof(DataTemplate), typeof(Browser), new FrameworkPropertyMetadata(null));
        public DataTemplate ShortcutArrowTemplate
        {
            get => (DataTemplate)GetValue(ShortcutArrowTemplateProperty);
            set => SetValue(ShortcutArrowTemplateProperty, value);
        }

        #endregion

        #region ShortcutArrowVisibility

        public static readonly DependencyProperty ShortcutArrowVisibilityProperty = DependencyProperty.Register(nameof(ShortcutArrowVisibility), typeof(Visibility), typeof(Browser), new FrameworkPropertyMetadata(Visibility.Visible));
        public Visibility ShortcutArrowVisibility
        {
            get => (Visibility)GetValue(ShortcutArrowVisibilityProperty);
            set => SetValue(ShortcutArrowVisibilityProperty, value);
        }

        #endregion

        #region SortDirection

        public static readonly DependencyProperty SortDirectionProperty = DependencyProperty.Register(nameof(SortDirection), typeof(ListSortDirection), typeof(Browser), new FrameworkPropertyMetadata(ListSortDirection.Ascending));
        public ListSortDirection SortDirection
        {
            get => (ListSortDirection)GetValue(SortDirectionProperty);
            set => SetValue(SortDirectionProperty, value);
        }

        #endregion

        #region SortName

        public static readonly DependencyProperty SortNameProperty = DependencyProperty.Register(nameof(SortName), typeof(ItemProperty), typeof(Browser), new FrameworkPropertyMetadata(ItemProperty.Name));
        public ItemProperty SortName
        {
            get => (ItemProperty)GetValue(SortNameProperty);
            set => SetValue(SortNameProperty, value);
        }

        #endregion

        #region View

        public static readonly DependencyProperty ViewProperty = DependencyProperty.Register(nameof(View), typeof(BrowserView), typeof(Browser), new FrameworkPropertyMetadata(BrowserView.Thumbnails));
        public BrowserView View
        {
            get => (BrowserView)GetValue(ViewProperty);
            set => SetValue(ViewProperty, value);
        }

        #endregion

        #region ViewCheckBoxes

        public static readonly DependencyProperty ViewCheckBoxesProperty = DependencyProperty.Register(nameof(ViewCheckBoxes), typeof(bool), typeof(Browser), new FrameworkPropertyMetadata(false));
        public bool ViewCheckBoxes
        {
            get => (bool)GetValue(ViewCheckBoxesProperty);
            set => SetValue(ViewCheckBoxesProperty, value);
        }

        #endregion

        #region ViewFiles

        public static readonly DependencyProperty ViewFilesProperty = DependencyProperty.Register(nameof(ViewFiles), typeof(bool), typeof(Browser), new FrameworkPropertyMetadata(true));
        public bool ViewFiles
        {
            get => (bool)GetValue(ViewFilesProperty);
            set => SetValue(ViewFilesProperty, value);
        }

        #endregion

        #region ViewFileExtensions

        public static readonly DependencyProperty ViewFileExtensionsProperty = DependencyProperty.Register(nameof(ViewFileExtensions), typeof(bool), typeof(Browser), new FrameworkPropertyMetadata(false));
        public bool ViewFileExtensions
        {
            get => (bool)GetValue(ViewFileExtensionsProperty);
            set => SetValue(ViewFileExtensionsProperty, value);
        }

        #endregion

        #region ViewSize

        public static readonly DependencyProperty ViewSizeProperty = DependencyProperty.Register(nameof(ViewSize), typeof(double), typeof(Browser), new FrameworkPropertyMetadata(32.0));
        public double ViewSize
        {
            get => (double)GetValue(ViewSizeProperty);
            set => SetValue(ViewSizeProperty, value);
        }

        #endregion

        #endregion

        #region Browser

        public Browser() : base()
        {
            DropHandler 
                = new BrowserDropHandler(this);
            Items
                = new();

            lengthTask 
                = new CancelTask(UpdateLength, true);
            selectionTask 
                = new CancelTask<string[]>(null, UpdateSelectionLength, true);

            this.RegisterHandler(i =>
            {
                Selection ??= XSelector.GetSelectedItems(this.GetChild<ListView>(ListViewKey));
                Selection.CollectionChanged += OnSelected;

                Items.Subscribe();
                Items.Refreshing += OnRefreshing;
                _ = Items.RefreshAsync(Path);

                this.AddPathChanged(OnPathChanged);

                this.GetChild<ListView>(ListViewKey).KeyUp 
                    += OnKeyUp;
                this.GetChild<ListView>(ListViewKey).PreviewMouseLeftButtonDown 
                    += OnPreviewMouseLeftButtonDown;
                this.GetChild<ListView>(ListViewKey).PreviewMouseRightButtonUp
                    += OnPreviewMouseRightButtonUp;
            }, i =>
            {
                Selection.CollectionChanged -= OnSelected;

                Items.Unsubscribe();
                Items.Refreshing -= OnRefreshing;
                Items.Clear();

                this.RemovePathChanged(OnPathChanged);

                this.GetChild<ListView>(ListViewKey).KeyUp 
                    -= OnKeyUp;
                this.GetChild<ListView>(ListViewKey).PreviewMouseLeftButtonDown
                    -= OnPreviewMouseLeftButtonDown;
                this.GetChild<ListView>(ListViewKey).PreviewMouseRightButtonUp
                    -= OnPreviewMouseRightButtonUp;
            });

            SetCurrentValue(HistoryProperty, 
                new History(Explorer.DefaultLimit));
        }

        #endregion

        #region Methods

        void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (FocusManager.GetFocusedElement(this) is not TextBox)
            {
                switch (e.Key)
                {
                    case Key.Back:
                        History?.Undo(i => Path = i);
                        break;

                    case Key.Enter:
                        if (Selection.First<Item>() is Storage.Container container)
                            Open(container);

                        else if (Selection.First<Item>() is Shortcut shortcut && Shortcut.TargetsFolder(shortcut.Path))
                            Open(shortcut);

                        else OnFilesOpened(Selection.Where<Item>(i => i is Storage.File && i is not Shortcut).Select(i => i.Path));
                        break;

                    case Key.Delete:
                        for (var i = Selection.Count - 1; i >= 0; i--)
                        {
                            if (Selection[i] is Item j && j is not Drive)
                                Computer.Recycle(j.Path);
                        }
                        break;
                }
            }
        }

        //...

        void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (e.OriginalSource.FindParent<ListViewItem>() is null)
                    Computer.OpenInWindowsExplorer(Path);
            }
        }

        void OnPreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource?.FindParent<GridViewColumnHeader>() is not null)
                return;

            Try.Invoke(() =>
            {
                var point = PointToScreen(e.GetPosition(this)).Int32();
                if (Selection.Count == 0)
                {
                    ShellContextMenu.Show(point, new DirectoryInfo(Path));
                    return;
                }
                ShellContextMenu.Show(point, Selection.Select<Item, FileSystemInfo>(i => i.Read()).ToArray());
            },
            e => Log.Write<Browser>(e));
        }

        //...

        void OnPathChanged(object sender, PathChangedEventArgs e) 
            => _ = Items.RefreshAsync(e.Path);

        void OnRefreshing(StorageCollection<Item> sender)
            => _ = lengthTask.Start();

        void OnSelected(object sender, NotifyCollectionChangedEventArgs e)
            => _ = selectionTask.StartAsync((sender as ICollectionChanged).Select<Item, string>(i => i.Path).ToArray());

        //...

        void Open(Item item)
        {
            switch (item.Type)
            {
                case ItemType.Drive:
                case ItemType.Folder:
                    if (!IsReadOnly)
                        Path = item.Path;

                    OnFolderOpened(item.Path);
                    break;

                case ItemType.File:
                    OnFileOpened(item.Path);
                    break;

                case ItemType.Shortcut:
                    var targetPath = Shortcut.TargetPath(item.Path);
                    if (Folder.Long.Exists(targetPath))
                    {
                        if (!IsReadOnly)
                            Path = targetPath;

                        OnFolderOpened(targetPath);
                    }
                    else if (Storage.File.Long.Exists(targetPath))
                    {
                        OnFileOpened(targetPath);
                    }
                    else OnFileOpened(item.Path);
                    break;
            }
        }

        //...

        async Task UpdateLength(CancellationToken token)
            => Length = await Folder.Long.TryGetSize(Path, token);

        async Task UpdateSelectionLength(string[] data, CancellationToken token)
        {
            SelectionCount = data.Length;
            SelectionLength = 0;

            foreach (var i in data)
            {
                if (token.IsCancellationRequested)
                    break;

                var type = Computer.GetType(i);
                switch (type)
                {
                    case ItemType.File:
                    case ItemType.Shortcut:
                        Try.Invoke(() =>
                        {
                            var fileInfo = new FileInfo(i);
                            SelectionLength += fileInfo.Length;
                        });
                        break;

                    case ItemType.Folder:
                        SelectionLength += await Folder.Long.TryGetSize(i, token);
                        break;
                }
            }
        }

        //...

        protected virtual void OnFileOpened(string filePath) => FileOpened?.Invoke(this, new EventArgs<string>(filePath));

        protected virtual void OnFilesOpened(IEnumerable<string> filePaths)
        {
            OnFileOpened(filePaths.First());
            FilesOpened?.Invoke(this, new(filePaths));
        }

        protected virtual void OnFolderOpened(string folderPath) => FolderOpened?.Invoke(this, new EventArgs<string>(folderPath));

        protected virtual void OnSelectionChanged(ICollectionChanged selection) => SelectionChanged?.Invoke(this, new(selection));

        //...

        ICommand clickCommand;
        public ICommand ClickCommand => clickCommand ??= new RelayCommand<Item>(i =>
        {
            if (!OpenOnDoubleClick)
            {
                if (i.Type != ItemType.File || OpenFilesOnClick)
                    Open(i);

                else OnFileOpened(i.Path);
            }
        });

        ICommand doubleClickCommand;
        public ICommand DoubleClickCommand => doubleClickCommand ??= new RelayCommand<Item>(i =>
        {
            if (OpenOnDoubleClick)
            {
                if (i.Type != ItemType.File || OpenFilesOnClick)
                    Open(i);

                else OnFileOpened(i.Path);
            }
        });

        //...

        ICommand renameCommand;
        public ICommand RenameCommand => renameCommand ??= new RelayCommand<DoubleReference>(i =>
        {
            if (i.First is TextBox box)
            {
                if (i.Second is Item item)
                {
                    var a = item.Path;
                    var b = $@"{box.Text}";

                    //If file
                    if (item is Storage.File)
                    {
                        //If file extensions aren't visible (only applies to files!)
                        if (!ViewFileExtensions)
                        {
                            //Append file extension to it!
                            b = $"{b}{System.IO.Path.GetExtension(a)}";
                        }
                    }

                    var result = Computer.Move(item, System.IO.Path.GetDirectoryName(item.Path), b);
                    if (result is Error)
                    {
                        box.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                        Dialog.Show("Rename", $"Renaming failed!", DialogImage.Error, Buttons.Ok);
                    }
                }
            }
        });

        ICommand selectAllCommand;
        public ICommand SelectAllCommand => selectAllCommand ??= new RelayCommand(() => this.GetChild(ListViewKey).SelectAll(), () => Items.Count > 0);

        ICommand unselectAllCommand;
        public ICommand UnselectAllCommand => unselectAllCommand ??= new RelayCommand(() => this.GetChild(ListViewKey).ClearSelection(), () => Selection?.Count > 0);
        
        #endregion
    }
}