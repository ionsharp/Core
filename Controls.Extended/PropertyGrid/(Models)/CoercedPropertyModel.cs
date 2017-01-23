using Imagin.Common;

namespace Imagin.Controls.Extended
{
    /// <summary>
    /// Represents a property that has both a minimum and maximum value.
    /// </summary>
    /// <typeparam name="TPrimitive"></typeparam>
    public class CoercedPropertyModel<TPrimitive> : PropertyModel<TPrimitive>, ICoercable, ICoercable<TPrimitive>
    {
        TPrimitive maximum = default(TPrimitive);
        /// <summary>
        /// 
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
        /// 
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
        /// 
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
