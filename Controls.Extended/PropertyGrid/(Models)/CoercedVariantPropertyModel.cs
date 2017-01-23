using Imagin.Common;
using Imagin.Common.Extensions;
using Imagin.Common.Input;
using System;

namespace Imagin.Controls.Extended
{
    /// <summary>
    /// Represents a property with a type that uses another type to modify it, and has both a minimum and maximum value.
    /// </summary>
    /// <typeparam name="TVariant">The type used to modify the property.</typeparam>
    /// <typeparam name="TPrimitive">The actual type of the property.</typeparam>
    public class CoercedVariantPropertyModel<TVariant, TPrimitive> : CoercedPropertyModel<TPrimitive> where TVariant : IVariant<TPrimitive>
    {
        bool ValueChangeHandled = false;

        TVariant variant = default(TVariant);
        /// <summary>
        /// 
        /// </summary>
        public TVariant Variant
        {
            get
            {
                return variant;
            }
            set
            {
                variant = value;
                OnPropertyChanged("Variant");
            }
        }

        internal CoercedVariantPropertyModel() : base()
        {
            Variant = typeof(TVariant).TryCreate<TVariant>();
            Variant.Changed += OnVariantChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected override void OnValueChanged(object Value)
        {
            base.OnValueChanged(Value);
            if (!ValueChangeHandled)
                Variant.Set(Value.As<TPrimitive>());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnVariantChanged(object sender, EventArgs<TPrimitive> e)
        {
            ValueChangeHandled = true;
            Value = e.Value;
            ValueChangeHandled = false;
        }
    }
}
