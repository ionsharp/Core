using System;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Extensions
{
    public static class TreeViewExtensions
    {
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
            Parent.As<ItemsControl>().CollapseSiblings(Item);
        }

        /// <summary>
        /// Imagin.Common: Collapse all items in ItemsControl (siblings of <param name="Source"/>).
        /// </summary>
        static void CollapseSiblings(this ItemsControl Parent, TreeViewItem Source)
        {
            if (Parent == null || Parent.Items == null) return;
            foreach (var i in Parent.Items)
            {
                var c = Parent.ItemContainerGenerator.ContainerFromItem(i);
                if (c == null) continue;
                var Child = c.As<TreeViewItem>();
                if (Child != null && !Child.Equals(Source))
                    Child.IsExpanded = false;
            }
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
    }
}
