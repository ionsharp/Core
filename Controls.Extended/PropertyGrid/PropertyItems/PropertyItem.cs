using System;
using System.Reflection;
using Imagin.Common;
using Imagin.Common.Events;

namespace Imagin.Controls.Extended
{
    public enum PropertyType
    {
        String,
        MultiLine,
        Password,
        FileSystemObject,
        Int,
        Double,
        Enum,
        Bool,
        Guid,
        LinearGradientBrush,
        SolidColorBrush,
        DateTime,
        Short,
        Decimal,
        Byte,
        Long
    }

    public abstract class PropertyItem : AbstractObject
    {
        #region Members

        public event EventHandler<EventArgs<object>> ValueChanged;
        
        private bool isFeatured = false;
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

        private bool isReadOnly = false;
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

        private string category = string.Empty;
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

        private PropertyType type;
        public PropertyType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
                OnPropertyChanged("Type");
            }
        }

        private string name = string.Empty;
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

        private object value = null;
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
                if (this.ValueChanged != null)
                    this.ValueChanged(this, new EventArgs<object>(value));
            }
        }

        private object selectedObject = null;
        public object SelectedObject
        {
            get
            {
                return this.selectedObject;
            }
            set
            {
                this.selectedObject = value;
                OnPropertyChanged("SelectedObject");
            }
        }

        private PropertyInfo info = null;
        public PropertyInfo Info
        {
            get
            {
                return this.info;
            }
            set
            {
                this.info = value;
                OnPropertyChanged("Info");
            }
        }

        #endregion

        #region PropertyItem

        public PropertyItem(object SelectedObject, string Name, object Value, string Category, bool IsReadOnly, bool IsFeatured = false)
        {
            this.SelectedObject = SelectedObject;
            this.Name = Name;
            this.Value = Value;
            this.ValueChanged += OnValueChanged;
            if (this.SelectedObject != null)
                this.Info = this.SelectedObject.GetType().GetProperty(this.Name, BindingFlags.Public | BindingFlags.Instance);
            this.Category = Category;
            this.IsReadOnly = IsReadOnly;
            this.IsFeatured = IsFeatured;
        }

        #endregion

        #region Virtual Methods

        public virtual void SetValue(object NewValue)
        {
        }

        #endregion

        #region Events

        private void OnValueChanged(object sender, EventArgs<object> e)
        {
            this.SetValue(e.Value);
        }

        #endregion
    }
}
