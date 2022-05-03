using Imagin.Common.Converters;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class ImageButton : Button
    {
        public static readonly ReferenceKey<ImageElement> ImageElementKey = new();

        public static readonly DependencyProperty ButtonMarginProperty = DependencyProperty.Register(nameof(ButtonMargin), typeof(Thickness), typeof(ImageButton), new FrameworkPropertyMetadata(default(Thickness)));
        public Thickness ButtonMargin
        {
            get => (Thickness)GetValue(ButtonMarginProperty);
            set => SetValue(ButtonMarginProperty, value);
        }

        public static readonly DependencyProperty ButtonSizeProperty = DependencyProperty.Register(nameof(ButtonSize), typeof(DoubleSize), typeof(ImageButton), new FrameworkPropertyMetadata(null));
        [TypeConverter(typeof(DoubleSizeTypeConverter))]
        public DoubleSize ButtonSize
        {
            get => (DoubleSize)GetValue(ButtonSizeProperty);
            set => SetValue(ButtonSizeProperty, value);
        }

        public static readonly DependencyProperty ButtonSourceProperty = DependencyProperty.Register(nameof(ButtonSource), typeof(ImageSource), typeof(ImageButton), new FrameworkPropertyMetadata(null));
        public ImageSource ButtonSource
        {
            get => (ImageSource)GetValue(ButtonSourceProperty);
            set => SetValue(ButtonSourceProperty, value);
        }

        public static readonly DependencyProperty ButtonToolTipProperty = DependencyProperty.Register(nameof(ButtonToolTip), typeof(string), typeof(ImageButton), new FrameworkPropertyMetadata(null));
        public string ButtonToolTip
        {
            get => (string)GetValue(ButtonToolTipProperty);
            set => SetValue(ButtonToolTipProperty, value);
        }

        public static readonly DependencyProperty ButtonVisibilityProperty = DependencyProperty.Register(nameof(ButtonVisibility), typeof(Visibility), typeof(ImageButton), new FrameworkPropertyMetadata(Visibility.Collapsed));
        public Visibility ButtonVisibility
        {
            get => (Visibility)GetValue(ButtonVisibilityProperty);
            set => SetValue(ButtonVisibilityProperty, value);
        }

        public static readonly DependencyProperty ContentMarginProperty = DependencyProperty.Register(nameof(ContentMargin), typeof(Thickness), typeof(ImageButton), new FrameworkPropertyMetadata(default(Thickness)));
        public Thickness ContentMargin
        {
            get => (Thickness)GetValue(ContentMarginProperty);
            set => SetValue(ContentMarginProperty, value);
        }

        public static readonly DependencyProperty ContentVisibilityProperty = DependencyProperty.Register(nameof(ContentVisibility), typeof(Visibility), typeof(ImageButton), new FrameworkPropertyMetadata(Visibility.Visible));
        public Visibility ContentVisibility
        {
            get => (Visibility)GetValue(ContentVisibilityProperty);
            set => SetValue(ContentVisibilityProperty, value);
        }

        public static readonly DependencyProperty HasMenuProperty = DependencyProperty.Register(nameof(HasMenu), typeof(bool), typeof(ImageButton), new FrameworkPropertyMetadata(false));
        public bool HasMenu
        {
            get => (bool)GetValue(HasMenuProperty);
            set => SetValue(HasMenuProperty, value);
        }

        public static readonly DependencyProperty ImageForegroundProperty = ImageElement.ForegroundProperty.AddOwner(typeof(ImageButton), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush, FrameworkPropertyMetadataOptions.Inherits));
        public Brush ImageForeground
        {
            get => (Brush)GetValue(ImageForegroundProperty);
            set => SetValue(ImageForegroundProperty, value);
        }

        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof(IsChecked), typeof(bool), typeof(ImageButton), new FrameworkPropertyMetadata(false));
        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        public static readonly DependencyProperty MenuProperty = DependencyProperty.Register(nameof(Menu), typeof(ContextMenu), typeof(ImageButton), new FrameworkPropertyMetadata(null, OnMenuChanged));
        public ContextMenu Menu
        {
            get => (ContextMenu)GetValue(MenuProperty);
            set => SetValue(MenuProperty, value);
        }
        static void OnMenuChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<ImageButton>().OnMenuChanged(new Value<ContextMenu>(e));

        public static readonly DependencyProperty MenuAnimationProperty = DependencyProperty.Register(nameof(MenuAnimation), typeof(PopupAnimation), typeof(ImageButton), new FrameworkPropertyMetadata(PopupAnimation.Fade));
        public PopupAnimation MenuAnimation
        {
            get => (PopupAnimation)GetValue(MenuAnimationProperty);
            set => SetValue(MenuAnimationProperty, value);
        }

        public static readonly DependencyProperty MenuPlacementProperty = DependencyProperty.Register(nameof(MenuPlacement), typeof(PlacementMode), typeof(ImageButton), new FrameworkPropertyMetadata(PlacementMode.Bottom));
        public PlacementMode MenuPlacement
        {
            get => (PlacementMode)GetValue(MenuPlacementProperty);
            set => SetValue(MenuPlacementProperty, value);
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(ImageButton), new FrameworkPropertyMetadata(null));
        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public static readonly DependencyProperty SourceSizeProperty = DependencyProperty.Register(nameof(SourceSize), typeof(DoubleSize), typeof(ImageButton), new FrameworkPropertyMetadata(null));
        [TypeConverter(typeof(DoubleSizeTypeConverter))]
        public DoubleSize SourceSize
        {
            get => (DoubleSize)GetValue(SourceSizeProperty);
            set => SetValue(SourceSizeProperty, value);
        }

        //...

        public ImageButton() : base()
            => this.RegisterHandler(i => this.GetChild(ImageElementKey).If(j => j.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown), i => this.GetChild(ImageElementKey).If(j => j.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown));

        //...

        void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            SetCurrentValue(IsCheckedProperty, true);
        }

        //...

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