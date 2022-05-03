using Imagin.Common.Collections;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public partial class Navigator : UserControl, IExplorer
    {
        public static readonly ReferenceKey<TreeView> TreeViewKey = new();

        #region Events

        public event EventHandler<EventArgs<string>> FileOpened;

        public event EventHandler<EventArgs<ICollectionChanged>> SelectionChanged;

        #endregion

        #region Properties

        public NavigatorDropHandler DropHandler { get; private set; } = null;

        public static readonly DependencyProperty FavoritesProperty = DependencyProperty.Register(nameof(Favorites), typeof(Favorites), typeof(Navigator), new FrameworkPropertyMetadata(null, OnFavoritesChanged));
        public Favorites Favorites
        {
            get => (Favorites)GetValue(FavoritesProperty);
            set => SetValue(FavoritesProperty, value);
        }
        static void OnFavoritesChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<Navigator>().OnFavoritesChanged(new Value<Favorites>(e));

        public static readonly DependencyProperty FileAttributesProperty = DependencyProperty.Register(nameof(FileAttributes), typeof(Attributes), typeof(Navigator), new FrameworkPropertyMetadata(Attributes.All));
        public Attributes FileAttributes
        {
            get => (Attributes)GetValue(FileAttributesProperty);
            set => SetValue(FileAttributesProperty, value);
        }

        public static readonly DependencyProperty FileExtensionsProperty = DependencyProperty.Register(nameof(FileExtensions), typeof(string), typeof(Navigator), new FrameworkPropertyMetadata(null));
        public string FileExtensions
        {
            get => (string)GetValue(FileExtensionsProperty);
            set => SetValue(FileExtensionsProperty, value);
        }

        public static readonly DependencyProperty FolderAttributesProperty = DependencyProperty.Register(nameof(FolderAttributes), typeof(Attributes), typeof(Navigator), new FrameworkPropertyMetadata(Attributes.All));
        public Attributes FolderAttributes
        {
            get => (Attributes)GetValue(FolderAttributesProperty);
            set => SetValue(FolderAttributesProperty, value);
        }

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(TreeViewModes), typeof(Navigator), new FrameworkPropertyMetadata(TreeViewModes.Default));
        public TreeViewModes Mode
        {
            get => (TreeViewModes)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }
        
        public string Path
        {
            get => XExplorer.GetPath(this);
            set => XExplorer.SetPath(this, value);
        }

        public static readonly DependencyProperty RootProperty = DependencyProperty.Register(nameof(Root), typeof(NavigatorCollection), typeof(Navigator), new FrameworkPropertyMetadata(null));
        public NavigatorCollection Root
        {
            get => (NavigatorCollection)GetValue(RootProperty);
            set => SetValue(RootProperty, value);
        }

        static readonly DependencyPropertyKey SelectionKey = DependencyProperty.RegisterReadOnly(nameof(Selection), typeof(ICollectionChanged), typeof(Navigator), new FrameworkPropertyMetadata(null, OnSelectionChanged));
        public static readonly DependencyProperty SelectionProperty = SelectionKey.DependencyProperty;
        public ICollectionChanged Selection
        {
            get => (ICollectionChanged)GetValue(SelectionProperty);
            private set => SetValue(SelectionKey, value);
        }
        static void OnSelectionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.As<Navigator>().OnSelectionChanged(e.NewValue as ICollectionChanged);

        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(nameof(SelectionMode), typeof(SelectionMode), typeof(Navigator), new FrameworkPropertyMetadata(SelectionMode.Multiple));
        public SelectionMode SelectionMode
        {
            get => (SelectionMode)GetValue(SelectionModeProperty);
            set => SetValue(SelectionModeProperty, value);
        }

        public static readonly DependencyProperty SortDirectionProperty = DependencyProperty.Register(nameof(SortDirection), typeof(ListSortDirection), typeof(Navigator), new FrameworkPropertyMetadata(ListSortDirection.Ascending));
        public ListSortDirection SortDirection
        {
            get => (ListSortDirection)GetValue(SortDirectionProperty);
            set => SetValue(SortDirectionProperty, value);
        }

        public static readonly DependencyProperty SortNameProperty = DependencyProperty.Register(nameof(SortName), typeof(ItemProperty), typeof(Navigator), new FrameworkPropertyMetadata(ItemProperty.Name));
        public ItemProperty SortName
        {
            get => (ItemProperty)GetValue(SortNameProperty);
            set => SetValue(SortNameProperty, value);
        }

        public static readonly DependencyProperty ViewCheckBoxesProperty = DependencyProperty.Register(nameof(ViewCheckBoxes), typeof(bool), typeof(Navigator), new FrameworkPropertyMetadata(false));
        public bool ViewCheckBoxes
        {
            get => (bool)GetValue(ViewCheckBoxesProperty);
            set => SetValue(ViewCheckBoxesProperty, value);
        }

        public static readonly DependencyProperty ViewFilesProperty = DependencyProperty.Register(nameof(ViewFiles), typeof(bool), typeof(Navigator), new FrameworkPropertyMetadata(true));
        public bool ViewFiles
        {
            get => (bool)GetValue(ViewFilesProperty);
            set => SetValue(ViewFilesProperty, value);
        }

        public static readonly DependencyProperty ViewFileExtensionsProperty = DependencyProperty.Register(nameof(ViewFileExtensions), typeof(bool), typeof(Navigator), new FrameworkPropertyMetadata(false));
        public bool ViewFileExtensions
        {
            get => (bool)GetValue(ViewFileExtensionsProperty);
            set => SetValue(ViewFileExtensionsProperty, value);
        }

        #endregion

        #region Navigator

        public Navigator() : base()
        {
            DropHandler = new NavigatorDropHandler(this);
            this.RegisterHandler(i =>
            {
                Selection ??= XTreeView.GetSelectedItems(this.GetChild<TreeView>(TreeViewKey));
                foreach (var j in Root)
                {
                    if (j is FolderGroup folder)
                        folder.Items.Subscribe();
                }
            }, i => 
            {
                foreach (var j in Root)
                {
                    if (j is FolderGroup folder)
                    {
                        folder.Items.Unsubscribe();
                        folder.Items.Clear();
                    }
                }
            });

            SetCurrentValue(RootProperty, NavigatorCollection.Default);
        }

        #endregion

        #region Methods

        void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is Item item)
            {
                if (item is File file)
                {
                    if (file is Shortcut)
                    {
                        if (Shortcut.TargetsFile(file.Path))
                            OnFileOpened(file.Path);
                    }
                    else OnFileOpened(file.Path);
                }
            }
        }

        //...

        protected virtual void OnFavoritesChanged(Value<Favorites> input)
        {
            if (Root is NavigatorCollection collection)
            {
                if (collection.FirstOrDefault(i => i is FavoriteGroup) is FavoriteGroup group)
                    group.Favorites = Favorites;
            }
        }

        protected virtual void OnFileOpened(string filePath) => FileOpened?.Invoke(this, new EventArgs<string>(filePath));

        protected virtual void OnSelectionChanged(ICollectionChanged selection) => SelectionChanged?.Invoke(this, new(selection));

        #endregion

        #region Commands

        ICommand selectCommand;
        public ICommand SelectCommand => selectCommand ??= new RelayCommand<object>(i =>
        {
            if (i is Favorite favorite)
                Path = favorite.Path;

            if (i is Item item)
            {
                if (item is Storage.Container || (item is Shortcut && Shortcut.TargetsFolder(item.Path)))
                    Path = item.Path;
            }
        },
        i => i is Favorite || i is Item);

        #endregion
    }
}