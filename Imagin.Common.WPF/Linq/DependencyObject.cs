using Imagin.Common.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class DependencyObjectExtensions
    {
        #region Properties

        #region IsVisible

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.RegisterAttached("IsVisible", typeof(bool), typeof(DependencyObjectExtensions), new PropertyMetadata(true));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="value"></param>
        public static void SetIsVisible(DependencyObject d, bool value)
        {
            d.SetValue(IsVisibleProperty, value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool GetIsVisible(DependencyObject d)
        {
            return (bool)d.GetValue(IsVisibleProperty);
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="property"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        public static BindingExpressionBase Bind(this DependencyObject source, DependencyProperty property, Binding binding)
            => BindingOperations.SetBinding(source, property, binding);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Object"></param>
        public static void CollapseAll(this DependencyObject Object)
        {
            Object.ToggleAll(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Object"></param>
        public static void ExpandAll(this DependencyObject Object)
        {
            Object.ToggleAll(true);
        }

        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <param name="findIndex">the index of the item to be found.  0 to find the first name/type match, 1 to find the second match, etc </param>
        /// <param name="foundCount">recursion counter to keep track of the number of name/type matches found so far. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, a null parent is being returned.</returns>
        static T FindChild<T>(DependencyObject parent, string childName, int findIndex, ref int foundCount) where T : DependencyObject
        {
            //Confirm parent and childName are valid. 
            if (parent == null)
                return null;

            var foundChild = default(T);

            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                var childType = child as T;
                if (childType == null)
                {
                    //Recursively drill down the tree
                    foundChild = FindChild<T>(child, childName, findIndex, ref foundCount);

                    //If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    //If the child's name is set for search
                    if (frameworkElement != null && (frameworkElement.Name == childName || AutomationProperties.GetAutomationId(frameworkElement) == childName))
                    {
                        //If the child's name is of the request name
                        if (foundCount == findIndex)
                        {
                            foundChild = (T)child;
                            break;
                        }
                        else
                            foundCount++;
                    }
                    else
                    {
                        //Recursively drill down the tree
                        foundChild = FindChild<T>(child, childName, findIndex, ref foundCount);

                        //If the child is found, break so we do not overwrite the found child. 
                        if (foundChild != null) break;
                    }
                }
                else
                {
                    //Child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="childName"></param>
        /// <param name="findIndex"></param>
        /// <returns></returns>
        public static T FindChild<T>(this DependencyObject parent, string childName, int findIndex) where T : DependencyObject
        {
            var foundCount = 0;
            return FindChild<T>(parent, childName, findIndex, ref foundCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Object"></param>
        /// <returns></returns>
        public static T GetChildOfType<T>(this DependencyObject Object) where T : DependencyObject
        {
            if (Object == null)
                return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(Object); i++)
            {
                var Child = VisualTreeHelper.GetChild(Object, i);
                var Result = (Child as T) ?? GetChildOfType<T>(Child);
                if (Result != null) return Result;
            }

            return null;
        }

        /// <summary>
        /// Attempts to find parent for specified object in following order: 
        /// VisualParent -> LogicalParent -> LogicalTemplatedParent.
        /// </summary>
        /// <remarks>
        /// Visual, FrameworkElement, and FrameworkContentElement types are supported. If the logical parent is not found, we try TemplatedParent.
        /// </remarks>
        /// <param name="Child">The object to get the parent for.</param>
        public static DependencyObject GetParent(this DependencyObject Child)
        {
            if (Child is Popup)
            {
                //Case 126732 : To correctly detect parent of a popup we must do that exception case
                var Popup = Child as Popup;
                if (Popup != null && Popup.PlacementTarget != null)
                    return Popup.PlacementTarget;
            }

            Visual Visual = Child as Visual;
            DependencyObject Parent = Visual == null ? null : VisualTreeHelper.GetParent(Visual);
            if (Parent == null)
            {
                //No visual parent, check logical tree.
                var FrameworkElement = Child as FrameworkElement;
                if (FrameworkElement != null)
                {
                    Parent = FrameworkElement.Parent;
                    if (Parent == null)
                        Parent = FrameworkElement.TemplatedParent;
                }
                else
                {
                    var FrameworkContentElement = Child as FrameworkContentElement;
                    if (FrameworkContentElement != null)
                    {
                        Parent = FrameworkContentElement.Parent;
                        if (Parent == null)
                            Parent = FrameworkContentElement.TemplatedParent;
                    }
                }
            }
            return Parent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Object"></param>
        /// <returns></returns>
        public static T GetParent<T>(this DependencyObject Object) where T : DependencyObject
        {
            var Parent = Object.GetParent();
            while (Parent != null && !Parent.Is<T>())
                Parent = Parent.GetParent();
            return Parent.As<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Child"></param>
        /// <returns></returns>
        public static DependencyObject GetLogicalParent(this DependencyObject Child)
        {
            return LogicalTreeHelper.GetParent(Child);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Child"></param>
        /// <returns></returns>
        public static T GetLogicalParent<T>(this DependencyObject Child) where T : DependencyObject
        {
            do
            {
                if (Child is T) return (T)Child;
                Child = Child.GetLogicalParent();
            }
            while (Child != null);
            return null;
        }
        
        /// <summary>
        /// Gets all logical children for the given <see cref="DependencyObject"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Parent"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetLogicalChildren<T>(this DependencyObject Parent) where T : DependencyObject
        {
            if (Parent == null)
                yield break;

            yield return Parent as T;

            foreach (var Child in LogicalTreeHelper.GetChildren(Parent).OfType<DependencyObject>())
            {
                foreach (var Descendant in Child.GetLogicalChildren<T>())
                {
                    if (Descendant is T)
                        yield return Descendant;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Parent"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetVisualChildren<T>(this DependencyObject Parent) where T : DependencyObject
        {
            if (Parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(Parent); i++)
                {
                    var Child = VisualTreeHelper.GetChild(Parent, i);
                    if (Child != null && Child is T)
                        yield return (T)Child;
                    foreach (var ChildOfChild in GetVisualChildren<T>(Child))
                        yield return ChildOfChild;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Child"></param>
        /// <returns></returns>
        public static DependencyObject GetVisualParent(this DependencyObject Child)
        {
            return VisualTreeHelper.GetParent(Child);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Child"></param>
        /// <returns></returns>
        public static T GetVisualParent<T>(this DependencyObject Child) where T : DependencyObject
        {
            do
            {
                if (Child is T) return (T)Child;
                Child = Child.GetVisualParent();
            }
            while (Child != null);
            return null;
        }

        /// <summary>
        /// Returns true if the specified element is a child of parent somewhere in the visual 
        /// tree. This method will work for Visual, FrameworkElement and FrameworkContentElement.
        /// </summary>
        /// <param name="Object">The element that is potentially a child of the specified parent.</param>
        /// <param name="Parent">The element that is potentially a parent of the specified element.</param>
        public static bool IsDescendantOf(this DependencyObject Object, DependencyObject Parent)
        {
            while (Object != null)
            {
                if (Object == Parent)
                    return true;
                Object = Object.GetParent();
            }
            return false;
        }

        /// <summary>
        /// Select the given element, if supported; element is valid if supports <see cref="Selector.SetIsSelected(DependencyObject, bool)"/> or is <see cref="TreeViewItem"/>.
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="IsSelected"></param>
        public static void Select(this DependencyObject Value, bool IsSelected)
        {
            if (Value.IsAny(typeof(ListBoxItem), typeof(DataGridRow)))
            {
                Selector.SetIsSelected(Value, IsSelected);
            }
            else if (Value is TreeViewItem)
            {
                TreeViewItemExtensions.SetIsSelected(Value as TreeViewItem, IsSelected);
            }
            else throw new ArgumentException("Object doesn't support selection.");
        }

        /// <summary>
        /// Sets IsExpanded property to specified value on all TreeViewItems found.
        /// </summary>
        public static void ToggleAll(this DependencyObject Object, bool IsExpanded)
        {
            if (Object.Is<TreeViewItem>())
            {
                ((TreeViewItem)Object).IsExpanded = IsExpanded;
                foreach (DependencyObject d in ((TreeViewItem)Object).Items)
                    d.ToggleAll(IsExpanded);
            }
            else if (Object.Is<ItemsControl>())
            {
                foreach (var i in Object.As<ItemsControl>().Items)
                {
                    if (i != null)
                    {
                        Object.As<ItemsControl>().ItemContainerGenerator.ContainerFromItem(i).ToggleAll(IsExpanded);
                        TreeViewItem t = i.As<TreeViewItem>();
                        if (t != null)
                            t.IsExpanded = IsExpanded;
                    }
                }
            }
        }

        /// <summary>
        /// Attempt to select the given element, if supported; element is valid if supports <see cref="Selector.SetIsSelected(DependencyObject, bool)"/> or is <see cref="TreeViewItem"/>.
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="IsSelected"></param>
        /// <returns></returns>
        public static Result TrySelect(this DependencyObject Object, bool IsSelected)
        {
            try
            {
                Object.Select(IsSelected);
                return new Success();
            }
            catch (Exception e)
            {
                return new Error(e);
            }
        }

        #endregion
    }
}