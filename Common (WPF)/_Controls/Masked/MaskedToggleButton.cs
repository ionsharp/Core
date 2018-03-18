using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [TemplatePart(Name = "PART_DropDownButton", Type = typeof(MaskedImage))]
    public class MaskedToggleButton : ToggleButton
    {
        #region Properties

        MaskedImage PART_DropDownButton;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CheckedContentProperty = DependencyProperty.Register("CheckedContent", typeof(object), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public object CheckedContent
        {
            get
            {
                return GetValue(CheckedContentProperty);
            }
            set
            {
                SetValue(CheckedContentProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CheckedSourceProperty = DependencyProperty.Register("CheckedSource", typeof(ImageSource), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(default(ImageSource), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public ImageSource CheckedSource
        {
            get
            {
                return (ImageSource)GetValue(CheckedSourceProperty);
            }
            set
            {
                SetValue(CheckedSourceProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CheckedToolTipProperty = DependencyProperty.Register("CheckedToolTip", typeof(string), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public string CheckedToolTip
        {
            get
            {
                return (string)GetValue(CheckedToolTipProperty);
            }
            set
            {
                SetValue(CheckedToolTipProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ContentMarginProperty = DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static readonly DependencyProperty DropDownProperty = DependencyProperty.Register("DropDown", typeof(ContextMenu), typeof(MaskedToggleButton), new UIPropertyMetadata(null, OnDropDownChanged));
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
            d.As<MaskedToggleButton>().OnDropDownChanged((ContextMenu)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DropDownAnimationProperty = DependencyProperty.Register("DropDownAnimation", typeof(PopupAnimation), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(PopupAnimation.Fade, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty DropDownButtonToolTipProperty = DependencyProperty.Register("DropDownButtonToolTip", typeof(string), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty DropDownButtonVisibilityProperty = DependencyProperty.Register("DropDownButtonVisibility", typeof(Visibility), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty DropDownDataContextProperty = DependencyProperty.Register("DropDownDataContext", typeof(object), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(null));
        /// <summary>
        /// 
        /// </summary>
        public object DropDownDataContext
        {
            get
            {
                return GetValue(DropDownDataContextProperty);
            }
            set
            {
                SetValue(DropDownDataContextProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty DropDownPlacementProperty = DependencyProperty.Register("DropDownPlacement", typeof(PlacementMode), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(PlacementMode.Bottom, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty DropDownVisibilityProperty = DependencyProperty.Register("DropDownVisibility", typeof(Visibility), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Visibility DropDownVisibility
        {
            get
            {
                return (Visibility)GetValue(DropDownVisibilityProperty);
            }
            set
            {
                SetValue(DropDownVisibilityProperty, value);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty GroupNameProperty = DependencyProperty.Register("GroupName", typeof(string), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGroupNameChanged));
        /// <summary>
        /// 
        /// </summary>
        public string GroupName
        {
            get
            {
                return (string)GetValue(GroupNameProperty);
            }
            set
            {
                SetValue(GroupNameProperty, value);
            }
        }
        static void OnGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<MaskedToggleButton>().OnGroupNameChanged((string)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty IsRippleEnabledProperty = DependencyProperty.Register("IsRippleEnabled", typeof(bool), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(default(ImageSource), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SourceColorProperty = DependencyProperty.Register("SourceColor", typeof(Brush), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty SourceHeightProperty = DependencyProperty.Register("SourceHeight", typeof(double), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty SourceWidthProperty = DependencyProperty.Register("SourceWidth", typeof(double), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        #region MaskedToggleButton

        /// <summary>
        /// 
        /// </summary>
        public MaskedToggleButton()
        {
            DefaultStyleKey = typeof(MaskedToggleButton);
            SetCurrentValue(ContentMarginProperty, new Thickness(5, 0, 0, 0));
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnChecked(object sender, EventArgs e)
        {
            try
            {
                var Parent = this.GetParent<DependencyObject>();

                for (int i = 0, Count = VisualTreeHelper.GetChildrenCount(Parent); i < Count; i++)
                {
                    var j = VisualTreeHelper.GetChild(Parent, i) as MaskedToggleButton;

                    if (j != null && j != this)
                        j.IsChecked = false;
                }
            }
            catch
            {
                //Do nothing!
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();

            PART_DropDownButton = Template.FindName("PART_DropDownButton", this) as MaskedImage;

            if (PART_DropDownButton != null)
                PART_DropDownButton.MouseLeftButtonDown += OnDropDownButtonMouseLeftButtonDown; ;
        }

        void OnDropDownButtonMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            DropDown.IfNotNull(i => i.IsOpen = true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnDropDownChanged(ContextMenu value)
        {
            if (value != null)
            {
                value.Placement = PlacementMode.Bottom;
                value.PlacementTarget = this;

                BindingOperations.SetBinding(value, ContextMenu.DataContextProperty, new Binding()
                {
                    Mode = BindingMode.OneWay,
                    Path = new PropertyPath("DropDownDataContext"),
                    Source = this
                });

                BindingOperations.SetBinding(value, ContextMenu.IsOpenProperty, new Binding()
                {
                    Converter = new Imagin.Common.Converters.BooleanToVisibilityConverter(),
                    Path = new PropertyPath("DropDownVisibility"),
                    Source = this
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnGroupNameChanged(string Value)
        {
            if (Value.IsNullOrEmpty())
            {
                Checked -= OnChecked;
            }
            else Checked += OnChecked;
        }

        #endregion
    }
}
