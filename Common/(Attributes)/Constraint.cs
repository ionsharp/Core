using System;

namespace Imagin.Common
{
    /// <summary>
    /// Specifies a constraint for a property with both a minimum and maximum value.
    /// </summary>
    /// <remarks>
    /// Constraint values are stored as object type because generic types cannot inherit from Attribute.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ConstraintAttribute : Attribute
    {
        public object Maximum
        {
            get; set;
        }

        public object Minimum
        {
            get; set;
        }

        public ConstraintAttribute(object Minimum, object Maximum)
        {
            this.Maximum = Maximum;
            this.Minimum = Minimum;
        }
    }
}
