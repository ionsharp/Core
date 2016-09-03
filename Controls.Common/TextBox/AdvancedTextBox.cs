using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    public class AdvancedTextBox : TextBox
    {
        #region Properties
        
        public static DependencyProperty PlaceholderForegroundProperty = DependencyProperty.Register("PlaceholderForeground", typeof(Brush), typeof(AdvancedTextBox), new FrameworkPropertyMetadata(Brushes.LightGray, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Brush PlaceholderForeground
        {
            get
            {
                return (Brush)GetValue(PlaceholderForegroundProperty);
            }
            set
            {
                SetValue(PlaceholderForegroundProperty, value);
            }
        }

        public static DependencyProperty PlaceholderProperty = DependencyProperty.Register("Placeholder", typeof(string), typeof(AdvancedTextBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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

        public static DependencyProperty SelectOnFocusProperty = DependencyProperty.Register("SelectOnFocus", typeof(bool), typeof(AdvancedTextBox), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool SelectOnFocus
        {
            get
            {
                return (bool)GetValue(SelectOnFocusProperty);
            }
            set
            {
                SetValue(SelectOnFocusProperty, value);
            }
        }

        #endregion

        #region AdvancedTextBox

        public AdvancedTextBox() : base()
        {
            this.DefaultStyleKey = typeof(AdvancedTextBox);
        }

        #endregion

        #region Methods

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            if (this.IsKeyboardFocusWithin)
                return;
            e.Handled = true;
            this.Focus();
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            if (this.SelectOnFocus)
                this.SelectAll();
        }

        #endregion
    }
}
