using Imagin.Common;
using Imagin.Common.Globalization;
using Imagin.Common.Linq;
using System;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// The base implementation for an object that represents a property.
    /// </summary>
    public abstract partial class PropertyModel : NamedObject
    {
        #region Fields

        /// <summary>
        /// Gets or sets the object this property belongs to.
        /// </summary>
        object source = null;

        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> for the property.
        /// </summary>
        PropertyInfo info = null;

        /// <summary>
        /// 
        /// </summary>
        internal bool HostPropertyChangeHandled = false;

        /// <summary>
        /// 
        /// </summary>
        internal bool ValueChangeHandled = false;

        #endregion

        #region Properties

        string category = string.Empty;
        /// <summary>
        /// Gets or sets the property category.
        /// </summary>
        public string Category
        {
            get => category;
            private set
            {
                SetValue(ref category, value, () => Category);

                var localizedCategory = Localizer.GetValue<string>(category);
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
            get => description;
            private set => SetValue(ref description, value, () => Description);
        }

        string displayName = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string DisplayName
        {
            get => displayName;
            private set => SetValue(ref displayName, value, () => DisplayName);
        }

        object format = default(object);
        /// <summary>
        /// 
        /// </summary>
        public object Format
        {
            get => format;
            set => SetValue(ref format, value, () => Format);
        }

        bool isFeatured = false;
        /// <summary>
        /// Gets whether or not the property is featured.
        /// </summary>
        public bool IsFeatured
        {
            get => isFeatured;
            private set => SetValue(ref isFeatured, value, () => IsFeatured);
        }

        bool isEnabled = true;
        /// <summary>
        /// Gets whether or not the property is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get => isEnabled;
            set => SetValue(ref isEnabled, value, () => IsEnabled);
        }

        bool isReadOnly = false;
        /// <summary>
        /// Gets whether or not the property is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get => isReadOnly;
            private set => SetValue(ref isReadOnly, value, () => IsReadOnly);
        }

        string localizedCategory = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string LocalizedCategory
        {
            get => localizedCategory;
            set => SetValue(ref localizedCategory, value, () => LocalizedCategory);
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
            get => stringFormat;
            set => SetValue(ref stringFormat, value, () => StringFormat);
        }

        object tag = default(object);
        /// <summary>
        /// An object for general use.
        /// </summary>
        public object Tag
        {
            get => tag;
            set => SetValue(ref tag, value, () => Tag);
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

        object value = default(object);
        /// <summary>
        /// Gets or sets the current value for the property.
        /// </summary>
        public object Value
        {
            get => value;
            set
            {
                //If the property is NOT read only OR we're making an internal change...
                if (!isReadOnly || ValueChangeHandled)
                {
                    this.value = OnPreviewValueChanged(this.value, value);
                    OnPropertyChanged("Value");
                    OnValueChanged(value);
                }
            }
        }

        #endregion

        #region PropertyModel

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyModel"/> class.
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
            return default(object);
        }

        void SetValue(object source, object value)
        {
            info?.SetValue(source, value, null);
        }

        /// <summary>
        /// Occurs before <see cref="Value"/> changes.
        /// </summary>
        /// <param name="OldValue">The old value.</param>
        /// <param name="NewValue">The new value.</param>
        /// <returns>The value to assign.</returns>
        protected virtual object OnPreviewValueChanged(object OldValue, object NewValue)
        {
            return NewValue ?? default(object);
        }

        /// <summary>
        /// Occurs when <see cref="Value"/> changes.
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