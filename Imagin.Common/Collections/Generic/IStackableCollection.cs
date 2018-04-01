namespace Imagin.Common.Collections.Generic
{
    /// <summary>
    /// Defines methods to manipulate stackable collections.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public interface IStackable<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void Push(T item);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        T Peek();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        T Pop();
    }
}
