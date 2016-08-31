using Imagin.Common;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    public class ImageToggleButton : ContentControl
    {
        public event EventHandler<EventArgs> Checked;

        public event EventHandler<EventArgs> Click;

        #region DependencyProperties

        public static DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(ImageToggleButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSourceChanged));
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
            ImageToggleButton ImageToggleButton = Object as ImageToggleButton;
            ImageToggleButton.ImageBrush = new ImageBrush(ImageToggleButton.Source);
        }

        public static DependencyProperty ImageBrushProperty = DependencyProperty.Register("ImageBrush", typeof(ImageBrush), typeof(ImageToggleButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static readonly DependencyProperty ImageColorProperty = DependencyProperty.Register("ImageColor", typeof(Brush), typeof(ImageToggleButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(ImageToggleButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsCheckedChanged));
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
        private static void OnIsCheckedChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ImageToggleButton ImageToggleButton = Object as ImageToggleButton;
            if (ImageToggleButton.IsChecked && ImageToggleButton.Checked != null)
                ImageToggleButton.Checked(ImageToggleButton, new EventArgs());
        }

        public static DependencyProperty GroupNameProperty = DependencyProperty.Register("GroupName", typeof(string), typeof(ImageToggleButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGroupNameChanged));
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
        private static void OnGroupNameChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            ImageToggleButton ImageToggleButton = Object as ImageToggleButton;
            if (ImageToggleButton.GroupName == string.Empty)
            {
                ImageToggleButton.Checked -= ImageToggleButton.ImageToggleButton_Checked;
            } else
            {
                ImageToggleButton.Checked += ImageToggleButton.ImageToggleButton_Checked;
            }
        }

        private void ImageToggleButton_Checked(object sender, EventArgs e)
        {
            //We only want to affect the other values if current is true. This avoids other controls from attempting to execute same method when their values have changed.
            //In order for this to work, all controls sharing same group name should be in same parent.
            DependencyObject Parent = this.FindParent<DependencyObject>(this);
            for (int i = 0, Count = VisualTreeHelper.GetChildrenCount(Parent); i < Count; i++)
            {
                var Child = VisualTreeHelper.GetChild(Parent, i);
                if (!(Child is ImageToggleButton)) continue; //If it's not same type of control, skip it
                ImageToggleButton Button = Child as ImageToggleButton;
                if (Button == this) continue; //If we're at this, skip it
                Button.IsChecked = false; //If it's not this, we'll want to uncheck it.
            }
        }

        public static DependencyProperty CheckedToolTipProperty = DependencyProperty.Register("CheckedToolTip", typeof(string), typeof(ImageToggleButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty UncheckedToolTipProperty = DependencyProperty.Register("UncheckedToolTip", typeof(string), typeof(ImageToggleButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string UncheckedToolTip
        {
            get
            {
                return (string)GetValue(UncheckedToolTipProperty);
            }
            set
            {
                SetValue(UncheckedToolTipProperty, value);
            }
        }

        public static DependencyProperty ImageWidthProperty = DependencyProperty.Register("ImageWidth", typeof(double), typeof(ImageToggleButton), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty ImageHeightProperty = DependencyProperty.Register("ImageHeight", typeof(double), typeof(ImageToggleButton), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(object), typeof(ImageToggleButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public object Text
        {
            get
            {
                return (object)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static DependencyProperty ContentMarginProperty = DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(ImageToggleButton), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty ContentPlacementProperty = DependencyProperty.Register("ContentPlacement", typeof(Side), typeof(ImageToggleButton), new FrameworkPropertyMetadata(Side.Right, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        #endregion

        #region Methods

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child); //Get parent item
            if (parentObject == null) return null; //We've reached the end of the tree
            T parent = parentObject as T; //Check if the parent matches the type we're looking for
            if (parent != null) return parent; else return FindParent<T>(parentObject);
        }

        public void SetGroup()
        {
        }

        #region Overrides

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.IsChecked = this.IsChecked ? false : true;
            if (this.Click != null) this.Click(this, new EventArgs());
        }

        #endregion

        #endregion

        #region ImageToggleButton

        public ImageToggleButton()
        {
            this.DefaultStyleKey = typeof(ImageToggleButton);
            this.ContentMargin = new Thickness(5, 0, 0, 0);
        }

        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();
        }

        #endregion
    }
}
