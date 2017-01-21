using Imagin.Common.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBoxExt))]
    public class AdvancedComboBox : ComboBox
    {
        public event EventHandler<KeyEventArgs> Entered;
        
        public static DependencyProperty CheckedToolTipProperty = DependencyProperty.Register("CheckedToolTip", typeof(string), typeof(AdvancedComboBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string CheckedToolTip
        {
            get
            {
                return (string)GetValue(CheckedToolTipProperty);
            }
            set
            {
                SetValue(CheckedToolTipProperty, value);
            }
        }

        public static DependencyProperty PlaceholderProperty = DependencyProperty.Register("Placeholder", typeof(string), typeof(AdvancedComboBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string Placeholder
        {
            get
            {
                return (string)GetValue(PlaceholderProperty);
            }
            set
            {
                SetValue(PlaceholderProperty, value);
            }
        }

        public static DependencyProperty UncheckedToolTipProperty = DependencyProperty.Register("UncheckedToolTip", typeof(string), typeof(AdvancedComboBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string UncheckedToolTip
        {
            get
            {
                return (string)GetValue(UncheckedToolTipProperty);
            }
            set
            {
                SetValue(UncheckedToolTipProperty, value);
            }
        }

        public AdvancedComboBox() : base()
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var PART_TextBox = this.Template.FindName("PART_TextBox", this);
            if (PART_TextBox != null && PART_TextBox.Is<TextBoxExt>())
            {
                PART_TextBox.As<TextBoxExt>().Entered += (s, e) =>
                {
                    if (Entered != null)
                        Entered(this, e);
                };
            }
        }
    }
}
