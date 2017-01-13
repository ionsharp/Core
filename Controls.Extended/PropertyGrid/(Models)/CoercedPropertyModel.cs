using Imagin.Common;

namespace Imagin.Controls.Extended
{
    /// <summary>
    /// Represents a property that has both a minimum and maximum value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CoercedPropertyModel<T> : PropertyModel<T>, ICoercable, ICoercable<T>
    {
        T maximum = default(T);
        public T Maximum
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

        T minimum = default(T);
        public T Minimum
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

        public void SetConstraint(object minimum, object maximum)
        {
            Maximum = (T)maximum;
            Minimum = (T)minimum;
        }

        internal CoercedPropertyModel() : base()
        {
        }
    }
}
