using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    public class AdvancedImageDropDownButton : ContentControl
    {
        #region DependencyProperties

        public static DependencyProperty ContentMarginProperty = DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(AdvancedImageDropDownButton), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static readonly DependencyProperty DropDownProperty = DependencyProperty.Register("DropDown", typeof(ContextMenu), typeof(AdvancedImageDropDownButton), new UIPropertyMetadata(null, OnDropDownChanged));
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
        private static void OnDropDownChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            AdvancedImageDropDownButton a = (AdvancedImageDropDownButton)Object;
            if (a.DropDown != null)
            {
                a.DropDown.PlacementTarget = a;
                a.DropDown.Placement = PlacementMode.Bottom;
            }
        }

        public static DependencyProperty ArrowPaddingProperty = DependencyProperty.Register("ArrowPadding", typeof(Thickness), typeof(AdvancedImageDropDownButton), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Thickness ArrowPadding
        {
            get
            {
                return (Thickness)GetValue(ArrowPaddingProperty);
            }
            set
            {
                SetValue(ArrowPaddingProperty, value);
            }
        }

        public static DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(AdvancedImageDropDownButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty DropDownToolTipProperty = DependencyProperty.Register("DropDownToolTip", typeof(string), typeof(AdvancedImageDropDownButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string DropDownToolTip
        {
            get
            {
                return (string)GetValue(DropDownToolTipProperty);
            }
            set
            {
                SetValue(DropDownToolTipProperty, value);
            }
        }

        public static DependencyProperty ButtonToolTipProperty = DependencyProperty.Register("ButtonToolTip", typeof(string), typeof(AdvancedImageDropDownButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string ButtonToolTip
        {
            get
            {
                return (string)GetValue(ButtonToolTipProperty);
            }
            set
            {
                SetValue(ButtonToolTipProperty, value);
            }
        }

        public static DependencyProperty ImageWidthProperty = DependencyProperty.Register("ImageWidth", typeof(double), typeof(AdvancedImageDropDownButton), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty ImageHeightProperty = DependencyProperty.Register("ImageHeight", typeof(double), typeof(AdvancedImageDropDownButton), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty ShowDropDownProperty = DependencyProperty.Register("ShowDropDown", typeof(bool), typeof(AdvancedImageDropDownButton), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ShowDropDown
        {
            get
            {
                return (bool)GetValue(ShowDropDownProperty);
            }
            set
            {
                SetValue(ShowDropDownProperty, value);
            }
        }

        public static DependencyProperty ClickProperty = DependencyProperty.Register("Click", typeof(RoutedEventHandler), typeof(AdvancedImageDropDownButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public RoutedEventHandler Click
        {
            get
            {
                return (RoutedEventHandler)GetValue(ClickProperty);
            }
            set
            {
                SetValue(ClickProperty, value);
            }
        }

        public static DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(AdvancedImageDropDownButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        #endregion

        #region AdvancedImageDropDownButton

        public AdvancedImageDropDownButton()
        {
            this.DefaultStyleKey = typeof(AdvancedImageDropDownButton);

            this.ArrowPadding = new Thickness(5, 0, 5, 0);

            this.SetBinding(AdvancedImageDropDownButton.IsCheckedProperty, new Binding("DropDown.IsOpen")
            {
                Source = this
            });
        }

        #endregion

        #region Events

        void OnClick(object sender, RoutedEventArgs e)
        {
            if (this.Click != null)
                this.Click.Invoke(sender, e);
        }

        #endregion

        #region Override Methods

        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();
            (this.Template.FindName("PART_Icon", this) as ImageButton).Click += OnClick;

            (this.Template.FindName("PART_Dropdown", this) as ContentControl).MouseLeftButtonDown += OnDropdownMouseLeftButtonDown;
        }

        void OnDropdownMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.DropDown != null)
                this.DropDown.IsOpen = true;
        }

        #endregion
    }
}
