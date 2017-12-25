using System;

namespace Imagin.Common
{
    /// <summary>
    /// A generic alternative for <see cref="System.ComponentModel.BrowsableAttribute"/>, which isn't available in some frameworks.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class BrowsableAttribute : Attribute
    {
        readonly bool browsable;
        /// <summary>
        /// 
        /// </summary>
        public bool Browsable
        {
            get
            {
                return browsable;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Browsable"></param>
        public BrowsableAttribute(bool Browsable)
        {
            browsable = Browsable;
        }
    }
}
