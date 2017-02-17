using System;

namespace Imagin.Common.Input
{
    /// <summary>
    /// Specifies a changed value.
    /// </summary>
    /// <typeparam name="TKind">The kind of value.</typeparam>
    public class ChangedValue<TKind> : Tuple<TKind, TKind>
    {
        /// <summary>
        /// The old value.
        /// </summary>
        public TKind OldValue
        {
            get
            {
                return this.Item1;
            }
        }

        /// <summary>
        /// The new value.
        /// </summary>
        public TKind NewValue
        {
            get
            {
                return this.Item2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        public ChangedValue(TKind OldValue, TKind NewValue) : base(OldValue, NewValue)
        {
        }
    }
}
