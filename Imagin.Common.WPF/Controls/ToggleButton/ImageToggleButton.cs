using Imagin.Common.Converters;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class ImageToggleButton : ToggleButton
    {
        public static readonly DependencyProperty ButtonMarginProperty = DependencyProperty.Register(nameof(ButtonMargin), typeof(Thickness), typeof(ImageToggleButton), new FrameworkPropertyMetadata(default(Thickness)));
        public Thickness ButtonMargin
        {
            get => (Thickness)GetValue(ButtonMarginProperty);
            set => SetValue(ButtonMarginProperty, value);
        }

        public static readonly DependencyProperty ButtonSizeProperty = DependencyProperty.Register(nameof(ButtonSize), typeof(DoubleSize), typeof(ImageToggleButton), new FrameworkPropertyMetadata(null));
        [TypeConverter(typeof(DoubleSizeTypeConverter))]
        public DoubleSize ButtonSize
        {
            get => (DoubleSize)GetValue(ButtonSizeProperty);
            set => SetValue(ButtonSizeProperty, value);
        }

        public static readonly DependencyProperty ButtonSourceProperty = DependencyProperty.Register(nameof(ButtonSource), typeof(ImageSource), typeof(ImageToggleButton), new FrameworkPropertyMetadata(null));
        public ImageSource ButtonSource
        {
            get => (ImageSource)GetValue(ButtonSourceProperty);
            set => SetValue(ButtonSourceProperty, value);
        }

        public static readonly DependencyProperty ButtonToolTipProperty = DependencyProperty.Register(nameof(ButtonToolTip), typeof(string), typeof(ImageToggleButton), new FrameworkPropertyMetadata(null));
        public string ButtonToolTip
        {
            get => (string)GetValue(ButtonToolTipProperty);
            set => SetValue(ButtonToolTipProperty, value);
        }

        public static readonly DependencyProperty ButtonVisibilityProperty = DependencyProperty.Register(nameof(ButtonVisibility), typeof(Visibility), typeof(ImageToggleButton), new FrameworkPropertyMetadata(Visibility.Collapsed));
        public Visibility ButtonVisibility
        {
            get => (Visibility)GetValue(ButtonVisibilityProperty);
            set => SetValue(ButtonVisibilityProperty, value);
        }

        public static readonly DependencyProperty CheckedContentProperty = DependencyProperty.Register(nameof(CheckedContent), typeof(object), typeof(ImageToggleButton), new FrameworkPropertyMetadata(null));
        public object CheckedContent
        {
            get => GetValue(CheckedContentProperty);
            set => SetValue(CheckedContentProperty, value);
        }

        public static readonly DependencyProperty CheckedSourceProperty = DependencyProperty.Register(nameof(CheckedSource), typeof(ImageSource), typeof(ImageToggleButton), new FrameworkPropertyMetadata(null));
        public ImageSource CheckedSource
        {
            get => (ImageSource)GetValue(CheckedSourceProperty);
            set => SetValue(CheckedSourceProperty, value);
        }

        public static readonly DependencyProperty CheckedToolTipProperty = DependencyProperty.Register(nameof(CheckedToolTip), typeof(object), typeof(ImageToggleButton), new FrameworkPropertyMetadata(null));
        public object CheckedToolTip
        {
            get => GetValue(CheckedToolTipProperty);
            set => SetValue(CheckedToolTipProperty, value);
        }

        public static readonly DependencyProperty ContentMarginProperty = DependencyProperty.Register(nameof(ContentMargin), typeof(Thickness), typeof(ImageToggleButton), new FrameworkPropertyMetadata(default(Thickness)));
        public Thickness ContentMargin
        {
            get => (Thickness)GetValue(ContentMarginProperty);
            set => SetValue(ContentMarginProperty, value);
        }

        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register(nameof(GroupName), typeof(string), typeof(ImageToggleButton), new FrameworkPropertyMetadata(null));
        public string GroupName
        {
            get => (string)GetValue(GroupNameProperty);
            set => SetValue(GroupNameProperty, value);
        }

        public static readonly DependencyProperty HasMenuProperty = DependencyProperty.Register(nameof(HasMenu), typeof(bool), typeof(ImageToggleButton), new FrameworkPropertyMetadata(false));
        public bool HasMenu
        {
            get => (bool)GetValue(HasMenuProperty);
            set => SetValue(HasMenuProperty, value);
        }

        public static readonly DependencyProperty ImageForegroundProperty = ImageElement.ForegroundProperty.AddOwner(typeof(ImageToggleButton), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush, FrameworkPropertyMetadataOptions.Inherits));
        public Brush ImageForeground
        {
            get => (Brush)GetValue(ImageForegroundProperty);
            set => SetValue(ImageForegroundProperty, value);
        }

        public static readonly DependencyProperty MenuProperty = DependencyProperty.Register(nameof(Menu), typeof(ContextMenu), typeof(ImageToggleButton), new FrameworkPropertyMetadata(null, OnMenuChanged));
        public ContextMenu Menu
        {
            get => (ContextMenu)GetValue(MenuProperty);
            set => SetValue(MenuProperty, value);
        }
        static void OnMenuChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<ImageToggleButton>().OnMenuChanged(new Value<ContextMenu>(e));

        public static readonly DependencyProperty MenuAnimationProperty = DependencyProperty.Register(nameof(MenuAnimation), typeof(PopupAnimation), typeof(ImageToggleButton), new FrameworkPropertyMetadata(PopupAnimation.Fade));
        public PopupAnimation MenuAnimation
        {
            get => (PopupAnimation)GetValue(MenuAnimationProperty);
            set => SetValue(MenuAnimationProperty, value);
        }

        public static readonly DependencyProperty MenuPlacementProperty = DependencyProperty.Register(nameof(MenuPlacement), typeof(PlacementMode), typeof(ImageToggleButton), new FrameworkPropertyMetadata(PlacementMode.Bottom));
        public PlacementMode MenuPlacement
        {
            get => (PlacementMode)GetValue(MenuPlacementProperty);
            set => SetValue(MenuPlacementProperty, value);
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(ImageToggleButton), new FrameworkPropertyMetadata(default(ImageSource)));
        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty SourceSizeProperty = DependencyProperty.Register(nameof(SourceSize), typeof(DoubleSize), typeof(ImageToggleButton), new FrameworkPropertyMetadata(null));
        [TypeConverter(typeof(DoubleSizeTypeConverter))]
        public DoubleSize SourceSize
        {
            get => (DoubleSize)GetValue(SourceSizeProperty);
            set => SetValue(SourceSizeProperty, value);
        }

        //...

        public ImageToggleButton() : base() { }

        //...

        protected override void OnChecked(RoutedEventArgs e)
        {
            return;
            base.OnChecked(e);
            if (!GroupName.NullOrEmpty())
            {
                Try.Invoke(() =>
                {
                    var parent = this.FindParent<DependencyObject>();
                    for (int i = 0, Count = VisualTreeHelper.GetChildrenCount(parent); i < Count; i++)
                    {
                        if (VisualTreeHelper.GetChild(parent, i) is ImageToggleButton j)
                        {
                            if (j != this)
                                j.SetCurrentValue(IsCheckedProperty, false);
                        }
                    }
                });
            }
        }

        protected virtual void OnMenuChanged(Value<ContextMenu> input)
        {
            if (input.New != null)
            {
                input.New.Placement 
                    = MenuPlacement;
                input.New.PlacementTarget
                    = this;
                input.New.Bind(ContextMenu.IsOpenProperty, nameof(IsChecked), this, BindingMode.TwoWay);
            }
            SetCurrentValue(HasMenuProperty, input.New is not null);
        }
    }
}