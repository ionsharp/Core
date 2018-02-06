using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PropertyAttributes : List<PropertyAttribute>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Attributes"></param>
        public PropertyAttributes(params PropertyAttribute[] Attributes) : base()
        {
            foreach (var i in Attributes)
                Add(i);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Property"></param>
        public void ExtractFrom(PropertyInfo Property)
        {
            foreach (var i in Property.GetCustomAttributes(true))
            {
                var PropertyAttribute = this.Where(k => k.Type == i.GetType()).FirstOrDefault();

                if (PropertyAttribute is PropertyAttribute)
                    PropertyAttribute.SetAttribute(i as Attribute);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <returns></returns>
        public TAttribute Get<TAttribute>() where TAttribute : Attribute
        {
            return (TAttribute)this.Where(i => i.Type == typeof(TAttribute)).FirstOrDefault()?.Attribute;
        }
    }
}
