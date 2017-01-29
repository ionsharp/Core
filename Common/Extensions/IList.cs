using Imagin.Common.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Imagin.Common.Debug;

namespace Imagin.Common.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class IListExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Item"></param>
        /// <param name="Parent"></param>
        /// <returns></returns>
        static async Task<object> BeginGetParent<TItem>(this IList<TItem> Source, TItem Item, object Parent) where TItem : IContainer<TItem>
        {
            if (Item == null)
                throw new NullReferenceException("The item does not exist.");

            return await Task.Run(async () =>
            {
                var z = default(IList<TItem>);

                if (Parent.Equals(Source))
                {
                    z = Source;
                }
                else if (Parent is IContainer<TItem>)
                {
                    z = Parent.As<IContainer<TItem>>().Items;
                }
                else return null;

                var Result = default(object);
                foreach (var i in z)
                {
                    if (i.Equals(Item))
                    {
                        return Parent;
                    }
                    else
                    {
                        Result = await Source.BeginGetParent(Item, i);
                        if (Result != null)
                            break;
                    }
                }
                return Result;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Item"></param>
        /// <param name="Direction"></param>
        /// <returns></returns>
        static async Task BeginMove<TItem>(this IList<TItem> Source, TItem Item, Direction Direction) where TItem : IContainer<TItem>
        {
            var Parent = await Source.BeginGetParent(Item);

            var z = Source;
            if (Parent is TItem)
                z = ((TItem)Parent).Items;

            var i = Source.IndexOf(Item);

            var Remove = default(TItem);

            if (Direction == Direction.Up && (i - 1) >= 0)
                Remove = (TItem)Source[i - 1];
            else if (Direction == Direction.Down && (i + 1) < Source.Count)
                Remove = (TItem)Source[i + 1];

            if (Remove != null)
            {
                Source.Remove(Remove);
                Source.Insert(i, Remove);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public static async Task BeginClone<TItem>(this IList<TItem> Source, TItem Item) where TItem : ICloneable, IContainer<TItem>
        {
            var Parent = await Source.BeginGetParent(Item);

            var z = Source;
            if (Parent is TItem)
                z = ((TItem)Parent).Items;

            z?.Add((TItem)Item.Clone());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public static async Task<object> BeginGetParent<TItem>(this IList<TItem> Source, TItem Item) where TItem : IContainer<TItem>
        {
            return await Source.BeginGetParent(Item, Source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public static async Task BeginLevelUp<TItem>(this IList<TItem> Source, TItem Item) where TItem : IContainer<TItem>
        {
            var Parent = await Source.BeginGetParent(Item);
            if (Parent is TItem)
            {
                var ParentOfParent = await Source.BeginGetParent((TItem)Parent);

                Parent.As<TItem>().Items.Remove(Item);

                var i = Source;
                if (ParentOfParent is TItem)
                    i = ((TItem)ParentOfParent).Items;

                i?.Add(Item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public static async Task BeginMoveDown<TItem>(this IList<TItem> Source, TItem Item) where TItem : IContainer<TItem>
        {
            await Source.BeginMove(Item, Direction.Down);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public static async Task BeginMoveUp<TItem>(this IList<TItem> Source, TItem Item) where TItem : IContainer<TItem>
        {
            await Source.BeginMove(Item, Direction.Up);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public static async Task BeginRemove<TItem>(this IList<TItem> Source, TItem Item) where TItem : IContainer<TItem>
        {
            var Parent = await Source.BeginGetParent(Item);

            var z = Source;
            if (Parent is TItem)
                z = ((TItem)Parent).Items;

            z?.Remove(Item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="Source"></param>
        /// <param name="Folder"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public static async Task BeginWrap<TItem>(this IList<TItem> Source, TItem Folder, TItem Item) where TItem : IContainer<TItem>
        {
            var Parent = await Source.BeginGetParent(Item);

            var z = Source;
            if (Parent is TItem)
                z = ((IContainer<TItem>)Parent).Items;

            var i = z?.IndexOf(Item);

            z?.Remove(Item);
            Folder?.Items.Add(Item);
            z?.Insert(i.Value, Folder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Action"></param>
        public static void ForEach(this IList Source, Action<object> Action)
        {
            foreach (var i in Source)
                Action(i);
        }
    }
}
