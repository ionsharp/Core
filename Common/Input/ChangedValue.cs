using System;

namespace Imagin.Common.Input
{
    /// <summary>
    /// Represents a value that has changed.
    /// </summary>
    public class ChangedValue : Tuple<object, object>
    {
        /// <summary>
        /// The old value.
        /// </summary>
        public object OldValue
        {
            get
            {
                return this.Item1;
            }
        }

        /// <summary>
        /// The new value.
        /// </summary>
        public object NewValue
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
        public ChangedValue(object OldValue, object NewValue) : base(OldValue, NewValue)
        {
        }
    }
}
