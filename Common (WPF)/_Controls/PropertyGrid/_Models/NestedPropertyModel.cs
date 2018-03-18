using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class NestedPropertyModel : PropertyModel
    {
        /// <summary>
        /// Gets the underlying property type (if the property is <see cref="Nullable"/>, this will be the underlying type).
        /// </summary>
        public override Type Primitive
        {
            get
            {
                return typeof(object);
            }
        }

        internal NestedPropertyModel() : base()
        {
        }

        /// <summary>
        /// Occurs when the value changes.
        /// </summary>
        /// <param name="Value">The new value.</param>
        protected override void OnValueChanged(object Value)
        {
            //Do nothing!
        }
    }
}
