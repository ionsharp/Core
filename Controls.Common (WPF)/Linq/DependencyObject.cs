using Imagin.Common.Debug;
using Imagin.Common.Linq;
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
