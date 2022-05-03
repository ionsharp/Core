using Imagin.Common.Analytics;
using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class AddressBox : ComboBox, IExplorer
    {
        public static readonly ReferenceKey<TextBox> TextBoxKey = new();

        public static readonly ReferenceKey<ToolBar> ToolBarKey = new();

        #region Fields

        readonly Handle handle = false;

        readonly Handle handleFavorite = false;

        //...

        public event RoutedEventHandler Refreshed;

        #endregion

        #region Properties

        new public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(AddressBox), new FrameworkPropertyMetadata(null, null, OnBackgroundCoerced));
        new public Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }
        static object OnBackgroundCoerced(DependencyObject i, object value) => value ?? Brushes.Transparent;

        public static readonly DependencyProperty CrumbsProperty = DependencyProperty.Register(nameof(Crumbs), typeof(StringCollection), typeof(AddressBox), new FrameworkPropertyMetadata(default(StringCollection)));
        public StringCollection Crumbs
        {
            get => (StringCollection)GetValue(CrumbsProperty);
            set => SetValue(CrumbsProperty, value);
        }

        public AddressBoxDropHandler DropHandler { get; private set; } = null;

        public static readonly DependencyProperty FavoritesProperty = DependencyProperty.Register(nameof(Favorites), typeof(Favorites), typeof(AddressBox), new FrameworkPropertyMetadata(null, OnFavoritesChanged));
        public Favorites Favorites
        {
            get => (Favorites)GetValue(FavoritesProperty);
            set => SetValue(FavoritesProperty, value);
        }
        static void OnFavoritesChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<AddressBox>().OnFavoritesChanged(new Value<Favorites>(e));

        public static readonly DependencyProperty HistoryProperty = DependencyProperty.Register(nameof(History), typeof(History), typeof(AddressBox), new FrameworkPropertyMetadata(null));
        public History History
        {
            get => (History)GetValue(HistoryProperty);
            set => SetValue(HistoryProperty, value);
        }

        public static readonly DependencyProperty IsFavoriteProperty = DependencyProperty.Register(nameof(IsFavorite), typeof(bool), typeof(AddressBox), new FrameworkPropertyMetadata(false, OnIsFavoriteChanged));
        public bool IsFavorite
        {
            get => (bool)GetValue(IsFavoriteProperty);
            set => SetValue(IsFavoriteProperty, value);
        }
        static void OnIsFavoriteChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<AddressBox>().OnIsFavoriteChanged(new Value<bool>(e));

        public string Path
        {
            get => XExplorer.GetPath(this);
            set => XExplorer.SetPath(this, value);
        }

        public static readonly DependencyProperty RefreshButtonVisibilityProperty = DependencyProperty.Register(nameof(RefreshButtonVisibility), typeof(Visibility), typeof(AddressBox), new FrameworkPropertyMetadata(Visibility.Visible));
        public Visibility RefreshButtonVisibility
        {
            get => (Visibility)GetValue(RefreshButtonVisibilityProperty);
            set => SetValue(RefreshButtonVisibilityProperty, value);
        }

        #endregion

        #region AddressBox

        public AddressBox() : base()
        {
            DropHandler 
                = new AddressBoxDropHandler(this);

            SetCurrentValue(CrumbsProperty, 
                new StringCollection());
            SetCurrentValue(HistoryProperty, 
                new History(Explorer.DefaultLimit));

            this.RegisterHandler(i =>
            {
                Update();
                this.GetChild<ToolBar>(ToolBarKey).If(j => j is not null, j => j.PreviewMouseDown += OnPreviewMouseDown);
                this.AddPathChanged(OnPathChanged);
            }, i => 
            {
                this.GetChild<ToolBar>(ToolBarKey).If(j => j is not null, j => j.PreviewMouseDown -= OnPreviewMouseDown);
                this.RemovePathChanged(OnPathChanged);
            });
        }

        #endregion

        #region Methods

        void Update()
        {
            Crumbs.Clear();
            Try.Invoke(() =>
            {
                var i = Path;
                while (!i.NullOrEmpty())
                {
                    Crumbs.Insert(0, i);
                    i = System.IO.Path.GetDirectoryName(i);
                }
            });
            UpdateFavorite();
        }

        void UpdateFavorite() => handleFavorite.SafeInvoke(() => SetCurrentValue(IsFavoriteProperty, Favorites?.FirstOrDefault(i => i.Path == Path) != null));

        //...

        void OnPathChanged(object sender, PathChangedEventArgs e)
        {
            Update();
            handle.SafeInvoke(() => History?.Add(e.Path));
        }

        void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource.FindParent<ButtonBase>() is null)
            {
                SetCurrentValue(IsEditableProperty, true);
                this.GetChild<TextBox>(TextBoxKey)?.Focus();
            }
        }

        //...

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            Path = SelectedItem.ToString();
        }

        protected virtual void OnFavoritesChanged(Value<Favorites> input) => UpdateFavorite();

        protected virtual void OnIsFavoriteChanged(Value<bool> input)
            => handleFavorite.SafeInvoke(() => Favorites?.Is(input.New, Path));

        //...

        ICommand backCommand;
        public ICommand BackCommand => backCommand ??= new RelayCommand<object>(i => History.Undo(j => handle.Invoke(() => Path = j)), i => History?.CanUndo() == true);

        ICommand clearHistoryCommand;
        public ICommand ClearHistoryCommand => clearHistoryCommand ??= new RelayCommand<object>(i => History.Clear(), i => History?.Count > 0);

        ICommand enterCommand;
        public ICommand EnterCommand => enterCommand ??= new RelayCommand(() => SetCurrentValue(IsEditableProperty, false));

        ICommand forwardCommand;
        public ICommand ForwardCommand => forwardCommand ??= new RelayCommand<object>(i => History.Redo(j => handle.Invoke(() => Path = j)), i => History?.CanRedo() == true);

        ICommand goCommand;
        public ICommand GoCommand => goCommand ??= new RelayCommand<object>(i => SetCurrentValue(IsEditableProperty, false), i => true);

        ICommand goUpCommand;
        public ICommand GoUpCommand => goUpCommand ??= new RelayCommand(() => Try.Invoke(() => Path = Folder.Long.Parent(Path), e => Log.Write<AddressBox>(e)), () => Path != StoragePath.Root);

        ICommand refreshCommand;
        public ICommand RefreshCommand => refreshCommand ??= new RelayCommand<object>(i => Refreshed?.Invoke(this, new RoutedEventArgs()), i => true);

        ICommand setPathCommand;
        public ICommand SetPathCommand => setPathCommand ??= new RelayCommand<string>(i => Path = i, i => i is not null);

        #endregion
    }
}