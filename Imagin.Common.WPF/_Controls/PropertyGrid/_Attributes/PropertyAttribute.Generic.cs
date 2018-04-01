using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    public class PropertyAttribute<TAttribute> : PropertyAttribute where TAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public override Type Type
        {
            get
            {
                return typeof(TAttribute);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Attribute"></param>
        public PropertyAttribute(TAttribute Attribute) : base(Attribute)
        {
        }
    }
}
