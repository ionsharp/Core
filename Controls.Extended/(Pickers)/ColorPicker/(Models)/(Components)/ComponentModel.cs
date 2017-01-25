using Imagin.Common;
using Imagin.Common.Extensions;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

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
        public abstract string ComponentLabel
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract string UnitLabel
        {
            get;
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
        /// The largest possible value for a component (value when slider at top)
        /// </summary>
        public virtual int Maximum
        {
            get
            {
                return 100;
            }
        }

        /// <summary>
        /// The smallest possible value for a component (value when slider at bottom)
        /// </summary>
        public virtual int Minimum
        {
            get
            {
                return 0;
            }
        }

        int value = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }

        /// <summary>
        /// The value of the component for a given color
        /// </summary>
        public abstract int GetValue(Color Color);

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
        /// <param name="Color"></param>
        public void SetValue(Color Color)
        {
            Value = GetValue(Color);
        }

        /// <summary>
        /// 
        /// </summary>
        public ComponentModel() : base()
        {
            OnPropertyChanged("Maximum");
            OnPropertyChanged("Minimum");
        }
    }
}
