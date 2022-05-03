using Imagin.Common.Converters;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Imagin.Common.Controls
{
    #region (enum) SwipeButtonDirection

    [Serializable]
    public enum SwipeButtonDirection
    {
        Horizontal,
        Vertical
    }

    #endregion

    #region (enum) SwipeButtonMode

    [Serializable]
    public enum SwipeButtonMode
    {
        Default,
        Overlap
    }

    #endregion

    public class SwipeButton : ContentControl
    {
        #region Events

        public static readonly RoutedEvent CheckedEvent = EventManager.RegisterRoutedEvent("Checked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SwipeButton));
        public static void AddCheckedHandler(SwipeButton sender, RoutedEventHandler handler)
            => sender.AddHandler(CheckedEvent, handler);
        public static void RemoveCheckedHandler(SwipeButton sender, RoutedEventHandler handler)
            => sender.RemoveHandler(CheckedEvent, handler);

        public static readonly RoutedEvent SwipedEvent = EventManager.RegisterRoutedEvent("Swiped", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SwipeButton));
        public static void AddSwipedHandler(SwipeButton sender, RoutedEventHandler handler)
            => sender.AddHandler(SwipedEvent, handler);
        public static void RemoveSwipedHandler(SwipeButton sender, RoutedEventHandler handler)
            => sender.RemoveHandler(SwipedEvent, handler);

        public static readonly RoutedEvent UncheckedEvent = EventManager.RegisterRoutedEvent("Unchecked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SwipeButton));
        public static void AddUncheckedHandler(SwipeButton sender, RoutedEventHandler handler)
            => sender.AddHandler(UncheckedEvent, handler);
        public static void RemoveUncheckedHandler(SwipeButton sender, RoutedEventHandler handler)
            => sender.RemoveHandler(UncheckedEvent, handler);

        #endregion

        #region Fields

        Point aPoint; Point bPoint;

        //...

        Action lastAction;

        Storyboard lastAnimation;

        #endregion

        #region Properties

        static IMultiValueConverter visibilityConverter;
        public static IMultiValueConverter VisibilityConverter => visibilityConverter ??= new MultiConverter<Visibility>(i =>
        {
            if (i.Values?.Length == 3)
            {
                if (i.Values[0] is bool a)
                {
                    if (i.Values[1] is bool isAnimating)
                    {
                        if (i.Values[2] is bool isSwiping)
                            return (a && (isAnimating || isSwiping)).Visibility();
                    }
                }
            }
            return Visibility.Collapsed;
        });

        //...

        public static readonly DependencyProperty CheckedCommandProperty = DependencyProperty.Register(nameof(CheckedCommand), typeof(ICommand), typeof(SwipeButton), new FrameworkPropertyMetadata(null));
        public ICommand CheckedCommand
        {
            get => (ICommand)GetValue(CheckedCommandProperty);
            set => SetValue(CheckedCommandProperty, value);
        }

        public static readonly DependencyProperty CheckedCommandParameterProperty = DependencyProperty.Register(nameof(CheckedCommandParameter), typeof(object), typeof(SwipeButton), new FrameworkPropertyMetadata(null));
        public object CheckedCommandParameter
        {
            get => GetValue(CheckedCommandParameterProperty);
            set => SetValue(CheckedCommandParameterProperty, value);
        }

        public static readonly DependencyProperty UncheckedCommandProperty = DependencyProperty.Register(nameof(UncheckedCommand), typeof(ICommand), typeof(SwipeButton), new FrameworkPropertyMetadata(null));
        public ICommand UncheckedCommand
        {
            get => (ICommand)GetValue(UncheckedCommandProperty);
            set => SetValue(UncheckedCommandProperty, value);
        }

        public static readonly DependencyProperty UncheckedCommandParameterProperty = DependencyProperty.Register(nameof(UncheckedCommandParameter), typeof(object), typeof(SwipeButton), new FrameworkPropertyMetadata(null));
        public object UncheckedCommandParameter
        {
            get => GetValue(UncheckedCommandParameterProperty);
            set => SetValue(UncheckedCommandParameterProperty, value);
        }

        public static readonly DependencyProperty ClickModeProperty = DependencyProperty.Register(nameof(ClickMode), typeof(ClickMode), typeof(SwipeButton), new FrameworkPropertyMetadata(ClickMode.Press));
        public ClickMode ClickMode
        {
            get => (ClickMode)GetValue(ClickModeProperty);
            set => SetValue(ClickModeProperty, value);
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(SwipeButton), new FrameworkPropertyMetadata(null));
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(SwipeButton), new FrameworkPropertyMetadata(null));
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public static readonly DependencyProperty ContentXProperty = DependencyProperty.Register(nameof(ContentX), typeof(double), typeof(SwipeButton), new FrameworkPropertyMetadata(0d, OnContentXChanged));
        public double ContentX
        {
            get => (double)GetValue(ContentXProperty);
            set => SetValue(ContentXProperty, value);
        }
        static void OnContentXChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is SwipeButton button)
            {
                if (button.IsLeftSwiping)
                {
                    button.LeftSwipeProgress = (button.ContentX / (button.ActualWidth / 2d)).Coerce(1);
                    button.RightSwipeProgress = 1;
                }
                if (button.IsRightSwiping)
                {
                    button.RightSwipeProgress = (button.ContentX.Absolute() / (button.ActualWidth / 2d)).Coerce(1);
                    button.LeftSwipeProgress = 1;
                }
            }
        }

        public static readonly DependencyProperty ContentYProperty = DependencyProperty.Register(nameof(ContentY), typeof(double), typeof(SwipeButton), new FrameworkPropertyMetadata(0d, OnContentYChanged));
        public double ContentY
        {
            get => (double)GetValue(ContentYProperty);
            set => SetValue(ContentYProperty, value);
        }
        static void OnContentYChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is SwipeButton button)
            {
                if (button.IsDownSwiping)
                {
                    button.LeftSwipeProgress = (button.ContentY / (button.ActualHeight / 2d)).Coerce(1);
                    button.RightSwipeProgress = 1;
                }
                if (button.IsUpSwiping)
                {
                    button.RightSwipeProgress = (button.ContentY / (button.ActualHeight / 2d)).Coerce(1);
                    button.LeftSwipeProgress = 1;
                }
            }
        }

        static readonly DependencyPropertyKey IsAnimatingKey = DependencyProperty.RegisterReadOnly(nameof(IsAnimating), typeof(bool), typeof(SwipeButton), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsAnimatingProperty = IsAnimatingKey.DependencyProperty;
        public bool IsAnimating
        {
            get => (bool)GetValue(IsAnimatingProperty);
            private set => SetValue(IsAnimatingKey, value);
        }

        public static readonly DependencyProperty IsCheckableProperty = DependencyProperty.Register(nameof(IsCheckable), typeof(bool), typeof(SwipeButton), new FrameworkPropertyMetadata(false, OnIsCheckableChanged));
        public bool IsCheckable
        {
            get => (bool)GetValue(IsCheckableProperty);
            set => SetValue(IsCheckableProperty, value);
        }
        static void OnIsCheckableChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is SwipeButton button)
            {
                if (e.NewValue is bool isCheckable)
                {
                    if (!isCheckable)
                        button.SetCurrentValue(IsCheckedProperty, false);
                }
            }
        }

        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(SwipeButton), new FrameworkPropertyMetadata(false, OnIsCheckedChanged, OnIsCheckedCoerced));
        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }
        static object OnIsCheckedCoerced(DependencyObject sender, object input)
        {
            if (sender is SwipeButton button)
            {
                if (!button.IsCheckable)
                    return false;
            }
            return input;
        }
        static void OnIsCheckedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is SwipeButton button)
            {
                if (e.NewValue is bool isChecked)
                {
                    if (isChecked)
                        button.OnChecked();

                    if (!isChecked)
                        button.OnUnchecked();
                }
            }
        }

        static readonly DependencyPropertyKey IsDownSwipingKey = DependencyProperty.RegisterReadOnly(nameof(IsDownSwiping), typeof(bool), typeof(SwipeButton), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsDownSwipingProperty = IsDownSwipingKey.DependencyProperty;
        public bool IsDownSwiping
        {
            get => (bool)GetValue(IsDownSwipingProperty);
            private set => SetValue(IsDownSwipingKey, value);
        }

        static readonly DependencyPropertyKey IsLeftSwipingKey = DependencyProperty.RegisterReadOnly(nameof(IsLeftSwiping), typeof(bool), typeof(SwipeButton), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsLeftSwipingProperty = IsLeftSwipingKey.DependencyProperty;
        public bool IsLeftSwiping
        {
            get => (bool)GetValue(IsLeftSwipingProperty);
            private set => SetValue(IsLeftSwipingKey, value);
        }

        static readonly DependencyPropertyKey IsRightSwipingKey = DependencyProperty.RegisterReadOnly(nameof(IsRightSwiping), typeof(bool), typeof(SwipeButton), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsRightSwipingProperty = IsRightSwipingKey.DependencyProperty;
        public bool IsRightSwiping
        {
            get => (bool)GetValue(IsRightSwipingProperty);
            private set => SetValue(IsRightSwipingKey, value);
        }

        static readonly DependencyPropertyKey IsMouseDownKey = DependencyProperty.RegisterReadOnly(nameof(IsMouseDown), typeof(bool), typeof(SwipeButton), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsMouseDownProperty = IsMouseDownKey.DependencyProperty;
        public bool IsMouseDown
        {
            get => (bool)GetValue(IsMouseDownProperty);
            private set => SetValue(IsMouseDownKey, value);
        }

        static readonly DependencyPropertyKey IsSwipingKey = DependencyProperty.RegisterReadOnly(nameof(IsSwiping), typeof(bool), typeof(SwipeButton), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsSwipingProperty = IsSwipingKey.DependencyProperty;
        public bool IsSwiping
        {
            get => (bool)GetValue(IsSwipingProperty);
            private set => SetValue(IsSwipingKey, value);
        }

        static readonly DependencyPropertyKey IsUpSwipingKey = DependencyProperty.RegisterReadOnly(nameof(IsUpSwiping), typeof(bool), typeof(SwipeButton), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsUpSwipingProperty = IsUpSwipingKey.DependencyProperty;
        public bool IsUpSwiping
        {
            get => (bool)GetValue(IsUpSwipingProperty);
            private set => SetValue(IsUpSwipingKey, value);
        }

        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register(nameof(Left), typeof(object), typeof(SwipeButton), new FrameworkPropertyMetadata(null));
        public object Left
        {
            get => (object)GetValue(LeftProperty);
            set => SetValue(LeftProperty, value);
        }

        public static readonly DependencyProperty LeftSwipeCommandProperty = DependencyProperty.Register(nameof(LeftSwipeCommand), typeof(ICommand), typeof(SwipeButton), new FrameworkPropertyMetadata(null));
        public ICommand LeftSwipeCommand
        {
            get => (ICommand)GetValue(LeftSwipeCommandProperty);
            set => SetValue(LeftSwipeCommandProperty, value);
        }

        public static readonly DependencyProperty LeftSwipeCommandParameterProperty = DependencyProperty.Register(nameof(LeftSwipeCommandParameter), typeof(object), typeof(SwipeButton), new FrameworkPropertyMetadata(null));
        public object LeftSwipeCommandParameter
        {
            get => (object)GetValue(LeftSwipeCommandParameterProperty);
            set => SetValue(LeftSwipeCommandParameterProperty, value);
        }

        static readonly DependencyPropertyKey LeftSwipeProgressKey = DependencyProperty.RegisterReadOnly(nameof(LeftSwipeProgress), typeof(double), typeof(SwipeButton), new FrameworkPropertyMetadata(0d));
        public static readonly DependencyProperty LeftSwipeProgressProperty = LeftSwipeProgressKey.DependencyProperty;
        public double LeftSwipeProgress
        {
            get => (double)GetValue(LeftSwipeProgressProperty);
            set => SetValue(LeftSwipeProgressKey, value);
        }

        public static readonly DependencyProperty LeftTemplateProperty = DependencyProperty.Register(nameof(LeftTemplate), typeof(DataTemplate), typeof(SwipeButton), new FrameworkPropertyMetadata(null));
        public DataTemplate LeftTemplate
        {
            get => (DataTemplate)GetValue(LeftTemplateProperty);
            set => SetValue(LeftTemplateProperty, value);
        }

        public static readonly DependencyProperty LeftTemplateSelectorProperty = DependencyProperty.Register(nameof(LeftTemplateSelector), typeof(DataTemplateSelector), typeof(SwipeButton), new FrameworkPropertyMetadata(null));
        public DataTemplateSelector LeftTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(LeftTemplateSelectorProperty);
            set => SetValue(LeftTemplateSelectorProperty, value);
        }

        public static readonly DependencyProperty RightProperty = DependencyProperty.Register(nameof(Right), typeof(object), typeof(SwipeButton), new FrameworkPropertyMetadata(null));
        public object Right
        {
            get => (object)GetValue(RightProperty);
            set => SetValue(RightProperty, value);
        }

        public static readonly DependencyProperty RightSwipeCommandProperty = DependencyProperty.Register(nameof(RightSwipeCommand), typeof(ICommand), typeof(SwipeButton), new FrameworkPropertyMetadata(null));
        public ICommand RightSwipeCommand
        {
            get => (ICommand)GetValue(RightSwipeCommandProperty);
            set => SetValue(RightSwipeCommandProperty, value);
        }

        public static readonly DependencyProperty RightSwipeCommandParameterProperty = DependencyProperty.Register(nameof(RightSwipeCommandParameter), typeof(object), typeof(SwipeButton), new FrameworkPropertyMetadata(null));
        public object RightSwipeCommandParameter
        {
            get => (object)GetValue(RightSwipeCommandParameterProperty);
            set => SetValue(RightSwipeCommandParameterProperty, value);
        }

        static readonly DependencyPropertyKey RightSwipeProgressKey = DependencyProperty.RegisterReadOnly(nameof(RightSwipeProgress), typeof(double), typeof(SwipeButton), new FrameworkPropertyMetadata(0d));
        public static readonly DependencyProperty RightSwipeProgressProperty = RightSwipeProgressKey.DependencyProperty;
        public double RightSwipeProgress
        {
            get => (double)GetValue(RightSwipeProgressProperty);
            set => SetValue(RightSwipeProgressKey, value);
        }

        public static readonly DependencyProperty RightTemplateProperty = DependencyProperty.Register(nameof(RightTemplate), typeof(DataTemplate), typeof(SwipeButton), new FrameworkPropertyMetadata(null));
        public DataTemplate RightTemplate
        {
            get => (DataTemplate)GetValue(RightTemplateProperty);
            set => SetValue(RightTemplateProperty, value);
        }

        public static readonly DependencyProperty RightTemplateSelectorProperty = DependencyProperty.Register(nameof(RightTemplateSelector), typeof(DataTemplateSelector), typeof(SwipeButton), new FrameworkPropertyMetadata(null));
        public DataTemplateSelector RightTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(RightTemplateSelectorProperty);
            set => SetValue(RightTemplateSelectorProperty, value);
        }

        public static readonly DependencyProperty SwipeAnimationAccelerationProperty = DependencyProperty.Register(nameof(SwipeAnimationAcceleration), typeof(double), typeof(SwipeButton), new FrameworkPropertyMetadata(0.4));
        public double SwipeAnimationAcceleration
        {
            get => (double)GetValue(SwipeAnimationAccelerationProperty);
            set => SetValue(SwipeAnimationAccelerationProperty, value);
        }

        public static readonly DependencyProperty SwipeAnimationDecelerationProperty = DependencyProperty.Register(nameof(SwipeAnimationDeceleration), typeof(double), typeof(SwipeButton), new FrameworkPropertyMetadata(0.6));
        public double SwipeAnimationDeceleration
        {
            get => (double)GetValue(SwipeAnimationDecelerationProperty);
            set => SetValue(SwipeAnimationDecelerationProperty, value);
        }

        public static readonly DependencyProperty SwipeAnimationDurationProperty = DependencyProperty.Register(nameof(SwipeAnimationDuration), typeof(TimeSpan), typeof(SwipeButton), new FrameworkPropertyMetadata(0.8.Seconds()));
        public TimeSpan SwipeAnimationDuration
        {
            get => (TimeSpan)GetValue(SwipeAnimationDurationProperty);
            set => SetValue(SwipeAnimationDurationProperty, value);
        }

        public static readonly DependencyProperty SwipeCommandThresholdProperty = DependencyProperty.Register(nameof(SwipeCommandThreshold), typeof(One), typeof(SwipeButton), new FrameworkPropertyMetadata(new One(0.5)));
        [TypeConverter(typeof(OneTypeConverter))]
        public One SwipeCommandThreshold
        {
            get => (One)GetValue(SwipeCommandThresholdProperty);
            set => SetValue(SwipeCommandThresholdProperty, value);
        }

        public static readonly DependencyProperty SwipedCommandProperty = DependencyProperty.Register(nameof(SwipedCommand), typeof(ICommand), typeof(SwipeButton), new FrameworkPropertyMetadata(null));
        public ICommand SwipedCommand
        {
            get => (ICommand)GetValue(SwipedCommandProperty);
            set => SetValue(SwipedCommandProperty, value);
        }

        public static readonly DependencyProperty SwipedCommandParameterProperty = DependencyProperty.Register(nameof(SwipedCommandParameter), typeof(object), typeof(SwipeButton), new FrameworkPropertyMetadata(null));
        public object SwipedCommandParameter
        {
            get => GetValue(SwipedCommandParameterProperty);
            set => SetValue(SwipedCommandParameterProperty, value);
        }

        public static readonly DependencyProperty SwipeDirectionProperty = DependencyProperty.Register(nameof(SwipeDirection), typeof(SwipeButtonDirection), typeof(SwipeButton), new FrameworkPropertyMetadata(SwipeButtonDirection.Horizontal));
        public SwipeButtonDirection SwipeDirection
        {
            get => (SwipeButtonDirection)GetValue(SwipeDirectionProperty);
            set => SetValue(SwipeDirectionProperty, value);
        }

        public static readonly DependencyProperty SwipeStartLengthProperty = DependencyProperty.Register(nameof(SwipeStartLength), typeof(double), typeof(SwipeButton), new FrameworkPropertyMetadata(4.0));
        public double SwipeStartLength
        {
            get => (double)GetValue(SwipeStartLengthProperty);
            set => SetValue(SwipeStartLengthProperty, value);
        }

        public static readonly DependencyProperty SwipeModeProperty = DependencyProperty.Register(nameof(SwipeMode), typeof(SwipeButtonMode), typeof(SwipeButton), new FrameworkPropertyMetadata(SwipeButtonMode.Default));
        public SwipeButtonMode SwipeMode
        {
            get => (SwipeButtonMode)GetValue(SwipeModeProperty);
            set => SetValue(SwipeModeProperty, value);
        }

        #endregion

        #region SwipeButton

        public SwipeButton() : base() { }

        #endregion

        #region Methods

        Storyboard GetAnimation(double from, DependencyProperty targetProperty)
        {
            var animation = new DoubleAnimation()
            {
                AccelerationRatio = SwipeAnimationAcceleration,
                DecelerationRatio = SwipeAnimationDeceleration,
                Duration = new(SwipeAnimationDuration),
                From = from,
                To = 0,
            };
            Storyboard.SetTarget(animation, this);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(0)", targetProperty));

            var result = new Storyboard
            {
                FillBehavior = FillBehavior.Stop
            };
            result.Children.Add(animation);
            return result;
        }

        void OnAnimationCompleted(object sender, EventArgs e)
        {
            lastAnimation.Completed -= OnAnimationCompleted;
            lastAnimation = null;

            IsAnimating = false;
            IsDownSwiping = IsLeftSwiping = IsRightSwiping = IsUpSwiping = false;

            if (lastAction != null)
            {
                lastAction();
                lastAction = null;
            }

            ContentX = 0; ContentY = 0;
            LeftSwipeProgress = 0; RightSwipeProgress = 0;
        }

        //...

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            if (IsAnimating)
                return;

            aPoint = e.GetPosition(this);
            IsMouseDown = true;
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
            if (IsMouseDown)
            {
                bPoint = e.GetPosition(this);
                if (IsSwiping)
                {
                    switch (SwipeDirection)
                    {
                        case SwipeButtonDirection.Horizontal:
                            IsLeftSwiping
                                = ContentX >= 0;
                            IsRightSwiping
                                = ContentX < 0;

                            ContentX = bPoint.X - aPoint.X;
                            ContentY = 0;
                            break;
                        case SwipeButtonDirection.Vertical:
                            IsDownSwiping
                                = ContentY < 0;
                            IsUpSwiping
                                = ContentY >= 0;

                            ContentX = 0;
                            ContentY = bPoint.Y - aPoint.Y;
                            break;
                    }
                    OnSwiping();
                    return;
                }

                switch (SwipeDirection)
                {
                    case SwipeButtonDirection.Horizontal:
                        if ((aPoint.X - bPoint.X).Absolute() >= SwipeStartLength)
                            goto default;

                        break;

                    case SwipeButtonDirection.Vertical:
                        if ((aPoint.Y - bPoint.Y).Absolute() >= SwipeStartLength)
                            goto default;

                        break;

                    default:
                        IsSwiping = true;
                        OnSwipeStarted();
                        CaptureMouse();
                        break;
                }
            }
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);
            IsMouseDown = false;
            if (IsSwiping)
            {
                ReleaseMouseCapture();

                double from = 0;
                DependencyProperty targetProperty = null;

                switch (SwipeDirection)
                {
                    case SwipeButtonDirection.Horizontal:
                        from
                            = ContentX;
                        targetProperty
                            = ContentXProperty;

                        if (IsLeftSwiping)
                        {
                            if (LeftSwipeProgress >= SwipeCommandThreshold)
                                lastAction = new Action(() => LeftSwipeCommand?.Execute(LeftSwipeCommandParameter));
                        }
                        if (IsRightSwiping)
                        {
                            if (RightSwipeProgress >= SwipeCommandThreshold)
                                lastAction = new Action(() => RightSwipeCommand?.Execute(RightSwipeCommandParameter));
                        }
                        break;

                    case SwipeButtonDirection.Vertical:
                        from
                            = ContentY;
                        targetProperty
                            = ContentYProperty;

                        if (IsDownSwiping)
                        {
                            if (LeftSwipeProgress >= SwipeCommandThreshold)
                                lastAction = new Action(() => LeftSwipeCommand?.Execute(LeftSwipeCommandParameter));
                        }
                        if (IsUpSwiping)
                        {
                            if (RightSwipeProgress >= SwipeCommandThreshold)
                                lastAction = new Action(() => RightSwipeCommand?.Execute(RightSwipeCommandParameter));
                        }
                        break;
                }

                lastAnimation = GetAnimation(from, targetProperty);
                lastAnimation.Completed += OnAnimationCompleted;
                lastAnimation.Begin();

                aPoint = bPoint = default;
                IsAnimating = true; IsSwiping = false;

                OnSwipeEnded();
            }
            
            switch (ClickMode)
            {
                case ClickMode.Release:
                    Command?.Execute(CommandParameter);
                    if (IsCheckable)
                        SetCurrentValue(IsCheckedProperty, !IsChecked);

                    break;
            }
        }

        //...

        protected virtual void OnChecked()
        {
            RaiseEvent(new(CheckedEvent, this));
            CheckedCommand?.Execute(CheckedCommandParameter);
        }

        protected virtual void OnSwipeEnded()
        {
            RaiseEvent(new(SwipedEvent, this));
            SwipedCommand?.Execute(SwipedCommandParameter);
        }

        protected virtual void OnSwipeStarted() { }

        protected virtual void OnSwiping() { }

        protected virtual void OnUnchecked()
        {
            RaiseEvent(new(UncheckedEvent, this));
            UncheckedCommand?.Execute(UncheckedCommandParameter);
        }

        #endregion
    }
}