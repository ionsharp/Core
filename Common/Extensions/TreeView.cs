using System.Windows.Controls;
using System;

namespace Imagin.Common.Extensions
{
    public static class TreeViewExtensions
    {
        /// <summary>
        /// Collapses siblings. This is an extension of <CollapseSiblings(ItemsControl Items, bool Collapse)>.
        /// </summary>
        public static void CollapseSiblings(this TreeViewItem Item)
        {
            if (Item == null)
                return;
            Console.WriteLine(Item.GetType().ToString());
            ItemsControl Parent = Item.FindParent<TreeViewItem>();
            if (Parent == null)
            {
                if ((Parent = Item.FindParent<TreeView>()) == null)
                    return;
            }
            Console.WriteLine(Parent.GetType().ToString());
            foreach (var i in Parent.Items)
            {
                TreeViewItem t = Parent.ItemContainerGenerator.ContainerFromItem(i).As<TreeViewItem>();
                if (t != null && t != Item)
                {
                    t.IsExpanded = false;
                    Collapse(t);
                }
            }
        }

        static void Collapse(this ItemsControl Items)
        {
            foreach (TreeViewItem t in Items.Items)
            {
                t.IsExpanded = false;
                Collapse(t);
            }
        }
    }
}
