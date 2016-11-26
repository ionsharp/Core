using System;

namespace Imagin.Common.Events
{
    public class ChangedValue : Tuple<object, object>
    {
        public object OldValue
        {
            get
            {
                return this.Item1;
            }
        }

        public object NewValue
        {
            get
            {
                return this.Item2;
            }
        }

        public ChangedValue(object OldValue, object NewValue) : base(OldValue, NewValue)
        {
        }
    }
}
