using System.Windows;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ExtendedScrollViewer : System.Windows.Controls.ScrollViewer
    {
        /// <summary>
        /// 
        /// </summary>
        public static new DependencyProperty PaddingProperty = DependencyProperty.Register("Padding", typeof(Thickness), typeof(ExtendedScrollViewer), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public new Thickness Padding
        {
            get
            {
                return (Thickness)GetValue(PaddingProperty);
            }
            set
            {
                SetValue(PaddingProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ExtendedScrollViewer() : base()
        {
        }
    }
}
