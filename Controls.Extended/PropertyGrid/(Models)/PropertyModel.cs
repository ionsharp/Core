using Imagin.Common;
using Imagin.Common.Attributes;
using Imagin.Common.Extensions;
using Imagin.Common.Text;
using System;
using System.Collections;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public abstract class PropertyModel : NamedObject
    {
        #region Properties

        string category = string.Empty;
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

        PropertyInfo info = null;
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
                OnInfoChanged(value);
            }
        }

        bool isFeatured = false;
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

        object value_ = null;
        public object Value
        {
            get
            {
                return value_;
            }
            set
            {
                value_ = OnPreviewValueChanged(value);
                OnPropertyChanged("Value");
                OnPropertyChanged("ValueType");
                OnValueChanged(value);
            }
        }

        public Type ValueType
        {
            get
            {
                return value_.GetType();
            }
        }

        object object_ = null;
        public object Object
        {
            get
            {
                return object_;
            }
            set
            {
                object_ = value;
                OnObjectChanged(value);
                OnPropertyChanged("Object");
            }
        }

        public string Type
        {
            get
            {
                return GetType().ToString();
            }
        }

        #endregion

        #region PropertyModel

        protected PropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base()
        {
            this.Name = Name;
            this.Value = Value;
            this.Category = Category;
            this.Description = Description;
            this.IsReadOnly = IsReadOnly;
            this.IsFeatured = IsFeatured;

            OnPropertyChanged("Type");
        }

        #endregion

        #region Methods
        
        protected virtual object OnPreviewValueChanged(object Value)
        {
            return Value;
        }

        protected virtual void OnObjectChanged(object Value)
        {
            Info = Value.NullOr<PropertyInfo>(Value.GetType().GetProperty(Name, BindingFlags.Public | BindingFlags.Instance));
        }

        protected virtual void OnValueChanged(object Value)
        {
            if (Info != null)
                Info.SetValue(Object, Value, null);
        }

        protected virtual void OnInfoChanged(PropertyInfo Value)
        {
        }

        public static T New<T>(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) where T : PropertyModel
        {
            return (T)PropertyModel.New(typeof(T), Name, Value, Category, Description, IsReadOnly, IsFeatured);
        }

        public static PropertyModel New(Type Type, string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured)
        {
            if (Type == typeof(PropertyModel))
                throw new InvalidCastException("Cannot create instance of abstract class 'PropertyModel.'");
            if (!Type.IsSubclassOf(typeof(PropertyModel)))
                throw new InvalidCastException("Specified type must inherit 'PropertyModel' type.");
            return (PropertyModel)Activator.CreateInstance(Type, Name, Value, Category, Description, IsReadOnly, IsFeatured);
        }

        public static PropertyModel New(object Object, PropertyInfo Property, PropertyAttributes Attributes)
        {
            var Result = PropertyModel.New(PropertyModel.GetType(Property.PropertyType), Property.Name, Property.GetValue(Object), Attributes["Category", false].ToString(), Attributes["Description", false].ToString(), Attributes["ReadOnly", false].To<bool>(), Attributes["Featured", false].To<bool>());

            if (Result is StringPropertyModel)
                Result.As<StringPropertyModel>().Representation = Attributes["StringRepresentation", false].To<StringRepresentation>();
            else if (Result is NumericPropertyModel && Attributes["Constraint", false] != null)
                Result.As<NumericPropertyModel>().SetConstraint(Attributes["Constraint", false].To<ConstraintAttribute>().Minimum, Attributes["Constraint", false].To<ConstraintAttribute>().Maximum);
            else if (Result is LongPropertyModel)
                Result.As<LongPropertyModel>().Int64Representation = Attributes["Int64Representation", false].To<Int64Representation>();

            Result.Object = Object;

            return Result;
        }

        public static Type GetType(Type Type)
        {
            if (Type == typeof(bool))
                return typeof(BoolPropertyModel);
            if (Type == typeof(byte))
                return typeof(BytePropertyModel);
            if (typeof(IList).IsAssignableFrom(Type))
                return typeof(CollectionPropertyModel);
            if (Type == typeof(DateTime))
                return typeof(DateTimePropertyModel);
            if (Type == typeof(decimal))
                return typeof(DecimalPropertyModel);
            if (Type == typeof(double))
                return typeof(DoublePropertyModel);
            if (Type.IsEnum)
                return typeof(EnumPropertyModel);
            if (Type == typeof(Guid))
                return typeof(GuidPropertyModel);
            if (Type == typeof(int))
                return typeof(IntPropertyModel);
            if (Type == typeof(long))
                return typeof(LongPropertyModel);
            if (Type == typeof(LinearGradientBrush))
                return typeof(LinearGradientPropertyModel);
            if (Type == typeof(NetworkCredential))
                return typeof(NetworkCredentialPropertyModel);
            if (Type == typeof(short))
                return typeof(ShortPropertyModel);
            if (Type == typeof(Size))
                return typeof(SizePropertyModel);
            if (Type == typeof(SolidColorBrush))
                return typeof(SolidColorBrushPropertyModel);
            if (Type == typeof(string))
                return typeof(StringPropertyModel);
            if (Type == typeof(Uri))
                return typeof(UriPropertyModel);
            if (Type == typeof(Version))
                return typeof(VersionPropertyModel);
            return default(Type);
    }
    #endregion
    }
}
