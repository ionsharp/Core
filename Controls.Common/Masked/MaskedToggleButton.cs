using Imagin.Common.Extensions;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    public class MaskedToggleButton : ToggleButton
    {
        #region DependencyProperties

        public static DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSourceChanged));
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
            MaskedToggleButton MaskedToggleButton = Object as MaskedToggleButton;
            MaskedToggleButton.ImageBrush = new ImageBrush(MaskedToggleButton.Source);
        }

        public static DependencyProperty ImageBrushProperty = DependencyProperty.Register("ImageBrush", typeof(ImageBrush), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static readonly DependencyProperty ImageColorProperty = DependencyProperty.Register("ImageColor", typeof(Brush), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty IsRippleEnabledProperty = DependencyProperty.Register("IsRippleEnabled", typeof(bool), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty GroupNameProperty = DependencyProperty.Register("GroupName", typeof(string), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGroupNameChanged));
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
            MaskedToggleButton MaskedToggleButton = Object as MaskedToggleButton;
            if (MaskedToggleButton.GroupName == string.Empty)
            {
                MaskedToggleButton.Checked -= MaskedToggleButton.MaskedToggleButton_Checked;
            } else
            {
                MaskedToggleButton.Checked += MaskedToggleButton.MaskedToggleButton_Checked;
            }
        }

        private void MaskedToggleButton_Checked(object sender, EventArgs e)
        {
            DependencyObject Parent = this.GetParent<DependencyObject>();
            for (int i = 0, Count = VisualTreeHelper.GetChildrenCount(Parent); i < Count; i++)
            {
                var Child = VisualTreeHelper.GetChild(Parent, i);
                if (!(Child is MaskedToggleButton)) continue; 
                MaskedToggleButton Button = Child as MaskedToggleButton;
                if (Button == this) continue; 
                Button.IsChecked = false; 
            }
        }

        public static DependencyProperty CheckedToolTipProperty = DependencyProperty.Register("CheckedToolTip", typeof(string), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty UncheckedToolTipProperty = DependencyProperty.Register("UncheckedToolTip", typeof(string), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty ImageWidthProperty = DependencyProperty.Register("ImageWidth", typeof(double), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty ImageHeightProperty = DependencyProperty.Register("ImageHeight", typeof(double), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(16.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty ContentMarginProperty = DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        #endregion

        #region Methods

        public void SetGroup()
        {
        }

        #endregion

        #region MaskedToggleButton

        public MaskedToggleButton()
        {
            this.DefaultStyleKey = typeof(MaskedToggleButton);
            this.ContentMargin = new Thickness(5, 0, 0, 0);
        }

        #endregion
    }
}
