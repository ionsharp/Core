using System;
using System.Collections.Generic;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractContainer : AbstractObject, ICloneable, IContainer<AbstractContainer>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();

        /// <summary>
        /// 
        /// </summary>
        IList<AbstractContainer> items = default(IList<AbstractContainer>);
        /// <summary>
        /// 
        /// </summary>
        public IList<AbstractContainer> Items
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
    }
}
