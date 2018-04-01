using System;
using Imagin.Common.Input;

namespace Imagin.Common
{
    /// <summary>
    /// Specifies a bindable <see cref="object"/> that encapsulates an unbindable <see cref="object"/>.
    /// </summary>
    /// <typeparam name="TStructure"></typeparam>
    public interface IVariation<TStructure>
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs<TStructure>> Changed;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        TStructure Get();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        void OnChanged(TStructure Value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        void Set(TStructure Value);
    }
}
