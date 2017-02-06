using Imagin.Common.Collections.Concurrent;
using Imagin.Common.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class AbstractContainer : AbstractContainer<AbstractObject>
    {
        /// <summary>
        /// 
        /// </summary>
        public AbstractContainer() : this(string.Empty)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        public AbstractContainer(string Name) : base(Name)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class AbstractContainer<TObject> : NamedObject, ICloneable, IContainer, IContainer<TObject>, INamable
    {
        /// <summary>
        /// 
        /// </summary>
        ConcurrentCollection<TObject> items = new ConcurrentCollection<TObject>();
        /// <summary>
        /// 
        /// </summary>
        public ConcurrentCollection<TObject> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
                OnPropertyChanged("Items");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AbstractContainer<TObject> Clone()
        {
            var Result = new AbstractContainer<TObject>(Name);

            foreach (var i in Items)
            {
                if (i is ICloneable)
                    Result.Items.Add((TObject)i.As<ICloneable>().Clone());
            }

            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        public AbstractContainer() : this(string.Empty)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        public AbstractContainer(string Name) : base(Name)
        {
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        IList IContainer.Items
        {
            get
            {
                return items;
            }
            set
            {
                if (value is ConcurrentCollection<TObject>)
                {
                    items = (ConcurrentCollection<TObject>)value;
                    OnPropertyChanged("Items");
                }
            }
        }

        IList<TObject> IContainer<TObject>.Items
        {
            get
            {
                return items;
            }
            set
            {
                if (value is ConcurrentCollection<TObject>)
                {
                    items = (ConcurrentCollection<TObject>)value;
                    OnPropertyChanged("Items");
                }
            }
        }
    }
}
