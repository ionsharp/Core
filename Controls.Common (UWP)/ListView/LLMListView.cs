using System;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using XP;

namespace Imagin.Controls.Common
{
    public sealed class LLMListView : ListView
    {
        private const int Refresh_Notify_Interval = 300;
        private const int Refresh_Status_Interval = 100;
        private const string Refreshing_State = "Refreshing";
        private const string Normal_State = "Normal";
        private const string Unvalid_State = "Unvalid";
        private const string FloatBtn_Refreshing_State = "FloatBtnWorking";
        private const string FloatBtn_Normal_State = "FloatBtnNormal";
        private const string FloatBtn_Visible_State = "FloatBtnVisible";
        private const string FloatBtn_Collapse_State = "FloatBtnCollapse";
        private const string FloatBtn_Shadow_Base64 = "iVBORw0KGgoAAAANSUhEUgAAAF4AAABeCAMAAACdDFNcAAAAnFBMVEUAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAQEAAAAAAAAAAABLnuFaiK1sos4AAAB3l698rdRqrOA4kdc2l+RcfphWa3xejbROm9g7lt4ykNxllr1Ij8eIqsVSksVGks5+lKdPX2thfZM2P0RecoJBRkwdICNicoFMV2BZZG0tV3O/AAAANHRSTlMAAgUICg0SGzEhFg8qDBQYJx8tIzUkETIiOiMgHA4DNzctGAoGKCEeJRwaMzEzLREtIycXLy7ODAAAA/NJREFUaN60k91qg0AQhU+ru4KyK7lZZLVexATBmxD6/u/W0bWMaTH+jd9dCHxz9syItXy8ABnY/PmC1IzRHEVRPIF+8oiD7t5surJyDgPOVWVnaMY44oiccpq2qvGPumoN/bl3QAhObodZXGvGJ+yUV1igMuEFm+0kL7GCkgfIynlAzAPW2eMW66m79f4QvcEmXHjAOnv8wFbqB/sXijEtdtCZUNCCPZ7d6fKG2T9rV1fspFLv/YO9wW4c+ef1nP1Y/jf2AocoZv3DzXgcgO9nxv6NwzwU+1/0kdI4Tq1NxPpp8dkXBHAZ1z+tRnmI4DOqR7oa5qYV18PVFBDC/73OvhoNMTi+YHimGLfL4TMLQWzYLofXHF44Puk5vBB20n4f3kMUryn+5OYdRLny8dBidQphUk3LHfXGegjj7W871E1eQJgipXZO6wZNmlH5493kECen2wl6lT4hztMa0ofq7xDnnlP54ZNNihP0CZUfqk8cxLkm/eUH/Q3iuIuOg94mkKe52KA3p+jrxJpBr9Iz9EjSk/XqRH19rr75adbsdtSGgSh8s8QzjuP4JyHiYku2CxFoYbXq+z9cx3Yi09IFQuaivkICfTOcOePYsTHj3/nx64zHNT/+iLIajYkHfvwedTW2Fb7x49+mtnop7Cc//pctJrzw/HiMU1qakJF9TntFmpCnx0nLXtt9mx4nyZknbvwnpodhqm3LjW9TZdNCROBPXvoQpSf8KP6JWRs/LaNS37bc2uRFYFRnz9qyo+unBbj0hhNvUOb9Q2pcxnmn8yL6Jm9+6lax0XfG1zH5vHXT1ndc+JOKyRN+Uiekb3ZM0w0lP208885TKCbvf+Tkc/rBPCyte8y2+TN9o3YM0ijzd/LJ+xrVeTn+S9m8Z874UF1v+sXCX9b1Wp6F7txeSnMlD/EPi+YaousqSHOFj+6xyi2wz1EF4b9/kVlKVO7wdO5OoSwz/Vr+siZ+96TuRK/L2y+Ri8Dvn2vWTL/NV1/vs7vJ3aMTPuojrQE4ziwqgLEy0Al/jy8MuP51RuofzpEjM/0WP/jfK4Dtj8fguy2A8oIceZue/U8FMNBA95BhoHHG1kU14+Cq1AIVOLj7AO7AgcIgzCP0LFAhRRsC9MP3vzz0Ae6FLFazDw1JIUsBGmj67h9lXnc9fQWqJV1WLxN9ToCyqIU3DpoNNOdtN4z/Yxi67bmBTQPOoKiLMsPn8GMAKbA1CijEhoLEAfSxAVDGWyHnw3OAKJGuhQ0hnINxOKdMi8TWUZZAf/4oniKEEBQD0dNAJLKodVESOx3FL71IUK1KiqFlHFoXhK7yRQKeaxAUJI7q8hrE/32JY+EVlN/KRTd0EkKQDQAAAABJRU5ErkJggg==";

        private bool _isNotifyToRefreshTimerStarting = false;
        private bool _isRefreshing = false;
        private bool _isLoadingMore = false;
        private double _lastVerticalOffset = 0;

        private ScrollViewer _scrollViewer;
        private Grid _container;
        private Border _pullToRefreshIndicator;
        private DispatcherTimer _timer;
        private DispatcherTimer _notifyToRefreshTimer;
        private ProgressBar _pullProgressBar;
        private ProgressRing _refreshProgressRing;
        private ProgressBar _loadMoreProgressBar;
        private XPButton _floatButton;
        private Image _floatButtonShadow;
        private ItemsPresenter _itemsPresenter;
        private ContentControl _emptyTemplateControl;

        public Action Refresh { get; set; }

        public Action LoadMore { get; set; }

        public Action FloatButtonAction { get; set; }

        public bool CanPullToRefresh
        {
            get { return (bool)GetValue(CanPullToRefreshProperty); }
            set { SetValue(CanPullToRefreshProperty, value); }
        }
        public static readonly DependencyProperty CanPullToRefreshProperty =
            DependencyProperty.Register("CanPullToRefresh", typeof(bool), typeof(LLMListView), new PropertyMetadata(false));

        public double RefreshAreaHeight
        {
            get { return (double)GetValue(RefreshAreaHeightProperty); }
            set { SetValue(RefreshAreaHeightProperty, value); }
        }
        public static readonly DependencyProperty RefreshAreaHeightProperty =
            DependencyProperty.Register("RefreshAreaHeight", typeof(double), typeof(LLMListView), new PropertyMetadata(50.0));

        public Brush RefreshProgressRingBrush
        {
            get { return (Brush)GetValue(RefreshProgressRingBrushProperty); }
            set { SetValue(RefreshProgressRingBrushProperty, value); }
        }
        public static readonly DependencyProperty RefreshProgressRingBrushProperty =
            DependencyProperty.Register("RefreshProgressRingBrush", typeof(Brush), typeof(LLMListView), new PropertyMetadata(Application.Current.Resources["ProgressBarForegroundThemeBrush"]));

        public Brush RefreshProgressBarBrush
        {
            get { return (Brush)GetValue(RefreshProgressBarBrushProperty); }
            set { SetValue(RefreshProgressBarBrushProperty, value); }
        }
        public static readonly DependencyProperty RefreshProgressBarBrushProperty =
            DependencyProperty.Register("RefreshProgressBarBrush", typeof(Brush), typeof(LLMListView), new PropertyMetadata(Application.Current.Resources["ProgressBarForegroundThemeBrush"]));

        public Brush LoadMoreProgressBarBrush
        {
            get { return (Brush)GetValue(LoadMoreProgressBarBrushProperty); }
            set { SetValue(LoadMoreProgressBarBrushProperty, value); }
        }
        public static readonly DependencyProperty LoadMoreProgressBarBrushProperty =
            DependencyProperty.Register("LoadMoreProgressBarBrush", typeof(Brush), typeof(LLMListView), new PropertyMetadata(Application.Current.Resources["ProgressBarForegroundThemeBrush"]));

        public Brush FloatButtonForeground
        {
            get { return (Brush)GetValue(FloatButtonForegroundProperty); }
            set { SetValue(FloatButtonForegroundProperty, value); }
        }
        public static readonly DependencyProperty FloatButtonForegroundProperty =
            DependencyProperty.Register("FloatButtonForeground", typeof(Brush), typeof(LLMListView), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        public Brush FloatButtonBackground
        {
            get { return (Brush)GetValue(FloatButtonBackgroundProperty); }
            set { SetValue(FloatButtonBackgroundProperty, value); }
        }
        public static readonly DependencyProperty FloatButtonBackgroundProperty =
            DependencyProperty.Register("FloatButtonBackground", typeof(Brush), typeof(LLMListView), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 33, 150, 243))));

        public Visibility FloatButtonVisibility
        {
            get { return (Visibility)GetValue(FloatButtonVisibilityProperty); }
            set { SetValue(FloatButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty FloatButtonVisibilityProperty =
            DependencyProperty.Register("FloatButtonVisibility", typeof(Visibility), typeof(LLMListView), new PropertyMetadata(Visibility.Collapsed));

        public IconElement FloatButtonIcon
        {
            get { return (IconElement)GetValue(FloatButtonIconProperty); }
            set { SetValue(FloatButtonIconProperty, value); }
        }
        public static readonly DependencyProperty FloatButtonIconProperty =
            DependencyProperty.Register("FloatButtonIcon", typeof(IconElement), typeof(LLMListView), new PropertyMetadata(null));

        public DataTemplate EmptyDataTemplate
        {
            get { return (DataTemplate) GetValue(EmptyDataTemplateProperty); }
            set { SetValue(EmptyDataTemplateProperty, value); }
        }
        public static readonly DependencyProperty EmptyDataTemplateProperty = DependencyProperty.Register(
            "EmptyDataTemplate", typeof(DataTemplate), typeof(LLMListView), new PropertyMetadata(default(DataTemplate)));

        public int ScrollOffsetOfLoadMoreTrigger
        {
            get { return (int)GetValue(ScrollOffsetOfLoadMoreTriggerProperty); }
            set { SetValue(ScrollOffsetOfLoadMoreTriggerProperty, value); }
        }
        public static readonly DependencyProperty ScrollOffsetOfLoadMoreTriggerProperty =
            DependencyProperty.Register("ScrollOffsetOfLoadMoreTrigger", typeof(int), typeof(LLMListView), new PropertyMetadata(300, (s, e)=> { if ((s as LLMListView).ScrollOffsetOfLoadMoreTrigger <= 0) throw new ArgumentException("invalid ScrollOffsetOfLoadMoreTrigger"); }));


        #region list view item property

        public event SwipeBeginEventHandler ItemSwipeBeginInTouch;
        public event SwipeProgressEventHandler ItemSwipeProgressInTouch;
        public event SwipeCompleteEventHandler ItemSwipeRestoreComplete;
        public event SwipeCompleteEventHandler ItemSwipeTriggerComplete;
        public event SwipeReleaseEventHandler ItemSwipeBeginTrigger;
        public event SwipeReleaseEventHandler ItemSwipeBeginRestore;
        public event SwipeTriggerEventHandler ItemSwipeTriggerInTouch;

        public bool IsItemSwipeEnabled
        {
            get { return (bool)GetValue(IsItemSwipeEnabledProperty); }
            set { SetValue(IsItemSwipeEnabledProperty, value); }
        }
        public static readonly DependencyProperty IsItemSwipeEnabledProperty =
            DependencyProperty.Register("IsItemSwipeEnabled", typeof(bool), typeof(LLMListView), new PropertyMetadata(true));

        public DataTemplate ItemLeftSwipeContentTemplate
        {
            get { return (DataTemplate)GetValue(ItemLeftSwipeContentTemplateProperty); }
            set { SetValue(ItemLeftSwipeContentTemplateProperty, value); }
        }
        public static readonly DependencyProperty ItemLeftSwipeContentTemplateProperty =
            DependencyProperty.Register("ItemLeftSwipeContentTemplate", typeof(DataTemplate), typeof(LLMListView), new PropertyMetadata(null));

        public DataTemplate ItemRightSwipeContentTemplate
        {
            get { return (DataTemplate)GetValue(ItemRightSwipeContentTemplateProperty); }
            set { SetValue(ItemRightSwipeContentTemplateProperty, value); }
        }
        public static readonly DependencyProperty ItemRightSwipeContentTemplateProperty =
            DependencyProperty.Register("ItemRightSwipeContentTemplate", typeof(DataTemplate), typeof(LLMListView), new PropertyMetadata(null));

        public int ItemBackAnimDuration
        {
            get { return (int)GetValue(ItemBackAnimDurationProperty); }
            set { SetValue(ItemBackAnimDurationProperty, value); }
        }
        public static readonly DependencyProperty ItemBackAnimDurationProperty =
            DependencyProperty.Register("ItemBackAnimDuration", typeof(int), typeof(LLMListView), new PropertyMetadata(200));


        public SwipeMode ItemLeftSwipeMode
        {
            get { return (SwipeMode)GetValue(ItemLeftSwipeModeProperty); }
            set { SetValue(ItemLeftSwipeModeProperty, value); }
        }
        public static readonly DependencyProperty ItemLeftSwipeModeProperty =
            DependencyProperty.Register("ItemLeftSwipeMode", typeof(SwipeMode), typeof(LLMListView), new PropertyMetadata(SwipeMode.None));

        public EasingFunctionBase ItemLeftBackAnimEasingFunction
        {
            get { return (EasingFunctionBase)GetValue(ItemLeftBackAnimEasingFunctionProperty); }
            set { SetValue(ItemLeftBackAnimEasingFunctionProperty, value); }
        }
        public static readonly DependencyProperty ItemLeftBackAnimEasingFunctionProperty =
            DependencyProperty.Register("ItemLeftBackAnimEasingFunction", typeof(EasingFunctionBase), typeof(LLMListView), new PropertyMetadata(new ExponentialEase() { EasingMode = EasingMode.EaseOut }));

        public double ItemLeftSwipeLengthRate
        {
            get { return (double)GetValue(ItemLeftSwipeLengthRateProperty); }
            set { SetValue(ItemLeftSwipeLengthRateProperty, value); }
        }
        public static readonly DependencyProperty ItemLeftSwipeLengthRateProperty =
            DependencyProperty.Register("ItemLeftSwipeLengthRate", typeof(double), typeof(LLMListView), new PropertyMetadata(1.0));

        public double ItemLeftSwipeMaxLength
        {
            get { return (double)GetValue(ItemLeftSwipeMaxLengthProperty); }
            set { SetValue(ItemLeftSwipeMaxLengthProperty, value); }
        }
        public static readonly DependencyProperty ItemLeftSwipeMaxLengthProperty =
            DependencyProperty.Register("ItemLeftSwipeMaxLength", typeof(double), typeof(LLMListView), new PropertyMetadata(0.0));

        public double ItemLeftActionRateForSwipeLength
        {
            get { return (double)GetValue(ItemLeftActionRateForSwipeLengthProperty); }
            set { SetValue(ItemLeftActionRateForSwipeLengthProperty, value); }
        }
        public static readonly DependencyProperty ItemLeftActionRateForSwipeLengthProperty =
            DependencyProperty.Register("ItemLeftActionRateForSwipeLength", typeof(double), typeof(LLMListView), new PropertyMetadata(0.5));


        public SwipeMode ItemRightSwipeMode
        {
            get { return (SwipeMode)GetValue(ItemRightSwipeModeProperty); }
            set { SetValue(ItemRightSwipeModeProperty, value); }
        }
        public static readonly DependencyProperty ItemRightSwipeModeProperty =
            DependencyProperty.Register("ItemRightSwipeMode", typeof(SwipeMode), typeof(LLMListView), new PropertyMetadata(SwipeMode.None));

        public EasingFunctionBase ItemRightBackAnimEasingFunction
        {
            get { return (EasingFunctionBase)GetValue(ItemRightBackAnimEasingFunctionProperty); }
            set { SetValue(ItemRightBackAnimEasingFunctionProperty, value); }
        }
        public static readonly DependencyProperty ItemRightBackAnimEasingFunctionProperty =
            DependencyProperty.Register("ItemBackEasingFunction", typeof(EasingFunctionBase), typeof(LLMListView), new PropertyMetadata(new ExponentialEase() { EasingMode = EasingMode.EaseOut }));

        public double ItemRightSwipeLengthRate
        {
            get { return (double)GetValue(ItemRightSwipeLengthRateProperty); }
            set { SetValue(ItemRightSwipeLengthRateProperty, value); }
        }
        public static readonly DependencyProperty ItemRightSwipeLengthRateProperty =
            DependencyProperty.Register("ItemRightSwipeLengthRate", typeof(double), typeof(LLMListView), new PropertyMetadata(1.0));

        public double ItemRightSwipeMaxLength
        {
            get { return (double)GetValue(ItemRightSwipeMaxLengthProperty); }
            set { SetValue(ItemRightSwipeMaxLengthProperty, value); }
        }
        public static readonly DependencyProperty ItemRightSwipeMaxLengthProperty =
            DependencyProperty.Register("ItemRightSwipeMaxLength", typeof(double), typeof(LLMListViewItem), new PropertyMetadata(0.0));

        public double ItemRightActionRateForSwipeLength
        {
            get { return (double)GetValue(ItemRightActionRateForSwipeLengthProperty); }
            set { SetValue(ItemRightActionRateForSwipeLengthProperty, value); }
        }
        public static readonly DependencyProperty ItemRightActionRateForSwipeLengthProperty =
            DependencyProperty.Register("ItemRightActionRateForSwipeLength", typeof(double), typeof(LLMListView), new PropertyMetadata(0.5));

        public string IsSwipedRightMemberPath
        {
            get { return (string)GetValue(IsSwipedRightMemberPathProperty); }
            set { SetValue(IsSwipedRightMemberPathProperty, value); }
        }
        public static readonly DependencyProperty IsSwipedRightMemberPathProperty =
            DependencyProperty.Register("IsSwipedRightMemberPath", typeof(string), typeof(LLMListView), new PropertyMetadata(null));

        public string IsSwipedLeftMemberPath
        {
            get { return (string)GetValue(IsSwipedLeftMemberPathProperty); }
            set { SetValue(IsSwipedLeftMemberPathProperty, value); }
        }
        public static readonly DependencyProperty IsSwipedLeftMemberPathProperty =
            DependencyProperty.Register("IsSwipedLeftMemberPath", typeof(string), typeof(LLMListView), new PropertyMetadata(null));

        public string EnableSwipeRightMemberPath
        {
            get { return (string)GetValue(EnableSwipeRightMemberPathProperty); }
            set { SetValue(EnableSwipeRightMemberPathProperty, value); }
        }
        public static readonly DependencyProperty EnableSwipeRightMemberPathProperty =
            DependencyProperty.Register("EnableSwipeRightMemberPath", typeof(string), typeof(LLMListView), new PropertyMetadata(null));

        public string EnableSwipeLeftMemberPath
        {
            get { return (string)GetValue(EnableSwipeLeftMemberPathProperty); }
            set { SetValue(EnableSwipeLeftMemberPathProperty, value); }
        }
        public static readonly DependencyProperty EnableSwipeLeftMemberPathProperty =
            DependencyProperty.Register("EnableSwipeLeftMemberPath", typeof(string), typeof(LLMListView), new PropertyMetadata(null));

        #endregion

        public LLMListView()
        {
            DefaultStyleKey = typeof(LLMListView);
            Loaded += LLMListView_Loaded;
            Unloaded += LLMListView_OnUnloaded;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            LLMListViewItem item = new LLMListViewItem();
            SetItemBinding(item, LLMListViewItem.LeftSwipeModeProperty, "ItemLeftSwipeMode");
            SetItemBinding(item, LLMListViewItem.RightSwipeModeProperty, "ItemRightSwipeMode");
            SetItemBinding(item, LLMListViewItem.BackAnimDurationProperty, "ItemBackAnimDuration");
            SetItemBinding(item, LLMListViewItem.LeftBackAnimEasingFunctionProperty, "ItemLeftBackAnimEasingFunction");
            SetItemBinding(item, LLMListViewItem.RightBackAnimEasingFunctionProperty, "ItemRightBackAnimEasingFunction");
            SetItemBinding(item, LLMListViewItem.LeftSwipeContentTemplateProperty, "ItemLeftSwipeContentTemplate");
            SetItemBinding(item, LLMListViewItem.RightSwipeContentTemplateProperty, "ItemRightSwipeContentTemplate");
            SetItemBinding(item, LLMListViewItem.LeftSwipeLengthRateProperty, "ItemLeftSwipeLengthRate");
            SetItemBinding(item, LLMListViewItem.LeftActionRateForSwipeLengthProperty, "ItemLeftActionRateForSwipeLength");
            SetItemBinding(item, LLMListViewItem.RightSwipeLengthRateProperty, "ItemRightSwipeLengthRate");
            SetItemBinding(item, LLMListViewItem.RightActionRateForSwipeLengthProperty, "ItemRightActionRateForSwipeLength");
            SetItemBinding(item, LLMListViewItem.LeftSwipeMaxLengthProperty, "ItemLeftSwipeMaxLength");
            SetItemBinding(item, LLMListViewItem.RightSwipeMaxLengthProperty, "ItemRightSwipeMaxLength"); 
            SetItemBinding(item, LLMListViewItem.IsSwipeEnabledProperty, "IsItemSwipeEnabled");
            SetItemBinding(item, LLMListViewItem.IsSwipedRightMemberPathProperty, "IsSwipedRightMemberPath");
            SetItemBinding(item, LLMListViewItem.IsSwipedLeftMemberPathProperty, "IsSwipedLeftMemberPath");
            SetItemBinding(item, LLMListViewItem.EnableSwipeRightMemberPathProperty, "EnableSwipeRightMemberPath");
            SetItemBinding(item, LLMListViewItem.EnableSwipeLeftMemberPathProperty, "EnableSwipeLeftMemberPath");

            item.SwipeBeginInTouch += Item_SwipeBeginInTouch;
            item.SwipeProgressInTouch += Item_SwipeProgressInTouch;
            item.SwipeRestoreComplete += Item_SwipeStoreComplete;
            item.SwipeTriggerComplete += Item_SwipeTriggerComplete;
            item.SwipeBeginTrigger += Item_SwipeBeginTrigger;
            item.SwipeBeginRestore += Item_SwipeBeginRestore;
            item.SwipeTriggerInTouch += Item_SwipeTriggerInTouch;
            return item;
        }

        private void Item_SwipeTriggerComplete(object sender, SwipeCompleteEventArgs args)
        {
            ItemSwipeTriggerComplete?.Invoke(sender, args);
        }

        private void Item_SwipeTriggerInTouch(object sender, SwipeTriggerEventArgs args)
        {
            ItemSwipeTriggerInTouch?.Invoke(sender, args);
        }

        private void Item_SwipeBeginRestore(object sender, SwipeReleaseEventArgs args)
        {
            ItemSwipeBeginRestore?.Invoke(sender, args);
        }

        private void Item_SwipeBeginTrigger(object sender, SwipeReleaseEventArgs args)
        {
            ItemSwipeBeginTrigger?.Invoke(sender, args);
        }

        private void Item_SwipeStoreComplete(object sender, SwipeCompleteEventArgs args)
        {
            ItemSwipeRestoreComplete?.Invoke(sender, args);
        }
        
        private void Item_SwipeProgressInTouch(object sender, SwipeProgressEventArgs args)
        {
            ItemSwipeProgressInTouch?.Invoke(sender, args);
        }
        
        private void Item_SwipeBeginInTouch(object sender)
        {
            ItemSwipeBeginInTouch?.Invoke(sender);
        }

        private void SetItemBinding(LLMListViewItem item, DependencyProperty originProperty, string targetProperty)
        {
            var binding = new Binding() { Source = this, Path = new PropertyPath(targetProperty) };
            BindingOperations.SetBinding(item, originProperty, binding);
        }

        private void LLMListView_Loaded(object sender, RoutedEventArgs e)
        {
            InitTimer();
            InitVisualState();
            UpdateProgressBarLayout();
            UpdateEmptyDataTemplateVisibility();
        }

        protected override void OnItemsChanged(object e)
        {
            base.OnItemsChanged(e);
            UpdateLoadingMore();
            UpdateEmptyDataTemplateVisibility();
        }

        private void UpdateEmptyDataTemplateVisibility()
        {
            if (EmptyDataTemplate == null || _itemsPresenter == null || _emptyTemplateControl == null) return;
            var itemsCount = Items?.Count ?? 0;
            if (itemsCount > 0)
            {
                _itemsPresenter.Visibility = Visibility.Visible;
                _emptyTemplateControl.Visibility = Visibility.Collapsed;
            }
            else
            {
                _itemsPresenter.Visibility = Visibility.Collapsed;
                _emptyTemplateControl.Visibility = Visibility.Visible;
            }
        }

        private void LLMListView_OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_timer != null)
            {
                _timer.Tick -= Timer_Tick;
                _timer.Stop();
            }
            if (_notifyToRefreshTimer != null)
            {
                _notifyToRefreshTimer.Tick -= NotifyToRefreshTimer_Tick;
                _notifyToRefreshTimer.Stop();
            }
        }

        private void UpdateProgressBarLayout()
        {
            if(_pullProgressBar != null)
                _pullProgressBar.Width = ActualWidth;

            if(_loadMoreProgressBar != null)
                _loadMoreProgressBar.Width = ActualWidth;
        }

        private void InitTimer()
        {
            if (!CanPullToRefresh || !Utils.IsOnMobile)
                return;

            _notifyToRefreshTimer = new DispatcherTimer();
            _notifyToRefreshTimer.Interval = TimeSpan.FromMilliseconds(Refresh_Notify_Interval);
            _notifyToRefreshTimer.Tick += NotifyToRefreshTimer_Tick;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(Refresh_Status_Interval);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void InitVisualState()
        {
            if (CanPullToRefresh && Utils.IsOnMobile)
            {
                VisualStateManager.GoToState(this, Normal_State, false);
            }
            else
            {
                VisualStateManager.GoToState(this, Unvalid_State, false);
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            InitControls();

            InitScrollViewEventsForPullToRefresh();

            InitFloatButtonClickEvent();

            InitOtherEvents();
        }

        private void InitControls()
        {
            _scrollViewer = (ScrollViewer) GetTemplateChild("ScrollViewer");
            _container = (Grid) GetTemplateChild("Container");
            _pullToRefreshIndicator = (Border) GetTemplateChild("PullToRefreshIndicator");
            _pullProgressBar = (ProgressBar) GetTemplateChild("PullProgressBar");
            _refreshProgressRing = (ProgressRing) GetTemplateChild("RefreshProgressRing");
            _loadMoreProgressBar = (ProgressBar) GetTemplateChild("LoadMoreProgressBar");
            _floatButton = (XPButton) GetTemplateChild("FloatButton");
            _floatButtonShadow = (Image) GetTemplateChild("FloatButtonShadow");
            _itemsPresenter = (ItemsPresenter) GetTemplateChild("ItemsPresenter");
            _emptyTemplateControl = (ContentControl) GetTemplateChild("EmptyTemplateControl");

            Utils.SetBase64ToImage((BitmapSource) _floatButtonShadow.Source, FloatBtn_Shadow_Base64);
        }

        private void InitScrollViewEventsForPullToRefresh()
        {
            if (CanPullToRefresh && Utils.IsOnMobile)
            {
                _scrollViewer.ViewChanging += ScrollViewer_ViewChanging;
                _scrollViewer.Margin = new Thickness(0, 0, 0, -RefreshAreaHeight);
                _scrollViewer.RenderTransform = new CompositeTransform() { TranslateY = -RefreshAreaHeight };
            }
        }

        private void InitFloatButtonClickEvent()
        {
            if (FloatButtonVisibility == Visibility.Visible)
            {
                _floatButton.Click += (s, e) =>
                {
                    if(FloatButtonAction != null)
                    {
                        FloatButtonAction();
                        return;
                    }

                    if (_isRefreshing)
                        return;

                    SetRefresh(true);
                };
            }
        }

        private void InitOtherEvents()
        {
            _scrollViewer.ViewChanged += _scrollViewer_ViewChanged;
            SizeChanged += LLMListView_SizeChanged;
        }

        private void _scrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            UpdateFloatButtonState();

            UpdateLoadingMore();
        }

        private void UpdateFloatButtonState()
        {
            const int responseOffset = 2;
            var verticalOffset = (int)_scrollViewer.VerticalOffset;

            if (_lastVerticalOffset + responseOffset < verticalOffset)
            {
                VisualStateManager.GoToState(this, FloatBtn_Collapse_State, true);
            }
            else if(_lastVerticalOffset > verticalOffset + responseOffset)
            {
                VisualStateManager.GoToState(this, FloatBtn_Visible_State, true);
            }
            _lastVerticalOffset = verticalOffset;
        }

        private void UpdateLoadingMore()
        {
            if (LoadMore == null || _isLoadingMore || _scrollViewer == null) return;

            var bottomOffset = _scrollViewer.ExtentHeight - _scrollViewer.VerticalOffset - _scrollViewer.ViewportHeight;
            if (bottomOffset < ScrollOffsetOfLoadMoreTrigger)
            {
                ToggleLoadingMoreStatus(true);
                LoadMore();
            }
        }

        private void ToggleLoadingMoreStatus(bool isLoadingMore)
        {
            _isLoadingMore = isLoadingMore;
            _loadMoreProgressBar.Visibility = isLoadingMore ? Visibility.Visible : Visibility.Collapsed;
        }

        public void FinishLoadingMore()
        {
            ToggleLoadingMoreStatus(false);
        }

        public void SetRefresh(bool isRefresh)
        {
            _isRefreshing = isRefresh;
            if (_isRefreshing)
            {
                VisualStateManager.GoToState(this, RefreshState, true);
                Refresh?.Invoke();
            }
            else
            {
                VisualStateManager.GoToState(this, NormalState, true);
            }
        }

        private string NormalState { get { return CanPullToRefresh && Utils.IsOnMobile ? Normal_State : FloatBtn_Normal_State; } }

        private string RefreshState { get { return CanPullToRefresh && Utils.IsOnMobile ? Refreshing_State : FloatBtn_Refreshing_State; } }

        #region events

        private void Timer_Tick(object sender, object e)
        {
            if (_isRefreshing)
                return;

            var pullOffsetRect = _pullToRefreshIndicator.TransformToVisual(_container).TransformBounds(new Rect(0, 0, ActualWidth, RefreshAreaHeight));
            var pullOffsetBottom = pullOffsetRect.Bottom;
            _pullProgressBar.Value = pullOffsetRect.Bottom * 100 / RefreshAreaHeight;

            if (pullOffsetBottom > RefreshAreaHeight)
            {
                if (!_isNotifyToRefreshTimerStarting)
                {
                    _isNotifyToRefreshTimerStarting = true;
                    _notifyToRefreshTimer.Start();
                }
            }
            else
            {
                _isNotifyToRefreshTimerStarting = false;
                _notifyToRefreshTimer.Stop();
            }
        }

        private void NotifyToRefreshTimer_Tick(object sender, object e)
        {
            if (_isRefreshing)
                return;

            SetRefresh(true);
        }

        private void ScrollViewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            if (!CanPullToRefresh || !Utils.IsOnMobile)
                return;

            if (e.NextView.VerticalOffset == 0)
            {
                _timer.Start();
            }
            else
            {
                _timer.Stop();
                _notifyToRefreshTimer.Stop();
                _pullProgressBar.Value = 0;
                _isNotifyToRefreshTimerStarting = false;
            }
        }

        private void LLMListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Clip = new RectangleGeometry() { Rect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height) };
            UpdateProgressBarLayout();
        }

        #endregion
    }
}
