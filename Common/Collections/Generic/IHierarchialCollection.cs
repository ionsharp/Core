using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace Imagin.Common.Collections.Generic
{
    /// <summary>
    /// Defines methods to manipulate hierarchial collections.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public interface IHierarchialCollection<T>
    {
        int Count
        {
            get;
        }

        void Add(T Item);

        bool AddTo(T A, T B);

        Task<bool> BeginAddTo(T A, T B);
        
        Task<bool> BeginClone(T Item);

        Task<object> BeginGetParent(T Item);

        Task<bool> BeginLevelUp(T Item);

        Task<bool> BeginMove(T Item, Direction Direction);

        Task<bool> BeginMoveDown(T Item);

        Task<bool> BeginMoveUp(T Item);

        Task<bool> BeginRemove(T Item);

        Task<bool> BeginWrap(T Folder, T Item);

        void Clear();

        int IndexOf(T Item);
    }
}
