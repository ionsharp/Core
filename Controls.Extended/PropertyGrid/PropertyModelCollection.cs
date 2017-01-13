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

namespace Imagin.Controls.Extended
{
    public class PropertyModelCollection : ConcurrentObservableCollection<PropertyModel>
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
                return featured;
            }
            set
            {
                featured = value;
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
                return activeProperty;
            }
            set
            {
                activeProperty = value;
                OnPropertyChanged("ActiveProperty");
            }
        }

        #endregion

        #region PropertyModelCollection

        public PropertyModelCollection() : base()
        {
            ItemAdded += OnItemAdded;
        }

        #endregion

        #region Methods

        bool IsSupported(PropertyInfo Property)
        {
            return Property.IsPublic() && (Property.PropertyType.EqualsAny(PropertyModel.SupportedTypes) || Property.PropertyType.IsEnum || Property.PropertyType.Implements<IList>());
        }

        void OnItemAdded(object sender, EventArgs<PropertyModel> e)
        {
            if (e.Value.IsFeatured)
                Featured = e.Value;
        }

        /// <summary>
        /// Set and add custom properties.
        /// </summary>
        /// <param name="Source">A function that enumerates an object and returns a list of property models.</param>
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

                    var Attributes = new PropertyAttributes()
                    {
                        { "Browsable", "Browsable", true },
                        { "Category", "Category", string.Empty },
                        { "Constraint", null, null },
                        { "Description", "Description", string.Empty },
                        { "DisplayName", "DisplayName", false },
                        { "Featured", "IsFeatured", false },
                        { "Int64Kind", "Kind", Int64Kind.Default },
                        { "ReadOnly", "IsReadOnly", false },
                        { "StringKind", "Kind", StringKind.Regular },
                        { "DisplayFormat", "DataFormatString", string.Empty },
                    };

                    Attributes.ExtractFrom(Property);

                    if ((bool)Attributes["Browsable", false])
                    {
                        var Model = PropertyModel.New(Object, Property, Attributes);

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

        #endregion
    }
}
