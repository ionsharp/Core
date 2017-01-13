using Imagin.Common;
using Imagin.Common.Extensions;
using Imagin.Common.Input;
using System;

namespace Imagin.Controls.Extended
{
    /// <summary>
    /// Represents a property with a type that uses another type to modify it, and has both a minimum and maximum value.
    /// </summary>
    /// <typeparam name="T1">The type used to modify the property.</typeparam>
    /// <typeparam name="T2">The actual type of the property.</typeparam>
    public class CoercedVariantPropertyModel<T1, T2> : CoercedPropertyModel<T2> where T1 : IVariant<T2>
    {
        bool ValueChangeHandled = false;

        T1 variant = default(T1);
        public T1 Variant
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
            Variant = typeof(T1).TryCreate<T1>();
            Variant.Changed += OnVariantChanged;
        }

        protected override void OnValueChanged(object Value)
        {
            base.OnValueChanged(Value);
            if (!ValueChangeHandled)
                Variant.Set(Value.As<T2>());
        }

        protected virtual void OnVariantChanged(object sender, EventArgs<T2> e)
        {
            ValueChangeHandled = true;
            Value = e.Value;
            ValueChangeHandled = false;
        }
    }
}
