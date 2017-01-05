using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// A ComboBox containing system font families.
    /// </summary>
    public partial class FontFamilyBox : ComboBox
    {
        public static DependencyProperty ShowPreviewProperty = DependencyProperty.Register("ShowPreview", typeof(bool), typeof(FontFamilyBox), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public FontFamilyBox()
        {
            InitializeComponent();
        }
    }
}
