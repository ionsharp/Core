using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class MaskedToggleButton : ToggleButton
    {
        #region Properties
        
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
        public static DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(ImageSource), typeof(MaskedToggleButton), new FrameworkPropertyMetadata(default(ImageSource), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSourceChanged));
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
            d.As<MaskedToggleButton>().OnSourceChanged((ImageSource)e.NewValue);
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
        /// <param name="Value"></param>
        protected virtual void OnGroupNameChanged(string Value)
        {
            if (Value.IsNullOrEmpty())
            {
                Checked -= OnChecked;
            }
            else Checked += OnChecked;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnSourceChanged(ImageSource Value)
        {
        }

        #endregion
    }
}
