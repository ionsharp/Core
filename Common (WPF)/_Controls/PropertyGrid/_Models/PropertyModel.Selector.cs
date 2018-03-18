using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Common
{
    /// <summary>
    /// The base implementation for an object that represents a property.
    /// </summary>
    public abstract partial class PropertyModel : NamedObject
    {
        /// <summary>
        /// A utility for creating a <see cref="PropertyModel"/> based on various parameters.
        /// To support additional types, the type's model and the type must be defined in <see cref="Models"/> 
        /// and <see cref="Types"/>, respectively. Additionally, a <see cref="DataTemplate"/> corresponding 
        /// to the type must also be defined.
        /// </summary>
        internal static class Selector
        {
            /// <summary>
            /// Gets a list of each type's corresponding <see cref="PropertyModel"/>.
            /// </summary>
            static Dictionary<Type, Func<PropertyModel>> Models = new Dictionary<Type, Func<PropertyModel>>()
            {
                {typeof(Boolean),                   () => new PropertyModel<Boolean>()},
                {typeof(Byte),                      () => new CoercedPropertyModel<Byte>()},
                {typeof(System.Windows.Media.Color),() => new PropertyModel<System.Windows.Media.Color>()},
                {typeof(DateTime),                  () => new CoercedPropertyModel<DateTime>()},
                {typeof(Decimal),                   () => new CoercedPropertyModel<Decimal>()},
                {typeof(Double),                    () => new CoercedPropertyModel<Double>()},
                {typeof(Enum),                      () => new PropertyModel<Enum>()},
                {typeof(Guid),                      () => new PropertyModel<Guid>()},
                {typeof(IList),                     () => new PropertyModel<IList>()},
                {typeof(Int16),                     () => new CoercedPropertyModel<Int16>()},
                {typeof(Int32),                     () => new CoercedPropertyModel<Int32>()},
                {typeof(Int64),                     () => new CoercedPropertyModel<Int64>()},
                {typeof(LinearGradientBrush),       () => new PropertyModel<LinearGradientBrush>()},
                {typeof(NetworkCredential),         () => new PropertyModel<NetworkCredential>()},
                //{typeof(System.Drawing.Point),    () => },
                {typeof(System.Windows.Point),      () => new CoercedVariantPropertyModel<Point2D, System.Windows.Point>()},
                {typeof(RadialGradientBrush),       () => new PropertyModel<RadialGradientBrush>()},
                {typeof(Single),                    () => new CoercedPropertyModel<Single>()},
                {typeof(Size),                      () => new CoercedVariantPropertyModel<Proportions, Size>()},
                {typeof(SolidColorBrush),           () => new PropertyModel<SolidColorBrush>()},
                {typeof(String),                    () => new PropertyModel<String>()},
                {typeof(UInt16),                    () => new CoercedPropertyModel<UInt16>()},
                {typeof(UInt32),                    () => new CoercedPropertyModel<UInt32>()},
                {typeof(UInt64),                    () => new CoercedPropertyModel<UInt64>()},
                {typeof(Uri),                       () => new PropertyModel<Uri>()},
                {typeof(Version),                   () => new PropertyModel<Version>()},
                {typeof(object),                    () => new NestedPropertyModel()}
            };

            /// <summary>
            /// Gets a list of all types that can be modelled (excluding special types <see cref="Enum"/>, <see cref="IList"/>, and <see cref="object"/>).
            /// </summary>
            public static Type[] Types = new Type[]
            {
                typeof(Boolean),
                typeof(Byte),
                typeof(System.Windows.Media.Color),
                typeof(DateTime),
                typeof(Decimal),
                typeof(Double),
                typeof(Guid),
                typeof(Int16),
                typeof(Int32),
                typeof(Int64),
                typeof(LinearGradientBrush),
                typeof(NetworkCredential),
                typeof(System.Drawing.Point),
                typeof(System.Windows.Point),
                typeof(RadialGradientBrush),
                typeof(Single),
                typeof(Size),
                typeof(SolidColorBrush),
                typeof(String),
                typeof(UInt16),
                typeof(UInt32),
                typeof(UInt64),
                typeof(Uri),
                typeof(Version)
            };

            /// <summary>
            /// Assigns the given <see cref="PropertyAttributes"/> to the given <see cref="PropertyModel"/>.
            /// </summary>
            /// <param name="model"></param>
            /// <param name="info"></param>
            /// <param name="attributes"></param>
            static void AssignAttributes(PropertyModel model, PropertyInfo info, PropertyAttributes attributes)
            {
                var category = attributes.Get<Imagin.Common.CategoryAttribute>().Category;
                if (category.IsNullOrEmpty()) category = attributes.Get<System.ComponentModel.CategoryAttribute>().Category;
                model.Category = category;

                var description = attributes.Get<Imagin.Common.DescriptionAttribute>().Description;
                if (description.IsNullOrEmpty()) description = attributes.Get<System.ComponentModel.DescriptionAttribute>().Description;
                model.Description = description;

                var dname = attributes.Get<Imagin.Common.DisplayNameAttribute>().DisplayName;
                if (dname.IsNullOrEmpty()) dname = attributes.Get<System.ComponentModel.DisplayNameAttribute>().DisplayName;
                if (dname.IsNullOrEmpty()) dname = model.Name;
                model.DisplayName = dname;
                
                model.Format = model.Primitive == typeof(DateTime)  ? attributes.Get<DateFormatAttribute>().Format      : model.Format;
                model.Format = model.Primitive == typeof(Enum)      ? attributes.Get<EnumFormatAttribute>().Format      : model.Format;
                model.Format = model.Primitive == typeof(Int64)     ? attributes.Get<LongFormatAttribute>().Format      : model.Format;
                model.Format = model.Primitive == typeof(String)    ? attributes.Get<StringFormatAttribute>().Format    : model.Format;

                model.IsFeatured = attributes.Get<FeaturedAttribute>().IsFeatured;

                //If the property has a public setter, it is NOT read only.
                var ronly = !(info?.GetSetMethod(true) != null);
                ronly = !ronly ? attributes.Get<Imagin.Common.ReadOnlyAttribute>().IsReadOnly : ronly;
                ronly = !ronly ? attributes.Get<System.ComponentModel.ReadOnlyAttribute>().IsReadOnly : ronly;
                model.IsReadOnly = ronly;

                if (model is IRange)
                {
                    var constraint = attributes.Get<ConstraintAttribute>();
                    if (constraint != null) model.As<IRange>().SetRange(constraint.Minimum, constraint.Maximum);
                }
            }

            /// <summary>
            /// Gets a <see cref="PropertyModel"/> based on the given type.
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            static PropertyModel Select(Type type)
            {
                //If type is nullable, get underlying type!
                //type = type.IsNullable() ? type.GetGenericArguments().FirstOrDefault(i => true) : type;

                if (typeof(IList).IsAssignableFrom(type))
                    type = typeof(IList);

                if (type.IsEnum)
                    type = typeof(Enum);

                return Models.ContainsKey(type) ? Models[type]() : default(PropertyModel);
            }

            /// <summary>
            /// Gets a <see cref="PropertyModel"/> based on the given <see cref="object"/>.
            /// </summary>
            /// <param name="source"></param>
            /// <param name="info"></param>
            /// <param name="attributes"></param>
            /// <param name="type"></param>
            /// <returns></returns>
            public static PropertyModel Select(object source, PropertyInfo info, PropertyAttributes attributes, PropertyType type)
            {
                var result = default(PropertyModel);

                switch (type)
                {
                    case PropertyType.Default:
                        result = Select(info.PropertyType);
                        break;
                    case PropertyType.Host:
                        result = Select(typeof(object));
                        break;
                }

                //If the result is null, the type is defined, but not implemented in the creation method...
                if (result == null) return result;

                result.source = source;

                result.info = info;
                result.Name = info.Name;

                AssignAttributes(result, info, attributes);

                result.ValueChangeHandled = true;
                result.RefreshValue();
                result.ValueChangeHandled = false;

                return result;
            }

            /// <summary>
            /// Gets a <see cref="PropertyModel"/> based on the given parameters.
            /// </summary>
            /// <param name="Type"></param>
            /// <param name="Host"></param>
            /// <param name="Name"></param>
            /// <param name="Value"></param>
            /// <param name="Category"></param>
            /// <param name="Description"></param>
            /// <param name="StringFormat"></param>
            /// <param name="IsReadOnly"></param>
            /// <param name="IsFeatured"></param>
            /// <returns></returns>
            public static PropertyModel Select(Type Type, object Host, string Name, object Value, string Category, string Description, string StringFormat, bool IsReadOnly, bool IsFeatured)
            {
                var result = Select(Type);

                if (result == null) return result;

                result.source       = Host;
                result.Name         = Name;
                result.DisplayName  = Name;
                result.Value        = Value;
                result.Category     = Category;
                result.Description  = Description;
                result.StringFormat = StringFormat;
                result.IsReadOnly   = IsReadOnly;
                result.IsFeatured   = IsFeatured;

                return result;
            }
        }
    }
}
