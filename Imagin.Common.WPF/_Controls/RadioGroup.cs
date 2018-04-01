using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common
{
    /// <summary>
    /// A container for similar RadioButtons.
    /// </summary>
    public class RadioGroup : ItemsControl
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(RadioGroup), new FrameworkPropertyMetadata(default(CornerRadius), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty GroupNameProperty = DependencyProperty.Register("GroupName", typeof(string), typeof(RadioGroup), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGroupNameChanged));
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
        static void OnGroupNameChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            RadioGroup RadioGroup = (RadioGroup)Object;
            foreach (RadioButton r in RadioGroup.Items)
                r.GroupName = RadioGroup.GroupName;
        }

        /// <summary>
        /// 
        /// </summary>
        public RadioGroup() : base()
        {
            this.DefaultStyleKey = typeof(RadioGroup);
        }
    }
}