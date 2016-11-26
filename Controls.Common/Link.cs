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

        public static DependencyProperty TextDecorationsProperty = DependencyProperty.Register("TextDecorations", typeof(TextDecorationCollection), typeof(Link), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty TextTrimmingProperty = DependencyProperty.Register("TextTrimming", typeof(TextTrimming), typeof(Link), new FrameworkPropertyMetadata(TextTrimming.None, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(Link), new FrameworkPropertyMetadata(TextWrapping.NoWrap, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty UriProperty = DependencyProperty.Register("Uri", typeof(string), typeof(Link), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();
        }

        #endregion
    }
}
