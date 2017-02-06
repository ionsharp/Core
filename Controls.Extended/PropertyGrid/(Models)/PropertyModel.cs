using Imagin.Common;
using Imagin.Common.Attributes;
using Imagin.Common.Extensions;
using Imagin.Common.Primitives;
using System;
using System.Collections;
using System.ComponentModel;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    /// <summary>
    /// The base implementation to represent an object property.
    /// </summary>
    public abstract class PropertyModel : NamedObject, IDisposable
    {
        #region Properties

        bool disposed = false; 

        bool hostPropertyChangeHandled = false;

        bool valueChangeHandled = false;

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
            private set
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
            private set
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

                //If the property has a public setter, it is settable.
                IsSettable = value?.GetSetMethod(true) != null;
            }
        }

        bool isFeatured = false;
        /// <summary>
        /// Gets whether or not the property is featured.
        /// </summary>
        public bool IsFeatured
        {
            get
            {
                return isFeatured;
            }
            private set
            {
                isFeatured = value;
                OnPropertyChanged("IsFeatured");
            }
        }

        bool isReadOnly = false;
        /// <summary>
        /// Gets whether or not the property is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return isReadOnly;
            }
            private set
            {
                isReadOnly = value;
                OnPropertyChanged("IsReadOnly");
            }
        }

        bool isSettable = false;
        /// <summary>
        /// Gets whether or not the property has a public setter; if it doesn't, the property is automatically readonly.
        /// </summary>
        public bool IsSettable
        {
            get
            {
                return isSettable;
            }
            private set
            {
                isSettable = value;

                //If NOT settable, it is readonly; else, it is whatever was already specified.
                IsReadOnly = value ? isReadOnly : true;
            }
        }

        /// <summary>
        /// Gets the actual property type (note, if the property is nullable, this will be the underlying type).
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
        /// Gets the <see cref="System.String"/> representation of the property's type.
        /// </summary>
        /// <remarks>
        /// Used for sorting only.
        /// </remarks>
        public string Type
        {
            get
            {
                return Primitive.Name;
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
                //If the property is settable OR we're making an internal change...
                if (IsSettable || valueChangeHandled)
                {
                    _value = OnPreviewValueChanged(_value, value);
                    OnPropertyChanged("Value");
                    OnValueChanged(_value);
                }
            }
        }

        #endregion

        #region PropertyModel

        protected PropertyModel() : base()
        {
            OnPropertyChanged("Type");
        }

        /// <summary>
        /// 
        /// </summary>
        ~PropertyModel()
        {
            Dispose(false);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyModel"/> class based on given type. 
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        static PropertyModel New(Type Type)
        {
            //If type is nullable, get underlying type!
            if (Type.IsNullable())
                Type = Type.GetGenericArguments().WhereFirst(i => true);

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
                return new CoercedVariantPropertyModel<Proportions, Size>();

            if (Type == typeof(SolidColorBrush))
                return new PropertyModel<SolidColorBrush>();

            if (Type == typeof(string))
                return new PropertyModel<string>();

            if (Type == typeof(Uri))
                return new PropertyModel<Uri>();

            if (Type == typeof(Version))
                return new CoercedVariantPropertyModel<Release, Version>();

            if (Type == typeof(object))
                return new NestedPropertyModel();

            return null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyModel"/> class based on given data; some restrictions are observed.
        /// </summary>
        /// <param name="Host"></param>
        /// <param name="Property"></param>
        /// <param name="Attributes"></param>
        /// <returns></returns>
        internal static PropertyModel New(object Host, PropertyInfo Property, PropertyAttributes Attributes, bool IsNested)
        {
            var Result = New(IsNested ? typeof(object) : Property.PropertyType);

            if (Result != null)
            {
                Result.Info = Property;

                var Name = Attributes.Get<DisplayNameAttribute, string>();
                Name = Name.IsNullOrEmpty() ? Property.Name : Name;

                var Value = default(object);

                try
                {
                    //Will fail if locked
                    Value = Property.GetValue(Host);
                }
                catch (Exception)
                {
                    //Do nothing!
                }

                //Set the important stuff first
                Result.Host = Host;
                Result.Name = Name;
                Result.Value = Value;

                //Set the minor stuff
                Result.Category = Attributes.Get<CategoryAttribute, string>();
                Result.Description = Attributes.Get<DescriptionAttribute, string>();
                Result.StringFormat = Attributes.Get<StringFormatAttribute, string>();
                Result.IsFeatured = Attributes.Get<FeaturedAttribute, bool>();

                //Honor the attribute if the property is settable; if it isn't, it is automatically readonly and should always be!
                if (Result.IsSettable)
                    Result.IsReadOnly = Attributes.Get<ReadOnlyAttribute, bool>();

                if (Result is PropertyModel<string>)
                    Result.As<PropertyModel<string>>().Tag = Attributes.Get<StringKindAttribute, StringKind>();

                if (Result is PropertyModel<long>)
                    Result.As<PropertyModel<long>>().Tag = Attributes.Get<Int64KindAttribute, Int64Kind>();

                //Honor constraints
                if (Result is ICoercable)
                {
                    var Constraint = Attributes.Get<ConstraintAttribute, ConstraintAttribute>();

                    if (Constraint != null)
                        Result.As<ICoercable>().SetConstraint(Constraint.Minimum, Constraint.Maximum);
                }
            }

            return Result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyModel"/> class based on given type and values; no restrictions.
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
        internal static PropertyModel New(Type Type, object Host, string Name, object Value, string Category, string Description, string StringFormat, bool IsReadOnly, bool IsFeatured)
        {
            var Result = New(Type);

            if (Result != null)
            {
                Result.Host = Host;
                Result.Name = Name;
                Result.Value = Value;
                Result.Category = Category;
                Result.Description = Description;
                Result.StringFormat = StringFormat;
                Result.IsReadOnly = IsReadOnly;
                Result.IsFeatured = IsFeatured;
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
            }

            if (Host is INotifyPropertyChanged)
                Host.As<INotifyPropertyChanged>().PropertyChanged -= OnHostPropertyChanged;

            disposed = true;
        }

        /// <summary>
        /// Occurs when the host object changes.
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnHostChanged(object Value)
        {
            if (Value is INotifyPropertyChanged)
                Value.As<INotifyPropertyChanged>().PropertyChanged += OnHostPropertyChanged;
        }

        /// <summary>
        /// Occurs if the host object implements <see cref="INotifyPropertyChanged"/> and one of it's properties has changed; this is necessary to capture external changes while the object is a host.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnHostPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //If the property that changed is modelled by the current instance...
            if (!hostPropertyChangeHandled && e.PropertyName == Name && Info != null)
            {
                //Update the property of the current instance
                valueChangeHandled = true;
                //If the property is NOT settable, it is still okay to set here.
                Value = Info.GetValue(Host);
                valueChangeHandled = false;
            }
        }

        /// <summary>
        /// Occurs just before setting the value.
        /// </summary>
        /// <param name="OldValue">The original value.</param>
        /// <param name="NewValue">The new value.</param>
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
                if (Host.As<ResourceDictionary>().Contains(Name))
                    Host.As<ResourceDictionary>()[Name] = Value;
            }
            //If the property is NOT settable, this condition will (or should) always be false.
            else if (!valueChangeHandled && Info != null)
            {
                hostPropertyChangeHandled = true;
                Info.SetValue(Host, Value, null);
                hostPropertyChangeHandled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
