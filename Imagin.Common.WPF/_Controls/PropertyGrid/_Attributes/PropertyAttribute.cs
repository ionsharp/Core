using System;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PropertyAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract Type Type
        {
            get;
        }

        Attribute attribute;
        /// <summary>
        /// 
        /// </summary>
        public Attribute Attribute
        {
            get
            {
                return attribute;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Attribute"></param>
        public PropertyAttribute(Attribute Attribute)
        {
            attribute = Attribute;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Attribute"></param>
        public void SetAttribute(Attribute Attribute)
        {
            attribute = Attribute;
        }
    }
}
