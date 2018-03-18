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
        readonly object maximum;
        /// <summary>
        /// 
        /// </summary>
        public object Maximum
        {
            get => maximum;
        }

        readonly object minimum;
        /// <summary>
        /// 
        /// </summary>
        public object Minimum
        {
            get => minimum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Minimum"></param>
        /// <param name="Maximum"></param>
        public ConstraintAttribute(object Minimum, object Maximum) : base()
        {
            maximum = Maximum;
            minimum = Minimum;
        }
    }
}
