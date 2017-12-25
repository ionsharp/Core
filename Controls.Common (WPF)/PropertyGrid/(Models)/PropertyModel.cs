using Imagin.Common;
using Imagin.Common.Data;
using Imagin.Common.Globalization;
using Imagin.Common.Linq;
using Imagin.Common.Primitives;
using System;
using System.Collections;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// The base implementation to represent an object property.
    /// </summary>
    public abstract class PropertyModel : NamedObject
    {
        #region Properties

        #region Internal

        /// <summary>
        /// 
        /// </summary>
        internal bool HostPropertyChangeHandled = false;

        /// <summary>
        /// 
        /// </summary>
        internal bool ValueChangeHandled = false;

        #endregion

        #region Private

        /// <summary>
        /// Gets or sets the object this property belongs to.
        /// </summary>
        object source = null;

        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> for the property.
        /// </summary>
        PropertyInfo info = null;

        #endregion

        #region Public

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
                    typeof(uint),
                    typeof(ulong),
                    typeof(ushort),
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

                var localizedCategory = LocalizationProvider.GetValue<string>(category);
                localizedCategory = localizedCategory.IsNullOrEmpty() ? category : localizedCategory;
                LocalizedCategory = localizedCategory;
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

        string displayName = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string DisplayName
        {
            get
            {
                return displayName;
            }
            private set
            {
                displayName = value;
                OnPropertyChanged("DisplayName");
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

        bool isEnabled = true;
        /// <summary>
        /// Gets whether or not the property is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");
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

        string localizedCategory = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string LocalizedCategory
        {
            get
            {
                return localizedCategory;
            }
            set
            {
                localizedCategory = value;
                OnPropertyChanged("LocalizedCategory");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                name = value;
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
                //If the property is NOT read only OR we're making an internal change...
                if (!isReadOnly || ValueChangeHandled)
                {
                    _value = OnPreviewValueChanged(_value, value);
                    OnPropertyChanged("Value");
                    OnValueChanged(_value);
                }
            }
        }

        #endregion

        #endregion

        #region PropertyModel

        /// <summary>
        /// 
        /// </summary>
        protected PropertyModel() : base()
        {
            OnPropertyChanged("Primitive");
            OnPropertyChanged("Type");
        }

        #endregion

        #region Methods

        object GetValue()
        {
            try
            {
                //Will fail if locked
                return info?.GetValue(source);
            }
            catch (Exception)
            {
                //Do nothing!
            }
            return null;
        }

        void SetValue(object source, object value)
        {
            info?.SetValue(source, value, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyModel"/> class based on given type. 
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        static PropertyModel New(Type Type)
        {
            //If type is nullable, get underlying type!
            /*
            if (Type.IsNullable())
                Type = Type.GetGenericArguments().WhereFirst(i => true);
            */

            if (Type == typeof(bool))
                return new PropertyModel<bool>();

            if (Type == typeof(byte))
                return new CoercedPropertyModel<byte>();

            if (typeof(IList).IsAssignableFrom(Type))
                return new PropertyModel<IList>();

            if (Type == typeof(DateTime))
                return new CoercedPropertyModel<DateTime>();

            if (Type == typeof(DateTime?))
                return new CoercedPropertyModel<DateTime?>();

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

            if (Type == typeof(uint))
                return new CoercedPropertyModel<uint>();

            if (Type == typeof(ulong))
                return new CoercedPropertyModel<ulong>();

            if (Type == typeof(ushort))
                return new CoercedPropertyModel<ushort>();

            if (Type == typeof(Uri))
                return new PropertyModel<Uri>();

            if (Type == typeof(Version))
                return new PropertyModel<Version>();

            if (Type == typeof(object))
                return new NestedPropertyModel();

            return null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyModel"/> class based on given data; some restrictions are observed.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyInfo"></param>
        /// <param name="propertyAttributes"></param>
        /// <param name="isNested"></param>
        /// <returns></returns>
        internal static PropertyModel New(object source, PropertyInfo propertyInfo, PropertyAttributes propertyAttributes, bool isNested)
        {
            var result = New(isNested ? typeof(object) : propertyInfo.PropertyType);

            if (result != null)
            {
                result.source = source;
                result.info = propertyInfo;
                result.Name = propertyInfo.Name;

                var Category = propertyAttributes.Get<Imagin.Common.CategoryAttribute>().Category;
                if (Category.IsNullOrEmpty())
                    Category = propertyAttributes.Get<System.ComponentModel.CategoryAttribute>().Category;

                var Description = propertyAttributes.Get<Imagin.Common.DescriptionAttribute>().Description;
                if (Description.IsNullOrEmpty())
                    Description = propertyAttributes.Get<System.ComponentModel.DescriptionAttribute>().Description;

                //If the property has a public setter, it is NOT read only.
                var IsReadOnly = !(propertyInfo?.GetSetMethod(true) != null);
                if (!IsReadOnly)
                    IsReadOnly = propertyAttributes.Get<Imagin.Common.ReadOnlyAttribute>().IsReadOnly;
                if (!IsReadOnly)
                    IsReadOnly = propertyAttributes.Get<System.ComponentModel.ReadOnlyAttribute>().IsReadOnly;

                var DisplayName = propertyAttributes.Get<Imagin.Common.DisplayNameAttribute>().DisplayName;
                if (DisplayName.IsNullOrEmpty())
                    DisplayName = propertyAttributes.Get<System.ComponentModel.DisplayNameAttribute>().DisplayName;
                if (DisplayName.IsNullOrEmpty())
                    DisplayName = result.name;

                result.Category = Category;
                result.Description = Description;
                result.DisplayName = DisplayName;
                result.IsFeatured = propertyAttributes.Get<FeaturedAttribute>().IsFeatured;
                result.IsReadOnly = IsReadOnly;

                if (result.Primitive == typeof(DateTime))
                    result.Tag = propertyAttributes.Get<DateFormatAttribute>().Format;

                if (result.Primitive == typeof(Enum))
                    result.Tag = propertyAttributes.Get<EnumFormatAttribute>().Format;

                if (result.Primitive == typeof(long))
                    result.Tag = propertyAttributes.Get<LongFormatAttribute>().Format;

                if (result.Primitive == typeof(string))
                    result.Tag = propertyAttributes.Get<StringFormatAttribute>().Format?.As<StringFormat>() ?? Imagin.Common.Data.StringFormat.Default;

                if (result is ICoercable)
                {
                    var constraint = propertyAttributes.Get<ConstraintAttribute>();

                    if (constraint != null)
                        result.As<ICoercable>().SetConstraint(constraint.Minimum, constraint.Maximum);
                }

                result.ValueChangeHandled = true;
                result.RefreshValue();
                result.ValueChangeHandled = false;
            }

            return result;
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
                Result.source = Host;
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
            if (source is ResourceDictionary)
            {
                var dictonary = source as ResourceDictionary;
                if (dictonary.Contains(Name))
                    dictonary[Name] = Value;
            }
            //If the property is NOT settable, this condition will (or should) always be false.
            else if (!ValueChangeHandled && info != null)
            {
                HostPropertyChangeHandled = true;
                SetValue(source, Value);
                HostPropertyChangeHandled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RefreshValue()
        {
            Value = GetValue();
        }

        #endregion
    }
}
