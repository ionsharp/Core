using Imagin.Common.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    [TemplatePart(Name = "PART_TextBox", Type = typeof(AdvancedTextBox))]
    public class AdvancedComboBox : ComboBox
    {
        public event EventHandler<KeyEventArgs> Entered;

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

        public AdvancedComboBox() : base()
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var PART_TextBox = this.Template.FindName("PART_TextBox", this);
            if (PART_TextBox != null && PART_TextBox.Is<AdvancedTextBox>())
            {
                PART_TextBox.As<AdvancedTextBox>().Entered += (s, e) =>
                {
                    if (this.Entered != null)
                        this.Entered(this, e);
                };
            }
        }
    }
}
