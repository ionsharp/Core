using Imagin.Common.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Linq
{
    [Extends(typeof(Control))]
    public static class XControl
    {
        #region Properties

        #region (private) DropAdorner

        static readonly DependencyProperty DropAdornerProperty = DependencyProperty.RegisterAttached("DropAdorner", typeof(DropAdorner), typeof(XControl), new FrameworkPropertyMetadata(null));
        static DropAdorner GetDropAdorner(Control i) => (DropAdorner)i.GetValue(DropAdornerProperty);
        static void SetDropAdorner(Control i, DropAdorner input) => i.SetValue(DropAdornerProperty, input);

        #endregion

        #region DropTemplate

        public static readonly DependencyProperty DropTemplateProperty = DependencyProperty.RegisterAttached("DropTemplate", typeof(DataTemplate), typeof(XControl), new FrameworkPropertyMetadata(null));
        public static DataTemplate GetDropTemplate(Control i) => (DataTemplate)i.GetValue(DropTemplateProperty);
        public static void SetDropTemplate(Control i, DataTemplate input) => i.SetValue(DropTemplateProperty, input);

        #endregion

        #region DropTemplateVisibility

        public static readonly DependencyProperty DropTemplateVisibilityProperty = DependencyProperty.RegisterAttached("DropTemplateVisibility", typeof(Visibility), typeof(XControl), new FrameworkPropertyMetadata(Visibility.Collapsed, OnDropTemplateVisibilityChanged));
        public static Visibility GetDropTemplateVisibility(Control i) => (Visibility)i.GetValue(DropTemplateVisibilityProperty);
        public static void SetDropTemplateVisibility(Control i, Visibility input) => i.SetValue(DropTemplateVisibilityProperty, input);
        static void OnDropTemplateVisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Control control)
            {
                control.RegisterHandlerAttached((Visibility)e.NewValue == Visibility.Visible, DropTemplateVisibilityProperty, i =>
                {
                    var result = i.AddAdorner<DropAdorner>(() => new(i));
                    SetDropAdorner(i, result);
                }, i =>
                {
                    i.RemoveAdorners<DropAdorner>();
                    SetDropAdorner(i, null);
                });
            }
        }

        #endregion

        #region FontScale

        public static readonly DependencyProperty FontScaleProperty = DependencyProperty.RegisterAttached("FontScale", typeof(double), typeof(XControl), new FrameworkPropertyMetadata(1.0, OnFontScaleChanged));
        public static double GetFontScale(Control i) => (double)i.GetValue(FontScaleProperty);
        public static void SetFontScale(Control i, double value) => i.SetValue(FontScaleProperty, value);
        static void OnFontScaleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as Control;
            control.FontSize = GetFontScaleOrigin(control) * (double)e.NewValue;
        }

        #endregion

        #region FontScaleOrigin

        public static readonly DependencyProperty FontScaleOriginProperty = DependencyProperty.RegisterAttached("FontScaleOrigin", typeof(double), typeof(XControl), new FrameworkPropertyMetadata(12.0, OnFontScaleOriginChanged));
        public static double GetFontScaleOrigin(Control i) => (double)i.GetValue(FontScaleOriginProperty);
        public static void SetFontScaleOrigin(Control i, double value) => i.SetValue(FontScaleOriginProperty, value);
        static void OnFontScaleOriginChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as Control;
            control.FontSize = (double)e.NewValue * GetFontScale(control);
        }

        #endregion

        #region IgnoreDrop

        public static readonly DependencyProperty IgnoreDropProperty = DependencyProperty.RegisterAttached("IgnoreDrop", typeof(bool), typeof(XControl), new FrameworkPropertyMetadata(false));
        public static bool GetIgnoreDrop(Control i) => (bool)i.GetValue(IgnoreDropProperty);
        public static void SetIgnoreDrop(Control i, bool input) => i.SetValue(IgnoreDropProperty, input);

        #endregion

        #region IsDraggingOver

        static bool CanDrop;

        static Control currentDropTarget;
        public static Control CurrentDropTarget
        {
            get => currentDropTarget;
            private set => currentDropTarget = value;
        }

        static readonly DependencyPropertyKey IsDraggingOverKey = DependencyProperty.RegisterAttachedReadOnly("IsDraggingOver", typeof(bool), typeof(XControl), new FrameworkPropertyMetadata(null, new CoerceValueCallback(OnIsDraggingOverCoerced)));
        public static readonly DependencyProperty IsDraggingOverProperty = IsDraggingOverKey.DependencyProperty;
        public static bool GetIsDraggingOver(Control i) => (bool)i.GetValue(IsDraggingOverProperty);
        static object OnIsDraggingOverCoerced(DependencyObject i, object input) => i == CurrentDropTarget && CanDrop && !GetIgnoreDrop(CurrentDropTarget);

        #endregion

        #region IsMouseDirectlyOver

        /// <summary>
        /// Gets or sets the current element directly under the mouse.
        /// </summary>
        static Control MouseDirectlyOver = null;

        static readonly DependencyPropertyKey IsMouseDirectlyOverKey = DependencyProperty.RegisterAttachedReadOnly("IsMouseDirectlyOver", typeof(bool), typeof(XControl), new FrameworkPropertyMetadata(null, new CoerceValueCallback(OnIsMouseDirectlyOverCoerced)));
        /// <summary>
        /// Gets whether or not the mouse is directly over an element (closest logically, not visually).
        /// </summary>
        public static readonly DependencyProperty IsMouseDirectlyOverProperty = IsMouseDirectlyOverKey.DependencyProperty;
        public static bool GetIsMouseDirectlyOver(Control i) => (bool)i.GetValue(IsMouseDirectlyOverProperty);
        static object OnIsMouseDirectlyOverCoerced(DependencyObject i, object value) => i == MouseDirectlyOver;

        #endregion

        #region (private) IsMouseDirectlyOverEvent

        static readonly RoutedEvent IsMouseDirectlyOverEvent = EventManager.RegisterRoutedEvent("IsMouseDirectlyOver", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(XControl));
        static void OnIsMouseDirectlyOver(object sender, RoutedEventArgs e)
        {
            //The mouse is now directly over this object
            MouseDirectlyOver = sender as Control;
            //Tell that object to recalculate
            MouseDirectlyOver.InvalidateProperty(IsMouseDirectlyOverProperty);
            //Prevent this event from notifying other objects higher in tree
            e.Handled = true;
        }

        #endregion

        #region MouseDoubleClickCommand

        public static readonly DependencyProperty MouseDoubleClickCommandProperty = DependencyProperty.RegisterAttached("MouseDoubleClickCommand", typeof(ICommand), typeof(XControl), new FrameworkPropertyMetadata(null, OnMouseDoubleClickCommandChanged));
        public static ICommand GetMouseDoubleClickCommand(Control i) => (ICommand)i.GetValue(MouseDoubleClickCommandProperty);
        public static void SetMouseDoubleClickCommand(Control i, ICommand input) => i.SetValue(MouseDoubleClickCommandProperty, input);
        static void OnMouseDoubleClickCommandChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Control control)
                control.RegisterHandlerAttached(e.NewValue != null, MouseDoubleClickCommandProperty, i => i.MouseDoubleClick += MouseDoubleClickCommand_MouseDoubleClick, i => i.MouseDoubleClick -= MouseDoubleClickCommand_MouseDoubleClick);
        }

        static void MouseDoubleClickCommand_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is Control control)
            {
                if (e.ChangedButton == GetMouseDoubleClickCommandButton(control))
                    GetMouseDoubleClickCommand(control).Execute(GetMouseDoubleClickCommandParameter(control));
            }
        }

        #endregion

        #region MouseDoubleClickCommandButton

        public static readonly DependencyProperty MouseDoubleClickCommandButtonProperty = DependencyProperty.RegisterAttached("MouseDoubleClickCommandButton", typeof(MouseButton), typeof(XControl), new FrameworkPropertyMetadata(MouseButton.Left));
        public static MouseButton GetMouseDoubleClickCommandButton(Control i) => (MouseButton)i.GetValue(MouseDoubleClickCommandButtonProperty);
        public static void SetMouseDoubleClickCommandButton(Control i, MouseButton input) => i.SetValue(MouseDoubleClickCommandButtonProperty, input);

        #endregion

        #region MouseDoubleClickCommandParameter

        public static readonly DependencyProperty MouseDoubleClickCommandParameterProperty = DependencyProperty.RegisterAttached("MouseDoubleClickCommandParameter", typeof(object), typeof(XControl), new FrameworkPropertyMetadata(null));
        public static object GetMouseDoubleClickCommandParameter(Control i) => i.GetValue(MouseDoubleClickCommandParameterProperty);
        public static void SetMouseDoubleClickCommandParameter(Control i, object input) => i.SetValue(MouseDoubleClickCommandParameterProperty, input);

        #endregion

        #endregion

        #region XControl

        static XControl()
        {
            //EventManager.RegisterClassHandler(typeof(UIElement), UIElement.MouseEnterEvent,
                //new MouseEventHandler(OnMouseEnterLeave), true);
            //EventManager.RegisterClassHandler(typeof(UIElement), UIElement.MouseLeaveEvent,
                //new MouseEventHandler(OnMouseEnterLeave), true);
            //EventManager.RegisterClassHandler(typeof(UIElement), IsMouseDirectlyOverEvent,
                //new RoutedEventHandler(OnIsMouseDirectlyOver));

            //...

            EventManager.RegisterClassHandler(typeof(Control), UIElement.PreviewDragEnterEvent,
                new DragEventHandler(OnPreviewDragEnterOver), true);
            EventManager.RegisterClassHandler(typeof(Control), UIElement.PreviewDragLeaveEvent,
                new DragEventHandler(OnPreviewDragLeave), true);
            EventManager.RegisterClassHandler(typeof(Control), UIElement.PreviewDragOverEvent,
                new DragEventHandler(OnPreviewDragEnterOver), true);
            EventManager.RegisterClassHandler(typeof(Control), UIElement.PreviewDropEvent,
                new DragEventHandler(OnPreviewDrop), true);
        }

        #endregion

        #region Methods

        static void OnMouseEnterLeave(object sender, MouseEventArgs args)
        {
            lock (IsMouseDirectlyOverProperty)
            {
                if (MouseDirectlyOver != null)
                {
                    var oldValue = MouseDirectlyOver;
                    MouseDirectlyOver = null;
                    oldValue.InvalidateProperty(IsMouseDirectlyOverProperty);
                }
                if (Mouse.DirectlyOver is IInputElement i)
                    i.RaiseEvent(new RoutedEventArgs(IsMouseDirectlyOverEvent));
            }
        }

        //...

        static void OnPreviewDragLeave(object sender, DragEventArgs e)
        {
            lock (IsDraggingOverProperty)
            {
                CanDrop = false;
                if (CurrentDropTarget != null)
                {
                    var oldValue = CurrentDropTarget;
                    CurrentDropTarget = null;
                    oldValue.InvalidateProperty(IsDraggingOverProperty);
                }
                if (sender is Control control)
                {
                    if (!GetIgnoreDrop(control))
                    {
                        CurrentDropTarget = control;
                        CurrentDropTarget.InvalidateProperty(IsDraggingOverProperty);
                    }
                }
            }
        }

        static void OnPreviewDragEnterOver(object sender, DragEventArgs e)
        {
            lock (IsDraggingOverProperty)
            {
                CanDrop = false;
                if (CurrentDropTarget != null)
                {
                    var oldValue = CurrentDropTarget;
                    CurrentDropTarget = null;
                    oldValue.InvalidateProperty(IsDraggingOverProperty);
                }

                if (e.Effects != DragDropEffects.None)
                    CanDrop = true;

                if (sender is Control control)
                {
                    if (!GetIgnoreDrop(control))
                    {
                        CurrentDropTarget = control;
                        CurrentDropTarget.InvalidateProperty(IsDraggingOverProperty);
                    }
                }
            }
        }

        static void OnPreviewDrop(object sender, DragEventArgs e)
        {
            lock (IsDraggingOverProperty)
            {
                CanDrop = false;
                if (CurrentDropTarget != null)
                    CurrentDropTarget.InvalidateProperty(IsDraggingOverProperty);

                if (sender is Control control)
                {
                    if (!GetIgnoreDrop(control))
                    {
                        CurrentDropTarget = control;
                        CurrentDropTarget.InvalidateProperty(IsDraggingOverProperty);
                    }
                }
            }
        }

        #endregion
    }
}