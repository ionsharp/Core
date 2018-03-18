using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Imagin.Colour.Controls.Models
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Component : ObjectBase
    {
        /// <summary>
        /// Occurs when the value of the component changes.
        /// </summary>
        public event EventHandler<EventArgs<double>> ValueChanged;

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
        internal ColorModel ColorSpace { get; set; } = default(ColorModel);
        
        /// <summary>
        /// 
        /// </summary>
        public Imagin.Colour.Conversion.ColorConverter Converter { get; set; }

        /// <summary>
        /// Gets a label that represents the component.
        /// </summary>
        public abstract string Label { get; }

        /// <summary>
        /// Gets the maximum value of the component.
        /// </summary>
        public abstract double Maximum { get; }

        /// <summary>
        /// Gets the minimum value of the component.
        /// </summary>
        public abstract double Minimum { get; }

        /// <summary>
        /// Gets whether or not the component can be selected.
        /// </summary>
        public virtual bool CanSelect => false;

        /// <summary>
        /// Gets the increment.
        /// </summary>
        public virtual double Increment => 1.0;

        /// <summary>
        /// Gets the unit of measurement.
        /// </summary>
        public virtual ComponentUnit Unit => ComponentUnit.None;

        double _value = 0;
        /// <summary>
        /// Gets or sets the value of the component.
        /// </summary>
        public double Value
        {
            get => _value;
            set
            {
                Property.Set(this, ref _value, value);
                OnValueChanged(value);
            }
        }

        /// <summary>
        /// Gets the hashcode of the component.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => GetType().ToString().GetHashCode();

        /// <summary>
        /// Initializes a new instance of the <see cref="Component"/> class.
        /// </summary>
        public Component() : base()
        {
            OnPropertyChanged("Increment");
            OnPropertyChanged("Maximum");
            OnPropertyChanged("Minimum");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Clean(double value) => value.Round().Coerce(Maximum, Minimum).ToInt32();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Clean(int value) => value.Coerce(Maximum.ToInt32(), Minimum.ToInt32());

        /// <summary>
        /// Gets the value of the component based on the given color.
        /// </summary>
        public abstract double GetValue(Color Color);
        
        /// <summary>
        /// Sets the value of the component based on the given color (asynchronously).
        /// </summary>
        /// <param name="Color"></param>
        public async Task SetAsync(Color Color) => Value = await Task.Run(() => GetValue(Color));

        /// <summary>
        /// Sets the value of the component based on the given color.
        /// </summary>
        /// <param name="Color"></param>
        public void Set(Color Color) => Value = GetValue(Color);

        /// <summary>
        /// Occurs when the value of the component changes.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnValueChanged(double value) => ValueChanged?.Invoke(this, new EventArgs<double>(value));
    }
}
