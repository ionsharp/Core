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
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs<T>> Changed;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        T Get();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        void OnChanged(T Value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        void Set(T Value);
    }
}
