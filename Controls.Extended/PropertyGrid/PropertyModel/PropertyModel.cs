using Imagin.Common;
using System;
using System.Reflection;

namespace Imagin.Controls.Extended
{
    public abstract class PropertyModel : AbstractObject
    {
        #region Properties

        string category = string.Empty;
        public string Category
        {
            get
            {
                return this.category;
            }
            set
            {
                this.category = value;
                OnPropertyChanged("Category");
            }
        }

        string description = string.Empty;
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
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
                this.OnInfoChanged(value);
            }
        }

        bool isFeatured = false;
        public bool IsFeatured
        {
            get
            {
                return this.isFeatured;
            }
            set
            {
                this.isFeatured = value;
                OnPropertyChanged("IsFeatured");
            }
        }

        bool isReadOnly = false;
        public bool IsReadOnly
        {
            get
            {
                return this.isReadOnly;
            }
            set
            {
                this.isReadOnly = value;
                OnPropertyChanged("IsReadOnly");
            }
        }

        string name = string.Empty;
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                OnPropertyChanged("Name");
            }
        }

        object value = null;
        public object Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
                this.OnValueChanged(value);
            }
        }

        object selectedObject = null;
        public object SelectedObject
        {
            get
            {
                return this.selectedObject;
            }
            set
            {
                this.selectedObject = value;
                OnPropertyChanged("Info");
                this.Info = value == null ? null : value.GetType().GetProperty(this.Name, BindingFlags.Public | BindingFlags.Instance);
            }
        }

        public string Type
        {
            get
            {
                return this.GetType().ToString();
            }
        }

        #endregion

        #region PropertyModel

        protected PropertyModel(string Name, object Value, string Category, string Description, bool IsReadOnly, bool IsFeatured) : base()
        {
            this.Name = Name;

            this.value = Value;
            this.OnPropertyChanged("Value");

            this.Category = Category;
            this.Description = Description;
            this.IsReadOnly = IsReadOnly;
            this.IsFeatured = IsFeatured;

            this.OnPropertyChanged("Type");
        }

        #endregion

        #region Methods

        protected virtual void OnValueChanged(object Value)
        {
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

        #endregion
    }
}
