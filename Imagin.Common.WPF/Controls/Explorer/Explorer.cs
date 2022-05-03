using Imagin.Common.Collections;
using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public class Explorer : Control, IExplorer
    {
        public static readonly ReferenceKey<AddressBox> AddressBoxKey = new();

        public static readonly ReferenceKey<Browser> BrowserKey = new();

        public static readonly ReferenceKey<FavoriteBar> FavoriteBarKey = new();

        public static readonly ReferenceKey<Navigator> NavigatorKey = new();

        //...

        public static readonly Limit DefaultLimit = new(50);

        public static string DefaultPath => StoragePath.Root;

        //...

        public const double HiddenOpacity = 0.6;

        //...

        public event EventHandler<EventArgs<string>> FileOpened;
        
        public event DefaultEventHandler<IEnumerable<string>> FilesOpened;

        public event CollectionChangedEventHandler SelectionChanged;

        //...

        public static readonly DependencyProperty BrowserOptionsProperty = DependencyProperty.Register(nameof(BrowserOptions), typeof(BrowserOptions), typeof(Explorer), new FrameworkPropertyMetadata(null));
        public BrowserOptions BrowserOptions
        {
            get => (BrowserOptions)GetValue(BrowserOptionsProperty);
            set => SetValue(BrowserOptionsProperty, value);
        }
        
        public static readonly DependencyProperty FavoritesProperty = DependencyProperty.Register(nameof(Favorites), typeof(Favorites), typeof(Explorer), new FrameworkPropertyMetadata(null));
        public Favorites Favorites
        {
            get => (Favorites)GetValue(FavoritesProperty);
            set => SetValue(FavoritesProperty, value);
        }

        public static readonly DependencyProperty FavoriteSortDirectionProperty = DependencyProperty.Register(nameof(FavoriteSortDirection), typeof(ListSortDirection), typeof(Explorer), new FrameworkPropertyMetadata(ListSortDirection.Ascending));
        public ListSortDirection FavoriteSortDirection
        {
            get => (ListSortDirection)GetValue(FavoriteSortDirectionProperty);
            set => SetValue(FavoriteSortDirectionProperty, value);
        }

        public static readonly DependencyProperty FavoriteSortNameProperty = DependencyProperty.Register(nameof(FavoriteSortName), typeof(ItemProperty), typeof(Explorer), new FrameworkPropertyMetadata(ItemProperty.Name));
        public ItemProperty FavoriteSortName
        {
            get => (ItemProperty)GetValue(FavoriteSortNameProperty);
            set => SetValue(FavoriteSortNameProperty, value);
        }
        
        public static readonly DependencyProperty FileExtensionsProperty = DependencyProperty.Register(nameof(FileExtensions), typeof(string), typeof(Explorer), new FrameworkPropertyMetadata(null));
        public string FileExtensions
        {
            get => (string)GetValue(FileExtensionsProperty);
            set => SetValue(FileExtensionsProperty, value);
        }

        public static readonly DependencyProperty FileOpenedCommandProperty = DependencyProperty.Register(nameof(FileOpenedCommand), typeof(ICommand), typeof(Explorer), new FrameworkPropertyMetadata(null));
        public ICommand FileOpenedCommand
        {
            get => (ICommand)GetValue(FileOpenedCommandProperty);
            set => SetValue(FileOpenedCommandProperty, value);
        }

        public static readonly DependencyProperty HistoryProperty = DependencyProperty.Register(nameof(History), typeof(History), typeof(Explorer), new FrameworkPropertyMetadata(null));
        public History History
        {
            get => (History)GetValue(HistoryProperty);
            set => SetValue(HistoryProperty, value);
        }

        public static readonly DependencyProperty NavigatorOptionsProperty = DependencyProperty.Register(nameof(NavigatorOptions), typeof(NavigatorOptions), typeof(Explorer), new FrameworkPropertyMetadata(null));
        public NavigatorOptions NavigatorOptions
        {
            get => (NavigatorOptions)GetValue(NavigatorOptionsProperty);
            set => SetValue(NavigatorOptionsProperty, value);
        }

        public static readonly DependencyProperty Panel1LengthProperty = DependencyProperty.Register(nameof(Panel1Length), typeof(GridLength), typeof(Explorer), new FrameworkPropertyMetadata(new GridLength(30, GridUnitType.Star)));
        public GridLength Panel1Length
        {
            get => (GridLength)GetValue(Panel1LengthProperty);
            set => SetValue(Panel1LengthProperty, value);
        }

        public static readonly DependencyProperty Panel2LengthProperty = DependencyProperty.Register(nameof(Panel2Length), typeof(GridLength), typeof(Explorer), new FrameworkPropertyMetadata(new GridLength(70, GridUnitType.Star)));
        public GridLength Panel2Length
        {
            get => (GridLength)GetValue(Panel2LengthProperty);
            set => SetValue(Panel2LengthProperty, value);
        }

        public string Path
        {
            get => XExplorer.GetPath(this);
            set => XExplorer.SetPath(this, value);
        }

        static readonly DependencyPropertyKey SelectionKey = DependencyProperty.RegisterReadOnly(nameof(Selection), typeof(ICollectionChanged), typeof(Explorer), new FrameworkPropertyMetadata(null, OnSelectionChanged));
        public static readonly DependencyProperty SelectionProperty = SelectionKey.DependencyProperty;
        public ICollectionChanged Selection
        {
            get => (ICollectionChanged)GetValue(SelectionProperty);
            private set => SetValue(SelectionKey, value);
        }
        static void OnSelectionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.As<Explorer>().OnSelectionChanged(e.NewValue as ICollectionChanged);

        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(nameof(SelectionMode), typeof(SelectionMode), typeof(Explorer), new FrameworkPropertyMetadata(SelectionMode.Multiple));
        public SelectionMode SelectionMode
        {
            get => (SelectionMode)GetValue(SelectionModeProperty);
            set => SetValue(SelectionModeProperty, value);
        }

        public static readonly DependencyProperty ShowFavoriteBarProperty = DependencyProperty.Register(nameof(ShowFavoriteBar), typeof(bool), typeof(Explorer), new FrameworkPropertyMetadata(true));
        public bool ShowFavoriteBar
        {
            get => (bool)GetValue(ShowFavoriteBarProperty);
            set => SetValue(ShowFavoriteBarProperty, value);
        }

        public static readonly DependencyProperty ViewFilesProperty = DependencyProperty.Register(nameof(ViewFiles), typeof(bool), typeof(Explorer), new FrameworkPropertyMetadata(true));
        public bool ViewFiles
        {
            get => (bool)GetValue(ViewFilesProperty);
            set => SetValue(ViewFilesProperty, value);
        }

        //...

        public Explorer()
        {
            SetCurrentValue(HistoryProperty,
                new History(DefaultLimit));

            this.RegisterHandler(i =>
            {
                this.GetChild(AddressBoxKey).Refreshed
                    += OnRefreshed;

                this.GetChild(BrowserKey).FilesOpened
                    += OnFilesOpened;
                this.GetChild(BrowserKey).SelectionChanged
                    += OnSelectionChanged;

                this.GetChild(FavoriteBarKey).Clicked
                    += OnFavoriteClicked;

                this.GetChild(NavigatorKey).FileOpened
                    += OnFileOpened;
            },
            i =>
            {
                this.GetChild(AddressBoxKey).Refreshed
                    -= OnRefreshed;

                this.GetChild(BrowserKey).FilesOpened
                    -= OnFilesOpened;
                this.GetChild(BrowserKey).SelectionChanged
                    -= OnSelectionChanged;

                this.GetChild(FavoriteBarKey).Clicked
                    -= OnFavoriteClicked;

                this.GetChild(NavigatorKey).FileOpened
                    -= OnFileOpened;
            });
        }

        //...

        void OnFavoriteClicked(object sender, EventArgs<string> e) => Path = e.Value;

        void OnFileOpened(object sender, EventArgs<string> e)
        {
            FileOpened?.Invoke(sender, e);
            FileOpenedCommand?.Execute(e.Value);
        }

        void OnFilesOpened(object sender, EventArgs<IEnumerable<string>> e)
        {
            OnFileOpened(this, new(e.Value.First()));
            FilesOpened?.Invoke(sender, e);
        }

        void OnRefreshed(object sender, RoutedEventArgs e) => _ = this.GetChild(BrowserKey).Items.RefreshAsync();

        void OnSelectionChanged(object sender, EventArgs<ICollectionChanged> e) => Selection ??= e.Value;

        //...

        protected virtual void OnSelectionChanged(ICollectionChanged input) => SelectionChanged?.Invoke(this, new(input));

        //...

        ICommand groupCommand;
        public ICommand GroupCommand => groupCommand ??= new RelayCommand<ItemProperty>(i => BrowserOptions.GroupName = i, i => BrowserOptions != null);

        ICommand groupDirectionCommand;
        public ICommand GroupDirectionCommand => groupDirectionCommand ??= new RelayCommand<ListSortDirection>(i => BrowserOptions.GroupDirection = i.Convert(), i => BrowserOptions != null);

        ICommand refreshCommand;
        public ICommand RefreshCommand => refreshCommand ??= new RelayCommand(() => _ = this.GetChild(BrowserKey).Items.RefreshAsync());

        ICommand sortCommand;
        public ICommand SortCommand => sortCommand ??= new RelayCommand<ItemProperty>(i => BrowserOptions.SortName = i, i => BrowserOptions != null);

        ICommand sortDirectionCommand;
        public ICommand SortDirectionCommand => sortDirectionCommand ??= new RelayCommand<ListSortDirection>(i => BrowserOptions.SortDirection = i.Convert(), i => BrowserOptions != null);
    }
}