using Imagin.Common.Input;
using System;
using System.Globalization;

namespace Imagin.Common.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILanguages
    {
        /// <summary>
        /// 
        /// </summary>
        WeakEvent<EventArgs<CultureInfo>> SetWeakEvent
        {
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Should be implemented in the following way:
        /// 
        /// add 
        /// { 
        ///     setWeak.Subscribe(value); 
        /// }
        /// remove 
        /// { 
        ///     setWeak.Unsubscribe(value); 
        /// }
        /// </remarks>
        event EventHandler<EventArgs<CultureInfo>> SetWeak;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<EventArgs<CultureInfo>> Set;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Language"></param>
        void OnSet(CultureInfo Language);
    }
}
