namespace Imagin.Common.Collections
{
    /// <summary>
    /// Defines methods to manipulate stackable collections.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public interface IStackable<T>
    {
        void Push(T item);

        T Peek();

        T Pop();
    }
}
