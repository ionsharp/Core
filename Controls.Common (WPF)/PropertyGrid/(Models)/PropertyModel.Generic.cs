using Imagin.Common.Linq;
using System;
using System.Linq;

namespace Imagin.Controls.Common
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
        /// Gets the underlying property type.
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
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        /// <returns></returns>
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
                    case "byte":
                        Result = NewValue.ToString().ToByte();
                        break;
                    case "decimal":
                        Result = NewValue.ToString().ToDecimal();
                        break;
                    case "double":
                        Result = NewValue.ToString().ToDouble();
                        break;
                    case "guid":
                        var g = default(Guid);
                        Result = Guid.TryParse(NewValue.ToString(), out g) ? g : new Guid(OldValue.ToString());
                        break;
                    case "int16":
                        Result = NewValue.ToString().ToInt16();
                        break;
                    case "int32":
                        Result = NewValue.ToString().ToInt32();
                        break;
                    case "int64":
                        Result = NewValue.ToString().ToInt64();
                        break;
                    case "uint16":
                        Result = NewValue.ToString().ToUInt16();
                        break;
                    case "uint32":
                        Result = NewValue.ToString().ToUInt32();
                        break;
                    case "uint64":
                        Result = NewValue.ToString().ToUInt64();
                        break;
                    case "uri":
                        var u = default(Uri);
                        Result = Uri.TryCreate(NewValue.ToString(), UriKind.Absolute, out u) ? u : new Uri(OldValue.ToString());
                        break;
                    case "version":
                        var v = default(Version);
                        Result = Version.TryParse(NewValue.ToString(), out v) ? v : new Version(OldValue.ToString());
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
