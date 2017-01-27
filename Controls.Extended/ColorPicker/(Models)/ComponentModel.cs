using Imagin.Common;
using Imagin.Common.Drawing;
using System.Windows.Media;
using System;
using Imagin.Common.Extensions;
using System.Threading.Tasks;

namespace Imagin.Controls.Extended
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ComponentModel : AbstractObject
    {
        /// <summary>
        /// 
        /// </summary>
        protected internal bool ColorChangedHandled = false;

        /// <summary>
        /// 
        /// </summary>
        protected internal bool TextChangedHandled = false;

        /// <summary>
        /// 
        /// </summary>
        protected ColorSpaceModel ColorSpace { get; private set; } = default(ColorSpaceModel);

        /// <summary>
        /// 
        /// </summary>
        protected Illuminant Illuminant
        {
            get
            {
                return ColorSpace.Illuminant;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected ObserverAngle Observer
        {
            get
            {
                return ColorSpace.Observer;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract string ComponentLabel
        {
            get;
        }

        /// <summary>
        /// The largest possible value for a component (value when slider at top)
        /// </summary>
        public double Maximum
        {
            get
            {
                return GetMaximum();
            }
        }

        /// <summary>
        /// The smallest possible value for a component (value when slider at bottom)
        /// </summary>
        public double Minimum
        {
            get
            {
                return GetMinimum();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool CanSelect
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual double Increment
        {
            get
            {
                return 1d;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string UnitLabel
        {
            get
            {
                return string.Empty;
            }
        }

        double _value = 0;
        /// <summary>
        /// 
        /// </summary>
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return GetType().ToString().GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract double GetMaximum();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract double GetMinimum();

        /// <summary>
        /// The value of the component for a given color
        /// </summary>
        public abstract double GetValue(Color Color);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Color"></param>
        public async Task BeginSet(Color Color)
        {
            Value = await Task.Run(() => GetValue(Color));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Color"></param>
        public void Set(Color Color)
        {
            Value = GetValue(Color);
        }

        /// <summary>
        /// 
        /// </summary>
        public ComponentModel(ColorSpaceModel colorSpace) : base()
        {
            ColorSpace = colorSpace;
            OnPropertyChanged("Increment");
            OnPropertyChanged("Maximum");
            OnPropertyChanged("Minimum");
        }
    }
}
