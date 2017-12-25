using Imagin.Common.Linq;
using Imagin.Controls.Common.Input;
using System;
using System.Windows;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    public class ComboBox : System.Windows.Controls.ComboBox
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<TextSubmittedEventArgs> Entered;
        
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CheckedToolTipProperty = DependencyProperty.Register("CheckedToolTip", typeof(string), typeof(ComboBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty PlaceholderProperty = DependencyProperty.Register("Placeholder", typeof(string), typeof(ComboBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty UncheckedToolTipProperty = DependencyProperty.Register("UncheckedToolTip", typeof(string), typeof(ComboBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public ComboBox() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var PART_TextBox = Template.FindName("PART_TextBox", this) as TextBox;
            if (PART_TextBox != null)
                PART_TextBox.Entered += (s, e) => OnEntered(s.As<ComboBox>().Text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Text"></param>
        protected virtual void OnEntered(string Text)
        {
            Entered?.Invoke(this, new TextSubmittedEventArgs(Text));
        }
    }
}
