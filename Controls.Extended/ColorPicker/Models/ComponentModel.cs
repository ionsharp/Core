using Imagin.Common;
using Imagin.Common.Extensions;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public abstract class ComponentModel : AbstractObject
    {
        protected internal bool ColorChangedHandled = false;

        protected internal bool TextChangedHandled = false;

        public abstract string ComponentLabel
        {
            get;
        }

        public abstract string UnitLabel
        {
            get;
        }

        public virtual bool CanSelect
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// The largest possible value for a component (value when slider at top)
        /// </summary>
        public virtual int MaxValue
        {
            get
            {
                return 100;
            }
        }

        /// <summary>
        /// The smallest possible value for a component (value when slider at bottom)
        /// </summary>
        public virtual int MinValue
        {
            get
            {
                return 0;
            }
        }

        double currentValue = 0.0;
        public double CurrentValue
        {
            get
            {
                return this.currentValue;
            }
            set
            {
                this.currentValue = value;
                OnPropertyChanged("CurrentValue");
            }
        }

        /// <summary>
        /// The value of the component for a given color
        /// </summary>
        public abstract int GetValue(Color Color);

        public override int GetHashCode()
        {
            return this.GetType().ToString().GetHashCode();
        }

        public void SetValue(Color Color)
        {
            this.CurrentValue = this.GetValue(Color).ToDouble();
        }
    }
}
