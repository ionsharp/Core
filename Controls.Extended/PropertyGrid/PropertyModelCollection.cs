using Imagin.Common.Attributes;
using Imagin.Common.Collections.Concurrent;
using Imagin.Common.Extensions;
using Imagin.Common.Input;
using Imagin.Common.Scheduling;
using Imagin.Common.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Reflection;
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
                    typeof(RepeatOptions),
                    typeof(short),
                    typeof(Size),
                    typeof(SolidColorBrush),
                    typeof(string),
                    typeof(Uri),
                    typeof(Version)
                };
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
            return Property.IsPublic() && (Property.PropertyType.EqualsAny(SupportedTypes) || Property.PropertyType.IsEnum || Property.PropertyType.Implements<IList>());
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

            if (Callback != null)
                Callback.Invoke();
        }

        /// <summary>
        /// Set properties by enumerating the properties of an object.
        /// </summary>
        /// <param name="Callback">What to do afterwards.</param>
        public async Task BeginFromObject(object Object, Action Callback = null)
        {
            await Task.Run(() =>
            {
                //TO-DO: Evaluate dynamic properties if <Object> implements a certain interface?

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
                        { "Constraint", string.Empty, null },
                        { "Description", "Description", string.Empty },
                        { "DisplayName", "DisplayName", false },
                        { "Featured", "IsFeatured", false },
                        { "Int64Representation", "Representation", Int64Representation.Default },
                        { "ReadOnly", "IsReadOnly", false },
                        { "StringRepresentation", "Representation", StringRepresentation.Regular },
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

            if (Callback != null)
                Callback.Invoke();
        }

        #endregion
    }
}
