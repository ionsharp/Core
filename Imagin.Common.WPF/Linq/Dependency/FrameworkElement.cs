using GongSolutions.Wpf.DragDrop;
using Imagin.Common.Analytics;
using Imagin.Common.Collections;
using Imagin.Common.Controls;
using Imagin.Common.Media;
using Imagin.Common.Converters;
using Imagin.Common.Input;
using Imagin.Common.Numbers;
using Imagin.Common.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Linq
{
    [Extends(typeof(FrameworkElement))]
    public static class XElement
    {
        public static readonly ResourceKey<FrameworkElement> FocusVisualStyleKey = new();
        
        #region Constants

        public const double DisabledOpacity = 0.6;

        #endregion

        #region Properties

        #region CanResize

        public static readonly DependencyProperty CanResizeProperty = DependencyProperty.RegisterAttached("CanResize", typeof(bool), typeof(XElement), new FrameworkPropertyMetadata(false, OnCanResizeChanged));
        public static bool GetCanResize(FrameworkElement i) => (bool)i.GetValue(CanResizeProperty);
        public static void SetCanResize(FrameworkElement i, bool input) => i.SetValue(CanResizeProperty, input);
        static void OnCanResizeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
                element.RegisterHandlerAttached((bool)e.NewValue, CanResizeProperty, CanResize_Loaded, CanResize_Unloaded);
        }

        static void CanResize_Loaded(FrameworkElement i)
        {
            var result = i.AddAdorner<ResizeAdorner>(() => new(i));
            SetResizeAdorner(i, result);
        }

        static void CanResize_Unloaded(FrameworkElement i)
        {
            i.RemoveAdorners<ResizeAdorner>();
            SetResizeAdorner(i, null);
        }

        #endregion

        #region CanSelect

        public static readonly DependencyProperty CanSelectProperty = DependencyProperty.RegisterAttached("CanSelect", typeof(bool), typeof(XElement), new FrameworkPropertyMetadata(false, OnCanSelectChanged));
        public static bool GetCanSelect(FrameworkElement i) => (bool)i.GetValue(CanSelectProperty);
        public static void SetCanSelect(FrameworkElement i, bool input) => i.SetValue(CanSelectProperty, input);
        static void OnCanSelectChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                element.RegisterHandlerAttached((bool)e.NewValue, CanSelectProperty, i =>
                {
                    i.MouseDown
                        += CanSelect_MouseDown;
                    i.MouseMove
                        += CanSelect_MouseMove;
                    i.MouseUp
                        += CanSelect_MouseUp;

                    SetSelectionAdorner(i, i.AddAdorner<SelectionAdorner>(() => new(i)));
                }, i =>
                {
                    i.MouseDown
                        -= CanSelect_MouseDown;
                    i.MouseMove
                        -= CanSelect_MouseMove;
                    i.MouseUp
                        -= CanSelect_MouseUp;

                    i.RemoveAdorners<SelectionAdorner>();
                    SetSelectionAdorner(i, null);
                });
            }
        }

        //...

        static void CanSelect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (e.ChangedButton == GetSelectionButton(element))
                {
                    element.CaptureMouse();

                    var start = e.GetPosition(element);
                    SetSelectionStart(element, start);

                    var selection = GetSelection(element);
                    selection.X = start.X; selection.Y = start.Y;
                    selection.Height = selection.Width = 0;
                }
            }
        }

        static void CanSelect_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (GetSelectionStart(element) != null)
                {
                    var result = CalculateSelection(GetSelectionStart(element).Value, e.GetPosition(element));

                    var selection
                        = GetSelection(element);
                    selection.X 
                        = result.X;
                    selection.Y 
                        = result.Y;
                    selection.Height 
                        = result.Height;
                    selection.Width 
                        = result.Width;
                }
            }
        }

        static void CanSelect_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                SetSelectionStart(element, null);

                if (element.IsMouseCaptured)
                    element.ReleaseMouseCapture();

                var selection = GetSelection(element);

                OnSelected(element, selection);
                if (GetSelectionResets(element))
                    selection.X = selection.Y = selection.Height = selection.Width = 0;
            }
        }

        //...

        static Rect CalculateSelection(Point a, Point b)
        {
            Rect result = new();
            double
                x = (a.X < b.X ? a.X : b.X),
                y = (a.Y < b.Y ? a.Y : b.Y);

            result.Size = new Size(Math.Abs(b.X - a.X), Math.Abs(b.Y - a.Y));
            result.X = x;
            result.Y = y;
            return result;
        }

        //...

        static void OnSelected(FrameworkElement element, DoubleRegion selection)
            => element.RaiseEvent(new RoutedEventArgs<DoubleRegion>(SelectedEvent, selection, element));

        #endregion

        #region Cursor

        public static readonly DependencyProperty CursorProperty = DependencyProperty.RegisterAttached("Cursor", typeof(Uri), typeof(XElement), new FrameworkPropertyMetadata(null, OnCursorChanged));
        public static Uri GetCursor(FrameworkElement i) => (Uri)i.GetValue(CursorProperty);
        public static void SetCursor(FrameworkElement i, Uri input) => i.SetValue(CursorProperty, input);
        static void OnCursorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (e.NewValue is Uri uri)
                {
                    var bitmap = new ImageSourceConverter().ConvertFromString(uri.OriginalString).As<ImageSource>().Bitmap(ImageExtensions.Png);
                    element.Cursor = bitmap.Cursor(bitmap.Width / 2, bitmap.Height / 2).Convert();
                }
            }
        }

        #endregion

        #region CursorBitmap

        public static readonly DependencyProperty CursorBitmapProperty = DependencyProperty.RegisterAttached("CursorBitmap", typeof(WriteableBitmap), typeof(XElement), new FrameworkPropertyMetadata(null, OnCursorBitmapChanged));
        public static WriteableBitmap GetCursorBitmap(FrameworkElement i) => (WriteableBitmap)i.GetValue(CursorBitmapProperty);
        public static void SetCursorBitmap(FrameworkElement i, WriteableBitmap input) => i.SetValue(CursorBitmapProperty, input);
        static void OnCursorBitmapChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (e.NewValue is WriteableBitmap bitmap)
                    element.Cursor = bitmap.Bitmap(ImageExtensions.Png).Cursor(bitmap.PixelWidth / 2, bitmap.PixelHeight / 2).Convert();
            }
        }

        #endregion

        #region DefaultDropHandler

        public static readonly DependencyProperty DefaultDropHandlerProperty = DependencyProperty.RegisterAttached("DefaultDropHandler", typeof(IDropTarget), typeof(XElement), new FrameworkPropertyMetadata(null));
        public static IDropTarget GetDefaultDropHandler(FrameworkElement i) => i.GetValueOrSetDefault<IDropTarget>(DefaultDropHandlerProperty, () => new DefaultDropHandler(i));

        #endregion

        #region DragMoveWindow

        public static readonly DependencyProperty DragMoveWindowProperty = DependencyProperty.RegisterAttached("DragMoveWindow", typeof(bool), typeof(XElement), new FrameworkPropertyMetadata(false, OnDragMoveWindowChanged));
        public static bool GetDragMoveWindow(FrameworkElement i) => (bool)i.GetValue(DragMoveWindowProperty);
        public static void SetDragMoveWindow(FrameworkElement i, bool input) => i.SetValue(DragMoveWindowProperty, input);
        static void OnDragMoveWindowChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
                element.RegisterHandlerAttached((bool)e.NewValue, DragMoveWindowProperty, i => i.MouseDown += DragMoveWindow_MouseDown, i => i.MouseDown -= DragMoveWindow_MouseDown);
        }

        static void DragMoveWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    element.FindParent<Window>()?.DragMove();
            }
        }

        #endregion

        #region FadeIn

        public static readonly DependencyProperty FadeInProperty = DependencyProperty.RegisterAttached("FadeIn", typeof(bool), typeof(XElement), new FrameworkPropertyMetadata(false, OnFadeInChanged));
        public static bool GetFadeIn(FrameworkElement i) => (bool)i.GetValue(FadeInProperty);
        public static void SetFadeIn(FrameworkElement i, bool value) => i.SetValue(FadeInProperty, value);
        static void OnFadeInChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                element.SetCurrentValue(FrameworkElement.OpacityProperty, (bool)e.NewValue ? 0d : 1d);
                element.RegisterHandlerAttached((bool)e.NewValue, FadeInProperty, i =>
                {
                    var duration = GetFadeInDuration(i);
                    _ = i.FadeIn(duration == default ? new Duration(TimeSpan.FromSeconds(0.5)) : duration);
                }, null);
            }
        }

        #endregion

        #region FadeInDuration

        public static readonly DependencyProperty FadeInDurationProperty = DependencyProperty.RegisterAttached("FadeInDuration", typeof(Duration), typeof(XElement), new FrameworkPropertyMetadata(default(Duration)));
        public static Duration GetFadeInDuration(FrameworkElement i) => (Duration)i.GetValue(FadeInDurationProperty);
        public static void SetFadeInDuration(FrameworkElement i, Duration input) => i.SetValue(FadeInDurationProperty, input);

        #endregion

        #region FadeOut

        public static readonly DependencyProperty FadeOutProperty = DependencyProperty.RegisterAttached("FadeOut", typeof(bool), typeof(XElement), new FrameworkPropertyMetadata(false, OnFadeOutChanged));
        public static bool GetFadeOut(FrameworkElement i) => (bool)i.GetValue(FadeOutProperty);
        public static void SetFadeOut(FrameworkElement i, bool value) => i.SetValue(FadeOutProperty, value);
        static void OnFadeOutChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                element.RegisterHandlerAttached((bool)e.NewValue, FadeOutProperty, null, i =>
                {
                    var duration = GetFadeOutDuration(i);
                    _ = i.FadeOut(duration == default ? new Duration(TimeSpan.FromSeconds(0.5)) : duration);
                });
            }
        }

        #endregion

        #region FadeOutDuration

        public static readonly DependencyProperty FadeOutDurationProperty = DependencyProperty.RegisterAttached("FadeOutDuration", typeof(Duration), typeof(XElement), new FrameworkPropertyMetadata(default(Duration)));
        public static Duration GetFadeOutDuration(FrameworkElement i) => (Duration)i.GetValue(FadeOutDurationProperty);
        public static void SetFadeOutDuration(FrameworkElement i, Duration input) => i.SetValue(FadeOutDurationProperty, input);

        #endregion

        #region FadeTrigger

        static void FadeIn(FrameworkElement input)
        {
            var animation = new DoubleAnimation()
            {
                Duration
                    = GetFadeTriggerDuration(input),
                From
                    = input.Opacity,
                To
                    = 1,
            };
            Storyboard.SetTarget(animation, input);
            Storyboard.SetTargetProperty(animation, new PropertyPath(nameof(UIElement.Opacity)));

            input.IsHitTestVisible = true;

            var result = new Storyboard();
            result.Children.Add(animation);
            result.Begin(input, HandoffBehavior.SnapshotAndReplace);
        }

        static void FadeOut(FrameworkElement input)
        {
            var animation = new DoubleAnimation()
            {
                Duration
                    = GetFadeTriggerDuration(input),
                From
                    = input.Opacity,
                To
                    = 0
            };
            Storyboard.SetTarget(animation, input);
            Storyboard.SetTargetProperty(animation, new PropertyPath(nameof(UIElement.Opacity)));

            input.IsHitTestVisible = false;

            var result = new Storyboard();
            result.Children.Add(animation);
            result.Begin(input, HandoffBehavior.SnapshotAndReplace);
        }

        public static readonly DependencyProperty FadeTriggerProperty = DependencyProperty.RegisterAttached("FadeTrigger", typeof(bool), typeof(XElement), new FrameworkPropertyMetadata(false, OnFadeTriggerChanged));
        public static bool GetFadeTrigger(FrameworkElement i) => (bool)i.GetValue(FadeTriggerProperty);
        public static void SetFadeTrigger(FrameworkElement i, bool value) => i.SetValue(FadeTriggerProperty, value);
        static void OnFadeTriggerChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if ((bool)e.NewValue)
                {
                    if (GetFadeTriggerSource(element))
                        FadeIn(element);

                    else FadeOut(element);
                }
            }
        }

        #endregion

        #region FadeTriggerDuration

        public static readonly DependencyProperty FadeTriggerDurationProperty = DependencyProperty.RegisterAttached("FadeTriggerDuration", typeof(TimeSpan), typeof(XElement), new FrameworkPropertyMetadata(0.8.Seconds()));
        public static TimeSpan GetFadeTriggerDuration(FrameworkElement i) => (TimeSpan)i.GetValue(FadeTriggerDurationProperty);
        public static void SetFadeTriggerDuration(FrameworkElement i, TimeSpan value) => i.SetValue(FadeTriggerDurationProperty, value);

        #endregion

        #region FadeTriggerSource

        public static readonly DependencyProperty FadeTriggerSourceProperty = DependencyProperty.RegisterAttached("FadeTriggerSource", typeof(bool), typeof(XElement), new FrameworkPropertyMetadata(false, OnFadeTriggerSourceChanged));
        public static bool GetFadeTriggerSource(FrameworkElement i) => (bool)i.GetValue(FadeTriggerSourceProperty);
        public static void SetFadeTriggerSource(FrameworkElement i, bool value) => i.SetValue(FadeTriggerSourceProperty, value);
        static void OnFadeTriggerSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (GetFadeTrigger(element))
                {
                    if ((bool)e.NewValue)
                        FadeIn(element);

                    else FadeOut(element);
                }
            }
        }

        #endregion

        #region Handler

        public static readonly DependencyProperty HandlerProperty = DependencyProperty.RegisterAttached("Handler", typeof(FrameworkEventHandler), typeof(XElement), new FrameworkPropertyMetadata(null));
        static FrameworkEventHandler GetHandler(this FrameworkElement i) => i.GetValueOrSetDefault<FrameworkEventHandler>(HandlerProperty, () => new(i));

        //...

        public static void RegisterHandler(this FrameworkElement i, Action load, Action unload = null)
            => i.GetHandler().Add(load, unload);

        public static void RegisterHandler<T>(this T i, Action<T> load, Action<T> unload = null) where T : FrameworkElement
            => i.GetHandler().Add(load, unload);

        public static void RegisterHandlerAttached<T>(this T i, bool add, object key, Action<T> load, Action<T> unload = null) where T : FrameworkElement
            => i.GetHandler().AddAttached(add, key, load, unload);

        #endregion

        #region InheritChild

        public static readonly DependencyProperty InheritChildProperty = DependencyProperty.RegisterAttached("InheritChild", typeof(Type), typeof(XElement), new FrameworkPropertyMetadata(null, OnInheritChildChanged));
        public static Type GetInheritChild(FrameworkElement i) => (Type)i.GetValue(InheritChildProperty);
        public static void SetInheritChild(FrameworkElement i, Type value) => i.SetValue(InheritChildProperty, value);
        static void OnInheritChildChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
                element.RegisterHandlerAttached(e.NewValue != null, InheritChildProperty, InheritChild_Loaded, InheritChild_Unloaded);
        }

        static void InheritChild_Loaded(FrameworkElement a)
        {
            var property = GetInheritChildProperty(a);
            if (property != null)
            {
                var b = Try.Invoke(() => a.FindChildOfType(GetInheritChild(a)));
                if (b != null)
                    a.Bind(property, property.Name, b);
            }
        }

        static void InheritChild_Unloaded(FrameworkElement input)
            => GetInheritChildProperty(input).If(i => input.Unbind(i));

        #endregion

        #region InheritChildProperty

        public static readonly DependencyProperty InheritChildPropertyProperty = DependencyProperty.RegisterAttached("InheritChildProperty", typeof(DependencyProperty), typeof(XElement), new FrameworkPropertyMetadata(null));
        public static DependencyProperty GetInheritChildProperty(FrameworkElement i) => (DependencyProperty)i.GetValue(InheritChildPropertyProperty);
        public static void SetInheritChildProperty(FrameworkElement i, DependencyProperty value) => i.SetValue(InheritChildPropertyProperty, value);

        #endregion

        #region IsMouseDown

        static readonly DependencyPropertyKey IsMouseDownKey = DependencyProperty.RegisterAttachedReadOnly("IsMouseDown", typeof(bool), typeof(XElement), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsMouseDownProperty = IsMouseDownKey.DependencyProperty;
        public static bool GetIsMouseDown(FrameworkElement i) => (bool)i.GetValue(IsMouseDownProperty);
        static void SetIsMouseDown(this FrameworkElement i, bool input) => i.SetValue(IsMouseDownKey, input);

        #endregion

        #region IsMouseLeftButtonDown

        static readonly DependencyPropertyKey IsMouseLeftButtonDownKey = DependencyProperty.RegisterAttachedReadOnly("IsMouseLeftButtonDown", typeof(bool), typeof(XElement), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsMouseLeftButtonDownProperty = IsMouseLeftButtonDownKey.DependencyProperty;
        public static bool GetIsMouseLeftButtonDown(FrameworkElement i) => (bool)i.GetValue(IsMouseLeftButtonDownProperty);
        static void SetIsMouseLeftButtonDown(this FrameworkElement i, bool input) => i.SetValue(IsMouseLeftButtonDownKey, input);

        #endregion

        #region IsMouseMiddleButtonDown

        static readonly DependencyPropertyKey IsMouseMiddleButtonDownKey = DependencyProperty.RegisterAttachedReadOnly("IsMouseMiddleButtonDown", typeof(bool), typeof(XElement), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsMouseMiddleButtonDownProperty = IsMouseMiddleButtonDownKey.DependencyProperty;
        public static bool GetIsMouseMiddleButtonDown(FrameworkElement i) => (bool)i.GetValue(IsMouseMiddleButtonDownProperty);
        static void SetIsMouseMiddleButtonDown(this FrameworkElement i, bool input) => i.SetValue(IsMouseMiddleButtonDownKey, input);

        #endregion

        #region IsMouseRightButtonDown

        static readonly DependencyPropertyKey IsMouseRightButtonDownKey = DependencyProperty.RegisterAttachedReadOnly("IsMouseRightButtonDown", typeof(bool), typeof(XElement), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsMouseRightButtonDownProperty = IsMouseRightButtonDownKey.DependencyProperty;
        public static bool GetIsMouseRightButtonDown(FrameworkElement i) => (bool)i.GetValue(IsMouseRightButtonDownProperty);
        static void SetIsMouseRightButtonDown(this FrameworkElement i, bool input) => i.SetValue(IsMouseRightButtonDownKey, input);

        #endregion

        #region LostFocusCommand

        public static readonly DependencyProperty LostFocusCommandProperty = DependencyProperty.RegisterAttached("LostFocusCommand", typeof(ICommand), typeof(XElement), new FrameworkPropertyMetadata(null, OnLostFocusCommandChanged));
        public static ICommand GetLostFocusCommand(FrameworkElement i) => (ICommand)i.GetValue(LostFocusCommandProperty);
        public static void SetLostFocusCommand(FrameworkElement i, ICommand input) => i.SetValue(LostFocusCommandProperty, input);
        static void OnLostFocusCommandChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
                element.RegisterHandlerAttached(e.NewValue != null, LostFocusCommandProperty, i => i.LostFocus += LostFocusCommand_LostFocus, i => i.LostFocus -= LostFocusCommand_LostFocus);
        }

        static void LostFocusCommand_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
                GetLostFocusCommand(element).Execute(GetLostFocusCommandParameter(element));
        }

        #endregion

        #region LostFocusCommandParameter

        public static readonly DependencyProperty LostFocusCommandParameterProperty = DependencyProperty.RegisterAttached("LostFocusCommandParameter", typeof(object), typeof(XElement), new FrameworkPropertyMetadata(null));
        public static object GetLostFocusCommandParameter(FrameworkElement i) => (object)i.GetValue(LostFocusCommandParameterProperty);
        public static void SetLostFocusCommandParameter(FrameworkElement i, object input) => i.SetValue(LostFocusCommandParameterProperty, input);

        #endregion

        //...

        #region MouseDownCommand

        public static readonly DependencyProperty MouseDownCommandProperty = DependencyProperty.RegisterAttached("MouseDownCommand", typeof(ICommand), typeof(XElement), new FrameworkPropertyMetadata(null, OnMouseDownCommandChanged));
        public static ICommand GetMouseDownCommand(FrameworkElement i) => (ICommand)i.GetValue(MouseDownCommandProperty);
        public static void SetMouseDownCommand(FrameworkElement i, ICommand input) => i.SetValue(MouseDownCommandProperty, input);
        static void OnMouseDownCommandChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
                element.RegisterHandlerAttached(e.NewValue != null, MouseDownCommandProperty, i => i.MouseDown += MouseDownCommand_MouseDown, i => i.MouseDown -= MouseDownCommand_MouseDown);
        }

        static void MouseDownCommand_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (e.ChangedButton == GetMouseDownCommandButton(element))
                    GetMouseDownCommand(element).Execute(GetMouseDownCommandParameter(element));
            }
        }

        #endregion

        #region MouseDownCommandButton

        public static readonly DependencyProperty MouseDownCommandButtonProperty = DependencyProperty.RegisterAttached("MouseDownCommandButton", typeof(MouseButton), typeof(XElement), new FrameworkPropertyMetadata(MouseButton.Left));
        public static MouseButton GetMouseDownCommandButton(FrameworkElement i) => (MouseButton)i.GetValue(MouseDownCommandButtonProperty);
        public static void SetMouseDownCommandButton(FrameworkElement i, MouseButton input) => i.SetValue(MouseDownCommandButtonProperty, input);

        #endregion

        #region MouseDownCommandParameter

        public static readonly DependencyProperty MouseDownCommandParameterProperty = DependencyProperty.RegisterAttached("MouseDownCommandParameter", typeof(object), typeof(XElement), new FrameworkPropertyMetadata(null));
        public static object GetMouseDownCommandParameter(FrameworkElement i) => i.GetValue(MouseDownCommandParameterProperty);
        public static void SetMouseDownCommandParameter(FrameworkElement i, object input) => i.SetValue(MouseDownCommandParameterProperty, input);

        #endregion

        //...

        #region MouseUpCommand

        public static readonly DependencyProperty MouseUpCommandProperty = DependencyProperty.RegisterAttached("MouseUpCommand", typeof(ICommand), typeof(XElement), new FrameworkPropertyMetadata(null, OnMouseUpCommandChanged));
        public static ICommand GetMouseUpCommand(FrameworkElement i) => (ICommand)i.GetValue(MouseUpCommandProperty);
        public static void SetMouseUpCommand(FrameworkElement i, ICommand input) => i.SetValue(MouseUpCommandProperty, input);
        static void OnMouseUpCommandChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
                element.RegisterHandlerAttached(e.NewValue != null, MouseUpCommandProperty, i => i.MouseUp += MouseUpCommand_MouseUp, i => i.MouseUp -= MouseUpCommand_MouseUp);
        }

        static void MouseUpCommand_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (e.ChangedButton == GetMouseUpCommandButton(element))
                    GetMouseUpCommand(element).Execute(GetMouseUpCommandParameter(element));
            }
        }

        #endregion

        #region MouseUpCommandButton

        public static readonly DependencyProperty MouseUpCommandButtonProperty = DependencyProperty.RegisterAttached("MouseUpCommandButton", typeof(MouseButton), typeof(XElement), new FrameworkPropertyMetadata(MouseButton.Left));
        public static MouseButton GetMouseUpCommandButton(FrameworkElement i) => (MouseButton)i.GetValue(MouseUpCommandButtonProperty);
        public static void SetMouseUpCommandButton(FrameworkElement i, MouseButton input) => i.SetValue(MouseUpCommandButtonProperty, input);

        #endregion

        #region MouseUpCommandParameter

        public static readonly DependencyProperty MouseUpCommandParameterProperty = DependencyProperty.RegisterAttached("MouseUpCommandParameter", typeof(object), typeof(XElement), new FrameworkPropertyMetadata(null));
        public static object GetMouseUpCommandParameter(FrameworkElement i) => i.GetValue(MouseUpCommandParameterProperty);
        public static void SetMouseUpCommandParameter(FrameworkElement i, object input) => i.SetValue(MouseUpCommandParameterProperty, input);

        #endregion

        //...

        #region MouseEnterCommand

        public static readonly DependencyProperty MouseEnterCommandProperty = DependencyProperty.RegisterAttached("MouseEnterCommand", typeof(ICommand), typeof(XElement), new FrameworkPropertyMetadata(null, OnMouseEnterCommandChanged));
        public static ICommand GetMouseEnterCommand(FrameworkElement i) => (ICommand)i.GetValue(MouseEnterCommandProperty);
        public static void SetMouseEnterCommand(FrameworkElement i, ICommand input) => i.SetValue(MouseEnterCommandProperty, input);
        static void OnMouseEnterCommandChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
                element.RegisterHandlerAttached(e.NewValue != null, MouseEnterCommandProperty, i => i.MouseEnter += MouseEnterCommand_MouseEnter, i => i.MouseEnter -= MouseEnterCommand_MouseEnter);
        }

        static void MouseEnterCommand_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is FrameworkElement element)
                GetMouseEnterCommand(element).Execute(GetMouseEnterCommandParameter(element));
        }

        #endregion

        #region MouseEnterCommandParameter

        public static readonly DependencyProperty MouseEnterCommandParameterProperty = DependencyProperty.RegisterAttached("MouseEnterCommandParameter", typeof(object), typeof(XElement), new FrameworkPropertyMetadata(null));
        public static object GetMouseEnterCommandParameter(FrameworkElement i) => (object)i.GetValue(MouseEnterCommandParameterProperty);
        public static void SetMouseEnterCommandParameter(FrameworkElement i, object input) => i.SetValue(MouseEnterCommandParameterProperty, input);

        #endregion

        #region MouseLeaveCommand

        public static readonly DependencyProperty MouseLeaveCommandProperty = DependencyProperty.RegisterAttached("MouseLeaveCommand", typeof(ICommand), typeof(XElement), new FrameworkPropertyMetadata(null, OnMouseLeaveCommandChanged));
        public static ICommand GetMouseLeaveCommand(FrameworkElement i) => (ICommand)i.GetValue(MouseLeaveCommandProperty);
        public static void SetMouseLeaveCommand(FrameworkElement i, ICommand input) => i.SetValue(MouseLeaveCommandProperty, input);
        static void OnMouseLeaveCommandChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
                element.RegisterHandlerAttached(e.NewValue != null, MouseLeaveCommandProperty, i => i.MouseLeave += MouseLeaveCommand_MouseLeave, i => i.MouseLeave -= MouseLeaveCommand_MouseLeave);
        }

        static void MouseLeaveCommand_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is FrameworkElement element)
                GetMouseLeaveCommand(element).Execute(GetMouseLeaveCommandParameter(element));
        }

        #endregion

        #region MouseLeaveCommandParameter

        public static readonly DependencyProperty MouseLeaveCommandParameterProperty = DependencyProperty.RegisterAttached("MouseLeaveCommandParameter", typeof(object), typeof(XElement), new FrameworkPropertyMetadata(null));
        public static object GetMouseLeaveCommandParameter(FrameworkElement i) => (object)i.GetValue(MouseLeaveCommandParameterProperty);
        public static void SetMouseLeaveCommandParameter(FrameworkElement i, object input) => i.SetValue(MouseLeaveCommandParameterProperty, input);

        #endregion

        #region Name

        public static readonly DependencyProperty NameProperty = DependencyProperty.RegisterAttached("Name", typeof(IFrameworkKey), typeof(XElement), new FrameworkPropertyMetadata(null));
        public static IFrameworkKey GetName(FrameworkElement i) => (IFrameworkKey)i.GetValue(NameProperty);
        public static void SetName(FrameworkElement i, IFrameworkKey input) => i.SetValue(NameProperty, input);

        #endregion

        #region OverrideHorizontalAlignment

        public static readonly DependencyProperty OverrideHorizontalAlignmentProperty = DependencyProperty.RegisterAttached("OverrideHorizontalAlignment", typeof(HorizontalAlignment?), typeof(XElement), new FrameworkPropertyMetadata(null, OnOverrideHorizontalAlignmentChanged));
        public static HorizontalAlignment? GetOverrideHorizontalAlignment(FrameworkElement i) => (HorizontalAlignment?)i.GetValue(OverrideHorizontalAlignmentProperty);
        public static void SetOverrideHorizontalAlignment(FrameworkElement i, HorizontalAlignment? input) => i.SetValue(OverrideHorizontalAlignmentProperty, input);
        static void OnOverrideHorizontalAlignmentChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (e.NewValue is HorizontalAlignment alignment)
                    element.HorizontalAlignment = alignment;
            }
        }

        #endregion

        #region OverrideMargin

        public static readonly DependencyProperty OverrideMarginProperty = DependencyProperty.RegisterAttached("OverrideMargin", typeof(Thickness?), typeof(XElement), new FrameworkPropertyMetadata(null, OnOverrideMarginChanged));
        public static Thickness? GetOverrideMargin(FrameworkElement i) => (Thickness?)i.GetValue(OverrideMarginProperty);
        public static void SetOverrideMargin(FrameworkElement i, Thickness? value) => i.SetValue(OverrideMarginProperty, value);
        static void OnOverrideMarginChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (e.NewValue is Thickness thickness)
                    element.Margin = thickness;
            }
        }

        #endregion

        #region OverrideVerticalAlignment

        public static readonly DependencyProperty OverrideVerticalAlignmentProperty = DependencyProperty.RegisterAttached("OverrideVerticalAlignment", typeof(VerticalAlignment?), typeof(XElement), new FrameworkPropertyMetadata(null, OnOverrideVerticalAlignmentChanged));
        public static VerticalAlignment? GetOverrideVerticalAlignment(FrameworkElement i) => (VerticalAlignment?)i.GetValue(OverrideVerticalAlignmentProperty);
        public static void SetOverrideVerticalAlignment(FrameworkElement i, VerticalAlignment? input) => i.SetValue(OverrideVerticalAlignmentProperty, input);
        static void OnOverrideVerticalAlignmentChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (e.NewValue is VerticalAlignment alignment)
                    element.VerticalAlignment = alignment;
            }
        }

        #endregion
        
        #region PreviewMouseLeftButtonUpCommand

        public static readonly DependencyProperty PreviewMouseLeftButtonUpCommandProperty = DependencyProperty.RegisterAttached("PreviewMouseLeftButtonUpCommand", typeof(ICommand), typeof(XElement), new FrameworkPropertyMetadata(null, OnPreviewMouseLeftButtonUpCommandChanged));
        public static ICommand GetPreviewMouseLeftButtonUpCommand(FrameworkElement i) => (ICommand)i.GetValue(PreviewMouseLeftButtonUpCommandProperty);
        public static void SetPreviewMouseLeftButtonUpCommand(FrameworkElement i, ICommand input) => i.SetValue(PreviewMouseLeftButtonUpCommandProperty, input);
        static void OnPreviewMouseLeftButtonUpCommandChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
                element.RegisterHandlerAttached(e.NewValue != null, PreviewMouseLeftButtonUpCommandProperty, i => i.PreviewMouseLeftButtonUp += PreviewMouseLeftButtonUpCommand_PreviewMouseLeftButtonUp, i => i.PreviewMouseLeftButtonUp -= PreviewMouseLeftButtonUpCommand_PreviewMouseLeftButtonUp);
        }

        static void PreviewMouseLeftButtonUpCommand_PreviewMouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            if (sender is FrameworkElement element)
                GetPreviewMouseLeftButtonUpCommand(element).Execute(GetPreviewMouseLeftButtonUpCommandParameter(element));
        }

        #endregion

        #region PreviewMouseLeftButtonUpCommandParameter

        public static readonly DependencyProperty PreviewMouseLeftButtonUpCommandParameterProperty = DependencyProperty.RegisterAttached("PreviewMouseLeftButtonUpCommandParameter", typeof(object), typeof(XElement), new FrameworkPropertyMetadata(null));
        public static object GetPreviewMouseLeftButtonUpCommandParameter(FrameworkElement i) => (object)i.GetValue(PreviewMouseLeftButtonUpCommandParameterProperty);
        public static void SetPreviewMouseLeftButtonUpCommandParameter(FrameworkElement i, object input) => i.SetValue(PreviewMouseLeftButtonUpCommandParameterProperty, input);

        #endregion
        
        #region Reference

        public static readonly DependencyProperty ReferenceProperty = DependencyProperty.RegisterAttached("Reference", typeof(IFrameworkReference), typeof(XElement), new FrameworkPropertyMetadata(null, OnReferenceChanged));
        public static IFrameworkReference GetReference(FrameworkElement i) => (IFrameworkReference)i.GetValue(ReferenceProperty);
        public static void SetReference(FrameworkElement i, IFrameworkReference input) => i.SetValue(ReferenceProperty, input);
        static void OnReferenceChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (e.NewValue is IFrameworkReference reference)
                {
                    if (GetReferenceKey(element) is IFrameworkKey key)
                        reference.SetReference(key, element);
                }
            }
        }

        #endregion

        #region ReferenceKey

        public static readonly DependencyProperty ReferenceKeyProperty = DependencyProperty.RegisterAttached("ReferenceKey", typeof(IFrameworkKey), typeof(XElement), new FrameworkPropertyMetadata(null, OnReferenceKeyChanged));
        public static IFrameworkKey GetReferenceKey(FrameworkElement i) => (IFrameworkKey)i.GetValue(ReferenceKeyProperty);
        public static void SetReferenceKey(FrameworkElement i, IFrameworkKey input) => i.SetValue(ReferenceKeyProperty, input);
        static void OnReferenceKeyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (e.NewValue is IFrameworkKey key)
                {
                    if (GetReference(element) is IFrameworkReference reference)
                        reference.SetReference(key, element);
                }
            }
        }

        #endregion

        #region RelativeContext

        public static readonly DependencyProperty RelativeContextProperty = DependencyProperty.RegisterAttached("RelativeContext", typeof(Type), typeof(XElement), new FrameworkPropertyMetadata(null, OnRelativeContextChanged));
        public static Type GetRelativeContext(FrameworkElement i) => (Type)i.GetValue(RelativeContextProperty);
        public static void SetRelativeContext(FrameworkElement i, Type input) => i.SetValue(RelativeContextProperty, input);
        static void OnRelativeContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
                element.UpdateRelative(FrameworkElement.DataContextProperty);
        }

        #endregion

        #region RelativeContextSource

        public static readonly DependencyProperty RelativeContextSourceProperty = DependencyProperty.RegisterAttached("RelativeContextSource", typeof(object), typeof(XElement), new FrameworkPropertyMetadata(null, OnRelativeContextSourceChanged));
        public static object GetRelativeContextSource(FrameworkElement i) => i.GetValue(RelativeContextSourceProperty);
        public static void SetRelativeContextSource(FrameworkElement i, object input) => i.SetValue(RelativeContextSourceProperty, input);
        static void OnRelativeContextSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
                element.UpdateRelative(FrameworkElement.DataContextProperty);
        }

        #endregion

        #region RelativeTag

        public static readonly DependencyProperty RelativeTagProperty = DependencyProperty.RegisterAttached("RelativeTag", typeof(Type), typeof(XElement), new FrameworkPropertyMetadata(null, OnRelativeTagChanged));
        public static Type GetRelativeTag(FrameworkElement i) => (Type)i.GetValue(RelativeTagProperty);
        public static void SetRelativeTag(FrameworkElement i, Type input) => i.SetValue(RelativeTagProperty, input);
        static void OnRelativeTagChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
                element.UpdateRelative(FrameworkElement.TagProperty);
        }

        #endregion

        #region RelativeTagSource

        public static readonly DependencyProperty RelativeTagSourceProperty = DependencyProperty.RegisterAttached("RelativeTagSource", typeof(object), typeof(XElement), new FrameworkPropertyMetadata(null, OnRelativeTagSourceChanged));
        public static object GetRelativeTagSource(FrameworkElement i) => i.GetValue(RelativeTagSourceProperty);
        public static void SetRelativeTagSource(FrameworkElement i, object input) => i.SetValue(RelativeTagSourceProperty, input);
        static void OnRelativeTagSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
                element.UpdateRelative(FrameworkElement.TagProperty);
        }

        #endregion

        #region (private) ResizeAdorner

        static readonly DependencyProperty ResizeAdornerProperty = DependencyProperty.RegisterAttached("ResizeAdorner", typeof(ResizeAdorner), typeof(XElement), new FrameworkPropertyMetadata(null));
        static void SetResizeAdorner(FrameworkElement i, ResizeAdorner input) => i.SetValue(ResizeAdornerProperty, input);

        #endregion

        #region ResizeCoerceAxis

        public static readonly DependencyProperty ResizeCoerceAxisProperty = DependencyProperty.RegisterAttached("ResizeCoerceAxis", typeof(Axis2D?), typeof(XElement), new FrameworkPropertyMetadata(null));
        public static Axis2D? GetResizeCoerceAxis(FrameworkElement i) => (Axis2D?)i.GetValue(ResizeCoerceAxisProperty);
        public static void SetResizeCoerceAxis(FrameworkElement i, Axis2D? input) => i.SetValue(ResizeCoerceAxisProperty, input);

        #endregion

        #region ResizeCoerceDirection

        public static readonly DependencyProperty ResizeCoerceDirectionProperty = DependencyProperty.RegisterAttached("ResizeCoerceDirection", typeof(CardinalDirection?), typeof(XElement), new FrameworkPropertyMetadata(null));
        public static CardinalDirection? GetResizeCoerceDirection(FrameworkElement i) => (CardinalDirection?)i.GetValue(ResizeCoerceDirectionProperty);
        public static void SetResizeCoerceDirection(FrameworkElement i, CardinalDirection? input) => i.SetValue(ResizeCoerceDirectionProperty, input);

        #endregion

        #region ResizeSnap

        public static readonly DependencyProperty ResizeSnapProperty = DependencyProperty.RegisterAttached("ResizeSnap", typeof(double), typeof(XElement), new FrameworkPropertyMetadata(8d));
        public static double GetResizeSnap(FrameworkElement i) => (double)i.GetValue(ResizeSnapProperty);
        public static void SetResizeSnap(FrameworkElement i, double input) => i.SetValue(ResizeSnapProperty, input);

        #endregion
        
        #region ResizeThumbStyle

        public static readonly DependencyProperty ResizeThumbStyleProperty = DependencyProperty.RegisterAttached("ResizeThumbStyle", typeof(Style), typeof(XElement), new FrameworkPropertyMetadata(default(Style)));
        public static Style GetResizeThumbStyle(FrameworkElement i) => (Style)i.GetValue(ResizeThumbStyleProperty);
        public static void SetResizeThumbStyle(FrameworkElement i, Style input) => i.SetValue(ResizeThumbStyleProperty, input);

        #endregion

        #region Ripple

        public static readonly DependencyProperty RippleProperty = DependencyProperty.RegisterAttached("Ripple", typeof(bool), typeof(XElement), new FrameworkPropertyMetadata(false, OnRippleChanged));
        public static bool GetRipple(FrameworkElement i) => (bool)i.GetValue(RippleProperty);
        public static void SetRipple(FrameworkElement i, bool input) => i.SetValue(RippleProperty, input);
        static void OnRippleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                element.RegisterHandlerAttached((bool)e.NewValue, RippleProperty, i =>
                {
                    i.PreviewMouseDown
                        += Ripple_PreviewMouseDown;
                    i.PreviewMouseUp
                        += Ripple_PreviewMouseUp;

                    var result = i.AddAdorner<RippleAdorner>(() => new(i));
                    SetRippleAdorner(i, result);
                }, i =>
                {
                    i.PreviewMouseDown
                        -= Ripple_PreviewMouseDown;
                    i.PreviewMouseUp
                        -= Ripple_PreviewMouseUp;

                    i.RemoveAdorners<RippleAdorner>();
                    SetRippleAdorner(i, null);
                });
            }
        }

        //...

        static void Ripple_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            if (sender is FrameworkElement element)
            {
                switch (GetRippleMouseEvent(element))
                {
                    case MouseEvent.MouseDoubleClick:
                        if (e.ClickCount == 2)
                            GetRippleAdorner(element)?.Ripple(e.GetPosition(element));

                        break;
                    
                    case MouseEvent.MouseDown:
                        GetRippleAdorner(element)?.Ripple(e.GetPosition(element));
                        break;
                }
            }
        }

        static void Ripple_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            if (sender is FrameworkElement element)
            {
                switch (GetRippleMouseEvent(element))
                {
                    case MouseEvent.MouseUp:
                        GetRippleAdorner(element)?.Ripple(e.GetPosition(element));
                        break;
                }
            }
        }

        #endregion

        #region RippleAcceleration

        public static readonly DependencyProperty RippleAccelerationProperty = DependencyProperty.RegisterAttached("RippleAcceleration", typeof(double), typeof(XElement), new FrameworkPropertyMetadata(0.6));
        public static double GetRippleAcceleration(FrameworkElement i) => (double)i.GetValue(RippleAccelerationProperty);
        public static void SetRippleAcceleration(FrameworkElement i, double input) => i.SetValue(RippleAccelerationProperty, input);

        #endregion

        #region (private) RippleAdorner

        static readonly DependencyProperty RippleAdornerProperty = DependencyProperty.RegisterAttached("RippleAdorner", typeof(RippleAdorner), typeof(XElement), new FrameworkPropertyMetadata(null));
        static RippleAdorner GetRippleAdorner(FrameworkElement i) => (RippleAdorner)i.GetValue(RippleAdornerProperty);
        static void SetRippleAdorner(FrameworkElement i, RippleAdorner input) => i.SetValue(RippleAdornerProperty, input);

        #endregion

        #region RippleDeceleration

        public static readonly DependencyProperty RippleDecelerationProperty = DependencyProperty.RegisterAttached("RippleDeceleration", typeof(double), typeof(XElement), new FrameworkPropertyMetadata(0.4));
        public static double GetRippleDeceleration(FrameworkElement i) => (double)i.GetValue(RippleDecelerationProperty);
        public static void SetRippleDeceleration(FrameworkElement i, double input) => i.SetValue(RippleDecelerationProperty, input);

        #endregion

        #region RippleDelay

        public static readonly DependencyProperty RippleDelayProperty = DependencyProperty.RegisterAttached("RippleDelay", typeof(Duration), typeof(XElement), new FrameworkPropertyMetadata(new Duration(0.Seconds())));
        public static Duration GetRippleDelay(FrameworkElement i) => (Duration)i.GetValue(RippleDelayProperty);
        public static void SetRippleDelay(FrameworkElement i, Duration input) => i.SetValue(RippleDelayProperty, input);

        #endregion

        #region RippleDuration

        public static readonly DependencyProperty RippleDurationProperty = DependencyProperty.RegisterAttached("RippleDuration", typeof(Duration), typeof(XElement), new FrameworkPropertyMetadata(new Duration(0.75.Seconds())));
        public static Duration GetRippleDuration(FrameworkElement i) => (Duration)i.GetValue(RippleDurationProperty);
        public static void SetRippleDuration(FrameworkElement i, Duration input) => i.SetValue(RippleDurationProperty, input);

        #endregion

        #region RippleLocation

        public static readonly DependencyProperty RippleLocationProperty = DependencyProperty.RegisterAttached("RippleLocation", typeof(RippleLocations), typeof(XElement), new FrameworkPropertyMetadata(RippleLocations.Center));
        public static RippleLocations GetRippleLocation(FrameworkElement i) => (RippleLocations)i.GetValue(RippleLocationProperty);
        public static void SetRippleLocation(FrameworkElement i, RippleLocations input) => i.SetValue(RippleLocationProperty, input);

        #endregion

        #region RippleMouseEvent

        public static readonly DependencyProperty RippleMouseEventProperty = DependencyProperty.RegisterAttached("RippleMouseEvent", typeof(MouseEvent), typeof(XElement), new FrameworkPropertyMetadata(MouseEvent.MouseDown));
        public static MouseEvent GetRippleMouseEvent(FrameworkElement i) => (MouseEvent)i.GetValue(RippleMouseEventProperty);
        public static void SetRippleMouseEvent(FrameworkElement i, MouseEvent input) => i.SetValue(RippleMouseEventProperty, input);

        #endregion

        #region RippleMaximumOpacity

        public static readonly DependencyProperty RippleMaximumOpacityProperty = DependencyProperty.RegisterAttached("RippleMaximumOpacity", typeof(double), typeof(XElement), new FrameworkPropertyMetadata(0.8));
        public static double GetRippleMaximumOpacity(FrameworkElement i) => (double)i.GetValue(RippleMaximumOpacityProperty);
        public static void SetRippleMaximumOpacity(FrameworkElement i, double input) => i.SetValue(RippleMaximumOpacityProperty, input);

        #endregion

        #region RippleMaximumRadius

        public static readonly DependencyProperty RippleMaximumRadiusProperty = DependencyProperty.RegisterAttached("RippleMaximumRadius", typeof(double), typeof(XElement), new FrameworkPropertyMetadata(75.0));
        public static double GetRippleMaximumRadius(FrameworkElement i) => (double)i.GetValue(RippleMaximumRadiusProperty);
        public static void SetRippleMaximumRadius(FrameworkElement i, double input) => i.SetValue(RippleMaximumRadiusProperty, input);

        #endregion

        #region RippleStroke

        public static readonly DependencyProperty RippleStrokeProperty = DependencyProperty.RegisterAttached("RippleStroke", typeof(Brush), typeof(XElement), new FrameworkPropertyMetadata(Brushes.Black));
        public static Brush GetRippleStroke(FrameworkElement i) => (Brush)i.GetValue(RippleStrokeProperty);
        public static void SetRippleStroke(FrameworkElement i, Brush input) => i.SetValue(RippleStrokeProperty, input);

        #endregion

        #region RippleStrokeThickness

        public static readonly DependencyProperty RippleStrokeThicknessProperty = DependencyProperty.RegisterAttached("RippleStrokeThickness", typeof(DoubleRange), typeof(XElement), new FrameworkPropertyMetadata(null));
        [TypeConverter(typeof(DoubleRangeTypeConverter))]
        public static DoubleRange GetRippleStrokeThickness(FrameworkElement i) => i.GetValueOrSetDefault<DoubleRange>(RippleStrokeThicknessProperty, () => new(15, 3));
        public static void SetRippleStrokeThickness(FrameworkElement i, DoubleRange input) => i.SetValue(RippleStrokeThicknessProperty, input);

        #endregion

        #region Selected

        public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble, typeof(RoutedEventHandler<DoubleRegion>), typeof(FrameworkElement));
        public static void AddSelectedHandler(FrameworkElement i, RoutedEventHandler<DoubleRegion> handler)
            => i.AddHandler(SelectedEvent, handler);
        public static void RemoveSelectedHandler(FrameworkElement i, RoutedEventHandler<DoubleRegion> handler)
            => i.RemoveHandler(SelectedEvent, handler);

        #endregion

        #region Selection

        public static readonly DependencyProperty SelectionProperty = DependencyProperty.RegisterAttached("Selection", typeof(DoubleRegion), typeof(XElement), new FrameworkPropertyMetadata(default(DoubleRegion)));
        [TypeConverter(typeof(DoubleRegionTypeConverter))]
        public static DoubleRegion GetSelection(FrameworkElement i) => i.GetValueOrSetDefault(SelectionProperty, () => new DoubleRegion());
        public static void SetSelection(FrameworkElement i, DoubleRegion input) => i.SetValue(SelectionProperty, input);

        #endregion

        #region (private) SelectionAdorner

        static readonly DependencyProperty SelectionAdornerProperty = DependencyProperty.RegisterAttached("SelectionAdorner", typeof(SelectionAdorner), typeof(XElement), new FrameworkPropertyMetadata(null));
        static SelectionAdorner GetSelectionAdorner(FrameworkElement i) => (SelectionAdorner)i.GetValue(SelectionAdornerProperty);
        static void SetSelectionAdorner(FrameworkElement i, SelectionAdorner input) => i.SetValue(SelectionAdornerProperty, input);

        #endregion

        #region SelectionButton

        public static readonly DependencyProperty SelectionButtonProperty = DependencyProperty.RegisterAttached("SelectionButton", typeof(MouseButton), typeof(XElement), new FrameworkPropertyMetadata(MouseButton.Left));
        public static MouseButton GetSelectionButton(FrameworkElement i) => (MouseButton)i.GetValue(SelectionButtonProperty);
        public static void SetSelectionButton(FrameworkElement i, MouseButton input) => i.SetValue(SelectionButtonProperty, input);

        #endregion

        #region SelectionResets

        public static readonly DependencyProperty SelectionResetsProperty = DependencyProperty.RegisterAttached("SelectionResets", typeof(bool), typeof(XElement), new FrameworkPropertyMetadata(false));
        public static bool GetSelectionResets(FrameworkElement i) => (bool)i.GetValue(SelectionResetsProperty);
        public static void SetSelectionResets(FrameworkElement i, bool input) => i.SetValue(SelectionResetsProperty, input);

        #endregion

        #region (private) SelectionStart

        static readonly DependencyProperty SelectionStartProperty = DependencyProperty.RegisterAttached("SelectionStart", typeof(Point?), typeof(XElement), new FrameworkPropertyMetadata(null));
        static Point? GetSelectionStart(FrameworkElement i) => (Point?)i.GetValue(SelectionStartProperty);
        static void SetSelectionStart(FrameworkElement i, Point? input) => i.SetValue(SelectionStartProperty, input);

        #endregion

        #region ShellContextMenu

        public static readonly DependencyProperty ShellContextMenuProperty = DependencyProperty.RegisterAttached("ShellContextMenu", typeof(object), typeof(XElement), new FrameworkPropertyMetadata(null, OnShellContextMenuChanged));
        public static object GetShellContextMenu(FrameworkElement i) => (object)i.GetValue(ShellContextMenuProperty);
        public static void SetShellContextMenu(FrameworkElement i, object input) => i.SetValue(ShellContextMenuProperty, input);
        static void OnShellContextMenuChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
                element.RegisterHandlerAttached(e.NewValue?.ToString().NullOrEmpty() == false, ShellContextMenuProperty, i => i.PreviewMouseRightButtonUp += ShellContextMenu_PreviewMouseRightButtonUp, i => i.PreviewMouseRightButtonUp -= ShellContextMenu_PreviewMouseRightButtonUp);
        }

        static void Add(List<FileSystemInfo> input, string path)
        {
            FileSystemInfo result = null;
            Try.Invoke(() => result = Storage.File.Long.Exists(path) ? (FileSystemInfo)new FileInfo(path) : Folder.Long.Exists(path) ? new DirectoryInfo(path) : null);
            if (result != null)
                input.Add(result);
        }

        static void ShellContextMenu_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                List<FileSystemInfo> result = new();

                if (GetShellContextMenu(element) is string path)
                    Add(result, path);

                else if (GetShellContextMenu(element) is string[] paths)
                {
                    foreach (var i in paths)
                        Add(result, i);
                }

                else if(GetShellContextMenu(element) is ICollectionChanged collect)
                {
                    foreach (var i in collect)
                    {
                        if (i is Item j)
                            Add(result, j.Path);
                    }
                }

                if (result.Count > 0)
                {
                    var point = element.PointToScreen(e.GetPosition(element));
                    ShellContextMenu.Show(point.Int32(), result.ToArray());
                }
            }
        }

        #endregion

        #region ToolTipTemplate

        public static readonly DependencyProperty ToolTipTemplateProperty = DependencyProperty.RegisterAttached("ToolTipTemplate", typeof(DataTemplate), typeof(XElement), new FrameworkPropertyMetadata(null));
        public static DataTemplate GetToolTipTemplate(FrameworkElement i) => (DataTemplate)i.GetValue(ToolTipTemplateProperty);
        public static void SetToolTipTemplate(FrameworkElement i, DataTemplate input) => i.SetValue(ToolTipTemplateProperty, input);

        #endregion

        #region ToolTipTemplateSelector

        public static readonly DependencyProperty ToolTipTemplateSelectorProperty = DependencyProperty.RegisterAttached("ToolTipTemplateSelector", typeof(DataTemplateSelector), typeof(XElement), new FrameworkPropertyMetadata(null));
        public static DataTemplateSelector GetToolTipTemplateSelector(FrameworkElement i) => (DataTemplateSelector)i.GetValue(ToolTipTemplateSelectorProperty);
        public static void SetToolTipTemplateSelector(FrameworkElement i, DataTemplateSelector input) => i.SetValue(ToolTipTemplateSelectorProperty, input);

        #endregion

        #region Wheel

        public static readonly DependencyProperty WheelProperty = DependencyProperty.RegisterAttached("Wheel", typeof(bool), typeof(XElement), new FrameworkPropertyMetadata(false, OnWheelChanged));
        public static bool GetWheel(FrameworkElement i) => (bool)i.GetValue(WheelProperty);
        public static void SetWheel(FrameworkElement i, bool value) => i.SetValue(WheelProperty, value);
        static void OnWheelChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
                element.RegisterHandlerAttached((bool)e.NewValue, WheelProperty, i => i.PreviewMouseWheel += Wheel_MouseWheel, i => i.PreviewMouseWheel -= Wheel_MouseWheel);
        }

        static void Wheel_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                if (ModifierKeys.Control.Pressed())
                {
                    if (GetWheelValues(frameworkElement)?.Count > 0)
                    {
                        var value
                            = GetWheelValue(frameworkElement);
                        var values
                            = GetWheelValues(frameworkElement);

                        var increment
                            = GetWheelIncrement(frameworkElement);
                        var maximum
                            = GetWheelIncrement(frameworkElement);
                        var minimum
                            = GetWheelIncrement(frameworkElement);

                        if (e.Delta > 0)
                        {
                            if (increment == 0)
                            {
                                var i = values.IndexOf(value) + 1;
                                if (i <= values.Count - 1)
                                    value = (double)values[i];
                            }
                            else
                            {
                                if (value + increment <= maximum)
                                    value += increment;
                            }
                        }
                        else
                        {
                            if (increment == 0)
                            {
                                var i = values.IndexOf(value) - 1;
                                if (i >= 0)
                                    value = (double)values[i];
                            }
                            else
                            {
                                if (value - increment >= minimum)
                                    value -= increment;
                            }
                        }
                        SetWheelValue(frameworkElement, value);
                        return;
                    }

                    if (e.Delta > 0)
                    {
                        var i = GetWheelValue(frameworkElement) + GetWheelIncrement(frameworkElement);
                        if (i <= GetWheelMaximum(frameworkElement))
                            SetWheelValue(frameworkElement, i);
                    }
                    else
                    {
                        var i = GetWheelValue(frameworkElement) - GetWheelIncrement(frameworkElement);
                        if (i >= GetWheelMinimum(frameworkElement))
                            SetWheelValue(frameworkElement, i);
                    }
                    e.Handled = true;
                }
            }
        }

        #endregion

        #region WheelIncrement

        public static readonly DependencyProperty WheelIncrementProperty = DependencyProperty.RegisterAttached("WheelIncrement", typeof(double), typeof(XElement), new FrameworkPropertyMetadata(1.0));
        public static double GetWheelIncrement(FrameworkElement i) => (double)i.GetValue(WheelIncrementProperty);
        public static void SetWheelIncrement(FrameworkElement i, double value) => i.SetValue(WheelIncrementProperty, value);

        #endregion

        #region WheelMaximum

        public static readonly DependencyProperty WheelMaximumProperty = DependencyProperty.RegisterAttached("WheelMaximum", typeof(double), typeof(XElement), new FrameworkPropertyMetadata(32.0));
        public static double GetWheelMaximum(FrameworkElement i) => (double)i.GetValue(WheelMaximumProperty);
        public static void SetWheelMaximum(FrameworkElement i, double value) => i.SetValue(WheelMaximumProperty, value);

        #endregion

        #region WheelMinimum

        public static readonly DependencyProperty WheelMinimumProperty = DependencyProperty.RegisterAttached("WheelMinimum", typeof(double), typeof(XElement), new FrameworkPropertyMetadata(8.0));
        public static double GetWheelMinimum(FrameworkElement i) => (double)i.GetValue(WheelMinimumProperty);
        public static void SetWheelMinimum(FrameworkElement i, double value) => i.SetValue(WheelMinimumProperty, value);

        #endregion

        #region WheelValue

        public static readonly DependencyProperty WheelValueProperty = DependencyProperty.RegisterAttached("WheelValue", typeof(double), typeof(XElement), new FrameworkPropertyMetadata(8.0));
        public static double GetWheelValue(FrameworkElement i) => (double)i.GetValue(WheelValueProperty);
        public static void SetWheelValue(FrameworkElement i, double value) => i.SetValue(WheelValueProperty, value);

        #endregion

        #region WheelValues

        public static readonly DependencyProperty WheelValuesProperty = DependencyProperty.RegisterAttached("WheelValues", typeof(IList), typeof(XElement), new FrameworkPropertyMetadata(null));
        public static IList GetWheelValues(FrameworkElement i) => (IList)i.GetValue(WheelValuesProperty);
        public static void SetWheelValues(FrameworkElement i, IList value) => i.SetValue(WheelValuesProperty, value);

        #endregion

        #endregion

        #region XElement

        static XElement()
        {
            EventManager.RegisterClassHandler(typeof(FrameworkElement), FrameworkElement.LoadedEvent,
                new RoutedEventHandler(OnLoaded), true);
            EventManager.RegisterClassHandler(typeof(FrameworkElement), FrameworkElement.MouseDownEvent,
                new MouseButtonEventHandler(OnMouseDown), true);
            EventManager.RegisterClassHandler(typeof(FrameworkElement), FrameworkElement.PreviewMouseUpEvent,
                new MouseButtonEventHandler(OnPreviewMouseUp), true);
        }

        static void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (GetRelativeContext(element) != null)
                    element.UpdateRelative(FrameworkElement.DataContextProperty);

                if (GetRelativeTag(element) != null)
                    element.UpdateRelative(FrameworkElement.TagProperty);
            }
        }

        static void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                SetIsMouseDown(element, true);
                switch (e.ChangedButton)
                {
                    case MouseButton.Left:
                        SetIsMouseLeftButtonDown(element, true);
                        break;
                    case MouseButton.Middle:
                        SetIsMouseMiddleButtonDown(element, true);
                        break;
                    case MouseButton.Right:
                        SetIsMouseRightButtonDown(element, true);
                        break;
                }
            }
        }

        static void OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                SetIsMouseDown
                    (element, false);
                SetIsMouseLeftButtonDown
                    (element, false);
                SetIsMouseMiddleButtonDown
                    (element, false);
                SetIsMouseRightButtonDown
                    (element, false);
            }
        }

        #endregion

        #region Methods

        static void UpdateRelative(this FrameworkElement element, DependencyProperty property)
        {
            object findSource = null; Type findType = null;
            if (property == FrameworkElement.DataContextProperty)
            {
                findSource
                    = GetRelativeContextSource(element);
                findType
                    = GetRelativeContext(element);
            }
            else if (property == FrameworkElement.TagProperty)
            {
                findSource
                    = GetRelativeTagSource(element);
                findType
                    = GetRelativeTag(element);
            }
            else return;

            var result = findSource is DependencyObject i ? i : element;
            if (result.FindParent(findType) is object parent)
                element.SetCurrentValue(property, parent);
        }

        //...

        public static Size ActualSize(this FrameworkElement input) => new(input.ActualWidth, input.ActualHeight);

        //...

        /// <summary>
        /// Gets if the given <see cref="FrameworkElement"/> has the mouse over it or not.
        /// </summary>
        /// <param name="input">The <see cref="FrameworkElement"/> to test for mouse containment.</param>
        /// <returns>True, if the mouse is over the FrameworkElement; false, otherwise.</returns>
        public static bool ContainsMouse(this FrameworkElement input)
        {
            var point = Mouse.GetPosition(input);
            return
            (
                point.X >= 0
                &&
                point.X <= input.ActualWidth
                &&
                point.Y >= 0
                &&
                point.Y <= input.ActualHeight
            );
        }

        public static Style FindStyle<Element>(this Element input) where Element : FrameworkElement
            => (Style)input.FindResource(typeof(Element));

        //...

        public static T GetChild<T>(this FrameworkElement input, ReferenceKey<T> key) where T : FrameworkElement
            => input.FindVisualChildren<T>().FirstOrDefault(i => ReferenceEquals(GetName(i), key)) ?? input.FindLogicalChildren<T>().FirstOrDefault(i => ReferenceEquals(GetName(i), key));

        public static IEnumerable<T> GetChildren<T>(this FrameworkElement input, ReferenceKey<T> key) where T : FrameworkElement
            => input.FindVisualChildren<T>().Where(i => ReferenceEquals(GetName(i), key));

        //...

        public static T GetValueOrSetDefault<T>(this FrameworkElement input, DependencyProperty property, Func<T> action)
        {
            if (input.GetValue(property) is T j)
                return j;

            j = action();
            input.SetValue(property, j);
            return j;
        }

        public static T GetValueOrSetDefault<T>(this FrameworkElement input, DependencyPropertyKey key, Func<T> action)
        {
            if (input.GetValue(key.DependencyProperty) is T j)
                return j;

            j = action();
            input.SetValue(key, j);
            return j;
        }

        //...

        public static int Index(this FrameworkElement input, uint origin = 0)
        {
            var itemsControl = ItemsControl.ItemsControlFromItemContainer(input);
            var index = itemsControl.ItemContainerGenerator.IndexFromContainer(input);
            return origin.Int32() + index;
        }

        public static System.Drawing.Bitmap Render(this FrameworkElement input)
        {
            try
            {
                input.Measure(new Size(input.ActualWidth, input.ActualHeight));
                input.Arrange(new Rect(new Size(input.ActualWidth, input.ActualHeight)));

                var bmp = new RenderTargetBitmap((int)input.ActualWidth, (int)input.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                bmp.Render(input);

                var stream = new MemoryStream();

                var encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                encoder.Save(stream);

                return new System.Drawing.Bitmap(stream);
            }
            catch
            {
                return null;
            }
        }

        public static Result TryFindStyle<T>(this T input, out Style style) where T : FrameworkElement
        {
            Style i = null;
            var result = Try.Invoke(() => i = input.FindStyle());
            style = i;
            return result;
        }

        #endregion
    }
}