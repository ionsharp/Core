using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;

namespace Imagin.Common.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class TreeViewExtensions
    {
        #region Methods

        /// <summary>
        /// Enumerate <see cref="ItemsControl"/> performing given action for each child.
        /// <para>(recursive)</para>
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="Action"></param>
        public static void Enumerate(this ItemsControl Control, Action<object, ItemsControl> Action)
        {
            Control.Enumerate<object>(Action);
        }

        /// <summary>
        /// Enumerate <see cref="ItemsControl"/> performing given action for each child; return false to continue (skip children, continue with next), null to break (skip everything else), or true (proceed normally).
        /// <para>(recursive)</para>
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="Action"></param>
        public static void Enumerate(this ItemsControl Control, Func<object, ItemsControl, bool?> Action)
        {
            Control.Enumerate<object>(Action);
        }

        /// <summary>
        /// Enumerate given indices of <see cref="ItemsControl"/> and perform given action for each child.
        /// <para>(recursive)</para>
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="Action"></param>
        /// <param name="Indices"></param>
        public static void Enumerate(this ItemsControl Control, Action<object, ItemsControl> Action, params int[] Indices)
        {
            Control.Enumerate<object>(Action, Indices);
        }

        /// <summary>
        /// Enumerate <see cref="ItemsControl"/> performing given action for each child.
        /// <para>(recursive)</para>
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="Action"></param>
        public static void Enumerate<T>(this ItemsControl Control, Action<T, ItemsControl> Action)
        {
            if (Control.Items != null)
            {
                foreach (var i in Control.Items)
                {
                    if (i.IsNot<T>())
                        throw new InvalidCastException("Item must be of type T");

                    var j = Control.ItemContainerGenerator.ContainerFromItem(i).To<ItemsControl>();
                    Action(i.As<T>(), j);

                    if (j != null)
                        j.Enumerate(Action);
                }
            }
        }

        /// <summary>
        /// Enumerate <see cref="ItemsControl"/> performing given action for each child; return false to continue (skip children, continue with next), null to break (skip everything else), or true (proceed normally). 
        /// <para>(recursive)</para>
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="Action"></param>
        public static void Enumerate<T>(this ItemsControl Control, Func<T, ItemsControl, bool?> Action)
        {
            if (Control.Items != null)
            {
                foreach (var i in Control.Items)
                {
                    if (i.IsNot<T>())
                        throw new InvalidCastException("Item must be of type T");

                    var j = Control.ItemContainerGenerator.ContainerFromItem(i).To<ItemsControl>();

                    var Result = Action(i.As<T>(), j);

                    if (Result == null)
                        break;

                    if (!Result.Value)
                        continue;

                    if (j != null)
                        j.Enumerate(Action);
                }
            }
        }

        /// <summary>
        /// Enumerate given indices of <see cref="ItemsControl"/> and perform given action for each child. 
        /// <para>(recursive)</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Control"></param>
        /// <param name="Action"></param>
        /// <param name="Indices"></param>
        public static void Enumerate<T>(this ItemsControl Control, Action<T, ItemsControl> Action, params int[] Indices)
        {
            if (Control.Items != null && Indices.Length > 0)
            {
                var m = Indices[0];
                var n = 0;
                foreach (var i in Control.Items)
                {
                    if (m == n)
                    {
                        if (i.IsNot<T>())
                            throw new InvalidCastException("Item must be of type T");

                        var j = (ItemsControl)Control.ItemContainerGenerator.ContainerFromItem(i);
                        Action((T)i, j);

                        if (j != null)
                            j.Enumerate<T>(Action, Indices.Skip(1).ToArray());
                    }

                    n++;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="AllItems"></param>
        public static void GetAllItems(ItemsControl Control, ICollection<TreeViewItem> AllItems)
        {
            if (Control != null)
            {
                for (int i = 0; i < Control.Items.Count; i++)
                {
                    var Item = Control.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                    if (Item != null)
                    {
                        AllItems.Add(Item);
                        GetAllItems(Item, AllItems);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TreeView"></param>
        /// <param name="Item"></param>
        public static void SelectItem(this TreeView TreeView, TreeViewItem Item)
        {
            SelectNone(TreeView);
            TreeViewItemExtensions.SetIsSelected(Item, true);
            TreeViewExtensions.SetStartItem(TreeView, Item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TreeView"></param>
        /// <param name="Item"></param>
        public static void SelectItems(this TreeView TreeView, TreeViewItem Item)
        {
            if (Item == null) return;

            if ((Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) == (ModifierKeys.Control | ModifierKeys.Shift))
                SelectItemsContinuously(TreeView, Item, true);
            else if (Keyboard.Modifiers == ModifierKeys.Control)
                SelectItemsRandomly(TreeView, Item);
            else if (Keyboard.Modifiers == ModifierKeys.Shift)
                SelectItemsContinuously(TreeView, Item);
            else SelectItem(TreeView, Item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TreeView"></param>
        /// <param name="TreeViewItem"></param>
        /// <param name="ShiftControl"></param>
        public static void SelectItemsContinuously(this TreeView TreeView, TreeViewItem TreeViewItem, bool ShiftControl = false)
        {
            var StartItem = TreeViewExtensions.GetStartItem(TreeView);
            if (StartItem != null)
            {
                if (StartItem == TreeViewItem)
                {
                    SelectItem(TreeView, TreeViewItem);
                    return;
                }
                var AllItems = new List<TreeViewItem>();
                GetAllItems(TreeView, AllItems);

                bool IsBetween = false;
                foreach (var Item in AllItems)
                {
                    if (Item == TreeViewItem || Item == StartItem)
                    {
                        //Toggle to true if first element is found and back to false if last element is found
                        IsBetween = !IsBetween;
                        //Set boundary element
                        TreeViewItemExtensions.SetIsSelected(Item, true);
                        continue;
                    }
                    if (IsBetween)
                    {
                        TreeViewItemExtensions.SetIsSelected(Item, true);
                        continue;
                    }
                    if (!ShiftControl)
                        TreeViewItemExtensions.SetIsSelected(Item, false);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TreeView"></param>
        /// <param name="TreeViewItem"></param>
        public static void SelectItemsRandomly(this TreeView TreeView, TreeViewItem TreeViewItem)
        {
            TreeViewItemExtensions.SetIsSelected(TreeViewItem, !TreeViewItemExtensions.GetIsSelected(TreeViewItem));
            if (TreeViewExtensions.GetStartItem(TreeView) == null || Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (TreeViewItemExtensions.GetIsSelected(TreeViewItem))
                    TreeViewExtensions.SetStartItem(TreeView, TreeViewItem);
            }
            else
            {
                if (TreeViewExtensions.GetSelectedItems(TreeView).Count == 0)
                    TreeViewExtensions.SetStartItem(TreeView, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Control"></param>
        public static void SelectNone(this ItemsControl Control)
        {
            if (Control != null)
            {
                for (int i = 0; i < Control.Items.Count; i++)
                {
                    var Item = Control.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                    if (Item != null)
                    {
                        TreeViewItemExtensions.SetIsSelected(Item, false);
                        SelectNone(Item);
                    }
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(TreeViewExtensions));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TreeView"></param>
        /// <returns></returns>
        public static IList GetSelectedItems(TreeView TreeView)
        {
            return (IList)TreeView.GetValue(SelectedItemsProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TreeView"></param>
        /// <param name="Value"></param>
        public static void SetSelectedItems(TreeView TreeView, IList Value)
        {
            TreeView.SetValue(SelectedItemsProperty, Value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.RegisterAttached("SelectionMode", typeof(TreeViewSelectionMode), typeof(TreeViewExtensions), new PropertyMetadata(TreeViewSelectionMode.Single, OnSelectionModeChanged));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TreeView"></param>
        /// <returns></returns>
        public static TreeViewSelectionMode GetSelectionMode(TreeView TreeView)
        {
            return (TreeViewSelectionMode)TreeView.GetValue(SelectionModeProperty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TreeView"></param>
        /// <param name="Value"></param>
        public static void SetSelectionMode(TreeView TreeView, TreeViewSelectionMode Value)
        {
            TreeView.SetValue(SelectionModeProperty, Value);
        }
        static void OnSelectionModeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var TreeView = sender as TreeView;
            if ((TreeViewSelectionMode)e.NewValue == TreeViewSelectionMode.Single)
            {
                var SelectedItems = TreeViewExtensions.GetSelectedItems(TreeView);
                if (SelectedItems != null && SelectedItems.Count > 1)
                    TreeView.SelectItem(TreeView.ItemContainerGenerator.ContainerFromItem(SelectedItems[0]).As<TreeViewItem>());
            }
        }

        internal static readonly DependencyProperty StartItemProperty = DependencyProperty.RegisterAttached("StartItem", typeof(TreeViewItem), typeof(TreeViewExtensions));
        internal static TreeViewItem GetStartItem(TreeView element)
        {
            return (TreeViewItem)element.GetValue(StartItemProperty);
        }
        internal static void SetStartItem(TreeView element, TreeViewItem value)
        {
            element.SetValue(StartItemProperty, value);
        }

        #endregion
    }
}
