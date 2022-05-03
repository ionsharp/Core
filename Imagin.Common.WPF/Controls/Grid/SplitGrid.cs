using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media.Animation;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class SplitGrid : Grid
    {
        readonly Handle handle = false;

        public const Orientation DefaultOrientation = Orientation.Vertical;

        #region Properties

        readonly GridSplitter Splitter;

        //...

        public event EventHandler<EventArgs> Aligned;

        public event EventHandler<EventArgs> Collapsed;

        public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register(nameof(Buttons), typeof(FrameworkElementCollection), typeof(SplitGrid), new FrameworkPropertyMetadata(null));
        public FrameworkElementCollection Buttons
        {
            get => (FrameworkElementCollection)GetValue(ButtonsProperty);
            set => SetValue(ButtonsProperty, value);
        }

        public static readonly DependencyProperty ButtonSpacingProperty = DependencyProperty.Register(nameof(ButtonSpacing), typeof(Thickness), typeof(SplitGrid), new FrameworkPropertyMetadata(default(Thickness)));
        public Thickness ButtonSpacing
        {
            get => (Thickness)GetValue(ButtonSpacingProperty);
            set => SetValue(ButtonSpacingProperty, value);
        }
        
        public static readonly DependencyProperty CanAlignProperty = DependencyProperty.Register(nameof(CanAlign), typeof(bool), typeof(SplitGrid), new FrameworkPropertyMetadata(true));
        public bool CanAlign
        {
            get => (bool)GetValue(CanAlignProperty);
            set => SetValue(CanAlignProperty, value);
        }

        public static readonly DependencyProperty CanCollapseProperty = DependencyProperty.Register(nameof(CanCollapse), typeof(bool), typeof(SplitGrid), new FrameworkPropertyMetadata(true));
        public bool CanCollapse
        {
            get => (bool)GetValue(CanCollapseProperty);
            set => SetValue(CanCollapseProperty, value);
        }

        public static readonly DependencyProperty CanOrientationChangeProperty = DependencyProperty.Register(nameof(CanOrientationChange), typeof(bool), typeof(SplitGrid), new FrameworkPropertyMetadata(true));
        public bool CanOrientationChange
        {
            get => (bool)GetValue(CanOrientationChangeProperty);
            set => SetValue(CanOrientationChangeProperty, value);
        }

        public static readonly DependencyProperty CanSwapProperty = DependencyProperty.Register(nameof(CanSwap), typeof(bool), typeof(SplitGrid), new FrameworkPropertyMetadata(true));
        public bool CanSwap
        {
            get => (bool)GetValue(CanSwapProperty);
            set => SetValue(CanSwapProperty, value);
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(SplitGrid), new FrameworkPropertyMetadata(DefaultOrientation, FrameworkPropertyMetadataOptions.AffectsRender, OnOrientationChanged));
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }
        static void OnOrientationChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<SplitGrid>().OnOrientationChanged(new Value<Orientation>(e).New);

        public static readonly DependencyProperty Panel1Property = DependencyProperty.Register(nameof(Panel1), typeof(UIElement), typeof(SplitGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnPanel1Changed));
        public UIElement Panel1
        {
            get => (UIElement)GetValue(Panel1Property);
            set => SetValue(Panel1Property, value);
        }
        static void OnPanel1Changed(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<SplitGrid>().OnPanel1Changed(new Value<UIElement>(e));

        public static readonly DependencyProperty Panel1LengthProperty = DependencyProperty.Register(nameof(Panel1Length), typeof(GridLength), typeof(SplitGrid), new FrameworkPropertyMetadata(new GridLength(1, GridUnitType.Star)));
        public GridLength Panel1Length
        {
            get => (GridLength)GetValue(Panel1LengthProperty);
            set => SetValue(Panel1LengthProperty, value);
        }

        public static readonly DependencyProperty Panel2Property = DependencyProperty.Register(nameof(Panel2), typeof(UIElement), typeof(SplitGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnPanel2Changed));
        public UIElement Panel2
        {
            get => (UIElement)GetValue(Panel2Property);
            set => SetValue(Panel2Property, value);
        }
        static void OnPanel2Changed(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<SplitGrid>().OnPanel2Changed(new Value<UIElement>(e));

        public static readonly DependencyProperty Panel2LengthProperty = DependencyProperty.Register(nameof(Panel2Length), typeof(GridLength), typeof(SplitGrid), new FrameworkPropertyMetadata(new GridLength(1, GridUnitType.Star)));
        public GridLength Panel2Length
        {
            get => (GridLength)GetValue(Panel2LengthProperty);
            set => SetValue(Panel2LengthProperty, value);
        }

        public static readonly DependencyProperty ReverseProperty = DependencyProperty.Register(nameof(Reverse), typeof(bool), typeof(SplitGrid), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));
        public bool Reverse
        {
            get => (bool)GetValue(ReverseProperty);
            set => SetValue(ReverseProperty, value);
        }

        public static readonly DependencyProperty ShowPanel1Property = DependencyProperty.Register(nameof(ShowPanel1), typeof(bool), typeof(SplitGrid), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));
        public bool ShowPanel1
        {
            get => (bool)GetValue(ShowPanel1Property);
            set => SetValue(ShowPanel1Property, value);
        }

        public static readonly DependencyProperty ShowPanel2Property = DependencyProperty.Register(nameof(ShowPanel2), typeof(bool), typeof(SplitGrid), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender));
        public bool ShowPanel2
        {
            get => (bool)GetValue(ShowPanel2Property);
            set => SetValue(ShowPanel2Property, value);
        }

        public static readonly DependencyProperty SplitterStyleProperty = DependencyProperty.Register(nameof(SplitterStyle), typeof(Style), typeof(SplitGrid), new FrameworkPropertyMetadata(null));
        public Style SplitterStyle
        {
            get => (Style)GetValue(SplitterStyleProperty);
            set => SetValue(SplitterStyleProperty, value);
        }

        #endregion

        #region SplitGrid

        public SplitGrid()
        {
            SetCurrentValue(ButtonsProperty, new FrameworkElementCollection());

            RowDefinitions.Add(new RowDefinition());
            RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            RowDefinitions.Add(new RowDefinition());

            RowDefinitions[0].Bind(RowDefinition.HeightProperty, nameof(Panel1Length), this, System.Windows.Data.BindingMode.TwoWay);
            RowDefinitions[2].Bind(RowDefinition.HeightProperty, nameof(Panel2Length), this, System.Windows.Data.BindingMode.TwoWay);

            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            ColumnDefinitions.Add(new ColumnDefinition());

            ColumnDefinitions[0].Bind(ColumnDefinition.WidthProperty, nameof(Panel1Length), this, System.Windows.Data.BindingMode.TwoWay);
            ColumnDefinitions[2].Bind(ColumnDefinition.WidthProperty, nameof(Panel2Length), this, System.Windows.Data.BindingMode.TwoWay);

            Splitter = new();
            Splitter.Bind(GridSplitter.StyleProperty, nameof(SplitterStyle), this);
            Children.Add(Splitter);
        }

        #endregion

        #region Methods

        protected override void OnRender(DrawingContext input)
        {
            base.OnRender(input);
            if (Panel1 is null || Panel2 is null)
                return;

            if (ShowPanel1)
            {
                if (ShowPanel2)
                {
                    Splitter.Visibility
                        = Visibility.Visible;
                    Panel1.Visibility
                        = Visibility.Visible;
                    Panel2.Visibility
                        = Visibility.Visible;

                    Range<int> index = Reverse ? new(0, 2) : new(2, 0);
                    switch (Orientation)
                    {
                        case Orientation.Horizontal:
                            SetRow(Panel1, index.Minimum);
                            SetRow(Panel2, index.Maximum);

                            SetColumn(Panel1, 0);
                            SetColumnSpan(Panel1, 3);
                            SetRowSpan(Panel1, 1);

                            SetColumn(Panel2, 0);
                            SetColumnSpan(Panel2, 3);
                            SetRowSpan(Panel2, 1);

                            SetColumn(Splitter, 0);
                            SetColumnSpan(Splitter, 3);
                            SetRow(Splitter, 1);
                            SetRowSpan(Splitter, 1);

                            Splitter.ResizeDirection = GridResizeDirection.Rows;
                            break;

                        case Orientation.Vertical:
                            SetColumn(Panel1, index.Minimum);
                            SetColumn(Panel2, index.Maximum);

                            SetColumnSpan(Panel1, 1);
                            SetRow(Panel1, 0);
                            SetRowSpan(Panel1, 3);

                            SetColumnSpan(Panel2, 1);
                            SetRow(Panel2, 0);
                            SetRowSpan(Panel2, 3);

                            SetColumn(Splitter, 1);
                            SetColumnSpan(Splitter, 1);
                            SetRow(Splitter, 0);
                            SetRowSpan(Splitter, 3);

                            Splitter.ResizeDirection = GridResizeDirection.Columns;
                            break;
                    }
                }
                else
                {
                    Splitter.Visibility
                        = Visibility.Collapsed;
                    Panel1.Visibility
                        = Visibility.Visible;
                    Panel2.Visibility
                        = Visibility.Collapsed;

                    SetColumn(Panel1, 0);
                    SetColumnSpan(Panel1, 3);

                    SetRow(Panel1, 0);
                    SetRowSpan(Panel1, 3);
                }
            }
            else
            {
                if (!ShowPanel2)
                {
                    Splitter.Visibility 
                        = Visibility.Collapsed;
                    Panel1.Visibility 
                        = Visibility.Collapsed;
                    Panel2.Visibility 
                        = Visibility.Collapsed;
                }
                else
                {
                    Splitter.Visibility
                        = Visibility.Collapsed;
                    Panel1.Visibility
                        = Visibility.Collapsed;
                    Panel2.Visibility
                        = Visibility.Visible;

                    SetColumn(Panel2, 0);
                    SetColumnSpan(Panel2, 3);

                    SetRow(Panel2, 0);
                    SetRowSpan(Panel2, 3);
                }
            }
        }

        //...

        protected virtual void OnAligned()
        {
            var a = new GridLength(1, GridUnitType.Star);
            var b = new GridLength(1, GridUnitType.Star);

            if (Orientation == Orientation.Horizontal)
            {
                RowDefinitions[0].Height = a;
                RowDefinitions[2].Height = b;
            }
            else
            {
                ColumnDefinitions[0].Width = a;
                ColumnDefinitions[2].Width = b;
            }

            Aligned?.Invoke(this, new EventArgs());
        }

        protected virtual void OnCollapsed(bool LeftOrTop)
        {
            var Animation = new GridLengthAnimation()
            {
                From = Panel1Length,
                To = new GridLength(0, GridUnitType.Star),
                Duration = TimeSpan.FromSeconds(0.5),
                AccelerationRatio = 0.4,
                DecelerationRatio = 0.4
            };

            if (LeftOrTop)
            {
                BeginAnimation(Panel1LengthProperty, Animation);
            }
            else BeginAnimation(Panel1LengthProperty, Animation);

            Collapsed?.Invoke(this, new EventArgs());
        }

        protected virtual async void OnOrientationChanged(Orientation orientation)
        {
            Opacity = 0;
            await this.FadeIn();
        }

        protected virtual void OnPanel1Changed(Value<UIElement> input)
        {
            if (input.Old is UIElement i)
                Children.Remove(i);

            if (input.New is UIElement j)
                Children.Add(j);
        }

        protected virtual void OnPanel2Changed(Value<UIElement> input)
        {
            if (input.Old is UIElement i)
                Children.Remove(i);

            if (input.New is UIElement j)
                Children.Add(j);
        }

        async void Swap() => await handle.SafeInvokeAsync(async () =>
        {
            Opacity = 0;
            Reverse = !Reverse;
            await this.FadeIn();
        });

        ICommand alignCommand;
        public ICommand AlignCommand => alignCommand ??= new RelayCommand(() => OnAligned(), () => CanAlign);

        ICommand collapseCommand;
        public ICommand CollapseCommand => collapseCommand ??= new RelayCommand<object>(p => OnCollapsed(p.ToString().Boolean().Value), p => CanCollapse && p != null);

        ICommand setOrientationCommand;
        public ICommand SetOrientationCommand => setOrientationCommand ??= new RelayCommand<Orientation>(i => OnOrientationChanged(i), i => CanOrientationChange);

        ICommand swapCommand;
        public ICommand SwapCommand => swapCommand ??= new RelayCommand(Swap, () => CanSwap);

        #endregion
    }
}