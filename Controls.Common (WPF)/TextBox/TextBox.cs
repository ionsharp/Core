using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Controls.Common.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class TextBox : System.Windows.Controls.TextBox
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<TextSubmittedEventArgs> Entered;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<RoutedEventArgs> TripleClick;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ClearButtonTemplateProperty = DependencyProperty.Register("ClearButtonTemplate", typeof(DataTemplate), typeof(TextBox), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public DataTemplate ClearButtonTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ClearButtonTemplateProperty);
            }
            set
            {
                SetValue(ClearButtonTemplateProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CharacterMaskForegroundProperty = DependencyProperty.Register("CharacterMaskForeground", typeof(Brush), typeof(TextBox), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Brush CharacterMaskForeground
        {
            get
            {
                return (Brush)GetValue(CharacterMaskForegroundProperty);
            }
            set
            {
                SetValue(CharacterMaskForegroundProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty EnterButtonTemplateProperty = DependencyProperty.Register("EnterButtonTemplate", typeof(DataTemplate), typeof(TextBox), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public DataTemplate EnterButtonTemplate
        {
            get
            {
                return (DataTemplate)GetValue(EnterButtonTemplateProperty);
            }
            set
            {
                SetValue(EnterButtonTemplateProperty, value);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty InnerPaddingProperty = DependencyProperty.Register("InnerPadding", typeof(Thickness), typeof(TextBox), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Thickness InnerPadding
        {
            get
            {
                return (Thickness)GetValue(InnerPaddingProperty);
            }
            set
            {
                SetValue(InnerPaddingProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty IsClearEnabledProperty = DependencyProperty.Register("IsClearEnabled", typeof(bool), typeof(TextBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty IsCharacterMaskingEnabledProperty = DependencyProperty.Register("IsCharacterMaskingEnabled", typeof(bool), typeof(TextBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public bool IsCharacterMaskingEnabled
        {
            get
            {
                return (bool)GetValue(IsCharacterMaskingEnabledProperty);
            }
            set
            {
                SetValue(IsCharacterMaskingEnabledProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty PlaceholderProperty = DependencyProperty.Register("Placeholder", typeof(string), typeof(TextBox), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty PlaceholderStyleProperty = DependencyProperty.Register("PlaceholderStyle", typeof(Style), typeof(TextBox), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Style PlaceholderStyle
        {
            get
            {
                return (Style)GetValue(PlaceholderStyleProperty);
            }
            set
            {
                SetValue(PlaceholderStyleProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ScrollViewerTemplateProperty = DependencyProperty.Register("ScrollViewerTemplate", typeof(ControlTemplate), typeof(TextBox), new FrameworkPropertyMetadata(default(ControlTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public ControlTemplate ScrollViewerTemplate
        {
            get
            {
                return (ControlTemplate)GetValue(ScrollViewerTemplateProperty);
            }
            set
            {
                SetValue(ScrollViewerTemplateProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectAllOnFocusProperty = DependencyProperty.Register("SelectAllOnFocus", typeof(bool), typeof(TextBox), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SelectAllOnTripleClickProperty = DependencyProperty.Register("SelectAllOnTripleClick", typeof(bool), typeof(TextBox), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ShowEnterButtonProperty = DependencyProperty.Register("ShowEnterButton", typeof(bool), typeof(TextBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public bool ShowEnterButton
        {
            get
            {
                return (bool)GetValue(ShowEnterButtonProperty);
            }
            set
            {
                SetValue(ShowEnterButtonProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ShowToggleButtonProperty = DependencyProperty.Register("ShowToggleButton", typeof(bool), typeof(TextBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public bool ShowToggleButton
        {
            get
            {
                return (bool)GetValue(ShowToggleButtonProperty);
            }
            set
            {
                SetValue(ShowToggleButtonProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ToggleButtonTemplateProperty = DependencyProperty.Register("ToggleButtonTemplate", typeof(DataTemplate), typeof(TextBox), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public DataTemplate ToggleButtonTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ToggleButtonTemplateProperty);
            }
            set
            {
                SetValue(ToggleButtonTemplateProperty, value);
            }
        }

        #endregion

        #region TextBox

        /// <summary>
        /// 
        /// </summary>
        public TextBox() : base()
        {
            DefaultStyleKey = typeof(TextBox);
        }

        #endregion

        #region Methods

        ICommand clearCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand ClearCommand
        {
            get
            {
                clearCommand = clearCommand ?? new RelayCommand(() => Text = string.Empty, () => IsFocused && Text.Length > 0 && !IsReadOnly);
                return clearCommand;
            }
        }

        ICommand enterCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand EnterCommand
        {
            get
            {
                enterCommand = enterCommand ?? new RelayCommand(() => OnEntered(Text), () => true);
                return enterCommand;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);

            if (SelectAllOnFocus)
                SelectAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Enter)
                OnEntered(Text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            if (SelectAllOnFocus && !IsKeyboardFocusWithin && !OnPreviewMouseLeftButtonDownHandled(e))
            {
                Focus();
                e.Handled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Text"></param>
        protected virtual void OnEntered(string Text)
        {
            Entered?.Invoke(this, new TextSubmittedEventArgs(Text));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="HandledTypes"></param>
        /// <returns></returns>
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

            HandledTypes = HandledTypes == null ? Arr.New(typeof(Button), typeof(ToggleButton)) : HandledTypes;

            while (!Parent.Is<TextBox>())
            {
                Parent = Parent.GetParent();
                if (Parent.IsAny(HandledTypes))
                    break;
            }

            return Parent.IsAny(HandledTypes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTripleClick(RoutedEventArgs e = null)
        {
            if (TripleClick != null)
                TripleClick(this, e == null ? new RoutedEventArgs() : e);
        }

        #endregion
    }
}
