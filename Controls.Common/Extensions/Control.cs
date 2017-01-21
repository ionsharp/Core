using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ControlExtensions
    {
        #region Properties

        static Control CurrentDropTarget = null;

        static Control CurrentItem = null;

        /// <summary>
        /// Indicates whether or not the current item is a possible drop target
        /// </summary>
        static bool IsPossibleDropTarget;

        /// <summary>
        /// Indicates whether or not the mouse is directly over an item.
        /// </summary>
        static readonly DependencyPropertyKey IsMouseDirectlyOverItemKey = DependencyProperty.RegisterAttachedReadOnly("IsMouseDirectlyOverItem", typeof(bool), typeof(ControlExtensions), new FrameworkPropertyMetadata(null, new CoerceValueCallback(CalculateIsMouseDirectlyOverItem)));
        public static readonly DependencyProperty IsMouseDirectlyOverItemProperty = IsMouseDirectlyOverItemKey.DependencyProperty;
        public static bool GetIsMouseDirectlyOverItem(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMouseDirectlyOverItemProperty);
        }
        static object CalculateIsMouseDirectlyOverItem(DependencyObject item, object value)
        {
            return item == CurrentItem;
        }

        /// <summary>
        /// Indicates whether or not the current item is a possible drop target
        /// </summary>
        static readonly DependencyPropertyKey IsPossibleDropTargetKey = DependencyProperty.RegisterAttachedReadOnly("IsPossibleDropTarget", typeof(bool), typeof(ControlExtensions), new FrameworkPropertyMetadata(null, new CoerceValueCallback(CalculateIsPossibleDropTarget)));
        public static readonly DependencyProperty IsPossibleDropTargetProperty = IsPossibleDropTargetKey.DependencyProperty;
        public static bool GetIsPossibleDropTarget(DependencyObject Object)
        {
            return (bool)Object.GetValue(IsPossibleDropTargetProperty);
        }
        static object CalculateIsPossibleDropTarget(DependencyObject Item, object value)
        {
            return Item == CurrentDropTarget && IsPossibleDropTarget ? true : false;
        }

        static readonly RoutedEvent UpdateOverItemEvent = EventManager.RegisterRoutedEvent("UpdateOverItem", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ControlExtensions));
        /// <summary>
        /// This method is a listener for the UpdateOverItemEvent.  
        /// When it is received, it means that the sender is the 
        /// closest item to the mouse (closest logically, not visually).
        /// </summary>
        static void OnUpdateOverItem(object sender, RoutedEventArgs args)
        {
            //Mark this object as the tree view item over which the mouse is currently positioned.
            CurrentItem = sender as TreeViewItem;
            //Tell that item to recalculate
            CurrentItem.InvalidateProperty(IsMouseDirectlyOverItemProperty);
            //Prevent this event from notifying other items higher in tree
            args.Handled = true;
        }

        /// <summary>
        /// Enables assigning any FrameworkElement a 'content' object.
        /// </summary>
        public static readonly DependencyProperty Content = DependencyProperty.RegisterAttached("Content", typeof(object), typeof(ControlExtensions), new PropertyMetadata(null, OnContentChanged));
        public static object GetContent(DependencyObject obj)
        {
            return (object)obj.GetValue(Content);
        }
        public static void SetContent(DependencyObject obj, object value)
        {
            obj.SetValue(Content, value);
        }
        static void OnContentChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var FrameworkElement = sender as FrameworkElement;
            if (FrameworkElement != null)
            {
                if (e.NewValue != null && e.NewValue is FrameworkElement)
                    e.NewValue.As<FrameworkElement>().DataContext = FrameworkElement.DataContext;
            }
        }

        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.RegisterAttached("IsVisible", typeof(bool), typeof(ControlExtensions), new PropertyMetadata(true));
        public static void SetIsVisible(DependencyObject obj, bool value)
        {
            obj.SetValue(IsVisibleProperty, value);
        }
        public static bool GetIsVisible(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsVisibleProperty);
        }

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.RegisterAttached("IsReadOnly", typeof(bool), typeof(ControlExtensions), new PropertyMetadata(false));
        public static void SetIsReadOnly(DependencyObject obj, bool value)
        {
            obj.SetValue(IsReadOnlyProperty, value);
        }
        public static bool GetIsReadOnly(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsReadOnlyProperty);
        }

        public static readonly DependencyProperty IsDragMoveEnabled = DependencyProperty.RegisterAttached("IsDragMoveEnabled", typeof(bool), typeof(ControlExtensions), new PropertyMetadata(false, OnIsDragMoveEnabledChanged));
        public static bool GetIsDragMoveEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDragMoveEnabled);
        }
        public static void SetIsDragMoveEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDragMoveEnabled, value);
        }
        static void OnIsDragMoveEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var Window = (sender as DependencyObject).GetParent<Window>();
            if (Window != null && (bool)e.NewValue)
            {
                (sender as FrameworkElement).MouseDown += (a, b) =>
                {
                    if (b.LeftButton == MouseButtonState.Pressed)
                        Window.DragMove();
                };
            }
        }

        #endregion

        #region ControlExtensions

        static ControlExtensions()
        {
            EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.PreviewDragEnterEvent, new DragEventHandler(OnDragEvent), true);
            EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.PreviewDragLeaveEvent, new DragEventHandler(OnDragLeave), true);
            EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.PreviewDragOverEvent, new DragEventHandler(OnDragEvent), true);
            EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.PreviewDropEvent, new DragEventHandler(OnDrop), true);
            EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.MouseEnterEvent, new MouseEventHandler(OnMouseTransition), true);
            EventManager.RegisterClassHandler(typeof(TreeViewItem), TreeViewItem.MouseLeaveEvent, new MouseEventHandler(OnMouseTransition), true);
            EventManager.RegisterClassHandler(typeof(TreeViewItem), UpdateOverItemEvent, new RoutedEventHandler(OnUpdateOverItem));
        }

        #endregion

        #region Methods

        static void OnDrop(object sender, DragEventArgs args)
        {
            lock (IsPossibleDropTargetProperty)
            {
                IsPossibleDropTarget = false;
                if (CurrentDropTarget != null)
                    CurrentDropTarget.InvalidateProperty(IsPossibleDropTargetProperty);
                var Item = sender as Control;
                if (Item != null)
                {
                    CurrentDropTarget = Item;
                    Item.InvalidateProperty(IsPossibleDropTargetProperty);
                }
            }
        }

        /// <summary>
        /// Called when an item is dragged over the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Windows.DragEventArgs"/> instance containing the event data.</param>
        static void OnDragEvent(object sender, DragEventArgs Args)
        {
            lock (IsPossibleDropTargetProperty)
            {
                IsPossibleDropTarget = false;
                if (CurrentDropTarget != null)
                {
                    var OldItem = CurrentDropTarget;
                    CurrentDropTarget = null;
                    OldItem.InvalidateProperty(IsPossibleDropTargetProperty);
                }

                if (Args.Effects != DragDropEffects.None)
                    IsPossibleDropTarget = true;

                var Control = sender as Control;
                if (Control != null)
                {
                    CurrentDropTarget = Control;
                    CurrentDropTarget.InvalidateProperty(IsPossibleDropTargetProperty);
                }
            }
        }

        /// <summary>
        /// Called when the drag cursor leaves the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Windows.DragEventArgs"/> instance containing the event data.</param>
        static void OnDragLeave(object sender, DragEventArgs args)
        {
            lock (IsPossibleDropTargetProperty)
            {
                IsPossibleDropTarget = false;
                if (CurrentDropTarget != null)
                {
                    var OldItem = CurrentDropTarget;
                    CurrentDropTarget = null;
                    OldItem.InvalidateProperty(IsPossibleDropTargetProperty);
                }
                var Control = sender as Control;
                if (Control != null)
                {
                    CurrentDropTarget = Control;
                    CurrentDropTarget.InvalidateProperty(IsPossibleDropTargetProperty);
                }
            }
        }

        static void OnMouseTransition(object sender, MouseEventArgs args)
        {
            lock (IsMouseDirectlyOverItemProperty)
            {
                if (CurrentItem != null)
                {
                    var OldItem = CurrentItem;
                    CurrentItem = null;
                    OldItem.InvalidateProperty(IsMouseDirectlyOverItemProperty);
                }
                //Get the element that is currently under the mouse.
                var CurrentPosition = Mouse.DirectlyOver;

                // See if the mouse is still over something.
                if (CurrentPosition != null)
                {
                    var RoutedEventArgs = new RoutedEventArgs(UpdateOverItemEvent);
                    CurrentPosition.RaiseEvent(RoutedEventArgs);
                }
            }
        }

        #endregion
    }
}
