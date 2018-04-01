using Imagin.Common.Collections.Generic;
using Imagin.Colour.Controls.Models;
using System;
using System.Linq;

namespace Imagin.Colour.Controls.Collections
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ComponentCollection : TCollection<Component>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public Component this[Type Type]
        {
            get => this.Where(i => i.GetType() == Type).FirstOrDefault();
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
        public ComponentCollection() : base() {}
    }
}
