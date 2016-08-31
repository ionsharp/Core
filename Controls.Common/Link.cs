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
        #region Dependency Properties

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
