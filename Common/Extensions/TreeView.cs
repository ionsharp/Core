using System.Windows.Controls;
using System;

namespace Imagin.Common.Extensions
{
    public static class TreeViewExtensions
    {
        /*
        public static void CollapseSiblings(this TreeViewItem Item, TreeView TreeView)
        {
            if (Item == null)
                return;
            Console.WriteLine("Item.Type = " + Item.GetType().ToString());
            ItemsControl Parent = Item.FindParent<TreeViewItem>();
            if (Parent == null)
                Parent = TreeView;
            Console.WriteLine("Parent.Type = " + Parent.GetType().ToString());
            foreach (var i in Parent.Items)
            {
                TreeViewItem t = TreeView.ItemContainerGenerator.ContainerFromItem(i).As<TreeViewItem>();
                if (t != null && t != Item)
                {
                    t.IsExpanded = false;
                    t.Collapse(TreeView);
                }
            }
        }

        static void Collapse(this ItemsControl Items, TreeView TreeView)
        {
            foreach (TreeViewItem t in Items.Items)
            {
                t.IsExpanded = false;
                Collapse(t);
            }
        }
        */

        /// <summary>
        /// Collapses siblings. This is an extension of <CollapseSiblings(ItemsControl Items, bool Collapse)>.
        /// </summary>
        public static void CollapseSiblings(this TreeViewItem Item, bool Collapse = true)
        {
            if (Item == null || !Item.Parent.Is<TreeViewItem>())
                return;
            TreeViewItem Parent = (TreeViewItem)Item.Parent;
            foreach (TreeViewItem t in Parent.Items)
            {
                if (t != Item)
                {
                    CollapseSiblings(t, true);
                    t.IsExpanded = !Collapse;
                }
            }
        }

        /// <summary>
        /// Recursively collapse siblings.
        /// </summary>
        static void CollapseSiblings(this ItemsControl Items, bool Collapse)
        {
            foreach (object i in Items.Items)
            {
                ItemsControl Child = Items.ItemContainerGenerator.ContainerFromItem(i) as ItemsControl;
                if (Child == null)
                    continue;
                CollapseSiblings(Child, Collapse);
                TreeViewItem Item = Child.As<TreeViewItem>();
                if (Item != null)
                    Item.IsExpanded = !Collapse;
            }
        }
    }
}
