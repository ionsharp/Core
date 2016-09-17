using Imagin.Common.Attributes;
using Imagin.Common.Collections;
using Imagin.Common.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public sealed class PropertyItemCollection : ConcurrentObservableCollection<PropertyItem>
    {
        #region Properties

        /// <summary>
        /// The object to evaluate.
        /// </summary>
        public object Object = null;

        PropertyItem featured = null;
        /// <summary>
        /// The featured property. This property is placed above all other properties.
        /// </summary>
        public PropertyItem Featured
        {
            get
            {
                return this.featured;
            }
            set
            {
                this.featured = value;
                OnPropertyChanged("Featured");
            }
        }

        #endregion

        #region PropertyItemCollection

        public PropertyItemCollection() : base()
        {
            this.ItemAdded += OnItemAdded;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a PropertyItem based on given arguments.
        /// </summary>
        PropertyItem GetPropertyItem(object Object, PropertyInfo Property, Dictionary<string, object> Attributes)
        {
            PropertyItem PropertyItem = null;
            string Name = Property.Name;
            object Value = Property.GetValue(Object);
            string Category = (string)Attributes["Category"];
            bool IsReadOnly = (bool)Attributes["IsReadOnly"];
            bool IsFeatured = (bool)Attributes["IsFeatured"];
            if (Property.PropertyType == typeof(string))
            {
                if ((bool)Attributes["IsPassword"])
                    PropertyItem = new PasswordPropertyItem(Object, Name, Value, Category, IsReadOnly, IsFeatured);
                else if ((bool)Attributes["IsFile"])
                    PropertyItem = new FileSystemObjectPropertyItem(Object, Name, Value, Category, IsReadOnly, IsFeatured);
                else if ((bool)Attributes["IsMultiLine"])
                    PropertyItem = new MultiLinePropertyItem(Object, Name, Value, Category, IsReadOnly, IsFeatured);
                else
                    PropertyItem = new StringPropertyItem(Object, Name, Value, Category, IsReadOnly, IsFeatured);
            }
            else if (Property.PropertyType == typeof(int))
                PropertyItem = new IntPropertyItem(Object, Name, Value, Category, IsReadOnly, IsFeatured);
            else if (Property.PropertyType == typeof(short))
                PropertyItem = new ShortPropertyItem(Object, Name, Value, Category, IsReadOnly, IsFeatured);
            else if (Property.PropertyType == typeof(byte))
                PropertyItem = new BytePropertyItem(Object, Name, Value, Category, IsReadOnly, IsFeatured);
            else if (Property.PropertyType == typeof(decimal))
                PropertyItem = new DecimalPropertyItem(Object, Name, Value, Category, IsReadOnly, IsFeatured);
            else if (Property.PropertyType == typeof(double))
                PropertyItem = new DoublePropertyItem(Object, Name, Value, Category, IsReadOnly, IsFeatured);
            else if (Property.PropertyType.IsEnum)
                PropertyItem = new EnumPropertyItem(Object, Property, Name, Value, Category, IsReadOnly, IsFeatured);
            else if (Property.PropertyType == typeof(bool))
                PropertyItem = new BoolPropertyItem(Object, Name, Value, Category, IsReadOnly, IsFeatured);
            else if (Property.PropertyType == typeof(DateTime))
                PropertyItem = new DateTimePropertyItem(Object, Name, Value, Category, IsReadOnly, IsFeatured);
            else if (Property.PropertyType == typeof(long))
                PropertyItem = new LongPropertyItem(Object, Name, Value, Category, IsReadOnly, IsFeatured);
            return PropertyItem;
        }

        void OnItemAdded(object sender, Imagin.Common.Collections.Events.ItemAddedEventArgs<PropertyItem> e)
        {
            if (e.NewItem.IsFeatured)
                this.Featured = e.NewItem;
        }

        #region Public

        /// <summary>
        /// Set properties by enumerating a resource dictionary.
        /// </summary>
        /// <param name="Dictionary">The dictionary to enumerate.</param>
        /// <param name="Callback">What to do afterwards.</param>
        public async void BeginFromResourceDictionary(ResourceDictionary Dictionary, Action Callback = null)
        {
            await Task.Run(new Action(() =>
            {
                foreach (DictionaryEntry Entry in Dictionary)
                {
                    if (Entry.Value == null)
                        continue;
                    if (Entry.Value.Is<LinearGradientBrush>())
                        this.Add(new LinearGradientPropertyItem(this.Object, Entry.Key.ToString(), Entry.Value, "Gradients", false));
                    else if (Entry.Value.Is<SolidColorBrush>())
                        this.Add(new SolidColorBrushPropertyItem(this.Object, Entry.Key.ToString(), Entry.Value, "Brushes", false));
                }
            }));
            if (Callback != null)
                Callback.Invoke();
        }

        /// <summary>
        /// Set properties by enumerating the properties of an object.
        /// </summary>
        /// <param name="Callback">What to do afterwards.</param>
        public async void BeginFromObject(Action Callback = null)
        {
            object Object = this.Object;
            await Task.Run(new Action(() =>
            {
                PropertyInfo[] ObjectProperties = Object.GetType().GetProperties();
                ObjectProperties = ObjectProperties.OrderBy(x => x.Name).ToArray();

                for (int i = 0, Length = ObjectProperties.Length; i < Length; i++)
                {
                    bool Skip = false;

                    PropertyInfo Property = ObjectProperties[i];
                    if (!(Property.CanWrite && Property.GetSetMethod(true).IsPublic))
                        continue;

                    Dictionary<string, object> FoundAttributes = new Dictionary<string, object>();
                    FoundAttributes.Add("IsHidden", false);
                    FoundAttributes.Add("Category", null);
                    FoundAttributes.Add("IsPassword", false);
                    FoundAttributes.Add("IsFile", false);
                    FoundAttributes.Add("IsFeatured", false);
                    FoundAttributes.Add("IsMultiLine", false);
                    FoundAttributes.Add("IsReadOnly", false);

                    object[] Attributes = Property.GetCustomAttributes(true);
                    foreach (object Attribute in Attributes)
                    {
                        if (Attribute is HideAttribute)
                        {
                            bool IsHidden = (Attribute as HideAttribute).Value;
                            if (IsHidden)
                            {
                                Skip = true;
                                break;
                            }
                            FoundAttributes["IsHidden"] = IsHidden;
                        }
                        if (Attribute is CategoryAttribute)
                            FoundAttributes["Category"] = Attribute.As<CategoryAttribute>().Category;
                        if (Attribute is PasswordAttribute)
                            FoundAttributes["IsPassword"] = Attribute.As<PasswordAttribute>().Value;
                        if (Attribute is FileAttribute)
                            FoundAttributes["IsFile"] = Attribute.As<FileAttribute>().Value;
                        if (Attribute is FeaturedAttribute)
                            FoundAttributes["IsFeatured"] = Attribute.As<FeaturedAttribute>().IsFeatured;
                        if (Attribute is MultiLineAttribute)
                            FoundAttributes["IsMultiLine"] = Attribute.As<MultiLineAttribute>().IsMultiLine;
                        if (Attribute is ReadOnlyAttribute)
                            FoundAttributes["IsReadOnly"] = Attribute.As<ReadOnlyAttribute>().IsReadOnly;
                    }
                    if (Skip)
                        continue;
                    PropertyItem PropertyItem = this.GetPropertyItem(Object, Property, FoundAttributes);
                    if (PropertyItem == null)
                        continue;
                    this.Add(PropertyItem);
                }
            }));

            if (Callback != null)
                Callback.Invoke();
        }

        /// <summary>
        /// Set properties by enumerating an unknown object. 
        /// </summary>
        /// <param name="Source">A function that enumerates an unknown object and returns a list of properties.</param>
        public async void BeginFromUnknown(Func<object, IEnumerable<PropertyItem>> Source)
        {
            await Task.Run(new Action(() =>
            {
                IEnumerable<PropertyItem> Properties = Source(new object());
                if (Properties == null)
                    return;
                foreach (PropertyItem p in Properties)
                    this.Add(p);
            }));
        }

        #endregion

        #endregion
    }
}
