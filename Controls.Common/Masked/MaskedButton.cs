using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Data;

namespace Imagin.Controls.Common
{
    [TemplatePart(Name = "PART_Dropdown", Type = typeof(ContentControl))]
    public class MaskedButton : Button
    {
        #region Properties

        ContentControl PART_Dropdown { get; set; } = null;

        public static DependencyProperty ContentMarginProperty = DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(MaskedButton), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Thickness ContentMargin
        {
            get
            {
                return (Thickness)GetValue(ContentMarginProperty);
            }
            set
            {
                SetValue(ContentMarginProperty, value);
            }
        }

        public static readonly DependencyProperty DropDownProperty = DependencyProperty.Register("DropDown", typeof(ContextMenu), typeof(MaskedButton), new UIPropertyMetadata(null, OnDropDownChanged));
        public ContextMenu DropDown
        {
            get
            {
                return (ContextMenu)GetValue(DropDownProperty);
            }
            set
            {
                SetValue(DropDownProperty, value);
            }
        }
        static void OnDropDownChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<MaskedButton>().OnDropDownChanged((ContextMenu)e.NewValue);
        }

        public static DependencyProperty DropDownDataContextProperty = DependencyProperty.Register("DropDownDataContext", typeof(object), typeof(MaskedButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDropDownDataContextChanged));
        public object DropDownDataContext
        {
            get
            {
                return (object)GetValue(DropDownDataContextProperty);
            }
            set
            {
                SetValue(DropDownDataContextProperty, value);
            }
        }
        static void OnDropDownDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<MaskedButton>().OnDropDownDataContextChanged(e.NewValue);
        }

        public static DependencyProperty DropDownButtonToolTipProperty = DependencyProperty.Register("DropDownButtonToolTip", typeof(string), typeof(MaskedButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string DropDownButtonToolTip
        {
            get
            {
                return (string)GetValue(DropDownButtonToolTipProperty);
            }
            set
            {
                SetValue(DropDownButtonToolTipProperty, value);
            }
        }

        public static DependencyProperty DropDownButtonVisibilityProperty = DependencyProperty.Register("DropDownButtonVisibility", typeof(Visibility), typeof(MaskedButton), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Visibility DropDownButtonVisibility
        {
            get
            {
                return (Visibility)GetValue(DropDownButtonVisibilityProperty);
            }
            set
            {
                SetValue(DropDownButtonVisibilityProperty, value);
            }
        }

        public static DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(MaskedButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool IsChecked
        {
            get
            {
                return (bool)GetValue(IsCheckedProperty);
            }
            set
            {
                SetValue(IsCheckedProperty, value);
            }
        }

        public static DependencyProperty IsRippleEnabledProperty = DependencyProperty.Register("IsRippleEnabled", typeof(bool), typeof(MaskedButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool IsRippleEnabled
        {
            get
            {
                return (bool)GetValue(IsRippleEnabledProperty);
            }
            set
            {
                SetValue(IsRippleEnabledProperty, value);
            }
        }

        public static DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(MaskedButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSourceChanged));
        public ImageSource Source
        {
            get
            {
                return (ImageSource)GetValue(SourceProperty);
            }
            set
            {
                SetValue(SourceProperty, value);
            }
        }
        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<MaskedButton>().OnSourceChanged((ImageSource)e.NewValue);
        }

        public static DependencyProperty ImageBrushProperty = DependencyProperty.Register("ImageBrush", typeof(ImageBrush), typeof(MaskedButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ImageBrush ImageBrush
        {
            get
            {
                return (ImageBrush)GetValue(ImageBrushProperty);
            }
            set
            {
                SetValue(ImageBrushProperty, value);
            }
        }

        public static readonly DependencyProperty ImageColorProperty = DependencyProperty.Register("ImageColor", typeof(Brush), typeof(MaskedButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush ImageColor
        {
            get
            {
                return (Brush)GetValue(ImageColorProperty);
            }
            set
            {
                SetValue(ImageColorProperty, value);
            }
        }

        public static DependencyProperty ImageWidthProperty = DependencyProperty.Register("ImageWidth", typeof(double), typeof(MaskedButton), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double ImageWidth
        {
            get
            {
                return (double)GetValue(ImageWidthProperty);
            }
            set
            {
                SetValue(ImageWidthProperty, value);
            }
        }

        public static DependencyProperty ImageHeightProperty = DependencyProperty.Register("ImageHeight", typeof(double), typeof(MaskedButton), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public double ImageHeight
        {
            get
            {
                return (double)GetValue(ImageHeightProperty);
            }
            set
            {
                SetValue(ImageHeightProperty, value);
            }
        }

        #endregion

        #region MaskedButton

        public MaskedButton() : base()
        {
            DefaultStyleKey = typeof(MaskedButton);
        }

        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();

            PART_Dropdown = Template.FindName("PART_Dropdown", this).As<ContentControl>();

            if (PART_Dropdown != null)
                PART_Dropdown.MouseLeftButtonDown += OnDropdownMouseLeftButtonDown;
        }

        #endregion

        #region Methods

        void OnDropdownMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (DropDown != null)
                DropDown.IsOpen = true;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Handled)
                return;
        }
        
        protected virtual void OnDropDownChanged(ContextMenu Value)
        {
            if (Value != null)
            {
                Value.DataContext = DropDownDataContext;
                Value.Placement = PlacementMode.Bottom;
                Value.PlacementTarget = this;

                BindingOperations.SetBinding(Value, ContextMenu.IsOpenProperty, new Binding()
                {
                    Path = new PropertyPath("IsChecked"),
                    Source = this
                });
            }
        }

        protected virtual void OnDropDownDataContextChanged(object Value)
        {
            if (DropDown != null)
                DropDown.DataContext = Value;
        }

        protected virtual void OnSourceChanged(ImageSource Value)
        {
            ImageBrush = new ImageBrush(Value);
        }

        #endregion
    }
}
