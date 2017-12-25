using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ToggleSwitch : CheckBox
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ToggleSwitch), new FrameworkPropertyMetadata(default(CornerRadius), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ThumbStyleProperty = DependencyProperty.Register("ThumbStyle", typeof(Style), typeof(ToggleSwitch), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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
        
        /// <summary>
        /// 
        /// </summary>
        public ToggleSwitch() : base()
        {
            DefaultStyleKey = typeof(ToggleSwitch);
        }
    }
}
