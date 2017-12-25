using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// Opens up a web link in default browser when clicked.
    /// </summary>
    public partial class Link : Button
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TextDecorationsProperty = DependencyProperty.Register("TextDecorations", typeof(TextDecorationCollection), typeof(Link), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public TextDecorationCollection TextDecorations
        {
            get
            {
                return (TextDecorationCollection)GetValue(TextDecorationsProperty);
            }
            set
            {
                SetValue(TextDecorationsProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TextTrimmingProperty = DependencyProperty.Register("TextTrimming", typeof(TextTrimming), typeof(Link), new FrameworkPropertyMetadata(TextTrimming.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public TextTrimming TextTrimming
        {
            get
            {
                return (TextTrimming)GetValue(TextTrimmingProperty);
            }
            set
            {
                SetValue(TextTrimmingProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(Link), new FrameworkPropertyMetadata(TextWrapping.NoWrap, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public TextWrapping TextWrapping
        {
            get
            {
                return (TextWrapping)GetValue(TextWrappingProperty);
            }
            set
            {
                SetValue(TextWrappingProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty UriProperty = DependencyProperty.Register("Uri", typeof(string), typeof(Link), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public string Uri
        {
            get
            {
                return (string)GetValue(UriProperty);
            }
            set
            {
                SetValue(UriProperty, value);
            }
        }

        #endregion

        #region Link

        /// <summary>
        /// 
        /// </summary>
        public Link()
        {
            this.DefaultStyleKey = typeof(Link);
            this.Click += OnClick;
        }

        void OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Uri))
                return;
            try
            {
                Process.Start(Uri);
            }
            catch { }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();
        }

        #endregion
    }
}
