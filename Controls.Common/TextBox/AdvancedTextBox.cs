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

        public static DependencyProperty CanUserClearProperty = DependencyProperty.Register("CanUserClear", typeof(bool), typeof(AdvancedTextBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool CanUserClear
        {
            get
            {
                return (bool)GetValue(CanUserClearProperty);
            }
            set
            {
                SetValue(CanUserClearProperty, value);
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

        public static DependencyProperty SelectAllOnFocusProperty = DependencyProperty.Register("SelectAllOnFocus", typeof(bool), typeof(AdvancedTextBox), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool SelectAllOnFocus
        {
            get
            {
                return (bool)GetValue(SelectAllOnFocusProperty);
            }
            set
            {
                SetValue(SelectAllOnFocusProperty, value);
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

        #endregion

        #region AdvancedTextBox

        public AdvancedTextBox() : base()
        {
            DefaultStyleKey = typeof(AdvancedTextBox);
        }

        #endregion

        #region Methods

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);

            if (SelectAllOnFocus)
                SelectAll();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Enter)
                OnEntered(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.ClickCount == 3)
            {
                OnTripleClick();
                if (SelectAllOnTripleClick)
                    SelectAll();
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            if (SelectAllOnFocus && !IsKeyboardFocusWithin && !OnPreviewMouseLeftButtonDownHandled(e))
            {
                Focus();
                e.Handled = true;
            }
        }

        protected virtual void OnEntered(KeyEventArgs e)
        {
            if (Entered != null)
                Entered(this, e);
        }

        /// <remarks>
        /// Normally, focus is obtained when left mouse button is pressed.
        /// When clicking buttons that might be contained in the template,
        /// focus is obtained first, thus, requiring a second click in 
        /// order to actually click the button. To prevent this, we must
        /// detect whether or not the intention is to click a button or 
        /// focus. Therefore, if the element clicked IS a button, handle 
        /// the focus; otherwise, focus!
        /// </remarks>
        protected virtual bool OnPreviewMouseLeftButtonDownHandled(MouseButtonEventArgs e, Type[] HandledTypes = null)
        {
            var Parent = e.OriginalSource.As<DependencyObject>();

            HandledTypes = HandledTypes == null ? new Type[] { typeof(Button) } : HandledTypes;

            while (!Parent.Is<AdvancedTextBox>())
            {
                Parent = Parent.GetParent();
                if (Parent.IsAny(HandledTypes))
                    break;
            }

            return Parent.IsAny(HandledTypes);
        }

        protected virtual void OnTripleClick(RoutedEventArgs e = null)
        {
            if (TripleClick != null)
                TripleClick(this, e == null ? new RoutedEventArgs() : e);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_ClearButton = Template.FindName("PART_ClearButton", this) as Button;
            if (PART_ClearButton != null)
                PART_ClearButton.Click += (s, e) => Text = string.Empty;
        }

        #endregion
    }
}
