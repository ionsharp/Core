using Imagin.Common.Extensions;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Controls.Common.Extensions
{
    public static class TreeViewItemExtensions
    {
        #region Methods

        /// <summary>
        /// Imagin.Common: Collapses siblings of <param name="Item"/>.
        /// </summary>
        public static void CollapseSiblings(this TreeViewItem Item)
        {
            var Parent = Item.GetParent().As<FrameworkElement>();
            while (!Parent.Is<TreeViewItem>())
            {
                if (Parent == null || Parent.Is<TreeView>())
                    break;
                Parent = Parent.GetParent().As<FrameworkElement>();
            }
            ItemsControlExtensions.CollapseSiblings(Parent as ItemsControl, Item);
        }

        /// <summary>
        /// Imagin.Common: Get node depth for specified TreeViewItem.
        /// </summary>
        public static int GetDepth(this TreeViewItem Item)
        {
            int Depth = 0;

            var Temp = Item as DependencyObject;
            while (Temp != null && !Temp.Is<TreeView>())
            {
                if (Temp.Is<TreeViewItem>()) Depth++;
                Temp = Temp.GetParent();
            }

            return Depth;
        }

        #endregion

        #region Properties

        public static readonly DependencyProperty CollapseSiblingsOnClickProperty = DependencyProperty.RegisterAttached("CollapseSiblingsOnClick", typeof(bool), typeof(TreeViewItemExtensions), new PropertyMetadata(false, OnCollapseSiblingsOnClickChanged));
        public static bool GetCollapseSiblingsOnClick(TreeViewItem Item)
        {
            return (bool)Item.GetValue(CollapseSiblingsOnClickProperty);
        }
        public static void SetCollapseSiblingsOnClick(TreeViewItem Item, bool Value)
        {
            if (Item != null)
                Item.SetValue(CollapseSiblingsOnClickProperty, Value);
        }
        static void OnCollapseSiblingsOnClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var TreeViewItem = d as TreeViewItem;
            var TreeView = TreeViewItem.GetVisualParent<TreeView>();

            if (TreeViewItem != null && TreeView != null)
            {
                if ((bool)e.NewValue)
                    TreeView.MouseLeftButtonUp += RegisterCollapseSiblingsOnClick;
                else TreeView.MouseLeftButtonUp -= RegisterCollapseSiblingsOnClick;
            }
        }
        static void RegisterCollapseSiblingsOnClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var Item = e.OriginalSource.As<DependencyObject>().GetVisualParent<TreeViewItem>();
            if (Item != null)
                Item.CollapseSiblings();
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.RegisterAttached("IsSelected", typeof(bool), typeof(TreeViewItemExtensions), new PropertyMetadata(false, OnIsSelectedChanged));
        public static bool GetIsSelected(TreeViewItem Item)
        {
            return (bool)Item.GetValue(IsSelectedProperty);
        }
        public static void SetIsSelected(TreeViewItem Item, bool Value)
        {
            if (Item != null)
                Item.SetValue(IsSelectedProperty, Value);
        }
        static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var TreeViewItem = d as TreeViewItem;
            var TreeView = TreeViewItem.GetVisualParent<TreeView>();

            if (TreeViewItem != null && TreeView != null)
            {
                var SelectedItems = TreeViewExtensions.GetSelectedItems(TreeView);
                if (SelectedItems != null)
                {
                    if (GetIsSelected(TreeViewItem))
                        SelectedItems.Add(TreeViewItem.DataContext);
                    else SelectedItems.Remove(TreeViewItem.DataContext);
                }
            }
        }

        public static readonly DependencyProperty ExpandOnClickProperty = DependencyProperty.RegisterAttached("ExpandOnClick", typeof(bool), typeof(TreeViewItemExtensions), new PropertyMetadata(false, OnExpandOnClickChanged));
        public static bool GetExpandOnClick(TreeViewItem Item)
        {
            return (bool)Item.GetValue(ExpandOnClickProperty);
        }
        public static void SetExpandOnClick(TreeViewItem Item, bool Value)
        {
            if (Item != null)
                Item.SetValue(ExpandOnClickProperty, Value);
        }
        static void OnExpandOnClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var TreeViewItem = d as TreeViewItem;
            var TreeView = TreeViewItem.GetVisualParent<TreeView>();

            if (TreeViewItem != null && TreeView != null)
            {
                if ((bool)e.NewValue)
                    TreeView.MouseLeftButtonUp += RegisterExpandOnClick;
                else TreeView.MouseLeftButtonUp -= RegisterExpandOnClick;
            }
        }
        static void RegisterExpandOnClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var Item = e.OriginalSource.As<DependencyObject>().GetVisualParent<TreeViewItem>();
            if (Item != null) 
                Item.IsExpanded = !Item.IsExpanded;
        }

        public static readonly DependencyProperty SelectOnRightClickProperty = DependencyProperty.RegisterAttached("SelectOnRightClick", typeof(bool), typeof(TreeViewItemExtensions), new PropertyMetadata(false, OnSelectOnRightClickChanged));
        public static bool GetSelectOnRightClick(TreeViewItem Item)
        {
            return (bool)Item.GetValue(SelectOnRightClickProperty);
        }
        public static void SetSelectOnRightClick(TreeViewItem Item, bool Value)
        {
            if (Item != null)
                Item.SetValue(SelectOnRightClickProperty, Value);
        }
        static void OnSelectOnRightClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var TreeViewItem = d as TreeViewItem;
            var TreeView = TreeViewItem.GetVisualParent<TreeView>();

            if (TreeViewItem != null && TreeView != null)
            {
                if ((bool)e.NewValue)
                    TreeView.PreviewMouseRightButtonDown += RegisterSelectOnRightClick;
                else TreeView.PreviewMouseRightButtonDown -= RegisterSelectOnRightClick;
            }
        }
        static void RegisterSelectOnRightClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var Item = e.OriginalSource.As<DependencyObject>().GetVisualParent<TreeViewItem>();
            if (Item != null)
            {
                Item.IsSelected = true;
                e.Handled = true;
            }
        }

        #endregion
    }
}
