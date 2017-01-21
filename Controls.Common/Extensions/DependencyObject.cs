using Imagin.Common.Debug;
using Imagin.Common.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Imagin.Controls.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DependencyObjectExtensions
    {
        /// <summary>
        /// Select the given element, if supported; element is valid if supports <see cref="Selector.SetIsSelected(DependencyObject, bool)"/> or is <see cref="TreeViewItem"/>.
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="IsSelected"></param>
        public static void Select(this DependencyObject Value, bool IsSelected)
        {
            if (Value.IsAny(typeof(ListBoxItem)))
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
    }
}
