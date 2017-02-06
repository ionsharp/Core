using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Imagin.Common.Collections.Concurrent;

namespace Imagin.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class IListGenericExtensions
    {
        /// <summary>
        /// Enumerate all items contained in a collection (recursively), including each item's items (all items must implement <see cref="IContainer{T}"/>).
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Parent"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        static bool Enumerate<TItem>(this IList<TItem> Source, object Parent, Func<object, TItem, bool> Action) where TItem : IContainer<TItem>
        {
            //For each item in the current collection of items...
            foreach (var i in Source)
            {
                //Perform action with the current item and it's parent (the parent is the collection if one isn't already specified by method argument).
                if (!Action(Parent ?? Source, i))
                    return false;

                //If the item has been assigned an instance of a collection, enumerate it's items (recursively!). If the result is false, stop enumeration.
                if (i.Items != null && !i.Items.Enumerate(i, Action))
                    return false;
            }
            //Enumeration of all items in the current collection has successfully ended.
            return true;
        }

        /// <summary>
        /// Get the logical parent for the given item. <para>(recursive)</para>
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source">The collection to search.</param>
        /// <param name="Parent">The parent to evaluate.</param>
        /// <param name="Item">The item to get the parent of.</param>
        /// <returns></returns>
        static object GetParent<TItem>(this IList<TItem> Source, object Parent, TItem Item) where TItem : IContainer<TItem>
        {

            var Result = default(object);

            //Enumerate all items in the collection
            Source.Enumerate((i, j) =>
            {
                //If the current item is the item we want the parent of, get a reference to it's parent and stop enumeration.
                if (j.Equals(Item))
                {
                    Result = i;
                    return false;
                }
                return true;
            });

            return Result;
        }

        /// <summary>
        /// Move the given item in the given direction (i.e., increase or decrease the item's index).
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Item"></param>
        /// <param name="Direction"></param>
        /// <returns></returns>
        static void Move<TItem>(this IList<TItem> Source, TItem Item, Direction Direction) where TItem : IContainer<TItem>
        {
            //Get the parent of the item.
            var Parent = Source.GetParent(Item);
            var p = Parent.Equals(Source) ? Source : Parent is TItem ? ((TItem)Parent).Items : null;

            //If parent cannot be found, assume we are done already.
            if (p == null) return;

            //Get the index of the item using it's parent.
            var i = p.IndexOf(Item);

            var sibling = default(TItem);

            //If moving up (or decreasing index) and previous index is valid (or >= 0)...
            if (Direction == Direction.Up && (i - 1) >= 0)
            {
                //Get reference to previous item
                sibling = p[i - 1];
            }
            //If moving down (or increasing index) and next index is valid (or < total items)...
            else if (Direction == Direction.Down && (i + 1) < p.Count)
                sibling = p[i + 1];

            //If moving is possible...
            if (sibling != null)
            {
                //Remove the item's sibling.
                p.Remove(sibling);
                //Insert it's sibling at it's former location.
                p.Insert(i, sibling);
            }
        }

        //.......................................................................................................

        /// <summary>
        /// Clone the given item contained in the collection.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public static void Clone<TItem>(this IList<TItem> Source, TItem Item) where TItem : ICloneable, IContainer<TItem>
        {
            //Get the parent of the item we want to clone.
            var Parent = Source.GetParent(Item);
            var p = Parent.Equals(Source) ? Source : Parent is TItem ? ((TItem)Parent).Items : null;
            //If we have a reference to a collection, clone the item and add to it's parent's collection.
            p?.Add((TItem)Item.Clone());
        }

        /// <summary>
        /// Enumerate all items contained in a collection (recursively), including each item's items (all items must implement <see cref="IContainer{T}"/>).
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Action"></param>
        public static void Enumerate<TItem>(this IList<TItem> Source, Func<object, TItem, bool> Action) where TItem : IContainer<TItem>
        {
            Source.Enumerate(Source, Action);
        }

        /// <summary>
        /// Get the logical parent for the given item. <para>(recursive)</para>
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public static object GetParent<TItem>(this IList<TItem> Source, TItem Item) where TItem : IContainer<TItem>
        {
            return Source.GetParent(Source, Item);
        }

        /// <summary>
        /// Move the given item up one level, or remove the item from it's current parent and add it to the collection of it's parent's parent.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public static void LevelUp<TItem>(this IList<TItem> Source, TItem Item) where TItem : IContainer<TItem>
        {
            //Get the parent of the item.
            var Parent = Source.GetParent(Item);

            //If the parent is an item, leveling up is possible; in all other cases, it is not.
            if (Parent is TItem)
            {
                //Get the parent of the item's parent, or the item's parent's parent.
                var parentOfParent = Source.GetParent((TItem)Parent);
                var ParentOfParent = parentOfParent == Source ? Source : parentOfParent is TItem ? ((TItem)parentOfParent).Items : null;

                //Remove the item from it's current parent.
                Parent.As<TItem>().Items.Remove(Item);

                //Add the item to it's former parent's parent.
                ParentOfParent.Add(Item);
            }
        }

        /// <summary>
        /// Move the given item down, or increase the item's index.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public static void MoveDown<TItem>(this IList<TItem> Source, TItem Item) where TItem : IContainer<TItem>
        {
            Source.Move(Item, Direction.Down);
        }

        /// <summary>
        /// Move the given item up, or decrease the item's index.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public static void MoveUp<TItem>(this IList<TItem> Source, TItem Item) where TItem : IContainer<TItem>
        {
            Source.Move(Item, Direction.Up);
        }

        /// <summary>
        /// Remove the given item from the collection without knowing what's it's parent is. <para>(recursive)</para> 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public static void RemoveRecursively<TItem>(this IList<TItem> Source, TItem Item) where TItem : IContainer<TItem>
        {
            //Get the parent of the item.
            var Parent = Source.GetParent(Item);
            var p = Parent.Equals(Source) ? Source : Parent is TItem ? ((TItem)Parent).Items : null;

            //If removing is possible, remove the item from it's parent.
            p.Remove(Item);
        }

        /// <summary>
        /// Add the given item to the given folder and insert the folder at the item's former location.
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Folder"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public static void Wrap<TItem>(this IList<TItem> Source, TItem Folder, TItem Item) where TItem : IContainer<TItem>
        {
            //Get the parent of the item.
            var Parent = Source.GetParent(Item);
            var p = Parent.Equals(Source) ? Source : Parent is TItem ? ((TItem)Parent).Items : null;

            //If parent cannot be found, assume we are done already.
            if (p == null) return;

            //Get the index of the item using it's parent.
            var i = p.IndexOf(Item);

            //Remove the item from it's parent.
            p.Remove(Item);
            //Add the item to the folder's collection.
            Folder.Items.Add(Item);
            //Insert the folder at the item's former location.
            p.Insert(i, Folder);
        }
    }
}
