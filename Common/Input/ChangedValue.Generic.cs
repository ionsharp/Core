using System;

namespace Imagin.Common.Input
{
    /// <summary>
    /// Specifies a changed value.
    /// </summary>
    /// <typeparam name="TValue">The kind of value.</typeparam>
    public class ChangedValue<TValue> : Tuple<TValue, TValue>
    {
        /// <summary>
        /// The old value.
        /// </summary>
        public TValue OldValue
        {
            get
            {
                return Item1;
            }
        }

        /// <summary>
        /// The new value.
        /// </summary>
        public TValue NewValue
        {
            get
            {
                return Item2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        public ChangedValue(TValue OldValue, TValue NewValue) : base(OldValue, NewValue)
        {
        }
    }
}
