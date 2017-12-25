using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Shapes;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    [TemplatePart(Name = "PART_Dropdown", Type = typeof(ContentControl))]
    public class MaskedButton : Button
    {
        #region Properties

        ContentControl PART_Dropdown;

        Rectangle PART_Image;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ContentMarginProperty = DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(MaskedButton), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DropDownProperty = DependencyProperty.Register("DropDown", typeof(ContextMenu), typeof(MaskedButton), new UIPropertyMetadata(null, OnDropDownChanged));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DropDownAnimationProperty = DependencyProperty.Register("DropDownAnimation", typeof(PopupAnimation), typeof(MaskedButton), new FrameworkPropertyMetadata(PopupAnimation.Fade, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public PopupAnimation DropDownAnimation
        {
            get
            {
                return (PopupAnimation)GetValue(DropDownAnimationProperty);
            }
            set
            {
                SetValue(DropDownAnimationProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DropDownButtonToolTipProperty = DependencyProperty.Register("DropDownButtonToolTip", typeof(string), typeof(MaskedButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DropDownButtonVisibilityProperty = DependencyProperty.Register("DropDownButtonVisibility", typeof(Visibility), typeof(MaskedButton), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DropDownDataContextProperty = DependencyProperty.Register("DropDownDataContext", typeof(object), typeof(MaskedButton), new FrameworkPropertyMetadata(null, OnDropDownDataContextChanged));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DropDownPlacementProperty = DependencyProperty.Register("DropDownPlacement", typeof(PlacementMode), typeof(MaskedButton), new FrameworkPropertyMetadata(PlacementMode.Bottom, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public PlacementMode DropDownPlacement
        {
            get
            {
                return (PlacementMode)GetValue(DropDownPlacementProperty);
            }
            set
            {
                SetValue(DropDownPlacementProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(MaskedButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty IsRippleEnabledProperty = DependencyProperty.Register("IsRippleEnabled", typeof(bool), typeof(MaskedButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(MaskedButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSourceChanged));
        /// <summary>
        /// 
        /// </summary>
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
        static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<MaskedButton>().OnSourceChanged((ImageSource)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SourceColorProperty = DependencyProperty.Register("SourceColor", typeof(Brush), typeof(MaskedButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Brush SourceColor
        {
            get
            {
                return (Brush)GetValue(SourceColorProperty);
            }
            set
            {
                SetValue(SourceColorProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SourceHeightProperty = DependencyProperty.Register("SourceHeight", typeof(double), typeof(MaskedButton), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double SourceHeight
        {
            get
            {
                return (double)GetValue(SourceHeightProperty);
            }
            set
            {
                SetValue(SourceHeightProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SourceWidthProperty = DependencyProperty.Register("SourceWidth", typeof(double), typeof(MaskedButton), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public double SourceWidth
        {
            get
            {
                return (double)GetValue(SourceWidthProperty);
            }
            set
            {
                SetValue(SourceWidthProperty, value);
            }
        }

        #endregion

        #region MaskedButton

        /// <summary>
        /// 
        /// </summary>
        public MaskedButton() : base()
        {
            DefaultStyleKey = typeof(MaskedButton);
            Loaded += OnLoaded;
        }

        #endregion

        #region Methods

        #region Private

        void OnDropdownMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (DropDown != null)
                DropDown.IsOpen = true;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            OnLoaded(e);
        }

        void RefreshImage()
        {
            if (PART_Image != null)
                PART_Image.OpacityMask = new ImageBrush(Source);
        }
        
        #endregion

        #region Overrides

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Handled)
                return;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();

            PART_Dropdown = Template.FindName("PART_Dropdown", this) as ContentControl;

            if (PART_Dropdown != null)
                PART_Dropdown.MouseLeftButtonDown += OnDropdownMouseLeftButtonDown;

            PART_Image = Template.FindName("PART_Image", this) as Rectangle;
            RefreshImage();
        }

        #endregion

        #region Virtual

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnDropDownChanged(ContextMenu Value)
        {
            if (Value != null)
            {
                Value.Placement = PlacementMode.Bottom;
                Value.PlacementTarget = this;

                BindingOperations.SetBinding(Value, ContextMenu.DataContextProperty, new Binding()
                {
                    Mode = BindingMode.OneWay,
                    Path = new PropertyPath("DropDownDataContext"),
                    Source = this
                });
                BindingOperations.SetBinding(Value, ContextMenu.IsOpenProperty, new Binding()
                {
                    Path = new PropertyPath("IsChecked"),
                    Source = this
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnDropDownDataContextChanged(object Value)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnLoaded(RoutedEventArgs e)
        {
            SetCurrentValue(DropDownDataContextProperty, DataContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnSourceChanged(ImageSource Value)
        {
            RefreshImage();
        }

        #endregion

        #endregion
    }
}
