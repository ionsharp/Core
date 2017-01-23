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

        #region Static

        static Control CurrentDropTarget = null;

        static Control CurrentItem = null;

        /// <summary>
        /// Indicates whether or not the current item is a possible drop target
        /// </summary>
        static bool IsPossibleDropTarget;

        #endregion

        #region Content

        /// <summary>
        /// Enables assigning any FrameworkElement a 'content' object.
        /// </summary>
        public static readonly DependencyProperty ContentProperty = DependencyProperty.RegisterAttached("Content", typeof(object), typeof(ControlExtensions), new PropertyMetadata(null, OnContentChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static object GetContent(Control d)
        {
            return d.GetValue(ContentProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetContent(Control d, object value)
        {
            d.SetValue(ContentProperty, value);
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

        #endregion

        #region IsDraggingOver

        /// <summary>
        /// Indicates whether or not the current item is a possible drop target
        /// </summary>
        static readonly DependencyPropertyKey IsDraggingOverKey = DependencyProperty.RegisterAttachedReadOnly("IsDraggingOver", typeof(bool), typeof(ControlExtensions), new FrameworkPropertyMetadata(null, new CoerceValueCallback(OnIsDraggingOverCoerced)));
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsDraggingOverProperty = IsDraggingOverKey.DependencyProperty;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool GetIsDraggingOver(Control d)
        {
            return (bool)d.GetValue(IsDraggingOverProperty);
        }
        static object OnIsDraggingOverCoerced(DependencyObject Item, object value)
        {
            return Item == CurrentDropTarget && IsPossibleDropTarget ? true : false;
        }

        #endregion

        #region IsMouseDirectlyOver

        /// <summary>
        /// Indicates whether or not the mouse is directly over an item.
        /// </summary>
        static readonly DependencyPropertyKey IsMouseDirectlyOverKey = DependencyProperty.RegisterAttachedReadOnly("IsMouseDirectlyOver", typeof(bool), typeof(ControlExtensions), new FrameworkPropertyMetadata(null, new CoerceValueCallback(OnIsMouseDirectlyOverCoerced)));
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsMouseDirectlyOverProperty = IsMouseDirectlyOverKey.DependencyProperty;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool GetIsMouseDirectlyOver(Control d)
        {
            return (bool)d.GetValue(IsMouseDirectlyOverProperty);
        }
        static object OnIsMouseDirectlyOverCoerced(DependencyObject item, object value)
        {
            return item == CurrentItem;
        }

        #endregion

        #region IsReadOnly

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.RegisterAttached("IsReadOnly", typeof(bool), typeof(ControlExtensions), new PropertyMetadata(false));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetIsReadOnly(Control d, bool value)
        {
            d.SetValue(IsReadOnlyProperty, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool GetIsReadOnly(Control d)
        {
            return (bool)d.GetValue(IsReadOnlyProperty);
        }

        #endregion

        #region UpdateOverItem

        static readonly RoutedEvent UpdateOverItemEvent = EventManager.RegisterRoutedEvent("UpdateOverItem", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ControlExtensions));
        /// <summary>
        /// This method is a listener for the UpdateOverItemEvent.  
        /// When it is received, it means that the sender is the 
        /// closest item to the mouse (closest logically, not visually).
        /// </summary>
        static void OnUpdateOverItem(object sender, RoutedEventArgs args)
        {
            //Mark this object as the tree view item over which the mouse is currently positioned.
            CurrentItem = sender as Control;
            //Tell that item to recalculate
            CurrentItem.InvalidateProperty(IsMouseDirectlyOverProperty);
            //Prevent this event from notifying other items higher in tree
            args.Handled = true;
        }

        #endregion

        #endregion

        #region ControlExtensions

        static ControlExtensions()
        {
            EventManager.RegisterClassHandler(typeof(Control), Control.PreviewDragEnterEvent, new DragEventHandler(OnDragOver), true);
            EventManager.RegisterClassHandler(typeof(Control), Control.PreviewDragLeaveEvent, new DragEventHandler(OnDragLeave), true);
            EventManager.RegisterClassHandler(typeof(Control), Control.PreviewDragOverEvent, new DragEventHandler(OnDragOver), true);
            EventManager.RegisterClassHandler(typeof(Control), Control.PreviewDropEvent, new DragEventHandler(OnDrop), true);
            EventManager.RegisterClassHandler(typeof(Control), Control.MouseEnterEvent, new MouseEventHandler(OnMouseTransition), true);
            EventManager.RegisterClassHandler(typeof(Control), Control.MouseLeaveEvent, new MouseEventHandler(OnMouseTransition), true);
            EventManager.RegisterClassHandler(typeof(Control), UpdateOverItemEvent, new RoutedEventHandler(OnUpdateOverItem));
        }

        #endregion

        #region Methods

        static void OnDrop(object sender, DragEventArgs args)
        {
            lock (IsDraggingOverProperty)
            {
                IsPossibleDropTarget = false;
                if (CurrentDropTarget != null)
                    CurrentDropTarget.InvalidateProperty(IsDraggingOverProperty);
                var Item = sender as Control;
                if (Item != null)
                {
                    CurrentDropTarget = Item;
                    Item.InvalidateProperty(IsDraggingOverProperty);
                }
            }
        }

        /// <summary>
        /// Called when an item is dragged over the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.Windows.DragEventArgs"/> instance containing the event data.</param>
        static void OnDragOver(object sender, DragEventArgs e)
        {
            lock (IsDraggingOverProperty)
            {
                IsPossibleDropTarget = false;
                if (CurrentDropTarget != null)
                {
                    var OldItem = CurrentDropTarget;
                    CurrentDropTarget = null;
                    OldItem.InvalidateProperty(IsDraggingOverProperty);
                }

                if (e.Effects != DragDropEffects.None)
                    IsPossibleDropTarget = true;

                var Control = sender as Control;
                if (Control != null)
                {
                    CurrentDropTarget = Control;
                    CurrentDropTarget.InvalidateProperty(IsDraggingOverProperty);
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
            lock (IsDraggingOverProperty)
            {
                IsPossibleDropTarget = false;
                if (CurrentDropTarget != null)
                {
                    var OldItem = CurrentDropTarget;
                    CurrentDropTarget = null;
                    OldItem.InvalidateProperty(IsDraggingOverProperty);
                }
                var Control = sender as Control;
                if (Control != null)
                {
                    CurrentDropTarget = Control;
                    CurrentDropTarget.InvalidateProperty(IsDraggingOverProperty);
                }
            }
        }

        static void OnMouseTransition(object sender, MouseEventArgs args)
        {
            lock (IsMouseDirectlyOverProperty)
            {
                if (CurrentItem != null)
                {
                    var OldItem = CurrentItem;
                    CurrentItem = null;
                    OldItem.InvalidateProperty(IsMouseDirectlyOverProperty);
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
