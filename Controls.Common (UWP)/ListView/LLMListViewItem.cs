using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using System.Reflection;
using Windows.UI.Core;
using Windows.UI.Xaml.Data;

namespace Imagin.Controls.Common
{
    public sealed class LLMListViewItem : ListViewItem
    {
        private TranslateTransform _mainLayerTransform;
        private ScaleTransform _swipeLayerClipTransform;
        private RectangleGeometry _swipeLayerClip;
        private ContentControl _rightSwipeContent;
        private ContentControl _leftSwipeContent;
        private readonly SwipeReleaseAnimationConstructor _swipeAnimationConstructor;
        private bool _isTriggerInTouch;
        private bool IsSwipedByGesture;
        private bool _isLoaded;

        public event SwipeBeginEventHandler SwipeBeginInTouch;
        public event SwipeProgressEventHandler SwipeProgressInTouch;
        public event SwipeCompleteEventHandler SwipeRestoreComplete;
        public event SwipeCompleteEventHandler SwipeTriggerComplete;
        public event SwipeReleaseEventHandler SwipeBeginTrigger;
        public event SwipeReleaseEventHandler SwipeBeginRestore;
        public event SwipeTriggerEventHandler SwipeTriggerInTouch;

        public SwipeConfig Config => _swipeAnimationConstructor?.Config;

        #region property

        public string IsSwipedRightMemberPath
        {
            get { return (string)GetValue(IsSwipedRightMemberPathProperty); }
            set { SetValue(IsSwipedRightMemberPathProperty, value); }
        }
        public static readonly DependencyProperty IsSwipedRightMemberPathProperty =
            DependencyProperty.Register(nameof(IsSwipedRightMemberPath), typeof(string), typeof(LLMListViewItem), new PropertyMetadata(null));

        public bool IsSwipedRight
        {
            get { return (bool)GetValue(IsSwipedRightProperty); }
            set { SetValue(IsSwipedRightProperty, value); }
        }
        public static readonly DependencyProperty IsSwipedRightProperty =
            DependencyProperty.Register(nameof(IsSwipedRight), typeof(bool), typeof(LLMListViewItem), new PropertyMetadata(false, (o, args) => SwipeByDependencyChange(o, args, SwipeDirection.Right)));

        public string IsSwipedLeftMemberPath
        {
            get { return (string)GetValue(IsSwipedLeftMemberPathProperty); }
            set { SetValue(IsSwipedLeftMemberPathProperty, value); }
        }
        public static readonly DependencyProperty IsSwipedLeftMemberPathProperty =
            DependencyProperty.Register(nameof(IsSwipedLeftMemberPath), typeof(string), typeof(LLMListViewItem), new PropertyMetadata(null));

        public bool IsSwipedLeft
        {
            get { return (bool)GetValue(IsSwipedLeftProperty); }
            set { SetValue(IsSwipedLeftProperty, value); }
        }
        public static readonly DependencyProperty IsSwipedLeftProperty =
            DependencyProperty.Register(nameof(IsSwipedLeft), typeof(bool), typeof(LLMListViewItem), new PropertyMetadata(false, (o, args) => SwipeByDependencyChange(o, args, SwipeDirection.Left)));
        
        private void SetIsSwipedByGesture(SwipeDirection direction, bool isSwiped)
        {
            IsSwipedByGesture = true;
            if (direction == SwipeDirection.Right)
            {
                if (IsSwipedLeft) IsSwipedLeft = false;
                IsSwipedByGesture = true;
                IsSwipedRight = isSwiped;
            }
            else if (direction == SwipeDirection.Left)
            {
                if (IsSwipedRight) IsSwipedRight = false;
                IsSwipedByGesture = true;
                IsSwipedLeft = isSwiped;
            }
        }

        public string EnableSwipeRightMemberPath
        {
            get { return (string)GetValue(EnableSwipeRightMemberPathProperty); }
            set { SetValue(EnableSwipeRightMemberPathProperty, value); }
        }
        public static readonly DependencyProperty EnableSwipeRightMemberPathProperty =
            DependencyProperty.Register("EnableSwipeRightMemberPath", typeof(string), typeof(LLMListViewItem), new PropertyMetadata(null));

        public bool EnableSwipeRight
        {
            get { return (bool)GetValue(EnableSwipeRightProperty); }
            set { SetValue(EnableSwipeRightProperty, value); }
        }
        public static readonly DependencyProperty EnableSwipeRightProperty =
            DependencyProperty.Register("EnableSwipeRight", typeof(bool), typeof(LLMListViewItem), new PropertyMetadata(true, (s, e) => ((LLMListViewItem)s).UpdateConfig()));

        public string EnableSwipeLeftMemberPath
        {
            get { return (string)GetValue(EnableSwipeLeftMemberPathProperty); }
            set { SetValue(EnableSwipeLeftMemberPathProperty, value); }
        }
        public static readonly DependencyProperty EnableSwipeLeftMemberPathProperty =
            DependencyProperty.Register("EnableSwipeLeftMemberPath", typeof(string), typeof(LLMListViewItem), new PropertyMetadata(null));

        public bool EnableSwipeLeft
        {
            get { return (bool)GetValue(EnableSwipeLeftProperty); }
            set { SetValue(EnableSwipeLeftProperty, value); }
        }
        public static readonly DependencyProperty EnableSwipeLeftProperty =
            DependencyProperty.Register("EnableSwipeLeft", typeof(bool), typeof(LLMListViewItem), new PropertyMetadata(true, (s, e) => ((LLMListViewItem)s).UpdateConfig()));

        public bool IsSwipeEnabled
        {
            get { return (bool)GetValue(IsSwipeEnabledProperty); }
            set { SetValue(IsSwipeEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsSwipeEnabledProperty =
            DependencyProperty.Register(nameof(IsSwipeEnabled), typeof(bool), typeof(LLMListViewItem),
                new PropertyMetadata(true, (s, e) =>
                {
                    var listViewItem = s as LLMListViewItem;
                    if (listViewItem == null)
                        return;

                    listViewItem.ItemManipulationMode = (bool) e.NewValue
                        ? ManipulationModes.TranslateX | ManipulationModes.System
                        : ManipulationModes.System;
                }));

        public ManipulationModes ItemManipulationMode
        {
            get { return (ManipulationModes)GetValue(ItemManipulationModeProperty); }
            set { SetValue(ItemManipulationModeProperty, value); }
        }
        public static readonly DependencyProperty ItemManipulationModeProperty =
            DependencyProperty.Register(nameof(ItemManipulationMode), typeof(ManipulationModes), typeof(LLMListViewItem), new PropertyMetadata(ManipulationModes.TranslateX | ManipulationModes.System));

        public DataTemplate LeftSwipeContentTemplate
        {
            get { return (DataTemplate)GetValue(LeftSwipeContentTemplateProperty); }
            set { SetValue(LeftSwipeContentTemplateProperty, value); }
        }
        public static readonly DependencyProperty LeftSwipeContentTemplateProperty =
            DependencyProperty.Register(nameof(LeftSwipeContentTemplate), typeof(DataTemplate), typeof(LLMListViewItem), new PropertyMetadata(null));

        public DataTemplate RightSwipeContentTemplate
        {
            get { return (DataTemplate)GetValue(RightSwipeContentTemplateProperty); }
            set { SetValue(RightSwipeContentTemplateProperty, value); }
        }
        public static readonly DependencyProperty RightSwipeContentTemplateProperty =
            DependencyProperty.Register(nameof(RightSwipeContentTemplate), typeof(DataTemplate), typeof(LLMListViewItem), new PropertyMetadata(null));
        
        public int BackAnimDuration
        {
            get { return (int)GetValue(BackAnimDurationProperty); }
            set { SetValue(BackAnimDurationProperty, value); }
        }
        public static readonly DependencyProperty BackAnimDurationProperty =
            DependencyProperty.Register(nameof(BackAnimDuration), typeof(int), typeof(LLMListViewItem), new PropertyMetadata(200, (s, e) => ((LLMListViewItem)s).UpdateConfig()));

        public SwipeMode LeftSwipeMode
        {
            get { return (SwipeMode)GetValue(LeftSwipeModeProperty); }
            set { SetValue(LeftSwipeModeProperty, value); }
        }
        public static readonly DependencyProperty LeftSwipeModeProperty =
            DependencyProperty.Register(nameof(LeftSwipeMode), typeof(SwipeMode), typeof(LLMListViewItem), new PropertyMetadata(SwipeMode.None, (s, e) => ((LLMListViewItem)s).UpdateConfig()));

        public EasingFunctionBase LeftBackAnimEasingFunction
        {
            get { return (EasingFunctionBase)GetValue(LeftBackAnimEasingFunctionProperty); }
            set { SetValue(LeftBackAnimEasingFunctionProperty, value); }
        }
        public static readonly DependencyProperty LeftBackAnimEasingFunctionProperty =
            DependencyProperty.Register(nameof(LeftBackAnimEasingFunction), typeof(EasingFunctionBase), typeof(LLMListViewItem), new PropertyMetadata(new ExponentialEase() { EasingMode = EasingMode.EaseOut }, (s, e) => ((LLMListViewItem)s).UpdateConfig()));

        public double LeftSwipeMaxLength
        {
            get { return (double )GetValue(LeftSwipeMaxLengthProperty); }
            set { SetValue(LeftSwipeMaxLengthProperty, value); }
        }
        public static readonly DependencyProperty LeftSwipeMaxLengthProperty =
            DependencyProperty.Register(nameof(LeftSwipeMaxLength), typeof(double ), typeof(LLMListViewItem), new PropertyMetadata(0.0));

        public double LeftSwipeLengthRate
        {
            get { return (double)GetValue(LeftSwipeLengthRateProperty); }
            set { SetValue(LeftSwipeLengthRateProperty, value); }
        }
        public static readonly DependencyProperty LeftSwipeLengthRateProperty =
            DependencyProperty.Register(nameof(LeftSwipeLengthRate), typeof(double), typeof(LLMListViewItem), new PropertyMetadata(1.0, (s, e) => ((LLMListViewItem)s).UpdateConfig()));

        public double LeftActionRateForSwipeLength
        {
            get { return (double)GetValue(LeftActionRateForSwipeLengthProperty); }
            set { SetValue(LeftActionRateForSwipeLengthProperty, value); }
        }
        public static readonly DependencyProperty LeftActionRateForSwipeLengthProperty =
            DependencyProperty.Register(nameof(LeftActionRateForSwipeLength), typeof(double), typeof(LLMListViewItem), new PropertyMetadata(0.5, (s, e) => ((LLMListViewItem)s).UpdateConfig()));

        public double ActualLeftSwipeLengthRate => LeftSwipeMaxLength.Equals(0) ? LeftSwipeLengthRate : LeftSwipeMaxLength / ActualWidth;


        public SwipeMode RightSwipeMode
        {
            get { return (SwipeMode)GetValue(RightSwipeModeProperty); }
            set { SetValue(RightSwipeModeProperty, value); }
        }
        public static readonly DependencyProperty RightSwipeModeProperty =
            DependencyProperty.Register(nameof(RightSwipeMode), typeof(SwipeMode), typeof(LLMListViewItem), new PropertyMetadata(SwipeMode.None, (s, e) => ((LLMListViewItem)s).UpdateConfig()));

        public double RightSwipeMaxLength
        {
            get { return (double)GetValue(RightSwipeMaxLengthProperty); }
            set { SetValue(RightSwipeMaxLengthProperty, value); }
        }
        public static readonly DependencyProperty RightSwipeMaxLengthProperty =
            DependencyProperty.Register(nameof(RightSwipeMaxLength), typeof(double), typeof(LLMListViewItem), new PropertyMetadata(0.0));

        public double RightSwipeLengthRate
        {
            get { return (double)GetValue(RightSwipeLengthRateProperty); }
            set { SetValue(RightSwipeLengthRateProperty, value); }
        }
        public static readonly DependencyProperty RightSwipeLengthRateProperty =
            DependencyProperty.Register(nameof(RightSwipeLengthRate), typeof(double), typeof(LLMListViewItem), new PropertyMetadata(1.0, (s, e) => ((LLMListViewItem)s).UpdateConfig()));

        public double ActualRightSwipeLengthRate => RightSwipeMaxLength.Equals(0) ? RightSwipeLengthRate : RightSwipeMaxLength / ActualWidth;

        public double RightActionRateForSwipeLength
        {
            get { return (double)GetValue(RightActionRateForSwipeLengthProperty); }
            set { SetValue(RightActionRateForSwipeLengthProperty, value); }
        }
        public static readonly DependencyProperty RightActionRateForSwipeLengthProperty =
            DependencyProperty.Register(nameof(RightActionRateForSwipeLength), typeof(double), typeof(LLMListViewItem), new PropertyMetadata(0.5, (s, e)=>((LLMListViewItem)s).UpdateConfig() ));

        public EasingFunctionBase RightBackAnimEasingFunction
        {
            get { return (EasingFunctionBase)GetValue(RightBackAnimEasingFunctionProperty); }
            set { SetValue(RightBackAnimEasingFunctionProperty, value); }
        }
        public static readonly DependencyProperty RightBackAnimEasingFunctionProperty =
            DependencyProperty.Register(nameof(RightBackAnimEasingFunction), typeof(EasingFunctionBase), typeof(LLMListViewItem), new PropertyMetadata(new ExponentialEase() { EasingMode = EasingMode.EaseOut }, (s, e) => ((LLMListViewItem)s).UpdateConfig()));

        #endregion

        public LLMListViewItem()
        {
            DefaultStyleKey = typeof(LLMListViewItem);
            Loaded += LLMListViewItem_Loaded;
            _swipeAnimationConstructor = SwipeReleaseAnimationConstructor.Create(new SwipeConfig());
        }

        private void LLMListViewItem_Loaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;
            UpdateConfig();
            SyncSwipeStateToBindings();
            SizeChanged += LLMListViewItem_SizeChanged;
        }

        private void SyncSwipeStateToBindings()
        {
            if (IsSwipedLeft && IsSwipedRight)
            {
                throw new NotSupportedException(
                    "Item can't be in IsSwipedLeft and IsSwipedRight states at the same time");
            }

            ResetSwipe();
            if (IsSwipedRight)
            {
                SwipeTo(SwipeDirection.Right, false);
            }
            else if (IsSwipedLeft)
            {
                SwipeTo(SwipeDirection.Left, false);
            }
        }

        private static void SwipeByDependencyChange(DependencyObject obj, DependencyPropertyChangedEventArgs args, SwipeDirection direction)
        {
            var ctrl = (LLMListViewItem)obj;
            if (ctrl.IsSwipedByGesture)
            {
                ctrl.IsSwipedByGesture = false;
                return;
            }

            if (direction == SwipeDirection.Left && ctrl.IsSwipedRight)
            {
                ctrl.IsSwipedByGesture = true;
                ctrl.IsSwipedRight = false;
                ctrl.ResetSwipe();
            }
            else if (direction == SwipeDirection.Right && ctrl.IsSwipedLeft)
            {
                ctrl.IsSwipedByGesture = true;
                ctrl.IsSwipedLeft = false;
                ctrl.ResetSwipe();
            }

            var newSwipeValue = (bool)args.NewValue;
            if (newSwipeValue)
            {
                ctrl.SwipeTo(direction, true);
            }
            else
            {
                ctrl.ResetSwipeWithAnimation();
            }
        }

        private void UpdateConfig()
        {
            Config.Duration = BackAnimDuration;
            Config.LeftEasingFunc = LeftBackAnimEasingFunction;
            Config.RightEasingFunc = RightBackAnimEasingFunction;
            Config.LeftSwipeMode = LeftSwipeMode;
            Config.RightSwipeMode = RightSwipeMode;
            Config.MainTransform = _mainLayerTransform;
            Config.SwipeClipTransform = _swipeLayerClipTransform;
            Config.SwipeClipRectangle = _swipeLayerClip;
            Config.LeftActionRateForSwipeLength = LeftActionRateForSwipeLength;
            Config.RightActionRateForSwipeLength = RightActionRateForSwipeLength;
            Config.LeftSwipeLengthRate = ActualLeftSwipeLengthRate;
            Config.RightSwipeLengthRate = ActualRightSwipeLengthRate;
            Config.ItemActualWidth = ActualWidth;
            Config.ItemActualHeight = ActualHeight;
            Config.EnableSwipeLeft = EnableSwipeLeft;
            Config.EnableSwipeRight = EnableSwipeRight;
        }

        private void LLMListViewItem_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateConfig();
            ResetSwipe();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _mainLayerTransform = (TranslateTransform)GetTemplateChild("ContentPresenterTranslateTransform");
            _swipeLayerClipTransform = (ScaleTransform)GetTemplateChild("SwipeLayerClipTransform");
            _swipeLayerClip = (RectangleGeometry)GetTemplateChild("SwipeLayerClip");
            _rightSwipeContent = (ContentControl)GetTemplateChild("RightSwipeContent");
            _leftSwipeContent = (ContentControl)GetTemplateChild("LeftSwipeContent");
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            ResetSwipe();

            ApplyBindingBooleanProperty(newContent, IsSwipedRightProperty, IsSwipedRightMemberPath);
            ApplyBindingBooleanProperty(newContent, IsSwipedLeftProperty, IsSwipedLeftMemberPath);

            ApplyBindingBooleanProperty(newContent, EnableSwipeRightProperty, EnableSwipeRightMemberPath);
            ApplyBindingBooleanProperty(newContent, EnableSwipeLeftProperty, EnableSwipeLeftMemberPath);

            if (_isLoaded) SyncSwipeStateToBindings();
            base.OnContentChanged(oldContent, newContent);
        }

        private void ApplyBindingBooleanProperty(object context, DependencyProperty dependencyProperty, string property)
        {
            if (context == null || string.IsNullOrEmpty(property)) return;

            var isSwipedFieldInfo = context.GetType().GetProperty(property);
            if (isSwipedFieldInfo == null || isSwipedFieldInfo.PropertyType != typeof(bool)) return;

            SetBinding(dependencyProperty, new Binding
            {
                Source = context,
                Path = new PropertyPath(property),
                Mode = BindingMode.TwoWay
            });
        }

        protected override void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
            SwipeBeginInTouch?.Invoke(this);
            var cumulativeX = e.Cumulative.Translation.X;
            var deltaX = e.Delta.Translation.X;

            if (Config.Direction == SwipeDirection.None)
            {
                ResetSwipe();
                Config.Direction = deltaX > 0 ? SwipeDirection.Left : SwipeDirection.Right;
                _leftSwipeContent.Visibility = Config.CanSwipeLeft ? Visibility.Visible : Visibility.Collapsed;
                _rightSwipeContent.Visibility = Config.CanSwipeRight ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (Config.CanSwipeLeft)
            {
                SwipeToLeft(cumulativeX, deltaX);
            }
            else if(Config.CanSwipeRight)
            {
                SwipeToRight(cumulativeX, deltaX);
            }
        }

        private void SwipeToLeft(double cumulativeX, double deltaX)
        {
            cumulativeX = deltaX + _mainLayerTransform.X;
            var swipeLengthRate = Math.Abs(cumulativeX) / ActualWidth;

            if (cumulativeX <= 0)
            {
                ResetSwipe();
            }
            else if (swipeLengthRate <= ActualLeftSwipeLengthRate)
            {
                _swipeLayerClip.Rect = new Rect(0, 0, Math.Max(0, cumulativeX), ActualHeight);
                _mainLayerTransform.X = cumulativeX;
                SwipeActionInTouch(cumulativeX, deltaX);
            }
        }

        private void SwipeToRight(double cumulativeX, double deltaX)
        {
            cumulativeX = deltaX + _mainLayerTransform.X;
            var swipeLengthRate = Math.Abs(cumulativeX) / ActualWidth;

            if (cumulativeX >= 0)
            {
                ResetSwipe();
            }
            else if (swipeLengthRate <= ActualRightSwipeLengthRate)
            {
                _swipeLayerClip.Rect = new Rect(ActualWidth + cumulativeX, 0, Math.Max(0, -cumulativeX), ActualHeight);
                _mainLayerTransform.X = cumulativeX;
                SwipeActionInTouch(cumulativeX, deltaX);
            }
        }

        private void SwipeActionInTouch(double cumulativeX, double deltaX)
        {
            double currRate = Math.Abs(cumulativeX) / ActualWidth;
            var isTriggerRate = currRate >= (Config.Direction == SwipeDirection.Left ? LeftActionRateForSwipeLength / ActualLeftSwipeLengthRate : RightActionRateForSwipeLength / ActualRightSwipeLengthRate);
            if (_isTriggerInTouch != isTriggerRate)
            {
                _isTriggerInTouch = isTriggerRate;
                SwipeTriggerInTouch?.Invoke(this, new SwipeTriggerEventArgs(Config.Direction, isTriggerRate));
            }
            SwipeProgressInTouch?.Invoke(this, new SwipeProgressEventArgs(Config.Direction, cumulativeX, deltaX, Math.Abs(cumulativeX) / ActualWidth));
        }

        private void ResetSwipe()
        {
            if (!_isLoaded) return;

            Config.Direction = SwipeDirection.None;
            _swipeLayerClip.Rect = new Rect(0, 0, 0, 0);
            _mainLayerTransform.X = 0;
            _isTriggerInTouch = false;
        }

        private void SwipeTo(SwipeDirection direction, bool animated)
        {
            if (!_isLoaded) return;

            Config.Direction = direction;
            _leftSwipeContent.Visibility = Config.CanSwipeLeft ? Visibility.Visible : Visibility.Collapsed;
            _rightSwipeContent.Visibility = Config.CanSwipeRight ? Visibility.Visible : Visibility.Collapsed;
            FixedSwipeAnimator.Instance.SwipeTo(direction, Config, animated);
        }

        private void ResetSwipeWithAnimation()
        {
            FixedSwipeAnimator.Instance.Restore(Config, null, ResetSwipe);
        }

        protected override void OnManipulationCompleted(ManipulationCompletedRoutedEventArgs e)
        {
            var oldDirection = Config.Direction;
            var swipeRate = e.Cumulative.Translation.X / ActualWidth * Config.SwipeLengthRate;
            _swipeAnimationConstructor.Config.CurrentSwipeWidth = Math.Abs(_mainLayerTransform.X);

            _swipeAnimationConstructor.DisplaySwipeAnimation(oldDirection,
                (easingFunc, itemToX, duration) => ReleaseAnimationBeginTrigger(oldDirection, easingFunc, itemToX, duration),
                () => ReleaseAnimationTriggerComplete(oldDirection),
                (easingFunc, itemToX, duration) => ReleaseAnimationBeginRestore(oldDirection, easingFunc, itemToX, duration),
                () => ReleaseAnimationRestoreComplete(oldDirection)
            );

            Config.Direction = SwipeDirection.None;
        }

        private void ReleaseAnimationBeginTrigger(SwipeDirection direction, EasingFunctionBase easingFunc, double itemToX, double duration)
        {
            SwipeBeginTrigger?.Invoke(this, new SwipeReleaseEventArgs(direction, easingFunc, itemToX, duration));
        }

        private void ReleaseAnimationTriggerComplete(SwipeDirection direction)
        {
            if (Config.GetSwipeMode(direction) == SwipeMode.Fix)
            {
                Config.Direction = direction;
                SetIsSwipedByGesture(direction, true);
            }
            SwipeTriggerComplete?.Invoke(this, new SwipeCompleteEventArgs(direction));
        }

        private void ReleaseAnimationBeginRestore(SwipeDirection direction, EasingFunctionBase easingFunc, double itemToX, double duration)
        {
            SwipeBeginRestore?.Invoke(this, new SwipeReleaseEventArgs(direction, easingFunc, itemToX, duration));
        }

        private void ReleaseAnimationRestoreComplete(SwipeDirection direction)
        {
            SetIsSwipedByGesture(direction, false);
            SwipeRestoreComplete?.Invoke(this, new SwipeCompleteEventArgs(direction));
        }

        public T GetSwipeControl<T>(SwipeDirection direction, string name) where T : FrameworkElement
        {
            if (direction == SwipeDirection.None)
                return default(T);

            var contentCtrl = (direction == SwipeDirection.Left ? _leftSwipeContent : _rightSwipeContent) as DependencyObject;
            return Utils.FindVisualChild<T>(contentCtrl, name);
        }
    }
}
