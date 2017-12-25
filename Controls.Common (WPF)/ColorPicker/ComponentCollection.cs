using Imagin.Common.Collections.Generic;
using System;
using System.Linq;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ComponentCollection : TrackableCollection<ComponentModel>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public ComponentModel this[Type Type]
        {
            get
            {
                return this.Where(i => i.GetType() == Type).First();
            }
            set
            {
                for (var i = 0; i < Count; i++)
                {
                    if (this[i].GetType() == Type)
                    {
                        this[i] = value;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ComponentCollection() : base()
        {
        }
    }
}
