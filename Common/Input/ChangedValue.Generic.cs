using System;

namespace Imagin.Common.Input
{
    /// <summary>
    /// Specifies a changed value.
    /// </summary>
    /// <typeparam name="TValue">The kind of value.</typeparam>
    public class ChangedValue<TValue>
    {
        readonly Tuple<TValue, TValue> _value;

        /// <summary>
        /// The old value.
        /// </summary>
        public TValue OldValue => _value.Item1;

        /// <summary>
        /// The new value.
        /// </summary>
        public TValue NewValue => _value.Item2;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        public ChangedValue(TValue OldValue, TValue NewValue) => _value = Tuple.Create<TValue, TValue>(OldValue, NewValue);
    }
}
