using Imagin.Common.Attributes;
using Imagin.Common.Collections;
using Imagin.Common.Events;
using Imagin.Common.Extensions;
using Imagin.Common.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Controls.Extended
{
    public sealed class PropertyModelCollection : ConcurrentObservableCollection<PropertyModel>
    {
        #region Properties

        /// <summary>
        /// The object to evaluate.
        /// </summary>
        public object Object = null;

        PropertyModel featured = null;
        /// <summary>
        /// The featured property. This property is placed above all other properties.
        /// </summary>
        public PropertyModel Featured
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

        PropertyModel activeProperty = null;
        /// <summary>
        /// The active property.
        /// </summary>
        public PropertyModel ActiveProperty
        {
            get
            {
                return this.activeProperty;
            }
            set
            {
                this.activeProperty = value;
                OnPropertyChanged("ActiveProperty");
            }
        }

        public Type[] SupportedTypes
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
                    typeof(short),
                    typeof(Size),
                    typeof(SolidColorBrush),
                    typeof(string)
                };
            }
        }

        #endregion

        #region PropertyModelCollection

        public PropertyModelCollection() : base()
        {
            this.ItemAdded += OnItemAdded;
        }

        #endregion

        #region Methods

        void OnItemAdded(object sender, EventArgs<PropertyModel> e)
        {
            if (e.Value.IsFeatured)
                this.Featured = e.Value;
        }

        #region Public

        /// <summary>
        /// Set properties by enumerating a resource dictionary.
        /// </summary>
        /// <param name="Dictionary">The dictionary to enumerate.</param>
        /// <param name="Callback">What to do afterwards.</param>
        public async void BeginFromResourceDictionary(ResourceDictionary Dictionary, Action Callback = null)
        {
            if (Dictionary == null) return;
            await Task.Run(new Action(() =>
            {
                foreach (DictionaryEntry Entry in Dictionary)
                {
                    var Value = Entry.Value;
                    if (Value == null) continue;

                    var Type = Value.GetType();
                    if (Type.EqualsAny(typeof(LinearGradientBrush), typeof(SolidColorBrush)))
                    {
                        var t = Type.GetPropertyModelType();
                        if (t == null) continue;

                        var Result = PropertyModel.New(t, Entry.Key.ToString(), Value, Type.Name.SplitCamelCase(), string.Empty, false, false);
                        Result.SelectedObject = this.Object;
                        this.Add(Result);
                    }
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
            var Object = this.Object;
            if (Object == null) return;
            await Task.Run(new Action(() =>
            {
                //Enumerate object class attributes to find dynamic properties.
                foreach (var i in Object.GetType().GetCustomAttributes(true))
                {
                    if (i is DynamicPropertyAttribute)
                    {
                        //To do: Generate a PropertyModel based on DynamicPropertyAttribute properties
                    }
                }

                //Get list of object's properties sorted by name.
                var Properties = Object.GetType().GetProperties().OrderBy(x => x.Name).ToArray();
                //Enumerate the object's properties.
                for (int i = 0, Length = Properties.Length; i < Length; i++)
                {
                    var Property = Properties[i];

                    //Skip property if it (is not public and has get/set methods) or (isn't a supported type and isn't an enum).
                    if (!(Property.CanWrite && Property.GetSetMethod(true).IsPublic) || (!Property.PropertyType.EqualsAny(this.SupportedTypes) && !Property.PropertyType.IsEnum && !typeof(IList).IsAssignableFrom(Property.PropertyType)))
                        continue;

                    string Category = string.Empty, Description = string.Empty;
                    bool IsFeatured = false, IsHidden = false, IsReadOnly = false;
                    StringRepresentation StringRepresentation = StringRepresentation.Regular;

                    //Enumerate property's attributes.
                    foreach (var Attribute in Property.GetCustomAttributes(true))
                    {
                        //If hidden, discontinue enumeration.
                        if (Attribute is BrowsableAttribute && (IsHidden = !(Attribute as BrowsableAttribute).Browsable).As<bool>())
                            break;

                        if (Attribute is CategoryAttribute)
                            Category = Attribute.As<CategoryAttribute>().Category;
                        else if (Attribute is DescriptionAttribute)
                            Description = Attribute.As<DescriptionAttribute>().Description;
                        else if (Attribute is FeaturedAttribute)
                            IsFeatured = Attribute.As<FeaturedAttribute>().IsFeatured;
                        else if (Attribute is StringRepresentationAttribute)
                            StringRepresentation = Attribute.As<StringRepresentationAttribute>().Representation;
                        else if (Attribute is ReadOnlyAttribute)
                            IsReadOnly = Attribute.As<ReadOnlyAttribute>().IsReadOnly;
                    }

                    //If hidden, skip property.
                    if (IsHidden) continue;

                    //Create and add PropertyModel.
                    PropertyModel PropertyModel = PropertyModel.New(Property.PropertyType.GetPropertyModelType(), Property.Name, Property.GetValue(Object), Category, Description, IsReadOnly, IsFeatured);
                    if (PropertyModel is StringPropertyModel)
                        PropertyModel.As<StringPropertyModel>().Representation = StringRepresentation;
                    PropertyModel.SelectedObject = Object;
                    this.Add(PropertyModel);
                }
            }));

            if (Callback != null)
                Callback.Invoke();
        }

        /// <summary>
        /// Set properties by enumerating an unknown object. 
        /// </summary>
        /// <param name="Source">A function that enumerates an unknown object and returns a list of properties.</param>
        public async void BeginFromUnknown(Func<object, IEnumerable<PropertyModel>> Source)
        {
            await Task.Run(new Action(() =>
            {
                IEnumerable<PropertyModel> Properties = Source(new object());
                if (Properties == null)
                    return;
                foreach (PropertyModel p in Properties)
                    this.Add(p);
            }));
        }

        #endregion

        #endregion
    }
}
