using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common
{
    /// <summary>
    /// A ComboBox containing system font families.
    /// </summary>
    public partial class FontFamilyBox : ComboBox
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ShowPreviewProperty = DependencyProperty.Register("ShowPreview", typeof(bool), typeof(FontFamilyBox), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public bool ShowPreview
        {
            get
            {
                return (bool)GetValue(ShowPreviewProperty);
            }
            set
            {
                SetValue(ShowPreviewProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public FontFamilyBox()
        {
            InitializeComponent();
        }
    }
}
