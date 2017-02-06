using Imagin.Common.Collections.Concurrent;
using Imagin.Common.Extensions;
using Imagin.Common.Input;
using Imagin.Common.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Imagin.Common.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Imagin.Controls.Extended
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

        /// <summary>
        /// Gets or sets the object that is currently hosted.
        /// </summary>
        public object Object = null;

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

            Console.WriteLine("Name => {0}, Type = {1}, Nullable => {2}".F(Property.Name, t, t.IsNullable() ? "nullable" : "not nullable"));

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
        /// <param name="Item"></param>
        protected override void OnItemAdded(PropertyModel Item)
        {
            base.OnItemAdded(Item);

            if (Item.IsFeatured)
                Featured = Item;
        }

        /// <summary>
        /// Set and add custom properties.
        /// </summary>
        /// <param name="Source">A function that enumerates an object and returns a list of property models.</param>
        /// <param name="Callback"></param>
        /// <returns></returns>
        public async Task BeginFrom(Func<object, IEnumerable<PropertyModel>> Source, Action Callback = null)
        {
            var i = Object;
            await Task.Run(() =>
            {
                var Properties = Source(i);
                if (Properties != null)
                {
                    foreach (var j in Properties)
                        Add(j);
                }
            });

            Callback.InvokeIf(x => !x.IsNull());
        }

        /// <summary>
        /// Set properties by enumerating the properties of an object.
        /// </summary>
        /// <param name="Object">The object to examine.</param>
        /// <param name="Callback">What to do afterwards.</param>
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
        public async Task BeginFromObject(object Object, Action Callback = null)
        {
            await Task.Run(() =>
            {
                var Properties = Object.GetType().GetProperties();

                for (int i = 0, Length = Properties.Length; i < Length; i++)
                {
                    var Property = Properties[i];

                    if (!IsSupported(Property))
                        continue;

                    var a = new PropertyAttributes()
                    {
                        { typeof(BrowsableAttribute), "Browsable", true },
                        { typeof(CategoryAttribute), "Category", string.Empty },
                        { typeof(ConstraintAttribute), null, null },
                        { typeof(DescriptionAttribute), "Description", string.Empty },
                        { typeof(DisplayNameAttribute), "DisplayName", string.Empty },
                        { typeof(FeaturedAttribute), "IsFeatured", false },
                        { typeof(Int64KindAttribute), "Kind", Int64Kind.Default },
                        { typeof(ReadOnlyAttribute), "IsReadOnly", false },
                        { typeof(StringKindAttribute), "Kind", StringKind.Default },
                        { typeof(StringFormatAttribute), "Format", string.Empty },
                    };

                    a.ExtractFrom(Property);
                    
                    if (a.Get<BrowsableAttribute, bool>())
                    {
                        var Model = PropertyModel.New(Object, Property, a);

                        if (Model != null)
                            Add(Model);
                    }
                }
            });

            Callback.InvokeIf(x => !x.IsNull());
        }

        /// <summary>
        /// Set properties by enumerating a resource dictionary.
        /// </summary>
        /// <param name="Dictionary">The dictionary to enumerate.</param>
        /// <param name="Callback">What to do afterwards.</param>
        public async Task BeginFromResourceDictionary(ResourceDictionary Dictionary, Action Callback = null)
        {
            if (Dictionary == null) return;

            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                foreach (DictionaryEntry i in Dictionary)
                {
                    if (i.Value != null)
                    {
                        var Type = i.Value.GetType();
                        if (Type.EqualsAny(typeof(LinearGradientBrush), typeof(SolidColorBrush)))
                        {
                            var Model = PropertyModel.New(Type, Dictionary, i.Key.ToString(), i.Value, Type.Name.SplitCamelCase(), string.Empty, string.Empty, false, false);

                            if (Model != null)
                                Add(Model);
                        }
                    }
                }
            }));

            Callback.InvokeIf(x => !x.IsNull());
        }

        /// <summary>
        /// Clear all properties and assign a reference to the given object, if any.
        /// </summary>
        /// <param name="Value"></param>
        public void Reset(object Value = null)
        {
            Featured = null;
            Object = Value;
            Clear();
        }

        #endregion
    }
}
