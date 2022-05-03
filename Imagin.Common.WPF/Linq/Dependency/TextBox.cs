using Imagin.Common.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Linq
{
    [Extends(typeof(TextBox))]
    public static class XTextBox
    {
        #region Properties

        #region CanLabel

        /// <summary>
        /// Gets or sets if a label should display based on the current (logical) state of the <see cref="TextBox"/>. 
        /// This should always be <see langword="true"/> when a displayable label is desired. This property differs
        /// from (but is related to) <see cref="IsEditableProperty"/>, which affects the (visual) state of the <see cref="TextBox"/>.
        /// If <see langword="true"/>, the label is displayed when the <see cref="TextBox"/> loses focus or  <see cref="Key.Enter"/> is pressed.
        /// </summary>
        public static readonly DependencyProperty CanLabelProperty = DependencyProperty.RegisterAttached("CanLabel", typeof(bool), typeof(XTextBox), new FrameworkPropertyMetadata(false));
        public static bool GetCanLabel(TextBox i) => (bool)i.GetValue(CanLabelProperty);
        public static void SetCanLabel(TextBox i, bool input) => i.SetValue(CanLabelProperty, input);

        #endregion

        #region ClearButtonTemplate

        public static readonly DependencyProperty ClearButtonTemplateProperty = DependencyProperty.RegisterAttached("ClearButtonTemplate", typeof(DataTemplate), typeof(XTextBox), new FrameworkPropertyMetadata(null));
        public static DataTemplate GetClearButtonTemplate(TextBox i) => (DataTemplate)i.GetValue(ClearButtonTemplateProperty);
        public static void SetClearButtonTemplate(TextBox i, DataTemplate input) => i.SetValue(ClearButtonTemplateProperty, input);

        #endregion

        #region ClearButtonVisibility

        public static readonly DependencyProperty ClearButtonVisibilityProperty = DependencyProperty.RegisterAttached("ClearButtonVisibility", typeof(Visibility), typeof(XTextBox), new FrameworkPropertyMetadata(Visibility.Collapsed));
        public static Visibility GetClearButtonVisibility(TextBox i) => (Visibility)i.GetValue(ClearButtonVisibilityProperty);
        public static void SetClearButtonVisibility(TextBox i, Visibility input) => i.SetValue(ClearButtonVisibilityProperty, input);

        #endregion

        #region EditButtonAlignment

        public static readonly DependencyProperty EditButtonAlignmentProperty = DependencyProperty.RegisterAttached("EditButtonAlignment", typeof(HorizontalAlignment), typeof(XTextBox), new FrameworkPropertyMetadata(HorizontalAlignment.Left));
        public static HorizontalAlignment GetEditButtonAlignment(TextBox i) => (HorizontalAlignment)i.GetValue(EditButtonAlignmentProperty);
        public static void SetEditButtonAlignment(TextBox i, HorizontalAlignment input) => i.SetValue(EditButtonAlignmentProperty, input);

        #endregion

        #region EditButtonTemplate

        public static readonly DependencyProperty EditButtonTemplateProperty = DependencyProperty.RegisterAttached("EditButtonTemplate", typeof(DataTemplate), typeof(XTextBox), new FrameworkPropertyMetadata(null));
        public static DataTemplate GetEditButtonTemplate(TextBox i) => (DataTemplate)i.GetValue(EditButtonTemplateProperty);
        public static void SetEditButtonTemplate(TextBox i, DataTemplate input) => i.SetValue(EditButtonTemplateProperty, input);

        #endregion

        #region EditButtonVisibility

        public static readonly DependencyProperty EditButtonVisibilityProperty = DependencyProperty.RegisterAttached("EditButtonVisibility", typeof(Visibility), typeof(XTextBox), new FrameworkPropertyMetadata(Visibility.Collapsed));
        public static Visibility GetEditButtonVisibility(TextBox i) => (Visibility)i.GetValue(EditButtonVisibilityProperty);
        public static void SetEditButtonVisibility(TextBox i, Visibility input) => i.SetValue(EditButtonVisibilityProperty, input);

        #endregion

        #region EditCommand

        public static readonly RoutedUICommand EditCommand = new(nameof(EditCommand), nameof(EditCommand), typeof(XTextBox));
        static void OnEdit(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is TextBox box)
            {
                SetIsEditable(box, true);
                box.Focus();
            }
        }
        static void OnCanEdit(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is TextBox box)
                e.CanExecute = !GetIsEditable(box);
        }

        #endregion

        #region (RoutedEvent) Edited

        public static readonly RoutedEvent EditedEvent = EventManager.RegisterRoutedEvent("Edited", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TextBox));
        public static void AddEditedHandler(DependencyObject i, RoutedEventHandler handler)
        {
            if (i is UIElement j)
                j.AddHandler(EditedEvent, handler);
        }
        public static void RemoveEditedHandler(DependencyObject i, RoutedEventHandler handler)
        {
            if (i is UIElement j)
                j.RemoveHandler(EditedEvent, handler);
        }

        #endregion

        #region EditedCommand

        public static readonly DependencyProperty EditedCommandProperty = DependencyProperty.RegisterAttached("EditedCommand", typeof(ICommand), typeof(XTextBox), new FrameworkPropertyMetadata(null));
        public static ICommand GetEditedCommand(TextBox i) => (ICommand)i.GetValue(EditedCommandProperty);
        public static void SetEditedCommand(TextBox i, ICommand input) => i.SetValue(EditedCommandProperty, input);

        #endregion

        #region EditedCommandParameter

        public static readonly DependencyProperty EditedCommandParameterProperty = DependencyProperty.RegisterAttached("EditedCommandParameter", typeof(object), typeof(XTextBox), new FrameworkPropertyMetadata(null));
        public static object GetEditedCommandParameter(TextBox i) => (object)i.GetValue(EditedCommandParameterProperty);
        public static void SetEditedCommandParameter(TextBox i, object input) => i.SetValue(EditedCommandParameterProperty, input);

        #endregion

        #region (private) EditMouseDown

        static readonly DependencyProperty EditMouseDownProperty = DependencyProperty.RegisterAttached("EditMouseDown", typeof(int), typeof(XTextBox), new FrameworkPropertyMetadata(0));
        static int GetEditMouseDown(TextBox i) => (int)i.GetValue(EditMouseDownProperty);
        static void SetEditMouseDown(TextBox i, int input) => i.SetValue(EditMouseDownProperty, input);

        #endregion

        #region EditMouseEvent

        public static readonly DependencyProperty EditMouseEventProperty = DependencyProperty.RegisterAttached("EditMouseEvent", typeof(MouseEvent), typeof(XTextBox), new FrameworkPropertyMetadata(MouseEvent.MouseUp));
        public static MouseEvent GetEditMouseEvent(TextBox i) => (MouseEvent)i.GetValue(EditMouseEventProperty);
        public static void SetEditMouseEvent(TextBox i, MouseEvent input) => i.SetValue(EditMouseEventProperty, input);

        #endregion

        #region EnterButtonSource

        public static readonly DependencyProperty EnterButtonSourceProperty = DependencyProperty.RegisterAttached("EnterButtonSource", typeof(ImageSource), typeof(XTextBox), new FrameworkPropertyMetadata(null));
        public static ImageSource GetEnterButtonSource(TextBox i) => (ImageSource)i.GetValue(EnterButtonSourceProperty);
        public static void SetEnterButtonSource(TextBox i, ImageSource input) => i.SetValue(EnterButtonSourceProperty, input);

        #endregion

        #region EnterButtonTemplate

        public static readonly DependencyProperty EnterButtonTemplateProperty = DependencyProperty.RegisterAttached("EnterButtonTemplate", typeof(DataTemplate), typeof(XTextBox), new FrameworkPropertyMetadata(null));
        public static DataTemplate GetEnterButtonTemplate(TextBox i) => (DataTemplate)i.GetValue(EnterButtonTemplateProperty);
        public static void SetEnterButtonTemplate(TextBox i, DataTemplate input) => i.SetValue(EnterButtonTemplateProperty, input);

        #endregion

        #region EnterButtonVisibility

        public static readonly DependencyProperty EnterButtonVisibilityProperty = DependencyProperty.RegisterAttached("EnterButtonVisibility", typeof(Visibility), typeof(XTextBox), new FrameworkPropertyMetadata(Visibility.Collapsed));
        public static Visibility GetEnterButtonVisibility(TextBox i) => (Visibility)i.GetValue(EnterButtonVisibilityProperty);
        public static void SetEnterButtonVisibility(TextBox i, Visibility input) => i.SetValue(EnterButtonVisibilityProperty, input);

        #endregion

        #region EnterCommand

        public static readonly DependencyProperty EnterCommandProperty = DependencyProperty.RegisterAttached("EnterCommand", typeof(ICommand), typeof(XTextBox), new FrameworkPropertyMetadata(null, OnEnterCommandChanged));
        public static ICommand GetEnterCommand(TextBox i) => (ICommand)i.GetValue(EnterCommandProperty);
        public static void SetEnterCommand(TextBox i, ICommand input) => i.SetValue(EnterCommandProperty, input);
        static void OnEnterCommandChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TextBox textBox)
                textBox.RegisterHandlerAttached(e.NewValue is ICommand, EnterCommandProperty, i => i.KeyDown += EnterCommand_KeyDown, i => i.KeyDown -= EnterCommand_KeyDown);
        }

        static void EnterCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox box)
            {
                if (e.Key == Key.Enter)
                {
                    if (GetEnterCommand(box) is ICommand command)
                    {
                        e.Handled = true;
                        command.Execute(box.Text);
                    }
                }
            }
        }

        #endregion

        #region FontScale

        public static readonly DependencyProperty FontScaleProperty = DependencyProperty.RegisterAttached("FontScale", typeof(double), typeof(XTextBox), new FrameworkPropertyMetadata(1.0, OnFontScaleChanged));
        public static double GetFontScale(TextBox i) => (double)i.GetValue(FontScaleProperty);
        public static void SetFontScale(TextBox i, double input) => i.SetValue(FontScaleProperty, input);
        static void OnFontScaleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.FontSize = GetFontScaleOrigin(textBox) * (double)e.NewValue;
        }

        #endregion

        #region FontScaleOrigin

        public static readonly DependencyProperty FontScaleOriginProperty = DependencyProperty.RegisterAttached("FontScaleOrigin", typeof(double), typeof(XTextBox), new FrameworkPropertyMetadata(SystemFonts.MessageFontSize, OnFontScaleOriginChanged));
        public static double GetFontScaleOrigin(TextBox i) => (double)i.GetValue(FontScaleOriginProperty);
        public static void SetFontScaleOrigin(TextBox i, double input) => i.SetValue(FontScaleOriginProperty, input);
        static void OnFontScaleOriginChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.FontSize = (double)e.NewValue * GetFontScale(textBox);
        }

        #endregion

        #region IsEditable

        /// <summary>
        /// Gets or sets if a label should display. This affects the (visual) state of the <see cref="TextBox"/>. 
        /// This property differs from (but is related to) <see cref="CanLabelProperty"/>, which only affects 
        /// the (logical) state of the <see cref="TextBox"/>. <see cref="CanLabelProperty"/> for more details.
        /// </summary>
        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.RegisterAttached("IsEditable", typeof(bool), typeof(XTextBox), new FrameworkPropertyMetadata(true));
        public static bool GetIsEditable(TextBox i) => (bool)i.GetValue(IsEditableProperty);
        public static void SetIsEditable(TextBox i, bool input) => i.SetValue(IsEditableProperty, input);

        #endregion

        #region Label

        public static readonly DependencyProperty LabelProperty = DependencyProperty.RegisterAttached("Label", typeof(string), typeof(XTextBox), new FrameworkPropertyMetadata(null));
        public static string GetLabel(TextBox i) => (string)i.GetValue(LabelProperty);
        public static void SetLabel(TextBox i, string input) => i.SetValue(LabelProperty, input);

        #endregion

        #region LabelTemplate

        public static readonly DependencyProperty LabelTemplateProperty = DependencyProperty.RegisterAttached("LabelTemplate", typeof(DataTemplate), typeof(XTextBox), new FrameworkPropertyMetadata(null));
        public static DataTemplate GetLabelTemplate(TextBox i) => (DataTemplate)i.GetValue(LabelTemplateProperty);
        public static void SetLabelTemplate(TextBox i, DataTemplate input) => i.SetValue(LabelTemplateProperty, input);

        #endregion

        #region SelectAllOnTripleClick

        public static readonly DependencyProperty SelectAllOnTripleClickProperty = DependencyProperty.RegisterAttached("SelectAllOnTripleClick", typeof(bool), typeof(XTextBox), new FrameworkPropertyMetadata(false, OnSelectAllOnTripleClickChanged));
        public static bool GetSelectAllOnTripleClick(TextBox i) => (bool)i.GetValue(SelectAllOnTripleClickProperty);
        public static void SetSelectAllOnTripleClick(TextBox i, bool input) => i.SetValue(SelectAllOnTripleClickProperty, input);
        static void OnSelectAllOnTripleClickChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TextBox textBox)
                textBox.RegisterHandlerAttached((bool)e.NewValue, SelectAllOnTripleClickProperty, i => i.PreviewMouseLeftButtonDown += SelectAllOnTripleClick_PreviewMouseLeftButtonDown, i => i.MouseLeftButtonDown -= SelectAllOnTripleClick_PreviewMouseLeftButtonDown);
        }

        static void SelectAllOnTripleClick_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (GetIsEditable(textBox))
                {
                    if (textBox.IsKeyboardFocusWithin)
                    {
                        if (e.OriginalSource?.GetType().FullName == XTextBoxBase.DefaultMouseTarget)
                        {
                            switch (e.ClickCount)
                            {
                                case 3:
                                    textBox.SelectAll();
                                    e.Handled = true;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region XTextBox

        static XTextBox()
        {
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.KeyDownEvent,
                new KeyEventHandler(OnKeyDown), true);
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.LoadedEvent,
                new RoutedEventHandler(OnLoaded), true);
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.LostFocusEvent,
                new RoutedEventHandler(OnLostFocus), true);
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.MouseDoubleClickEvent,
                new MouseButtonEventHandler(OnMouseDoubleClick), true);
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.MouseDownEvent,
                new MouseButtonEventHandler(OnMouseDown), true);
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.MouseUpEvent,
                new MouseButtonEventHandler(OnMouseUp), true);
        }

        //...

        static void OnEdited(TextBox box)
        {
            GetEditedCommand(box)?.Execute(GetEditedCommandParameter(box) ?? box.Text);
            box.RaiseEvent(new RoutedEventArgs(EditedEvent));
        }

        static void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox box)
                box.AddOnce(new CommandBinding(EditCommand, OnEdit, OnCanEdit));
        }

        static void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox box)
            {
                if (e.Key == Key.Enter)
                {
                    if (GetCanLabel(box))
                    {
                        e.Handled = true;
                        SetIsEditable(box, false);
                        OnEdited(box);
                    }
                }
            }
        }

        static void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox box)
            {
                if (GetCanLabel(box))
                {
                    SetIsEditable(box, false);
                    OnEdited(box);
                }
            }
        }

        static void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox box)
            {
                if (box.IsReadOnly)
                    return;

                if (!GetIsEditable(box))
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        if (GetEditMouseEvent(box) == MouseEvent.MouseDoubleClick)
                        {
                            SetIsEditable(box, true);
                            box.Focus();

                            e.Handled = true;
                        }
                    }
                }
            }
        }

        static void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox box)
            {
                if (box.IsReadOnly)
                    return;

                if (!GetIsEditable(box))
                {
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        var handle = false;
                        switch (GetEditMouseEvent(box))
                        {
                            case MouseEvent.DelayedMouseDown:
                                if (GetEditMouseDown(box) == 1)
                                {
                                    SetEditMouseDown(box, 0);
                                    handle = true;
                                }
                                else SetEditMouseDown(box, GetEditMouseDown(box) + 1);
                                break;

                            case MouseEvent.MouseDown:
                                handle = true;
                                break;
                        }

                        if (handle)
                        {
                            SetIsEditable(box, true);
                            box.Focus();

                            e.Handled = true;
                        }
                    }
                }
            }
        }

        static void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox box)
            {
                if (box.IsReadOnly)
                    return;

                if (!GetIsEditable(box))
                {
                    if (e.LeftButton == MouseButtonState.Released)
                    {
                        if (GetEditMouseEvent(box) == MouseEvent.MouseUp)
                        {
                            SetIsEditable(box, true);
                            box.Focus();

                            e.Handled = true;
                        }
                    }
                }
            }
        }

        #endregion
    }
}