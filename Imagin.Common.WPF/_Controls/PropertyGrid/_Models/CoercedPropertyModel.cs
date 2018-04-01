using Imagin.Common;
using System;

namespace Imagin.Common
{
    /// <summary>
    /// Represents a property that has both a minimum and maximum value.
    /// </summary>
    /// <typeparam name="TPrimitive"></typeparam>
    public class CoercedPropertyModel<TPrimitive> : PropertyModel<TPrimitive>, IRange, IRange<TPrimitive>
    {
        TPrimitive _maximum = default(TPrimitive);
        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        public TPrimitive Maximum
        {
            get => _maximum;
            set => SetValue(ref _maximum, value);
        }

        TPrimitive _minimum = default(TPrimitive);
        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        public TPrimitive Minimum
        {
            get => _minimum;
            set => SetValue(ref _minimum, value);
        }

        /// <summary>
        /// Sets the constraint with the given minimum and maximum values.
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public void SetRange(object minimum, object maximum)
        {
            Maximum = (TPrimitive)maximum;
            Minimum = (TPrimitive)minimum;
        }

        /// <summary>
        /// This particular method is neither needed nor supported.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        object IRange.Coerce(object value)
            => throw new NotSupportedException();

        internal CoercedPropertyModel() : base() { }
    }
}
