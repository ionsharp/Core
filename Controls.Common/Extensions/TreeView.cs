using Imagin.Common.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Controls.Common.Extensions
{
    public static class TreeViewExtensions
    {
        #region Methods

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

        public static void SelectItem(this TreeView TreeView, TreeViewItem Item)
        {
            SelectNone(TreeView);
            TreeViewItemExtensions.SetIsSelected(Item, true);
            TreeViewExtensions.SetStartItem(TreeView, Item);
        }

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

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(TreeViewExtensions));
        public static IList GetSelectedItems(TreeView TreeView)
        {
            return (IList)TreeView.GetValue(SelectedItemsProperty);
        }
        public static void SetSelectedItems(TreeView TreeView, IList Value)
        {
            TreeView.SetValue(SelectedItemsProperty, Value);
        }

        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.RegisterAttached("SelectionMode", typeof(TreeViewSelectionMode), typeof(TreeViewExtensions), new PropertyMetadata(TreeViewSelectionMode.Single, OnSelectionModeChanged));
        public static TreeViewSelectionMode GetSelectionMode(TreeView TreeView)
        {
            return (TreeViewSelectionMode)TreeView.GetValue(SelectionModeProperty);
        }
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
