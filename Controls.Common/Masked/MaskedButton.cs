using Imagin.Common;
using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    public class MaskedButton : Button
    {
        #region DependencyProperties

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

        public static DependencyProperty ContentPlacementProperty = DependencyProperty.Register("ContentPlacement", typeof(Side), typeof(MaskedButton), new FrameworkPropertyMetadata(Side.Right, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Side ContentPlacement
        {
            get
            {
                return (Side)GetValue(ContentPlacementProperty);
            }
            set
            {
                SetValue(ContentPlacementProperty, value);
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
        static void OnDropDownChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            MaskedButton MaskedButton = (MaskedButton)Object;
            if (MaskedButton.DropDown != null)
            {
                MaskedButton.DropDown.PlacementTarget = MaskedButton;
                MaskedButton.DropDown.Placement = PlacementMode.Bottom;
            }
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
        static void OnDropDownDataContextChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            MaskedButton MaskedButton = (MaskedButton)Object;
            if (MaskedButton.DropDown != null)
                MaskedButton.DropDown.DataContext = MaskedButton.DropDownDataContext;
        }

        public static DependencyProperty DropDownToolTipProperty = DependencyProperty.Register("DropDownToolTip", typeof(string), typeof(MaskedButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty DropDownVisibilityProperty = DependencyProperty.Register("DropDownVisibility", typeof(Visibility), typeof(MaskedButton), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        private static void OnSourceChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            MaskedButton Button = Object as MaskedButton;
            Button.ImageBrush = new ImageBrush(Button.Source);
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

        public MaskedButton()
        {
            this.DefaultStyleKey = typeof(MaskedButton);
            this.ContentMargin = new Thickness(5, 0, 0, 0);
        }

        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();
            this.Template.FindName("PART_Dropdown", this).As<ContentControl>().MouseLeftButtonDown += OnDropdownMouseLeftButtonDown;
        }

        #endregion

        #region Methods

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Handled)
                return;
        }

        void OnDropdownMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (this.DropDown != null)
                this.DropDown.IsOpen = true;
        }

        #endregion
    }
}
