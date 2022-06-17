using System;

namespace Imagin.Core
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class RangeAttribute : Attribute
    {
        public readonly object Increment;

        public readonly object Maximum;

        public readonly object Minimum;

        public RangeAttribute(object minimum, object maximum, object increment = null) : base()
        {
            Minimum = minimum;
            Maximum = maximum;
            Increment = increment;
        }
    }
}