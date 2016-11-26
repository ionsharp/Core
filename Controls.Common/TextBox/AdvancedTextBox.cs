using Imagin.Common.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    [TemplatePart(Name = "PART_ClearButton", Type = typeof(Button))]
    public class AdvancedTextBox : TextBox
    {
        #region Properties

        Button PART_ClearButton
        {
            get; set;
        }

        public event EventHandler<KeyEventArgs> Entered;

        public event EventHandler<RoutedEventArgs> TripleClick;

        public static DependencyProperty IsClearEnabledProperty = DependencyProperty.Register("IsClearEnabled", typeof(bool), typeof(AdvancedTextBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool IsClearEnabled
        {
            get
            {
                return (bool)GetValue(IsClearEnabledProperty);
            }
            set
            {
                SetValue(IsClearEnabledProperty, value);
            }
        }

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
        public bool SelectAllOnFocus
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

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var Button = this.Template.FindName("PART_ClearButton", this);
            if (Button != null && Button.Is<Button>())
            {
                var PART_ClearButton = Button.As<Button>();
                PART_ClearButton.Click += (s, e) => this.Text = string.Empty;
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            if (!this.IsKeyboardFocusWithin && !this.OnPreviewMouseLeftButtonDownHandled(e)) 
                this.Focus();
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            if (this.SelectAllOnFocus)
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

        #region Public



        #endregion

        #region Virtual

        protected virtual void OnEntered(KeyEventArgs e)
        {
            if (this.Entered != null)
                this.Entered(this, e);
        }

        protected virtual bool OnPreviewMouseLeftButtonDownHandled(MouseButtonEventArgs e)
        {
            return e.Handled;
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
