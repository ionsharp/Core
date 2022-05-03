using Imagin.Common.Analytics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace Imagin.Common.Linq
{
    [Extends(typeof(DependencyObject))]
    public static class XDependency
    {
        #region IsVisible

        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.RegisterAttached("IsVisible", typeof(bool), typeof(XDependency), new FrameworkPropertyMetadata(true));
        public static bool GetIsVisible(DependencyObject i) => (bool)i.GetValue(IsVisibleProperty);
        public static void SetIsVisible(DependencyObject i, bool input) => i.SetValue(IsVisibleProperty, input);

        #endregion

        #region (internal) Popup

        internal static readonly DependencyProperty PopupProperty = DependencyProperty.RegisterAttached("Popup", typeof(Popup), typeof(XDependency), new FrameworkPropertyMetadata(null));
        internal static Popup GetPopup(DependencyObject i) => (Popup)i.GetValue(PopupProperty);
        internal static void SetPopup(DependencyObject i, Popup input) => i.SetValue(PopupProperty, input);

        #endregion

        #region Methods

        public static void AddChanged(this DependencyObject input, DependencyProperty p, EventHandler e)
            => DependencyPropertyDescriptor.FromProperty(p, input.GetType()).AddValueChanged(input, e);

        public static void RemoveChanged(this DependencyObject input, DependencyProperty p, EventHandler e)
            => DependencyPropertyDescriptor.FromProperty(p, input.GetType()).RemoveValueChanged(input, e);

        //...

        public static DependencyProperty GetDependencyProperty(this DependencyObject input, string propertyName)
        {
            var type = input.GetType();
            DependencyProperty result(string i)
            {
                var field = type.GetField(i, BindingFlags.Public | BindingFlags.Static);
                return field?.GetValue(null) as DependencyProperty ?? (field?.GetValue(null) as GenericDependencyProperty)?.Property;
            }
            return result($"{propertyName}Property") ?? result(propertyName);
        }

        public static bool HasDependencyProperty(this DependencyObject input, string propertyName) => input.GetDependencyProperty(propertyName) != null;

        //...

        public static BindingExpressionBase Bind(this DependencyObject source, DependencyProperty property, BindingBase binding) => BindingOperations.SetBinding(source, property, binding);

        public static BindingExpressionBase Bind(this DependencyObject input, DependencyProperty property, string path, object source, BindingMode mode = BindingMode.OneWay, IValueConverter converter = null, object converterParameter = null, UpdateSourceTrigger updateSourceTrigger = UpdateSourceTrigger.PropertyChanged) => input.Bind(property, new PropertyPath(path), source, mode, converter, converterParameter, updateSourceTrigger);

        public static BindingExpressionBase Bind(this DependencyObject input, DependencyProperty property, PropertyPath path, object source, BindingMode mode = BindingMode.OneWay, IValueConverter converter = null, object converterParameter = null, UpdateSourceTrigger updateSourceTrigger = UpdateSourceTrigger.PropertyChanged)
        {
            return BindingOperations.SetBinding(input, property, new Binding()
            {
                Converter
                    = converter,
                ConverterParameter
                    = converterParameter,
                Mode
                    = mode,
                Path
                    = path,
                Source
                    = source,
                UpdateSourceTrigger
                    = updateSourceTrigger
            });
        }

        //...

        public static BindingExpressionBase MultiBind(this DependencyObject input, DependencyProperty property, IMultiValueConverter converter, object converterParameter, params Binding[] bindings)
        {
            var result = new MultiBinding()
            {
                Converter = converter,
                ConverterParameter = converterParameter,
                Mode = BindingMode.OneWay,
            };
            if (bindings?.Length > 0)
                bindings.ForEach(i => result.Bindings.Add(i));

            return input.Bind(property, result);
        }

        public static BindingExpressionBase MultiBind(this DependencyObject input, DependencyProperty property, IMultiValueConverter converter, object source, params PropertyPath[] paths)
        {
            var result = new MultiBinding()
            {
                Converter = converter,
                Mode = BindingMode.OneWay,
            };

            if (paths?.Length > 0)
            {
                foreach (var i in paths)
                {
                    var j = new Binding() { Path = i };
                    j.Mode 
                        = BindingMode.OneWay;
                    j.Source 
                        = source;

                    result.Bindings.Add(j);
                }
            }
            return input.Bind(property, result);
        }

        //...

        public static void Unbind(this DependencyObject input, DependencyProperty property) => BindingOperations.ClearBinding(input, property);

        //...

        public static T FindChildOfType<T>(this DependencyObject input) where T : DependencyObject
            => input.FindLogicalChildOfType<T>() ?? input.FindVisualChildOfType<T>();

        public static DependencyObject FindChildOfType(this DependencyObject input, Type type)
            => input.FindLogicalChildOfType(type) ?? input.FindVisualChildOfType(type);

        //...

        public static T FindLogicalChildOfType<T>(this DependencyObject input) where T : DependencyObject
        {
            if (input is not null)
            {
                foreach (var i in LogicalTreeHelper.GetChildren(input))
                {
                    if (i is DependencyObject j)
                    {
                        var result = j is T k ? k : j.FindLogicalChildOfType<T>();
                        if (result != null) return result;
                    }
                }
            }
            return default;
        }

        public static DependencyObject FindLogicalChildOfType(this DependencyObject input, Type type)
        {
            if (input is not null)
            {
                foreach (var i in LogicalTreeHelper.GetChildren(input))
                {
                    if (i is DependencyObject j)
                    {
                        var result = j.GetType().Inherits(type, true) || j.GetType().Implements(type) ? j : j.FindLogicalChildOfType(type);
                        if (result != null) return result;
                    }
                }
            }
            return default;
        }

        public static T FindVisualChildOfType<T>(this DependencyObject input) where T : DependencyObject
        {
            if (input is not null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(input); i++)
                {
                    var child = VisualTreeHelper.GetChild(input, i);
                    var result = child is T j ? j : child.FindVisualChildOfType<T>();
                    if (result != null) return result;
                }
            }
            return default;
        }

        public static DependencyObject FindVisualChildOfType(this DependencyObject input, Type type)
        {
            if (input is not null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(input); i++)
                {
                    var j = VisualTreeHelper.GetChild(input, i);
                    var result = j.GetType().Inherits(type, true) || j.GetType().Implements(type) ? j : j.FindVisualChildOfType(type);
                    if (result != null) return result;
                }
            }
            return default;
        }

        //...

        /// <summary>
        /// Attempts to find parent for specified object in following order: 
        /// VisualParent -> LogicalParent -> LogicalTemplatedParent.
        /// </summary>
        /// <remarks>
        /// Visual, FrameworkElement, and FrameworkContentElement types are supported. If the logical parent is not found, we try TemplatedParent.
        /// </remarks>
        /// <param name="input">The object to get the parent for.</param>
        public static DependencyObject FindParent(this DependencyObject input)
        {
            if (input is Popup)
            {
                //Case 126732 : To correctly detect parent of a popup we must do that exception case
                var popup = input as Popup;
                if (popup != null && popup.PlacementTarget != null)
                    return popup.PlacementTarget;
            }

            var visual = input as Visual;
            var parent = visual == null ? null : VisualTreeHelper.GetParent(visual);
            if (parent == null)
            {
                //No visual parent, check logical tree.
                var frameworkElement = input as FrameworkElement;
                if (frameworkElement != null)
                {
                    parent = frameworkElement.Parent;
                    if (parent == null)
                        parent = frameworkElement.TemplatedParent;
                }
                else
                {
                    var frameworkContentElement = input as FrameworkContentElement;
                    if (frameworkContentElement != null)
                    {
                        parent = frameworkContentElement.Parent;
                        if (parent == null)
                            parent = frameworkContentElement.TemplatedParent;
                    }
                }
            }
            return parent;
        }

        public static T FindParent<T>(this DependencyObject input, Predicate<T> predicate = null) where T : DependencyObject
        {
            var parent = input;
            while (parent != null)
            {
                parent = parent.FindParent();
                if (parent is T)
                {
                    if (predicate?.Invoke((T)parent) != false)
                        break;
                }
            }
            return parent is T result ? result : default;
        }

        public static DependencyObject FindParent(this DependencyObject input, Type type)
        {
            var parent = input.FindParent();
            while (parent != null && parent.GetType() != type)
            {
                if (parent is ContextMenu contextMenu)
                    parent = contextMenu.PlacementTarget;

                else parent = parent.FindParent();
            }
            return parent;
        }

        //...

        public static DependencyObject FindLogicalParent(this DependencyObject input) => LogicalTreeHelper.GetParent(input);

        public static T FindLogicalParent<T>(this DependencyObject input) where T : DependencyObject
        {
            do
            {
                if (input is T) 
                    return (T)input;

                input = input.FindLogicalParent();
            }
            while (input != null);
            return null;
        }

        //...

        public static IEnumerable<T> FindLogicalChildren<T>(this DependencyObject input) where T : DependencyObject
        {
            if (input == null)
                yield break;

            if (input is T t)
                yield return t;

            foreach (var i in LogicalTreeHelper.GetChildren(input).OfType<DependencyObject>())
            {
                foreach (var j in i.FindLogicalChildren<T>())
                {
                    if (j is T)
                        yield return j;
                }
            }
        }

        //...

        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject input) where T : DependencyObject
        {
            if (input != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(input); i++)
                {
                    var j = VisualTreeHelper.GetChild(input, i);
                    if (j != null && j is T)
                        yield return (T)j;

                    foreach (var k in FindVisualChildren<T>(j))
                        yield return k;
                }
            }
            yield break;
        }

        //...

        public static DependencyObject FindVisualParent(this DependencyObject input) => VisualTreeHelper.GetParent(input);

        public static T FindVisualParent<T>(this DependencyObject input) where T : DependencyObject
        {
            if (input is DependencyObject)
            {
                do
                {
                    if (input is T i)
                        return i;

                    if (input is Visual)
                    {
                        input = input.FindVisualParent();
                    }
                    else break;
                }
                while (input != null);
            }
            return null;
        }

        //...

        /// <summary>
        /// Returns true if the specified element is a child of parent somewhere in the visual 
        /// tree. This method will work for Visual, FrameworkElement and FrameworkContentElement.
        /// </summary>
        /// <param name="input">The element that is potentially a child of the specified parent.</param>
        /// <param name="parent">The element that is potentially a parent of the specified element.</param>
        public static bool IsDescendantOf(this DependencyObject input, DependencyObject parent)
        {
            while (input != null)
            {
                if (input == parent)
                    return true;

                input = input.FindParent();
            }
            return false;
        }

        //...

        public static void Select(this DependencyObject input, bool select)
        {
            if (input.IsAny(typeof(ListBoxItem), typeof(DataGridRow)))
                Selector.SetIsSelected(input, select);

            else if (input is TreeViewItem item)
                XTreeViewItem.SetIsSelected(item, select);

            else throw new NotSupportedException();
        }

        public static Result TrySelect(this DependencyObject input, bool select) => Try.Invoke(() => input.Select(select));

        #endregion
    }
}