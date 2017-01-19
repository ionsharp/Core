using System;
using Imagin.Common.Input;

namespace Imagin.Common
{
    /// <summary>
    /// Represents an object that is a variant of another object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IVariant<T>
    {
        event EventHandler<EventArgs<T>> Changed;

        T Get();

        void OnChanged(T Value);

        void Set(T Value);
    }
}
