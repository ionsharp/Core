using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
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
        }

        public override void OnApplyTemplate()
        {
            base.ApplyTemplate();
        }

        #endregion

        #region Override Methods

        protected override void OnClick()
        {
            if (string.IsNullOrEmpty(this.Uri))
            {
                try
                {
                    Process.Start(Uri);
                }
                catch
                {

                }
            }
            base.OnClick();
        }

        #endregion
    }
}
