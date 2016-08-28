using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace Imagin.Common.Extensions
{
    public static class DependencyObjectExtensions
    {
        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static IEnumerable<T> GetChildren<T>(this DependencyObject Parent) where T : DependencyObject
        {
            if (Parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(Parent); i++)
                {
                    DependencyObject Child = VisualTreeHelper.GetChild(Parent, i);
                    if (Child != null && Child is T)
                        yield return (T)Child;
                    foreach (T childOfChild in GetChildren<T>(Child))
                        yield return childOfChild;
                }
            }
        }

        /// <summary>
        /// Imagin.Common: Tries its best to return the specified element's parent. It will 
        /// try to find, in this order, the VisualParent, LogicalParent, LogicalTemplatedParent.
        /// It only works for Visual, FrameworkElement or FrameworkContentElement.
        /// </summary>
        /// <param name="element">The element to which to return the parent. It will only 
        /// work if element is a Visual, a FrameworkElement or a FrameworkContentElement.</param>
        /// <remarks>If the logical parent is not found (Parent), we check the TemplatedParent
        /// (see FrameworkElement.Parent documentation). But, we never actually witnessed
        /// this situation.</remarks>
        public static DependencyObject GetParent(this DependencyObject element)
        {
            return element.GetParent(true);
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        static DependencyObject GetParent(this DependencyObject element, bool recurseIntoPopup)
        {
            if (recurseIntoPopup)
            {
                // Case 126732 : To correctly detect parent of a popup we must do that exception case
                Popup popup = element as Popup;
                if ((popup != null) && (popup.PlacementTarget != null))
                    return popup.PlacementTarget;
            }

            Visual visual = element as Visual;
            DependencyObject parent = (visual == null) ? null : VisualTreeHelper.GetParent(visual);

            if (parent == null)
            {
                // No Visual parent. Check in the logical tree.
                FrameworkElement fe = element as FrameworkElement;

                if (fe != null)
                {
                    parent = fe.Parent;

                    if (parent == null)
                    {
                        parent = fe.TemplatedParent;
                    }
                }
                else
                {
                    FrameworkContentElement fce = element as FrameworkContentElement;

                    if (fce != null)
                    {
                        parent = fce.Parent;

                        if (parent == null)
                        {
                            parent = fce.TemplatedParent;
                        }
                    }
                }
            }

            return parent;
        }

        /// <summary>
        /// Imagin.Common: Returns true if the specified element is a child of parent somewhere in the visual 
        /// tree. This method will work for Visual, FrameworkElement and FrameworkContentElement.
        /// </summary>
        /// <param name="element">The element that is potentially a child of the specified parent.</param>
        /// <param name="parent">The element that is potentially a parent of the specified element.</param>
        public static bool IsDescendantOf(this DependencyObject element, DependencyObject parent)
        {
            return element.IsDescendantOf(parent, true);
        }

        /// <summary>
        /// Imagin.Common: Returns true if the specified element is a child of parent somewhere in the visual 
        /// tree. This method will work for Visual, FrameworkElement and FrameworkContentElement.
        /// </summary>
        /// <param name="element">The element that is potentially a child of the specified parent.</param>
        /// <param name="parent">The element that is potentially a parent of the specified element.</param>
        public static bool IsDescendantOf(this DependencyObject element, DependencyObject parent, bool recurseIntoPopup)
        {
            while (element != null)
            {
                if (element == parent)
                    return true;

                element = element.GetParent(recurseIntoPopup);
            }

            return false;
        }

        /// <summary>
        /// Imagin.Common
        /// </summary>
        public static T FindParent<T>(this DependencyObject Child) where T : DependencyObject
        {
            do
            {
                if (Child is T)
                    return (T)Child;
                Child = VisualTreeHelper.GetParent(Child);
            }
            while (Child != null);
            return null;
        }
    }
}
