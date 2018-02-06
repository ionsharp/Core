using Imagin.Common;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// Represents a property that has both a minimum and maximum value.
    /// </summary>
    /// <typeparam name="TPrimitive"></typeparam>
    public class CoercedPropertyModel<TPrimitive> : PropertyModel<TPrimitive>, ICoercable, ICoercable<TPrimitive>
    {
        TPrimitive maximum = default(TPrimitive);
        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        public TPrimitive Maximum
        {
            get
            {
                return this.maximum;
            }
            set
            {
                this.maximum = value;
                OnPropertyChanged("Maximum");
            }
        }

        TPrimitive minimum = default(TPrimitive);
        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        public TPrimitive Minimum
        {
            get
            {
                return this.minimum;
            }
            set
            {
                this.minimum = value;
                OnPropertyChanged("Minimum");
            }
        }

        /// <summary>
        /// Sets the constraint with the given minimum and maximum values.
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        public void SetConstraint(object minimum, object maximum)
        {
            Maximum = (TPrimitive)maximum;
            Minimum = (TPrimitive)minimum;
        }

        internal CoercedPropertyModel() : base()
        {
        }
    }
}
