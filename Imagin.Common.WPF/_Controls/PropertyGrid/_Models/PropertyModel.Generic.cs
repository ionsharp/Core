using Imagin.Common.Linq;
using System;
using System.Linq;

namespace Imagin.Common
{
    /// <summary>
    /// Represents an object property with specific type.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    public class PropertyModel<T> : PropertyModel
    {
        /// <summary>
        /// Gets the default value of the property.
        /// </summary>
        protected virtual T Default
        {
            get
            {
                return default(T);
            }
        }

        /// <summary>
        /// Gets the underlying property type (if the property is <see cref="Nullable"/>, this will be the underlying type).
        /// </summary>
        public override Type Primitive
        {
            get
            {
                if (typeof(T).IsNullable())
                    return typeof(T).GetGenericArguments().FirstOrDefault<Type>();

                return typeof(T);
            }
        }

        internal PropertyModel() : base()
        {
        }

        /// <summary>
        /// Occurs before <see cref="PropertyModel.Value"/> changes.
        /// </summary>
        /// <param name="OldValue">The old value.</param>
        /// <param name="NewValue">The new value.</param>
        /// <returns>The value to assign.</returns>
        protected override object OnPreviewValueChanged(object OldValue, object NewValue)
        {
            if (NewValue == null)
            {
                return Default;
            }
            else
            {
                var Result = default(object);
                switch (typeof(T).Name.ToString().ToLower())
                {
                    case "guid":
                        var g = default(Guid);
                        Result = Guid.TryParse(NewValue.ToString(), out g) ? g : new Guid(OldValue.ToString());
                        break;
                    default:
                        Result = (T)NewValue;
                        break;
                }
                return Result.To<T>();
            }
        }
    }
}
