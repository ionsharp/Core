using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// A container for similar RadioButtons.
    /// </summary>
    public class RadioGroup : ItemsControl
    {
        public static DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(RadioGroup), new FrameworkPropertyMetadata(default(CornerRadius), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty GroupNameProperty = DependencyProperty.Register("GroupName", typeof(string), typeof(RadioGroup), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnGroupNameChanged));
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

        public RadioGroup() : base()
        {
            this.DefaultStyleKey = typeof(RadioGroup);
        }
    }
}