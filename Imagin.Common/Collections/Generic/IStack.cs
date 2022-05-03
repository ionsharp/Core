namespace Imagin.Common.Collections.Generic
{
    public interface IStack<T>
    {
        void Push(T item);

        T Peek();

        T Pop();
    }
}