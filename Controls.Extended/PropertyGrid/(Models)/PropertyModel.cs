using Imagin.Common;
using Imagin.Common.Attributes;
using Imagin.Common.Extensions;
using Imagin.Common.Primitives;
using System;
using System.Linq;
using System.Collections;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    /// <summary>
    /// The base implementation to represent an object property.
    /// </summary>
    public abstract class PropertyModel : NamedObject
    {
        /// <summary>
        /// A list of all supported types.
        /// </summary>
        public static Type[] SupportedTypes
        {
            get
            {
                return new Type[]
                {
                    typeof(bool),
                    typeof(byte),
                    typeof(DateTime),
                    typeof(decimal),
                    typeof(double),
                    typeof(Guid),
                    typeof(int),
                    typeof(long),
                    typeof(LinearGradientBrush),
                    typeof(NetworkCredential),
                    typeof(System.Drawing.Point),
                    typeof(System.Windows.Point),
                    typeof(RadialGradientBrush),
                    typeof(short),
                    typeof(Size),
                    typeof(SolidColorBrush),
                    typeof(string),
                    typeof(Uri),
                    typeof(Version)
                };
            }
        }

        string category = string.Empty;
        /// <summary>
        /// Gets or sets the property category.
        /// </summary>
        public string Category
        {
            get
            {
                return category;
            }
            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }

        string description = string.Empty;
        /// <summary>
        /// Gets or sets the property description.
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        object host = null;
        /// <summary>
        /// Gets or sets the object this property belongs to.
        /// </summary>
        public object Host
        {
            get
            {
                return host;
            }
            set
            {
                host = value;
                OnHostChanged(host);
                OnPropertyChanged("Host");
            }
        }

        PropertyInfo info = null;
        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> for the property.
        /// </summary>
        public PropertyInfo Info
        {
            get
            {
                return info;
            }
            private set
            {
                info = value;
                OnPropertyChanged("Info");
            }
        }

        bool isFeatured = false;
        /// <summary>
        /// Gets or sets whether or not the property is featured.
        /// </summary>
        public bool IsFeatured
        {
            get
            {
                return isFeatured;
            }
            set
            {
                isFeatured = value;
                OnPropertyChanged("IsFeatured");
            }
        }

        bool isReadOnly = false;
        /// <summary>
        /// Gets or sets whether or not the property is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return isReadOnly;
            }
            set
            {
                isReadOnly = value;
                OnPropertyChanged("IsReadOnly");
            }
        }

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        public abstract Type Primitive
        {
            get;
        }

        string stringFormat = string.Empty;
        /// <summary>
        /// Gets or sets the string format for the property.
        /// </summary>
        public string StringFormat
        {
            get
            {
                return stringFormat;
            }
            set
            {
                stringFormat = value;
                OnPropertyChanged("StringFormat");
            }
        }

        object tag = null;
        /// <summary>
        /// An object for general use.
        /// </summary>
        public object Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
                OnPropertyChanged("Tag");
            }
        }

        /// <summary>
        /// Gets the string representation of the property's type.
        /// </summary>
        /// <remarks>
        /// Used for sorting only.
        /// </remarks>
        public string Type
        {
            get
            {
                return Primitive.ToString();
            }
        }

        object _value = null;
        /// <summary>
        /// Gets or sets the current value for the property.
        /// </summary>
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = OnPreviewValueChanged(_value, value);
                OnPropertyChanged("Value");
                OnValueChanged(_value);
            }
        }

        internal PropertyModel() : base()
        {
            OnPropertyChanged("Type");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyModel"/> class based on given type. 
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        static PropertyModel New(Type Type)
        {
            if (Type == typeof(bool))
                return new PropertyModel<bool>();

            if (Type == typeof(byte))
                return new CoercedPropertyModel<byte>();

            if (typeof(IList).IsAssignableFrom(Type))
                return new PropertyModel<IList>();

            if (Type == typeof(DateTime))
                return new CoercedPropertyModel<DateTime>();

            if (Type == typeof(decimal))
                return new CoercedPropertyModel<decimal>();

            if (Type == typeof(double))
                return new CoercedPropertyModel<double>();

            if (Type.IsEnum)
                return new PropertyModel<Enum>();

            if (Type == typeof(Guid))
                return new PropertyModel<Guid>();

            if (Type == typeof(int))
                return new CoercedPropertyModel<int>();

            if (Type == typeof(long))
                return new CoercedPropertyModel<long>();

            if (Type == typeof(LinearGradientBrush))
                return new PropertyModel<Brush>();

            if (Type == typeof(NetworkCredential))
                return new PropertyModel<NetworkCredential>();

            if (Type == typeof(System.Windows.Point))
                return new CoercedVariantPropertyModel<Position, System.Windows.Point>();

            if (Type == typeof(RadialGradientBrush))
                return new PropertyModel<Brush>();

            if (Type == typeof(short))
                return new CoercedPropertyModel<short>();

            if (Type == typeof(Size))
                return new CoercedVariantPropertyModel<Dimensions, Size>();

            if (Type == typeof(SolidColorBrush))
                return new PropertyModel<SolidColorBrush>();

            if (Type == typeof(string))
                return new PropertyModel<string>();

            if (Type == typeof(Uri))
                return new PropertyModel<Uri>();

            if (Type == typeof(Version))
                return new PropertyModel<Version>();

            return null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyModel"/> class based on given object, property, and attributes.
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="Property"></param>
        /// <param name="Attributes"></param>
        /// <returns></returns>
        internal static PropertyModel New(object Host, PropertyInfo Property, PropertyAttributes Attributes)
        {
            var Result = PropertyModel.New(Property.PropertyType);

            if (Result != null)
            {
                Result.Info = Property;

                Result.Set(Host, Property.Name, Property.GetValue(Host), Attributes.Get<string>("Category", false), Attributes.Get<string>("Description", false), Attributes.Get<string>("DisplayFormat", false), Attributes.Get<bool>("ReadOnly", false), Attributes.Get<bool>("Featured", false));

                if (Result is PropertyModel<string>)
                    Result.As<PropertyModel<string>>().Tag = Attributes.Get<StringKind>("StringKind", false);

                if (Result is PropertyModel<long>)
                    Result.As<PropertyModel<long>>().Tag = Attributes.Get<Int64Kind>("Int64Kind", false);

                if (Result is ICoercable)
                {
                    var Constraint = Attributes.Get<ConstraintAttribute>("Constraint", false);

                    if (Constraint != null)
                        Result.As<ICoercable>().SetConstraint(Constraint.Minimum, Constraint.Maximum);
                }
            }

            return Result;
        }

        internal static PropertyModel New(Type Type, object host, string name, object value, string category, string description, string stringFormat, bool isReadOnly, bool isFeatured)
        {
            var Result = PropertyModel.New(Type);

            if (Result != null)
                Result.Set(host, name, value, category, description, stringFormat, isReadOnly, isFeatured);

            return Result;
        }

        internal void Set(object host, string name, object value, string category, string description, string stringFormat, bool isReadOnly, bool isFeatured)
        {
            Host = host;
            Name = name;
            Value = value;
            Category = category;
            Description = description;
            StringFormat = stringFormat;
            IsReadOnly = isReadOnly;
            IsFeatured = isFeatured;
        }

        /// <summary>
        /// Occurs when the host object changes.
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnHostChanged(object Value)
        {
        }

        /// <summary>
        /// Occurs just before setting the value.
        /// </summary>
        /// <param name="Value">The original value.</param>
        /// <returns>The actual value to set for the property.</returns>
        protected virtual object OnPreviewValueChanged(object OldValue, object NewValue)
        {
            return NewValue == null ? default(object) : NewValue;
        }

        /// <summary>
        /// Occurs when the value changes.
        /// </summary>
        /// <param name="Value">The new value.</param>
        protected virtual void OnValueChanged(object Value)
        {
            if (Host is ResourceDictionary)
            {
                Console.WriteLine("OnValueChanged => Host is ResourceDictionary");
                if (Host.As<ResourceDictionary>().Contains(Name))
                {
                    Console.WriteLine("Host.As<ResourceDictionary>().Contains(Name)");
                    Host.As<ResourceDictionary>()[Name] = Value;
                }
            }
            else if (Info != null)
            {
                Info.SetValue(Host, Value, null);
            }
        }
    }
}
