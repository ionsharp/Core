using Imagin.Common;
using Imagin.Common.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    public class AdvancedTextBox : TextBox
    {
        #region Properties

        public event EventHandler<KeyEventArgs> Entered;

        public event EventHandler<RoutedEventArgs> TripleClick;

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

        public static DependencyProperty SelectAllOnTripleClickProperty = DependencyProperty.Register("SelectAllOnTripleClick", typeof(bool), typeof(AdvancedTextBox), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool SelectAllOnTripleClick
        {
            get
            {
                return (bool)GetValue(SelectAllOnTripleClickProperty);
            }
            set
            {
                SetValue(SelectAllOnTripleClickProperty, value);
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

        public static DependencyProperty ShowClearButtonProperty = DependencyProperty.Register("ShowClearButton", typeof(bool), typeof(AdvancedTextBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool ShowClearButton
        {
            get
            {
                return (bool)GetValue(ShowClearButtonProperty);
            }
            set
            {
                SetValue(ShowClearButtonProperty, value);
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

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var Button = this.Template.FindName("PART_ClearButton", this);
            if (Button != null && Button.Is<Button>())
            {
                Button PART_ClearButton = Button.As<Button>();
                PART_ClearButton.Click += (s, e) => this.Text = string.Empty;
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            if (this.IsKeyboardFocusWithin) return;
            e.Handled = this.OnPreviewMouseLeftButtonDownHandled(e);
            if (e.Handled) this.Focus();
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            if (this.SelectOnFocus)
                this.SelectAll();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Enter)
                this.OnEntered(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.ClickCount == 3)
            {
                this.OnTripleClick();
                if (SelectAllOnTripleClick)
                    this.SelectAll();
            }
        }

        #endregion

        #region Virtual

        protected virtual void OnEntered(KeyEventArgs e)
        {
            if (this.Entered != null)
                this.Entered(this, e);
        }

        protected virtual bool OnPreviewMouseLeftButtonDownHandled(MouseButtonEventArgs e)
        {
            return true;
            /*
            DependencyObject Parent = e.OriginalSource.As<DependencyObject>();
            while (!Parent.Is<AdvancedTextBox>())
            {
                Parent = Parent.GetParent();
                if (Parent.Is<Button>())
                    break;
            }
            if (Parent.Is<AdvancedTextBox>())
            {
                e.Handled = true;
                this.Focus();
            }

            */
        }

        protected virtual void OnTripleClick(RoutedEventArgs e = null)
        {
            if (this.TripleClick != null)
                this.TripleClick(this, e == null ? new RoutedEventArgs() : e);
        }

        #endregion

        #endregion
    }
}
