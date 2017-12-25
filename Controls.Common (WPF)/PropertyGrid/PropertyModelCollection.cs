using Imagin.Common;
using Imagin.Common.Collections.Concurrent;
using Imagin.Common.Data;
using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyModelCollection : ConcurrentCollection<PropertyModel>
    {
        #region Properties

        PropertyModel activeProperty = null;
        /// <summary>
        /// The active, or selected, property.
        /// </summary>
        public PropertyModel ActiveProperty
        {
            get
            {
                return activeProperty;
            }
            set
            {
                activeProperty = value;
                OnPropertyChanged("ActiveProperty");
            }
        }

        PropertyModel featured = null;
        /// <summary>
        /// Gets the featured property, which is placed above all others.
        /// </summary>
        public PropertyModel Featured
        {
            get
            {
                return featured;
            }
            private set
            {
                featured = value;
                OnPropertyChanged("Featured");
            }
        }

        object source;
        /// <summary>
        /// Gets or sets the object that is currently hosted.
        /// </summary>
        public object Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;
                OnPropertyChanged("Source");
            }
        }

        #endregion

        #region PropertyModelCollection

        /// <summary>
        /// 
        /// </summary>
        public PropertyModelCollection() : base()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// If the property is public (i.e., have public getter AND setter) AND:
        /// 
        /// a) Has a type that is supported (nullable or not), 
        /// b) Is <see cref="Enum"/>, or 
        /// c) Implements <see cref="IList"/>.
        /// </summary>
        /// <param name="Property"></param>
        /// <returns></returns>
        bool IsSupported(PropertyInfo Property)
        {
            //If property has a public getter, with or without a setter...
            var a = Property.GetGetMethod(false) != null;

            var t = Property.PropertyType;

            if (t.IsNullable())
                t = t.GetGenericArguments().WhereFirst(i => true);

            var b = t.EqualsAny(PropertyModel.SupportedTypes);

            b = b || t.IsEnum;
            b = b || t.Implements<IList>();

            return a && b;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        protected override void OnItemAdded(PropertyModel item)
        {
            base.OnItemAdded(item);

            if (item.IsFeatured)
                Featured = item;
        }

        /// <summary>
        /// Set properties by enumerating the properties of an object.
        /// </summary>
        /// <param name="source">The object to examine.</param>
        /// <param name="callback">What to do afterwards.</param>
        /// <remarks>
        /// TO-DO: Evaluate dynamic properties if the object implements a certain interface? 
        /// 
        /// Dynamic properties would be properties that don't need to be owned by the object
        /// and cannot be modified, but should be displayed to the user anyway. 
        /// 
        /// The object would have to specify how to get the value for each dynamic property 
        /// internally using an action; the action simply returns the object we want.
        /// 
        /// The interface would expose a method that accepts the latter-described action,
        /// invokes it, and returns the resulting object (the current value of the 
        /// dynamic property). Note, this enables you to calculate the value for the 
        /// dynamic property however you like.
        /// 
        /// If the object implements this interface, we can safely check for dynamic 
        /// properties. While enumerating, we'd get the initial value; subsequently,
        /// we'd need a way of updating it when it should be (another TO-DO).
        /// 
        /// Assuming each dynamic property specifies a general type, we'll know what type
        /// to cast to when retrieving the it's value.
        /// 
        /// Ultimately, this would enable you to display properties in addition to the ones
        /// the object already owns without the additional overhead.
        /// </remarks>
        public async Task LoadAsync(object source, Action callback = null)
        {
            var type = source?.GetType();

            if (type != null)
            {
                await Task.Run(() =>
                {
                    var Properties = type.GetProperties();

                    for (int i = 0, Length = Properties.Length; i < Length; i++)
                    {
                        var Property = Properties[i];

                        var IsNested = false;
                        //If the type isn't explicitly supported...
                        if (!IsSupported(Property))
                        {
                            //...and it's not a reference type, skip it
                            if (Property.PropertyType.IsValueType)
                            {
                                continue;
                            }
                            //...and it is a reference type, consider it "nested"
                            else IsNested = true;
                        }

                        var Attributes = new PropertyAttributes
                        (
                            new PropertyAttribute<BrowsableAttribute>(new BrowsableAttribute(true)),
                            new PropertyAttribute<System.ComponentModel.BrowsableAttribute>(new System.ComponentModel.BrowsableAttribute(true)),

                            new PropertyAttribute<CategoryAttribute>(new CategoryAttribute(string.Empty)),
                            new PropertyAttribute<System.ComponentModel.CategoryAttribute>(new System.ComponentModel.CategoryAttribute(string.Empty)),

                            new PropertyAttribute<DescriptionAttribute>(new DescriptionAttribute(string.Empty)),
                            new PropertyAttribute<System.ComponentModel.DescriptionAttribute>(new System.ComponentModel.DescriptionAttribute(string.Empty)),

                            new PropertyAttribute<DisplayNameAttribute>(new DisplayNameAttribute(string.Empty)),
                            new PropertyAttribute<System.ComponentModel.DisplayNameAttribute>(new System.ComponentModel.DisplayNameAttribute(string.Empty)),

                            new PropertyAttribute<ConstraintAttribute>(default(ConstraintAttribute)),

                            new PropertyAttribute<FeaturedAttribute>(new FeaturedAttribute(false)),

                            new PropertyAttribute<DateFormatAttribute>(new DateFormatAttribute(DateFormat.Default)),
                            new PropertyAttribute<EnumFormatAttribute>(new EnumFormatAttribute(EnumFormat.Default)),
                            new PropertyAttribute<LongFormatAttribute>(new LongFormatAttribute(LongFormat.Default)),
                            new PropertyAttribute<StringFormatAttribute>(new StringFormatAttribute(StringFormat.Default)),

                            new PropertyAttribute<ReadOnlyAttribute>(new ReadOnlyAttribute(false)),
                            new PropertyAttribute<System.ComponentModel.ReadOnlyAttribute>(new System.ComponentModel.ReadOnlyAttribute(false))
                        );

                        Attributes.ExtractFrom(Property);

                        if (Attributes.Get<BrowsableAttribute>().Browsable && Attributes.Get<System.ComponentModel.BrowsableAttribute>().Browsable)
                        {
                            var Model = PropertyModel.New(source, Property, Attributes, IsNested);

                            if (Model != null)
                                Add(Model);
                        }
                    }
                });
            }

            callback?.Invoke();
        }

        /// <summary>
        /// Set properties by enumerating a resource dictionary.
        /// </summary>
        /// <param name="source">The dictionary to enumerate.</param>
        /// <param name="callback">What to do afterwards.</param>
        public async Task LoadAsync(ResourceDictionary source, Action callback = null)
        {
            if (source != null)
            {
                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    foreach (DictionaryEntry i in source)
                    {
                        if (i.Value != null)
                        {
                            var Type = i.Value.GetType();
                            if (Type.EqualsAny(typeof(LinearGradientBrush), typeof(SolidColorBrush)))
                            {
                                var Model = PropertyModel.New(Type, source, i.Key.ToString(), i.Value, Type.Name.SplitCamelCase(), string.Empty, string.Empty, false, false);

                                if (Model != null)
                                    Add(Model);
                            }
                        }
                    }
                }));
                callback?.Invoke();
            }
        }

        /// <summary>
        /// Clear all properties and assign a reference to the given object, if any.
        /// </summary>
        /// <param name="source"></param>
        public void Reset(object source)
        {
            Featured = default(PropertyModel);
            Source = source;
            Clear();
        }

        #endregion
    }
}
