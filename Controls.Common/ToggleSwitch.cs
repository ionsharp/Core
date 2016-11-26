using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    public class ToggleSwitch : CheckBox
    {
        public static DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ToggleSwitch), new FrameworkPropertyMetadata(default(CornerRadius), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }

        public static DependencyProperty ThumbStyleProperty = DependencyProperty.Register("ThumbStyle", typeof(Style), typeof(ToggleSwitch), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Style ThumbStyle
        {
            get
            {
                return (Style)GetValue(ThumbStyleProperty);
            }
            set
            {
                SetValue(ThumbStyleProperty, value);
            }
        }
        
        public ToggleSwitch() : base()
        {
            this.DefaultStyleKey = typeof(ToggleSwitch);
        }
    }
}
